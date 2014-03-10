using System;

namespace Appleseed.Framework.Scheduler
{
    /// <summary>
    /// Summary description for SchedulerException.
    /// </summary>
    /// <remarks>
    /// Author: Federico Dal Maso
    /// e-mail: ifof@libero.it
    /// date: 2003-06-17
    /// </remarks>
    [Serializable]
    public class SchedulerException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SchedulerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public SchedulerException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public SchedulerException() : base()
        {
        }
    }
}