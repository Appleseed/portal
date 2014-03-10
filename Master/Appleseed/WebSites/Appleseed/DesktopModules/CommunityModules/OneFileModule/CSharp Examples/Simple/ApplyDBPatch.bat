@echo ---------------------------------------------------------
@echo      Welcome to Appleseed database patch Installation.
@echo Support: please visit http://Appleseed.duemetri.net/Appleseed
@echo ---------------------------------------------------------
@echo If you use MSDE: read instructions inside this file
@echo ---------------------------------------------------------
@echo Press Ctrl-C to abort or Enter to continue
@pause

set DBNAME=(local)
rem Uncomment the following line for MSDE
rem set DBNAME=(local)\NETSDK

osql -S %DBNAME% -E -n -i DBPatch.sql

@echo Setup Complete!
@pause
