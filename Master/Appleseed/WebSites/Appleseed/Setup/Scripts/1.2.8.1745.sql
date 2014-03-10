-- Thierry : delete the references to the roles for users before deleting a role

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteRole]
GO

CREATE PROCEDURE rb_DeleteRole
(
    @RoleID int
)
AS

DELETE FROM
    rb_UserRoles
WHERE
    RoleID = @RoleID

DELETE FROM
    rb_Roles
WHERE
    RoleID = @RoleID
GO

