/* Install script, Newsletter by Manu 06/10/2003 */

---NEWSLETTER
-IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetUsersNewsletter]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
-DROP PROCEDURE [rb_GetUsersNewsletter]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_SendNewsletterTo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_SendNewsletterTo]
GO
