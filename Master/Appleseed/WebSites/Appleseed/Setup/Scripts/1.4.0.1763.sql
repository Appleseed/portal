---------------------
--1.4.0.1763.sql
---------------------

--by Cemil on 24/05/2004
-- Updated version of rb_UserLoginByID to change the UserID type from nvarchar to int

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserLoginByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLoginByID]
GO

CREATE PROCEDURE rb_UserLoginByID
(
    @PortalID int,
    @UserID   int,
    @Password nvarchar(20)
)
AS
SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email
FROM   rb_Users
WHERE
	rb_Users.UserID = @UserID AND rb_Users.Password = @Password AND rb_Users.PortalID = @PortalID
GO