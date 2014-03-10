IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModuleByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModuleByGuid]
GO

CREATE PROCEDURE rb_FindModuleByGuid
(
	@PortalID int,
	@Guid uniqueidentifier,
	@ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     rb_Modules.ModuleID
    FROM         rb_Modules 
    INNER JOIN rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID
    WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_ModuleDefinitions.GeneralModDefID = @Guid)
)
GO

/* Ensure to restrict access to admin modules only to Admins users */
update rb_modules
set 
    AuthorizedEditRoles       	= 'Admins;',
    AuthorizedAddRoles        	= 'Admins;',
    AuthorizedViewRoles       	= 'Admins;',
    AuthorizedDeleteRoles     	= 'Admins;',
    AuthorizedPropertiesRoles 	= 'Admins;',
    AuthorizedMoveModuleRoles 	= 'Admins;',
    AuthorizedDeleteModuleRoles = 'Admins;',
    AuthorizedPublishingRoles 	= 'Admins;',
    AuthorizedApproveRoles    	= 'Admins;'
from rb_modules
Inner Join rb_ModuleDefinitions on rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID
Inner Join rb_GeneralModuleDefinitions on rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
Where Admin = 1
GO

