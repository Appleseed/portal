SET NOCOUNT on
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_LOGO' WHERE SettingName = 'Image'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_LAYOUT' WHERE SettingName = 'TabLayout'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_THEME' WHERE SettingName = 'Theme'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_ALT_THEME' WHERE SettingName = 'ThemeAlt'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_ALLOW_NEW_REGISTRATION' WHERE SettingName = 'AllowNewRegistrations'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_REGISTER_TYPE', SettingValue = 'RegisterFull.ascx' WHERE SettingName = 'Register'AND SettingValue = 'RegisterFull'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_TERMS_OF_SERVICE' WHERE SettingName = 'TermsOfService'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_COUNTRY_FILTER' WHERE SettingName = 'CountriesFilter'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_TITLE' WHERE SettingName = 'TabTitle'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_URL_KEYWORD' WHERE SettingName = 'TabUrlKeyword'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_META_KEYWORDS' WHERE SettingName = 'TabMetaKeyWords'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_META_DESCRIPTION' WHERE SettingName = 'TabMetaDescription'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_META_ENCODING' WHERE SettingName = 'TabMetaEncoding'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_META_OTHERS' WHERE SettingName = 'TabMetaOther'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_PAGE_KEY_PHRASE' WHERE SettingName = 'TabKeyPhrase'
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_SHOW_MODIFIED_BY' WHERE SettingName = 'ShowModifiedBy'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_APPLY_THEME' WHERE SettingName = 'ApplyTheme'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_THEME', SettingValue = '0' WHERE SettingName = 'Theme' AND SettingValue = 'Default'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_THEME', SettingValue = '1' WHERE SettingName = 'Theme' AND SettingValue = 'Alt'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_SHOW_TITLE' WHERE SettingName = 'ShowTitle'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_SHOW_MODIFIED_BY' WHERE SettingName = 'ShowModifiedBy'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_CULTURE' WHERE SettingName = 'Culture'
GO

UPDATE rb_ModuleSettings SET SettingName = 'MODULESETTINGS_PRINT_BUTTON' WHERE SettingName = 'ShowPrintButton'
GO

UPDATE rb_ModuleSettings SET SettingName = 'SHOWMOBILE' WHERE SettingName = 'ShowMobile'
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1734','1.2.8.1734', CONVERT(datetime, '08/13/2003', 101))
GO
