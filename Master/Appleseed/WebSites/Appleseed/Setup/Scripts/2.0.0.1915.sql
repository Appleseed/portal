IF NOT EXISTS(SELECT * FROM rb_GeneralModuleDefinitions where GeneralModDefID ='28E871BE-1A0C-4F05-99C4-14681D5FD02A')
BEGIN
	INSERT INTO [rb_GeneralModuleDefinitions]
				([GeneralModDefID],[FriendlyName],[DesktopSrc],[MobileSrc],[AssemblyName],[ClassName],[Admin],[Searchable])
		 VALUES
			   ('28E871BE-1A0C-4F05-99C4-14681D5FD02A' ,'Admin - Connected Sources', 'DesktopModules/CoreModules/AdminConnectedSources/SearchAdminSettingsModule.ascx' ,'' ,'Appleseed.DLL', 'Appleseed.DesktopModules.CoreModules.AdminConnectedSources',1,0)
END

IF NOT EXISTS(SELECT * FROM rb_ModuleDefinitions where GeneralModDefID ='28E871BE-1A0C-4F05-99C4-14681D5FD02A')
BEGIN
	INSERT INTO [rb_ModuleDefinitions] ([PortalID],[GeneralModDefID])VALUES (0,'28E871BE-1A0C-4F05-99C4-14681D5FD02A')
END