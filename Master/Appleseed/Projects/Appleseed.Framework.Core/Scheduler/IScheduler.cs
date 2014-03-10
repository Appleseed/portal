// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScheduler.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Standard interface for a scheduler
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Scheduler
{
    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17
    /// <summary>
    /// Standard interface for a scheduler
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface IScheduler
    {
        #region Properties

        /// <summary>
        ///   Get or set the scheduler timer period
        /// </summary>
        /// <value>The period.</value>
        /// <remarks>
        /// </remarks>
        long Period { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get an array of tasks of the specified module target
        /// </summary>
        /// <param name="moduleOwnerId">
        /// The module owner id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        SchedulerTask[] GetTasksByOwner(int moduleOwnerId);

        /// <summary>
        /// Get an array of tasks of the specified module owner
        /// </summary>
        /// <param name="moduleTargetId">
        /// The module target id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        SchedulerTask[] GetTasksByTarget(int moduleTargetId);

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
        SchedulerTask InsertTask(SchedulerTask task);

        /// <summary>
        /// Remove a task
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <remarks>
        /// </remarks>
        void RemoveTask(SchedulerTask task);

        /// <summary>
        /// Start the scheduler
        /// </summary>
        /// <remarks>
        /// </remarks>
        void Start();

        /// <summary>
        /// Stop scheduler activities
        /// </summary>
        /// <remarks>
        /// </remarks>
        void Stop();

        #endregion
    }
}