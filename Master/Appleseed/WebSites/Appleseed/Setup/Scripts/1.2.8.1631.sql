-----------------------
------1.2.8.1631.sql
-------------------------

---- Fix path for module Tasks (was moved into own folder)
--UPDATE rb_GeneralModuleDefinitions SET DesktopSrc = 'DesktopModules/Tasks/Tasks.ascx' 
--WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531012}'
--GO


---- Update rb_Tasks status AND priority fields for localization
--update rb_Tasks set Status='0' WHERE Status='Not Started'
--update rb_Tasks set Status='1' WHERE Status='In Progress'
--update rb_Tasks set Status='2' WHERE Status='Complete'
--update rb_Tasks set Priority='0' WHERE Priority='High'
--update rb_Tasks set Priority='1' WHERE Priority='Normal'
--update rb_Tasks set Priority='2' WHERE Priority='Low'
--go


--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1631','1.2.8.1631', CONVERT(datetime, '04/29/2003', 101))
--GO
