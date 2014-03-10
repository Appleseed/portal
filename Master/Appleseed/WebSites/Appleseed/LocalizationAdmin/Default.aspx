<%@ Page Language="C#" AutoEventWireup="true" Inherits="LocalizeAdmin_Default" EnableViewState="false" 
         ValidateRequest="false" EnableEventValidation="false" Theme="" Culture="auto" meta:resourcekey="Page1" UICulture="auto" Codebehind="Default.aspx.cs" %>
<%@ Register Assembly="Westwind.Globalization" Namespace="Westwind.Globalization" TagPrefix="ww" %>
<%@ Register Assembly="Westwind.Web.Controls" Namespace="Westwind.Web.Controls" TagPrefix="ww" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Application Resource Localization</title>
    <link href="LocalizeForm.css" rel="stylesheet" type="text/css" />  
</head>
<body>    
    <form id="form1" runat="server">    
    <br />
        <ww:wwErrorDisplay ID="ErrorDisplay" runat="server" 
                    DisplayTimeout="5000" width="600px" 
                    meta:resourcekey="ErrorDisplay" 
                    UseFixedHeightWhenHiding="False">
        </ww:wwErrorDisplay>
    <center>
    <div class="blackborder" style="background:#eeeeee;width:820px;text-align:left;">
    
    <div class="gridheader" style="font-size:large">
        <asp:Label runat="server" id="lblPageHeader" Text="Resource Localization" Title="Resource List (Alt-L)" meta:resourcekey="lblPageHeader" />
    </div>
    <table cellspacing="0" style=" width:820px;">    
    <tr>
        <td valign="top">
             <asp:Textbox runat="server" ID="txtResourceFilter"></asp:Textbox>
             <asp:Button runat="server" ID="btnResourceFilter" Text="Filter" OnClientClick="GetResourceList();return false;"  meta:resourcekey="btnResourceFilter" />
             <br />
             <asp:ListBox runat="server" ID="lstResourceIds"  
                          style="Height:505px;width:275px;" 
                          onchange="GetValue()" AccessKey="L" meta:resourcekey="lstResourceIds" >
             </asp:ListBox>
        </td>
        <td valign="top"> 
            <div id="Toolbar">
           
            <div id="divToolbar" class="toolbarbar">            
            <span class="hoverbutton">
                <img runat="server" id="imgExportResources" />
                <asp:LinkButton runat="server" ID="btnExportResources" Title="Export to Resource Files" Text="Export to Resource Files"  
                                OnClick="btnExportResources_Click" OnClientClick="if (!confirm(ResourceGenerationConfirmation)) return false;" meta:resourcekey="btnExportResources" />
             </span>
            
            <span class="hoverbutton"> 
                <img runat="server" id="imgImport"  />
                <asp:LinkButton runat="server" ID="btnImport" Title="Import Resources" Text="Import" onclick="btnImport_Click" meta:resourcekey="btnImport" />
            </span>                   
            
            <span class="hoverbutton">
                <img runat="server" id="imgCreateTable" visible="false" />
                <asp:LinkButton runat="server" ID="btnCreateTable" Title="Create" Text="Create Table" OnClick="btnCreateTable_Click"  meta:resourcekey="btnCreateTable" />
            </span>
            
            <span class="hoverbutton">            
                <img runat="server" id="imgBackup" />
                <asp:LinkButton runat="server" ID="btnBackup" Title="Backup Resource Database" Text="Backup Table" OnClientClick="Backup();return false;" meta:resourcekey="btnBackup"></asp:LinkButton>
            </span>
            </div>
            <div id="divToolbar2" class="toolbarbar">
             <span class="hoverbutton">            
                <img runat="server" id="imgRefresh" />
                <asp:LinkButton runat="server" ID="btnRefresh" Title="Refresh Page" Text="Refresh Page" meta:resourcekey="btnRefresh"></asp:LinkButton>
            </span>
            
            <span class="hoverbutton">            
                <img runat="server" id="imgRecycleApp" />
                <asp:LinkButton runat="server" ID="btnRecycleApp" Title="Recycle Application" AccessKey="R" Text="Recyle App" OnClientClick="ReloadResources();return false;" meta:resourcekey="btnRecycleApp"></asp:LinkButton>&nbsp;&nbsp;
            </span>            
            </div>
            </div>
            <hr /> 
            <b><asp:Label runat="server" id="lblResourceSetLabel" Text="ResourceSet:" meta:resourcekey="lblResourceSetLabel"></asp:Label></b>
            <br />
            <asp:DropDownList runat="server" ID="lstResourceSet" width="450px" AutoPostBack="True" meta:resourcekey="lstResourceSet"></asp:DropDownList>
            <a href="javascript:DeleteResourceSet();" class="hoverbutton"><asp:Image  runat="server"  ID="imgDeleteResourceSet" Title="Delete ResourceSet" meta:resourcekey="imgDeleteResourceSet" /></a>
            <a href="javascript:ShowResourceSetRenameDisplay();" class="hoverbutton"><asp:Image  runat="server"  ID="imgRenameResourceSet" Title="Rename ResourceSet" meta:resourcekey="imgRenameResourceSet" /></a>
            <br />
            <br />
            <asp:Label runat="server" ID="lblLanguages" Font-Bold="True" meta:resourcekey="lblLanguages" Text="Languages:"></asp:Label>
            <br />
            <asp:ListBox ID="lstLanguages" runat="server" style="" Height="90px" Width="500px" onchange="GetValue()" meta:resourcekey="lstLanguages">
            </asp:ListBox>
            <br />
            <br />
            <asp:Label runat="server" ID="lblValue" Font-Bold="True" meta:resourcekey="lblValue" Text="Value:"></asp:Label><br />
            <asp:TextBox runat="server" ID="txtValue" TextMode="MultiLine" Height="70px" Width="500px" AccessKey="V" Tooltip="Resource Value (Alt-V)" meta:resourcekey="txtValue"></asp:TextBox>
            <br />
            <asp:Button runat="server" id="btnSaveValue" Text="Save" UseSubmitBehavior="False" AccessKey="S" OnClientClick="SaveValue(); return false;" meta:resourcekey="btnSaveValue" />
            
            <asp:Button runat="server" ID="btnAddResourceDisplay"  Text="Add" OnClientClick="ShowNewResourceDisplay();return false;" AccessKey="a" meta:resourcekey="btnAddResourceDisplay"  />
            <asp:Button runat="server" ID="btnDelete"  Text="Delete" OnClientClick="DeleteResource();return false;" accesskey="d" meta:resourcekey="btnDelete" />
            <asp:Button runat="server" ID="btnRename"  Text="Rename" OnClientClick="ShowRenameResourceDisplay();return false;" meta:resourcekey="btnRename" />
            <asp:Button runat="server" id="btnTranslate" Text="Translate" OnClientClick="ShowTranslationDisplay();return false" meta:resourcekey="btnTranslate" />
            <br />
            <br />            
            <div id="divValues" class="gridalternate" style="padding:5px;width:490px;border:solid 2px steelblue;height:140px;overflow-y:scroll;display:none;">
            ...
            </div>            
        </td>
        </tr>
    </table>
    <div id="lblMessages" style="background:lightgrey;padding-left:8px;padding-top:4px;padding-bottom:4px;color:Maroon;border:solid 1px gray">    
    <%= Res("Ready") %>     
    </div> 
     
     <%--New Resource Panel--%>
     <ww:wwDragPanel ID="panelNewResource" runat="server" CssClass="gridalternate blackborder" 
                     style="top:200px;left:180px;width:525px;display:none;"  
                     Closable="true"
                     ShadowOffset="5"
                     DragHandleId="NewResourceHeader"
                     meta:resourcekey="panelNewResource">
        <div class='gridheader' runat="server" id="NewResourceHeader">        
        <asp:Label runat="server" ID="lblNewResourceHeader" Text="Add Resource" meta:resourcekey="lblNewResourceHeader"></asp:Label>
        </div>
        <div class="dialogcontent">
            <table border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td><asp:Label runat="server" ID="lblNewLanguage" Text="Lang:" meta:resourcekey="lblNewLanguage"></asp:Label></td>
                <td>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblNewResourceId" Text="Resource Id:" meta:resourcekey="lblNewResourceId"></asp:Label></td></tr>
            <tr>
                <td><asp:TextBox ID="txtNewLanguage" runat="server" Width="56px" meta:resourcekey="txtNewLanguage" /></td>
                <td>&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNewResourceId" runat="server" Width="407px" meta:resourcekey="txtNewResourceId" /></td>
            </tr>
            </table>
            <br />
          
            <asp:Label ID="lblNewValue" runat="server" Text="Value:" meta:resourcekey="lblNewValue"></asp:Label><br />
            <asp:TextBox ID="txtNewValue" runat="server" TextMode="MultiLine" Width="480px" meta:resourcekey="txtNewValue"></asp:TextBox><br />
            <br />
            <asp:Label ID="lblNewResourceSet" runat="server" Text="ResourceSet:" meta:resourcekey="lblNewResourceSet"></asp:Label>
            <asp:TextBox runat="server" ID="txtNewResourceSet" Width="480px" meta:resourcekey="txtNewResourceSet"></asp:TextBox>
            
            <asp:Button runat="server" ID="btnAddValue" Text="Add Resource" onClientClick="AddResource();return false;" meta:resourcekey="btnAddValue" />
            
            <hr />
            <asp:Label runat="server" ID="lblFileUpload" Text="File Resource Upload:" meta:resourcekey="lblFileUpload"></asp:Label><br />
            <asp:FileUpload ID="FileUpload" runat="server" style="width:410px"  meta:resourcekey="FileUpload" />
            <asp:Button runat="server" ID="btnFileUpload" OnClick="btnFileUpload_Click" Text="Upload" meta:resourcekey="btnFileUpload"/>    
        </div>                
    </ww:wwDragPanel> 
    <%--End New Resource Panel--%>
    
    <%--Rename Panel--%>
    <ww:wwDragPanel ID="panelRename" runat="server" CssClass="gridalternate blackborder" DragHandleID="Header2"  Closable="true"
               style="top:230px;left:140px;width:330px;display:none;" meta:resourcekey="panelRename1"  ShadowOffset="5" >
        <div class='gridheader' runat="server" id="Header2">        
        <asp:Label runat="server" ID="lblRenameResourceHeader" Text="Rename Resource" meta:resourcekey="lblRenameResourceHeader" ></asp:Label></div>
        <div style="padding:5px;">
        <b><asp:Label runat="server" ID="lblResourceToRename_Label" meta:resourcekey="lblResourceToRename_Label" Text="ResourceKey to Rename:"></asp:Label></b><br />
        <asp:TextBox runat="server" ID="txtResourceToRename" width="300px" meta:resourcekey="txtResourceToRename"></asp:TextBox>
        <br />
        <br />
        <b><asp:Label runat="server" ID="lblRenamedResource" meta:resourcekey="lblRenamedResource" Text="ResourceKey to Rename:"></asp:Label></b><br />
        <asp:TextBox runat="server" ID="txtRenamedResource" Width="300px" meta:resourcekey="txtRenamedResource"></asp:TextBox><br />
        <asp:CheckBox ID="chkPropertyRename" runat="server" Text="Rename all properties for key"
                Width="300px" meta:resourcekey="chkPropertyRename"/><br />
        <hr />
        <asp:Button runat="server" ID="btnRenameResourceKey" Text="Rename" UseSubmitBehavior="False" OnClientClick="RenameResource();return false;" meta:resourcekey="btnRenameResourceKey" />
        </div>
    </ww:wwDragPanel>                
    <%--End Rename Panel--%>          
    
    <%-- Translate Panel --%>
    <ww:wwDragPanel ID="panelTranslate" runat="server" CssClass="gridalternate blackborder" 
               closable="true" DragHandleID="TranslateHeader" ShadowOffset="5"
               style="top:305px;left:10px;width:383px;display:none;" meta:resourcekey="panelTranslate">
        <div class='gridheader' runat="server" id="TranslateHeader">
        <%--<div style="float:right;color:White;font-size:15px;font-weight:bolder;color:White;" onclick="HideTranslationDisplay();" title="Close Window">X</div>--%>
        <asp:label runat="server" ID="lblTranslateHeader" Text="Web Service Translation" meta:resourcekey="lblTranslateHeader"></asp:label>
        </div>
        <div class="dialogcontent">
        <asp:Label runat="server" ID="lblTranslateFrom" Text="From:" meta:resourcekey="lblTranslateFrom"></asp:Label>
            <asp:TextBox runat="server" ID="txtTranslateFrom" Text="en" Width="40px" meta:resourcekey="txtTranslateFrom"></asp:TextBox>&nbsp;
            <asp:Label runat="server" ID="lblTranslateTo" Text="To:" meta:resourcekey="lblTranslateTo"></asp:Label>
            <asp:TextBox runat="server" ID="txtTranslateTo" Text="de" Width="40px" meta:resourcekey="txtTranslateTo"></asp:TextBox>
            <br />
            <br />
            <asp:Label runat="server" ID="lblTranslationInputText" Text="Input Text:" meta:resourcekey="lblTranslationInputText" ></asp:Label><br />
            <asp:TextBox runat="server" ID="txtTranslationInputText" TextMode="MultiLine" Width="300px" meta:resourcekey="txtTranslationInputText"></asp:TextBox>
            <input type="button" runat="server" ID="btnTranslateSubmit" Value="Go" onclick="Translate()"/>
            <hr />                    
            <asp:HyperLink runat="server" ID="lblGoogle" Text="Google Translation:" Target="_Translate" 
                           NavigateUrl="http://www.google.com/translate_t" meta:resourcekey="lblGoogle" />
            <asp:TextBox runat="server" ID="txtGoogle" TextMode="MultiLine" Width="300px" meta:resourcekey="txtGoogle"></asp:TextBox>
            <input type="button" runat="server" ID="btnUseGoogle" value="Use"  onclick="UseTranslation('Google');"/>
            <br />            
            <asp:HyperLink runat="server" ID="lblBabelFish" Text="Babelfish/Yahoo Translation:" Target="_Translation" NavigateUrl="http://babelfish.yahoo.com/translate_txt" meta:resourcekey="lblBabelFish" style="margin-top:8px; display:block;"/>
            <asp:TextBox runat="server" ID="txtBabelFish" TextMode="MultiLine" Width="300px" meta:resourcekey="txtBabelFish" ></asp:TextBox>
            <input runat="server" type="button" ID="btnBabelFish" value="Use"  onclick="UseTranslation('BabelFish');"/>
        </div>
    </ww:wwDragPanel>        
    <%-- End Translate Panel --%>
    
    <%--  Rename Resource Set Window --%>
    <ww:wwDragPanel ID="panelRenameResourceSet" runat="server" CssClass="gridalternate blackborder" 
               Closable="true" DragHandleId="RenameResourceSetHeader"
               style="top:135px;left:300px;width:365px;display:none;font-size:8pt;" meta:resourcekey="panelRenameResourceSet">
        
        <div runat="server" id="RenameResourceSetHeader" class='gridheader'>
        <%--<div style="float:right;color:White;font-size:15px;font-weight:bolder;color:White;" onclick="HideResourceSetRenameDisplay();" title="Close Window">X</div>--%>
        <asp:label runat="server" ID="lblRenameResourceSetHeader" Text="Rename ResourceSet" meta:resourcekey="lblRenameResourceSetHeader"></asp:label>
        </div>
        <div style="padding:15px">
            <asp:Label runat="server" ID="lblOldResourceSet" Text="Old ResourceSet:" meta:resourcekey="lblOldResourceSet"></asp:Label><br />
            <asp:TextBox runat="server" id="txtOldResourceSet" width="300px" meta:resourcekey="txtOldResourceSet"></asp:TextBox>              
            <br />
            <br />
            <asp:Label runat="server" ID="lblRenamedResourceSet" Text="New ResourceSet:" meta:resourcekey="lblRenamedResourceSet"></asp:Label><br />
            <asp:TextBox runat="server" id="txtRenamedResourceSet" width="300px" meta:resourcekey="txtRenamedResourceSet"></asp:TextBox>                                      
            <hr />
            <asp:Button runat="server" ID="btnRenameResourceSet" Text="Rename ResourceSet" onclick="btnRenameResourceSet_Click" meta:resourcekey="btnRenameResourceSet" />
         </div>
    </ww:wwDragPanel>
    <%--  End Rename Resource Set Window --%>
    </div>  
    </center>
    
    <ww:wwDbResourceControl id="ResourceAdmin" runat="server" meta:resourcekey="ResourceAdmin" 
                            CssClass="errordisplay" Width="250px" 
                            visible="false"> 
    </ww:wwDbResourceControl>
    <br />

    <!-- The Ajax Callback control that drives all the method callbacks to page methods -->
    <ww:wwMethodCallback ID="Callback" runat="server"  />   
    
    <%--<div id="divDebug" class="errordisplay" style="font: normal normal 8pt;"></div> --%>
    
<script type="text/javascript">
//_debug.setOutput("divDebug");

var panelNewResource =  new wwControl("panelNewResource");
var panelRename = new wwControl("panelRename");
var panelTranslate = new wwControl("panelTranslate");
var panelRenameResourceSet = new wwControl("panelRenameResourceSet");

wwEvent.addEventListener(window,"load",OnLoad);

function OnLoad()
{
    var List = $w("lstResourceIds");
    if (List.options.length > 0)
       List.onchange();
}
function ShowNewResourceDisplay()
{
    var Ctl = panelNewResource;
    Ctl.control.style.position = "absolute";
    Ctl.show();
    Ctl.showShadow();        
    
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;
       
    var Language = $w('lstLanguages').value;
    
    var Ctl = new wwControl("txtNewLanguage");
    Ctl.setText(Language);            
    
    Ctl = new wwControl("txtNewResourceId");
    Ctl.setText(ResourceId);
    
    Ctl = new wwControl("txtNewResourceSet");
    Ctl.setText( $w("lstResourceSet").value );
}
function ShowRenameResourceDisplay()
{
    var Ctl = panelRename;
    Ctl.control.style.position = "absolute";
    Ctl.show();
    Ctl.showShadow();
    
    
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;
       
    Ctl = new wwControl("txtResourceToRename");
    Ctl.setText(ResourceId);
    
    Ctl = new wwControl("txtRenamedResource");
    Ctl.setText( ResourceId );
}
function ShowTranslationDisplay()
{
    var Ctl1 = new wwControl("txtValue");
    var Ctl2 = new wwControl("txtTranslationInputText");
    
    if (Ctl1 && Ctl2)
        Ctl2.setText( Ctl1.getText() );

    var Ctl1 = new wwControl("lstLanguages");
    var Ctl2 = new wwControl("txtTranslateFrom");
    
    var Text = Ctl1.control.value;
    if (Text == "")
       Text = "en";
    if (Text.length > 2)
       Text = Text.substring(0,2);
    
    Ctl2.setText(Text);
    
    var Ctl = panelTranslate;
    Ctl.control.style.position = "absolute";
    Ctl.show();
    Ctl.showShadow();
}
function ShowResourceSetRenameDisplay()
{
    var OldResourceSet = $w("lstResourceSet").value;
    if (OldResourceSet == null || OldResourceSet == "")
       return;
       
    $w("txtOldResourceSet").value = OldResourceSet;
    
    var Ctl = panelRenameResourceSet;
    Ctl.show();
    Ctl.showShadow();
}
function HideResourceSetRenameDisplay()
{
      panelRenameResourceSet.hide(); 
      panelRenameResourceSet.hideShadow();
}
function PreviewPanelDisplay(Show)
{
    if (Show)
        $w("divValues").style.display='';
    else
        $w("divValues").style.display='none';
}
function GetValue()
{            
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;

    var ResourceSet = $w("lstResourceSet").value;
           
    var LocaleId = $w("lstLanguages").value;
    if (LocaleId == null)
       LocaleId = "";
        
    Callback.GetResourceString(ResourceId,ResourceSet,LocaleId,GetResourceString_Callback );
    
    // *** Also load the preview list
    GetResourceStrings();
}
function GetResourceString_Callback(Result)
{
    if (Result == null)
        Result = "";
    
    $w("txtValue").value = Result;    
    panelNewResource.hide();     
}
function GetResourceStrings()
{
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;

    var ResourceSet = $w("lstResourceSet").value;

    Callback.GetResourceStrings(ResourceId,ResourceSet,GetResourceStrings_Callback );
}
function GetResourceStrings_Callback(Resources)
{
    // returns a two col array
    if (Resources == null)
    {
        this.ShowMessage(<%= ResC("NoResourcesAvailable") %>);
        return;
    }

    var ResourceId = $w("lstResourceIds").value;
            
    var Output = "<div style='margin-top:3px; padding-bottom: 5px;border-bottom:solid 1px steelblue;color: maroon;font-weight:bold;'>" +
                 ResourceId + "</div>";
    for( var x = 0; x < Resources.length; x++)
    {
        var Resource = Resources[x];
        if (Resource.Key == "")
            Resource.Key = "Invariant"
            
        Output = Output +  "<div style='padding:5px;border-bottom:solid 1px steelblue;'><a href='javascript:ChangeLanguage(\"" + Resource.Key  + "\");' >" + Resource.Key + "</a>: " + Resource.Value + "</div>\r\n";
    }
    
    $w("divValues").innerHTML = Output;
    PreviewPanelDisplay(true);
}
function ChangeLanguage(Lang)
{
    if (Lang == "Invariant")
        Lang = "";
    
    var Ctl = $w("lstLanguages");
        
    for(var x = 0; Ctl.options.length; x++)
    {
        if (Ctl.options[x].value == Lang)
        {
            Ctl.value = Lang;
            Ctl.onchange();
            return;
        }
    }
}
function SaveValue()
{  
    var Value = $w("txtValue").value;
    
    if ( (Value == null || Value == "") && !confirm(<%= ResC("AreYouSureYouWantToRemoveValue") %>) )
        return;
    
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;

    var ResourceSet = $w("lstResourceSet").value;

    var LocaleId = $w("lstLanguages").value;
    if (LocaleId == null)
       LocaleId = "";
           
    Callback.UpdateResourceString(Value,ResourceId,ResourceSet,LocaleId,SaveValue_Callback,OnPageError); 
    
    SetResourceIdValue = ResourceId;    
}
function SaveValue_Callback(Result)
{
    if (Result)    
    {
        ShowMessage(<%= ResC("ResourceUpdated") %>,5000);            
            
        // *** Select item
        if (SetResourceIdValue)
        {
           var List = new wwList("lstResourceIds");           
           List.control.value = SetResourceIdValue;
           List.control.onchange();
           SetResourceIdValue = null;
        }
    }
   
    else
        ShowMessage(<%= ResC("ResourceUpdateFailed") %>,5000); 
   

}
function AddResource()
{
    var Value = $w("txtNewValue").value;    
    
    if ( (Value == null || Value == "") )
    {
        alert(<%= ResC("NoValueEntered") %>);
        return;
    }
    
    var ResourceId = $w("txtNewResourceId").value;
    if ( (Value == null || Value == "") )
    {
        alert(<%= ResC("NoValueEntered") %>);
        return;
    }

    var ResourceSet = $w("txtNewResourceSet").value;
           
    var LocaleId = $w("txtNewLanguage").value;
    if (LocaleId == null)
       LocaleId = "";
       
    Callback.UpdateResourceString(Value,ResourceId,ResourceSet,LocaleId,AddResource_Callback,OnPageError);        
    
    // *** Force list to update
    SetResourceIdValue = ResourceId;
}
function AddResource_Callback(Result)
{
    if (Result)
    {
        ShowMessage(<%= this.ResC("ResourceUpdated") %>,5000);
        GetResourceList();                
        panelNewResource.hideShadow();        
    }
    else
        ShowMessage(<%= ResC("ResourceUpdateFailed") %>,5000);   
}
function DeleteResource()
{
    var ResourceId = $w("lstResourceIds").value;
    if (ResourceId == null || ResourceId == "")
       ResourceId = $w("lstResourceIds").options[0].value;

    var ResourceSet = $w("lstResourceSet").value;
           
    var LocaleId = $w("lstLanguages").value;
    if (LocaleId == null)
       LocaleId = "";
    
    Callback.DeleteResource(ResourceId,ResourceSet,LocaleId,DeleteResource_Callback,OnPageError);
}
function DeleteResource_Callback(Result)
{
    if (Result)
    {
        ShowMessage("Resource has been deleted",3000);
        GetResourceList();
    }
}
function RenameResource()
{
    var ResourceId = $w("txtResourceToRename").value;
    if (ResourceId == null || ResourceId == "")
    {
       ShowError( <%= ResC("InvalidResourceId") %> );
       return; 
    }
    
    var RenameProperty = $w("chkPropertyRename").checked;
    
    var ResourceSet = $w("lstResourceSet").value;    
    var NewResourceId = $w("txtRenamedResource").value;
    
    if (RenameProperty)
    {    
        Callback.RenameResourceProperty(ResourceId,NewResourceId,ResourceSet,RenameResource_Callback,OnPageError);
        
        // *** Force list to update
        SetResourceIdValue = NewResourceId + ".Text";
    }
    else
    {
        Callback.RenameResource(ResourceId,NewResourceId,ResourceSet,RenameResource_Callback,OnPageError);
        
        // *** Force list to update
        SetResourceIdValue = NewResourceId;
    }
}
function RenameResource_Callback(Result)
{
    panelRename.hide();         

    if (Result)
    {
        ShowMessage(<%= ResC("ResourceUpdated") %>,5000);
        GetResourceList();
    }
    else
        ShowMessage(<%= ResC("ResourceUpdateFailed") %>,5000);
   
}
var SetResourceIdValue = null;
function GetResourceList()
{
    var ResourceSet = $w("lstResourceSet").value;
    var ResourceFilter = $w("txtResourceFilter").value;
    Callback.GetResourceList(ResourceSet, ResourceFilter, GetResourceList_Callback,OnPageError);
}
function GetResourceList_Callback(Table)
{
    // *** Callback result is a DataTable
    var List = new wwList("lstResourceIds");
    
    // *** Bind it to the list
    List.dataValueField = "ResourceId";
    List.dataTextField = "ResourceId";
    List.setData(Table);
    
    // *** Select item
    if (SetResourceIdValue)
    {
       List.control.value = SetResourceIdValue;
       List.control.onchange();
       SetResourceIdValue = null;
    }
    
}
function DeleteResourceSet()
{
    var ResourceSet = $w("lstResourceSet").value;
    
    if (!confirm("Are you sure you want to delete this resource set?\r\n" + ResourceSet) )
       return;      
     
    Callback.DeleteResourceSet(ResourceSet,DeleteResourceSet_Callback,OnPageError);      
}
function DeleteResourceSet_Callback(Result)
{
   if (Result)
        ShowMessage("ResourceSet deleted...",5000);
   else
       ShowMessage("ResourceSet deletion failed...",5000);
}
function Translate()
{
    var Value = $w("txtTranslationInputText").value;
    var From = $w("txtTranslateFrom").value;
    var To = $w("txtTranslateTo").value;
    
    if (To == From)
    {
       ShowMessage("Please select two separate languages to translate")
       return;
    }
    
    var Loading = <%= ResC("Loading") %>  + "...";       
    $w("txtGoogle").value = Loading;
    $w("txtBabelFish").value = Loading;
    
    // *** Use two separate instances here so there won't be any interference in callbackss   
    Callback.Translate(Value,From,To,"Google",TranslateGoogle_Callback,OnPageError);    
    Callback.Translate(Value,From,To,"BabelFish",TranslateBabelFish_Callback,OnPageError);
}
function TranslateGoogle_Callback(Result)
{
    var Ctl = new wwControl("txtGoogle");
    Ctl.setText( Result );
}
function TranslateBabelFish_Callback(Result)    
{
    var Ctl = new wwControl("txtBabelFish");
    Ctl.setText( Result );
}
function UseTranslation(Service)
{
    var Ctl = $w("txt" + Service);
    var Text = Ctl.value;
    if (Text == null || Text == "")
       return;
       
    Ctl = $w("txtTranslateTo");
    Lang = Ctl.value;
    
    var CtlValue = new wwControl("txtValue");
    
    Ctl = $w("lstLanguages");
    
    if (Ctl.value == Lang)
    {
        CtlValue.setText(Text);
        HideTranslationDisplay();
        return;
    }
    
    for(var x = 0; x < Ctl.options.length; x++)
    {
        var Item = Ctl.options[x];
        if (Item.value == Lang )
        {
           Ctl.value = Lang;
           CtlValue.setText(Text);
           HideTranslationDisplay();
           return;
        }
    }   
    
    ShowMessage(<%= ResC("NoMatchingLanguage") %>);
}
function HideTranslationDisplay()
{
    panelTranslate.hide();
}
function ReloadResources()
{
    Callback.ReloadResources(ReloadResources_Callback);
}
function ReloadResources_Callback(Result)
{
    ShowMessage(<%= ResC("ResourcesReloaded") %>,5000);
}
function Backup()
{
    if ( !confirm( <%= ResC("BackupNotification") %> ) )
        return;
        
    Callback.Backup(Backup_Callback,OnPageError);        
}
function Backup_Callback(Result)
{
    if (Result)
       ShowMessage(  <%= ResC("BackupComplete") %>,5000 );
    else
       ShowMessage( <%= ResC("BackupFailed")  %>,5000);    
}
function OnPageError(Error)
{   
    ShowError(Error.message,7000);
}
function ShowMessage(Message,Timeout)
{
    var Ctl = new wwControl("lblMessages");
    if (Message == null || Message == "")
       Message = " ";
    Ctl.setText( Message);
    
    if (Timeout)
    {
        Ctl.element.style.background = "cornsilk";
        setTimeout(HideMessage,Timeout);
    }
}

function ShowError(Message,Timeout)
{
    var Ctl = new wwControl("lblMessages");
    Ctl.setText( Message);
    
    if (Timeout)
    {
        Ctl.element.style.background = "red";
        Ctl.element.style.color = "white"; 
        Ctl.element.style.fontWeight = "bold";       
        setTimeout(HideMessage,Timeout);
    }
}

function HideMessage()
{
    var Ctl = new wwControl("lblMessages");
    Ctl.setText(<%= ResC("Ready") %>);
    Ctl.element.style.background="";
    Ctl.element.style.color="";     
    Ctl.element.style.fontWeight = "";   
}
var ResourceGenerationConfirmation = <%= ResC("ResourceGenerationConfirmation") %>;
</script>    

    </form>
</body>
</html>
