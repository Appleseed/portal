Blacklist Admin Module - Setup which users receive emails

This module is typically used togeteher with the Newsletter module.
Using the Blacklist module you can block some of the registred users 
from receiving emails. Invalid emails are automatically blacklisted 
by newsletter module to prevent further errors.


Credits: Manu and Jakob Hansen


Another Appleseed desktop module - more to download on http://www.Appleseedportal.net


INSTALL
1. Run Blacklist_SetupDBPatch.bat (or execute Blacklist_DBPatch.sql in SQL Query Analyzer)
2. Copy file DBHelper.cs to folder Appleseed\Helpers and add to project
3. Copy file BlacklistDB.cs to folder Appleseed\DesktopComponents and add to project
4. Copy files *.ascx.* to folder Appleseed\Admin and add to project
5. Compile
6. Log on as Admin and add the "Blacklist" module to an admin tabpage 
8. Edit module settings: enter default sort header fieldname
7. Use it! ;o) 


HISTORY
Ver. 1.0 - 14. jan 2003 - First realase by Jakob


Issues and Known problems:
- Tested with Appleseed version 1.1.7.1209
- In the top of file DBPatch.sql is list of DB changes this patch introduces
- Version 1.0 only in english (the code is only partly localized)

