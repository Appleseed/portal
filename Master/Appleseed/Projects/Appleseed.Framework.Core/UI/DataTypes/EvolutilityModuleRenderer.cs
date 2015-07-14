namespace Appleseed.Framework.DataTypes
{
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Evolutility module Item
    /// </summary>
    public class EvolutilityModuleItem
    {
        /// <summary>
        /// ModuleID
        /// </summary>
        public int ModuleID { get; set; }

        /// <summary>
        /// Module Name
        /// </summary>
        public string ModuleName { get; set; }
    }

    /// <summary>
    /// Evolutility module renderer
    /// </summary>
    public class EvolutilityModuleRenderer : ListDataType<string, DropDownList>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "EvolutilityModuleRenderer" /> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public EvolutilityModuleRenderer()
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
                EvolutilityModuleDB moduleDB = new EvolutilityModuleDB();
                List<EvolutilityModuleItem> modules = new List<EvolutilityModuleItem>();
                modules.Add(new EvolutilityModuleItem() { ModuleID = 0, ModuleName = "Select Evolutility Module" });
                SqlDataReader modelReader = moduleDB.GetEvolutilyModuleList();
                if (modelReader.HasRows)
                {
                    while (modelReader.Read())
                    {
                        modules.Add(new EvolutilityModuleItem()
                        {
                            ModuleID = Convert.ToInt32(modelReader["ID"]),
                            ModuleName = modelReader["TITLE"].ToString()
                        });
                    }
                }

                return modules;
            }
        }

        /// <summary>
        /// Data text field
        /// </summary>
        public override string DataTextField
        {
            get
            {
                return "ModuleName";
            }
        }

        /// <summary>
        /// Data value field
        /// </summary>
        public override string DataValueField
        {
            get
            {
                return "ModuleID";
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
                return "Evolutility Modules";
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
        public static void ModuleRendererSettings(Dictionary<string, ISettingItem> editorSettings, SettingItemGroup group)
        {
            var modules = new SettingItem<string, DropDownList>(new EvolutilityModuleRenderer())
                {
                    Order = (int)group + 1,
                    Group = group,
                    EnglishName = "Modules",
                    Description = "Select the Module"
                };

            editorSettings.Add("Modules", modules);

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
           // this.Value = new EvolutilityModuleItem() { ModuleID = 0, ModuleName = "Select Evolutility Module" };

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
