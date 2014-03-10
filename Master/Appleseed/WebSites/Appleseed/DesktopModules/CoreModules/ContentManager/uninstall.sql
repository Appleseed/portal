
if  exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentManager]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    DROP TABLE [rb_ContentManager]
END



/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleTypes]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetPortals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetPortals]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleInstances]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleInstances]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleInstancesExc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleInstancesExc]
GO

/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetSourceModuleData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetSourceModuleData]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetDestModuleData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetDestModuleData]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_MoveItemLeft]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_MoveItemLeft]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_MoveItemRight]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_MoveItemRight]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_CopyItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_CopyItem]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_CopyAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_CopyAll]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_DeleteItemLeft]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_DeleteItemLeft]
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_DeleteItemRight]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_DeleteItemRight]
GO
