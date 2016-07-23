using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework.BLL.Utils;
using Appleseed.Framework.Design;
using Appleseed.Framework.Site.Configuration;
//
// This Namespace holds the information to manage the viewable controls;
// namely, min., max. and close buttons
// 

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// This class contains the data 
    /// </summary>
    internal class ViewControlData : IDisposable
    {
        private string name_ = string.Empty; // button name
        private string alt_text_ = string.Empty; // alternate text
        private string localize_ = string.Empty; // localized tag name
        private LinkButton button_ = null; // the control

        /// <summary>
        /// ctor to create the image button
        /// </summary>
        public ViewControlData()
        {
        }

        /// <summary>
        /// ctor to create the image button
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="localize">The localize.</param>
        public ViewControlData(string name, string localize)
            : this(name, localize, localize)
        {
        }

        /// <summary>
        /// Main ctor to create the image button
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="altText">The alt text.</param>
        /// <param name="localize">The localize.</param>
        public ViewControlData(string name, string altText, string localize)
        {
            name_ = name;
            alt_text_ = altText;
            localize_ = localize;
            button_ = new LinkButton();

            // setup the button attributes
            setup();
        } // end of ctor


        /// <summary>
        /// Attribute for the Image Button
        /// </summary>
        /// <value>The view control BTN.</value>
        public LinkButton viewControlBtn
        {
            get { return button_; }
            set { button_ = value; }
        } // end of viewControlBtn

        /// <summary>
        /// Attribute for the Image Button Name
        /// </summary>
        /// <value>The name.</value>
        public string name
        {
            get { return name_; }
            set { name_ = value; }
        } // end of Name

        /// <summary>
        /// Attribute for the Image Button Alternate Text
        /// </summary>
        /// <value>The alternate text.</value>
        public string alternateText
        {
            get { return alt_text_; }
            set { alt_text_ = value; }
        } // end of AlternateText

        /// <summary>
        /// Attribute for the Image Button Localized Text
        /// </summary>
        /// <value>The localized.</value>
        public string localized
        {
            get { return localize_; }
            set { localize_ = value; }
        } // end of AlternateText


        /// <summary>
        /// enable the view state of the button
        /// </summary>
        /// <param name="state">if set to <c>true</c> [state].</param>
        public void enableViewState(bool state)
        {
            if (button_ != null)
            {
                button_.EnableViewState = state;
            }
        } // end of enableViewState

        /// <summary>
        /// enable the visibility  of the button
        /// </summary>
        /// <param name="state">if set to <c>true</c> [state].</param>
        public void visible(bool state)
        {
            if (button_ != null)
            {
                button_.Visible = state;
            }
        } // end of visible

        /// <summary>
        /// Get the html format of the image
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal static string ImageFormat(string name)
        {
            StringBuilder sb = new StringBuilder("<img border=0 height='");
            int width = 0, height = 0;
            string url = Url(name, out width, out height);
            // set the new image
            sb.Append(height.ToString());
            sb.Append("' width='");
            sb.Append(width.ToString());

            // Change for compatibility with others buttons jviladiu@portalservices.net
            // sb.Append("' align=absmiddle src='");
            sb.Append("' src='");
            sb.Append(url);
            sb.Append("'>");

            return sb.ToString();
        } // en dof ImageFormat


        /// <summary>
        /// Determine the Url of the named string
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        internal static string Url(string name, out int width, out int height)
        {
            string url = string.Empty;
            // Obtain PortalSettings from Current Context 
            PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
            try
            {
                // current theme
                Theme CurrentTheme = PortalSettings.GetCurrentTheme();

                //TODO: verify default works in all cases
                Image image = CurrentTheme.GetImage(name, "~/Design/Themes/Default/" + name.Substring(8));
                // dimensions
                width = (int) image.Width.Value;
                height = (int) image.Height.Value;
                // url
                url = image.ImageUrl;
            }
            catch
            {
                width = 0;
                height = 0;
            }

            return url;
        } // end of Url

        /// <summary>
        /// setup the button
        /// </summary>
        private void setup()
        {
            if (button_ != null)
            {
                // url
                string url = ImageFormat(name);
                // if no url, then it's not wanted
                if (url == null || url.Length == 0)
                    button_ = null;
                else
                {
                    // button name used for determining type
                    button_.Attributes["bname"] = name;
                    // tool tip (  use localize string )
                    button_.ToolTip = General.GetString(localized, alternateText, button_);
                    // don't validate
                    button_.CausesValidation = false;
                    // set the image
                    button_.Text = ImageFormat(name);
                }
            }
        } // end of setup

        public void Dispose()
        {
            this.button_.Dispose();
        }
    } // end of ViewControlData

    /// ----------------------------------------------------------------------
    /// <summary>
    /// This manages the collection of view control button data
    /// </summary>
    public class ViewControlManager
    {
        // collection of buttons
        private ArrayList buttons_ = new ArrayList();
        private int tabID_ = -1;
        private int moduleID_ = -1;
        private string rawUrl_ = string.Empty;

        /// <summary>
        /// Default Ctor
        /// </summary>
        /// <param name="tabID">The tab ID.</param>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="rawUrl">The raw URL.</param>
        public ViewControlManager(int tabID
                                  , int moduleID
                                  , string rawUrl
            )
        {
            // what tab 
            tabID_ = tabID;
            // what module
            moduleID_ = moduleID;
            // what url
            rawUrl_ = rawUrl;
        } // end of ctor

        /// <summary>
        /// Returns a the  button, if present
        /// </summary>
        /// <value></value>
        public LinkButton this[string index]
        {
            get
            {
                // find control
                ViewControlData vcd = findControl(index);
                LinkButton btn = null;
                // if present assign it
                if (vcd != null)
                    btn = (LinkButton) vcd.viewControlBtn;
                return btn;
            }
        } // end of LinkButton


        /// <summary>
        /// Return the module ID
        /// </summary>
        /// <value>The module ID.</value>
        public int ModuleID
        {
            get { return moduleID_; }
        }

        /// <summary>
        /// Return the Tab id
        /// </summary>
        /// <value>The page ID.</value>
        public int PageID
        {
            get { return tabID_; }
        }

        /// <summary>
        /// Create the Image Button
        /// </summary>
        /// <param name="name">button name</param>
        /// <param name="local">index</param>
        /// <returns></returns>
        public LinkButton create(string name, string local)
        {
            return create(name, local, local);
        } // end of create

        /// <summary>
        /// Create the Image Button
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="altText">The alt text.</param>
        /// <param name="localize">The localize.</param>
        /// <returns></returns>
        public LinkButton create(string name, string altText, string localize)
        {
            ViewControlData vcd = findControl(name);
            // if present, then overwite
            if (vcd == null)
            {
                // create a new view control
                vcd = new ViewControlData(name, altText, localize);
                // set event handlers
                vcd.viewControlBtn.Click += new EventHandler(Button_Click);
                // set attributes
                vcd.viewControlBtn.Attributes["mID"] = moduleID_.ToString();
                vcd.viewControlBtn.Attributes["tID"] = tabID_.ToString();
                // add to list
                buttons_.Add(vcd);
            }

            return vcd.viewControlBtn;
        } // end of create


        /// <summary>
        /// Find the control data
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        private ViewControlData findControl(string index)
        {
            foreach (ViewControlData vcd in buttons_)
                if (vcd.name == index)
                    return vcd;
            return null;
        } // end of findControl

        /// <summary>
        /// This method gets called when user min/max the button
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="evt">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Button_Click(Object sender, EventArgs evt)
        {
            //// 
            //LinkButton ibut = (LinkButton) sender;
            //WindowStateEnum state = WindowStateEnum.Minimized;
            //string name = ibut.Attributes["bname"];
            //int mID = Int32.Parse(ibut.Attributes["mID"]);
            //int tadID = Int32.Parse(ibut.Attributes["tID"]);

            //// what state are we in
            //if (name != null)
            //{
            //    if (name == WindowStateStrings.ButtonMaxName)
            //    {
            //        state = WindowStateEnum.Maximized;
            //    }
            //    else if (name == WindowStateStrings.ButtonCloseName) // close
            //    {
            //        state = WindowStateEnum.Closed;
            //    }
            //    // set the state of visibility
            //    UserDesktop.UpdateUserDesktop(mID, state, tadID);
            //}

            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
        } // end of Button_Click
    } // end of ViewControlManager
}
