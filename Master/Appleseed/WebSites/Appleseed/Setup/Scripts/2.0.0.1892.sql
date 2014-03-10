CREATE PROCEDURE rb_GetModulesInRecycler
(
 @PortalID int,
 @SortField VarChar(50)
)
AS
SELECT     
	rb_Recycler.DateDeleted, 
	rb_Recycler.DeletedBy, 
	rb_Recycler.OriginalTab, 
	rb_Recycler.ModuleID, 
	rb_Modules.ModuleTitle, 
    rb_Pages.PageName as 'OriginalPageName'

FROM    rb_Recycler INNER JOIN
        rb_Modules ON rb_Recycler.ModuleID = rb_Modules.ModuleID INNER JOIN
        rb_Pages ON rb_Recycler.OriginalTab = rb_Pages.PageID
WHERE PortalID = @PortalID
ORDER BY 
	CASE 
	 WHEN @SortField = 'DateDeleted' THEN CAST(DateDeleted AS VarChar(50))
         WHEN @SortField = 'DeletedBy' THEN CAST(DeletedBy AS VarChar(50))
         WHEN @SortField = 'OriginalTab' THEN CAST(OriginalTab AS VarChar(50))
         WHEN @SortField = 'ModuleID' THEN CAST(rb_Recycler.ModuleID AS VarChar(50)) 
         WHEN @SortField = 'ModuleTitle' THEN CAST(ModuleTitle AS VarChar(50))
         WHEN @SortField = 'PageName' THEN CAST(rb_Pages.PageName AS VarChar(50))
         ELSE ModuleTitle
     END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  PROCEDURE rb_GetAuthViewRolesRecycler
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @ViewRoles   = rb_Modules.AuthorizedViewRoles
    
FROM    
    rb_Modules
  INNER JOIN
	rb_Recycler ON rb_Modules.ModuleID = rb_Recycler.ModuleID
  INNER join
    rb_Pages ON rb_Recycler.OriginalTab = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID
GO    


    
CREATE  PROCEDURE rb_GetAuthDeleteRolesRecycler
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @DeleteRoles   = rb_Modules.AuthorizedDeleteRoles
    
FROM    
    rb_Modules
  INNER JOIN
	rb_Recycler ON rb_Modules.ModuleID = rb_Recycler.ModuleID
  INNER join
    rb_Pages ON rb_Recycler.OriginalTab = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID

GO
