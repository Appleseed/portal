using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    // This is a control found on the web but unfortunately I've lost the site address. If found please add here to give credit.
    /// <summary>
    /// You can place this control an an aspx page (DesktopDefault.aspx for example) and it will retain scroll position on postback
    /// </summary>
    public class SmartScroller : Control
    {
        private HtmlForm m_theForm = new HtmlForm();

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartScroller"/> class.
        /// </summary>
        public SmartScroller()
        {
        }

        /// <summary>
        /// Gets the server form.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        private HtmlForm GetServerForm(ControlCollection parent)
        {
            foreach (Control child in parent)
            {
                Type t = child.GetType();
                if (t == typeof (HtmlForm))
                    return (HtmlForm) child;

                if (t == typeof (CustomForm))
                    return (CustomForm) child;

                if (child.HasControls())
                    return GetServerForm(child.Controls);
            }

            return new HtmlForm();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            m_theForm = GetServerForm(Page.Controls);

            HtmlInputHidden hidScrollLeft = new HtmlInputHidden();
            hidScrollLeft.ID = "scrollLeft";

            HtmlInputHidden hidScrollTop = new HtmlInputHidden();
            hidScrollTop.ID = "scrollTop";

            Controls.Add(hidScrollLeft);
            Controls.Add(hidScrollTop);

            string scriptString =
                @"
<script language = ""javascript"">
<!--
  function smartScroller_GetCoords()
  {
    var scrollX, scrollY;
    if (document.all)
    {
      if (!document.documentElement.scrollLeft)
        scrollX = document.body.scrollLeft;
      else
        scrollX = document.documentElement.scrollLeft;

      if (!document.documentElement.scrollTop)
        scrollY = document.body.scrollTop;
      else
        scrollY = document.documentElement.scrollTop;
    }
    else
    {
      scrollX = window.pageXOffset;
      scrollY = window.pageYOffset;
    }
    document.forms[""" +
                m_theForm.ClientID + @"""]." + hidScrollLeft.ClientID + @".value = scrollX;
    document.forms[""" +
                m_theForm.ClientID + @"""]." + hidScrollTop.ClientID +
                @".value = scrollY;
  }


  function smartScroller_Scroll()
  {
    var x = document.forms[""" +
                m_theForm.ClientID + @"""]." + hidScrollLeft.ClientID + @".value;
    var y = document.forms[""" +
                m_theForm.ClientID + @"""]." + hidScrollTop.ClientID +
                @".value;
    window.scrollTo(x, y);
  }

  
  window.onload = smartScroller_Scroll;
  window.onscroll = smartScroller_GetCoords;
  window.onclick = smartScroller_GetCoords;
  window.onkeypress = smartScroller_GetCoords;
// -->
</script>";


            Page.ClientScript.RegisterStartupScript(GetType(), "SmartScroller", scriptString);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            Page.VerifyRenderingInServerForm(this);
            base.Render(writer);
        }
    }
}