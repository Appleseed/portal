// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Theme.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The Theme class encapsulates all the settings
//   of the currently selected theme
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The Theme class encapsulates all the settings
    ///   of the currently selected theme
    /// </summary>
    [History("bja", "2003/04/26", "C1: [Future] Added minimize color for title bar")]
    public class Theme
    {
        #region Constants and Fields

        /// <summary>
        /// The default button path.
        /// </summary>
        public const string DefaultButtonPath = "~/Design/Themes/Default/icon";

        /// <summary>
        /// The default module css path.
        /// </summary>
        public const string DefaultModuleCSSPath = "~/Design/Themes/Default/mod";

        /// <summary>
        /// The default module image path.
        /// </summary>
        public const string DefaultModuleImagePath = "~/images/img";

        /// <summary>
        /// The theme images.
        /// </summary>
        public Hashtable ThemeImages = new Hashtable();

        /// <summary>
        /// The theme parts.
        /// </summary>
        public Hashtable ThemeParts = new Hashtable();

        /// <summary>
        /// The type.
        /// </summary>
        private string type = "classic";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Theme"/> class.
        /// </summary>
        public Theme()
        {
            this.MinimizeColor = string.Empty;
            this.Css = "Portal.css";
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Css file name without any path.
        /// </summary>
        /// <value>The CSS string.</value>
        public string Css { get; set; }

        /// <summary>
        ///   Gets the Css physical file name.
        ///   Set at runtime using Web Path.
        /// </summary>
        public string CssFile
        {
            get
            {
                return Settings.Path.WebPathCombine(this.WebPath, this.Css);
            }
        }

        /// <summary>
        ///   Gets or sets the Theme minimize color
        /// </summary>
        /// <remarks>
        ///   [START FUTURE bja:C1]
        /// </remarks>
        /// <value>The color of the minimize.</value>
        public string MinimizeColor { get; set; }

        /// <summary>
        ///   Gets or sets the Theme Name (must be the directory in which is located)
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///   Current Physical Path. Read-only.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                return HttpContext.Current.Server.MapPath(this.WebPath);
            }
        }

        /// <summary>
        ///   Get the Theme physical file name.
        ///   Set at runtime using Physical Path. NonSerialized.
        /// </summary>
        /// <value>The name of the theme file.</value>
        public string ThemeFileName
        {
            get
            {
                if (this.WebPath == string.Empty)
                {
                    throw new ArgumentNullException("Path", "Value cannot be null!");
                }

                // Try to get current theme from public folder
                return System.IO.Path.Combine(this.Path, "Theme.xml");
            }
        }

        /// <summary>
        ///   Gets or sets the type.
        /// </summary>
        /// <value>The type lowercased.</value>
        public string Type
        {
            get
            {
                return this.type.ToLower();
            }

            set
            {
                this.type = value.ToLower();
            }
        }

        /// <summary>
        ///   Gets or sets the current web path.
        ///   It is set at runtime and therefore is not serialized
        /// </summary>
        /// <value>The web path.</value>
        public string WebPath { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the HTML part.
        /// </summary>
        /// <param name="partName">
        /// The name of the part.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string GetHTMLPart(string partName)
        {
            // string html = GetThemePart(partName);
            // string w = string.Concat(WebPath, "/");
            // html = html.Replace("src='", string.Concat("src='", w));
            // html = html.Replace("src=\"", string.Concat("src=\"", w));
            // html = html.Replace("background='", string.Concat("background='", w));
            // html = html.Replace("background=\"", string.Concat("background=\"", w));
            // return html;
            return this.GetThemePart(partName);
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="defaultImagePath">
        /// The default image path.
        /// </param>
        /// <returns>
        /// A System.Web.UI.WebControls.Image value...
        /// </returns>
        public Image GetImage(string name, string defaultImagePath)
        {
            Image img;

            if (this.ThemeImages.ContainsKey(name))
            {
                img = ((ThemeImage)this.ThemeImages[name]).GetImage();
                img.ImageUrl = Settings.Path.WebPathCombine(this.WebPath, img.ImageUrl);
            }
            else
            {
                img = new Image
                    {
                        ImageUrl =
                            Settings.Path.WebPathCombine(
                                DefaultButtonPath.Replace("~", Settings.Path.ApplicationRoot), defaultImagePath)
                    };
            }

            return img;
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// A System.Web.UI.WebControls.Image value...
        /// </returns>
        [Obsolete("You are strongly invited to use the new overload the takes default as parameter")]
        public Image GetImage(string name)
        {
            return this.GetImage(name, "NoImage.gif");
        }

        /// <summary>
        /// Gets the literal control.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// A System.Web.UI.LiteralControl value...
        /// </returns>
        public LiteralControl GetLiteralControl(string name)
        {
            return new LiteralControl(this.GetHTMLPart(name));
        }

        /// <summary>
        /// Gets the literal image.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="defaultImagePath">
        /// The default image path.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string GetLiteralImage(string name, string defaultImagePath)
        {
            var img = this.GetImage(name, defaultImagePath);
            return string.Format("<img src='{0}' width='{1}' height='{2}'>", img.ImageUrl, img.Width, img.Height);
        }

        /// <summary>
        /// Get module specific image
        /// </summary>
        /// <param name="imageFileName">
        /// The image_file_name.
        /// </param>
        /// <returns>
        /// The get module image src.
        /// </returns>
        public string GetModuleImageSRC(string imageFileName)
        {
            string imagePath;

            // check if image file exists in current theme img folder
            // else fall back to default theme img folder
            // else fall back to module img folder
            // else use default spacer img
            if (
                File.Exists(
                    HttpContext.Current.Server.MapPath(string.Format("{0}/img/{1}", this.WebPath, imageFileName))))
            {
                imagePath = Settings.Path.WebPathCombine(this.WebPath, string.Format("/img/{0}", imageFileName));
            }
            else if (File.Exists(HttpContext.Current.Server.MapPath(DefaultModuleImagePath + imageFileName)))
            {
                imagePath =
                    Settings.Path.WebPathCombine(
                        DefaultModuleImagePath.Replace("~", Settings.Path.ApplicationRoot), imageFileName);
            }
                
                // TODO: Not Sure how to get current module path here
                // else if(File.Exists(HttpContext.Current.Server.MapPath(WebPath + "/img/" + image_file_name)))
                // {
                // DefaultModuleImagePath = "~/Design/Themes/Default/img";
                // Not Sure how to get current module path here
                // imagePath = Settings.Path.WebPathCombine(Settings.Path.ApplicationRoot, "/desktopmodules/"+ ;
                // }
            else
            {
                imagePath =
                    Settings.Path.WebPathCombine(
                        DefaultModuleImagePath.Replace("~", Settings.Path.ApplicationRoot), "1x1.gif");
            }

            return imagePath;
        }

        /// <summary>
        /// Gets the theme part.
        /// </summary>
        /// <param name="name">
        /// The part name.
        /// </param>
        /// <returns>
        /// The get theme part.
        /// </returns>
        /// <remarks>
        /// added: Jes1111 - 2004/08/27
        ///   Part of Zen support
        /// </remarks>
        public string GetThemePart(string name)
        {
            if (this.ThemeParts.ContainsKey(name))
            {
                var part = (ThemePart)this.ThemeParts[name];
                return part.Html;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the Css physical file name.
        ///   Set at runtime using Web Path.
        /// </summary>
        /// <param name="cssfilename">
        /// The css filename.
        /// </param>
        /// <returns>
        /// The module_ css file.
        /// </returns>
        public string Module_CssFile(string cssfilename)
        {
            var cssfilPath = string.Empty;

            if (File.Exists(HttpContext.Current.Server.MapPath(string.Format("{0}/mod/{1}", this.WebPath, cssfilename))))
            {
                cssfilPath = Settings.Path.WebPathCombine(this.WebPath, string.Format("/mod/{0}", cssfilename));
            }
            else if (
                File.Exists(
                    HttpContext.Current.Server.MapPath(string.Format("{0}/{1}", DefaultModuleCSSPath, cssfilename))))
            {
                cssfilPath =
                    Settings.Path.WebPathCombine(
                        DefaultModuleCSSPath.Replace("~", Settings.Path.ApplicationRoot), cssfilename);
            }

            return cssfilPath;
        }

        #endregion
    }
}