<%@ control autoeventwireup="false" inherits="Appleseed.Content.Web.Modules.FileDirectoryTree"
    language="c#" Codebehind="FileDirectoryTree.ascx.cs" %>
<!-- *** File Directory Tree *** -->
<asp:placeholder id="myPlaceHolder" runat="server"></asp:placeholder>
<script language="javascript" type="text/javascript">
	function Toggle(myObject) 
	{
		object = document.getElementById(myObject);
		mySpan = document.getElementById(myObject + "_img");
		if (object.style.display == 'inline'){
			object.style.display = 'none';
			mySpan.src = baseImg+'dir.gif';
		} else {
			object.style.display = 'inline';
			mySpan.src = baseImg+'dir_open.gif';
		}
	} 
</script>
<!-- *** End of File Directory Tree *** -->
