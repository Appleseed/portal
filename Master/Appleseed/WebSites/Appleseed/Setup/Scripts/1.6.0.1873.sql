IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeletePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeletePortal]
GO

CREATE PROCEDURE rb_DeletePortal
(
    @PortalID       int
)
AS
DELETE FROM 
    rb_Portals 
WHERE 
    PortalID = @PortalID

DELETE FROM 
    rb_Tabs 
WHERE 
    PortalID = @PortalID
GO