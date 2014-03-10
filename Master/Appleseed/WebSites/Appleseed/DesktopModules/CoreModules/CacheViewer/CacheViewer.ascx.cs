using System;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    public partial class CacheViewer : PortalModuleControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            int rb_cacheCount = 0;
            int rb_cacheSize = 0;
            int rb_cacheTotal = 0;
            int asp_cacheTotal = 0;
            SortedList cacheContents = new SortedList();

            foreach (DictionaryEntry cacheItem in HttpContext.Current.Cache)
            {
                asp_cacheTotal = asp_cacheTotal + cacheItem.Value.ToString().Length;

                if (cacheItem.Key.ToString().StartsWith(Portal.UniqueID))
                {
                    cacheContents.Add(cacheItem.Key.ToString(), cacheItem.Value.ToString());
                }
            }

            Table t = new Table();
            t.CellSpacing = 0;
            t.CellPadding = 4;
            TableRow r;
            TableCell c;
            t.CssClass = "Normal";

            foreach (DictionaryEntry contentsItem in cacheContents)
            {
                rb_cacheSize = contentsItem.Value.ToString().Length;
                rb_cacheCount = rb_cacheCount + 1;
                rb_cacheTotal = rb_cacheTotal + rb_cacheSize;

                r = new TableRow();
                r.BackColor = Color.Gray;
                r.ForeColor = Color.White;
                c = new TableCell();
                c.Text = contentsItem.Key.ToString();
                r.Cells.Add(c);
                c = new TableCell();
                c.Text = string.Concat("approx. ", rb_cacheSize.ToString("n0"), " characters");
                c.HorizontalAlign = HorizontalAlign.Right;
                r.Cells.Add(c);
                t.Rows.Add(r);

                r = new TableRow();
                c = new TableCell();
                c.ColumnSpan = 2;
                c.Text = contentsItem.Value.ToString();
                r.Cells.Add(c);
                t.Rows.Add(r);
            }

            CachePanel.Controls.Add(t);

            t = new Table();
            t.CellSpacing = 0;
            t.CellPadding = 4;
            t.CssClass = "Normal";

            r = new TableRow();
            c = new TableCell();
            c.Text = "Appleseed Cache Count:";
            r.Cells.Add(c);
            c = new TableCell();
            c.Text = rb_cacheCount.ToString("n0") + " items";
            r.Cells.Add(c);
            t.Rows.Add(r);

            r = new TableRow();
            c = new TableCell();
            c.Text = "Total Cache Count:";
            r.Cells.Add(c);
            c = new TableCell();
            c.Text = HttpContext.Current.Cache.Count.ToString("n0") + " items";
            r.Cells.Add(c);
            t.Rows.Add(r);

            r = new TableRow();
            c = new TableCell();
            c.Text = "Appleseed Cache Size:";
            r.Cells.Add(c);
            c = new TableCell();
            c.Text = rb_cacheTotal.ToString("n0") + " characters";
            r.Cells.Add(c);
            t.Rows.Add(r);

            r = new TableRow();
            c = new TableCell();
            c.Text = "Total Cache Size:";
            r.Cells.Add(c);
            c = new TableCell();
            c.Text = asp_cacheTotal.ToString("n0") + " characters";
            r.Cells.Add(c);
            t.Rows.Add(r);

            CachePanel.Controls.Add(t);
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

//			ModuleTitle = new DesktopModuleTitle();

//			Controls.AddAt(0, ModuleTitle);

            base.OnInit(e);
        }

        #endregion

        // Jes1111
        /// <summary>
        /// Overrides ModuleSetting to render this module type un-cacheable
        /// </summary>
        /// <value><c>true</c> if cacheable; otherwise, <c>false</c>.</value>
        public override bool Cacheable
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        public override Guid GuidID
        {
            get { return new Guid("{33F254F8-2537-4486-A91D-E8544D407200}"); }
        }

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value><c>true</c> if [admin module]; otherwise, <c>false</c>.</value>
        public override bool AdminModule
        {
            get { return true; }
        }
    }
}