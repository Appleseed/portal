FileManager - Module to manage files of portal 

Another Appleseed desktop module - more to download on http://www.Appleseedportal.net/

INSTALL 
1. Go to Admin all and to add module definition. 
2. Point to install.xml install file
3. Add the module to a page
4. Edit module settings: 
5. Use it!  ¿:-D
Note: The module is automatically installed when you install Appleseed.
The install procedure is only required if you deleted the module in Admin all

HISTORY
Ver. 1.0 - 14 Nov 2004 - Implemented by: Rob Siera based on free code at http://www.seekdotnet.com/freeware.aspx
8 april 2005 - Added to CVS


ISSUES AND KNOWN PROBLEMS:
--------------------------
- Not localized yet
- Module Property "Downloadable extentions" not fully implemented (should accept a comma-delimited list - now only accepts one extention which is quite useless)
- Still some hardcoded HTML. Should all be replaced with css from theme (including widths', heights', colors, etc ..)

See also http://support.Appleseedportal.net/confluence/display/DOX/Filemanager+Module

I will give all the support needed to a volunteer who wants to improve this module it.

Already done:
- it's already a Appleseed module.
- installer works
- doesn't use the database
- shows dirs/files that are children of a supplied directory

What needs to be done:
- make it use theme css styles
- make it multi-lingual
- implement Downloadable extentions (This defines a comma-delimited list of extentions that you can download On Click - if not provides then it is view only) (now only one extention is supported)
- review the On Click mechanism. I think it would be better to implement links there instead of postback (as is implemented now).  This postback makes the it irritating to use the Back button triggering the 'form: send again message'.

As you see, only little issues left.
I hope someone can take up this little task.

I'm available for chat on MSN  info at holoncom dot be





MODULE SETTINGS
---------------
Directory Path
Default: Current Portal Path 
  This defines the path to the directory you want to display.  The path must be 
  visible from the server and can be the Current Portal path or one of its subdirectories.


Downloadable extentions
Default: 'jpg'
  This defines a comma-delimited list of extentions that you can download On Click.

