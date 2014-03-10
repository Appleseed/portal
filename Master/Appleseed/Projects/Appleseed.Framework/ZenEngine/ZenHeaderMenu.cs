using System.Collections;
using System.Web.UI;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Summary description for ZenHeaderMenu.
    /// </summary>
    public class ZenHeaderMenu : HeaderMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZenHeaderMenu"/> class.
        /// </summary>
        public ZenHeaderMenu()
        {
        }

        private string _buttonsCssClass;
        private string _labelsCssClass;

        /// <summary>
        /// Gets or sets the buttons CSS class.
        /// </summary>
        /// <value>The buttons CSS class.</value>
        public string ButtonsCssClass
        {
            get { return _buttonsCssClass; }
            set { _buttonsCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the labels CSS class.
        /// </summary>
        /// <value>The labels CSS class.</value>
        public string LabelsCssClass
        {
            get { return _labelsCssClass; }
            set { _labelsCssClass = value; }
        }

        /// <summary>
        /// Overrides Render to produce simple unordered list for Zen
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"></see> that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            ArrayList _labels = new ArrayList();
            ArrayList _buttons = new ArrayList();
            ArrayList items = (ArrayList) DataSource;

            foreach (string item in items)
            {
                if (item.StartsWith("<a"))
                    _buttons.Add(item);
                else
                    _labels.Add(item);
            }

            if (_labels.Count > 0)
            {
                writer.Write("<div class=\"");
                writer.Write(LabelsCssClass);
                writer.Write("\">");
                writer.Write("<ul class=\"zen-hdrmenu-labels\">");
                foreach (string _label in _labels)
                {
                    writer.Write("<li>");
                    writer.Write(_label);
                    writer.Write("</li>");
                }
                writer.Write("</ul>");
                writer.Write("</div>");
            }

            if (_buttons.Count > 0)
            {
                writer.Write("<div class=\"");
                writer.Write(ButtonsCssClass);
                writer.Write("\">");
                writer.Write("<ul class=\"zen-hdrmenu-btns\">");
                foreach (string _button in _buttons)
                {
                    writer.Write("<li>");
                    writer.Write(_button);
                    writer.Write("</li>");
                }
                writer.Write("</ul>");
                writer.Write("</div>");
            }
        }
    }
}