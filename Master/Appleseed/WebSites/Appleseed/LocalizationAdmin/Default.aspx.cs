//#define OnlineDemo

using System;
using System.Data;
using System.Web.UI.WebControls;
using Westwind.Globalization;
using System.Globalization;
using Westwind.Web.Controls;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Westwind.Globalization.Tools;
using System.Web.Mvc;
using Appleseed.Framework.Web.UI;



public partial class LocalizeAdmin_Default : Page
{
    /// <summary>
    /// We talk directly to the Db resource manager (bus object) here rather than
    /// through the provider or resource manager, as we don't have the flexibility
    /// with the core resource managers.
    /// </summary>
    protected wwDbResourceDataManager Manager = new wwDbResourceDataManager();

    public string ResourceSet
    {
        get { return _ResourceSet; }
        set { _ResourceSet = value; }
    }
    private string _ResourceSet = "";


    #region Page Initialization and Data Binding routines

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // *** On callbacks we don't need to populate any data since they are
        // *** raw method calls. Callback routes to parser from here
        if (this.Callback.IsCallback)
            return;

        Response.Expires = 0;

        if (!Manager.IsLocalizationTable(null))
        {
            this.ErrorDisplay.ShowError(Res("ResourceTableDoesntExist"));
            this.btnCreateTable.Visible = true;
            return;
        }

        this.GetResourceSet();

        DataTable dt = Manager.GetAllResourceIds(ResourceSet);
        if (dt == null)
        {
            this.ErrorDisplay.ShowError("Couldn't load resources: " + Manager.ErrorMessage);
            return;
        }

        this.lstResourceIds.DataSource = dt;
        this.lstResourceIds.DataValueField = "ResourceId";
        this.lstResourceIds.DataBind();

        if (this.lstResourceIds.Items.Count > 0)
            this.lstResourceIds.SelectedIndex = 0;

        dt = Manager.GetAllLocaleIds(ResourceSet);
        if (dt == null)
        {
            this.ErrorDisplay.ShowError("Couldn't load resources: " + Manager.ErrorMessage);
            return;
        }

        foreach (DataRow row in dt.Rows)
        {
            string Code = row["LocaleId"] as string;
            CultureInfo ci = CultureInfo.GetCultureInfo(Code.Trim());

            if (Code != "")
                row["Language"] = ci.DisplayName + " (" + ci.Name + ")";
            else
                row["Language"] = "Invariant";
        }

        this.lstLanguages.DataSource = dt;
        this.lstLanguages.DataValueField = "LocaleId";
        this.lstLanguages.DataTextField = "Language";
        this.lstLanguages.DataBind();

        if (this.lstLanguages.Items.Count > 0)
            this.lstLanguages.SelectedIndex = 0;
        else
            this.lstLanguages.Items.Add(new ListItem("Invariant", ""));
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // *** On callbacks we don't need to populate any data since they are
        // *** raw method calls. Callback routes to parser from here
        if (this.Callback.IsCallback)
            return;

        this.SetControlId();

        this.imgExportResources.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_EXTRACTRESOURCES);
        this.imgRefresh.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_REFRESH);
        this.imgRecycleApp.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_RECYCLE);

        if (this.btnCreateTable.Visible)
        {
            this.imgCreateTable.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_CREATETABLE);
            this.imgCreateTable.Visible = true;
        }

        this.imgDeleteResourceSet.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_DELETE);
        this.imgRenameResourceSet.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_RENAME);

        this.imgImport.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_IMPORT);
        this.imgBackup.Src = this.Page.ClientScript.GetWebResourceUrl(typeof(GlobalizationResources), GlobalizationResources.INFO_ICON_BACKUP);


        // *** Check if resources are properly active if not we have a 
        //     problem and need to let user know
        if (Res("BackupComplete") == "BackupComplete")
        {
           // *** Not localized so it's always visible!!!
            this.ErrorDisplay.DisplayTimeout = 0;
            this.ErrorDisplay.ShowError("Resources are not available for this site. Most likely this means you have enabled the wwDbResourceProvider without first importing resources or that your database connection is not properly configured.<p/>" +
                                        "Please make sure you run this form without the wwDbResourceProvider enabled and ensure you have created the resource table and imported Resx resources of the site. <p />" +
                                        "For more information please check the configuration at: <p />" +
                                        "<a href='http://www.west-wind.com/tools/wwDbResourceProvider/docs'>www.west-wind.com/tools/wwDbResourceProvider/docs</a>");
        }

    }

    private void GetResourceSet()
    {
        this.ResourceSet = Request.Form[this.lstResourceSet.UniqueID];
        if (ResourceSet == null)
            this.ResourceSet = Request.QueryString["ResourceSet"];
        if (this.ResourceSet == null)
            this.ResourceSet = ViewState["ResourceSet"] as string;

        if (this.ResourceSet == null)
            this.ResourceSet = "";

        this.ResourceSet = this.ResourceSet.ToLower();

        if (!string.IsNullOrEmpty(this.ResourceSet))
            ViewState["ResourceSet"] = this.ResourceSet;

        // *** Clear selections
        this.lstResourceIds.SelectedValue = null;
        this.lstResourceSet.SelectedValue = null;

        DataTable dt = Manager.GetAllResourceSets(ResourceListingTypes.AllResources);
        this.lstResourceSet.DataSource = dt;
        this.lstResourceSet.DataValueField = "ResourceSet";
        this.lstResourceSet.DataBind();

        if (this.ResourceSet != null)
            this.lstResourceSet.SelectedValue = this.ResourceSet;
    }

    private void SetControlId()
    {
        string CtlId = null;
        if (this.IsPostBack)
            CtlId = Request.Form[this.lstResourceIds.UniqueID];

        if (CtlId == null)
            CtlId = Request.QueryString["CtlId"];

        if (string.IsNullOrEmpty(CtlId))
            return;

        string Id = CtlId;

        // *** Search for .Text first
        string[] Tokens = Id.Split('.');
        if (Tokens.Length == 2)
            Id = Tokens[0] + ".Text";

        for (int x = 0; x < 2; x++)
        {
            if (x == 1)
                // *** No match for .text - find passed property
                Id = CtlId;

            foreach (ListItem li in this.lstResourceIds.Items)
            {
                if (li.Value.ToLower() == Id.ToLower())
                {
                    this.lstResourceIds.SelectedValue = Id;
                    return;
                }
            }
        }

    }
    #endregion

    #region Page Event Handlers - only a few of them - most calls are Ajax Callbacks
  

    protected void btnFileUpload_Click(object sender, EventArgs e)
    {

#if OnlineDemo
        this.ErrorDisplay.ShowError(Res("FeatureDisabled"));
        return;   
#endif


        if (!this.FileUpload.HasFile)
            return;

        //FileInfo fi = new FileInfo(this.FileUpload.FileName);
        string Extension = Path.GetExtension(this.FileUpload.FileName).TrimStart('.');  // fi.Extension.TrimStart('.');

        string Filter = ",bmp,ico,gif,jpg,png,css,js,txt,wav,mp3,";
        if (Filter.IndexOf("," + Extension + ",") == -1)
        {
            this.ErrorDisplay.ShowError(Res("InvalidFileUploaded"));
            return;
        }

        string FilePath = Server.MapPath(this.FileUpload.FileName);

        File.WriteAllBytes(FilePath, FileUpload.FileBytes);

        string ResourceId = this.txtNewResourceId.Text;

        // *** Try to add the file
        wwDbResourceDataManager Data = new wwDbResourceDataManager();
        if (Data.UpdateOrAdd(ResourceId, FilePath, this.txtNewLanguage.Text, this.ResourceSet, true) == -1)
            this.ErrorDisplay.ShowError(Res("ResourceUpdateFailed") + "<br/>" + Data.ErrorMessage);
        else
            this.ErrorDisplay.ShowMessage(Res("ResourceUpdated"));

        File.Delete(FilePath);

        this.lstResourceIds.Items.Add(ResourceId);
        this.lstResourceIds.SelectedValue = ResourceId;
    }


    protected void btnCreateTable_Click(object sender, EventArgs e)
    {
#if OnlineDemo
        this.ErrorDisplay.ShowError(Res("FeatureDisabled"));
        return;
#endif

        if (!this.Manager.CreateLocalizationTable(null))
            this.ErrorDisplay.ShowError(Res("LocalizationTableNotCreated") + "<br />" + this.Manager.ErrorMessage);
        else
            this.ErrorDisplay.ShowMessage(Res("LocalizationTableCreated"));
    }


    protected void btnExportResources_Click(object sender, EventArgs e)
    {
        wwDbResXConverter Exporter = new wwDbResXConverter(this.Context.Request.PhysicalApplicationPath);

        if (!Exporter.GenerateLocalResourceResXFiles())
        {
            ErrorDisplay.ShowError(Res("ResourceGenerationFailed"));
            return;
        }
        if (!Exporter.GenerateGlobalResourceResXFiles())
        {
            ErrorDisplay.ShowError(Res("ResourceGenerationFailed"));
            return;
        }

        this.ErrorDisplay.ShowMessage(Res("ResourceGenerationComplete"));
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
#if OnlineDemo
        this.ErrorDisplay.ShowError(Res("FeatureDisabled"));
        return;
#endif

        wwDbResXConverter Converter = new wwDbResXConverter(this.Context.Request.PhysicalApplicationPath);
        Converter.ImportWebResources();
        this.ErrorDisplay.ShowMessage(Res("ResourceImportComplete"));

        this.lstResourceIds.SelectedValue = null;
        this.GetResourceSet();
    }

    protected void btnRenameResourceSet_Click(object sender, EventArgs e)
    {
#if OnlineDemo
        this.ErrorDisplay.ShowError(Res("FeatureDisabled"));
        return;
#endif
        
        if (!this.Manager.RenameResourceSet(this.txtOldResourceSet.Text, this.txtRenamedResourceSet.Text))
            this.ErrorDisplay.ShowError(this.Manager.ErrorMessage);
        else
        {
            // *** Force the selected value to be set
            this.lstResourceSet.Items.Add(new ListItem("", this.txtRenamedResourceSet.Text.ToLower()));
            this.lstResourceSet.SelectedValue = this.txtRenamedResourceSet.Text.ToLower();

            //this.lstResourceSet.SelectedValue = string.Empty;   // null; 

            // *** Refresh and reset the resource list
            this.GetResourceSet();

            this.ErrorDisplay.ShowMessage(Res("ResourceSetRenamed"));
        }
    }
    #endregion

    #region Ajax Callback Methods
    [CallbackMethod]
    public DataTable GetResourceList(string ResourceSet, string ResourceFilter)
    {
        DataTable dt = Manager.GetAllResourceIds(ResourceSet, ResourceFilter);
        if (Manager == null)
            throw new ApplicationException(Res("ResourceSetLoadingFailed") + ":" + Manager.ErrorMessage);

        return dt;
    }

    [CallbackMethod]
    public string GetResourceString(string ResourceId, string ResourceSet, string CultureName)
    {
        string Value = this.Manager.GetResourceString(ResourceId, ResourceSet, CultureName);

        if (Value == null && !string.IsNullOrEmpty(Manager.ErrorMessage))
            throw new ArgumentException(this.Manager.ErrorMessage);

        return Value;
    }

    [CallbackMethod]
    public Dictionary<string, string> GetResourceStrings(string ResourceId, string ResourceSet)
    {
        Dictionary<string, string> Resources = this.Manager.GetResourceStrings(ResourceId, ResourceSet);

        if (Resources == null)
            throw new ApplicationException(this.Manager.ErrorMessage);

        return Resources;
    }


    [CallbackMethod]
    public bool UpdateResourceString(string Value, string ResourceId, string ResourceSet, string LocaleId)
    {
        if (this.Manager.UpdateOrAdd(ResourceId, Value, LocaleId, ResourceSet) == -1)
            return false;

        return true;
    }

    [CallbackMethod]
    public bool DeleteResource(string ResourceId, string ResourceSet, string LocaleId)
    {

#if OnlineDemo        
        throw new ApplicationException(Res("FeatureDisabled"));
#endif

        if (!this.Manager.DeleteResource(ResourceId, LocaleId, ResourceSet))
            throw new ApplicationException(Res("ResourceUpdateFailed") + ": " + this.Manager.ErrorMessage);

        return true;
    }

    [CallbackMethod]
    public bool RenameResource(string ResourceId, string NewResourceId, string ResourceSet)
    {
#if OnlineDemo
        throw new ApplicationException(Res("FeatureDisabled"));
#endif

        if (!this.Manager.RenameResource(ResourceId, NewResourceId, ResourceSet))
            throw new ApplicationException(Res("InvalidResourceId"));

        return true;
    }

    /// <summary>
    /// Renames all resource keys that match a property (ie. lblName.Text, lblName.ToolTip)
    /// at once. This is useful if you decide to rename a meta:resourcekey in the ASP.NET
    /// markup.
    /// </summary>
    /// <param name="Property">Original property prefix</param>
    /// <param name="NewProperty">New Property prefix</param>
    /// <param name="ResourceSet">The resourceset it applies to</param>
    /// <returns></returns>
    [CallbackMethod]
    public bool RenameResourceProperty(string Property, string NewProperty, string ResourceSet)
    {
        if (!this.Manager.RenameResourceProperty(Property, NewProperty, ResourceSet))
            throw new ApplicationException(Res("InvalidResourceId"));

        return true;

    }

    [CallbackMethod]
    public string Translate(string Text, string From, string To, string Service)
    {
        Service = Service.ToLower();

        TranslationServices Translate = new TranslationServices();
        Translate.TimeoutSeconds = 10;

        string Result = null;
        if (Service == "google")
            Result = Translate.TranslateGoogle(Text, From, To);
        else if (Service == "babelfish")
            Result = Translate.TranslateBabelFish(Text, From, To);

        if (Result == null)
            Result = Translate.ErrorMessage;

        return Result;
    }

    [CallbackMethod]
    public bool DeleteResourceSet(string ResourceSet)
    {
#if OnlineDemo
        throw new ApplicationException(Res("FeatureDisabled"));
#endif

        return this.Manager.DeleteResourceSet(ResourceSet);
    }

    [CallbackMethod]
    public bool RenameResourceSet(string OldResourceSet, string NewResourceSet)
    {
#if OnlineDemo
        throw new ApplicationException(Res("FeatureDisabled"));
#endif

        return this.Manager.RenameResourceSet(OldResourceSet, NewResourceSet);
    }

    [CallbackMethod]
    public void ReloadResources()
    {
        //Westwind.Globalization.Tools.wwWebUtils.RestartWebApplication();
        wwDbResourceConfiguration.ClearResourceCache();
    }

    [CallbackMethod]
    public bool Backup()
    {
        return this.Manager.CreateBackupTable(null);
    }

    #endregion

    #region Localization Helper Functions

    /// <summary>
    /// Local Resource Help Function for easier syntax
    /// </summary>
    /// <param name="ResourceKey"></param>
    /// <param name="ResourceSet"></param>
    /// <returns></returns>
    public string Res(string ResourceKey)
    {
        string Value = this.GetLocalResourceObject(ResourceKey) as string;
        if (Value == null)
            return ResourceKey;

        return Value;
    }

    /// <summary>
    /// Returns a global Resource Key
    /// </summary>
    /// <param name="ResourceKey"></param>
    /// <returns></returns>   
    public string GRes(string ResourceKey, string ResourceSet)
    {
        string Value = this.GetGlobalResourceObject(ResourceSet, ResourceKey) as string;
        if (Value == null)
            return ResourceKey;

        return Value;
    }

    /// <summary>
    /// Creates a client side compatible string including the quote characters
    /// from a local resource key.
    /// 
    /// This simplifies adding values to client script with code like this:
    /// 
    /// &lt;%= ResC("ResourceID") %&gt;
    /// </summary>
    /// <param name="ResourceKey"></param>
    /// <returns></returns>
    public string ResC(string ResourceKey)
    {
        string Value = Res(ResourceKey);
        return wwWebUtils.EncodeJsString(Value);
    }

    /// <summary>
    /// returns a properly encoded JavaScript string for a global resource
    /// </summary>
    /// <param name="ResourceKey"></param>
    /// <param name="ResourceKey"></param>
    /// <returns></returns>
    public string GResC(string ResourceKey, string ResourceSet)
    {
        string Value = GRes(ResourceKey, ResourceSet);
        return wwWebUtils.EncodeJsString(Value);
    }

    #endregion

}
