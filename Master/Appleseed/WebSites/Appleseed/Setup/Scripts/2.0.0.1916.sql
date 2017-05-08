IF NOT EXISTS(SELECT * FROM rb_GeneralModuleDefinitions where GeneralModDefID ='8B647C1E-D9CE-4796-B789-EBD8C9A6480E')
BEGIN
	INSERT INTO [rb_GeneralModuleDefinitions]
				([GeneralModDefID],[FriendlyName],[DesktopSrc],[MobileSrc],[AssemblyName],[ClassName],[Admin],[Searchable])
		 VALUES
			   ('8B647C1E-D9CE-4796-B789-EBD8C9A6480E' ,'Admin - Member Registration Requests', 'Area/ASMemberRegistrationRequests/MemberRegistrationRequests/Index' ,'' ,'Appleseed.DLL', 'Appleseed.DesktopModules.CoreModules.AdminConnectedSources',1,0)
END

IF NOT EXISTS(SELECT * FROM rb_ModuleDefinitions where GeneralModDefID ='8B647C1E-D9CE-4796-B789-EBD8C9A6480E')
BEGIN
	INSERT INTO [rb_ModuleDefinitions] ([PortalID],[GeneralModDefID])VALUES (0,'8B647C1E-D9CE-4796-B789-EBD8C9A6480E')
END