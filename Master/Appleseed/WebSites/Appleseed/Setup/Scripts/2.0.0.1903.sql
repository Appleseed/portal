DECLARE @ModuleDefID int 
DECLARE @TAB_ID int;
DECLARE @ModuleID int;

IF NOT EXISTS(SELECT * FROM rb_GeneralModuleDefinitions where GeneralModDefID ='504F5B7F-400D-46D4-887A-B03479441E04')
BEGIN
	INSERT INTO [rb_GeneralModuleDefinitions]
				([GeneralModDefID],[FriendlyName],[DesktopSrc],[MobileSrc],[AssemblyName],[ClassName],[Admin],[Searchable])
		 VALUES
			   ('504F5B7F-400D-46D4-887A-B03479441E04' ,'Admin - Left Menu', 'DesktopModules/CoreModules/Admin/AdminLeftMenu.ascx' ,'' ,'Appleseed.DLL', 'Appleseed.DesktopModules.CoreModules.Admin.AdminLeftMenu',1,0)
END

IF NOT EXISTS(SELECT * FROM rb_ModuleDefinitions where GeneralModDefID ='504F5B7F-400D-46D4-887A-B03479441E04')
BEGIN
	INSERT INTO [rb_ModuleDefinitions] ([PortalID],[GeneralModDefID])VALUES (0,'504F5B7F-400D-46D4-887A-B03479441E04')
END

select @ModuleDefID = ModuleDefID from [rb_ModuleDefinitions] where GeneralModDefID ='504F5B7F-400D-46D4-887A-B03479441E04'

DECLARE db_cursor CURSOR FOR

SELECT PAGEID from  [rb_Pages] where ParentPageID=100 or PageName='Administration'

OPEN db_cursor

FETCH NEXT FROM db_cursor INTO @TAB_ID

WHILE @@FETCH_STATUS = 0
BEGIN
	
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomTheme', @SettingValue ='Appleseed.Admin'
	
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomLayout', @SettingValue ='Appleseed.Admin'
	IF NOT EXISTS(SELECT * FROM RB_MODULES WHERE TABID=@TAB_ID AND ModuleDefID=@ModuleDefID)
	BEGIN
		EXEC  [rb_AddModule] @TAB_ID, 1, 'Admin Menu','LeftPane',@ModuleDefID, 0, 'Admins;', 'Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,'Admins;',0,0,0,  @ModuleID output
	END
	
	FETCH NEXT FROM db_cursor INTO @TAB_ID 
END
CLOSE db_cursor
DEALLOCATE db_cursor;


/* -- revert changes */
/*
DECLARE @ModuleDefID int 
DECLARE @TAB_ID int;
DECLARE db_cursor_revert1 CURSOR FOR
SELECT PAGEID from [dbo].[rb_Pages] where ParentPageID=100 or PageName='Administration'
select @ModuleDefID = ModuleDefID from [rb_ModuleDefinitions] where GeneralModDefID ='504F5B7F-400D-46D4-887A-B03479441E04'
OPEN db_cursor_revert1
FETCH NEXT FROM db_cursor_revert1 INTO @TAB_ID

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC [dbo].[rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomTheme', @SettingValue =''

	EXEC [dbo].[rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomLayout', @SettingValue =''
	DELETE FROM RB_MODULES WHERE TABID=@TAB_ID AND ModuleDefID=@ModuleDefID
	FETCH NEXT FROM db_cursor_revert1 INTO @TAB_ID 
END
CLOSE db_cursor_revert1
DEALLOCATE db_cursor_revert1;
*/