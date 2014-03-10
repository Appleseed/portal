// --------------------------------------------------------------------------------------------------------------------
// <copyright file="History.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   History can be used to track code authors and modification
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;

    /// <summary>
    /// History can be used to track code authors and modification
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public class History : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class. 
        /// Requires all parameters
        /// </summary>
        /// <param name="name">
        /// name
        /// </param>
        /// <param name="email">
        /// email
        /// </param>
        /// <param name="version">
        /// version
        /// </param>
        /// <param name="date">
        /// date
        /// </param>
        /// <param name="comment">
        /// comment
        /// </param>
        public History(string name, string email, string version, string date, string comment)
        {
            this.Name = name;
            this.Email = email;
            this.Version = version;
            this.Date = DateTime.Parse(date);
            this.Comment = comment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class. 
        /// Requires name, date and comment
        /// </summary>
        /// <param name="name">
        /// name
        /// </param>
        /// <param name="date">
        /// date
        /// </param>
        /// <param name="comment">
        /// comment
        /// </param>
        public History(string name, string date, string comment)
        {
            this.Name = name;
            try
            {
                this.Date = DateTime.Parse(date);
            }
            catch (FormatException ex)
            {
                throw new Exception(string.Format("'{0}' is an invalid date", date), ex);
            }

            this.Comment = comment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class. 
        /// Requires name, date and comment
        /// </summary>
        /// <param name="name">
        /// name
        /// </param>
        /// <param name="date">
        /// date
        /// </param>
        public History(string name, string date)
        {
            this.Name = name;
            this.Date = DateTime.Parse(date);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Modification description
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        ///     Modification date
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        ///     The Email of the Author
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     The Author name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     Modification Version
        /// </summary>
        public virtual string Version { get; set; }

        #endregion
    }
}