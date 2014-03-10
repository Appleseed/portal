// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedDynamicModel.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed dynamic model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DAL
{
    using Massive;

    /// <summary>
    /// The appleseed dynamic model.
    /// </summary>
    /// <remarks>
    /// </remarks>
    internal class AppleseedDynamicModel : DynamicModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedDynamicModel"/> class.
        /// </summary>
        /// <param name="tableName">
        /// Name of the table.
        /// </param>
        /// <param name="primaryKeyField">
        /// The primary key field.
        /// </param>
        /// <remarks>
        /// </remarks>
        public AppleseedDynamicModel(string tableName = "", string primaryKeyField = "")
            : base(
                connectionStringName: "ConnectionString", 
                tableName: tableName, 
                tableNamePrefix: "rb_", 
                primaryKeyField: primaryKeyField)
        {
        }

        #endregion
    }
}