EventLogs - Windows Event viewer

An event viewer that (basically) does the same job as Windows 2000 Event viewer.
The user can control: Machine name, Log type (Application, IExplorer, Security, System) 
and source which depends on the Log type. Sort ascending/descending are provided by 
clicking the table headers.
Note1: You are not allowed to access the Security event log with standard permissions
Note2: This module will slow down the page - much data in the eventlog - have patience


Credits: Hervé LE ROY (www.hleroy.com)


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
Ver. 1.0 - 14. may 2003 - Moved from original IBS and VB into Appleseed and C# by Jakob Hansen


Issues and Known problems:
- Tested with Appleseed version 1.2.8.1710 - 14/05/2003
- Version 1.0 only in english (the code is not localized)
- You are not allowed to access the Security event log with standard permissions


Module settings
---------------
MachineName: Name of Windows box. Default value is "."
SortField: Default sort field when the module is displayed
SortDirection: Default sort direction when the module is displayed
