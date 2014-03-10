// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchedulerTask.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Describe a Task
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Scheduler
{
    using System;
    using System.Data;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    // Author: Federico Dal Maso
    // e-mail: ifof@libero.it
    // date: 2003-06-17

    /// <summary>
    /// Describe a Task
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SchedulerTask
    {
        #region Constants and Fields

        /// <summary>
        /// </summary>
        private const int MaxDescriptionLength = 150; // see db

        /// <summary>
        /// The arg.
        /// </summary>
        private object arg;

        /// <summary>
        /// The description.
        /// </summary>
        private string description;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <param name="dueTime">
        /// The due time.
        /// </param>
        /// <remarks>
        /// </remarks>
        public SchedulerTask(int idModuleOwner, int idModuleTarget, DateTime dueTime)
        {
            this.IDModuleOwner = idModuleOwner;
            this.IDModuleTarget = idModuleTarget;
            this.DueTime = dueTime;
            this.IDTask = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <param name="dueTime">
        /// The due time.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <remarks>
        /// </remarks>
        public SchedulerTask(int idModuleOwner, int idModuleTarget, DateTime dueTime, string description)
            : this(idModuleOwner, idModuleTarget, dueTime)
        {
            this.description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <param name="dueTime">
        /// The due time.
        /// </param>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <remarks>
        /// </remarks>
        public SchedulerTask(int idModuleOwner, int idModuleTarget, DateTime dueTime, object argument)
            : this(idModuleOwner, idModuleTarget, dueTime)
        {
            if (!argument.GetType().IsSerializable)
            {
                throw new ApplicationException("argument parameter must be a serializable type");
            }

            this.arg = argument;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <param name="dueTime">
        /// The due time.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <remarks>
        /// </remarks>
        public SchedulerTask(
            int idModuleOwner, int idModuleTarget, DateTime dueTime, string description, object argument)
            : this(idModuleOwner, idModuleTarget, dueTime, description)
        {
            if (!argument.GetType().IsSerializable)
            {
                throw new ApplicationException("argument parameter must be a serializable type");
            }

            this.arg = argument;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="iDTask">
        /// The i D task.
        /// </param>
        /// <param name="idModuleOwner">
        /// The id module owner.
        /// </param>
        /// <param name="idModuleTarget">
        /// The id module target.
        /// </param>
        /// <param name="dueTime">
        /// The due time.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="argument">
        /// The argument.
        /// </param>
        /// <remarks>
        /// </remarks>
        internal SchedulerTask(
            int iDTask, int idModuleOwner, int idModuleTarget, DateTime dueTime, string description, object argument)
            : this(idModuleOwner, idModuleTarget, dueTime, description, argument)
        {
            this.IDTask = iDTask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerTask"/> class.
        /// </summary>
        /// <param name="dr">
        /// The dr.
        /// </param>
        /// <remarks>
        /// </remarks>
        internal SchedulerTask(IDataReader dr)
        {
            var bf = new BinaryFormatter();
            using (var ss = new MemoryStream((byte[])dr["Argument"]))
            {
                this.arg = bf.Deserialize(ss);
                ss.Close();
            }

            this.IDTask = (int)dr["IDTask"];
            this.IDModuleOwner = (int)dr["IDModuleOwner"];
            this.IDModuleTarget = (int)dr["IDModuleTarget"];
            this.DueTime = (DateTime)dr["DueTime"];
            this.description = (string)dr["Description"];
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the argument.
        /// </summary>
        /// <value>The argument.</value>
        /// <remarks>
        /// </remarks>
        public object Argument
        {
            get
            {
                return this.arg;
            }

            set
            {
                if (!value.GetType().IsSerializable)
                {
                    throw new ApplicationException("argument parameter must be a serializable type");
                }

                this.arg = value;
            }
        }

        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks>
        /// </remarks>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
                if (this.description.Length > MaxDescriptionLength)
                {
                    this.description = this.description.Substring(0, MaxDescriptionLength);
                }
            }
        }

        /// <summary>
        ///   Gets or sets the due time.
        /// </summary>
        /// <value>The due time.</value>
        /// <remarks>
        /// </remarks>
        public DateTime DueTime { get; set; }

        /// <summary>
        ///   Gets or sets the ID module owner.
        /// </summary>
        /// <value>The ID module owner.</value>
        /// <remarks>
        /// </remarks>
        public int IDModuleOwner { get; set; }

        /// <summary>
        ///   Gets or sets the ID module target.
        /// </summary>
        /// <value>The ID module target.</value>
        /// <remarks>
        /// </remarks>
        public int IDModuleTarget { get; set; }

        /// <summary>
        ///   Gets the ID task.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int IDTask { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the ID task.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <remarks>
        /// </remarks>
        internal void SetIDTask(int id)
        {
            this.IDTask = id;
        }

        #endregion
    }
}