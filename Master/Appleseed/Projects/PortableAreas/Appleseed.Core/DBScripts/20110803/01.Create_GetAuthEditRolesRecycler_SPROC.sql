SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthEditRolesRecycler]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthEditRolesRecycler]
GO

CREATE  PROCEDURE rb_GetAuthEditRolesRecycler
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @EditRoles   = rb_Modules.AuthorizedEditRoles
    
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