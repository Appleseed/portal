IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rb_Proxy]') AND type in (N'U'))
BEGIN
	Create Table dbo.rb_Proxy
	(
		ServiceId int identity(1,1) PRIMARY KEY,
		ServiceTitle varchar(200),
		ServiceUrl varchar(max),
		ForwardHeaders bit default(0) NOT NULL,
		EnabledContentAccess bit default(0) NOT NULL,
		ContentAccessRoles varchar(max)
	)
END
GO

DECLARE @PAGEID INT,
		@ModuleDefId INT,
		@ModuleID INT

IF NOT EXISTS(SELECT * FROM rb_GeneralModuleDefinitions where GeneralModDefID ='E7C442C3-1825-42FE-BE68-AB75F3C35A02')
BEGIN
	INSERT INTO [rb_GeneralModuleDefinitions]
				([GeneralModDefID],[FriendlyName],[DesktopSrc],[MobileSrc],[AssemblyName],[ClassName],[Admin],[Searchable])
		 VALUES
			   ('E7C442C3-1825-42FE-BE68-AB75F3C35A02' ,'Admin - Proxy Settings', 'Area/ASProxy/Proxy/ProxyList' ,'' ,'Appleseed.DLL', 'Appleseed.DesktopModules.CoreModules.Admin.AdminLeftMenu',1,0)
END

IF NOT EXISTS(SELECT * FROM rb_ModuleDefinitions where GeneralModDefID ='E7C442C3-1825-42FE-BE68-AB75F3C35A02')
BEGIN
	INSERT INTO [rb_ModuleDefinitions] ([PortalID],[GeneralModDefID])VALUES (0,'E7C442C3-1825-42FE-BE68-AB75F3C35A02')
END

if not exists(select * from rb_pages where PageName='Proxy Settings')
BEGIN
	EXEC rb_AddTab 0,100,'Proxy Settings',1206,'Admins;',0,'', @PAGEID output
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @PAGEID,@SettingName ='CustomTheme', @SettingValue ='Appleseed.Admin'	
	EXEC  [rb_UpdateTabCustomSettings] @TabID = @PAGEID,@SettingName ='CustomLayout', @SettingValue ='Appleseed.Admin'
END

select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='E7C442C3-1825-42FE-BE68-AB75F3C35A02'

/* Add Proxy Settings Module on new created page if not loaded */
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId)
BEGIN
EXEC rb_addModule @PAGEID,1,'Proxy Settings','ContentPane',@ModuleDefId,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
END

