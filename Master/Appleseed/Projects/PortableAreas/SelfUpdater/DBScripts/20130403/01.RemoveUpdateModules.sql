DELETE FROM rb_Modules 
where ModuleDefID in (SELECT [ModuleDefID]
FROM rb_ModuleDefinitions md inner join rb_GeneralModuleDefinitions gmd
 on md.GeneralModDefID = gmd.GeneralModDefID
 where gmd.FriendlyName = 'SelfUpdater - Updates')
GO