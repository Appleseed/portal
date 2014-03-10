EnhancedHtml Module
===================

This module renders a list of localized html pages
NOTE: See list of keys and style classes at the end of this file!

Credits: 

José Viladiu jviladiu@portalServices.net  http://www.portalServices.net

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net

INSTALL
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings:
5. Use it! ;o) 

HISTORY
Ver. 1.0 - 16. June. 2004. Released by José Viladiu

Issues and Known problems:
- Tested with Appleseed version 1.4.0.1765 - 16/06/2004
For better performance is necessary to add in method FillPortalDS from PortalSearch.ascx.cs the next case:
	case "875254B7-2471-491F-BAF8-4AFC261CC224":  //EnhancedHtml
	      strLink = HttpUrlBuilder.BuildUrl("~/DesktopDefault.aspx", Convert.ToInt32(strTabID), strLocate);
		  break;

Keys used by this module (for file Resources\Appleseed.??.resx):

  <data name="ENHANCEDHTML_PAGE">
    <value>Page</value>
  </data>
  <data name="ENHANCEDHTML_OF">
    <value>of</value>
  </data>
  <data name="ENHANCEDHTML_SINGLEPAGEMODE">
    <value>Single Page Mode</value>
  </data>
  <data name="ENHANCEDHTML_EDITOR_DESCRIPTION">
    <value>Select the Html Editor for Module</value>
  </data>
  <data name="ENHANCEDHTML_EDITOR">
    <value>Editor</value>
  </data>
  <data name="ENHANCEDHTML_SHOWUPLOAD_DESCRIPTION">
    <value>Only used if Editor is ActiveUp HtmlTextBox</value>
  </data>
  <data name="ENHANCEDHTML_SHOWUPLOAD">
    <value>Upload?</value>
  </data>
  <data name="ENHANCEDHTML_SHOWMULTIMODE_DESCRIPTION">
    <value>Mark this if you like see icon multimode page</value>
  </data>
  <data name="ENHANCEDHTML_SHOWMULTIMODE">
    <value>Show Multi-Mode icon?</value>
  </data>
  <data name="ENHANCEDHTML_SHOWTITLEPAGE_DESCRIPTION">
    <value>Mark this if you like see the Title Page</value>
  </data>
  <data name="ENHANCEDHTML_SHOWTITLEPAGE">
    <value>Show Title Page?</value>
  </data>
  <data name="ENHANCEDHTML_SHOWUPMENU_DESCRIPTION">
    <value>Mark this if you like see a index menu whith the titles of all pages</value>
  </data>
  <data name="ENHANCEDHTML_SHOWUPMENU">
    <value>Show Index Menu?</value>
  </data>
  <data name="ENHANCEDHTML_ALIGNUPMENU_DESCRIPTION">
    <value>Select here the align for index menu</value>
  </data>
  <data name="ENHANCEDHTML_ALIGNUPMENU">
    <value>Align Index Menu</value>
  </data>
  <data name="ENHANCEDHTML_SHOWDOWNMENU_DESCRIPTION">
    <value>Mark this if you like see a navigation menu with previous and next page</value>
  </data>
  <data name="ENHANCEDHTML_SHOWDOWNMENU">
    <value>Show Navigation Menu?</value>
  </data>
  <data name="ENHANCEDHTML_ALIGNDOWNMENU_DESCRIPTION">
    <value>Select here the align for index menu</value>
  </data>
  <data name="ENHANCEDHTML_ALIGNDOWNMENU">
    <value>Align Navigation Menu</value>
  </data>
  <data name="ENHANCEDHTML_ADDINVARIANTCULTURE_DESCRIPTION">
    <value>Mark this if you like see pages with invariant culture after pages with actual culture code</value>
  </data>
  <data name="ENHANCEDHTML_ADDINVARIANTCULTURE">
    <value>Add Invariant Culture?</value>
  </data>
  <data name="ENHANCEDHTML_MULTIPAGEMODE">
    <value>Multi Page Mode</value>
  </data>
  <data name="ENHANCEDHTML_CONFIRMDELETEMESSAGE">
    <value>Are You Sure You Wish To Delete This Item ?</value>
  </data>
  <data name="ENHANCEDHTML_SHOWALLPAGES">
    <value>All Pages</value>
  </data>
  <data name="ENHANCEDHTML_LANGUAGE">
    <value>Language</value>
  </data>
  <data name="ENHANCEDHTML_NEWPAGE">
    <value>New Page</value>
  </data>
  <data name="ENHANCEDHTML_EDITPAGE">
    <value>Edit Page</value>
  </data>
  <data name="ENHANCEDHTML_DELETEPAGE">
    <value>Delete Page</value>
  </data>
  <data name="ENHANCEDHTML_RETURN">
    <value>Return</value>
  </data>
  <data name="ENHANCEDHTML_PAGENAME">
    <value>Page Name</value>
  </data>
  <data name="ENHANCEDHTML_VIEWORDER">
    <value>View Order</value>
  </data>
  <data name="ENHANCEDHTML_UPDATE">
    <value>Update</value>
  </data>
  <data name="ENHANCEDHTML_CANCEL">
    <value>Cancel</value>
  </data>


Style Class examples for use in themes:

/* ================================
   EnhancedHtml Module
   ================================ */

.EnhancedHtmlTitlePage
{
    font-family: Verdana, Helvetica, sans-serif;
    font-size: 12px;
    font-weight: normal;
    color:darkblue;
}

.EnhancedHtmlLink
{
    font-family: Verdana, Helvetica, sans-serif;
    font-size: 11px;
    font-weight: bold;
    color: darkgray
}

A.EnhancedHtmlLink:link
{
    text-decoration: none;
    color: black;
}

A.EnhancedHtmlLink:visited
{
	color: olive;
	text-decoration: none;
}

A.EnhancedHtmlLink:active
{
    text-decoration: none;
    color: green;
}

A.EnhancedHtmlLink:hover
{
    text-decoration: none;
    color: red;
}

