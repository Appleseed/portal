// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeManager.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The ThemeManager class encapsulates all data logic necessary to
//   use different themes across the entire portal.
//   Manages the Load and Save of the Themes.
//   Encapsulates a Theme object that contains all the settings
//   of the current Theme.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;
    using System.Xml.Serialization;

    using Appleseed.Framework.Settings.Cache;

    /// <summary>
    /// The ThemeManager class encapsulates all data logic necessary to
    ///   use different themes across the entire portal.
    ///   Manages the Load and Save of the Themes.
    ///   Encapsulates a Theme object that contains all the settings
    ///   of the current Theme.
    /// </summary>
    public class ThemeManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The portal path.
        /// </summary>
        private readonly string portalPath;

        /// <summary>
        ///   The current theme.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private Theme currentTheme = new Theme();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager"/> class.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public ThemeManager(string portalPath)
        {
            this.portalPath = portalPath;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the path of the Theme dir (Physical path)
        ///   used ot load Themes
        /// </summary>
        /// <value>The physical  path.</value>
        public static string Path
        {
            get
            {
                return HttpContext.Current.Server.MapPath(WebPath);
            }
        }

        /// <summary>
        ///   Gets the path of the Theme dir (Web side)
        ///   used to reference images
        /// </summary>
        /// <value>The web path.</value>
        public static string WebPath
        {
            get
            {
                return Settings.Path.WebPathCombine(Settings.Path.ApplicationRoot, "/Design/Themes");
            }
        }

        /// <summary>
        ///   Gets or sets the current theme.
        /// </summary>
        public Theme CurrentTheme
        {
            get
            {
                return this.currentTheme;
            }

            set
            {
                this.currentTheme = value;
            }
        }

        /// <summary>
        ///   Gets the path of the current portal Theme dir (Physical path)
        ///   used to load Themes
        /// </summary>
        /// <value>The portal theme path.</value>
        public string PortalThemePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath(this.PortalWebPath);
            }
        }

        /// <summary>
        ///   Gets the path of the current portal Theme dir (Web side)
        ///   used to reference images
        /// </summary>
        /// <value>The portal web path.</value>
        public string PortalWebPath
        {
            get
            {
                var portalWebPath = Settings.Path.WebPathCombine(
                    Settings.Path.ApplicationRoot, this.portalPath, "/Themes");
                return portalWebPath;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Read the Path dir and returns an ArrayList with all the Themes found.
        ///   Static because the list is Always the same.
        /// </summary>
        /// <returns>
        /// A list of theme items.
        /// </returns>
        public static List<ThemeItem> GetPublicThemes()
        {
            List<ThemeItem> baseThemeList;

            if (!CurrentCache.Exists(Key.ThemeList(Path)))
            {
                // Initialize array

                // Try to read directories from public theme path
                var themes = Directory.Exists(Path) ? Directory.GetDirectories(Path) : new string[0];

                // Ignore CVS and SVN.
                baseThemeList =
                    themes.Select(t1 => new ThemeItem { Name = t1.Substring(Path.Length + 1) }).Where(
                        t => t.Name != "CVS" && t.Name != "_svn").ToList();
                CurrentCache.Insert(Key.ThemeList(Path), baseThemeList, new CacheDependency(Path));
            }
            else
            {
                baseThemeList = (List<ThemeItem>)CurrentCache.Get(Key.ThemeList(Path));
            }

            return baseThemeList;
        }

        /// <summary>
        /// Called when [remove].
        /// </summary>
        /// <param name="key">
        /// The key to be remove.
        /// </param>
        /// <param name="cacheItem">
        /// The cache item.
        /// </param>
        /// <param name="reason">
        /// The reason.
        /// </param>
        public static void OnRemove(string key, object cacheItem, CacheItemRemovedReason reason)
        {
            ErrorHandler.Publish(
                LogLevel.Info, 
                string.Format("The cached value with key '{0}' was removed from the cache.  Reason: {1}", key, reason));
        }

        /// <summary>
        /// Clears the cache list.
        /// </summary>
        public void ClearCacheList()
        {
            // Clear cache
            CurrentCache.Remove(Key.ThemeList(Path));
            CurrentCache.Remove(Key.ThemeList(this.PortalThemePath));
        }

        /// <summary>
        /// Read the Path dir and returns
        ///   an ArrayList with all the Themes found, public and privates
        /// </summary>
        /// <returns>
        /// A list of theme items.
        /// </returns>
        public List<ThemeItem> GetPrivateThemes()
        {
            List<ThemeItem> privateThemeList;

            if (!CurrentCache.Exists(Key.ThemeList(this.PortalThemePath)))
            {
                privateThemeList = new List<ThemeItem>();

                // Try to read directories from private theme path
                var themes = Directory.Exists(this.PortalThemePath)
                                 ? Directory.GetDirectories(this.PortalThemePath)
                                 : new string[0];

                for (var i = 0; i <= themes.GetUpperBound(0); i++)
                {
                    var t = new ThemeItem { Name = themes[i].Substring(this.PortalThemePath.Length + 1) };

                    // Ignore CVS and SVN
                    if (t.Name != "CVS" && t.Name != "_svn")
                    {
                        privateThemeList.Add(t);
                    }
                }

                CurrentCache.Insert(
                    Key.ThemeList(this.PortalThemePath), privateThemeList, new CacheDependency(this.PortalThemePath));

                // Debug.WriteLine("Storing privateThemeList in Cache: item count is " + privateThemeList.Count.ToString());
            }
            else
            {
                privateThemeList = (List<ThemeItem>)CurrentCache.Get(Key.ThemeList(this.PortalThemePath));

                // Debug.WriteLine("Retrieving privateThemeList from Cache: item count is " + privateThemeList.Count.ToString());
            }

            return privateThemeList;
        }

        /// <summary>
        /// Read the Path dir and returns
        ///   an ArrayList with all the Themes found, public and privates
        /// </summary>
        /// <returns>
        /// A list of theme items.
        /// </returns>
        public List<ThemeItem> GetThemes()
        {
            var themeList = GetPublicThemes().Clone();
            var themeListPrivate = this.GetPrivateThemes();

            themeList.AddRange(themeListPrivate);

            return themeList;
        }

        /// <summary>
        /// Loads the specified theme name.
        /// </summary>
        /// <param name="themeName">
        /// Name of the theme.
        /// </param>
        public void Load(string themeName)
        {
            this.currentTheme = new Theme { Name = themeName };

            // Try loading private theme first
            if (this.LoadTheme(Settings.Path.WebPathCombine(this.PortalWebPath, themeName)))
            {
                return;
            }

            // Try loading public theme
            if (this.LoadTheme(Settings.Path.WebPathCombine(WebPath, themeName)))
            {
                return;
            }

            // Try default
            this.currentTheme.Name = "default";
            if (this.LoadTheme(Settings.Path.WebPathCombine(WebPath, "default")))
            {
                return;
            }

            var errormsg = General.GetString("LOAD_THEME_ERROR");
            throw new FileNotFoundException(
                errormsg.Replace("%1%", string.Format("'{0}'", themeName)), string.Format("{0}/{1}", WebPath, themeName));
        }

        /// <summary>
        /// Saves the specified theme name.
        /// </summary>
        /// <param name="themeName">
        /// Name of the theme.
        /// </param>
        public void Save(string themeName)
        {
            this.currentTheme.Name = themeName;
            this.currentTheme.WebPath = Settings.Path.WebPathCombine(WebPath, themeName);
            var serializer = new XmlSerializer(typeof(Theme));

            // Create an XmlTextWriter using a FileStream.
            using (Stream fs = new FileStream(this.currentTheme.ThemeFileName, FileMode.Create))
            using (XmlWriter writer = new XmlTextWriter(fs, new UTF8Encoding()))
            {
                serializer.Serialize(writer, this.currentTheme);
                writer.Close();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the theme.
        /// </summary>
        /// <param name="currentWebPath">
        /// The current web path.
        /// </param>
        /// <returns>
        /// A bool value...
        /// </returns>
        private bool LoadTheme(string currentWebPath)
        {
            this.currentTheme.WebPath = currentWebPath;

            // if (!Appleseed.Framework.Settings.Cache.CurrentCache.Exists (Appleseed.Framework.Settings.Cache.Key.CurrentTheme(CurrentWebPath)))
            if (!CurrentCache.Exists(Key.CurrentTheme(this.currentTheme.Path)))
            {
                if (File.Exists(this.currentTheme.ThemeFileName))
                {
                    if (this.LoadXml(this.currentTheme.ThemeFileName))
                    {
                        // Appleseed.Framework.Settings.Cache.CurrentCache.Insert(Appleseed.Framework.Settings.Cache.Key.CurrentTheme(CurrentWebPath), CurrentTheme, new CacheDependency(CurrentTheme.ThemeFileName));
                        CurrentCache.Insert(
                            Key.CurrentTheme(this.currentTheme.Path), 
                            this.currentTheme, 
                            new CacheDependency(this.currentTheme.Path));
                    }
                    else
                    {
                        // failed
                        return false;
                    }
                }
                else
                {
                    // Return fail
                    return false;
                }
            }
            else
            {
                // CurrentTheme = (Theme) Appleseed.Framework.Settings.Cache.CurrentCache.Get (Appleseed.Framework.Settings.Cache.Key.CurrentTheme(CurrentWebPath));
                this.currentTheme = (Theme)CurrentCache.Get(Key.CurrentTheme(this.currentTheme.Path));
            }

            this.currentTheme.WebPath = currentWebPath;
            return true;
        }

        /// <summary>
        /// Loads the XML.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        /// <returns>
        /// A bool value...
        /// </returns>
        private bool LoadXml(string filename)
        {
            var nt = new NameTable();
            var nsm = new XmlNamespaceManager(nt);
            nsm.AddNamespace(string.Empty, "http://www.w3.org/1999/xhtml");
            var context = new XmlParserContext(nt, nsm, string.Empty, XmlSpace.None);
            var returnValue = false;

            try
            {
                // Create an XmlTextReader using a FileStream.
                using (Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        var xtr = new XmlTextReader(fs, XmlNodeType.Document, context)
                            {
                               WhitespaceHandling = WhitespaceHandling.None 
                            };
                        ThemeImage themeImage;
                        var themePart = new ThemePart();

                        while (!xtr.EOF)
                        {
                            if (xtr.MoveToContent() == XmlNodeType.Element)
                            {
                                switch (xtr.LocalName)
                                {
                                    case "Name":
                                        this.currentTheme.Name = xtr.ReadString();
                                        break;

                                    case "Type":
                                        this.currentTheme.Type = xtr.ReadString();
                                        break;

                                    case "Css":
                                        this.currentTheme.Css = xtr.ReadString();
                                        break;

                                    case "MinimizeColor":
                                        this.currentTheme.MinimizeColor = xtr.ReadString();
                                        break;

                                    case "ThemeImage":
                                        themeImage = new ThemeImage();

                                        while (xtr.MoveToNextAttribute())
                                        {
                                            switch (xtr.LocalName)
                                            {
                                                case "Name":
                                                    themeImage.Name = xtr.Value;
                                                    break;

                                                case "ImageUrl":
                                                    themeImage.ImageUrl = xtr.Value;
                                                    break;

                                                case "Width":
                                                    themeImage.Width = double.Parse(xtr.Value);
                                                    break;

                                                case "Height":
                                                    themeImage.Height = double.Parse(xtr.Value);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        this.currentTheme.ThemeImages.Add(themeImage.Name, themeImage);
                                        xtr.MoveToElement();
                                        break;

                                    case "ThemePart":
                                        themePart = new ThemePart();

                                        while (xtr.MoveToNextAttribute())
                                        {
                                            switch (xtr.LocalName)
                                            {
                                                case "Name":
                                                    themePart.Name = xtr.Value;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        xtr.MoveToElement();
                                        break;

                                    case "HTML":

                                        if (themePart.Name.Length != 0)
                                        {
                                            themePart.Html = xtr.ReadString();
                                        }

                                        // Moved here on load instead on retrival.
                                        // by Manu
                                        var w = string.Concat(this.currentTheme.WebPath, "/");
                                        themePart.Html = themePart.Html.Replace("src='", string.Concat("src='", w));
                                        themePart.Html = themePart.Html.Replace("src=\"", string.Concat("src=\"", w));
                                        themePart.Html = themePart.Html.Replace(
                                            "background='", string.Concat("background='", w));
                                        themePart.Html = themePart.Html.Replace(
                                            "background=\"", string.Concat("background=\"", w));
                                        this.currentTheme.ThemeParts.Add(themePart.Name, themePart);
                                        break;

                                    default:

                                        // Debug.WriteLine(" - unwanted");
                                        break;
                                }
                            }

                            xtr.Read();
                        }

                        returnValue = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Publish(
                            LogLevel.Error, 
                            string.Format("Failed to Load XML Theme : {0} Message was: {1}", filename, ex.Message));
                    }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(
                    LogLevel.Error, 
                    string.Format("Failed to open XML Theme : {0} Message was: {1}", filename, ex.Message));
            }

            return returnValue;
        }

        #endregion
    }
}