// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachedScheduler.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Implementation of IScheduler with in-memory cache.
//   CacheScheduler uses an internal SortedList fill with a queue of nearest to occur tasks (ordered by dueTime)
//   Timer can check tasks list very often because it has to check only first element of sortedList and not send any request to database.
//   When a task occurs (because of DueTime expires) task is executed and removed from db and from cache.
//   When a module add a task, it's added to db and to cache.
//   When and only when cache is empty, scheduler performs a SELECT in db and refill the cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Scheduler
{
    using System;
    using System.Collections;
    using System.Data;

    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17

    /// <summary>
    /// Implementation of IScheduler with in-memory cache.
    ///   CacheScheduler uses an internal SortedList fill with a queue of nearest to occur tasks (ordered by dueTime)
    ///   Timer can check tasks list very often because it has to check only first element of sortedList and not send any request to database.
    ///   When a task occurs (because of DueTime expires) task is executed and removed from db and from cache.
    ///   When a module add a task, it's added to db and to cache.
    ///   When and only when cache is empty, scheduler performs a SELECT in db and refill the cache.
    /// </summary>
    public class CachedScheduler : SimpleScheduler
    {
        #region Constants and Fields

        /// <summary>
        /// The cache.
        /// </summary>
        protected SortedList _cache;

        /// <summary>
        /// The scheduler.
        /// </summary>
        private static volatile CachedScheduler theScheduler; // the scheduler instance

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedScheduler"/> class.
        /// The cached scheduler.
        /// </summary>
        /// <param name="applicationMapPath">The application map path.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="period">The period.</param>
        /// <param name="cacheSize">Size of the cache.</param>
        /// <remarks></remarks>
        protected CachedScheduler(string applicationMapPath, IDbConnection connection, long period, int cacheSize)
            : base(applicationMapPath, connection, period)
        {
            this._cache = new SortedList(new TaskComparer(), cacheSize);
            this.FillCache();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the scheduler, using a singleton pattern.
        /// </summary>
        /// <param name="applicationMapPath">
        /// usually HttpContext.Current.Server.MapPath(PortalSettings.ApplicationPath)
        /// </param>
        /// <param name="connection">
        /// db connection
        /// </param>
        /// <param name="period">
        /// scheduler timer milliseconds
        /// </param>
        /// <param name="cacheSize">
        /// max number of in-memory tasks
        /// </param>
        /// <returns>
        /// </returns>
        public static CachedScheduler GetScheduler(
            string applicationMapPath, IDbConnection connection, long period, int cacheSize)
        {
            // Singleton
            if (theScheduler == null)
            {
                lock (typeof(CachedScheduler))
                {
                    if (theScheduler == null)
                    {
                        theScheduler = new CachedScheduler(applicationMapPath, connection, period, cacheSize);
                    }
                }
            }

            return theScheduler;
        }

        /// <summary>
        /// Fill internal tasks cache
        /// </summary>
        public void FillCache()
        {
            using (var dr = this.localSchDB.GetOrderedTask())
            {
                while (dr.Read() && this._cache.Count < this._cache.Capacity)
                {
                    var tsk = new SchedulerTask(dr);

                    lock (this._cache.SyncRoot)
                    {
                        this._cache.Add(tsk, tsk);
                    }
                }

            }
        }

        /// <summary>
        /// Insert a new task
        /// </summary>
        /// <param name="task">
        /// </param>
        /// <remarks>
        /// After a new task is inserted it obtains a IDTask. Before it's -1.
        /// </remarks>
        /// <returns>
        /// </returns>
        public override SchedulerTask InsertTask(SchedulerTask task)
        {
            if (task.IDTask != -1)
            {
                throw new SchedulerException("Could not insert an inserted task");
            }

            task.SetIDTask(this.localSchDB.InsertTask(task));
            if (this._cache.Count != 0 &&
                task.DueTime < ((SchedulerTask)this._cache.GetKey(this._cache.Count - 1)).DueTime)
            {
                lock (this._cache.SyncRoot)
                {
                    this._cache.RemoveAt(this._cache.Count - 1);
                    this._cache.Add(task, task);
                }
            }

            return task;
        }

        /// <summary>
        /// Remove tasks
        /// </summary>
        /// <param name="task">
        /// </param>
        public override void RemoveTask(SchedulerTask task)
        {
            if (task.IDTask == -1)
            {
                return;
            }

            // remoce from DB
            base.RemoveTask(task);

            // remove from cache
            lock (this._cache.SyncRoot)
            {
                this._cache.Remove(task);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs a schedulation. Called by timer.
        /// </summary>
        /// <param name="timerState">
        /// </param>
        protected override void Schedule(object timerState)
        {
            lock (this)
            {
                this.localTimerState.Counter++;
                this.Stop(); // Stop timer while scheduler works
                while (this._cache.Count != 0)
                {
                    var task = (SchedulerTask)this._cache.GetKey(0);
                    if (task.DueTime > DateTime.Now)
                    {
                        break;
                    }

                    try
                    {
                        this.ExecuteTask(task);
                    }
                    catch
                    {
                        // TODO: We have to apply some policy here...
                        // i.e. Move failed tasks on a log, call a Module feedback interface,....
                        // now task is removed always
                    }

                    this.RemoveTask(task);
                }

                if (this._cache.Count == 0)
                {
                    this.FillCache();
                    if (this._cache.Count == 0)
                    {
                        return; // avoid loop in case of empty tasks-queue in db.
                    }

                    this.Schedule(timerState);
                }

                this.Start(); // restart timer
            }
        }

        #endregion

        /// <summary>
        /// The task comparer.
        /// </summary>
        private class TaskComparer : IComparer
        {
            #region Implemented Interfaces

            #region IComparer

            /// <summary>
            /// Compare two tasks first order by dueTime. If dueTimes are equal they are ordered by IDTask.
            /// Used in sortedlist.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>The compare.</returns>
            /// <exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception>
            /// <remarks></remarks>
            public int Compare(object x, object y)
            {
                var xtsk = x as SchedulerTask;
                var ytsk = y as SchedulerTask;
                if (xtsk == null || ytsk == null)
                {
                    throw new ArgumentException("object is not a SchedulerTask");
                }

                if (xtsk.DueTime < ytsk.DueTime)
                {
                    return -1;
                }

                if (xtsk.DueTime > ytsk.DueTime)
                {
                    return 1;
                }

                if (xtsk.DueTime == ytsk.DueTime)
                {
                    if (xtsk.IDTask < ytsk.IDTask)
                    {
                        return -1;
                    }

                    if (xtsk.IDTask > ytsk.IDTask)
                    {
                        return 1;
                    }

                    if (xtsk.IDTask == ytsk.IDTask)
                    {
                        return 0;
                    }
                }

                throw new ArgumentException("Impossible exception"); // ... to compile.
            }

            #endregion

            #endregion
        }
    }
}