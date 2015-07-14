using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Collections.Generic;
using Appleseed.Framework;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;
using TextAlign = Trirand.Web.Mvc.TextAlign;

namespace UserManager.Models
{
    public class JQGridModel
    {
        public JQGrid RetailDataGrid { get; set; }

        public JQGridModel()
        {
            RetailDataGrid = new JQGrid
            {
                    Columns = new List<JQGridColumn>()
                    {
                        new JQGridColumn { 
                                        DataField = "UserId",
                                        Visible = false,
                                        PrimaryKey = true
                                        },
                        new JQGridColumn { DataField = "UserName", Visible = false, },
                        new JQGridColumn { DataField = "UserEmail", Visible = false, },
                        new JQGridColumn { DataField = "UserRole", Visible = false, },
                        new JQGridColumn { DataField = General.GetString("VIEW"), Visible = false, Formatter = new CustomFormatter{ FormatFunction = "formatEditLink",}},
                        new JQGridColumn { DataField = General.GetString("DELETE"), Visible = false, Formatter = new CustomFormatter{ FormatFunction = "formatDeletLink",}},

                    },
                    Width = Unit.Pixel(600),
                    Height = Unit.Percentage(300),
                    SortSettings = new SortSettings
                                       {
                                           InitialSortColumn = "Position",
                                       },
                    TreeGridSettings = new TreeGridSettings
                    {
                        Enabled = true
                    }
           };

        }
    }

}