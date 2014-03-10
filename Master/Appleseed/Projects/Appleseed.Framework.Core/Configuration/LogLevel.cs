// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogLevel.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Defines the level of the event to log
// </summary>
// --------------------------------------------------------------------------------------------------------------------

 // Created by Manu

namespace Appleseed.Framework
{
    using System;

    /// <summary>
    /// Defines the level of the event to log
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        ///   None
        /// </summary>
        None = 0, 

        /// <summary>
        ///   Debug
        /// </summary>
        Debug = 2, 

        /// <summary>
        ///   Info
        /// </summary>
        Info = 4, 

        /// <summary>
        ///   Warn
        /// </summary>
        Warn = 8, 

        /// <summary>
        ///   Error
        /// </summary>
        Error = 16, 

        /// <summary>
        ///   Fatal
        /// </summary>
        Fatal = 32
    }
}