<%@ page autoeventwireup="True" inherits="Appleseed.Content.Web.Modules.FCK.filemanager.browse.imagegallery" language="c#" Codebehind="imagegallery.aspx.cs" %>
<%@ Register Assembly="Appleseed.Framework.Web.UI.WebControls" Namespace="Appleseed.Framework.Web.UI.WebControls" TagPrefix="rbfwebui" %>
<!doctype html public "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Insert Image</title>
    <meta content="text/html; charset=windows-1252" http-equiv="Content-Type" />
    <meta content="0" http-equiv="Expires" />
    <style type="text/css">
    BODY { BORDER-RIGHT: 0px; PADDING-RIGHT: 0px; BORDER-TOP: 0px; PADDING-LEFT: 0px; BACKGROUND: #ffffff; PADDING-BOTTOM: 0px; MARGIN: 0px; OVERFLOW: hidden; BORDER-LEFT: 0px; WIDTH: 100%; PADDING-TOP: 0px; BORDER-BOTTOM: 0px }
    BODY { FONT-SIZE: 10pt; COLOR: #000000; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
    TR { FONT-SIZE: 10pt; COLOR: #000000; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
    TD { FONT-SIZE: 10pt; COLOR: #000000; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif }
    DIV.imagespacer { FLOAT: left; MARGIN: 5px; FONT: 10pt verdana; OVERFLOW: hidden; WIDTH: 120px; HEIGHT: 126px; TEXT-ALIGN: center }
    DIV.imageholder { BORDER-RIGHT: #cccccc 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: #cccccc 1px solid; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px; BORDER-LEFT: #cccccc 1px solid; WIDTH: 100px; PADDING-TOP: 0px; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 100px }
    DIV.titleholder { FONT-SIZE: 8pt; OVERFLOW: hidden; WIDTH: 100px; FONT-FAMILY: ms sans serif, arial; WHITE-SPACE: nowrap; TEXT-OVERFLOW: ellipsis }
    </style>

    <script language="javascript" type="text/javascript">
lastDiv = null;
function divClick(theDiv,filename) {
	if (lastDiv) {
		lastDiv.style.border = "1 solid #CCCCCC";
	}
	lastDiv = theDiv;
	theDiv.style.border = "2 solid #316AC5";
	
	document.getElementById("FileToDelete").value = filename;

}
function gotoFolder(rootfolder,newfolder) {
	window.navigate("imagegallery.aspx?frame=1&rif=" + rootfolder + "&cif=" + newfolder);
}		
function returnImage(imagename,width,height) {
	var arr = new Array();
	arr["filename"] = imagename;  
	arr["width"] = width;  
	arr["height"] = height;			 
	window.parent.returnValue = arr;
	window.parent.setImage(imagename) ;
	window.parent.close();	
}		
    </script>

</head>
<body>
    <form id="FORM1" runat="server" enctype="multipart/form-data">
        <asp:panel id="MainPage" runat="server" visible="false">
            <table border="0" cellpadding="0" cellspacing="0" height="100%" width="100%">
                <tr>
                    <td>
                        <div id="galleryarea" style="overflow: auto; width: 100%; height: 100%">
                            <rbfwebui:Label id="gallerymessage" runat="server"></rbfwebui:Label>
                            <asp:panel id="GalleryPanel" runat="server">
                            </asp:panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="16">
                        <asp:panel id="UploadPanel" runat="server">
                            <table>
                                <tr>
                                    <td valign="top">
                                        <input id="UploadFile" runat="server" name="UploadFile" style="width: 300px" type="file" /></td>
                                    <td valign="top">
                                        <rbfwebui:Button id="UploadImage" runat="server" onclick="UploadImage_OnClick" text="Upload" /></td>
                                    <td valign="top">
                                        <rbfwebui:Button id="DeleteImage" runat="server" onclick="DeleteImage_OnClick" text="Delete" /></td>
                                    <td valign="middle"></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:regularexpressionvalidator id="FileValidator" runat="server" controltovalidate="UploadFile"
                                            display="dynamic"></asp:regularexpressionvalidator>
                                        <rbfwebui:Localize id="ResultsMessage" runat="server">
                                        </rbfwebui:Localize></td>
                                </tr>
                            </table>
                            <input id="FileToDelete" runat="server" type="hidden" />
                            <input id="RootImagesFolder" runat="server" type="hidden" value="images" />
                            <input id="CurrentImagesFolder" runat="server" type="hidden" value="images" />
                        </asp:panel>
                    </td>
                </tr>
            </table>
        </asp:panel>
        <asp:panel id="iframePanel" runat="server">
            <iframe border="0" frameborder="0" src="imagegallery.aspx?frame=1&amp;<%=Request.QueryString%>"
                style="border-right: 0px; border-top: 0px; border-left: 0px; width: 100%; border-bottom: 0px;
                height: 100%"></iframe>
        </asp:panel>
    </form>
</body>
</html>
