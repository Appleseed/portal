---------------------
--1.2.8.1625.sql
---------------------


---- Fix path for module Daily Dilbert (was moved into own folder)
--UPDATE rb_GeneralModuleDefinitions SET DesktopSrc = 'DesktopModules/DailyDilbert/DailyDilbert.ascx' 
--WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531031}'
--GO

-- Fix path for module DatabaseTool (was moved into own folder)
UPDATE rb_GeneralModuleDefinitions SET DesktopSrc = 'DesktopModules/DatabaseTool/DatabaseTool.ascx' 
WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531032}'
GO

-- Fix path for module DatabaseToolTableEdit (was moved into own folder)
UPDATE rb_GeneralModuleDefinitions SET DesktopSrc = 'DesktopModules/DatabaseTableEdit/DatabaseTableEdit.ascx' 
WHERE GeneralModDefID = '{AB02A3F4-A0A4-45E0-96ED-8450C19166C5}'
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1625','1.2.8.1625', CONVERT(datetime, '04/23/2003', 101))
GO
