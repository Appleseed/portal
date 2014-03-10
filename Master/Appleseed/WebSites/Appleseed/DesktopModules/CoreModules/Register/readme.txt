Register Modules 


Another Appleseed desktop module - more to download on http://www.Appleseedportal.net

INSTALL
THESE MODULES CAN BE INSTALLED (NO NEED IF YOU DO NOT USE CUSTOM SETTINGS) 
THESE MODULES ARE LOADED BY THE /DesktopModules/Register/REGISTER.ASPX


HISTORY
Ver. 2.0 - 18 mar 2004 - added custom settings by manu
Ver. 1.2 - 08 okt 2003 - moved to seperate folder and modified to be loaded dynamically by mario@hartmann.net
Ver. 1.1 - 10 mar 2003 - Updated to use Globalized controls and modified from original to be fully placeable module by Jes111
Ver. 1.0 - ?? ??? ???? - Original IBUYSPY


Current way to add register module (still ok)
- You must place you custom page in DesktopModules\Register folder
- Your custom page must be control named Register<....>.ascx (start with Register word)
- Your control must implement IEditUserProfile interface
- Your control may or may not be compiled with Appleseed (it can be in its own dll)
- On Portalsettings select your control from the menu.

Using custom settings:
- You must register your control as a standard module
- Add it to a page (hidden usually)
- Change your settings
- Record the module number (mid in querystring in property page)
- On Portalsettings fill the Register module ID box with the number.
