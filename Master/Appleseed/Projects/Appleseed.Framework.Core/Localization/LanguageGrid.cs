using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// LanguagesWebDataGrid
    /// </summary>
    public class LanguageGrid : DataGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LanguageGrid"/> class.
        /// </summary>
        public LanguageGrid()
        {
            AutoGenerateColumns = false;

            TemplateColumn colUILang = new TemplateColumn();
            colUILang.HeaderText = General.GetString("LANG_SWITCH_UI_LANGUAGE", "UI Language");
            //colUILang.ItemTemplate = new LanguagesTemplateDropDown(ListItemType.Item, "Language", CultureTypes.AllCultures);
            colUILang.ItemTemplate = new LanguagesTemplateDropDown(ListItemType.Item, CultureTypes.AllCultures);
            Columns.Add(colUILang);

            TemplateColumn colCulture = new TemplateColumn();
            colCulture.HeaderText = General.GetString("LANG_SWITCH_CULTURE", "Culture");
            //colCulture.ItemTemplate = new LanguagesTemplateDropDown(ListItemType.Item, "Culture", CultureTypes.SpecificCultures);
            colCulture.ItemTemplate = new LanguagesTemplateDropDown(ListItemType.Item, CultureTypes.SpecificCultures);
            Columns.Add(colCulture);

            ButtonColumn colDelete = new ButtonColumn();
            colDelete.Text = General.GetString("LANG_SWITCH_DELETE", "Delete");
            colDelete.CommandName = "Delete";
            Columns.Add(colDelete);

            ButtonColumn colAdd = new ButtonColumn();
            colAdd.Text = General.GetString("LANG_SWITCH_Add", "Add");
            colAdd.CommandName = "Add";
            Columns.Add(colAdd); 
        }

        /// <summary>
        /// Updates data sources with grid content
        /// </summary>
        public void UpdateRows()
        {
            LanguageCultureCollection LanguagesDataSource = (LanguageCultureCollection) DataSource;

            for (Int32 i = 0; i < Items.Count; i++)
            {
                DropDownList ddLanguage = (DropDownList) Items[i].Cells[0].Controls[0];
                DropDownList ddCulture = (DropDownList) Items[i].Cells[1].Controls[0];

                LanguagesDataSource[i].UICulture = new CultureInfo(ddLanguage.SelectedItem.Value);
                LanguagesDataSource[i].Culture = new CultureInfo(ddCulture.SelectedItem.Value);
            }
        }

        /// <summary>
        /// Binds data source to grid
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            LanguageCultureCollection LanguagesDataSource = (LanguageCultureCollection) DataSource;

            if (LanguagesDataSource.Count > 1)
                Columns[2].Visible = true;
            else
                Columns[2].Visible = false; //do not allow deleting last row

            for (Int32 i = 0; i < Items.Count; i++)
            {
                DropDownList ddLanguage = (DropDownList) Items[i].Cells[0].Controls[0];
                DropDownList ddCulture = (DropDownList) Items[i].Cells[1].Controls[0];

                if (ddLanguage.Items.FindByValue(LanguagesDataSource[i].UICulture.Name) != null)
                    ddLanguage.Items.FindByValue(LanguagesDataSource[i].UICulture.Name).Selected = true;

                if (ddCulture.Items.FindByValue(LanguagesDataSource[i].Culture.Name) != null)
                    ddCulture.Items.FindByValue(LanguagesDataSource[i].Culture.Name).Selected = true;
            }
        }

        /// <summary>
        /// Occours when user clicks on ADD or DELETE
        /// </summary>
        /// <param name="e">A <see cref="T:System.Web.UI.WebControls.DataGridCommandEventArgs"></see> that contains event data.</param>
        protected override void OnItemCommand(DataGridCommandEventArgs e)
        {
            LanguageCultureCollection LanguagesDataSource = (LanguageCultureCollection) DataSource;

            switch (e.CommandName)
            {
                case "Add":
                    LanguagesDataSource.Insert(e.Item.ItemIndex, new LanguageCultureItem());
                    DataBind();
                    break;
                case "Delete":
                    LanguagesDataSource.RemoveAt(e.Item.ItemIndex);
                    DataBind();
                    break;
                case "Up":
                    break;
                case "Down":
                    break;
            }
            base.OnItemCommand(e);
        }
    }
}