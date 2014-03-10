
IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserLoginByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLoginByID]
GO

CREATE PROCEDURE rb_UserLoginByID
(
    @PortalID int,
    @UserID   int
)
AS
SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email, rb_Users.Password, rb_Users.Salt
FROM   rb_Users
WHERE  (rb_Users.UserID = @UserID) AND (PortalID = @PortalID)
GO