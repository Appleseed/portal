ComponentModule - Portal module
This component make possible to edit and execute web controls like "<asp:Calendar runat='server' />"

Credits: José Viladiu

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net


INSTALL
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings: See below
5. Use it! ;o) 
Note: The module is automatically installed when you install Appleseed.
The install procedure is only required if you deleted the module in Admin all


HISTORY
Ver. 1.0 - 12.01.2003 - Creates by José Viladiu. Moved into Appleseed by Jakob Hansen
Ver. 1.1 - 09.05.2003 - Updated to follow "Appleseed best practices" by Jakob Hansen


Issues and Known problems:
- Tested with Appleseed version 1.1.8.1706
- Version 1.1 only in english (the code is not localized)


Module settings
---------------
None!


EXAMPLES
--------------------------
1) A simple calender:
<asp:Calendar id=Calendar1 runat="server"></asp:Calendar>
2) More calender:
<b>Two calenders</b><asp:Calendar id=Calendar1 runat="server"></asp:Calendar>&nbsp;<asp:Calendar id=Calendar2 runat="server"></asp:Calendar>
3) TBD - more examples!
