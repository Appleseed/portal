One File Module Example - ExcelReader
Credits: Thierry (Tiptopweb)


The example displays contents of an Excel file placed in the portal Data directory.
The code uses a SQL SELECT command to display the data from the worksheet.


Another Appleseed desktop module - more to download on http://www.Appleseedportal.net


Note: Before you install the "OneFileModuleKit" must have been installed!

INSTALL
1. Copy file ExcelReader.ascx to folder Appleseed\DesktopModules\ 
   Note: Do not add ExcelReader.ascx to the project - no compiling needed!
2. Run ApplyDBPatch.bat (or execute DBPatch.sql in SQL Query Analyzer)
   If you are on a production site use module "Admin - Database Tool"
   and exeute all sql in DBPatch.sql starting after the GO command.
3. Log on as Admin and add the "ExcelReader (OneFileModule)" module to a tabpage 
4. Edit Settings String: ExcelFile=Filename.xls;RangeName=a1:e5;
5. Use it! ;o) 


HISTORY
Ver. 1.0 - 3. march 2004 - First realase by Jakob Hansen
Ver. 1.1 - 29. jan 2005 - Updated DBPatch.sql for all examples


Issues and Known problems:
- Tested with Appleseed version 1.6.0.1876g - 13/09/2005

EXCEL DEMO DATA FILE
Enter in Settings String: ExcelFile=ExcelDemoData.xls;RangeName=a1:e5;
