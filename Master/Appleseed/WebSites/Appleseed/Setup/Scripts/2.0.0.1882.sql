set xact_abort on
go

begin transaction
go

set ANSI_NULLS on
go

alter PROCEDURE rb_GetModulesSinglePortal
(
    @PortalID  int
)
AS
SELECT      0 AS ModuleID, 'NO_MODULE' ModuleTitle, -1 AS PageOrder
UNION
	SELECT     rb_Modules.ModuleID, rb_Pages.PageName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')' AS ModuleTitle, rb_Pages.PageOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Pages ON rb_Modules.TabID = rb_Pages.PageID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Pages.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
	ORDER BY PageOrder, ModuleTitle
go

commit
go


