README
----------------------------------------------------------------------------------------

When adding new database sql scripts you should be aware of:
 - Adding a new folder inside DBScripts names as current date as {yyyymmdd} (e.g.: /DBSCripts/20110430/).
 - Adding .sql files containing scripts to execute in as {nn. description.sql} (e.g.: /DBScripts/20110430/01. Example.sql).
   First two digits of filename allows you to control execution order.

This procedure is valid for any new portable area you add to the website, created with the template provided within this distribution)
Scripts are embedded into portablearea dll and are dynamically executed on application_start lifecycle event.
All logging information goes to: ~/rb_logs/DBScriptsLog.txt