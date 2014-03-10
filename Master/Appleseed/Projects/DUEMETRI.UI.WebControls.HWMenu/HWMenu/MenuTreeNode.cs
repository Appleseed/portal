// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuTreeNode.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The Menu Tree Node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DUEMETRI.UI.WebControls.HWMenu
{
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The Menu Tree Node.
    /// </summary>
    public class MenuTreeNode : WebControl, INamingContainer
    {
        #region Constants and Fields

        /// <summary>
        ///     The control hi style.
        /// </summary>
        private Style controlHiStyle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuTreeNode"/> class.
        ///     Public constructor. Initializes text property.
        /// </summary>
        /// <param name="text">
        /// The text of the node.
        /// </param>
        public MenuTreeNode(string text)
        {
            this.Initialize();
            this.Text = text;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "MenuTreeNode" /> class. 
        ///     Public constructor.
        /// </summary>
        public MenuTreeNode()
        {
            this.Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets Background image for the element. 
        ///     Is not supported for NS4 when the menu is across frames. 
        ///     I had to disable this for NS4 in frame setup because 
        ///     I could not get it to work properly. 
        ///     (Everybody who wants to try and find a solution for this is very welcome. 
        ///     Enable in menu_com.js)
        /// </summary>
        [Category("Appearance")]
        [Description("Background image for the element.")]
        public string BackgroundImage { get; set; }

        /// <summary>
        ///     Gets Menu items collection.
        /// </summary>
        [Category("Data")]
        [Description("Menu items.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public MenuTreeNodes Childs { get; private set; }

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
        ///     Gets or sets Text string- Where you want to go when you click the element.
        ///     Looks like "MyLink" 
        ///     Can also be used to execute javascript statements. 
        ///     For instance when you want the link to open in the top window use 
        ///     "javascript:top.document.location.href='Link.htm';"
        ///     You can in fact start a whole script when the element is clicked 
        ///     with the help of javascript:. "javascript:{your script; another function;}"
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///     Gets or sets what you want to show in the element. It can be text, an image or html.
        ///     To show an image Text will be look like "&lt;img src='MyImage'&gt;"
        ///     To use roll over images use "rollover:MyImage1:MyImage2"
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a string representing the current menu array
        /// </summary>
        /// <returns>
        /// The current menu array
        /// </returns>
        public string ToMenuArray()
        {
            var wc = new WebColorConverter();

            var sb = new StringBuilder();

            sb.Append("=new Array(");
            sb.Append(CleanForJavascript(this.Text));
            sb.Append(", ");
            sb.Append(CleanForJavascript(this.Link));
            sb.Append(", ");
            sb.Append(CleanForJavascript(this.BackgroundImage));
            sb.Append(", ");
            sb.Append(this.Childs.Count);
            sb.Append(", ");
            if (this.Height.Value > 0)
            {
                sb.Append(this.Height.Value);
            }
            else
            {
                sb.Append(20);
            }

            sb.Append(", ");
            if (this.Width.Value > 0)
            {
                sb.Append(this.Width.Value);
            }
            else
            {
                sb.Append((this.Text.Length * 7) + 15);
            }

            sb.Append(", ");
            sb.Append(CleanForJavascript(wc.ConvertToString(this.ControlStyle.BackColor)));
            sb.Append(", ");
            sb.Append(CleanForJavascript(wc.ConvertToString(this.ControlHiStyle.BackColor)));
            sb.Append(", ");
            sb.Append(CleanForJavascript(wc.ConvertToString(this.ControlStyle.ForeColor)));
            sb.Append(", ");
            sb.Append(CleanForJavascript(wc.ConvertToString(this.ControlHiStyle.ForeColor)));
            sb.Append(", ");
            sb.Append(CleanForJavascript(wc.ConvertToString(this.ControlStyle.BorderColor)));
            sb.Append(");");
            sb.Append("\n");

            return sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">
        /// An object that represents the control state to restore.
        /// </param>
        protected override void LoadViewState(object savedState)
        {
            // Customize state management to handle saving state of contained objects.
            if (savedState == null)
            {
                return;
            }

            var state = (object[])savedState;

            if (state[0] != null)
            {
                base.LoadViewState(state[0]);
            }

            if (state[1] != null)
            {
                ((IStateManager)this.controlHiStyle).LoadViewState(state[1]);
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
            var style = (this.ControlHiStyle != null) ? ((IStateManager)this.controlHiStyle).SaveViewState() : null;

            var state = new object[2];
            state[0] = baseState;
            state[1] = style;

            return state;
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            // Customized state management to handle saving state of contained objects such as styles.
            base.TrackViewState();

            if (this.controlHiStyle != null)
            {
                ((IStateManager)this.controlHiStyle).TrackViewState();
            }
        }

        /// <summary>
        /// Cleans for javascript.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The cleaned string.
        /// </returns>
        private static string CleanForJavascript(string value)
        {
            var sb = new StringBuilder(value);
            sb.Replace("'", "\'");
            sb.Replace("\"", "\\\"");
            sb.Insert(0, "\"");
            sb.Append("\"");
            return sb.ToString();
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        private void Initialize()
        {
            this.Text = "Untitled menu";
            this.Childs = new MenuTreeNodes();
            this.ControlStyle.Width = new Unit(100);
            this.ControlStyle.Height = new Unit(20);
        }

        #endregion
    }
}