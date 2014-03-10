IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ComponentModule]') AND OBJECTPROPERTY(id, N'IsTable') = 1)
	ALTER TABLE rb_ComponentModule ALTER COLUMN Component nvarchar(MAX)
GO