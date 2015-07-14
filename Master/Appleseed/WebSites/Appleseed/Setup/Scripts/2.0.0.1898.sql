
DECLARE @TAB_ID int;
/*
DECLARE db_cursor CURSOR FOR
SELECT PAGEID from [dbo].[rb_Pages] where PortalID = 0 and AuthorizedRoles like '%Admins%'

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @TAB_ID

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC [dbo].[rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomTheme',
	@SettingValue ='Appleseed.Admin'

	EXEC [dbo].[rb_UpdateTabCustomSettings] @TabID = @TAB_ID,@SettingName ='CustomLayout',
	@SettingValue ='Appleseed.Admin'

	FETCH NEXT FROM db_cursor INTO @TAB_ID 
END
CLOSE db_cursor
*/