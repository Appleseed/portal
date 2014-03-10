SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rb_GetUsersNewsletter]') AND type in (N'P', N'PC'))
DROP PROCEDURE rb_GetUsersNewsletter
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[rb_SendNewsletterTo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [rb_SendNewsletterTo]
GO
