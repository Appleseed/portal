// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OneFileModule.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The OneFileModule provides basis for a module that only
//   consists of a single .ascx file.
//   See the OneFileModule Kit for documentation and examples.
//   Written by: Jakob Hansen, hansen3000@hotmail
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// The OneFileModule provides basis for a module that only
    ///   consists of a single .ascx file. 
    ///   See the OneFileModule Kit for documentation and examples.
    ///   Written by: Jakob Hansen, hansen3000@hotmail
    /// </summary>
    public class OneFileModule : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        ///   True if setting "Debug Mode" is clicked
        /// </summary>
        protected bool _debugMode;

        /// <summary>
        ///   False if settings are missing
        /// </summary>
        protected bool _settingsExists;

        /// <summary>
        ///   The content of setting "Settings string"
        /// </summary>
        protected string _settingsStr = string.Empty;

        /// <summary>
        /// The settings type.
        /// </summary>
        protected SettingsType _settingsType = SettingsType.Off;

        /// <summary>
        ///   The XML in file xmlFileName
        /// </summary>
        protected XmlDocument _settingsXml;

        /// <summary>
        ///   The filename in setting "XML settings file"
        /// </summary>
        protected string _xmlFileName = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OneFileModule"/> class. 
        ///   Creates 3 fields in the settings system:
        ///   "Settings string", "XML settings file" and "Debug Mode"
        /// </summary>
        public OneFileModule()
        {
            var setting = new SettingItem<string, TextBox>
                {
                    Required = false, 
                    Order = 1, 
                    Value = string.Empty, 
                    EnglishName = "Settings string", 
                    Description = "Settings are in pairs like: FirstName=Elvis;LastName=Presly;"
                };
            this.BaseSettings.Add("Settings string", setting);

            var xmlFile = new SettingItem<string, TextBox>(new PortalUrlDataType())
                {
                    Required = false, 
                    Order = 2, 
                    EnglishName = "XML settings file", 
                    Description =
                        "Name of file in folder Appleseed\\_Portalfolder (typically _Appleseed). Do not add a path!"
                };
            this.BaseSettings.Add("XML settings file", xmlFile);

            var debugMode = new SettingItem<bool, CheckBox>
                {
                    Order = 3, 
                    Value = true, 
                    EnglishName = "Debug Mode", 
                    Description = "Primarily for the developer. Controls property DebugMode"
                };
            this.BaseSettings.Add("Debug Mode", debugMode);
        }

        #endregion

        #region Enums

        /// <summary>
        /// The settings type.
        /// </summary>
        /// <remarks>
        /// With StrAndXml SettingsStr are searched first
        /// </remarks>
        public enum SettingsType
        {
            /// <summary>
            /// The off.
            /// </summary>
            Off, 

            /// <summary>
            /// The str.
            /// </summary>
            Str, 

            /// <summary>
            /// The xml.
            /// </summary>
            Xml, 

            /// <summary>
            /// The str and xml.
            /// </summary>
            StrAndXml
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether same as "Debug Mode" in the setting system.
        /// </summary>
        /// <value><c>true</c> if [debug mode]; otherwise, <c>false</c>.</value>
        public bool DebugMode
        {
            get
            {
                return this._debugMode;
            }

            set
            {
                this._debugMode = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [settings exists].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [settings exists]; otherwise, <c>false</c>.
        /// </value>
        public bool SettingsExists
        {
            get
            {
                return this._settingsExists;
            }
        }

        /// <summary>
        ///   Gets same as "Settings string" in the setting system
        /// </summary>
        /// <value>The settings STR.</value>
        public string SettingsStr
        {
            get
            {
                return this._settingsStr;
            }
        }

        /// <summary>
        ///   Gets the XML in file XmlFileName
        /// </summary>
        /// <value>The settings XML.</value>
        public XmlDocument SettingsXml
        {
            get
            {
                return this._settingsXml;
            }
        }

        /// <summary>
        ///   Gets same as "XML settings file" in the setting system
        /// </summary>
        /// <value>The name of the XML file.</value>
        public string XmlFileName
        {
            get
            {
                return this._xmlFileName;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the setting value from SettingsStr.
        ///   If not found in SettingsStr then SettingsXml are searched.
        ///   This function uses GetStrSetting() and GetXmlSetting
        /// </summary>
        /// <param name="settingName">
        /// Name of the setting.
        /// </param>
        /// <returns>
        /// The get setting.
        /// </returns>
        protected string GetSetting(string settingName)
        {
            if (this._settingsType == SettingsType.Off)
            {
                return string.Empty;
            }

            var retVal = this.GetStrSetting(settingName);
            if (retVal == string.Empty)
            {
                retVal = this.GetXmlSetting(settingName);
            }

            return retVal;
        }

        /// <summary>
        /// Fills the settingsXml parameter with the xml from a file
        /// </summary>
        /// <param name="settingsXml">
        /// The settings XML.
        /// </param>
        /// <param name="file">
        /// The file path.
        /// </param>
        /// <returns>
        /// The get settings xml.
        /// </returns>
        protected bool GetSettingsXml(ref XmlDocument settingsXml, string file)
        {
            var retValue = true;

            var pt = new PortalUrlDataType { Value = file };
            var xmlFile = pt.FullPath;

            if (!string.IsNullOrEmpty(xmlFile))
            {
                if (File.Exists(this.Server.MapPath(xmlFile)))
                {
                    settingsXml.Load(this.Server.MapPath(xmlFile));
                }
                else
                {
                    retValue = false;
                }
            }

            return retValue;
        }

        /// <summary>
        /// Get the setting value from SettingsStr which has the form:
        ///   nameA=valueA;nameB=valueB;nameC=valueC
        /// </summary>
        /// <param name="settingName">
        /// Name of the setting.
        /// </param>
        /// <returns>
        /// The get str setting.
        /// </returns>
        protected string GetStrSetting(string settingName)
        {
            if (this._settingsType == SettingsType.Off || this._settingsType == SettingsType.Xml ||
                this._settingsStr == string.Empty)
            {
                return string.Empty;
            }

            var idxStart = this._settingsStr.IndexOf(settingName);
            if (idxStart == -1)
            {
                return string.Empty;
            }

            idxStart = this._settingsStr.IndexOf('=', idxStart);
            if (idxStart == -1)
            {
                return string.Empty;
            }

            string val;
            var idxEnd = this._settingsStr.IndexOf(';', idxStart);
            if (idxEnd == -1)
            {
                val = this._settingsStr.Substring(idxStart).Length == 0
                          ? string.Empty
                          : this._settingsStr.Substring(++idxStart);
            }
            else
            {
                idxStart++;
                val = this._settingsStr.Substring(idxStart, idxEnd - idxStart);
            }

            return val;
        }

        /// <summary>
        /// Get the setting value from the XML Document SettingsXml.
        ///   settingName is a XPath expression.
        /// </summary>
        /// <param name="settingName">
        /// Name of the setting.
        /// </param>
        /// <returns>
        /// The get xml setting.
        /// </returns>
        protected string GetXmlSetting(string settingName)
        {
            if (this._settingsType == SettingsType.Off || this._settingsType == SettingsType.Str)
            {
                return string.Empty;
            }

            var val = string.Empty;

            // Add default root to the xpath expression if missing
            if (settingName.IndexOf('/') == -1)
            {
                settingName = string.Format("settings/{0}", settingName);
            }

            var xmlNodeReader = new XmlNodeReader(this._settingsXml.SelectSingleNode(settingName));

            try
            {
                if (xmlNodeReader.Read())
                {
                    // If we get here the setting is in the xml file
                    // Move to next node (the text node contains the actual setting value)
                    if (xmlNodeReader.Read())
                    {
                        val = xmlNodeReader.Value;
                    }
                }
            }
            finally
            {
                xmlNodeReader.Close(); // by Manu, fixed bug 807858
            }

            return val;
        }

        /// <summary>
        /// Fills all settings: SettingsStr, SettingsXml and DebugMode
        ///   InitSettings() should be the first line of code in Page_Load().
        ///   If settings is missing settingsExists is set to false
        ///   Note: It is not mandatory to use InitSettings() in the
        ///   Page_Load() - The programmer can just leave it out if he
        ///   decides that he is not going to use the setting system that
        ///   class OneFileModule provides.
        /// </summary>
        /// <param name="settingsType">
        /// Type of the settings.
        /// </param>
        protected void InitSettings(SettingsType settingsType)
        {
            this._settingsType = settingsType;
            if (this._settingsType == SettingsType.Off)
            {
                return;
            }

            this._debugMode = "True" == this.Settings["Debug Mode"].ToString();
            this._xmlFileName = this.Settings["XML settings file"].ToString();

            this._settingsExists = true;

            if (this._settingsType == SettingsType.Str || this._settingsType == SettingsType.StrAndXml)
            {
                this._settingsStr = this.Settings["Settings string"].ToString();
                if (this._settingsStr == string.Empty)
                {
                    this._settingsExists = false;
                    this.Controls.Add(
                        new LiteralControl("<br /><span class='Error'>Settings string is missing</span><br />"));
                }
            }

            if (this._settingsType == SettingsType.Xml || this._settingsType == SettingsType.StrAndXml)
            {
                this._settingsXml = new XmlDocument();
                if (this.GetSettingsXml(ref this._settingsXml, this._xmlFileName) == false)
                {
                    this._settingsExists = false;
                    this.Controls.Add(
                        new LiteralControl(
                            string.Format(
                                "<br /><span class='Error'>XML {0}<br />", 
                                General.GetString("FILE_NOT_FOUND").Replace("%1%", this._xmlFileName))));
                }
            }
        }

        #endregion
    }
}