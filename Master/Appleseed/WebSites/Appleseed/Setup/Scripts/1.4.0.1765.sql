UPDATE rb_ModuleSettings SET SettingValue = SettingValue + '.ascx' WHERE (SettingName = 'sm_MenuType')
--UPDATE rb_ModuleSettings SET SettingValue = 'StaticItemMenu.ascx' WHERE (SettingValue = 'SimpleItemMenu.ascx')
--UPDATE rb_ModuleSettings SET SettingValue = 'StaticMenu.ascx' WHERE (SettingValue = 'SimpleMenu.ascx')
GO
--UPDATE rb_GeneralModuleDefinitions SET AssemblyName = 'Appleseed.Modules.SimpleMenu.DLL' WHERE (GeneralModDefID = '{D3182CD6-DAFF-4E72-AD9E-0B28CB44F006}')
--UPDATE rb_GeneralModuleDefinitions SET ClassName = 'Appleseed.Modules.SimpleMenu.SimpleMenu' WHERE (GeneralModDefID = '{D3182CD6-DAFF-4E72-AD9E-0B28CB44F006}')
--GO
DELETE FROM rb_ModuleSettings WHERE (SettingName = 'sm_Menu_HeaderText')
DELETE FROM rb_ModuleSettings WHERE (SettingName = 'sm_Menu_FooterText')
GO