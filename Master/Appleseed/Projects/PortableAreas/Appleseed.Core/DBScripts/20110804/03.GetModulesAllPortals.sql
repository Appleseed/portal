SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModulesAllPortals]
GO

create PROCEDURE rb_GetModulesAllPortals
AS
SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' AS PortalAlias, -1 AS TabOrder
UNION
	SELECT     rb_Modules.ModuleID, rb_Portals.PortalAlias + '/' + rb_Pages.PageName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, rb_Pages.PageOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Pages ON rb_Modules.TabID = rb_Pages.PageID INNER JOIN
	                      rb_Portals ON rb_Pages.PortalID = rb_Portals.PortalID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Modules.ModuleID > 0) AND (rb_GeneralModuleDefinitions.Admin = 0) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
ORDER BY PortalAlias, ModuleTitle
go