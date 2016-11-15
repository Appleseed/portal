IF NOT EXISTS(SELECT * FROM [rb_PortalSettings] WHERE  [SettingName] = 'SITESETTINGS_LOGIN_TYPE')
BEGIN
	INSERT INTO [rb_PortalSettings] VALUES(0,'SITESETTINGS_LOGIN_TYPE','~/desktopmodules/coremodules/signin/signincool.ascx')
END
ELSE
BEGIN
	update [rb_PortalSettings] set [SettingValue] = '~/desktopmodules/coremodules/signin/signincool.ascx' where [SettingName] = 'SITESETTINGS_LOGIN_TYPE'
END
go