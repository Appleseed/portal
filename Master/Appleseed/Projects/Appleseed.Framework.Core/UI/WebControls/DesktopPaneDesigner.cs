// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopPaneDesigner.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   DesktopPanes design support class for Visual Studio.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;

    /// <summary>
    /// DesktopPanes design support class for Visual Studio.
    /// </summary>
    public class DesktopPanesDesigner : TemplatedControlDesigner
    {
        #region Constants and Fields

        /// <summary>
        ///   The desktop panes designer switch.
        /// </summary>
        internal static TraceSwitch DesktopPanesDesignerSwitch;

        /// <summary>
        /// The idx content pane template.
        /// </summary>
        private const int IdxContentPaneTemplate = 1;

        /// <summary>
        /// The idx horizontal separator template.
        /// </summary>
        private const int IdxHorizontalSeparatorTemplate = 0;

        /// <summary>
        /// The idx left pane template.
        /// </summary>
        private const int IdxLeftPaneTemplate = 0;

        /// <summary>
        /// The idx right pane template.
        /// </summary>
        private const int IdxRightPaneTemplate = 2;

        /// <summary>
        /// The idx vertical separator template.
        /// </summary>
        private const int IdxVerticalSeparatorTemplate = 1;

        /// <summary>
        ///   The pane templates.
        /// </summary>
        private const int PaneTemplates = 0;

        /// <summary>
        ///   The separator templates.
        /// </summary>
        private const int SeparatorTemplates = 1;

        /// <summary>
        ///   The pane template names.
        /// </summary>
        private static readonly string[] PaneTemplateNames;

        /// <summary>
        ///   The separator template names.
        /// </summary>
        private static readonly string[] SeparatorTemplateNames;

        /// <summary>
        ///   The desktop panes.
        /// </summary>
        private DesktopPanes desktopPanes;

        #pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        ///   The template verbs.
        /// </summary>
        private TemplateEditingVerb[] templateVerbs;
        #pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        ///   The template verbs dirty.
        /// </summary>
        private bool templateVerbsDirty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "DesktopPanesDesigner" /> class.
        /// </summary>
        static DesktopPanesDesigner()
        {
            DesktopPanesDesignerSwitch = new TraceSwitch(
                "DESKTOPPANEDESIGNER", "Enable DesktopPanes designer general purpose traces.");

            var templateNames = new string[3];
            templateNames[IdxLeftPaneTemplate] = "Left pane template";
            templateNames[IdxContentPaneTemplate] = "Content pane template";
            templateNames[IdxRightPaneTemplate] = "Right pane template";
            PaneTemplateNames = templateNames;

            templateNames = new string[2];
            templateNames[IdxHorizontalSeparatorTemplate] = "Horizontal Separator";
            templateNames[IdxVerticalSeparatorTemplate] = "Vertical Separator";
            SeparatorTemplateNames = templateNames;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "DesktopPanesDesigner" /> class. 
        ///   Default constructor
        /// </summary>
        public DesktopPanesDesigner()
        {
            this.templateVerbsDirty = true;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Override the AllowResize property inherited
        ///   from ControlDesigner to enable the control 
        ///   to be resized. 
        ///   It is recommended that controls allow resizing 
        ///   when in template mode even if they normally 
        ///   do not allow resizing.
        /// </summary>
        public override bool AllowResize
        {
            get
            {
                // When templates are not defined, render a read-only fixed-size block. 
                // Once templates are defined or are being edited, the control should allow resizing.
                #pragma warning disable CS0618 // Type or member is obsolete
                return this.desktopPanes.ContentPaneTemplate == null || InTemplateMode;
                #pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// As with any other designer, override the GetDesignTimeHtml.
        ///   Gets the HTML that is used to represent the control at design time.
        /// </summary>
        /// <returns>
        /// The get design time html.
        /// </returns>
        public override string GetDesignTimeHtml()
        {
            var designTimeHtml = new StringBuilder();

            designTimeHtml.Append("<TABLE");
            if (!this.desktopPanes.Width.IsEmpty)
            {
                designTimeHtml.Append(" width='");
                designTimeHtml.Append(this.desktopPanes.Width.Value);
                designTimeHtml.Append("'");
            }

            if (!this.desktopPanes.Height.IsEmpty)
            {
                designTimeHtml.Append(" height='");
                designTimeHtml.Append(this.desktopPanes.Height.Value);
                designTimeHtml.Append("'");
            }

            designTimeHtml.Append(" BORDER='1'>");
            designTimeHtml.Append("<TR>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.VerticalSeparatorTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.VerticalSeparatorTemplate));
            }

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.LeftPaneTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.LeftPaneTemplate));
            }
            

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.VerticalSeparatorTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.VerticalSeparatorTemplate));
            }

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.ContentPaneTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.ContentPaneTemplate));
            }

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.VerticalSeparatorTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.VerticalSeparatorTemplate));
            }

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.RightPaneTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.RightPaneTemplate));
            }
            
            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("<TD>");
            if (this.desktopPanes.VerticalSeparatorTemplate != null)
            {
                designTimeHtml.Append(this.GetTextFromTemplate(this.desktopPanes.VerticalSeparatorTemplate));
            }

            designTimeHtml.Append("</TD>");

            designTimeHtml.Append("</TR>");
            designTimeHtml.Append("</TABLE>");

            return designTimeHtml.ToString();
        }

        /// <summary>
        /// Override the GetTemplateContent method that gets 
        ///   the template content.
        /// </summary>
        /// <param name="editingFrame">
        /// The editing Frame.
        /// </param>
        /// <param name="templateName">
        /// The template Name.
        /// </param>
        /// <param name="allowEditing">
        /// The allow Editing.
        /// </param>
        /// <returns>
        /// The get template content.
        /// </returns>
        [Obsolete(
            "Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override string GetTemplateContent(
            ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing)
        {
            Trace.Assert(editingFrame.Verb.Index >= 0 && editingFrame.Verb.Index <= 2);

            allowEditing = true;
            ITemplate template = null;
            var templateContent = String.Empty;

            switch (editingFrame.Verb.Index)
            {
                case PaneTemplates:
                    if (templateName == PaneTemplateNames[IdxLeftPaneTemplate])
                    {
                        template = this.desktopPanes.LeftPaneTemplate;
                        break;
                    }

                    if (templateName == PaneTemplateNames[IdxContentPaneTemplate])
                    {
                        template = this.desktopPanes.ContentPaneTemplate;
                        break;
                    }

                    if (templateName == PaneTemplateNames[IdxRightPaneTemplate])
                    {
                        template = this.desktopPanes.RightPaneTemplate;
                        break;
                    }

                    break;

                case SeparatorTemplates:
                    if (templateName == PaneTemplateNames[IdxHorizontalSeparatorTemplate])
                    {
                        template = this.desktopPanes.HorizontalSeparatorTemplate;
                        break;
                    }

                    if (templateName == PaneTemplateNames[IdxVerticalSeparatorTemplate])
                    {
                        template = this.desktopPanes.VerticalSeparatorTemplate;
                        break;
                    }

                    break;
            }

            if (template != null)
            {
                templateContent = this.GetTextFromTemplate(template);
            }

            return templateContent;
        }

        /// <summary>
        /// Initializes the designer and loads the specified component.
        /// </summary>
        /// <param name="component">
        /// The control element being designed.
        /// </param>
        public override void Initialize(IComponent component)
        {
            this.desktopPanes = (DesktopPanes)component;

            base.Initialize(component);
        }

        /// <summary>
        /// Override the SetTemplateContent method that sets 
        ///   the template content.
        /// </summary>
        /// <param name="editingFrame">
        /// The editing Frame.
        /// </param>
        /// <param name="templateName">
        /// The template Name.
        /// </param>
        /// <param name="templateContent">
        /// The template Content.
        /// </param>
        [Obsolete(
            "Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        public override void SetTemplateContent(
            ITemplateEditingFrame editingFrame, string templateName, string templateContent)
        {
            Trace.Assert(editingFrame.Verb.Index >= 0 && editingFrame.Verb.Index <= 2);

            ITemplate template = null;

            if (!string.IsNullOrEmpty(templateContent))
            {
                template = this.GetTemplateFromText(templateContent);
            }

            switch (editingFrame.Verb.Index)
            {
                case PaneTemplates:
                    if (templateName == PaneTemplateNames[IdxLeftPaneTemplate])
                    {
                        this.desktopPanes.LeftPaneTemplate = template;
                        return;
                    }

                    if (templateName == PaneTemplateNames[IdxContentPaneTemplate])
                    {
                        this.desktopPanes.ContentPaneTemplate = template;
                        return;
                    }

                    if (templateName == PaneTemplateNames[IdxRightPaneTemplate])
                    {
                        this.desktopPanes.RightPaneTemplate = template;
                        return;
                    }

                    break;
                case SeparatorTemplates:
                    if (templateName == PaneTemplateNames[IdxHorizontalSeparatorTemplate])
                    {
                        this.desktopPanes.HorizontalSeparatorTemplate = template;
                        return;
                    }

                    if (templateName == PaneTemplateNames[IdxVerticalSeparatorTemplate])
                    {
                        this.desktopPanes.VerticalSeparatorTemplate = template;
                        return;
                    }

                    return;

                default:
                    return;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, creates a template editing frame for the specified verb.
        /// </summary>
        /// <param name="verb">
        /// The template editing verb to create a template editing frame for.
        /// </param>
        /// <returns>
        /// The new template editing frame.
        /// </returns>
        [Obsolete(
            "Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202"
            )]
        protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
        {
            var teService = (ITemplateEditingService)this.GetService(typeof(ITemplateEditingService));

            Trace.Assert(teService != null, "How did we get this far without an ITemplateEditingService?");
            Trace.Assert(verb.Index == 0 || verb.Index == SeparatorTemplates);

            string[] templateNames = null;
            Style[] templateStyles = null;
            Style[] outputTemplateStyles;

            switch (verb.Index)
            {
                case PaneTemplates:
                    templateNames = PaneTemplateNames;
                    outputTemplateStyles = new Style[3];
                    outputTemplateStyles[IdxLeftPaneTemplate] = this.desktopPanes.LeftPaneStyle;
                    outputTemplateStyles[IdxContentPaneTemplate] = this.desktopPanes.ControlStyle;
                    outputTemplateStyles[IdxRightPaneTemplate] = this.desktopPanes.RightPaneStyle;
                    templateStyles = outputTemplateStyles;
                    break;
                case SeparatorTemplates:
                    templateNames = SeparatorTemplateNames;
                    outputTemplateStyles = new Style[2];
                    outputTemplateStyles[IdxHorizontalSeparatorTemplate] = this.desktopPanes.HorizontalSeparatorStyle;
                    outputTemplateStyles[IdxVerticalSeparatorTemplate] = this.desktopPanes.VerticalSeparatorStyle;
                    templateStyles = outputTemplateStyles;
                    break;
            }

            if (teService != null)
            {
                var editingFrame = teService.CreateFrame(
                    this, verb.Text, templateNames, this.desktopPanes.ControlStyle, templateStyles);

                // editingFrame = teService.CreateFrame(this, verb.Text, templateNames);
                return editingFrame;
            }

            return null;
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the <see cref="T:System.Web.UI.Design.HtmlControlDesigner"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// true to release both managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeTemplateVerbs();
                this.desktopPanes = null;
            }

            this.Dispose(disposing);
        }

        /// <summary>
        /// Gets the cached template editing verbs.
        /// </summary>
        /// <returns>
        /// An array of <see cref="T:System.Web.UI.Design.TemplateEditingVerb"/> objects, if any.
        /// </returns>
        [Obsolete(
            "Use of this method is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
        protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
        {
            if (this.templateVerbsDirty)
            {
                this.DisposeTemplateVerbs();

                this.templateVerbs = new TemplateEditingVerb[2];
                this.templateVerbs[PaneTemplates] = new TemplateEditingVerb("PaneTemplates", PaneTemplates, this);
                this.templateVerbs[SeparatorTemplates] = new TemplateEditingVerb(
                    "SeparatorTemplates", SeparatorTemplates, this);
                this.templateVerbsDirty = false;
            }

            return this.templateVerbs;
        }

        /// <summary>
        /// As with any other designer, 
        ///   override the GetEmptyDesignTimeHtml.
        ///   Gets the HTML used to represent 
        ///   an empty template-based control at design time.
        /// </summary>
        /// <returns>
        /// The get empty design time html.
        /// </returns>
        protected override string GetEmptyDesignTimeHtml()
        {
            var text = this.CanEnterTemplateMode
                           ? "Right click and choose a set of templates to edit their content."
                           : "Switch to HTML view to edit the control's templates.";

            return this.CreatePlaceHolderDesignTimeHtml(text);
        }

        /// <summary>
        /// The dispose template verbs.
        /// </summary>
        private void DisposeTemplateVerbs()
        {
            if (this.templateVerbs == null)
            {
                return;
            }

            foreach (var t in this.templateVerbs)
            {
                t.Dispose();
            }

            this.templateVerbs = null;
            this.templateVerbsDirty = true;
        }

        #endregion
    }
}