using System;
using System.Collections;
using System.Data.SqlClient;
using System.Web.UI;
using Appleseed.Framework;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.PortalTemplate;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Module to manage portals (AdminAll)
    /// </summary>
    public partial class Portals : PortalModuleControl
    {
        /// <summary>
        /// 
        /// </summary>
        protected ArrayList portals;

        /// <summary>
        /// 
        /// </summary>
        protected IList templates;

        /// <summary>
        /// Admin Module
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        IPortalTemplateServices templateServices;

        public Portals()
        {
            templateServices = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
        }

        /// <summary>
        /// The Page_Load server event handler on this user control is used
        /// to populate the current portals list from the database
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            LoadPortalList();
            LoadTemplatesList();

            // If this is the first visit to the page, bind the tab data to the page listbox
            if (!Page.IsPostBack) {
                portalList.DataBind();
                templatesList.DataBind();
            }

            EditBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl;
            DeleteBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl;
            //ExportBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Right", "arrow_right.png").ImageUrl;

            //btnEditTemplate.ImageUrl = this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl;
            btnDeleteTemplate.ImageUrl = this.CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl;
            btnSaveTemplate.ImageUrl = this.CurrentTheme.GetImage("Buttons_Save", "Save.gif").ImageUrl;

            DeleteBtn.Attributes.Add("onclick", "return confirmDelete();");
            btnDeleteTemplate.Attributes.Add("onclick", "return confirmDelete();");
        }



        private void LoadTemplatesList()
        {
            var templateList = templateServices.GetTemplates(PortalSettings.PortalAlias, PortalSettings.PortalFullPath);
            

            templates = (from t in templateList select new { Name = t, ID = t }).ToList();
        }

        private void LoadPortalList()
        {
            portals = new ArrayList();
            PortalsDB portalsDb = new PortalsDB();
            SqlDataReader dr = portalsDb.GetPortals();
            try {
                while (dr.Read()) {
                    PortalItem p = new PortalItem();
                    p.Name = dr["PortalName"].ToString();
                    p.Path = dr["PortalPath"].ToString();
                    p.ID = Convert.ToInt32(dr["PortalID"].ToString());
                    portals.Add(p);
                }
            } finally {
                dr.Close(); //by Manu, fixed bug 807858
            }
        }



        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{366C247D-4CFB-451D-A7AE-649C83B05841}"); }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInit event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
            // Add a link for the edit page
            this.AddText = "ADD_PORTAL";
            this.AddUrl = "~/DesktopModules/CoreModules/PortalsAdministration/AddNewPortal.aspx";
        }

        #endregion

        /// <summary>
        /// OnDelete
        /// </summary>
        protected override void OnDelete()
        {
            if (portalList.SelectedIndex != -1) {
                try {
                    // must delete from database too
                    PortalItem p = (PortalItem)portals[portalList.SelectedIndex];
                    PortalsDB portalsdb = new PortalsDB();
                    //Response.Write("Will delete " + p.Name);
                    portalsdb.DeletePortal(p.ID);

                    // remove item from list
                    portals.RemoveAt(portalList.SelectedIndex);
                    // rebind list
                    portalList.DataBind();
                } catch (SqlException sqlex) {
                    string aux =
                        General.GetString("DELETE_PORTAL_ERROR", "There was an error on deleting the portal", this);
                    Appleseed.Framework.ErrorHandler.Publish(Appleseed.Framework.LogLevel.Error, aux, sqlex);
                    Controls.Add(new LiteralControl("<br><span class=NormalRed>" + aux + "<br>"));
                }
            }
            base.OnDelete();
        }

        /// <summary>
        /// OnEdit
        /// </summary>
        protected override void OnEdit()
        {
            if (portalList.SelectedIndex != -1) {
                // must delete from database too
                PortalItem p = (PortalItem)portals[portalList.SelectedIndex];

                //Add new portal
                // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/PortalsAdministration/EditPortal.aspx", 0,
                                                          "PortalID=" + p.ID + "&mID=" + ModuleID.ToString()));
            }
            base.OnEdit();
        }
        protected void EditBtn_Click(object sender, ImageClickEventArgs e)
        {
            OnEdit();
        }
        protected void DeleteBtn_Click(object sender, ImageClickEventArgs e)
        {
            OnDelete();
        }

        protected void SerializeBtn_Click(object sender, EventArgs e)
        {
            if (portalList.SelectedIndex != -1) {
                IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
                PortalItem p = (PortalItem)portals[portalList.SelectedIndex];
                bool ok = services.SerializePortal(p.ID, PortalSettings.PortalAlias, PortalSettings.PortalFullPath);
                
                if (!ok) {

                    DisplayMessage(ErrorMessage, "Export failed (full error logged) <br>");
                } else {
                    DisplayMessage(SuccessMessage, "Export succeeded! <br>");
                }
            } else {
                DisplayMessage(ErrorMessage, "You must select a portal <br>");
            }

            LoadTemplatesList();
            templatesList.DataBind();
        }



        protected void btnDeleteTemplate_Click(object sender, ImageClickEventArgs e)
        {
            if (templatesList.SelectedIndex != -1) {
                string templateName = ((dynamic)templates[templatesList.SelectedIndex]).Name;

                templateServices.DeleteTemplate(templateName, PortalSettings.PortalFullPath);
                

                LoadTemplatesList();
                templatesList.DataBind();
            } else {
                DisplayMessage(TemplateErrorMessage, "You must select a template <br>");
            }
        }

        protected void btnSaveTemplate_Click(object sender, ImageClickEventArgs e)
        {
            if (templatesList.SelectedIndex != -1) {
                string templateName = ((dynamic)templates[templatesList.SelectedIndex]).Name;

                FileInfo file = templateServices.GetTemplateInfo(templateName, PortalSettings.PortalFullPath);
                

                Response.ContentType = "text/xml";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + templateName);
                Response.TransmitFile(file.FullName);
                Response.End();
            } else {
                DisplayMessage(TemplateErrorMessage, "You must select a template <br>");
            }
        }



        private void DisplayMessage(Label lblControl, string message)
        {
            ErrorMessage.Visible = false;
            SuccessMessage.Visible = false;
            TemplateErrorMessage.Visible = false;
            TemplateSuccessMessage.Visible = false;

            lblControl.Visible = true;
            lblControl.Text = message;
        }

        protected void btnImport_click(object sender, EventArgs e)
        {
            if (templatesList.SelectedIndex != -1)
            {
                int idModule = this.ModuleID;
                string selectedValue = templatesList.SelectedValue;

                Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/PortalsAdministration/AddNewPortal.aspx", 0,
                                                          "PortalID=" + idModule + "&mID=" + idModule+"&chkUseXMLTemplate=true&selectedTemplate="+selectedValue));
            }
            else
            {
                DisplayMessage(ErrorMessage, "You must select a portal <br>");
            }
        }
    }
}
