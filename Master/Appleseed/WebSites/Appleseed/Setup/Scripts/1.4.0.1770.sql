if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetGuid]
GO

CREATE PROCEDURE rb_GetGuid
(
    @ModuleID int
) AS 
SELECT  rb_ModuleDefinitions.GeneralModDefID
FROM    rb_GeneralModuleDefinitions INNER JOIN
           rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
              rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE (rb_Modules.ModuleID = @ModuleID)
GO
