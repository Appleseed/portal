/* CREATE TABLE FOR DYNAMIC PAGE REDIRECTION */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rb_Pages_DynamicRedirects]') AND type in (N'U'))
BEGIN
Create table dbo.rb_Pages_DynamicRedirects
(
DynamicPageID int identity(1,1) primary key,
FriendlyUrl nvarchar(512),
RedirectToUrl nvarchar(512)
)
END

/* CREATE PAGE FOR Page Friendly URL if not exist */
DECLARE @PAGEID INT,
		@ModuleDefId INT,
		@ModuleID INT
if not exists(select * from rb_pages where PageName='Short Links')
BEGIN
	EXEC rb_AddTab 0,100,'Short Links',1205,'Admins;',0,'', @PAGEID output
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @PAGEID,@SettingName ='CustomTheme', @SettingValue ='Appleseed.Admin'	
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @PAGEID,@SettingName ='CustomLayout', @SettingValue ='Appleseed.Admin'
END

--if exists(select * from rb_pages where PageName='Packages')
--BEGIN
--	--set @PAGEID = null
--	--select @PAGEID = PageID from rb_pages where PageName='Packages'
EXEC  [rb_UpdateTabCustomSettings] @TabID = 5,@SettingName ='CustomTheme', @SettingValue ='Appleseed.Admin'	
EXEC  [rb_UpdateTabCustomSettings] @TabID = 5,@SettingName ='CustomLayout', @SettingValue ='Appleseed.Admin'
--END

select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='C1EA4115-E7F2-4CBC-B1E7-DDA46791493C'

/* Add PageFriendlyUrl Module on new created page if not loaded */
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId)
BEGIN
EXEC rb_addModule @PAGEID,1,'Short Links','ContentPane',@ModuleDefId,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
END

/*renaming page friendly url module to Admin - Short Links*/
UPDATE [rb_GeneralModuleDefinitions] SET FriendlyName = 'Admin - Short Links' where GeneralModDefID = 'C1EA4115-E7F2-4CBC-B1E7-DDA46791493C'

/* replacing new file browser with old file manager */
DECLARE @newMFIDFB int
SELECT @newMFIDFB = ModuleDefID FROM [rb_ModuleDefinitions] WHERE GeneralModDefID = 'D7B8B22F-366B-4D80-9E49-13C09120A89F'
UPDATE [rb_Modules] SET [ModuleDefID] = @newMFIDFB WHERE [ModuleDefID] = 7 AND TabID = 155

/* 27/6/2016 */
/* delete Pages page*/
IF EXISTS(SELECT * FROM rb_pages WHERE [PageName]='Pages' and PageID=200)
BEGIN
	DELETE FROM rb_pages WHERE PageID=200 or parentpageid = 200
END

IF EXISTS(SELECT * FROM rb_pages WHERE [PageName]='Database Editor' and PageID=151)
BEGIN
	DELETE FROM rb_pages WHERE [PageName]='Database Editor' and PageID=151
END

IF EXISTS(SELECT * FROM rb_pages WHERE [PageName]='Database Tool' and PageID=152)
BEGIN
	DELETE FROM rb_pages WHERE [PageName]='Database Tool' and PageID=152
END

EXEC rb_addModule 150,1,'Database Table Edit','ContentPane',4,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output

/*30/06/2016*/
UPDATE rb_Pages SET PageName = 'Global Modules' WHERE PageID = 120
GO

DELETE FROM rb_Modules WHERE tabid in (SELECT pageId FROM rb_pages WHERE ParentPageID = 170)
GO

DELETE FROM rb_Pages WHERE ParentPageID = 170
GO

UPDATE rb_Pages SET PageName = 'Monitoring' WHERE PageID = 170
GO

DECLARE @ModuleID INT

EXEC rb_addModule 170,1,'Monitoring','ContentPane',11,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output

EXEC rb_addModule 170,2,'Error Logs','ContentPane',29,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output

EXEC rb_addModule 170,3,'Event Logs','ContentPane',6,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
GO

UPDATE rb_Modules SET [TabID] = 280 where TabID = 281
GO

DELETE FROM rb_Modules WHERE tabid in (SELECT pageId FROM rb_pages WHERE ParentPageID = 280)
GO

DELETE FROM rb_Pages WHERE ParentPageID = 280
GO

