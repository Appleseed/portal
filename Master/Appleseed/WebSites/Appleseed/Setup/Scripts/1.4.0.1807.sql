/* Delete Used ThemeCacheManager module in all portals */
DELETE rb_Modules
FROM         rb_Modules INNER JOIN
                      rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
                      rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.ModuleID > 0) AND (rb_GeneralModuleDefinitions.GeneralModDefID = '48358161-D002-4d70-896A-49CF62D5110B')
GO

/* Delete ThemeCacheModule */
DELETE FROM rb_GeneralModuleDefinitions
WHERE     (GeneralModDefID = '48358161-D002-4d70-896A-49CF62D5110B')
GO
