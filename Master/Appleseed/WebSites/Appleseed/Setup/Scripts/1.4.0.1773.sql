/* Checks to see if rb_GetModulesInTab already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetModulesInTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModulesInTab]
GO

CREATE PROCEDURE rb_GetModulesInTab
(
 @PortalID int,
 @TabID int
)
AS
SELECT rb_Modules.ModuleID, rb_ModuleDefinitions.GeneralModDefID
FROM rb_Modules INNER JOIN
     rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
     rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE (rb_Modules.TabID = @TabID) AND (rb_ModuleDefinitions.PortalID = @PortalID)
GO

/* Set the correct roles properties to existing modules */
UPDATE rb_Modules
   SET AuthorizedMoveModuleRoles = 'Admins;'
   WHERE AuthorizedMoveModuleRoles is null
go

UPDATE rb_Modules
   SET AuthorizedDeleteModuleRoles = 'Admins;'
   WHERE AuthorizedDeleteModuleRoles is null
go

