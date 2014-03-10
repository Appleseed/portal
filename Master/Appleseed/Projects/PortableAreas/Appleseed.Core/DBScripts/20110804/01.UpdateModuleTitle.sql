SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModuleTitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleTitle]
GO

CREATE PROCEDURE [rb_UpdateModuleTitle]
(
    @ModuleID               int,
    @ModuleTitle            nvarchar(256)
    
)
AS
UPDATE
    rb_Modules
SET
    ModuleTitle               	= @ModuleTitle
    
WHERE
    ModuleID = @ModuleID

GO

