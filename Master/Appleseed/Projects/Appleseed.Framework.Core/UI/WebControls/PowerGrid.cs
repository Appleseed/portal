using System.Drawing;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Power Grid
    /// </summary>
    public class PowerGrid : DataGrid
    {
        /// <summary>
        /// 
        /// </summary>
        protected Label lblFooter;

        private string m_PagerCurrentPageCssClass = string.Empty;

        private string m_PagerOtherPageCssClass = string.Empty;

        #region Public Properties

        /// <summary>
        /// Gets or sets the sort expression.
        /// </summary>
        /// <value>The sort expression.</value>
        public string SortExpression
        {
            get { return base.Attributes["SortExpression"]; }

            set { base.Attributes["SortExpression"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is sorted ascending.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is sorted ascending; otherwise, <c>false</c>.
        /// </value>
        public bool IsSortedAscending
        {
            get { return base.Attributes["SortedAscending"].Equals("yes"); }

            set
            {
                if (!value)
                {
                    base.Attributes["SortedAscending"] = "no";
                    return;
                }
                base.Attributes["SortedAscending"] = "yes";
            }
        }

        /// <summary>
        /// Gets or sets the pager current page CSS class.
        /// </summary>
        /// <value>The pager current page CSS class.</value>
        public string PagerCurrentPageCssClass
        {
            get { return m_PagerCurrentPageCssClass; }

            set { m_PagerCurrentPageCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the pager other page CSS class.
        /// </summary>
        /// <value>The pager other page CSS class.</value>
        public string PagerOtherPageCssClass
        {
            get { return m_PagerOtherPageCssClass; }

            set { m_PagerOtherPageCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the footer text.
        /// </summary>
        /// <value>The footer text.</value>
        public string FooterText
        {
            get { return lblFooter.Text; }

            set { lblFooter.Text = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerGrid"/> class.
        /// </summary>
        public PowerGrid()
        {
            lblFooter = new Label();
            PagerStyle.Mode = PagerMode.NumericPages;
            PagerStyle.BackColor = Color.Gainsboro;
            PagerStyle.PageButtonCount = 10;
            PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            FooterStyle.BackColor = Color.Gainsboro;
            FooterStyle.HorizontalAlign = HorizontalAlign.Center;
            ShowFooter = true;
            AutoGenerateColumns = false;
            AllowPaging = true;
            PageSize = 7;
            CellSpacing = 2;
            CellPadding = 2;
            GridLines = GridLines.None;
            BorderColor = Color.Black;
            BorderStyle = BorderStyle.Solid;
            BorderWidth = 1;
            ForeColor = Color.Black;
            Font.Size = FontUnit.XXSmall;
            Font.Name = "Verdana";
            ItemStyle.BackColor = Color.Beige;
            AlternatingItemStyle.BackColor = Color.PaleGoldenrod;
            HeaderStyle.Font.Bold = true;
            HeaderStyle.BackColor = Color.Brown;
            HeaderStyle.ForeColor = Color.White;
            HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            AllowSorting = true;
            Attributes["SortedAscending"] = "yes";
            ItemCreated += new DataGridItemEventHandler(OnItemCreated);
            SortCommand += new DataGridSortCommandEventHandler(OnSortCommand);
            PageIndexChanged += new DataGridPageChangedEventHandler(OnPageIndexChanged);
        }

        /// <summary>
        /// Called when [item created].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        public void OnItemCreated(object sender, DataGridItemEventArgs e)
        {
            ListItemType listItemType = e.Item.ItemType;
            if (listItemType == ListItemType.Footer)
            {
                int i1 = e.Item.Cells.Count;
                for (int j = i1 - 1; j > 0; j--)
                {
                    e.Item.Cells.RemoveAt(j);
                }
                e.Item.Cells[0].ColumnSpan = i1;
                e.Item.Cells[0].Controls.Add(lblFooter);
            }
            if (listItemType == ListItemType.Pager)
            {
                TableCell tableCell1 = (TableCell) e.Item.Controls[0];
                for (int k = 0; k < tableCell1.Controls.Count; k += 2)
                {
                    try
                    {
                        LinkButton linkButton = (LinkButton) tableCell1.Controls[k];
                        linkButton.Text = string.Concat("[ ", linkButton.Text, " ]");
                        linkButton.CssClass = m_PagerOtherPageCssClass;
                    }
                    catch
                    {
                        Label label1 = (Label) tableCell1.Controls[k];
                        label1.Text = string.Concat("Page ", label1.Text);
                        if (m_PagerCurrentPageCssClass.Equals(string.Empty))
                        {
                            label1.ForeColor = Color.Blue;
                            label1.Font.Bold = true;
                        }
                        else
                        {
                            label1.CssClass = m_PagerCurrentPageCssClass;
                        }
                    }
                }
            }
            //if (listItemType == ListItemType.Item)
            if (listItemType == ListItemType.Header)
            {
                string str1 = base.Attributes["SortExpression"];
                string str2 = !base.Attributes["SortedAscending"].Equals("yes") ? " 6" : " 5";
                for (int i2 = 0; i2 < base.Columns.Count; i2++)
                {
                    if (str1 == base.Columns[i2].SortExpression)
                    {
                        TableCell tableCell2 = e.Item.Cells[i2];
                        Label label2 = new Label();
                        label2.Font.Name = "webdings";
                        label2.Font.Size = FontUnit.XXSmall;
                        label2.Text = str2;
                        tableCell2.Controls.Add(label2);
                    }
                }
            }
        }

        /// <summary>
        /// Called when [sort command].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridSortCommandEventArgs"/> instance containing the event data.</param>
        public void OnSortCommand(object sender, DataGridSortCommandEventArgs e)
        {
            string _SortExpression;
            string _SortedAscending;

            _SortExpression = Attributes["SortExpression"];
            _SortedAscending = Attributes["SortedAscending"];

            Attributes["SortExpression"] = e.SortExpression;
            Attributes["SortedAscending"] = "yes";

            if (e.SortExpression == _SortExpression)
            {
                if (_SortedAscending == "yes")
                    Attributes["SortedAscending"] = "no";
            }
        }

        /// <summary>
        /// Called when [page index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridPageChangedEventArgs"/> instance containing the event data.</param>
        public void OnPageIndexChanged(object sender, DataGridPageChangedEventArgs e)
        {
            CurrentPageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// Sets the GFW styles.
        /// </summary>
        public void SetGfwStyles()
        {
            CellPadding = 3;
            CellSpacing = 0;
            BorderColor = Color.Black;
            BackColor = Color.WhiteSmoke;
            ForeColor = Color.Black;
            GridLines = GridLines.Both;
            ItemStyle.BackColor = Color.WhiteSmoke;
            ItemStyle.VerticalAlign = VerticalAlign.Top;
            AlternatingItemStyle.BackColor = Color.LightGray;
            AlternatingItemStyle.VerticalAlign = VerticalAlign.Top;
            HeaderStyle.ForeColor = Color.Black;
            HeaderStyle.Font.Bold = true;
            HeaderStyle.BackColor = Color.LightGray;
        }
    }
}