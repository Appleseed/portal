// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BagHolder.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Define the Web Bag Interface as the
//   repository for temporary data
// </summary>
// --------------------------------------------------------------------------------------------------------------------
// Business Logic Layer
// Appleseed.Framework.BLL.Utils
// Classes to Manage the User Web Collection
// Created By : bja@reedtek.com Date: 26/04/2003
namespace Appleseed.Framework.BLL.Utils
{
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// Define the Web Bag Interface as the
    ///     repository for temporary data
    /// </summary>
    public interface IWebBagHolder
    {
        // attributes to get and set stored values
        // name-value pairs
        #region Indexers

        /// <summary>
        /// Get/Set values from integer key
        /// </summary>
        /// <value></value>
        object this[int index] { get; set; }

        /// <summary>
        /// Get/Set values from string key
        /// </summary>
        /// <value></value>
        object this[string index] { get; set; }

        #endregion
    }

    #region Cookie Bag

    /// <summary>
    /// This class utilizes cookies to store the data
    ///     Encryption was not added though and may not be complete
    /// </summary>
    public class CookieBag : IWebBagHolder
    {
        #region Indexers

        /// <summary>
        ///     Default property for accessing resources
        ///     <param name = "index">
        ///         Index for the desired resource</param>
        ///     <returns>
        ///         Resource string value</returns>
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                var cookie = (HttpCookie)CookieUtil.Retrieve(index);

                if (cookie != null)
                {
                    return cookie.Value;
                }

                return null;
            }

            set
            {
                lock (this)
                {
                    CookieUtil.Add(index, value);
                }
            }
        }

        /// <summary>
        ///     Get Data Value
        /// </summary>
        /// <value></value>
        public object this[string index]
        {
            get
            {
                var cookie = (HttpCookie)CookieUtil.Retrieve(index);

                return cookie != null ? cookie.Value : null;
            }

            set
            {
                lock (this)
                {
                    CookieUtil.Add(index, value);
                }
            }
        }

        #endregion
    }

    #endregion

    #region Session Bag

    /// <summary>
    /// The Session bag will attempt to use server-side Session Mgmt. However
    ///    in the event session is disabled fallback to cookies; otherwise, fallback
    ///    to the database :(
    /// </summary>
    public class SessionBag : IWebBagHolder
    {
        #region Indexers

        /// <summary>
        ///     Default property for accessing resources
        ///     <param name = "index">
        ///         Index for the desired resource</param>
        ///     <returns>
        ///         Resource string value</returns>
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                object obj = null;

                try
                {
                    // session state enabled
                    if (HttpContext.Current.Session != null)
                    {
                        obj = HttpContext.Current.Session[index];
                    }
                }
                catch
                {
                    obj = null;
                }

                return obj;
            }

            set
            {
                try
                {
                    // session state enabled
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[index] = value;
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     get data back
        /// </summary>
        /// <value></value>
        public object this[string index]
        {
            get
            {
                object obj = null;

                try
                {
                    // session state  enabled
                    if (HttpContext.Current.Session != null)
                    {
                        obj = HttpContext.Current.Session[index];
                    }
                }
                catch
                {
                    obj = null;
                }

                return obj;
            }

            set
            {
                try
                {
                    // session state  enabled
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[index] = value;
                    }
                }
                catch
                {
                }
            }
        }

        #endregion
    }

    #endregion

    #region Application Bag

    /// <summary>
    /// The Application bag will attempt to use server-side Application Mgmt.
    ///     This is the fallback. The database could also be a fallback as well
    ///     but then anonnymous user information would be logged there as well which
    ///     I don't you would want
    /// </summary>
    public class ApplicationBag : IWebBagHolder
    {
        #region Indexers

        /// <summary>
        ///     Default property for accessing resources
        ///     <param name = "index">
        ///         Index for the desired resource</param>
        ///     <returns>
        ///         Resource string value</returns>
        /// </summary>
        /// <value></value>
        public object this[int index]
        {
            get
            {
                return HttpContext.Current.Application.Get(index);
            }

            set
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application.Add(index.ToString(), value);
                HttpContext.Current.Application.UnLock();
            }
        }

        /// <summary>
        ///     Get data value
        /// </summary>
        /// <value></value>
        public object this[string index]
        {
            get
            {
                return HttpContext.Current.Application.Get(index);
            }

            set
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application.Add(index, value);
                HttpContext.Current.Application.UnLock();
            }
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// This singleton factory creates the appropriate Bag for a user to hold data
    ///     The data retention mechanism varies.
    /// </summary>
    public sealed class BagFactory
    {
        #region Constants and Fields

        /// <summary>
        ///     the singleton class instance (ONLY ONE -- APPLICATION LEVEL )
        /// </summary>
        public static readonly BagFactory instance = new BagFactory();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="BagFactory"/> class from being created. 
        ///     don't allow for construction outside of this class
        /// </summary>
        /// <returns>
        /// A void value...
        /// </returns>
        private BagFactory()
        {
        }

        #endregion

        #region Enums

        /// <summary>
        /// valid bag types
        /// </summary>
        public enum BagFactoryType
        {
            /// <summary>
            /// No bag factory type.
            /// </summary>
            /// <remarks>
            /// </remarks>
            None = 0, 

            /// <summary>
            /// The application type.
            /// </summary>
            /// <remarks>
            /// </remarks>
            ApplicationType = 1, 

            /// <summary>
            /// The session type.
            /// </summary>
            /// <remarks>
            /// </remarks>
            SessionType = 2, 

            /// <summary>
            /// The cookie type.
            /// </summary>
            /// <remarks>
            /// </remarks>
            CookieType = 3, 

            /// <summary>
            /// The db type.
            /// </summary>
            /// <remarks>
            /// </remarks>
            DbType = 4, 
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create the Bag according to what is available. First use Cookies,
        ///     then try Session and then fallback to Application.
        /// </summary>
        /// <returns>
        /// </returns>
        public IWebBagHolder create()
        {
            // if no session, use cookies
            if (HttpContext.Current.Session == null)
            {
                return new CookieBag();
            }

            if (SessionStateMode.InProc == HttpContext.Current.Session.Mode)
            {
                // use the session
                return new SessionBag();
            }

            // get the default bag
            return new ApplicationBag();
        }

        /// <summary>
        /// Create the Bag according to what is available. First use Cookies,
        ///     then try Session and then fallback to Application.
        /// </summary>
        /// <param name="bagType">
        /// The bag_type.
        /// </param>
        /// <returns>
        /// </returns>
        public IWebBagHolder create(BagFactoryType bagType)
        {
            // application type
            if (bagType == BagFactoryType.ApplicationType)
            {
                // get the appli. bag
                return new ApplicationBag();
            }

            if (bagType == BagFactoryType.SessionType && // if session is not available, then use the default
                     HttpContext.Current.Session != null)
            {
                // session type
                return new SessionBag();
            }

            if (bagType == BagFactoryType.CookieType)
            {
                // cookie type
                return new CookieBag();
            }

            // get the default bag
            return new ApplicationBag();
        }

        #endregion
    }
}