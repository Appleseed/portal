// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BLLBase.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Base class for all the classes in the BLL
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.BLL.Base
{
    using System;

    /// <summary>
    /// Base class for all the classes in the BLL
    /// </summary>
    public abstract class BLLBase : IDisposable
    {
        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks></remarks>
        protected void Dispose(bool disposing)
        {
        }

        #endregion
    }
}