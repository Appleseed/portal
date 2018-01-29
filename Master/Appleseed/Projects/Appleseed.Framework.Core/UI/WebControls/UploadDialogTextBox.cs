using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// UploadDialogTextBox
    /// </summary>
    public class UploadDialogTextBox : TextBox
    {
        /// <summary>
        /// Constructor
        /// Set default values
        /// </summary>
        public UploadDialogTextBox()
        {
            m_Value = " ... ";
            m_UploadDirectory = "images/";
            onClick = string.Empty;
            link = string.Empty;
            m_FileNameOnly = false;
            m_MaxWidth = -1;
            m_MaxHeight = -1;
            m_TotalWidth = -1;
            m_TotalHeight = -1;
            m_MinWidth = -1;
            m_MinHeight = -1;
            m_MaxBytes = -1;
            m_FileTypes = "jpg,gif,png";
            m_AllowUpload = true;
            m_ShowUploadFirst = false;
            m_AllowDelete = false;
            m_AllowEdit = false;
            m_AllowCreateDirectory = false;
            m_AllowEditTextBox = false;
            m_AllowRename = false;
            m_ReturnFunction = string.Empty;
            m_PreselectedFile = string.Empty;
            m_FontName = "Verdana, Helvetica, Sans";
            m_FontSize = "11px";
            m_DemoMode = false;
            m_DataStore = "Session";
            m_ShowExceptions = false;
        }


        /// <summary>
        /// Gets the upload path.
        /// </summary>
        /// <value>The upload path.</value>
        private string uploadPath
        {
            get
            {
                string appPath;
                //Build the relative Application Path
                if (HttpContext.Current != null)
                {
                    HttpRequest req = HttpContext.Current.Request;
                    appPath =
                        (req.ApplicationPath == "/")
                            ? string.Empty
                            : req.ApplicationPath;
                }
                else
                {
                    appPath = "/";
                }
                return string.Concat(appPath, "/UploadDialog.aspx");
            }
        }

        /// <summary>
        /// Registers client script for generating postback events prior to rendering on the client, if <see cref="P:System.Web.UI.WebControls.TextBox.AutoPostBack"></see> is true.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            string[] textArray1;

            if (string.Empty.Equals(m_ReturnFunction))
            {
                link = uploadPath + "?Id=" + ID;
                textArray1 =
                    new string[7]
                        {
                            "var host = window.open('" + uploadPath + "?Id=", ID, "' + ((document.getElementById('",
                            ClientID, "').value!='') ? '&selectedfile=' + document.getElementById('", ClientID,
                            "').value : ''), 'browse', 'width=760,height=400,status=yes,resizable=yes'); host.focus();"
                        };
                onClick = string.Concat(textArray1);
                if (!m_AllowEditTextBox)
                {
                    Attributes.Add("onkeydown", "return false;");
                    Attributes.Add("onClick", "this.blur();" + onClick);
                }
            }
            else
            {
                link = uploadPath + "?Id=" + m_ReturnFunction;
                textArray1 =
                    new string[5]
                        {
                            "var host = window.open('" + uploadPath + "?Id=", m_ReturnFunction, "&selectedfile=",
                            m_PreselectedFile,
                            "', 'browse', 'width=760,height=400,status=yes,resizable=yes'); host.focus();"
                        };
                onClick = string.Concat(textArray1);
            }
        }

        /// <summary>
        /// Registers the session.
        /// </summary>
        private void RegisterSession()
        {
            if (HttpContext.Current == null)
                return; //design time

            if (HttpContext.Current.Session == null)
                throw new ApplicationException("The session is disabled, please enable before continue",
                                               new NullReferenceException()); //session is off

            Hashtable hashtable1 = new Hashtable();
            if (string.Empty.Equals(m_ReturnFunction))
            {
                hashtable1.Add("ControlToFill", ClientID);
                hashtable1.Add("ReturnFunction", m_ReturnFunction);
            }
            else
            {
                hashtable1.Add("ControlToFill", m_ReturnFunction);
                hashtable1.Add("ReturnFunction", m_ReturnFunction);
            }
            hashtable1.Add("ShowExceptions", m_ShowExceptions);
            hashtable1.Add("UploadDirectory", m_UploadDirectory);
            hashtable1.Add("AllowUpload", m_AllowUpload.ToString());
            hashtable1.Add("ShowUploadFirst", m_ShowUploadFirst.ToString());
            hashtable1.Add("MaxWidth", MaxWidth.ToString());
            hashtable1.Add("MinWidth", m_MinWidth.ToString());
            hashtable1.Add("MaxHeight", m_MaxHeight.ToString());
            hashtable1.Add("MinHeight", m_MinHeight.ToString());
            hashtable1.Add("MaxBytes", m_MaxBytes.ToString());
            hashtable1.Add("AllowEdit", m_AllowEdit.ToString());
            hashtable1.Add("AllowDelete", m_AllowDelete.ToString());
            hashtable1.Add("AllowCreateDirectory", m_AllowCreateDirectory.ToString());
            hashtable1.Add("AllowRename", m_AllowRename.ToString());
            hashtable1.Add("FileTypes", m_FileTypes);
            hashtable1.Add("FileNameOnly", m_FileNameOnly.ToString());
            hashtable1.Add("DemoMode", m_DemoMode.ToString());
            hashtable1.Add("FontName", m_FontName);
            hashtable1.Add("FontSize", m_FontSize);
            if (m_DataStore == "Session")
            {
                if (string.Empty.Equals(m_ReturnFunction))
                {
                    HttpContext.Current.Session["UpldDlg" + ID] = hashtable1;
                }
                else
                {
                    HttpContext.Current.Session["UpldDlg" + m_ReturnFunction] = hashtable1;
                }
            }
            else if (m_DataStore == "Application")
            {
                if (string.Empty.Equals(m_ReturnFunction))
                {
                    HttpContext.Current.Application["UpldDlg" + ID] = hashtable1;
                }
                else
                {
                    HttpContext.Current.Application["UpldDlg" + m_ReturnFunction] = hashtable1;
                }
            }
        }

        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.WebControls.TextBox"></see> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter"></see> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> that receives the rendered output.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RegisterSession();
            base.Render(writer);
            //Button myButton = new Button();
            //myButton.Text = m_Value;
            //myButton.Attributes.Add("OnClick", onClick);
            //myButton.Width = m_buttonWidth;
            //myButton.RenderControl(writer);

            HyperLink link = new HyperLink();
            link.Text = m_Value;
            link.NavigateUrl = "javascript:;";
            link.CssClass = "CommandButton";
            link.Attributes.Add("OnClick", onClick);
            link.Width = m_buttonWidth;
            link.RenderControl(writer);
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [allow create directory].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [allow create directory]; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue("false"), Description("Gets/sets the AllowCreateDirectory."), Bindable(true), Category("Data")]
        public bool AllowCreateDirectory
        {
            get { return m_AllowCreateDirectory; }
            set { m_AllowCreateDirectory = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        [Description("Gets/sets the AllowDelete."), DefaultValue("false"), Bindable(true), Category("Data")]
        public bool AllowDelete
        {
            get { return m_AllowDelete; }
            set { m_AllowDelete = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow edit].
        /// </summary>
        /// <value><c>true</c> if [allow edit]; otherwise, <c>false</c>.</value>
        [Category("Data"), Description("Gets/sets the AllowEdit."), Bindable(true), DefaultValue("false")]
        public bool AllowEdit
        {
            get { return m_AllowEdit; }
            set { m_AllowEdit = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow edit text box].
        /// </summary>
        /// <value><c>true</c> if [allow edit text box]; otherwise, <c>false</c>.</value>
        [Description("Gets/sets the AllowEditTextBox.")]
        public bool AllowEditTextBox
        {
            get { return m_AllowEditTextBox; }
            set { m_AllowEditTextBox = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow rename].
        /// </summary>
        /// <value><c>true</c> if [allow rename]; otherwise, <c>false</c>.</value>
        [Description("Gets/sets the AllowRename.")]
        public bool AllowRename
        {
            get { return m_AllowRename; }
            set { m_AllowRename = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow upload].
        /// </summary>
        /// <value><c>true</c> if [allow upload]; otherwise, <c>false</c>.</value>
        [Description("Gets/sets the AllowUpload."), Bindable(true), Category("Data"), DefaultValue("true")]
        public bool AllowUpload
        {
            get { return m_AllowUpload; }
            set { m_AllowUpload = value; }
        }

        /// <summary>
        /// Gets or sets the data store.
        /// </summary>
        /// <value>The data store.</value>
        [Description("Gets/sets the DataStore.[Session|Application|Config]")]
        public string DataStore
        {
            get { return m_DataStore; }
            set { m_DataStore = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [demo mode].
        /// </summary>
        /// <value><c>true</c> if [demo mode]; otherwise, <c>false</c>.</value>
        [DefaultValue("false"), Bindable(true), Description("Gets/sets the DemoMode."), Category("Appearance")]
        public bool DemoMode
        {
            get { return m_DemoMode; }
            set { m_DemoMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [file name only].
        /// </summary>
        /// <value><c>true</c> if [file name only]; otherwise, <c>false</c>.</value>
        [Bindable(true), DefaultValue("false"), Description("Gets/sets the FileNameOnly."), Category("Data")]
        public bool FileNameOnly
        {
            get { return m_FileNameOnly; }
            set { m_FileNameOnly = value; }
        }


        /// <summary>
        /// Gets or sets the file types.
        /// </summary>
        /// <value>The file types.</value>
        [Description("Sets the FileTypes."), Bindable(true), Category("Data"), DefaultValue("jpg,gif,png")]
        public string FileTypes
        {
            get { return m_FileTypes; }
            set { m_FileTypes = value; }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        [Description("Gets/sets the FontName.")]
        public string FontName
        {
            get { return m_FontName; }
            set { m_FontName = value; }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>The size of the font.</value>
        [Description("Gets/sets the FontSize.")]
        public string FontSize
        {
            get { return m_FontSize; }
            set { m_FontSize = value; }
        }

        /// <summary>
        /// Gets the javascript link.
        /// </summary>
        /// <value>The javascript link.</value>
        [Description("Gets the JavascriptLink.")]
        public string JavascriptLink
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    RegisterSession();
                    return onClick;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the link.
        /// </summary>
        /// <value>The link.</value>
        [Description("Gets/sets the Link.")]
        public string Link
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    RegisterSession();
                    return link;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the max bytes.
        /// </summary>
        /// <value>The max bytes.</value>
        [Description("Gets/sets the MaxBytes."), Category("Data"), DefaultValue("-1"), Bindable(true)]
        public int MaxBytes
        {
            get { return m_MaxBytes; }
            set { m_MaxBytes = value; }
        }

        /// <summary>
        /// Gets or sets the height of the max.
        /// </summary>
        /// <value>The height of the max.</value>
        [Description("Gets/sets the MaxHeight."), Bindable(true), Category("Data"), DefaultValue("-1")]
        public int MaxHeight
        {
            get { return m_MaxHeight; }
            set { m_MaxHeight = value; }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <value>The width of the max.</value>
        [Bindable(true), Description("Gets/sets the MaxWidth."), Category("Data"), DefaultValue("-1")]
        public int MaxWidth
        {
            get { return m_MaxWidth; }
            set { m_MaxWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height of the min.
        /// </summary>
        /// <value>The height of the min.</value>
        [Category("Data"), Description("Gets/sets the MinHeight."), DefaultValue("-1"), Bindable(true)]
        public int MinHeight
        {
            get { return m_MinHeight; }
            set { m_MinHeight = value; }
        }

        /// <summary>
        /// Gets or sets the width of the min.
        /// </summary>
        /// <value>The width of the min.</value>
        [Bindable(true), Category("Data"), Description("Gets/sets the MinWidth."), DefaultValue("-1")]
        public int MinWidth
        {
            get { return m_MinWidth; }
            set { m_MinWidth = value; }
        }

        /// <summary>
        /// Gets or sets the preselected file.
        /// </summary>
        /// <value>The preselected file.</value>
        [Description("Gets/sets the PreselectedFile.")]
        public string PreselectedFile
        {
            get { return m_PreselectedFile; }
            set { m_PreselectedFile = value; }
        }

        /// <summary>
        /// Gets or sets the return function.
        /// </summary>
        /// <value>The return function.</value>
        [Description("Gets/sets the ReturnFunction.")]
        public string ReturnFunction
        {
            get { return m_ReturnFunction; }
            set
            {
                m_ReturnFunction = value.Replace("()", string.Empty);
                RegisterSession();
                string[] textArray1 =
                    new string[5]
                        {
                            "var host = window.open('" + uploadPath + "?Id=", m_ReturnFunction, "&selectedfile=",
                            m_PreselectedFile,
                            "', 'browse', 'width=700,height=400,status=yes,resizable=yes'); host.focus();"
                        };
                onClick = string.Concat(textArray1);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show exceptions].
        /// </summary>
        /// <value><c>true</c> if [show exceptions]; otherwise, <c>false</c>.</value>
        [Description("Gets/sets the ShowExceptions.")]
        public bool ShowExceptions
        {
            get { return m_ShowExceptions; }
            set { m_ShowExceptions = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show upload first].
        /// </summary>
        /// <value><c>true</c> if [show upload first]; otherwise, <c>false</c>.</value>
        [DefaultValue("false"), Category("Appearance"), Description("Gets/sets the ShowUploadFirst."), Bindable(true)]
        public bool ShowUploadFirst
        {
            get { return m_ShowUploadFirst; }
            set { m_ShowUploadFirst = value; }
        }

        /// <summary>
        /// Gets or sets the total height.
        /// </summary>
        /// <value>The total height.</value>
        [Bindable(true), DefaultValue("-1"), Description("Gets/sets the TotalHeight."), Category("Data")]
        public int TotalHeight
        {
            get { return m_TotalHeight; }
            set
            {
                m_TotalHeight = value;
                m_MinHeight = value;
                m_MaxHeight = value;
            }
        }


        /// <summary>
        /// Gets or sets the total width.
        /// </summary>
        /// <value>The total width.</value>
        [DefaultValue("-1"), Description("Gets/sets the TotalWidth."), Category("Data"), Bindable(true)]
        public int TotalWidth
        {
            get { return m_TotalWidth; }
            set
            {
                m_TotalWidth = value;
                m_MinWidth = value;
                m_MaxWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the upload directory.
        /// </summary>
        /// <value>The upload directory.</value>
        [Category("Data"), DefaultValue("images/"), Description("Gets/sets the UploadDirectory."), Bindable(true)]
        public string UploadDirectory
        {
            get { return m_UploadDirectory; }
            set
            {
                if (value.Length > 0)
                {
                    if ((value.Substring(0, 1) != "/") && (value.Substring(0, 1) != "."))
                    {
                        if (HttpContext.Current != null)
                        {
                            m_UploadDirectory = HttpContext.Current.Request.ApplicationPath +
                                                ((HttpContext.Current.Request.ApplicationPath != "/")
                                                     ? "/"
                                                     : string.Empty) + value;
                        }
                        else
                        {
                            m_UploadDirectory = value;
                        }
                    }
                    else
                    {
                        m_UploadDirectory = value;
                    }
                }
                else
                {
                    m_UploadDirectory = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Description("Gets/sets the Value."), Bindable(true), Category("Appearance"), DefaultValue(" ... ")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        /// <summary>
        /// Gets or sets the width of the Web server control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.Unit"></see> that represents the width of the control. The default is <see cref="F:System.Web.UI.WebControls.Unit.Empty"></see>.</returns>
        /// <exception cref="T:System.ArgumentException">The width of the Web server control was set to a negative value. </exception>
        [Description("Gets/sets the Width."), Bindable(true), Category("Appearance")]
        public override Unit Width
        {
            get { return new Unit(base.Width.Value + m_buttonWidth.Value); }
            set
            {
                if (value.Value >= m_buttonWidth.Value)
                    base.Width = new Unit(value.Value - m_buttonWidth.Value);
                else
                    base.Width = 0;
            }
        }

        #endregion

        #region Declerations

        private Unit m_buttonWidth = new Unit(25);
        private string link;
        private bool m_AllowCreateDirectory;
        private bool m_AllowDelete;
        private bool m_AllowEdit;
        private bool m_AllowEditTextBox;
        private bool m_AllowRename;
        private bool m_AllowUpload;
        private string m_DataStore;
        private bool m_DemoMode;
        private bool m_FileNameOnly;
        private string m_FileTypes;
        private string m_FontName;
        private string m_FontSize;
        private int m_MaxBytes;
        private int m_MaxHeight;
        private int m_MaxWidth;
        private int m_MinHeight;
        private int m_MinWidth;
        private string m_PreselectedFile;
        private string m_ReturnFunction;
        private bool m_ShowExceptions;
        private bool m_ShowUploadFirst;
        private int m_TotalHeight;
        private int m_TotalWidth;
        private string m_UploadDirectory;
        private string m_Value;
        private string onClick;

        #endregion
    }
}