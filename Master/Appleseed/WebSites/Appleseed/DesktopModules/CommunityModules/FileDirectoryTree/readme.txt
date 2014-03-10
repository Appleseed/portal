File Directory Tree - Module traverses and displays files and directories as an 
HTML-representation of a nested file tree.

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net

INSTALL 
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings: 
5. Use it!  ¿:-D
Note: The module is automatically installed when you install Appleseed.
The install procedure is only required if you deleted the module in Admin all

HISTORY
Ver. 1.0 - 07/23/2003 - Writen by: Josue de la Torre - josue@jdlt.com


ISSUES AND KNOWN PROBLEMS:
- Tested with Appleseed version 1.2.8.1728a - 07/24/2003 - Jakob Hansen



MODULE SETTINGS
---------------
Directory Path
Default: Appleseed Application Path 
  This defines the path to the directory you want to display.  The path must be 
  visible from the server.  Example: C:\FolderName or \\NetworkPC\SharedFolder.

Link Type
Options: Downloadable Link, Network Share
Default: Downloadable Link
  Downloadable Link: Clicking a link will prompt user to download the file.
  Network Share: Clicking Link will launch the file from the network share. 

Target Window 
Default: blank
Options: blank, top, parent, self
**This feature works only when the Link Type is Network Share!!!
  This defines the target on which the file contents will be displayed.  

Collapsed View
Default: Checked
  This defines whether the tree view will be collapsed or expanded. Collapsed 
  view displays only the subdirectories and files under the parent path. To 
  expand the contents of a subdirectory, just click on the subdirectory name.
  Click it again and the subdirectory returns to the collapsed view. Expanded 
  view expands all the nested subdirectories to display all files in the tree.
 
Style
Default: Empty
  Defines the tree style in CSS format.  
  Example: COLOR: Firebrick; FONT: x-small;

Sub Directory Indent
Default: 20px
  This defines the number of pixels to indent the contents of each nested 
  subdirectory.


CODE CONFIGURATION
------------------
Unauthorized Access Exception:
This exception is thrown if: 
  - You are using a network share or local path that has access-level 
    restrictions and 
  - Your network does not have Active Directory.

Temporary Work Around:
  1. Open the Appleseed Project in VS.NET. 
  2. Uncomment the blocks of code at lines 167-171 and 281-286 to catch the 
     exception.
  3. Copy DesktopDefault.aspx and rename it to WADesktopDefault.aspx.  
  4. Change all references of DesktopDefault to WADesktopDefault in the web 
     form and code files.  
  5. Rebuild the Project.
  6. Open IIS Manager and edit the file security settings for 
     WADesktopDefault.aspx.  Disable Anonymous Access and Integrated Windows 
     Authentication, and enable Basic Authentication enabled.  

Now when the user tries to display content where user authentication is 
required, they will be redirected to WADesktopDefault.aspx and be prompted 
for a username & password. The server will then use the supplied credentials 
to gain access to the resource.  
