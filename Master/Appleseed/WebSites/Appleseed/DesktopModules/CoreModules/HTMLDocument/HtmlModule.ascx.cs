// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlModule.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   HTML Document Module
//   Represents any text that can contain HTML
//   Edited with HTMLeditors
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.HTMLDocument
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.BLL.MergeEngine;
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Web.UI.WebControls;
    using Appleseed.PortalTemplate;
    using Appleseed.PortalTemplate.DTOs;
    using System.Xml.Serialization;
    using System.Web.UI.HtmlControls;
    /// <summary>
    /// HTML Document Module
    ///   Represents any text that can contain HTML
    ///   Edited with HTMLeditors
    /// </summary>
    public partial class HtmlModule : PortalModuleControl, IModuleExportable
    {
        // , IModuleExportable
        // Added by Hongwei Shen(Hongwei.shen@gmail.com) 10/9/2005
        // for supporting version compare
        #region Constants and Fields

        /// <summary>
        ///   Takes the html text and does a path correction before loading 
        ///   to the HTML document placeholder
        /// </summary>
        protected LiteralControl HtmlLiteral;

        /// <summary>
        /// The strings compare button.
        /// </summary>
        private const string StringsCompareButton = "MODULESETTINGS_HTMLDOCUMENT_SHOW_COMPARE_BUTTON";

        /// <summary>
        /// The btn compare.
        /// </summary>
        private ModuleButton btnCompare;

        #endregion

        // end of addition
        // /// <summary>
        // /// PlaceHolder for the HTML
        // /// </summary>
        // protected PlaceHolder HtmlHolder;
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "HtmlModule" /> class. 
        ///   Set the Module Settings
        ///   <list type = "">
        ///     <item>
        ///       ShowMobile
        ///     </item>
        ///   </list>
        /// </summary>
        /// <remarks>
        /// </remarks>
        public HtmlModule()
        {
            var group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            var groupOrderBase = (int)SettingItemGroup.MODULE_SPECIAL_SETTINGS;

            HtmlEditorDataType.HtmlEditorSettings(this.BaseSettings, group);

            // If false the input box for mobile content will be hidden
            var showMobileText = new SettingItem<bool, CheckBox>
            {
                Value = true,
                Order = groupOrderBase + 10,
                Group = group
            };
            this.BaseSettings.Add("ShowMobile", showMobileText);

            #region Button Display Settings for this module

            // added by Hongwei Shen(Hongwei.shen@gmail.com) 10/9/2005
            group = SettingItemGroup.BUTTON_DISPLAY_SETTINGS;
            groupOrderBase = (int)SettingItemGroup.BUTTON_DISPLAY_SETTINGS;

            // If false the compare button will be hidden
            var showCompareButton = new SettingItem<bool, CheckBox>
            {
                Value = true,
                Order = groupOrderBase + 60,
                Group = group,
                EnglishName = "Show Compare Button?",
                Description = "Compare the working version with the live one"
            };
            this.BaseSettings.Add(StringsCompareButton, showCompareButton);

            // end of addition
            #endregion

            this.SupportsWorkflow = true;

            // No need for view state on view. - jminond
            // Need view state to toggle the compare button Hongwei Shen 10/9/2005
            this.EnableViewState = true; // false;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the button toggles between displaying the staging content
        ///   and displaying the comparison of the staging with the
        ///   production version with the differences highlighted.
        /// </summary>
        /// <value>The compare button.</value>
        public ModuleButton CompareButton
        {
            get
            {
                if (this.btnCompare == null && HttpContext.Current != null)
                {
                    if (this.CanCompare)
                    {
                        // create the button
                        this.btnCompare = new ModuleButton { Group = ModuleButton.ButtonGroup.Admin };
                        this.btnCompare.ServerClick += this.CompareButtonClick;
                        this.btnCompare.RenderAs = this.ButtonsRenderAs;
                    }
                }

                if (this.btnCompare != null)
                {
                    if (this.IsComparing == 1)
                    {
                        // if it is in comparing status, clicking the button
                        // will bring the content back to staging
                        this.btnCompare.TranslationKey = "BackToStaging";
                        this.btnCompare.EnglishName = "Back to staging";
                        this.btnCompare.Image = this.CurrentTheme.GetImage("Buttons_Stage", "stage.gif");
                    }
                    else
                    {
                        // otherwise, clicking will do comparison
                        this.btnCompare.TranslationKey = "Compare";
                        this.btnCompare.EnglishName = "Compare staging with production";
                        this.btnCompare.Image = this.CurrentTheme.GetImage("Buttons_Compare", "Compare.gif");
                    }
                }

                return this.btnCompare;
            }
        }

        /// <summary>
        ///   General Module Def GUID
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{0B113F51-FEA3-499A-98E7-7B83C192FDBB}");
            }
        }

        /// <summary>
        ///   Searchable module
        /// </summary>
        /// <value></value>
        public override bool Searchable
        {
            get
            {
                return true;
            }
        }

        // end of addition

        /// <summary>
        ///   Gets a value indicating whether setting allows version Compare button
        /// </summary>
        /// <value><c>true</c> if [support compare]; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   Added by Hongwei. Shen(hongwei.shen@gmail.com) to support
        ///   version comparison. 10/9/2005
        /// </remarks>
        public bool SupportCompare
        {
            get
            {
                object o = this.Settings[StringsCompareButton];
                return o != null && bool.Parse(o.ToString());
            }
        }

       

        public bool IsCKEditorCurrentEditor
        {
            get
            {
                return this.Settings["Editor"].Value.ToString().ToLower() == "ckeditor";
            }
        }

        /// <summary>
        ///   Gets a value indicating whether permission for Compare and BackToStage buttons
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can compare; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool CanCompare
        {
            get
            {
                if (this.ModuleConfiguration == null || !this.SupportCompare)
                {
                    return false;
                }

                return this.SupportsWorkflow &&
                       (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedApproveRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteRoles)) &&
                       this.Version == WorkFlowVersion.Staging;
            }
        }

        /// <summary>
        /// Gets or sets the status of the Compare Button to allow
        /// toggling between displaying comparison and staging
        /// content.
        /// </summary>
        /// <value>The is comparing.</value>
        /// <remarks></remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual int IsComparing
        {
            get
            {
                var o = this.ViewState["NewHtmlModuleIsComparing"];

                // if o == null - the first time is special because the compare
                // button click event handler is not called, thus,
                // we need to toggle IsCompare manually
                return o == null ? -1 : (int)o;
            }

            set
            {
                this.ViewState["NewHtmlModuleIsComparing"] = value;
            }
        }

        #endregion

        // end of addition
        #region Public Methods

        /// <summary>
        /// Installs the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Install(IDictionary stateSaver)
        {
            var currentScriptName = System.IO.Path.Combine(
                this.Server.MapPath(this.TemplateSourceDirectory), "install.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        /// <summary>
        /// Searchable module implementation
        /// </summary>
        /// <param name="portalId">
        /// The portal ID
        /// </param>
        /// <param name="userId">
        /// ID of the user is searching
        /// </param>
        /// <param name="searchString">
        /// The text to search
        /// </param>
        /// <param name="searchField">
        /// The fields where performing the search
        /// </param>
        /// <returns>
        /// The SELECT sql to perform a search on the current module
        /// </returns>
        public override string SearchSqlSelect(int portalId, int userId, string searchString, string searchField)
        {
            var s = new SearchDefinition("rb_HtmlText", "DesktopHtml", "DesktopHtml", searchField);
            return s.SearchSqlSelect(portalId, userId, searchString, false);
        }

        /// <summary>
        /// Uninstalls the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Uninstall(IDictionary stateSaver)
        {
            var currentScriptName = System.IO.Path.Combine(
                this.Server.MapPath(this.TemplateSourceDirectory), "uninstall.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override to add the Compare button to the button list
        /// </summary>
        /// <remarks>
        /// Added by Hongwei Shen(Hongwei.shen@gmail.com) 10/9/2005
        ///   for supporting version comparison
        /// </remarks>
        protected override void BuildButtonLists()
        {
            // add Compare button
            if (this.CompareButton != null)
            {
                this.ButtonListAdmin.Add(this.btnCompare);
                if (this.IsComparing == -1)
                {
                    // it is the time to toggle the buttons
                    // manually
                    this.IsComparing = 1;
                }
            }

            base.BuildButtonLists();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// The Page_Load event handler on this User Control is
        ///   used to render a block of HTML or text to the page.
        ///   The text/HTML to render is stored in the HtmlText
        ///   database table.  This method uses the Appleseed.HtmlTextDB()
        ///   data component to encapsulate all data functionality.
        /// </remarks>
        [History("Mark Gregory", "mgregory@gt.com.au", "added use of HtmlLiteral_DataBinding")]
        [History("William Forney", "bill@improvdesign.com",
            "Moved data reader code to the DB class where it belongs & commented it out.")]
        protected override void OnInit(EventArgs e)
        {

            if (HasEditPermission())
                this.HtmlHolder.EnableViewState = false;
            else
                this.HtmlHolder2.EnableViewState = false;


            // Add title
            // ModuleTitle = new DesktopModuleTitle();
            this.EditUrl = "~/DesktopModules/CoreModules/HTMLDocument/HtmlEdit.aspx";

            // Controls.AddAt(0, ModuleTitle);
            var text = new HtmlTextDB();
            this.Content = this.Server.HtmlDecode(text.GetHtmlTextString(this.ModuleID, this.Version));
            if (PortalSecurity.HasEditPermissions(this.ModuleID) && string.IsNullOrEmpty(this.Content.ToString()))
            {
                this.Content = "Add content here ...<br/><br/><br/><br/>";

            }

            //this.Settings["Editor"].Value

            this.HtmlLiteral = new LiteralControl(this.Content.ToString());
            this.HtmlLiteral.DataBinding += HtmlLiteralDataBinding;
            this.HtmlLiteral.DataBind();
            if (HasEditPermission())
            {
                
                this.HtmlHolder.Controls.Add(this.HtmlLiteral);
            }
            else
                this.HtmlHolder2.Controls.Add(this.HtmlLiteral);

            base.OnInit(e);
        }

        /// <summary>
        /// On load event used to load aloha js and css
        /// </summary>
        /// <param name="e"> The <see cref="System.EventsArgs"/> instance containing event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Settings["HTMLWrapperClass"].ToString()))
            {
                this.HTMLContainer.Attributes.Add("class", this.Settings["HTMLWrapperClass"].ToString());
            }
            else if (!string.IsNullOrEmpty(this.PortalSettings.CustomSettings["HTML_WRAPPER_CLASS"].ToString()))
            {
                this.HTMLContainer.Attributes.Add("class", this.PortalSettings.CustomSettings["HTML_WRAPPER_CLASS"].ToString());
            }

            if (this.PortalSettings.EnabledCKEditorInlineEditing && IsCKEditorCurrentEditor)
            {
                this.ckEditor.ID = "ckEditor_" + this.ModuleID.ToString();
                this.ckEditor.Attributes.Add("contenteditable", "true");
                this.ckEditor.Attributes.Add("pageid", this.PageID.ToString());
                this.ckEditor.ClientIDMode = ClientIDMode.Static;
                this.plCkEditorScript.Visible = this.IsCKEditorCurrentEditor;
                this.plcCkEditorJS.Visible = this.IsCKEditorCurrentEditor;
            }
            else
            {
                this.ckEditor.Attributes.Remove("contenteditable");
            }

           
            base.OnLoad(e);
        }

        /// <summary>
        /// Handle the request for comparison
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void CompareButtonClick(object sender, EventArgs e)
        {
            var text = new HtmlTextDB();
            if (HasEditPermission())
                this.HtmlHolder.Controls.Clear();
            else
                this.HtmlHolder2.Controls.Clear();

            if (this.IsComparing == 0)
            {
                this.Content = this.Server.HtmlDecode(text.GetHtmlTextString(this.ModuleID, this.Version));
                this.HtmlLiteral = new LiteralControl(this.Content.ToString());
                this.HtmlLiteral.DataBinding += HtmlLiteralDataBinding;
                this.HtmlLiteral.DataBind();
                if (HasEditPermission())
                    this.HtmlHolder.Controls.Add(this.HtmlLiteral);
                else
                    this.HtmlHolder2.Controls.Add(this.HtmlLiteral);

                this.IsComparing = 1;
            }
            else
            {
                var prod = this.Server.HtmlDecode(text.GetHtmlTextString(this.ModuleID, WorkFlowVersion.Production));
                var stag = this.Server.HtmlDecode(text.GetHtmlTextString(this.ModuleID, WorkFlowVersion.Staging));
                var merger = new Merger(prod, stag);
                this.Content = this.Server.HtmlDecode(merger.merge());
                this.HtmlLiteral = new LiteralControl(this.Content.ToString());
                this.HtmlLiteral.DataBinding += HtmlLiteralDataBinding;
                this.HtmlLiteral.DataBind();
                if (HasEditPermission())
                    this.HtmlHolder.Controls.Add(this.HtmlLiteral);
                else
                    this.HtmlHolder2.Controls.Add(this.HtmlLiteral);

                this.IsComparing = 0;
            }
        }

        /// <summary>
        /// Handles the DataBinding event of the HtmlLiteral control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private static void HtmlLiteralDataBinding(object sender, EventArgs e)
        {
            ((LiteralControl)sender).Text = ((LiteralControl)sender).Text.Replace("~/", Path.ApplicationRoot + "/");
        }

        #endregion

        #region IModuleExportable Members

        public string GetContentData(int moduleId)
        {
            IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
            HtmlTextDTO _html = services.GetHtmlTextDTO(moduleId);
            if (_html == null)
            {
                return string.Empty;
            }
            else
            {
                System.IO.StringWriter xout = new System.IO.StringWriter();
                XmlSerializer xs = new XmlSerializer(typeof(HtmlTextDTO));
                xs.Serialize(xout, _html);
                return xout.ToString();
            }
        }

        public bool SetContentData(int moduleId, string content)
        {
            if (content == null || content.Equals(string.Empty))
            {
                //si el contenido es nullo es porque no existe ningun registro en htmltext para el modulo
                return true;
            }
            else
            {
                IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
                HtmlTextDTO _html = new HtmlTextDTO();
                System.IO.StringReader xin = new System.IO.StringReader(content);
                XmlSerializer xs = new XmlSerializer(typeof(HtmlTextDTO));
                _html = (HtmlTextDTO)xs.Deserialize(xin);

                return services.SaveHtmlText(moduleId, _html);
            }
        }

        public bool HasEditPermission()
        {
            return Appleseed.Framework.Security.PortalSecurity.HasEditPermissions(this.ModuleID);
        }

        #endregion



    }
}