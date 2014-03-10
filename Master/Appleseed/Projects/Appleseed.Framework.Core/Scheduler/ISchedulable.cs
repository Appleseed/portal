// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchedulable.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Must be implemented by module who want use scheduler callback feature.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Scheduler
{
    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17

    /// <summary>
    /// Must be implemented by module who want use scheduler callback feature.
    /// </summary>
    /// <remarks></remarks>
    public interface ISchedulable
    {
        #region Public Methods

        /// <summary>
        /// Called after ScheduleDo if it doesn't throw any exception
        /// </summary>
        /// <param name="task">The task.</param>
        /// <remarks></remarks>
        void ScheduleCommit(SchedulerTask task);

        /// <summary>
        /// Called when a task occurs
        /// </summary>
        /// <param name="task">The task.</param>
        /// <remarks></remarks>
        void ScheduleDo(SchedulerTask task);

        /// <summary>
        /// Called after ScheduleDo if it throws an exception
        /// </summary>
        /// <param name="task">The task.</param>
        /// <remarks></remarks>
        void ScheduleRollback(SchedulerTask task);

        #endregion
    }
}