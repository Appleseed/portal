namespace DUEMETRI.UI.WebControls.HWMenu
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Image = System.Web.UI.WebControls.Image;

    /// <summary>
    /// HWMenu WebControl.
    /// </summary>
    /// [History("mario@hartmann.net", "2003/06/14", "use of named CSS styles added")]
    /// [History("bill@improvdesign.com", "2010/12/06", "Reformatted and fixed up comments")]
    [Designer("DUEMETRI.UI.WebControls.HWMenu.Design.MenuDesigner")]
    [ParseChildren(true)]
    public class Menu : WebControl
    {
        #region Constants and Fields

        /// <summary>
        ///     The arrows.
        /// </summary>
        private Image[] arrws;

        /// <summary>
        ///     The control hi style.
        /// </summary>
        private Style controlHiStyle;

        /// <summary>
        ///     The control hi sub style.
        /// </summary>
        private Style controlHiSubStyle;

        /// <summary>
        ///     The control item style.
        /// </summary>
        private Style controlItemStyle;

        /// <summary>
        ///     The control sub style.
        /// </summary>
        private Style controlSubStyle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Menu" /> class.
        /// </summary>
        public Menu()
        {
            this.TopPaddng = 2;
            this.TargetLoc = "MenuContainer";
            this.TakeOverBgColor = true;
            this.ShowArrow = true;
            this.SecLineFrame = string.Empty;
            this.MenuWrap = true;
            this.MenuVerticalCentered = VerticalAlign.Top;
            this.MenuTextCentered = HorizontalAlign.Left;
            this.MenuFramesVertical = true;
            this.MenuCentered = HorizontalAlign.Left;
            this.LeftPaddng = 2;
            this.KeepHilite = true;
            this.FirstLineFrame = string.Empty;
            this.DocTargetFrame = "_self";
            this.DissapearDelay = 1000;
            this.ChildVerticalOverlap = .1;
            this.ChildOverlap = .1;
            this.BorderBtwnElmnts = true;
            this.Horizontal = true;
            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets arrow image
        /// </summary>
        [Category("Appearance")]
        [Description("Arrow image")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Image ArrowImage
        {
            get
            {
                return this.arrws[0];
            }

            set
            {
                this.arrws[0] = value;
            }
        }

        /// <summary>
        ///     Gets or sets arrow image down
        /// </summary>
        [Category("Appearance")]
        [Description("Arrow image down")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Image ArrowImageDown
        {
            get
            {
                return this.arrws[1];
            }

            set
            {
                this.arrws[1] = value;
            }
        }

        /// <summary>
        ///     Gets or sets arrow image left
        /// </summary>
        [Category("Appearance")]
        [Description("Arrow image left")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Image ArrowImageLeft
        {
            get
            {
                return this.arrws[2];
            }

            set
            {
                this.arrws[2] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether there is a border between elements
        /// </summary>
        public bool BorderBtwnElmnts { get; set; }

        /// <summary>
        ///     Gets or sets horizontal overlap child/ parent
        /// </summary>
        public double ChildOverlap { get; set; }

        /// <summary>
        ///     Gets or sets vertical overlap child/ parent
        /// </summary>
        public double ChildVerticalOverlap { get; set; }

        /// <summary>
        ///     Gets menu items collection.
        /// </summary>
        [Category("Data")]
        [Description("Menu items.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MenuTreeNodes Childs { get; private set; }

        /// <summary>
        ///     Gets or sets client script path.
        /// </summary>
        [Category("Data")]
        [Description("Client script path.")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ClientScriptPath
        {
            get
            {
                var clientScriptPath = this.ViewState["ClientScriptPath"];
                if (clientScriptPath != null)
                {
                    return (string)clientScriptPath;
                }

                return this.GetClientScriptPath();
            }

            set
            {
                var clientScriptPath = value;
                if (clientScriptPath.Length > 0 && !clientScriptPath.EndsWith("/"))
                {
                    clientScriptPath = clientScriptPath + "/";
                }

                this.ViewState["ClientScriptPath"] = clientScriptPath;
            }
        }

        /// <summary>
        ///     Gets or sets style of this element when the mouse is over the element.
        /// </summary>
        [Category("Style")]
        [Description("Style of this element when the mouse is over the element.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Style ControlHiStyle
        {
            get
            {
                if (this.controlHiStyle == null)
                {
                    this.controlHiStyle = new Style();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.controlHiStyle).TrackViewState();
                    }
                }

                return this.controlHiStyle;
            }

            set
            {
                this.controlHiStyle = value;
            }
        }

        /// <summary>
        ///     Gets or sets style of the Subs element when the mouse is over the element.
        /// </summary>
        [Category("Style")]
        [Description("Style of the Subs element when the mouse is over the element.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Style ControlHiSubStyle
        {
            get
            {
                if (this.controlHiSubStyle == null)
                {
                    this.controlHiSubStyle = new Style();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.controlHiSubStyle).TrackViewState();
                    }
                }

                return this.controlHiSubStyle;
            }

            set
            {
                this.controlHiSubStyle = value;
            }
        }

        /// <summary>
        ///     Gets or sets style of this element when the mouse is not over the element.
        /// </summary>
        [Category("Style")]
        [Description("Style of this element when the mouse is over the element.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Style ControlItemStyle
        {
            get
            {
                if (this.controlItemStyle == null)
                {
                    this.controlItemStyle = new Style();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.controlItemStyle).TrackViewState();
                    }
                }

                return this.controlItemStyle;
            }

            set
            {
                this.controlItemStyle = value;
            }
        }

        /// <summary>
        ///     Gets or sets style of tSubs element when the mouse is not over the element.
        /// </summary>
        [Category("Style")]
        [Description("Style of tSubs element when the mouse is over the element.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Style ControlSubStyle
        {
            get
            {
                if (this.controlSubStyle == null)
                {
                    this.controlSubStyle = new Style();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.controlSubStyle).TrackViewState();
                    }
                }

                return this.controlSubStyle;
            }

            set
            {
                this.controlSubStyle = value;
            }
        }

        /// <summary>
        ///     Gets or sets delay before menu folds in (milliseconds)
        /// </summary>
        public int DissapearDelay { get; set; }

        /// <summary>
        ///     Gets or sets Frame where target documents appear
        /// </summary>
        public string DocTargetFrame { get; set; }

        /// <summary>
        ///     Gets or sets Frame where first level appears
        /// </summary>
        public string FirstLineFrame { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide first level when loading new document 1 or 0
        /// </summary>
        public bool HideTop { get; set; }

        /// <summary>
        ///     Gets or sets multiple frames x correction
        /// </summary>
        public int HorCorrect { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether horizontal or vertical menu
        /// </summary>
        [Category("Appearance")]
        [Description("Horizontal or vertical menu.")]
        public bool Horizontal { get; set; }

        /// <summary>
        ///     Gets or sets the client images path.
        /// </summary>
        [Category("Data")]
        [Description("Client images path.")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string ImagesPath
        {
            get
            {
                var imagesPath = this.ViewState["ImagesPath"];
                if (imagesPath != null)
                {
                    return (string)imagesPath;
                }

                return this.GetClientScriptPath();
            }

            set
            {
                var imagesPath = value;
                if (imagesPath.Length > 0 && !imagesPath.EndsWith("/"))
                {
                    imagesPath = imagesPath + "/";
                }

                this.ViewState["ImagesPath"] = imagesPath;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether Keep selected path highligthed
        /// </summary>
        public bool KeepHilite { get; set; }

        /// <summary>
        ///     Gets or sets Left padding
        /// </summary>
        public int LeftPaddng { get; set; }

        /// <summary>
        ///     Gets or sets Menu horizontal position 'left', 'center' or 'right'
        /// </summary>
        public HorizontalAlign MenuCentered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether Frames in cols or rows 1 or 0
        /// </summary>
        public bool MenuFramesVertical { get; set; }

        /// <summary>
        ///     Gets or sets Item text position 'left', 'center' or 'right'
        /// </summary>
        public HorizontalAlign MenuTextCentered { get; set; }

        /// <summary>
        ///     Gets or sets Menu vertical position 'top', 'middle','bottom' or 'static'
        /// </summary>
        public VerticalAlign MenuVerticalCentered { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether enables/ disables menu wrap 1 or 0
        /// </summary>
        public bool MenuWrap { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether enables/ disables right to left unfold 1 or 0
        /// </summary>
        public bool RightToLeft { get; set; }

        /// <summary>
        ///     Gets or sets Frame where sub levels appear
        /// </summary>
        public string SecLineFrame { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether Uses arrow gifs when 1
        /// </summary>
        public bool ShowArrow { get; set; }

        /// <summary>
        ///     Gets or sets Menu offset y coordinate
        /// </summary>
        public int StartLeft { get; set; }

        /// <summary>
        ///     Gets or sets Menu offset x coordinate
        /// </summary>
        public int StartTop { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether Menu frame takes over background color subitem frame
        /// </summary>
        public bool TakeOverBgColor { get; set; }

        /// <summary>
        ///     Gets or sets span id for relative positioning
        /// </summary>
        public string TargetLoc { get; set; }

        /// <summary>
        ///     Gets or sets Top padding
        /// </summary>
        public int TopPaddng { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether Level 1 unfolds onclick/ onmouseover
        /// </summary>
        public bool UnfoldsOnClick { get; set; }

        /// <summary>
        ///     Gets or sets Multiple frames y correction
        /// </summary>
        public int VerCorrect { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether menu tree checking on or off 1 or 0
        /// </summary>
        public bool WebMasterCheck { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            var mywidth = new Unit();
            var myheight = new Unit();

            if (this.Childs.Count > 0)
            {
                if (this.Horizontal)
                {
                    myheight = this.Height;
                    mywidth = this.Childs.Width;
                }
                else
                {
                    myheight = this.Childs.Height;
                    mywidth = this.Width;
                }
            }

            myheight = new Unit(myheight.Value + this.StartTop); // correction
            mywidth = new Unit(mywidth.Value + this.StartLeft); // correction

            // MenuContainer.MergeStyle(this.ControlStyle);
            var menuContainer =
                new LiteralControl(
                    string.Format(
                        @"<div id='MenuPos' style='padding:0;margin:0;border-width:0;position:relative;width:{0}; height:{1};'><img style='padding:0;margin:0;border-width:0' src='{2}1x1.gif' border='0' width='{3}' height='{4}'></div>", 
                        mywidth.Value, 
                        myheight.Value, 
                        this.ClientScriptPath, 
                        mywidth.Value, 
                        myheight.Value))
                    {
                        // ns4 bug fix
                        ID = this.TargetLoc
                    };

            this.Controls.Add(menuContainer);
        }

        /// <summary>
        /// GetClientScriptPath() method -- works out the
        ///     location of the shared scripts and images.
        /// </summary>
        /// <returns>
        /// The get client script path.
        /// </returns>
        protected virtual string GetClientScriptPath()
        {
            string location = null;

            IDictionary configData = null;

            if (this.Context != null)
            {
                configData = (IDictionary)this.Context.GetSection("system.web/webControls");
            }

            if (configData != null)
            {
                location = (string)configData["clientScriptsLocation"];
            }

            if (location == null)
            {
                return String.Empty;
            }

            if (location.IndexOf("{0}") >= 0)
            {
                var assemblyName = this.GetType().Assembly.GetName();
                var assembly = assemblyName.Name.Replace('.', '_');
                var version = assemblyName.Version.ToString().Replace('.', '_');
                location = String.Format(location, assembly, version);
            }

            return location;
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">
        /// An object that represents the control state to restore.
        /// </param>
        protected override void LoadViewState(object savedState)
        {
            // Customize state management to handle saving state of contained objects.
            if (savedState != null)
            {
                var state = (object[])savedState;

                if (state[0] != null)
                {
                    base.LoadViewState(state[0]);
                }

                if (state[1] != null)
                {
                    ((IStateManager)this.controlItemStyle).LoadViewState(state[1]);
                }

                if (state[2] != null)
                {
                    ((IStateManager)this.controlHiStyle).LoadViewState(state[2]);
                }

                if (state[3] != null)
                {
                    ((IStateManager)this.controlSubStyle).LoadViewState(state[3]);
                }

                if (state[4] != null)
                {
                    ((IStateManager)this.controlHiSubStyle).LoadViewState(state[4]);
                }
            }
        }

        /// <summary>
        /// Renders the specified output.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        protected override void Render(HtmlTextWriter output)
        {
            if (!this.HasControls())
            {
                // HACK
                this.CreateChildControls(); // If on pane CreateChildControls is not fired
            }

            if (this.Childs.Count > 0 && this.HasControls())
            {
                // Added by groskrg@versifit.com: 10/13/2004 to resolve issue with the Menu consuming more width than it should in Netscape browser.
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "HWMenuScript", this.ToMenuArray("Menu"));

                // line made obsolete by the change above 
                // output.Write(ToMenuArray("Menu"));
                this.Controls[0].RenderControl(output);
            }
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="M:System.Web.UI.WebControls.Style.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            // Customized state management to handle saving state of contained objects  such as styles.
            var baseState = base.SaveViewState();
            var controlStyle = ((IStateManager)this.controlItemStyle).SaveViewState();
            var histyle = (this.ControlHiStyle != null) ? ((IStateManager)this.controlHiStyle).SaveViewState() : null;
            var subStyle = (this.ControlSubStyle != null) ? ((IStateManager)this.controlSubStyle).SaveViewState() : null;
            var hisubStyle = (this.ControlHiSubStyle != null)
                                 ? ((IStateManager)this.controlHiSubStyle).SaveViewState()
                                 : null;

            var state = new object[5];
            state[0] = baseState;
            state[1] = controlStyle;
            state[2] = histyle;
            state[3] = subStyle;
            state[4] = hisubStyle;

            return state;
        }

        /// <summary>
        /// Retrieves a string representing the current menu array
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// The current menu array
        /// </returns>
        protected string ToMenuArray(string prefix)
        {
            var wc = new WebColorConverter();

            var sb = new StringBuilder();

            sb.Append("<script type = 'text/javascript'>\n");
            sb.Append("  function Go(){return}\n");
            sb.Append("</script>\n");

            sb.Append("<script type = 'text/javascript'>\n");

            sb.Append("var NoOffFirstLineMenus = ");
            sb.Append(this.Childs.Count);
            sb.Append(";\n");

            // MH:
            sb.Append("var CssItemClassName = ");
            sb.Append("\"");
            sb.Append(this.ControlItemStyle.CssClass);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var CssHiClassName = ");
            sb.Append("\"");
            sb.Append(this.ControlHiStyle.CssClass);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var CssSubClassName = ");
            sb.Append("\"");
            sb.Append(this.ControlSubStyle.CssClass);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var CssHiSubClassName = ");
            sb.Append("\"");
            sb.Append(this.ControlHiSubStyle.CssClass);
            sb.Append("\"");
            sb.Append(";\n");

            // MH:
            sb.Append("var LowBgColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.BackColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var LowSubBgColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlSubStyle.BackColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var HighBgColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlHiStyle.BackColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var HighSubBgColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlHiSubStyle.BackColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var FontLowColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ForeColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var FontSubLowColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlSubStyle.ForeColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var FontHighColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlHiStyle.ForeColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var FontSubHighColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlHiSubStyle.ForeColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var BorderColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.BorderColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var BorderSubColor = ");
            sb.Append("\"");
            sb.Append(wc.ConvertToString(this.ControlSubStyle.BorderColor));
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var BorderWidth = ");
            sb.Append(this.BorderWidth.Value);
            sb.Append(";\n");

            sb.Append("var BorderBtwnElmnts = ");
            sb.Append(this.BorderBtwnElmnts ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var FontFamily = ");
            sb.Append("\"");
            sb.Append(this.Font.Name);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var FontSize = ");
            sb.Append(this.Font.Size.Unit.Value);
            sb.Append(";\n");

            sb.Append("var FontBold = ");
            sb.Append(this.Font.Bold ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var FontItalic = ");
            sb.Append(this.Font.Italic ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var MenuTextCentered = ");
            sb.Append("\"");
            sb.Append(this.MenuTextCentered.ToString().ToLower());
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var MenuCentered = ");
            sb.Append("\"");
            sb.Append(this.MenuCentered.ToString().ToLower());
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var MenuVerticalCentered = ");
            sb.Append("\"");
            sb.Append(this.MenuVerticalCentered.ToString().ToLower());
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var ChildOverlap = ");
            sb.Append(this.ChildOverlap.ToString(new CultureInfo("en-US").NumberFormat));
            sb.Append(";\n");

            sb.Append("var ChildVerticalOverlap = ");
            sb.Append(this.ChildVerticalOverlap.ToString(new CultureInfo("en-US").NumberFormat));
            sb.Append(";\n");

            sb.Append("var LeftPaddng = ");
            sb.Append(this.LeftPaddng);
            sb.Append(";\n");

            sb.Append("var TopPaddng = ");
            sb.Append(this.TopPaddng);
            sb.Append(";\n");

            sb.Append("var StartTop = ");
            sb.Append(this.StartTop);
            sb.Append(";\n");

            sb.Append("var StartLeft = ");
            sb.Append(this.StartLeft);
            sb.Append(";\n");

            sb.Append("var VerCorrect = ");
            sb.Append(this.VerCorrect);
            sb.Append(";\n");

            sb.Append("var HorCorrect = ");
            sb.Append(this.HorCorrect);
            sb.Append(";\n");

            sb.Append("var FirstLineHorizontal = ");
            sb.Append(this.Horizontal ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var MenuFramesVertical = ");
            sb.Append(this.MenuFramesVertical ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var DissapearDelay = ");
            sb.Append(this.DissapearDelay);
            sb.Append(";\n");

            sb.Append("var TakeOverBgColor = ");
            sb.Append(this.TakeOverBgColor ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var FirstLineFrame = ");
            sb.Append("\"");
            sb.Append(this.FirstLineFrame);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var SecLineFrame = ");
            sb.Append("\"");
            sb.Append(this.SecLineFrame);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var DocTargetFrame = ");
            sb.Append("\"");
            sb.Append(this.DocTargetFrame);
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var HideTop = ");
            sb.Append(this.HideTop ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var TargetLoc = ");
            sb.Append("\"");

            // sb.Append(TargetLoc);
            // sb.Append(this.Controls[0].ClientID);
            sb.Append("MenuPos"); // NS4 bug fix
            sb.Append("\"");
            sb.Append(";\n");

            sb.Append("var MenuWrap = ");
            sb.Append(this.MenuWrap ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var RightToLeft = ");
            sb.Append(this.RightToLeft ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var UnfoldsOnClick = ");
            sb.Append(this.UnfoldsOnClick ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var WebMasterCheck = ");
            sb.Append(this.WebMasterCheck ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var ShowArrow = ");
            sb.Append(this.ShowArrow ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var KeepHilite = ");
            sb.Append(this.KeepHilite ? 1 : 0);
            sb.Append(";\n");

            sb.Append("var Arrws = ");
            sb.Append("[");
            for (var i = 0; i <= this.arrws.GetUpperBound(0); i++)
            {
                sb.Append("\"");
                sb.Append(this.ImagesPath + this.arrws[i].ImageUrl);
                sb.Append("\", ");
                sb.Append(this.arrws[i].Width.Value);
                sb.Append(", ");
                sb.Append(this.arrws[i].Height.Value);
                if (i != this.arrws.GetUpperBound(0))
                {
                    sb.Append(", ");
                }
            }

            sb.Append("]");
            sb.Append(";\n");

            sb.Append("function BeforeStart(){;}\n");
            sb.Append("function AfterBuild(){;}\n");
            sb.Append("function BeforeFirstOpen(){;}\n");
            sb.Append("function AfterCloseAll(){;}\n");

            sb.Append(this.Childs.ToMenuArray(prefix));

            sb.Append("</script>\n");
            sb.AppendFormat("<script type = 'text/javascript' src = '{0}menu_com.js'></script>\n", this.ClientScriptPath);
            sb.Append("<noscript>Your browser does not support script</noscript>\n");

            return sb.ToString();
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            // Customized state management to handle saving state of contained objects such as styles.
            base.TrackViewState();

            if (this.controlItemStyle != null)
            {
                ((IStateManager)this.controlItemStyle).TrackViewState();
            }

            if (this.controlHiStyle != null)
            {
                ((IStateManager)this.controlHiStyle).TrackViewState();
            }

            if (this.controlSubStyle != null)
            {
                ((IStateManager)this.controlSubStyle).TrackViewState();
            }

            if (this.controlHiSubStyle != null)
            {
                ((IStateManager)this.controlHiSubStyle).TrackViewState();
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        private void Initialize()
        {
            this.Childs = new MenuTreeNodes();

            this.controlItemStyle = new Style();
            this.controlSubStyle = new Style();
            this.controlHiStyle = new Style();
            this.controlHiSubStyle = new Style();

            this.CssClass = string.Empty;
            this.controlItemStyle.CssClass = string.Empty;
            this.controlSubStyle.CssClass = string.Empty;
            this.controlHiStyle.CssClass = string.Empty;
            this.controlHiSubStyle.CssClass = string.Empty;

            this.BackColor = Color.White;
            this.controlItemStyle.BackColor = Color.White;
            this.controlSubStyle.BackColor = Color.White;
            this.controlHiStyle.BackColor = Color.Black;
            this.controlHiSubStyle.BackColor = Color.Black;

            this.ForeColor = Color.Black;
            this.controlItemStyle.ForeColor = Color.Black;
            this.controlSubStyle.ForeColor = Color.Black;
            this.controlHiStyle.ForeColor = Color.White;
            this.controlHiSubStyle.ForeColor = Color.White;

            this.BorderColor = Color.Black;
            this.controlItemStyle.BorderColor = Color.Black;
            this.controlSubStyle.BorderColor = Color.Black;

            this.BorderWidth = new Unit(1);

            this.ControlStyle.Width = new Unit(100);
            this.ControlStyle.Height = new Unit(20);

            this.Font.Names = new[] { "Arial", "sans-serif" };
            this.Font.Size = new FontUnit(9);
            this.Font.Bold = true;
            this.Font.Italic = false;

            this.arrws = new Image[3];

            this.arrws[0] = new Image { ImageUrl = "tri.gif", Width = 5, Height = 10 };

            this.arrws[1] = new Image { ImageUrl = "tridown.gif", Width = 10, Height = 5 };

            this.arrws[2] = new Image { ImageUrl = "trileft.gif", Width = 5, Height = 10 };
        }

        #endregion
    }
}