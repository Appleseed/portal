using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// ZenLayout is an ASP.NET WebControl which opens up a whole new way of specifying and building Appleseeds pages and sites. It is the principal element used to construct
    /// a Design Layout for a Appleseed web site. It accepts up to five templates, only one of which (CenterColTemplate) is effectively required. Since
    /// the other four (HeaderTemplate, LeftColTemplate, RightColTemplate and FooterTemplate) are optional, this gives tremendous flexibility
    /// in page layout. A page can consist of just a CenterColTemplate, plus optionally any combination of the other templates.
    /// For example:<br/>
    /// <code>
    /// &lt;zen:ZenLayout&gt;
    ///		&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///		&lt;CenterColTemplate&gt;...&lt;/CenterColTemplate&gt;
    /// &lt;/zen:ZenLayout&gt;
    /// </code>
    /// would produce a page with just two columns, with no header and no footer.<br/>
    /// To produce a 'classic Appleseed' page with header, three columns and footer, you would use:<br/>
    /// <code>
    /// &lt;zen:ZenLayout&gt;
    ///		&lt;HeaderTemplate&gt;...&lt;/HeaderTemplate&gt;
    ///		&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///		&lt;CenterColTemplate&gt;...&lt;/CenterColTemplate&gt;
    ///		&lt;RightColTemplate&gt;...&lt;/RightColTemplate&gt;
    ///		&lt;FooterTemplate&gt;...&lt;/FooterTemplate&gt;
    /// &lt;/zen:ZenLayout&gt;
    /// </code>
    /// Each template can contain any number of Zen, Appleseed, ASP.NET or third-party WebControls or even 'fixed' HTML elements and text. 
    /// There are currently four new WebControls provided with Zen:
    /// <list type="table">
    ///		<item>
    ///			<term>ZenContent</term>
    ///			<description>
    ///			The ZenContent control is used to fill it's containing Template area with Appleseed Modules. The 'Content'
    ///			attribute tells Zen which PaneName to look for. For example, if the 'Content' attribute is set to 'RightPane' then the parent 
    ///			template will receive any modules that are marked 'RightPane' in the Appleseed configuration for the current tab. It stands to reason that
    ///			this ZenContent control should be enclosed within the RightColTemplate. This would be so for current 'normal' Appleseed behaviour. Nevertheless,
    ///			the pane names used by Zen are not hard-coded. Zen reads through the current tab configuration and simply creates a list of PaneName values
    ///			that it finds (with a count for each). Currently it is only ever going to find LeftPane, ContentPane and RightPane since those are the only values that Appleseed
    ///			can currently assign. But Zen will keep up with Appleseed's development automatically: as soon as there is a way to assign other values to PaneName (e.g. 'Header', 'Footer' or even
    ///			'LowerRightAdvertisingPanel') then Zen will 'detect' the new value and allow you to fill a template area with those modules by specifying: Content=&quot;LowerRightAdvertisingPanel&quot;.
    ///			Until that is possible, the only valid values for the 'Content' attribute are: LeftPane, ContentPane or RightPane.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term>ZenHeaderTitle</term>
    ///			<description>
    ///			The ZenHeaderTitle control replaces both HeaderTitle and HeaderImage controls. It creates a specific HTML
    ///			structure:
    ///			<code>&lt;h1 id=&quot;portaltitle&quot; class=&quot;portaltitle&quot;&gt;Appleseed Portal&lt;span&gt;&lt;/span&gt;&lt;/h1&gt;</code>
    ///			This HTML is intended to be 'worked on' by a specific CSS technique called 'Levin Image Replacement'. You'll notice that
    ///			there is no reference to the HeaderImage in the HTML output: the image is specified in the current Theme's CSS and effectively
    ///			'overlays' the text, thus hiding it. See details of this elsewhere in the Zen documentation.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term>ZenHeaderMenu</term>
    ///			<description>
    ///			The ZenHeaderMenu control inherits from the regular Appleseed.HeaderMenu and overrides its Render to produce a Zen-specific HTML
    ///			output:
    ///			<code>
    ///				&lt;div class=&quot;...&quot;&gt;
    ///					&lt;ul class=&quot;zen-hdrmenu-btns&quot;&gt;
    ///						&lt;li&gt;&lt;a href='...'&gt;...&lt;/a&gt;&lt;/li&gt;
    ///						&lt;li&gt;&lt;a href='...'&gt;...&lt;/a&gt;&lt;/li&gt;
    ///					&lt;/ul&gt;
    ///				&lt;/div&gt;
    ///				&lt;div class=&quot;...&quot;&gt;
    ///					&lt;ul class=&quot;zen-hdrmenu-labels&quot;&gt;
    ///						&lt;li&gt;...&lt;/li&gt;
    ///					&lt;/ul&gt;
    ///				&lt;/div&gt;
    ///			</code>
    ///			Notice that link items and plain-text items are separated so they can be treated individually by the CSS. The CSS Class
    ///			for each &lt;div&gt; can be set using the ButtonsCssClass and LabelsCssClass attributes of ZenHeaderMenu. Through inheritance, the 
    ///			normal range of Appleseed attributes can also be set (e.g. ShowLogon, ShowSaveDesktop, etc.).
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<term>ZenNavigation</term>
    ///			<description>
    ///			The ZenNavigation control implements the Appleseed INavigation interface to produce horizontal or vertical menus. The actual output is a nested
    ///			&lt;ul&gt; (unordered list). Through the magic of CSS, this can be displayed as a drop-down or pull-out menu. This feature is still a little experimental and may change
    ///			in the near future: I'm experimenting with CSS techniques and may need to add more 'class' attributes within the output structure.
    ///			</description>
    ///		</item>
    /// </list>
    /// The output of the ZenLayout control is a complex hierarchy of &lt;div&gt; elements, within which will be contained any other controls specified in the Layout. The &lt;div&gt; hierarchy has a number
    /// of specific features:
    /// <list type="bullet">
    ///		<item>
    ///			<description>
    ///			Rather obviously it doesn't contain any &lt;table&gt; structures: the final appearance of the page is entirely
    ///			determined by the CSS applied to it.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<description>
    ///			The 'source order' of the template areas in the output HTML is: Header, CenterColumn, LeftColumn, RightColumn, Footer. This has obvious benefits
    ///			for Search Engine Optimization (SEO), since a spider will read the main content (assumed to be in the center column) before it encounters the left column content. It also greatly 
    ///			improves the 'accessibility' of the page by presenting the page contents in an order which is more logical to ScreenReaders and browsers without CSS capability or with such capability
    ///			disabled. Much of the apparent complexity of the structure is specifically to enable this 'trick'.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<description>
    ///			The HTML elements used to create the page are 'semantically meaningful' (e.g. &lt;h1&gt; for the PortaTitle, &lt;ul&gt; for navigation lists, etc) or 'semantically neutral' when their sole purpose is 
    ///			layout (e.g. &lt;div&gt;). This is essential for true Section 508 or Accessibility Guidelines compliance.
    ///			</description>
    ///		</item>
    ///		<item>
    ///			<description>
    ///			The various CSS classes which are assigned within the hierarchy are always prefixed with 'zen-' in order to ease the process of designing a site
    ///			with CSS alone. Each 'area' of the layout is 'marked' with a specific CSS class name which makes it easy to control the appearance of elements within that area. For example, if you
    ///			want the default paragraph text color in the left column to be 'red' then you'd add the CSS rule:
    ///			<code>.zen-col-left p { color: red }</code>
    ///			</description>
    ///		</item>
    /// </list>
    /// By combining these four controls with the ZenLayout control you have a flexible, extensible means for specifying the structure of a Appleseed page. The page's appearance is entirely controlled through the
    /// application of CSS rules. The page is compact, fast, Search Engine friendly and highly accessible. The only challenge is to learn a new programming language (CSS) in order to make full use of it!
    /// </summary>
    /// <remarks>
    /// But wait! There's more!<br/>
    /// A Zenlayout control may also contain, within any one of its templates, another ZenLayout control. Each ZenLayout has an identifier assigned to it, so each one is individually addressable in the Theme CSS. For example, combine the two examples given earlier:
    /// <code>
    /// &lt;zen:ZenLayout CssID=&quot;myMainLayout&quot;&gt;
    ///		&lt;HeaderTemplate&gt;...&lt;/HeaderTemplate&gt;
    ///		&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///		&lt;CenterColTemplate&gt;
    ///			&lt;zen:ZenLayout CssID=&quot;mySubLayout&quot;&gt;
    ///				&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///				&lt;CenterColTemplate&gt;...&lt;/CenterColTemplate&gt;
    ///			&lt;/zen:ZenLayout&gt;
    ///		&lt;/CenterColTemplate&gt;
    ///		&lt;RightColTemplate&gt;...&lt;/RightColTemplate&gt;
    ///		&lt;FooterTemplate&gt;...&lt;/FooterTemplate&gt;
    /// &lt;/zen:ZenLayout&gt;
    /// </code>
    /// would give you a Header, FOUR columns and a footer plus the means to control it all with CSS.
    /// Embed one 'classic' Appleseed layout in the center column of another, like this:
    /// <code>
    /// &lt;zen:ZenLayout&gt;
    ///		&lt;HeaderTemplate&gt;...&lt;/HeaderTemplate&gt;
    ///		&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///		&lt;CenterColTemplate&gt;
    ///			&lt;zen:ZenLayout&gt;
    ///				&lt;HeaderTemplate&gt;...&lt;/HeaderTemplate&gt;
    ///				&lt;LeftColTemplate&gt;...&lt;/LeftColTemplate&gt;
    ///				&lt;CenterColTemplate&gt;...&lt;/CenterColTemplate&gt;
    ///				&lt;RightColTemplate&gt;...&lt;/RightColTemplate&gt;
    ///				&lt;FooterTemplate&gt;...&lt;/FooterTemplate&gt;
    ///				&lt;/zen:ZenLayout&gt;
    ///		&lt;/CenterColTemplate&gt;
    ///		&lt;RightColTemplate&gt;...&lt;/RightColTemplate&gt;
    ///		&lt;FooterTemplate&gt;...&lt;/FooterTemplate&gt;
    /// &lt;/zen:ZenLayout&gt;
    /// </code>
    /// and you've got a page with Header, LeftColumn, RightColumn and Footer as normal, but now the CenterColumn contains a header and footer
    /// of it's own plus one, two or three 'sub columns'(according to what modules are inserted).<br/>
    /// Of course, Appleseed's admin can't support this yet, but you could poke around in the database and achieve this today. Makes you think, doesn't it?
    /// </remarks>
    public class ZenLayout : WebControl, INamingContainer
    {
        #region private members

        private ITemplate headerTemplate;
        private ITemplate leftColTemplate;
        private ITemplate centerColTemplate;
        private ITemplate rightColTemplate;
        private ITemplate footerTemplate;

        private HtmlGenericControl zenMain;
        private HtmlGenericControl zenHdr;
        private HtmlGenericControl zenCols;
        private HtmlGenericControl zenColsWrapper;
        private HtmlGenericControl zenFloatWrapper;
        private HtmlGenericControl zenLeftCol;
        private HtmlGenericControl zenCenterCol;
        private HtmlGenericControl zenRightCol;
        private HtmlGenericControl zenMiddleCol;
        private HtmlGenericControl zenFtr;
        private HtmlGenericControl zenColClear;

        private string cssID = string.Empty;
        private string headerContent = string.Empty;
        private string leftContent = "LeftPane";
        private string centerContent = "ContentPane";
        private string rightContent = "RightPane";
        private string footerContent = string.Empty;
        private bool forceLeft = false;
        private bool forceRight = false;


        private bool showLeft = false;
        private bool showRight = false;

        private const string ZEN_MAIN_CSS = "zen-main";
        private const string ZEN_HDR_CSS = "zen-hdr";
        private const string ZEN_COLS_CSS = "zen-cols";
        private const string ZEN_FTR_CSS = "zen-ftr";
        private const string ZEN_SHOW_ALL_CSS = "zen-show-all";
        private const string ZEN_HIDE_LEFT_CSS = "zen-hide-left";
        private const string ZEN_HIDE_RIGHT_CSS = "zen-hide-right";
        private const string ZEN_HIDE_BOTH_CSS = "zen-hide-both";
        private const string ZEN_COLS_WRAPPER_CSS = "zen-cols-wrapper";
        private const string ZEN_FLOAT_WRAPPER_CSS = "zen-float-wrapper";
        private const string ZEN_LEFT_COL_CSS = "zen-col-left";
        private const string ZEN_CENTER_COL_CSS = "zen-col-center";
        private const string ZEN_RIGHT_COL_CSS = "zen-col-right";
        private const string ZEN_MIDDLE_COL_CSS = "zen-col-middle";
        private const string ZEN_COL_CLEAR_CSS = "zen-clear";
        private const string ZEN_COL_CLEAR_ID = "zen-em";

        #endregion

        #region properties

        /// <summary>
        /// A string which will be added as the element ID of the outer DIV of this layout.
        /// The ID can then be used as a selector by the Theme CSS to limit the scope of a
        /// CSS rule to only this layout.
        /// </summary>
        /// <value>The CSS ID.</value>
        /// <remarks>
        /// Remember that CSS is CASE SENSITIVE (contrary to popular belief!), so use the exact case
        /// of whatever value you set here when constructing CSS rules.<br/>
        /// OPTIONAL: If no value is set then no ID attribute will be added and the elements within this layout
        /// cannot be addressed exclusively. This is the default behaviour.
        /// </remarks>
        /// <example>
        /// Set CssID to 'myLayout1'. Then a CSS rule like: <br/>
        /// 	<code>#myLayout1 p{color:red}</code>
        /// 	<br/>
        /// would make 'red' the default paragraph text color within that layout.<br/>
        /// </example>
        public string CssID
        {
            get { return cssID; }
            set { cssID = value.Trim(); }
        }

        /// <summary>
        /// Tells the layout which PaneName to expect as Header content within this layout
        /// </summary>
        /// <value>The content of the header.</value>
        /// <remarks>
        /// There is no default value. Value is case insensitive.
        /// </remarks>
        public string HeaderContent
        {
            get { return headerContent; }
            set { headerContent = value.Trim().ToLower(); }
        }

        /// <summary>
        /// Tells the layout which PaneName to expect as LeftColumn content within this layout
        /// </summary>
        /// <value>The content of the left.</value>
        /// <remarks>
        /// Defaults to 'LeftPane' (the Duemetri.ThreePanes value for left column). Value is case insensitive.
        /// </remarks>
        public string LeftContent
        {
            get { return leftContent; }
            set { leftContent = value.Trim().ToLower(); }
        }

        /// <summary>
        /// Tells the layout which PaneName to expect as CenterColumn content within this layout
        /// </summary>
        /// <value>The content of the center.</value>
        /// <remarks>
        /// Defaults to 'ContentPane' (the Duemetri.ThreePanes value for center column). Value is case insensitive.
        /// </remarks>
        public string CenterContent
        {
            get { return centerContent; }
            set { centerContent = value.Trim().ToLower(); }
        }

        /// <summary>
        /// Tells the layout which PaneName to expect as RightColumn content within this layout
        /// </summary>
        /// <value>The content of the right.</value>
        /// <remarks>
        /// Defaults to 'RightPane' (the Duemetri.ThreePanes value for right column). Value is case insensitive.
        /// </remarks>
        public string RightContent
        {
            get { return rightContent; }
            set { rightContent = value.Trim().ToLower(); }
        }

        /// <summary>
        /// Tells the layout which PaneName to expect as Footer content within this layout
        /// </summary>
        /// <value>The content of the footer.</value>
        /// <remarks>
        /// There is no default value. Value is case insensitive.
        /// </remarks>
        public string FooterContent
        {
            get { return footerContent; }
            set { footerContent = value.Trim().ToLower(); }
        }

        /// <summary>
        /// By default, Zen will 'hide' the left column if it is empty. Set ForceLeft to 'true' to force Zen to display an empty column.
        /// </summary>
        /// <value><c>true</c> if [force left]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Default value is 'false'.
        /// </remarks>
        public bool ForceLeft
        {
            get { return forceLeft; }
            set { forceLeft = value; }
        }

        /// <summary>
        /// By default, Zen will 'hide' the right column if it is empty. Set ForceRight to 'true' to force Zen to display an empty column.
        /// </summary>
        /// <value><c>true</c> if [force right]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Default value is 'false'.
        /// </remarks>
        public bool ForceRight
        {
            get { return forceRight; }
            set { forceRight = value; }
        }

        #endregion

        #region controls

        /// <summary>
        /// ZenMain control
        /// </summary>
        /// <value>The zen main.</value>
        public HtmlGenericControl ZenMain
        {
            get
            {
                if (zenMain == null)
                {
                    zenMain = new HtmlGenericControl("div");
                    zenMain.Attributes.Add("class", ZEN_MAIN_CSS);
                    if (CssID != string.Empty)
                        zenMain.Attributes.Add("id", CssID);
                }
                return zenMain;
            }
        }

        /// <summary>
        /// ZenHdr control
        /// </summary>
        /// <value>The zen HDR.</value>
        public HtmlGenericControl ZenHdr
        {
            get
            {
                if (zenHdr == null && HeaderTemplate != null)
                {
                    zenHdr = new HtmlGenericControl("div");
                    zenHdr.Attributes.Add("class", ZEN_HDR_CSS);
                }
                return zenHdr;
            }
        }

        /// <summary>
        /// ZenCols control
        /// </summary>
        /// <value>The zen cols.</value>
        public HtmlGenericControl ZenCols
        {
            get
            {
                if (zenCols == null)
                {
                    zenCols = new HtmlGenericControl("div");
                    zenCols.Attributes.Add("class", ZEN_COLS_CSS);
                }
                return zenCols;
            }
        }

        /// <summary>
        /// ZenColsWrapper control
        /// </summary>
        /// <value>The zen cols wrapper.</value>
        public HtmlGenericControl ZenColsWrapper
        {
            get
            {
                if (zenColsWrapper == null)
                {
                    zenColsWrapper = new HtmlGenericControl("div");
                    zenColsWrapper.Attributes.Add("class", ZEN_COLS_WRAPPER_CSS);
                }
                return zenColsWrapper;
            }
        }

        /// <summary>
        /// ZenFloatWrapper control
        /// </summary>
        /// <value>The zen float wrapper.</value>
        public HtmlGenericControl ZenFloatWrapper
        {
            get
            {
                if (zenFloatWrapper == null)
                {
                    zenFloatWrapper = new HtmlGenericControl("div");
                    zenFloatWrapper.Attributes.Add("class", ZEN_FLOAT_WRAPPER_CSS);
                }
                return zenFloatWrapper;
            }
        }

        /// <summary>
        /// ZenLeftCol control
        /// </summary>
        /// <value>The zen left col.</value>
        public HtmlGenericControl ZenLeftCol
        {
            get
            {
                if (zenLeftCol == null)
                {
                    zenLeftCol = new HtmlGenericControl("div");
                    zenLeftCol.Attributes.Add("class", ZEN_LEFT_COL_CSS);
                }
                return zenLeftCol;
            }
        }

        /// <summary>
        /// ZenCenterCol control
        /// </summary>
        /// <value>The zen center col.</value>
        public HtmlGenericControl ZenCenterCol
        {
            get
            {
                if (zenCenterCol == null)
                {
                    zenCenterCol = new HtmlGenericControl("div");
                    zenCenterCol.Attributes.Add("class", ZEN_CENTER_COL_CSS);
                }
                return zenCenterCol;
            }
        }

        /// <summary>
        /// ZenRightCol control
        /// </summary>
        /// <value>The zen right col.</value>
        public HtmlGenericControl ZenRightCol
        {
            get
            {
                if (zenRightCol == null)
                {
                    zenRightCol = new HtmlGenericControl("div");
                    zenRightCol.Attributes.Add("class", ZEN_RIGHT_COL_CSS);
                }
                return zenRightCol;
            }
        }

        /// <summary>
        /// ZenMiddleCol control
        /// </summary>
        /// <value>The zen middle col.</value>
        public HtmlGenericControl ZenMiddleCol
        {
            get
            {
                if (zenMiddleCol == null)
                {
                    zenMiddleCol = new HtmlGenericControl("div");
                    zenMiddleCol.Attributes.Add("class", ZEN_MIDDLE_COL_CSS);
                }
                return zenMiddleCol;
            }
        }

        /// <summary>
        /// ZenFtr control
        /// </summary>
        /// <value>The zen FTR.</value>
        public HtmlGenericControl ZenFtr
        {
            get
            {
                if (zenFtr == null)
                {
                    zenFtr = new HtmlGenericControl("div");
                    zenFtr.Attributes.Add("class", ZEN_FTR_CSS);
                }
                return zenFtr;
            }
        }

        /// <summary>
        /// ZenColClear control
        /// </summary>
        /// <value>The zen col clear.</value>
        public HtmlGenericControl ZenColClear
        {
            get
            {
                if (zenColClear == null)
                {
                    zenColClear = new HtmlGenericControl("div");
                    zenColClear.Attributes.Add("class", ZEN_COL_CLEAR_CSS);
                    zenColClear.Attributes.Add("id", ZEN_COL_CLEAR_ID);
                }
                return zenColClear;
            }
        }

        #endregion

        #region templates

        /// <summary>
        /// Left Column Template
        /// </summary>
        /// <value>The left col template.</value>
        public virtual ITemplate LeftColTemplate
        {
            get { return leftColTemplate; }
            set { leftColTemplate = value; }
        }

        /// <summary>
        /// Center Column Template
        /// </summary>
        /// <value>The center col template.</value>
        public virtual ITemplate CenterColTemplate
        {
            get { return centerColTemplate; }
            set { centerColTemplate = value; }
        }

        /// <summary>
        /// Right Column Template
        /// </summary>
        /// <value>The right col template.</value>
        public virtual ITemplate RightColTemplate
        {
            get { return rightColTemplate; }
            set { rightColTemplate = value; }
        }

        /// <summary>
        /// Header Template
        /// </summary>
        /// <value>The header template.</value>
        public virtual ITemplate HeaderTemplate
        {
            get { return headerTemplate; }
            set { headerTemplate = value; }
        }

        /// <summary>
        /// Footer Template
        /// </summary>
        /// <value>The footer template.</value>
        public virtual ITemplate FooterTemplate
        {
            get { return footerTemplate; }
            set { footerTemplate = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Creates the Control Hierarchy
        /// </summary>
        private void CreateControlHierarchy()
        {
            //this.Controls.Clear();

            if (HeaderTemplate != null)
                HeaderTemplate.InstantiateIn(ZenHdr);

            if (LeftColTemplate != null)
                LeftColTemplate.InstantiateIn(ZenLeftCol);

            if (CenterColTemplate != null)
                CenterColTemplate.InstantiateIn(ZenCenterCol);

            if (RightColTemplate != null)
                RightColTemplate.InstantiateIn(ZenRightCol);

            if (FooterTemplate != null)
                FooterTemplate.InstantiateIn(ZenFtr);

            HybridDictionary counts = GetModuleCount();

            foreach (DictionaryEntry myArea in counts)
            {
                if (myArea.Key.ToString() == LeftContent)
                    showLeft = true;
                if (myArea.Key.ToString() == RightContent)
                    showRight = true;
            }

            if (ForceLeft)
                showLeft = true;
            if (ForceRight)
                showRight = true;

            // adjust CSS to match column count
            if (showLeft && showRight)
                ZenCols.Attributes.Add("class", string.Concat(ZEN_COLS_CSS, " ", ZEN_SHOW_ALL_CSS));
            else if (showLeft && !showRight)
                ZenCols.Attributes.Add("class", string.Concat(ZEN_COLS_CSS, " ", ZEN_HIDE_RIGHT_CSS));
            else if (!showLeft && showRight)
                ZenCols.Attributes.Add("class", string.Concat(ZEN_COLS_CSS, " ", ZEN_HIDE_LEFT_CSS));
            else if (!showLeft && !showRight)
                ZenCols.Attributes.Add("class", string.Concat(ZEN_COLS_CSS, " ", ZEN_HIDE_BOTH_CSS));

            // build the control structure
            ZenMiddleCol.Controls.Add(ZenCenterCol);
            ZenFloatWrapper.Controls.Add(ZenMiddleCol);
            ZenFloatWrapper.Controls.Add(ZenLeftCol);
            ZenColsWrapper.Controls.Add(ZenFloatWrapper);
            ZenColsWrapper.Controls.Add(ZenRightCol);
            ZenColsWrapper.Controls.Add(ZenColClear);
            ZenCols.Controls.Add(ZenColsWrapper);

            //			if ( this.ZenHdr != null ) 
            //				this.ZenMain.Controls.Add(this.ZenHdr);
            //			this.ZenMain.Controls.Add(this.ZenCols);
            //			if ( this.ZenFtr != null ) 
            //				this.ZenMain.Controls.Add(this.ZenFtr);

            if (HeaderTemplate != null)
                ZenMain.Controls.Add(ZenHdr);
            if (CenterColTemplate != null && LeftColTemplate != null && RightColTemplate != null)
                ZenMain.Controls.Add(ZenCols);
            if (FooterTemplate != null)
                ZenMain.Controls.Add(ZenFtr);


            Controls.Add(ZenMain);
        }

        /// <summary>
        /// Counts the number of modules to be displayed by PaneName
        /// </summary>
        /// <returns>
        /// HybridDictionary containing counts for all PaneNames found
        /// </returns>
        /// <remarks>
        /// Method does not check Cultures setting, so could be fooled into displaying
        /// a column even though it is empty. This needs further investigation.
        /// </remarks>
        private HybridDictionary GetModuleCount()
        {
            //TODO: check Cultures setting?
            HybridDictionary counts = new HybridDictionary(3);
            string _key;

            PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            if (PortalSettings.ActivePage.Modules.Count > 0)
            {
                // Loop through each entry in the configuration system for this tab
                foreach (ModuleSettings _moduleSettings in PortalSettings.ActivePage.Modules)
                {
                    // Ensure that the visiting user has access to view the current module
                    if (PortalSecurity.IsInRoles(_moduleSettings.AuthorizedViewRoles) == true)
                    {
                        _key = _moduleSettings.PaneName.ToLower();
                        if (counts.Contains(_key))
                            counts[_key] = ((int) counts[_key]) + 1;
                        else
                            counts.Add(_key, 1);
                    }
                }
            }
            return counts;
        }

        #endregion

        #region overrides

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.DataBinding"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            EnsureChildControls();
            base.OnDataBinding(e);
        }

        //		/// <summary>
        //		/// Override OnLoad so we can force EnsureChildControls().
        //		/// Note: this is essential so that modules are processed before the Page itself,
        //		/// and each module can contribute to the &lt;head&gt; element (e.g. CSS references).
        //		/// </summary>
        //		/// <param name="e"></param>
        //		protected override void OnLoad(EventArgs e) 
        //		{
        //			EnsureChildControls();
        //			base.OnLoad(e);
        //		}

        /// <summary>
        /// Gets or sets the ie7 script.
        /// </summary>
        /// <value>The ie7 script.</value>
        public string Ie7Script
        {
            get
            {
                if (ie7Script == string.Empty)
                {
                    if (ConfigurationManager.AppSettings["Ie7Script"] != null &&
                        ConfigurationManager.AppSettings["Ie7Script"].ToString() != string.Empty)
                    {
                        ie7Script = ConfigurationManager.AppSettings["Ie7Script"].ToString();
                    }
                }
                return ie7Script;
            }
            set { ie7Script = value; }
        }

        private string ie7Script = string.Empty;

        /// <summary>
        /// This member overrides Control.CreateChildControls
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();
            if (Ie7Script != string.Empty)
            {
                // TODO: need to check registration for this
                if (!((Page) Page).IsAdditionalMetaElementRegistered("ie7"))
                {
                    string _ie7 = string.Empty;
                    string _ie7Part = string.Empty;

                    foreach (string _script in Ie7Script.Split(new char[] {';'}))
                    {
                        _ie7Part = Path.WebPathCombine(Path.ApplicationRoot, _script);
                        _ie7Part =
                            string.Format(
                                "<!--[if lt IE 7]><script src=\"{0}\" type=\"text/javascript\"></script><![endif]-->",
                                _ie7Part);
                        _ie7 += _ie7Part + "\n";
                    }
                    ((Page) Page).RegisterAdditionalMetaElement("ie7", _ie7);
                }
            }

            CreateControlHierarchy();
        }

        /// <summary>
        /// This member overrides Control.Render so we can exclude the useless outer &lt;span&gt; element that
        /// ASP.NET insists on adding.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

        #endregion
    }
}