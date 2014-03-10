-- for remove old rb_FindModuleByGuid procedure
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModuleByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModuleByGuid]
GO


-- change it with new rb_FindModulesByGuid
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModulesByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModulesByGuid]
GO

CREATE PROCEDURE rb_FindModulesByGuid
(
	@PortalID int,
	@Guid uniqueidentifier
)
AS

    SELECT     rb_Modules.ModuleID
    FROM         rb_Modules 
    INNER JOIN rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID
    WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_ModuleDefinitions.GeneralModDefID = @Guid)
GO

/*test code*/
-- declare @P int
-- declare @G uniqueidentifier
-- select  @P=0
-- select @G="{1C575D94-70FC-4A83-80C3-2087F726CBB3}"
-- exec rb_FindModulesByGuid @P, @G
-- go
