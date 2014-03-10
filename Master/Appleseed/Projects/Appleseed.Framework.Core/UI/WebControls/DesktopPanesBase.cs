// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopPanesBase.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   DesktopPanes class for supporting three pane browsing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// DesktopPanes class for supporting three pane browsing
    /// </summary>
    [Designer("Appleseed.Framework.Web.UI.WebControls.DesktopPanesDesigner")]
    public class DesktopPanesBase : WebControl, INamingContainer
    {
        #region Constants and Fields

        /// <summary>
        /// The idx content pane data.
        /// </summary>
        protected const string IDX_CONTENT_PANE_DATA = "contentpane";

        /// <summary>
        /// The idx left pane data.
        /// </summary>
        protected const string IDX_LEFT_PANE_DATA = "leftpane";

        /// <summary>
        /// The idx right pane data.
        /// </summary>
        protected const string IDX_RIGHT_PANE_DATA = "rightpane";

        /// <summary>
        /// The idx content pane style.
        /// </summary>
        private const int IdxContentPaneStyle = 2;

        /// <summary>
        /// The idx control style.
        /// </summary>
        private const int IdxControlStyle = 0;

        /// <summary>
        /// The idx horizontal separator style.
        /// </summary>
        private const int IdxHorizontalSeparatorStyle = 4;

        /// <summary>
        /// The idx left pane style.
        /// </summary>
        private const int IdxLeftPaneStyle = 1;

        /// <summary>
        /// The idx right pane style.
        /// </summary>
        private const int IdxRightPaneStyle = 3;

        /// <summary>
        /// The idx vertical separator style.
        /// </summary>
        private const int IdxVerticalSeparatorStyle = 5;

        /// <summary>
        /// The content pane.
        /// </summary>
        private TableCell contentPane;

        /// <summary>
        /// The data source.
        /// </summary>
        private Dictionary<string, List<Control>> dataSource;

        /// <summary>
        /// The left pane.
        /// </summary>
        private TableCell leftPane;

        /// <summary>
        /// The right pane.
        /// </summary>
        private TableCell rightPane;

        /// <summary>
        /// The content pane style.
        /// </summary>
        private TableItemStyle contentPaneStyle;

        /// <summary>
        /// The horizontal separator style.
        /// </summary>
        private Style horizontalSeparatorStyle;

        /// <summary>
        /// The left pane style.
        /// </summary>
        private TableItemStyle leftPaneStyle;

        /// <summary>
        /// The right pane style.
        /// </summary>
        private TableItemStyle rightPaneStyle;

        /// <summary>
        /// The vertical separator style.
        /// </summary>
        private TableItemStyle verticalSeparatorStyle;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the cell padding of the rendered table.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(0)]
        [Description("The cell padding of the rendered table.")]
        public virtual int CellPadding
        {
            get
            {
                return this.ControlStyleCreated == false ? 0 : ((TableStyle)this.ControlStyle).CellPadding;
            }

            set
            {
                ((TableStyle)this.ControlStyle).CellPadding = value;
            }
        }

        /// <summary>
        ///   Gets or sets the cell spacing of the rendered table.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(0)]
        [Description("The cell spacing of the rendered table.")]
        public virtual int CellSpacing
        {
            get
            {
                return this.ControlStyleCreated == false ? 0 : ((TableStyle)this.ControlStyle).CellSpacing;
            }

            set
            {
                ((TableStyle)this.ControlStyle).CellSpacing = value;
            }
        }

        /// <summary>
        ///   Gets the style to be applied to ContentPane.
        /// </summary>
        [Category("Style")]
        [Description("The style to be applied to ContentPane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual TableItemStyle ContentPaneStyle
        {
            get
            {
                if (this.contentPaneStyle == null)
                {
                    // Default
                    this.contentPaneStyle = new TableItemStyle { VerticalAlign = VerticalAlign.Top };

                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.contentPaneStyle).TrackViewState();
                    }
                }

                return this.contentPaneStyle;
            }
        }

        /// <summary>
        /// Gets or sets the content pane template.
        /// </summary>
        /// <value>
        /// The content pane template.
        /// </value>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The Content Pane.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DesktopPanesTemplate))]
        public virtual ITemplate ContentPaneTemplate { get; set; }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [Category("Data")]
        [Description("The DataSource.")]
        public Dictionary<string, List<Control>> DataSource
        {
            get
            {
                if (this.dataSource == null)
                {
                    this.InitializeDataSource();
                }

                return this.dataSource;
            }

            set
            {
                this.dataSource = value;
            }
        }

        /// <summary>
        ///   Gets or sets the grid lines to be shown in the rendered table.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(GridLines.None)]
        [Description("The grid lines to be shown in the rendered table.")]
        public virtual GridLines GridLines
        {
            get
            {
                return this.ControlStyleCreated == false ? GridLines.None : ((TableStyle)this.ControlStyle).GridLines;
            }

            set
            {
                ((TableStyle)this.ControlStyle).GridLines = value;
            }
        }

        /// <summary>
        ///   Gets or sets the height of the rendered table.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The height of the rendered table.")]
        public override Unit Height
        {
            get
            {
                return this.ControlStyleCreated ? this.ControlStyle.Height : 0;
            }

            set
            {
                this.ControlStyle.Height = value;
            }
        }

        /// <summary>
        ///   Gets or sets the style to be applied to Horizontal Separator.
        /// </summary>
        [Category("Style")]
        [Description("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual Style HorizontalSeparatorStyle
        {
            get
            {
                if (this.horizontalSeparatorStyle == null)
                {
                    this.horizontalSeparatorStyle = new Style();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.horizontalSeparatorStyle).TrackViewState();
                    }
                }

                return this.horizontalSeparatorStyle;
            }
        }

        /// <summary>
        ///   Gets or sets the HorizontalSeparator.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The HorizontalSeparator.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DesktopPanesTemplate))]
        public virtual ITemplate HorizontalSeparatorTemplate { get; set; }

        /// <summary>
        ///   Gets the style to be applied to LeftPane.
        /// </summary>
        [Category("Style")]
        [Description("The style to be applied to LeftPane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual TableItemStyle LeftPaneStyle
        {
            get
            {
                if (this.leftPaneStyle == null)
                {
                    // Default
                    this.leftPaneStyle = new TableItemStyle { Width = new Unit(170), VerticalAlign = VerticalAlign.Top };
                    
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.leftPaneStyle).TrackViewState();
                    }
                }

                return this.leftPaneStyle;
            }
        }

        /// <summary>
        ///   The Left Pane.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The Left Pane.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DesktopPanesTemplate))]
        public virtual ITemplate LeftPaneTemplate { get; set; }

        /// <summary>
        ///   The style to be applied to RightPane.
        /// </summary>
        [Category("Style")]
        [Description("The style to be applied to RightPane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual TableItemStyle RightPaneStyle
        {
            get
            {
                if (this.rightPaneStyle == null)
                {
                    // Default
                    this.rightPaneStyle = new TableItemStyle
                        {
                            Width = new Unit(230), VerticalAlign = VerticalAlign.Top 
                        };

                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.rightPaneStyle).TrackViewState();
                    }
                }

                return this.rightPaneStyle;
            }
        }

        /// <summary>
        ///   The Right Pane.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The Right Pane.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DesktopPanesTemplate))]
        public virtual ITemplate RightPaneTemplate { get; set; }

        /// <summary>
        ///   Show First Separator.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Show First Separator.")]
        public virtual bool ShowFirstSeparator { get; set; }

        /// <summary>
        ///   Show Last Separator.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Show Last Separator.")]
        public virtual bool ShowLastSeparator { get; set; }

        /// <summary>
        ///   The style to be applied to Horizontal Separator.
        /// </summary>
        [Category("Style")]
        [Description("The style to be applied to Horizontal Separator.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual TableItemStyle VerticalSeparatorStyle
        {
            get
            {
                if (this.verticalSeparatorStyle == null)
                {
                    this.verticalSeparatorStyle = new TableItemStyle();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this.verticalSeparatorStyle).TrackViewState();
                    }
                }

                return this.verticalSeparatorStyle;
            }
        }

        /// <summary>
        /// Gets or sets the vertical separator template.
        /// </summary>
        /// <value>
        /// The vertical separator template.
        /// </value>
        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The VerticalSeparator.")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DesktopPanesTemplate))]
        public virtual ITemplate VerticalSeparatorTemplate { get; set; }

        /// <summary>
        ///   The width of the rendered table.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("100%")]
        [Description("The width of the rendered table.")]
        public override Unit Width
        {
            get
            {
                return this.ControlStyleCreated ? this.ControlStyle.Width : 0;
            }

            set
            {
                this.ControlStyle.Width = value;
            }
        }

        /// <summary>
        /// Gets the content pane.
        /// </summary>
        [Browsable(false)]
        private TableCell ContentPane
        {
            get
            {
                return this.contentPane ?? (this.contentPane = new TableCell());
            }
        }

        /// <summary>
        /// Gets the left pane.
        /// </summary>
        [Browsable(false)]
        private TableCell LeftPane
        {
            get
            {
                return this.leftPane ?? (this.leftPane = new TableCell());
            }
        }

        /// <summary>
        /// Gets the right pane.
        /// </summary>
        [Browsable(false)]
        private TableCell RightPane
        {
            get
            {
                return this.rightPane ?? (this.rightPane = new TableCell());
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            if (this.DataSource.ContainsKey(IDX_LEFT_PANE_DATA))
            {
                foreach (Control c in this.DataSource[IDX_LEFT_PANE_DATA])
                {
                    this.LeftPane.Controls.Add(c);
                    this.LeftPane.Controls.Add(this.GetHorizontalSeparator());
                }
            }

            if (this.DataSource.ContainsKey(IDX_CONTENT_PANE_DATA))
            {
                foreach (Control c in this.DataSource[IDX_CONTENT_PANE_DATA])
                {
                    this.ContentPane.Controls.Add(c);
                    this.ContentPane.Controls.Add(this.GetHorizontalSeparator());
                }
            }

            if (this.DataSource.ContainsKey(IDX_RIGHT_PANE_DATA))
            {
                foreach (Control c in this.DataSource[IDX_RIGHT_PANE_DATA])
                {
                    this.RightPane.Controls.Add(c);
                    this.RightPane.Controls.Add(this.GetHorizontalSeparator());
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This member overrides Control.CreateChildControls
        /// </summary>
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.CreateControlHierarchy();
            base.CreateChildControls();
        }

        /// <summary>
        /// This member overrides Control.CreateControlHierarchy
        /// </summary>
        protected virtual void CreateControlHierarchy()
        {
            // NEVER hide controls on this routine
            // some events WILL NOT FIRED
            this.Controls.Clear();

            var table = new Table();
            this.Controls.Add(table);

            // Prepare Control Hierarchy
            var contentRow = new TableRow();

            contentRow.Controls.Add(this.GetVerticalSeparator());

            contentRow.Controls.Add(this.LeftPane);
            var leftToContentSeparator = this.GetVerticalSeparator();
            contentRow.Controls.Add(leftToContentSeparator);

            if (this.LeftPaneTemplate != null)
            {
                Control leftPaneContainer = new DesktopPanesTemplate(this);
                this.LeftPaneTemplate.InstantiateIn(leftPaneContainer);
                this.LeftPane.Controls.AddAt(0, leftPaneContainer);
                this.LeftPane.Controls.AddAt(1, this.GetHorizontalSeparator());
            }

            if (this.ContentPaneTemplate != null)
            {
                Control contentPaneContainer = new DesktopPanesTemplate(this);
                this.ContentPaneTemplate.InstantiateIn(contentPaneContainer);
                this.ContentPane.Controls.AddAt(0, contentPaneContainer);
                this.ContentPane.Controls.AddAt(1, this.GetHorizontalSeparator());
            }

            contentRow.Controls.Add(this.ContentPane);

            var contentToRightSeparator = this.GetVerticalSeparator();
            contentRow.Controls.Add(contentToRightSeparator);
            contentRow.Controls.Add(this.RightPane);

            if (this.RightPaneTemplate != null)
            {
                Control rightPaneContainer = new DesktopPanesTemplate(this);
                this.RightPaneTemplate.InstantiateIn(rightPaneContainer);
                this.RightPane.Controls.AddAt(0, rightPaneContainer);
                this.RightPane.Controls.AddAt(1, this.GetHorizontalSeparator());
            }

            contentRow.Controls.Add(this.GetVerticalSeparator());

            table.Controls.Add(contentRow);
        }

        /// <summary>
        /// Web server control can set its control style to 
        ///   any class that derives from Style by overriding 
        ///   the WebControl.CreateControlStyle method
        /// </summary>
        /// <returns>
        /// </returns>
        protected override Style CreateControlStyle()
        {
            // Note that the constructor of Style takes   
            // ViewState as an argument. 
            // Set up default initial state.
            var style = new TableStyle(this.ViewState) { CellSpacing = 0, Width = new Unit("100%") };

            return style;
        }

        /// <summary>
        /// Returns a reference to Horizontal separator
        /// </summary>
        /// <returns>
        /// </returns>
        protected Control GetHorizontalSeparator()
        {
            if (this.HorizontalSeparatorTemplate == null)
            {
                return new Control();
            }
            
            Control horizontalSeparatorContainer = new DesktopPanesTemplate(this);
            this.HorizontalSeparatorTemplate.InstantiateIn(horizontalSeparatorContainer);
            return horizontalSeparatorContainer;
        }

        /// <summary>
        /// Returns a reference to Vertical separator
        /// </summary>
        /// <returns>
        /// </returns>
        protected TableCell GetVerticalSeparator()
        {
            var tc = new TableCell();

            if (this.VerticalSeparatorTemplate != null)
            {
                Control verticalSeparatorContainer = new DesktopPanesTemplate(this);
                this.VerticalSeparatorTemplate.InstantiateIn(verticalSeparatorContainer);
                tc.Controls.Add(verticalSeparatorContainer);
            }

            return tc;
        }

        /// <summary>
        /// Initialize internal data source
        /// </summary>
        protected virtual void InitializeDataSource()
        {
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            // Customize state management to handle saving state of contained objects.
            if (savedState != null)
            {
                var state = (object[])savedState;

                if (state[IdxControlStyle] != null)
                {
                    base.LoadViewState(state[IdxControlStyle]);
                }

                if (state[IdxLeftPaneStyle] != null)
                {
                    ((IStateManager)this.leftPaneStyle).LoadViewState(state[IdxLeftPaneStyle]);
                }

                if (state[IdxContentPaneStyle] != null)
                {
                    ((IStateManager)this.contentPaneStyle).LoadViewState(state[IdxContentPaneStyle]);
                }

                if (state[IdxRightPaneStyle] != null)
                {
                    ((IStateManager)this.rightPaneStyle).LoadViewState(state[IdxRightPaneStyle]);
                }

                if (state[IdxHorizontalSeparatorStyle] != null)
                {
                    ((IStateManager)this.horizontalSeparatorStyle).LoadViewState(state[IdxHorizontalSeparatorStyle]);
                }

                if (state[IdxVerticalSeparatorStyle] != null)
                {
                    ((IStateManager)this.verticalSeparatorStyle).LoadViewState(state[IdxVerticalSeparatorStyle]);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.DataBinding"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            this.EnsureChildControls();
            base.OnDataBinding(e);
        }

        /// <summary>
        /// Prepares the control hierarchy.
        /// </summary>
        protected virtual void PrepareControlHierarchy()
        {
            if (this.HasControls() == false)
            {
                return;
            }

            var table = (Table)this.Controls[0];

            var cells = table.Rows[0].Cells;

            table.CopyBaseAttributes(this);
            if (this.ControlStyleCreated)
            {
                table.ApplyStyle(this.ControlStyle);
            }

            var firstSeparator = cells[0];
            firstSeparator.Visible = this.ShowFirstSeparator;
            firstSeparator.MergeStyle(this.VerticalSeparatorStyle);

            var leftCell = cells[1];
            leftCell.MergeStyle(this.LeftPaneStyle);

            var leftToContentSeparator = cells[2];
            leftToContentSeparator.MergeStyle(this.VerticalSeparatorStyle);

            if (this.LeftPane.HasControls() || this.LeftPane.Text.Trim() != String.Empty)
            {
                this.LeftPane.Visible = true;
                leftToContentSeparator.Visible = true;
            }
            else
            {
                this.LeftPane.Visible = false;
                leftToContentSeparator.Visible = false;
            }

            var contentCell = cells[3];
            contentCell.MergeStyle(this.ContentPaneStyle);

            var contentToRightSeparator = cells[4];
            contentToRightSeparator.MergeStyle(this.VerticalSeparatorStyle);

            var rightCell = cells[5];
            rightCell.MergeStyle(this.RightPaneStyle);

            var lastSeparator = cells[6];
            lastSeparator.Visible = this.ShowLastSeparator;
            lastSeparator.MergeStyle(this.VerticalSeparatorStyle);

            if (this.RightPane.HasControls() || this.RightPane.Text.Trim() != String.Empty)
            {
                contentToRightSeparator.Visible = true;
                this.RightPane.Visible = true;
            }
            else
            {
                contentToRightSeparator.Visible = false;
                this.RightPane.Visible = false;
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // Apply styles to the control hierarchy
            // and then render it out.

            // Apply styles during render phase, so the user can change styles
            // after calling DataBind without the property changes ending
            // up in view state.
            this.PrepareControlHierarchy();

            this.RenderContents(writer);
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
            var leftPaneStyleState = (this.leftPaneStyle != null)
                                         ? ((IStateManager)this.leftPaneStyle).SaveViewState()
                                         : null;
            var contentPaneStyleState = (this.contentPaneStyle != null)
                                            ? ((IStateManager)this.contentPaneStyle).SaveViewState()
                                            : null;
            var rightPaneStyleState = (this.rightPaneStyle != null)
                                          ? ((IStateManager)this.rightPaneStyle).SaveViewState()
                                          : null;
            var horizontalSeparatorStyleState = (this.horizontalSeparatorStyle != null)
                                                    ? ((IStateManager)this.horizontalSeparatorStyle).SaveViewState()
                                                    : null;
            var verticalSeparatorStyleState = (this.verticalSeparatorStyle != null)
                                                  ? ((IStateManager)this.verticalSeparatorStyle).SaveViewState()
                                                  : null;

            var state = new object[6];
            state[IdxControlStyle] = baseState;
            state[IdxLeftPaneStyle] = leftPaneStyleState;
            state[IdxContentPaneStyle] = contentPaneStyleState;
            state[IdxRightPaneStyle] = rightPaneStyleState;
            state[IdxHorizontalSeparatorStyle] = horizontalSeparatorStyleState;
            state[IdxVerticalSeparatorStyle] = verticalSeparatorStyleState;

            return state;
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            // Customized state management to handle saving state of contained objects such as styles.
            base.TrackViewState();

            if (this.leftPaneStyle != null)
            {
                ((IStateManager)this.leftPaneStyle).TrackViewState();
            }

            if (this.contentPaneStyle != null)
            {
                ((IStateManager)this.contentPaneStyle).TrackViewState();
            }

            if (this.rightPaneStyle != null)
            {
                ((IStateManager)this.rightPaneStyle).TrackViewState();
            }

            if (this.horizontalSeparatorStyle != null)
            {
                ((IStateManager)this.horizontalSeparatorStyle).TrackViewState();
            }

            if (this.verticalSeparatorStyle != null)
            {
                ((IStateManager)this.verticalSeparatorStyle).TrackViewState();
            }
        }

        #endregion
    }
}