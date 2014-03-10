if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rb_MoveModuleToNewTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_MoveModuleToNewTab]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rb_DeleteModuleToRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_DeleteModuleToRecycler]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rb_GetModuleSettingsForIndividualModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModuleSettingsForIndividualModule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rb_GetModulesInRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModulesInRecycler]
GO