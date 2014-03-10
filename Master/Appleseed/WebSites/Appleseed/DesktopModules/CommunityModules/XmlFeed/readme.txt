XmlFeed Module - RSS Client, XML File and KBAlertz example

IMPORTANT: XML URL must be filled with a valid url due to bug in settings system.

Credits: Manu and Jakob Hansen

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net


INSTALL
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings: See below
5. Use it! ;o) 
Note: The module is automatically installed when you install Appleseed.
The install procedure is only required if you deleted the module in Admin all

Note: The RSS -WithEscaping encode the resulted html, by default. Html is not encoded.

HISTORY
Ver. 1.0 - 1. feb 2003 - First realase by Manu and Jakob Hansen
Ver. 1.1 - 16. april 2003 - Updated to follow "Appleseed best practices"


Issues and Known problems:
- Tested with Appleseed version 1.2.8.1711c
- Version 1.1 only in english
- IMPORTANT: XML URL must be filled with a valid url due to bug in settings system


Module settings - RSS example:
-----------------------------------
XML Type : URL
XML URL  : http://www.news4sites.com/service/newsfeed.php?tech=rss&id=2290
XML File : <empty>
XSL Type : Predefined
XSL Predefined : RSS91  (or RSS2Html)
XSL File : <empty>

For more information on RSS: 
Go to http://www.syndic8.com or http://w.moreover.com/categories/category_list_rss.html
to find an rss news feed you like (9x format only)


Module settings - XML file example: (using the sales example)
-----------------------------------
XML Type : File
XML URL  : <empty>
XML File : \xml\sales.xml
XSL Type : File
XSL Predefined : <empty>
XSL File : \xml\sales.xsl

Module settings - RSS Service ItemList example:
-----------------------------------
XML Type : URL
XML URL  : http://localhost/Appleseed/CommunityRSS.aspx?LT=Item
XML File : <empty>
XSL Type : Predefined
XSL Predefined : RSSItemListWithHead
XSL File : <empty>

Module settings - RSS Service TabList example:
-----------------------------------
XML Type : URL
XML URL  : http://localhost/Appleseed/CommunityRSS.aspx?LT=Tab
XML File : <empty>
XSL Type : Predefined
XSL Predefined : RSSTabListNoHead
XSL File : <empty>


Module settings - KBAlertz example:
-----------------------------------
XML Type : URL
XML URL  : http://www.kbalertz.com/websvc/latestkbs.aspx?t=20&c=20
	   This URL accepts two parameters, T and C. T stands for TechnologyID 
	   and C stands for Count. A full list of available technologies can 
	   be found at http://KBAlertz.com/Webmaster/
XML File : <empty>
XSL Type : Predefined
XSL Predefined : KBAlertz
XSL File : <empty>

For more information on KBAlertz: 
http://tripleasp.net/kbalertz.aspx?NavID=32
http://KBAlertz.com/Webmaster
