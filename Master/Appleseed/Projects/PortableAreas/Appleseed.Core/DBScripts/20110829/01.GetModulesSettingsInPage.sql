SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModulesSettingsInPage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModulesSettingsInPage]
GO

CREATE PROCEDURE [rb_GetModulesSettingsInPage]
(
    @TabId             int,
    @PaneName			 nvarchar(256)
    
)
AS
SELECT ModuleID,ModuleDefID,ModuleOrder,ModuleTitle
FROM
	rb_Modules    
WHERE
   PaneName = @PaneName And TabID = @TabId

GO