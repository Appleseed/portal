/* Uninstall script */

if exists (select * from sysobjects where id = object_id(N'[rb_EnhancedHtml_stModified]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_EnhancedHtml_stModified]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_AddEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_AddEnhancedHtml]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_DeleteEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_DeleteEnhancedHtml]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEnhancedHtml]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetEnhancedLocalizedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEnhancedLocalizedHtml]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetSingleEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetSingleEnhancedHtml]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_UpdateEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateEnhancedHtml]
GO
