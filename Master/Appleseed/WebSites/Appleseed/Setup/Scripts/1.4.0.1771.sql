/* Checks to see if rb_GetUsersNoRole already exists */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetUsersNoRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetUsersNoRole]
GO

CREATE PROCEDURE rb_GetUsersNoRole
(
    @PortalID int
)
AS
/* Returns all members who do not have a specified role */
SELECT  
    rb_Users.UserID,
    Name,
    Email
FROM
    rb_Users
    
WHERE PortalID = @PortalID AND rb_Users.UserID  NOT IN 
(SELECT UserID FROM rb_UserRoles);

GO

/* Drops rb_GetCurrentModuleDefinitions */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetCurrentModuleDefinitions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetCurrentModuleDefinitions]
GO

CREATE PROCEDURE rb_GetCurrentModuleDefinitions
(
    @PortalID  int
)
AS
/* returns all module definitions for the specified portal */
SELECT  
    rb_GeneralModuleDefinitions.FriendlyName,
    rb_GeneralModuleDefinitions.DesktopSrc,
    rb_GeneralModuleDefinitions.MobileSrc,
    rb_GeneralModuleDefinitions.Admin,
    rb_ModuleDefinitions.ModuleDefID
FROM
    rb_ModuleDefinitions
INNER JOIN
	rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE   
    rb_ModuleDefinitions.PortalID = @PortalID
ORDER BY
rb_GeneralModuleDefinitions.Admin, rb_GeneralModuleDefinitions.FriendlyName
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetRoleNonMembership]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetRoleNonMembership]
GO

CREATE PROCEDURE rb_GetRoleNonMembership
(
    @RoleID  int,
    @PortalID int
)
AS
/* returns all members that are not in a specified role */
SELECT  
    rb_Users.UserID,
    Name,
    Email
FROM
    rb_Users
    
WHERE PortalID = @PortalID AND rb_Users.UserID  NOT IN 
(SELECT UserID 
FROM rb_UserRoles WHERE rb_UserRoles.RoleID = @RoleID);
GO

/* This is a duplicate sp that is no longer called from code so can be dropped */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthPublishRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthPublishRoles]
GO


