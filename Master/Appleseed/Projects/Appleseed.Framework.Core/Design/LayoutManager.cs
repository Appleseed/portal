// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutManager.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The LayoutManager class encapsulates all data logic necessary to
//   use different Layouts across the entire portal.
//   Manages the Load and Save of the Layouts.
//   Encapsulates a Layout object that contains all the settings
//   of the current Layout.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    using Appleseed.Framework.Settings.Cache;

    /// <summary>
    /// The LayoutManager class encapsulates all data logic necessary to
    ///   use different Layouts across the entire portal.
    ///   Manages the Load and Save of the Layouts.
    ///   Encapsulates a Layout object that contains all the settings
    ///   of the current Layout.
    /// </summary>
    /// <remarks>
    /// by Cory Isakson
    /// </remarks>
    public class LayoutManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The portal path.
        /// </summary>
        private readonly string portalPath;

        /*
        // Jes1111 - not needed for new version ... see below
        /// <summary>
        ///
        /// </summary>
        private static ArrayList cachedLayoutsList;
        */

        /// <summary>
        ///   The inner web path.
        /// </summary>
        private static string innerWebPath;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutManager"/> class.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        public LayoutManager(string portalPath)
        {
            this.portalPath = portalPath;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the path of the Layout dir (Physical path) used to load Layouts
        /// </summary>
        public static string Path
        {
            get
            {
                return HttpContext.Current.Server.MapPath(WebPath);
            }
        }

        /// <summary>
        ///   Gets the path of the Layout dir (Web side) used to reference images
        /// </summary>
        public static string WebPath
        {
            get
            {
                return innerWebPath ??
                       (innerWebPath =
                        Settings.Path.WebPathCombine(Settings.Path.ApplicationRoot, "/Design/DesktopLayouts"));
            }
        }

        /// <summary>
        ///   Gets the path of the current portal Layout dir (Physical path) used to load Layouts
        /// </summary>
        public string PortalLayoutPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(this.PortalWebPath);
            }
        }

        /// <summary>
        ///   Gets the path of the current portal Layout dir (Web side) used to reference images
        /// </summary>
        public string PortalWebPath
        {
            get
            {
                // FIX by George James (ghjames)
                // http://sourceforge.net/tracker/index.php?func=detail&aid=735716&group_id=66837&atid=515929
                return Settings.Path.WebPathCombine(Settings.Path.ApplicationRoot, this.portalPath, "/DesktopLayouts");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Read the Path dir and returns an ArrayList with all the Layouts found.
        ///   Static because the list is Always the same.
        /// </summary>
        /// <returns>
        /// A list of public layouts.
        /// </returns>
        public static List<LayoutItem> GetPublicLayouts()
        {
            // Jes1111 - 27-02-2005 - new version - correct caching
            List<LayoutItem> baseLayoutList;

            if (!CurrentCache.Exists(Key.LayoutList(Path)))
            {
                // Try to read directories from public Layout path
                var layouts = Directory.Exists(Path) ? Directory.GetDirectories(Path) : new string[0];

                // Ignore CVS and SVN
                baseLayoutList =
                    layouts.Select(layout => new LayoutItem { Name = layout.Substring(Path.Length + 1) })
                    .Where(layout => layout.Name != "CVS" && layout.Name != "_svn" && layout.Name != ".svn")
                    .ToList();

                CurrentCache.Insert(Key.LayoutList(Path), baseLayoutList, new CacheDependency(Path));
            }
            else
            {
                baseLayoutList = (List<LayoutItem>)CurrentCache.Get(Key.LayoutList(Path));
            }

            return baseLayoutList;
        }

        /// <summary>
        /// Clears the cache list.
        /// </summary>
        public void ClearCacheList()
        {
            // Clear cache
            lock (this)
            {
                // Jes1111
                // LayoutManager.cachedLayoutsList = null;
                CurrentCache.Remove(Key.LayoutList(Path));
                CurrentCache.Remove(Key.LayoutList(this.PortalLayoutPath));
            }
        }

        /// <summary>
        /// Read the Path dir and returns an ArrayList with all the Layouts found, public and privates
        /// </summary>
        /// <returns>
        /// A list of layouts.
        /// </returns>
        public List<LayoutItem> GetLayouts()
        {
            // Jes1111 - 27-02-2005 - new version - correct caching
            var layoutList = GetPublicLayouts().Clone();
            var layoutListPrivate = this.GetPrivateLayouts();

            layoutList.AddRange(layoutListPrivate);

            return layoutList;
        }

        /// <summary>
        /// Gets the private layouts.
        /// </summary>
        /// <returns>
        /// A list of private layouts.
        /// </returns>
        public List<LayoutItem> GetPrivateLayouts()
        {
            List<LayoutItem> privateLayoutList;

            if (!CurrentCache.Exists(Key.LayoutList(this.PortalLayoutPath)))
            {
                privateLayoutList = new List<LayoutItem>();

                // Try to read directories from private theme path
                var layouts = Directory.Exists(this.PortalLayoutPath)
                                  ? Directory.GetDirectories(this.PortalLayoutPath)
                                  : new string[0];

                for (var i = 0; i <= layouts.GetUpperBound(0); i++)
                {
                    var t = new LayoutItem { Name = layouts[i].Substring(this.PortalLayoutPath.Length + 1) };

                    // Ignore CVS
                    if (t.Name != "CVS" && t.Name != "_svn" && t.Name != ".svn")
                    {
                        privateLayoutList.Add(t);
                    }
                }

                CurrentCache.Insert(
                    Key.LayoutList(this.PortalLayoutPath), privateLayoutList, new CacheDependency(this.PortalLayoutPath));
            }
            else
            {
                privateLayoutList = (List<LayoutItem>)CurrentCache.Get(Key.LayoutList(this.PortalLayoutPath));
            }

            return privateLayoutList;
        }

        #endregion
    }
}