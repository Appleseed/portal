if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetEvents_Summary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEvents_Summary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_CopyItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_CopyItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_MoveItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_MoveItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_CopyAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_CopyAll]
GO
/*
THIS STEP REQUIRED OR CONTENT MANAGER UI WILL SHOW THE MODULE EVEN THOUGH IT HAS BEEN
UNINSTALLED
*/
DELETE FROM rb_ContentManager WHERE FriendlyName = 'Events'
GO