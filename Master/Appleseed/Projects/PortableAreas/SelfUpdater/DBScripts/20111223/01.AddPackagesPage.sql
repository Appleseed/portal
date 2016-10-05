BEGIN TRANSACTION
GO
ALTER TABLE rb_Pages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO

set IDENTITY_INSERT [rb_Pages] ON
INSERT INTO [rb_Pages]
           ([PageID]
		   ,[ParentPageID]
           ,[PageOrder]
           ,[PortalID]
           ,[PageName]
           ,[MobilePageName]
           ,[AuthorizedRoles]
           ,[ShowMobile]
           ,[PageLayout]
           ,[PageDescription])
     VALUES
           (5
		   ,100
           ,9999
           ,0
           ,'Packages'
           ,' '
           ,'Admins;'
           ,0
           ,Null
           ,' ')
GO

set IDENTITY_INSERT [rb_Pages] OFF

COMMIT

EXEC  [rb_UpdateTabCustomSettings] @TabID = 5,@SettingName ='CustomTheme', @SettingValue ='Appleseed.Admin'	
EXEC  [rb_UpdateTabCustomSettings] @TabID = 5,@SettingName ='CustomLayout', @SettingValue ='Appleseed.Admin'
GO