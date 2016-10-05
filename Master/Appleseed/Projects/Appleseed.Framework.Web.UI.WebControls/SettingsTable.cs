// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsTable.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that defines data for the event
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Settings;

    using Page = Appleseed.Framework.Web.UI.Page;
    using System.Text;
    using System.Collections.Generic;

    #region Event argument class

    /// <summary>
    /// Class that defines data for the event
    /// </summary>
    public class SettingsTableEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsTableEventArgs"/> class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public SettingsTableEventArgs(object item)
        {
            this.CurrentItem = item;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     CurrentItem
        /// </summary>
        /// <value>The current item.</value>
        public object CurrentItem { get; set; }

        #endregion
    }

    #endregion

    #region Delegate

    /// <summary>
    /// UpdateControlEventHandler delegate
    /// </summary>
    public delegate void UpdateControlEventHandler(object sender, SettingsTableEventArgs e);

    #endregion

    #region SettingsTable control

    /// <summary>
    /// A data bound control that takes in custom settings list in a SortedList
    ///    object and creates the hierarchy of the settings controls in two different
    ///    ways. One shows the grouped settings flat and the other shows the grouped 
    ///    settings in selectable tabs.  
    ///    Notes and Credits:
    ///    Motive: 
    ///    In the property page of Appleseed modules, there are groups of settings.
    ///    Some people like the old way of look and feel of the settings (before 
    ///    svn version 313, some like the new way of grouping the settings into 
    ///    tabs. This modification handles over the power to make choice to the end 
    ///    user by providing an attribute "UseGrouingTabs" which in turn will get 
    ///    value from Appleseed.Framework.Settings (an entry is added over there) that is set 
    ///    by user.
    /// 
    ///    What is changed: 
    ///    Many changes in order to implement the functionality and make the control
    ///    an nice data bound control. However, the child control creating logic is
    ///    NOT changed. Basically, these logic was in the DataBind() function of the 
    ///    previous implementation. Event processing logic is NOT changed.
    /// 
    ///    Credits:
    ///    Most credit should go to the developers who created and modifed the previous 
    ///    class because they created the logic for doing business. I keep their names 
    ///    and comments in the new code and I also keep a copy of the whole old code in
    ///    the region "Previous SettingsTable control" to honor their contributions.
    ///    
    ///    Special Credit:
    ///    This modification is done per Manu's request and he also gave many good 
    ///    suggestions.
    /// 
    ///    Hongwei Shen (hongwei.shen@gmail.com) Oct. 15, 2005
    /// </summary>
    [ToolboxData("<{0}:SettingsTable runat=server></{0}:SettingsTable>")]
    public class SettingsTable : WebControl, INamingContainer
    {
        #region Constants and Fields

        /// <summary>
        ///     Used to store reference to base object it, 
        ///     can be ModuleID or Portal ID
        /// </summary>
        public int ObjectID = -1;

        /// <summary>
        /// The edit controls.
        /// </summary>
        private Hashtable editControls;

        /// <summary>
        /// The grouping tabs created.
        /// </summary>
        private bool groupingTabsCreated;

        /// <summary>
        /// The settings.
        /// </summary>
        private SortedList settings;

        /// <summary>
        /// The use grouping tabs.
        /// </summary>
        private bool useGroupingTabs;

        #endregion

        #region Events

        /// <summary>
        ///     The UpdateControl event is defined using the event keyword.
        ///     The type of UpdateControl is UpdateControlEventHandler.
        /// </summary>
        public event UpdateControlEventHandler UpdateControl;

        #endregion

        #region Properties

        /// <summary>
        ///     DataSource, it is limited to SortedList type
        /// </summary>
        /// <value>The data source.</value>
        public object DataSource
        {
            get
            {
                return this.settings;
            }

            set
            {
                if (value == null || value is SortedList)
                {
                    this.settings = (SortedList)value;
                }
                else
                {
                    throw new ArgumentException("DataSource must be SortedList type", "value");
                }
            }
        }

        /// <summary>
        ///     Gets or sets the height of the Web server control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref = "T:System.Web.UI.WebControls.Unit"></see> that represents the height of the control. The default is <see cref = "F:System.Web.UI.WebControls.Unit.Empty"></see>.</returns>
        /// <exception cref = "T:System.ArgumentException">The height was set to a negative value.</exception>
        public override Unit Height
        {
            get
            {
                // return base.Height;
                return Config.SettingsGroupingHeight;
            }

            set
            {
                base.Height = value;
            }
        }

        /// <summary>
        ///     If set to true, create the control hirarchy grouping property
        ///     settings into selected tabs. Otherwise, create the control
        ///     hirarchy as flat fieldsets. Default is false.
        /// </summary>
        /// <value><c>true</c> if [use grouping tabs]; otherwise, <c>false</c>.</value>
        public bool UseGroupingTabs
        {
            get
            {
                // return _useGroupingTabs;
                return Config.UseSettingsGroupingTabs;
            }

            set
            {
                this.useGroupingTabs = value;
            }
        }

        /// <summary>
        ///     Gets or sets the width of the Web server control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref = "T:System.Web.UI.WebControls.Unit"></see> that represents the width of the control. The default is <see cref = "F:System.Web.UI.WebControls.Unit.Empty"></see>.</returns>
        /// <exception cref = "T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
        public override Unit Width
        {
            get
            {
                // return base.Width;
                return Config.SettingsGroupingWidth;
            }

            set
            {
                base.Width = value;
            }
        }

        /// <summary>
        ///     Gets a Settings control collection, it is initialized only
        ///     when referenced.
        /// </summary>
        /// <value>The edit controls.</value>
        protected virtual Hashtable EditControls
        {
            get
            {
                return this.editControls ?? (this.editControls = new Hashtable());
            }
        }

        /// <summary>
        ///     Gets the <see cref = "T:System.Web.UI.HtmlTextWriterTag"></see> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref = "T:System.Web.UI.HtmlTextWriterTag"></see> enumeration values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                // render the out tag as div
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            // raise databinding event in case there are binding scripts
            this.OnDataBinding(EventArgs.Empty);

            // clear existing control hierarchy
            this.Controls.Clear();
            this.ClearChildViewState();

            // start tracking changes during databinding
            this.TrackViewState();

            // create control hierarchy from the data source
            this.CreateControlHierarchy(true);
            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// This method provide a way to trigger UpdateControl event
        ///     for the child controls of this control from outside.
        /// </summary>
        public void UpdateControls()
        {
            foreach (string key in this.EditControls.Keys)
            {
                var c = (Control)this.EditControls[key];
                //var currentItem = (ISettingItem)this.settings[c.ID];                
                var currentItem = this.settings[key];
                ((ISettingItem)currentItem).EditControl = c;
                this.OnUpdateControl(new SettingsTableEventArgs(currentItem));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();

            // recover control hierarchy from viewstate is not implemented 
            // at this time. If somebody wants to do it, turn the following 
            // line on and do the logic in "createGroupFlat" and 
            // "createGroupingTabs"

            // CreateControlHierarchy(false);
        }

        /// <summary>
        /// Creating control hierarchy of this control. Depending on the
        ///     value of UseGroupingTabs, two exclusively different hierarchies
        ///     may be created. One uses grouping tabs and another uses flat
        ///     fieldsets.
        /// </summary>
        /// <param name="useDataSource">
        /// If true, create controlhierarchy from data source, otherwise
        ///     create control hierachy from view state
        /// </param>
        protected virtual void CreateControlHierarchy(bool useDataSource)
        {
            // re-order settings items, the re-ordered items
            // is put in SettingsOrder
            var orderedSettings = this.ProcessDataSource();

            if (this.UseGroupingTabs)
            {
                this.CreateGroupingTabs(useDataSource, orderedSettings);
            }
            else
            {
                this.CreateGroupFlat(useDataSource, orderedSettings);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> object that contains the event data.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // the scripts needed for using grouping tabs
            if (this.groupingTabsCreated)
            {
                /*   // Jonathan - tabsupport
                   if (((Page)this.Page).IsClientScriptRegistered("x_core") == false)
                   {
                       ((Page)this.Page).RegisterClientScript(
                           "x_core", Path.WebPathCombine(Path.ApplicationRoot, "/aspnet_client/x/x_core.js"));
                   }

                   if (((Page)this.Page).IsClientScriptRegistered("x_event") == false)
                   {
                       ((Page)this.Page).RegisterClientScript(
                           "x_event", Path.WebPathCombine(Path.ApplicationRoot, "/aspnet_client/x/x_event.js"));
                   }

                   if (((Page)this.Page).IsClientScriptRegistered("x_dom") == false)
                   {
                       ((Page)this.Page).RegisterClientScript(
                           "x_dom", Path.WebPathCombine(Path.ApplicationRoot, "/aspnet_client/x/x_dom.js"));
                   }

                   if (((Page)this.Page).IsClientScriptRegistered("tabs_js") == false)
                   {
                       ((Page)this.Page).RegisterClientScript(
                           "tabs_js", Path.WebPathCombine(Path.ApplicationRoot, "/aspnet_client/x/x_tpg.js"));
                   }

                   // End tab support

                   // this piece of script was previously in PropertyPage.aspx, but it should be
                   // part of the SettingsTable control when using tabs. So, I moved it to here.
                   // It is startup script
                   if (!this.Page.ClientScript.IsStartupScriptRegistered("tab_startup_js"))
                   {
                       var script = string.Format("<script language=\"javascript\" type=\"text/javascript\"> var tabW = {0};  var tabH = {1};  var tpg1 = new xTabPanelGroup('tpg1', tabW, tabH, 50, 'tabPanel', 'tabGroup', 'tabDefault', 'tabSelected'); </script>", this.Width.Value, this.Height.Value);

                       this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "tab_startup_js", script);
                   }*/
            }
        }

        /// <summary>
        /// Raises UpdateControl Event
        /// </summary>
        /// <param name="e">
        /// The <see cref="Appleseed.Framework.Web.UI.WebControls.SettingsTableEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnUpdateControl(SettingsTableEventArgs e)
        {
            if (this.UpdateControl != null)
            {
                // Invokes the delegates.
                this.UpdateControl(this, e);
            }
        }

        /// <summary>
        /// Re-order the settings items. The reason why this processing is
        ///     necessary is that two settings items may have same order.
        /// </summary>
        /// <returns>
        /// A sorted list.
        /// </returns>
        protected virtual SortedList ProcessDataSource()
        {
            // Jes1111 -- force the list to obey SettingItem.Order property and divide it into groups
            // Manu -- a better order system avoiding try and catch.
            // Now settings with no order have a progressive order number 
            // based on their position on list
            var settingsOrder = new SortedList();

            foreach (var key in this.settings.GetKeyList().Cast<string>().Where(key => this.settings[key] != null))
            {
                //if (!(this.settings[key] is SettingItem<string, TextBox>))
                //{
                //    // TODO: FIX THIS
                //    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Debug, "Unexpected '" + Settings[key].GetType().FullName + "' in settings table.");
                //}
                //else
                //{
                var order = ((ISettingItem)this.settings[key]).Order;

                while (settingsOrder.ContainsKey(order))
                {
                    // be sure do not have duplicate order key or 
                    // we get an error
                    order++;
                }

                settingsOrder.Add(order, key);
                //}
            }

            return settingsOrder;
        }

        /// <summary>
        /// Returns a new field set with legend for a new settings group
        /// </summary>
        /// <param name="currentItem">
        /// The settings item
        /// </param>
        /// <returns>
        /// Fieldset control
        /// </returns>
        private static HtmlGenericControl CreateNewFieldSet(ISettingItem currentItem)
        {
            // start a new fieldset
            var fieldset = new HtmlGenericControl("fieldset");
            fieldset.Attributes.Add(
                "class", string.Concat("SettingsTableGroup ", currentItem.Group.ToString().ToLower()));

            // create group legend
            var legend = new HtmlGenericControl("legend");
            legend.Attributes.Add("class", "SubSubHead");
            var legendText = new Localize
            {
                TextKey = currentItem.Group.ToString(),
                Text = currentItem.GroupDescription
            };
            legend.Controls.Add(legendText);
            fieldset.Controls.Add(legend);

            return fieldset;
        }

        /// <summary>
        /// Create the flat settings groups control hirarchy
        /// </summary>
        /// <param name="useDataSource">
        /// if set to <c>true</c> [use data source].
        /// </param>
        /// <param name="settingsOrder">
        /// The settings order.
        /// </param>
        private void CreateGroupFlat(bool useDataSource, SortedList settingsOrder)
        {
            if (!useDataSource)
            {
                // recover control hierarchy from view state is not implemented
                return;
            }

            var fieldset = new HtmlGenericControl("dummy");

            var tbl = new Table();

            // Initialize controls
            var currentGroup = SettingItemGroup.NONE;

            foreach (string currentSetting in settingsOrder.GetValueList())
            {
                var currentItem = (ISettingItem)this.settings[currentSetting];

                if (currentItem.Group != currentGroup)
                {
                    if (fieldset.Attributes.Count > 0)
                    {
                        // add built fieldset
                        fieldset.Controls.Add(tbl);
                        this.Controls.Add(fieldset);
                    }

                    // start a new fieldset
                    fieldset = CreateNewFieldSet(currentItem);

                    // start a new table
                    tbl = new Table();
                    tbl.Attributes.Add("class", "SettingsTableGroup");
                    tbl.Attributes.Add("width", "100%");
                    currentGroup = currentItem.Group;
                }

                tbl.Rows.Add(this.CreateOneSettingRow(currentSetting, currentItem));
            }

            fieldset.Controls.Add(tbl);
            this.Controls.Add(fieldset);
        }

        /// <summary>
        /// Create the grouping tabs control hirarchy
        /// </summary>
        /// <param name="useDataSource">
        /// if set to <c>true</c> [use data source].
        /// </param>
        /// <param name="settingsOrder">
        /// The settings order.
        /// </param>
        private void CreateGroupingTabs(bool useDataSource, SortedList settingsOrder)
        {
            if (!useDataSource)
            {
                // recover control hierarchy from view state is not implemented
                return;
            }

            var tabPanelGroup = new HtmlGenericControl("div");
            tabPanelGroup.Attributes.Add("id", "tpg1");
            tabPanelGroup.Attributes.Add("class", "tabPanelGroup");

            var tabGroup = new HtmlGenericControl("ul");
            tabGroup.Attributes.Add("class", "tabGroup");
            tabPanelGroup.Controls.Add(tabGroup);

            var tabPanel = new HtmlGenericControl("div");
            tabPanel.Attributes.Add("class", "tabPanel");

            var tabDefault = new HtmlGenericControl("li");
            tabDefault.Attributes.Add("class", "tabDefault");

            var aDefault = new HtmlGenericControl("a");
            aDefault.Attributes.Add("href", "#id");
            tabDefault.Controls.Add(aDefault);

            var fieldset = new HtmlGenericControl("dummy");

            var tbl = new Table();

            Dictionary<string, string> dicc = new Dictionary<string, string>();

            // Initialize controls
            var currentGroup = SettingItemGroup.NONE;

            foreach (string currentSetting in settingsOrder.GetValueList())
            {
                var currentItem = (ISettingItem)this.settings[currentSetting];

                if (aDefault.InnerText.Length == 0)
                {
                    tabDefault = new HtmlGenericControl("li");
                    tabDefault.Attributes.Add("class", "tabDefault");

                    // App_GlobalResources
                    //tabDefault.InnerText = General.GetString(currentItem.Group.ToString());
                    aDefault = new HtmlGenericControl("a");
                    aDefault.Attributes.Add("href", "#" + currentItem.Group.ToString());
                    aDefault.InnerText = General.GetString(currentItem.Group.ToString());
                    tabDefault.Controls.Add(aDefault);
                }

                if (currentItem.Group != currentGroup)
                {
                    if (fieldset.Attributes.Count > 0)
                    {
                        if (tabDefault.Controls.Count == 1 && HasTabAcess(currentGroup))
                        {
                            // add built fieldset
                            fieldset.Controls.Add(tbl);
                            tabPanel.Controls.Add(fieldset);
                            tabPanelGroup.Controls.Add(tabPanel);

                            var TabName = string.Empty;
                            foreach (var t in tabDefault.Controls)
                            {
                                TabName = ((HtmlGenericControl)t).InnerText;
                            }
                            if (!dicc.ContainsKey(TabName))
                            {
                                dicc.Add(TabName, TabName);
                                tabGroup.Controls.Add(tabDefault);
                            }
                            else {
                                tabGroup.Controls.Add(tabDefault);
                            }
                        }
                    }
                    // start a new fieldset
                    fieldset = CreateNewFieldSet(currentItem);

                    tabPanel = new HtmlGenericControl("div");
                    tabPanel.Attributes.Add("class", "tabPanel");
                    tabPanel.Attributes.Add("id", currentItem.Group.ToString());

                    tabDefault = new HtmlGenericControl("li");
                    tabDefault.Attributes.Add("class", "tabDefault");

                    aDefault = new HtmlGenericControl("a");
                    aDefault.Attributes.Add("href", "#" + currentItem.Group.ToString());
                    aDefault.InnerText = General.GetString(currentItem.Group.ToString());
                    tabDefault.Controls.Add(aDefault);

                    // start a new table
                    tbl = new Table();
                    tbl.Attributes.Add("class", "SettingsTableGroup");
                    tbl.Attributes.Add("width", "100%");
                    currentGroup = currentItem.Group;
                }

                tbl.Rows.Add(this.CreateOneSettingRow(currentSetting, currentItem));
            }

            if (tabDefault.Controls.Count == 1)
            {
                var TabName = string.Empty;
                foreach (var t in tabDefault.Controls)
                {
                    TabName = ((HtmlGenericControl)t).InnerText;
                }
                if (!dicc.ContainsKey(TabName))
                {
                    dicc.Add(TabName, TabName);
                    tabGroup.Controls.Add(tabDefault);
                }
            }
            else {
                tabGroup.Controls.Add(tabDefault);
            }

            fieldset.Controls.Add(tbl);

            tabPanel.Controls.Add(fieldset);
            tabPanelGroup.Controls.Add(tabPanel);

            this.Controls.AddAt(0, tabPanelGroup);

            this.groupingTabsCreated = true;
        }

        /// <summary>
        /// Returns one settings row that contains a cell for help, a cell for setting item
        ///     name and a cell for setting item and validators.
        /// </summary>
        /// <param name="currentSetting">
        /// The current setting.
        /// </param>
        /// <param name="currentItem">
        /// The current item.
        /// </param>
        /// <returns>
        /// A table row.
        /// </returns>
        private TableRow CreateOneSettingRow(string currentSetting, ISettingItem currentItem)
        {
            // the table row is going to have three cells 
            var row = new TableRow();

            // cell for help icon and description
            var helpCell = new TableCell();
            Image img;

            if (currentItem.Description.Length > 0)
            {
                var myimg = ((Page)this.Page).CurrentTheme.GetImage("Buttons_Help", "Help.gif");
                img = new Image
                {
                    ImageUrl = myimg.ImageUrl,
                    Height = myimg.Height,
                    Width = myimg.Width,
                    AlternateText = currentItem.Description
                };

                // Jminond: added netscape tooltip support
                img.Attributes.Add("title", General.GetString(currentSetting + "_DESCRIPTION"));
                img.ToolTip = General.GetString(currentSetting + "_DESCRIPTION"); // Fixed key for simplicity
            }
            else
            {
                // Jes1111 - 17/12/2004
                img = new Image
                {
                    Width = Unit.Pixel(25),
                    ImageUrl = ((Page)this.Page).CurrentTheme.GetImage("Spacer", "Spacer.gif").ImageUrl
                };
            }

            helpCell.Controls.Add(img);

            // add help cell to the row
            row.Cells.Add(helpCell);

            // Setting Name cell
            var nameCell = new TableCell();
            nameCell.Attributes.Add("width", "20%");
            nameCell.CssClass = "SubHead";

            nameCell.Text = currentItem.EnglishName.Length == 0
                                ? General.GetString(currentSetting, currentSetting + "<br />Key Not In Resources")
                                : General.GetString(currentItem.EnglishName, currentItem.EnglishName);

            // add name cell to the row
            row.Cells.Add(nameCell);

            // Setting Control cell
            var settingCell = new TableCell();
            settingCell.Attributes.Add("width", "80%");
            settingCell.CssClass = "st-control";

            StringBuilder script = new StringBuilder();
            script.Append("<script language=\"javascript\" type=\"text/javascript\">");
            //script.AppendFormat("$.extend($.ui.multiselect, { locale: { addAll: '{0}', removeAll: '{1}', itemsCount: '{2}' }});", "Agregar todo", "Remover todooooo", "items seleccionados");
            script.Append("$.extend($.ui.multiselect, { locale: { addAll: '");
            script.AppendFormat("{0}", General.GetString("ADD_ALL", "Add all", null));
            script.Append("',removeAll: '");
            script.AppendFormat("{0}", General.GetString("REMOVE_ALL", "Remove all", null));
            script.Append("',itemsCount: '");
            script.AppendFormat("{0}", General.GetString("ITEMS_SELECTED", "items selected", null));
            script.Append("' }});$(function(){ $.localise('ui-multiselect', {path: 'aspnet_client/jQuery/'});");
            script.Append("$(\".multiselect\").multiselect({sortable: false, searchable: false});}); </script>");

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "jqueryMultiselect", script.ToString());


            Control editControl;
            try
            {
                editControl = currentItem.EditControl;
                editControl.ID = currentSetting; // Jes1111
                editControl.EnableViewState = true;
            }
            catch (Exception)
            {
                editControl = new LiteralControl("There was an error loading this control");

                // LogHelper.Logger.Log(Appleseed.Framework.LogLevel.Warn, "There was an error loading '" + currentItem.EnglishName + "'", ex);
            }

            settingCell.Controls.Add(editControl);

            // TODO: WHAT IS THIS?
            // nameText.LabelForControl = editControl.ClientID;

            // Add control to edit controls collection
            this.EditControls.Add(currentSetting, editControl);

            // Validators
            settingCell.Controls.Add(new LiteralControl("<br />"));

            // Required
            // TODO : Whhn we bring back ELB easy list box, we need to put this back
            /*
            if (currentItem.Required && !(editControl is ELB.EasyListBox))
            {
                RequiredFieldValidator req = new RequiredFieldValidator();
                req.ErrorMessage =General.GetString("SETTING_REQUIRED", "%1% is required!", req).Replace("%1%", currentSetting);
                req.ControlToValidate = currentSetting;
                req.CssClass = "Error";
                req.Display = ValidatorDisplay.Dynamic;
                req.EnableClientScript = true;
                settingCell.Controls.Add(req);
            }
            */

            // Range Validator
            if (currentItem.MinValue != 0 || currentItem.MaxValue != 0)
            {
                var rang = new RangeValidator();

                switch (currentItem.Value.GetType().Name.ToLowerInvariant())
                {
                    case "string":
                        rang.Type = ValidationDataType.String;
                        break;

                    case "int":
                    case "int16":
                    case "int32":
                    case "int64":
                        rang.Type = ValidationDataType.Integer;
                        break;

                    // case PropertiesDataType.Currency:
                    //     rang.Type = ValidationDataType.Currency;
                    //     break;
                    case "datetime":
                        rang.Type = ValidationDataType.Date;
                        break;

                    case "double":
                        rang.Type = ValidationDataType.Double;
                        break;
                }

                if (currentItem.MinValue >= 0 && currentItem.MaxValue >= currentItem.MinValue)
                {
                    rang.MinimumValue = currentItem.MinValue.ToString();

                    if (currentItem.MaxValue == 0)
                    {
                        rang.ErrorMessage =
                            General.GetString(
                                "SETTING_EQUAL_OR_GREATER", "%1% must be equal or greater than %2%!", rang).Replace(
                                    "%1%", General.GetString(currentSetting)).Replace("%2%", currentItem.MinValue.ToString());
                    }
                    else
                    {
                        rang.MaximumValue = currentItem.MaxValue.ToString();
                        rang.ErrorMessage =
                            General.GetString("SETTING_BETWEEN", "%1% must be between %2% and %3%!", rang).Replace(
                                "%1%", General.GetString(currentSetting)).Replace("%2%", currentItem.MinValue.ToString()).Replace(
                                    "%3%", currentItem.MaxValue.ToString());
                    }
                }

                rang.ControlToValidate = currentSetting;
                rang.CssClass = "Error";
                rang.Display = ValidatorDisplay.Dynamic;
                rang.EnableClientScript = true;
                settingCell.Controls.Add(rang);
            }

            // add setting cell into the row
            row.Cells.Add(settingCell);

            // all done send it back
            return row;
        }

        private bool HasTabAcess(SettingItemGroup currentItem)
        {
            if (Appleseed.Framework.Security.UserProfile.isCurrentUserAdmin)
            {
                return true;
            }

            switch (currentItem)
            {
                case SettingItemGroup.SECURITY_USER_SETTINGS:
                    return false;

                case SettingItemGroup.THEME_LAYOUT_SETTINGS:
                    return Appleseed.Framework.Security.UserProfile.CurrentUser.HasPermission(Appleseed.Framework.Security.AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION);
            }

            return true;
        }
        #endregion
    }

    #endregion
}