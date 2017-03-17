// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlEditorDataType.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   List of available HTML editors
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.UI.WebControls.CodeMirror;

    using Appleseed.Framework.Web.UI.WebControls;

    using FreeTextBoxControls;

    using Syrinx.Gui.AspNet;

    using Appleseed.Framework.UI.WebControls;


    /// <summary>
    /// List of available HTML editors
    /// </summary>
    public class HtmlEditorDataType : ListDataType<string, DropDownList>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "HtmlEditorDataType" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public HtmlEditorDataType()
        {
            // this.Type = PropertiesDataType.List;
            this.InitializeComponents();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the data source.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override object DataSource
        {
            get
            {
                return "Code Mirror Plain Text;CKeditor;Codewriter".Split(';');
            }
        }

        /// <summary>
        ///   Gets the description.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override string Description
        {
            get
            {
                return "HtmlEditor List";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// HTMLs the editor settings.
        /// </summary>
        /// <param name="editorSettings">
        /// The editor settings.
        /// </param>
        /// <param name="group">
        /// The group.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void HtmlEditorSettings(Dictionary<string, ISettingItem> editorSettings, SettingItemGroup group)
        {
            var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            var editor = new SettingItem<string, DropDownList>(new HtmlEditorDataType())
            {
                // Order = 1; modified by Hongwei Shen(hongwei.shen@gmail.com) 11/9/2005
                Order = (int)group + 1,
                Group = group,
                EnglishName = "Editor",
                Description = "Select the Html Editor for Module"
            };

            var controlWidth = new SettingItem<int, TextBox>(new BaseDataType<int, TextBox>())
            {
                Value = 700,
                Order = (int)group + 2,
                Group = group,
                EnglishName = "Editor Width",
                Description = "The width of editor control"
            };

            var controlHeight = new SettingItem<int, TextBox>(new BaseDataType<int, TextBox>())
            {
                Value = 400,
                Order = (int)group + 3,
                Group = group,
                EnglishName = "Editor Height",
                Description = "The height of editor control"
            };

            var showUpload = new SettingItem<bool, CheckBox>(new BaseDataType<bool, CheckBox>())
            {
                Value = true,
                Order = (int)group + 4,
                Group = group,
                EnglishName = "Upload?",
                Description = "Only used if Editor is ActiveUp HtmlTextBox"
            };

            var HTMLWrapperClass = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {

                Order = (int)group + 5,
                Group = group,
                EnglishName = "HTML Wrapper Class",
                Description = "HTML Wrapper Class"
            };

            SettingItem<string, Panel> moduleImageFolder = null;
            if (pS != null)
            {
                if (pS.PortalFullPath != null)
                {
                    moduleImageFolder =
                        new SettingItem<string, Panel>(
                            new FolderDataType(
                                HttpContext.Current.Server.MapPath(string.Format("{0}/images", pS.PortalFullPath)),
                                "default"))
                        {
                            Value = "default",
                            Order = (int)group + 5,
                            Group = group,
                            EnglishName = "Default Image Folder",
                            Description = "This folder is used for editor in this module to take and upload images"
                        };
                }

                // Set the portal default values
                if (pS.CustomSettings != null)
                {
                    if (pS.CustomSettings["SITESETTINGS_DEFAULT_EDITOR"] != null)
                    {
                        editor.Value = pS.CustomSettings["SITESETTINGS_DEFAULT_EDITOR"].ToString();
                    }

                    if (pS.CustomSettings["SITESETTINGS_EDITOR_WIDTH"] != null)
                    {
                        controlWidth.Value =
                            pS.CustomSettings["SITESETTINGS_EDITOR_WIDTH"].ToInt32(CultureInfo.InvariantCulture);
                    }

                    if (pS.CustomSettings["SITESETTINGS_EDITOR_HEIGHT"] != null)
                    {
                        controlHeight.Value =
                            pS.CustomSettings["SITESETTINGS_EDITOR_HEIGHT"].ToInt32(CultureInfo.InvariantCulture);
                    }

                    if (pS.CustomSettings["SITESETTINGS_EDITOR_HEIGHT"] != null)
                    {
                        controlHeight.Value =
                            pS.CustomSettings["SITESETTINGS_EDITOR_HEIGHT"].ToInt32(CultureInfo.InvariantCulture);
                    }

                    if (pS.CustomSettings["SITESETTINGS_SHOWUPLOAD"] != null)
                    {
                        showUpload.Value =
                            pS.CustomSettings["SITESETTINGS_SHOWUPLOAD"].ToBoolean(CultureInfo.InvariantCulture);
                    }

                    if (pS.CustomSettings["HTML_WRAPPER_CLASS"] != null)
                    {
                        HTMLWrapperClass.Value =
                            pS.CustomSettings["HTML_WRAPPER_CLASS"].ToString(CultureInfo.InvariantCulture);
                    }

                    if (pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"] != null)
                    {
                        if (moduleImageFolder != null)
                        {
                            moduleImageFolder.Value = pS.CustomSettings["SITESETTINGS_DEFAULT_IMAGE_FOLDER"].ToString();
                        }
                    }
                }
            }

            editorSettings.Add("Editor", editor);
            editorSettings.Add("Width", controlWidth);
            editorSettings.Add("Height", controlHeight);
            editorSettings.Add("ShowUpload", showUpload);
            editorSettings.Add("HTMLWrapperClass", HTMLWrapperClass);
            if (moduleImageFolder != null)
            {
                editorSettings.Add("MODULE_IMAGE_FOLDER", moduleImageFolder);
            }
        }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        /// <param name="placeHolderHtmlEditor">
        /// The place holder HTML editor.
        /// </param>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="showUpload">
        /// The show Upload.
        /// </param>
        /// <param name="PortalSettings">
        /// The portal Settings.
        /// </param>
        /// <returns>
        /// The HTML editor interface.
        /// </returns>
        public IHtmlEditor GetEditor(
            Control placeHolderHtmlEditor, int moduleId, bool showUpload, PortalSettings PortalSettings)
        {
            IHtmlEditor desktopText;
            var moduleImageFolder = ModuleSettings.GetModuleSettings(moduleId)["MODULE_IMAGE_FOLDER"].ToString();

            // Grabs ID from the place holder so that a unique editor is on the page if more than one
            // But keeps same ID so that the information can be submitted to be saved. [CDT]
            var uniqueId = placeHolderHtmlEditor.ID;

            switch (this.Value)
            {

                case "CKeditor": // 09/03/2016
                    var CKfckv2 = new CKEditorTextbox
                    {
                        ImageFolder = moduleImageFolder,
                        ResizeEnabled = false,
                        Language = PortalSettings.PortalUILanguage.TwoLetterISOLanguageName
                    };

                    CKfckv2.BasePath = Path.WebPathCombine(
                        Path.ApplicationRoot, "/aspnet_client/CKeditorV4.5.7");

                    desktopText = CKfckv2;
                    break;

                case "Codewriter":
                    desktopText = new CodeWriterTextbox();
                    break;

                // case "Code Mirror Plain Text":
                default:
                    var codeMirrorTextBox = new CodeMirrorTextBox();
                    desktopText = codeMirrorTextBox;
                    break;
            }

            placeHolderHtmlEditor.Controls.Add((Control)desktopText);
            return desktopText;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the components.
        /// </summary>
        protected override void InitializeComponents()
        {
            base.InitializeComponents();

            // Default
            this.Value = "FreeTextBox";

            // Change the default value to Portal Default Editor Value by jviladiu@portalServices.net 13/07/2004
            if (HttpContext.Current == null || HttpContext.Current.Items["PortalSettings"] == null)
            {
                return;
            }

            var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            if (pS.CustomSettings == null)
            {
                return;
            }

            if (pS.CustomSettings["SITESETTINGS_DEFAULT_EDITOR"] != null)
            {
                this.Value = pS.CustomSettings["SITESETTINGS_DEFAULT_EDITOR"].ToString();
            }
        }

        /// <summary>
        /// Gets the FTB language.
        /// </summary>
        /// <param name="appleseedLanguage">
        /// The Appleseed language.
        /// </param>
        /// <returns>
        /// The language.
        /// </returns>
        private static string GetFtbLanguage(string appleseedLanguage)
        {
            switch (appleseedLanguage.Substring(appleseedLanguage.Length - 2).ToLower())
            {
                case "en":
                    return "en-US";
                case "us":
                    return "en-US";
                case "es":
                    return "es-ES";
                case "cn":
                    return "zh-cn";
                case "cz":
                    return "cz-CZ";
                case "fi":
                    return "fi-fi";
                case "nl":
                    return "nl-NL";
                case "de":
                    return "de-de";
                case "il":
                    return "he-IL";
                case "it":
                    return "it-IT";
                case "jp":
                    return "ja-JP";
                case "kr":
                    return "ko-kr";
                case "no":
                    return "nb-NO";
                case "pt":
                    return "pt-pt";
                case "ro":
                    return "ro-RO";
                case "ru":
                    return "ru-ru";
                case "se":
                    return "sv-se";
                case "tw":
                    return "zh-TW";
                default:
                    return "en-US";
            }
        }

        #endregion
    }
}