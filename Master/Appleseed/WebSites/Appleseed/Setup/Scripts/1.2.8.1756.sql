
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetModuleDefinitionByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleDefinitionByGuid]
GO

CREATE PROCEDURE rb_GetModuleDefinitionByGuid
(
	@PortalID int,
	@Guid uniqueidentifier,
	@ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     rb_ModuleDefinitions.ModuleDefID
    FROM         rb_GeneralModuleDefinitions LEFT OUTER JOIN
                          rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
    WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_ModuleDefinitions.GeneralModDefID = @Guid)
)
GO


