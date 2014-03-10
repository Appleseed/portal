-- for remove old rb_FindModuleByGuid procedure
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModuleByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModuleByGuid]
GO


-- change DesktopSrc in RoleAssignment module
UPDATE    rb_GeneralModuleDefinitions
SET       DesktopSrc = 'DesktopModules/RoleAssignment/RoleAssignment.ascx',
		  ClassName = 'Appleseed.Content.Web.ModulesRoleAssignment'
WHERE     (GeneralModDefID = '{5EEE69A2-35BA-4B54-8451-E13B0CD24E99}')
GO
