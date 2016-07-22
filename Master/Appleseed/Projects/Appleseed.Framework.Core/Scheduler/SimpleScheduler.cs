// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleScheduler.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   SimpleScheduler, perform a select every call to Scheduler
//   Useful only for long period timer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Scheduler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;

    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17

    /// <summary>
    /// SimpleScheduler, perform a select every call to Scheduler
    ///   Useful only for long period timer.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SimpleScheduler : IScheduler
    {
        #region Constants and Fields
        
        /// <summary>
        /// load schedule objects from caches
        /// </summary>
        private static object loadSchedCache = new Object();

        /// <summary>
        /// The local sch db.
        /// </summary>
        internal SchedulerDB localSchDB;

        /// <summary>
        /// The local period.
        /// </summary>
        protected long localPeriod;

        /// <summary>
        /// The local timer state.
        /// </summary>
        protected TimerState localTimerState;

        /// <summary>
        /// The scheduler.
        /// </summary>
        private static volatile SimpleScheduler theScheduler;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleScheduler"/> class.
        /// </summary>
        /// <param name="applicationMapPath">
        /// The application map path.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="period">
        /// The period.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected SimpleScheduler(string applicationMapPath, IDbConnection connection, long period)
        {
            this.localSchDB = new SchedulerDB(connection, applicationMapPath);
            this.localPeriod = period;

            this.localTimerState = new TimerState();

            var t = new Timer(this.Schedule, this.localTimerState, Timeout.Infinite, Timeout.Infinite);

            this.localTimerState.Timer = t;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Get or set the timer period.
        /// </summary>
        /// <value>The period.</value>
        /// <remarks>
        /// </remarks>
        public virtual long Period
        {
            get
            {
                return this.localPeriod;
            }

            set
            {
                this.localPeriod = value;
                this.localTimerState.Timer.Change(0L, this.localPeriod);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the scheduler.
        /// </summary>
        /// <param name="applicationMapPath">
        /// The application map path.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="period">
        /// The period.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static SimpleScheduler GetScheduler(string applicationMapPath, IDbConnection connection, long period)
        {
            // Singleton
            if (theScheduler == null)
            {
                lock (loadSchedCache)
                {
                    if (theScheduler == null)
                    {
                        theScheduler = new SimpleScheduler(applicationMapPath, connection, period);
                    }
                }
            }

            return theScheduler;
        }

        #endregion

        #region Implemented Interfaces

        #region IScheduler

        /// <summary>
        /// Get an array of tasks of the specified module owner
        /// </summary>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public virtual SchedulerTask[] GetTasksByOwner(int idModuleOwner)
        {
            var dr = this.localSchDB.GetTasksByOwner(idModuleOwner);
            var ary = new ArrayList();
            while (dr.Read())
            {
                ary.Add(new SchedulerTask(dr));
            }

            dr.Close();

            return (SchedulerTask[])ary.ToArray(typeof(SchedulerTask));
        }

        /// <summary>
        /// Get an array of tasks of the specified module target
        /// </summary>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public virtual SchedulerTask[] GetTasksByTarget(int idModuleTarget)
        {
            var dr = this.localSchDB.GetTasksByOwner(idModuleTarget);
            var ary = new List<SchedulerTask>();
            while (dr.Read())
            {
                ary.Add(new SchedulerTask(dr));
            }

            dr.Close();

            return ary.ToArray();
        }

        /// <summary>
        /// Insert a new task
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public virtual SchedulerTask InsertTask(SchedulerTask task)
        {
            if (task.IDTask != -1)
            {
                throw new SchedulerException("Could not insert an inserted task");
            }

            task.SetIDTask(this.localSchDB.InsertTask(task));
            return task;
        }

        /// <summary>
        /// Remove a task
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <remarks>
        /// </remarks>
        public virtual void RemoveTask(SchedulerTask task)
        {
            if (task.IDTask == -1)
            {
                return;
            }

            this.localSchDB.RemoveTask(task.IDTask);
            return;
        }

        /// <summary>
        /// Start the scheduler timer
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void Start()
        {
            this.localTimerState.Timer.Change(this.localPeriod, this.localPeriod);
        }

        /// <summary>
        /// Stop the scheduler timer
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void Stop()
        {
            this.localTimerState.Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Call the correct ISchedulable methods of a target module assigned to the task.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void ExecuteTask(SchedulerTask task)
        {
            ISchedulable module;
            try
            {
                module = this.localSchDB.GetModuleInstance(task.IDModuleTarget);
            }
            catch
            {
                // TODO:
                return;
            }

            try
            {
                module.ScheduleDo(task);
            }
            catch (Exception ex)
            {
                try
                {
                    module.ScheduleRollback(task);
                }
                catch (Exception ex2)
                {
                    throw new SchedulerException("ScheduleDo fail. Rollback fails", ex2);
                }

                throw new SchedulerException("ScheduleDo fails. Rollback called successfully", ex);
            }

            try
            {
                module.ScheduleCommit(task);
            }
            catch (Exception ex)
            {
                throw new SchedulerException("ScheduleDo called successfully. Commit fails", ex);
            }
        }

        /// <summary>
        /// Schedules the specified timer state.
        /// </summary>
        /// <param name="timerState">
        /// State of the timer.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected virtual void Schedule(object timerState)
        {
            lock (this)
            {
                this.localTimerState.Counter++;

                var tsks = this.localSchDB.GetExpiredTask();

                this.Stop(); // Stop the timer while it works

                foreach (var tsk in tsks)
                {
                    try
                    {
                        this.ExecuteTask(tsk);
                    }
                    catch
                    {
                        // TODO: We have to apply some policy here...
                        // i.e. Move failed tasks on a log, call a Module feedback interface,....
                        // now task is removed always
                    }

                    this.RemoveTask(tsk);
                }

                this.Start(); // restart the timer
            }
        }

        #endregion

        /// <summary>
        /// The timer state.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected class TimerState
        {
            #region Properties

            /// <summary>
            ///   Gets or sets the counter.
            /// </summary>
            /// <value>The counter.</value>
            /// <remarks>
            /// </remarks>
            public int Counter { get; set; }

            // = 0;

            /// <summary>
            ///   Gets or sets the timer.
            /// </summary>
            /// <value>The timer.</value>
            /// <remarks>
            /// </remarks>
            public Timer Timer { get; set; }

            #endregion
        }
    }
}