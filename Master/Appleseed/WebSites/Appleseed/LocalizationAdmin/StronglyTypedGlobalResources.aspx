<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" 
                       Inherits="LocalizationAdmin_StronglyTypedGlobalResources" 
                       Culture="auto" meta:resourcekey="Page" UICulture="auto" Codebehind="StronglyTypedGlobalResources.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Generate Strongly Typed Resources</title>
    <link href="LocalizeForm.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div class="gridheader" style="padding-bottom:4px;font-size:14pt;Height:24px;margin-bottom:4px;"><asp:Label ID="lblHeader" runat="server" Text="Create Strongly Typed Global Resources" meta:resourcekey="lblHeader"></asp:Label></div>
    <asp:HyperLink runat="server" ID="lnkAdmin" Text="Resource Administration" NavigateUrl="~/LocalizationAdmin/default.aspx" meta:resourcekey="lnkAdmin"></asp:HyperLink> | 
    <asp:LinkButton runat="server" ID="lnkRefresh" Text="Refresh" meta:resourcekey="lnkRefresh" />
    
    <div style="margin-left:20px;margin-top:20px">
    
    <asp:Localize runat="server" ID="lblWelcomeMessage" meta:resourcekey="lblWelcomeMessage" Text="&#13;&#10;    This page allows you to create one or more strongly typed resource classes from your global resources.&#13;&#10;    Please specify an output file path for the resulting class for this project. Use ASP.NET style syntax&#13;&#10;    to specify.&#13;&#10;    "></asp:Localize>
   <br />
   <br />
            <asp:Label ID="lblOutputFile" runat="server" Text="Output File Location:  (.cs or .vb file - project relative)" meta:resourcekey="lblOutputFile"></asp:Label>&nbsp;&nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp;<asp:RadioButtonList ID="lstExportFrom" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" meta:resourcekey="lstExportFrom">
            <asp:ListItem Selected="True" meta:resourcekey="ListItem">ResX</asp:ListItem>
            <asp:ListItem meta:resourcekey="ListItem1">wwDbResourceManager</asp:ListItem>
        </asp:RadioButtonList>
            <br />
            <asp:TextBox ID="txtOutputFile" runat="server" Width="619px" meta:resourcekey="txtOutputFile">~/app_code/Resources.cs</asp:TextBox>&nbsp;<br />
        <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" meta:resourcekey="btnGenerate" />        
            <hr />
            <div class="errordisplay" style="padding:10px;">
            <pre>
<asp:Label ID="lblGenetatedCode" runat="server"                       
                        style="font-family:Monospace;font-size:10pt;color:darkblue;" meta:resourcekey="lblGenetatedCode"></asp:Label></pre>
            </div>
            <br />
    </div>        
    </form>
</body>
</html>
