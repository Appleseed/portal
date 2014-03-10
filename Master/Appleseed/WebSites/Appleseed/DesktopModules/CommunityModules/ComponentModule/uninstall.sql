/* Uninstall script, ComponentModule, Jakob Hansen, 9 may 2003 */


/* No we do not DELETE the table!
IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ComponentModule]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_ComponentModule] */
--GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'rb_GetComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_GetComponentModule
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'rb_UpdateComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_UpdateComponentModule
GO
