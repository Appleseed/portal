using System;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Settings;

namespace Appleseed.Framework.Web.UI.WebControls
{
	/// <summary>
	/// Binds tabular XML data to a datagrid, and allows add, editing, and deleting
	/// items from the XML file.
	/// Modified from Susan Warren's XmlEditGrid available on http://asp.net
	/// Appleseed version by Jes1111 - 15-June-2005
	/// </summary> 
	[
		DefaultProperty("XmlFile")
	]
	public class XmlEditGrid : DataGrid
	{

		//
		// Fields
		//
		private bool shouldRebind = false;
		private DataTable dt = null;
		private DataSet ds = null;

		//
		// Properties
		//
		/// <summary>
		/// Gets or sets the XML file.
		/// </summary>
		/// <value>The XML file.</value>
		[
			Bindable(true),
				Category("Data"),
				Description("Xml file to edit.")
		]
		public string XmlFile
		{

			get
			{
				string s = (string)ViewState["XmlFile"];
				if (s == null)
					return String.Empty;
				return s;
			}
			set
			{
				ViewState["XmlFile"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the edit text box CSS class.
		/// </summary>
		/// <value>The edit text box CSS class.</value>
		[
			Bindable(true),
				Browsable(true),
				Category("Appearance"),
				Description("Name of CSS class to apply to textboxes in edit mode.")
		]
		public string EditTextBoxCssClass
		{

			get
			{
				string s = (string)ViewState["EditTextBoxCssClass"];
				if (s == null)
					return String.Empty;
				return s;
			}
			set
			{
				ViewState["EditTextBoxCssClass"] = value;
			}
		}

		#region Overridden DataGrid Methods
		/// <summary>
		/// OnInit() method -- Add the Edit and Delete columns to the grid
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			// Test for Page to ensure this doesn't run at design time
			if (Page != null)
			{
				string imagePath = GetClientScriptPath();

				EditCommandColumn c0 = new EditCommandColumn();
				c0.EditText = "<img src='" + imagePath + "edit.gif' border=0 alt='" +General.GetString("XMLEDITGRID_EDITITEM", "edit this item") + "'>";
				c0.CancelText = "<img src='" + imagePath + "cancel.gif' border=0 alt='" +General.GetString("XMLEDITGRID_CANCEL", "cancel") + "'>";
				c0.UpdateText = "<img src='" + imagePath + "update.gif' border=0 alt='" +General.GetString("XMLEDITGRID_SAVE", "save changes") + "'>";
				c0.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
				c0.HeaderStyle.Width = new Unit("35px");
				this.Columns.Add(c0);

				ButtonColumn c1 = new ButtonColumn();
				c1.CommandName = "Delete";
				c1.Text = "<img src='" + imagePath + "delete.gif' border=0 alt='" +General.GetString("XMLEDITGRID_DELETE", "delete this item") + "'>";
				c1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
				c1.HeaderStyle.Width = new Unit("35px");
				this.Columns.Add(c1);

				this.ShowFooter = true;
				this.ItemStyle.VerticalAlign = VerticalAlign.Top;
			}
		}

		/// <summary>
		/// OnLoad() method -- We read the XML file on each
		/// page request at runtime.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			// Test for Page to ensure this doesn't run at design time
			if (Page != null)
			{

				// get the data from the XML file
				ds = new DataSet();
				ds.ReadXml(HttpContext.Current.Server.MapPath(XmlFile));

				// set the datatable 
				if (DataMember.Length != 0)
					dt = ds.Tables[DataMember];
				else
					dt = ds.Tables[0];
			}

			// should bind data on first page load
			if (!Page.IsPostBack)
			{
				shouldRebind = true;
			}
		}

		/// <summary>
		/// OnPageIndexChanged() method -- handles datagrid
		/// paging
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridPageChangedEventArgs"></see> that contains event data.</param>
		protected override void OnPageIndexChanged(DataGridPageChangedEventArgs e)
		{
			this.CurrentPageIndex = e.NewPageIndex;
			shouldRebind = true;
		}

		/// <summary>
		/// OnEditCommand() method -- puts the grid in edit mode
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"></see> that contains event data.</param>
		protected override void OnEditCommand(DataGridCommandEventArgs e)
		{
			this.EditItemIndex = e.Item.ItemIndex;
			shouldRebind = true;
		}

		/// <summary>
		/// OnCancelCommand() method -- cancels edit mode
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"></see> that contains event data.</param>
		protected override void OnCancelCommand(DataGridCommandEventArgs e)
		{
			this.EditItemIndex = -1;
			shouldRebind = true;
		}

		/// <summary>
		/// OnDeleteCommand() method -- cancels edit mode
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"></see> that contains event data.</param>
		protected override void OnDeleteCommand(DataGridCommandEventArgs e)
		{
			// remove the row that fired the command
			if (e.Item.DataSetIndex != 0) // don't allow first row to be deleted
			{
				dt.Rows.RemoveAt(e.Item.DataSetIndex);

				// save the updated data as the XML file
				SaveData();

				this.EditItemIndex = -1;
				shouldRebind = true;
			}
		}

		/// <summary>
		/// OnUpdateCommand() method -- cancels edit mode
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"></see> that contains event data.</param>
		protected override void OnUpdateCommand(DataGridCommandEventArgs e)
		{
			// get a reference to this row of data, using the DataSetIndex
			DataRow row = dt.Rows[e.Item.DataSetIndex];

			// get the values and update the datarow
			for (int i = 0; i < dt.Columns.Count; i++)
			{

				// skip first two columns (Edit and delete columns)
				int j = i + 2;

				// get the value from the textbox and push it into the dataset
				TextBox t = (TextBox)e.Item.Cells[j].Controls[0];
				row[dt.Columns[i].Caption] = HttpUtility.HtmlEncode(t.Text);
			}

			// save the updated data as the XML file
			SaveData();

			// clear the edit item and rebind the grid
			this.EditItemIndex = -1;
			shouldRebind = true;
		}

		/// <summary>
		/// OnItemDataBound() method -- For the edit item, makes
		/// the textboxes multiline for long values
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridItemEventArgs"></see> that contains event data.</param>
		protected override void OnItemDataBound(DataGridItemEventArgs e)
		{
			// Buff up the appearance of textboxes in the Edit Item
			// Text for the edit item
			if ((ListItemType)e.Item.ItemType == ListItemType.EditItem)
			{

				foreach (TableCell c in e.Item.Controls)
				{

					if (c.Controls[0].GetType() == typeof(TextBox))
					{

						TextBox t = (TextBox)c.Controls[0];

						t.Text = HttpUtility.HtmlDecode(t.Text);

						// set style properties
						if (this.EditTextBoxCssClass.Length != 0)
							t.CssClass = this.EditTextBoxCssClass;
						else
							t.Style["width"] = "100%";

						// if the data is really long, make this a textarea
						int rows = (t.Text.Length / 50) + 1;
						if (rows > 1)
						{

							// set the textbox to render as a textarea
							t.TextMode = TextBoxMode.MultiLine;
							t.Rows = rows + 3;
						}
					}
				}
			}
		}

		/// <summary>
		/// OnItemCreated() method --Add the "Add new item" link to
		/// the footer row
		/// </summary>
		/// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridItemEventArgs"></see> that contains event data.</param>
		protected override void OnItemCreated(DataGridItemEventArgs e)
		{
			if ((ListItemType)e.Item.ItemType == ListItemType.Footer)
			{

				// The Footer row item (e.Item) is a TableRow that contains
				// one cell for each column in the grid.
				// Delete all of the current cells and insert a single one
				// that spans the entire row.

				// get the cell count, then clear the Cells collection
				int colcount = e.Item.Cells.Count;
				e.Item.Cells.Clear();

				// create the new cell, and set it to span all of the columns
				TableCell c = new TableCell();
				c.ColumnSpan = colcount;

				// create a LinkButton for adding a new item
				LinkButton l = new LinkButton();
				l.Click += new EventHandler(AddItem);
				l.Text =General.GetString("XMLEDITGRID_ADD", "add new item");
				l.CssClass = "CommandButton";

				// Add the LinkButton to the cell, and the cell to the pager row
				c.Controls.Add(l);
				e.Item.Cells.AddAt(0, c);
			}
		}

		/// <summary>
		/// OnPreRender() method -- We bind in this method since
		/// it's called only at runtime
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{

			if (shouldRebind)
			{

				this.DataSource = dt;
				this.DataBind();

			}
		}


		/// <summary>
		/// Hide some properties on the base DataGrid
		/// </summary>
		/// <value></value>
		/// <returns>The name of the key field in the data source specified by <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSource"></see>.</returns>
		[
			Bindable(false),
				Browsable(false)
		]
		public override string DataKeyField
		{

			get
			{
				return String.Empty;
			}
			set
			{
			}
		}

		/// <summary>
		/// Hide some properties on the base DataGrid
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Collections.IEnumerable"></see> or <see cref="T:System.ComponentModel.IListSource"></see> that contains a collection of values used to supply data to this control. The default value is null.</returns>
		/// <exception cref="T:System.Web.HttpException">The data source cannot be resolved because a value is specified for both the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSource"></see> property and the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSourceID"></see> property. </exception>
		[
			Bindable(false),
				Browsable(false)
		]
		public override object DataSource
		{

			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}
		#endregion

		#region Helper methods
		/// <summary>
		/// SaveData() method -- writes the data back to the XML file
		/// </summary>
		public void SaveData()
		{

			// save the XML data to a file
			ds.WriteXml(HttpContext.Current.Server.MapPath(XmlFile));
		}

		/// <summary>
		/// AddItem() method -- adds a new item to the end of the file
		/// </summary>
		/// <param name="Src">The SRC.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		void AddItem(Object Src, EventArgs e)
		{

			// Create a new row
			DataRow r = dt.NewRow();

			// initialize it
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				r[dt.Columns[i].Caption] = "";
			}

			// add it to the table
			dt.Rows.Add(r);

			// save the updated data as the XML file
			SaveData();

			// set the edit index to the new row, and bind
			this.EditItemIndex = dt.Rows.Count - 1;
			shouldRebind = true;
		}

		/// <summary>
		/// GetClientScriptPath() method -- works out the
		/// location of the shared image files.
		/// </summary>
		/// <returns></returns>
		string GetClientScriptPath()
		{
			return Path.ApplicationRootPath("/DesktopModules/MagicUrls/");
		}
		#endregion
	}
}
