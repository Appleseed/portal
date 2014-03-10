UPDATE rb_PortalSettings
SET SettingValue = 'registerfull.ascx' 
WHERE SettingName = 'SITESETTINGS_REGISTER_TYPE'
AND SettingValue = '0'
GO

UPDATE rb_PortalSettings
SET SettingValue = 'register.ascx' 
WHERE SettingName = 'SITESETTINGS_REGISTER_TYPE'
AND SettingValue = '1'
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_UpdateRole]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateRole]
GO

CREATE PROCEDURE rb_UpdateRole
(
    @RoleID      int,
    @RoleName    nvarchar(50)
)
AS

DECLARE @OldRoleName nvarchar(50)
SET @OldRoleName = (SELECT RoleName FROM rb_Roles WHERE RoleID = @RoleID)
--SELECT @OldRoleName  --For testing, make sure we got it...

IF UPPER(@OldRoleName) <> 'ADMINS'  --we don't change the name of the 'Admins' role, ever.
 BEGIN

  SELECT 
    rb_Modules.ModuleID, 
    REPLACE(rb_Modules.AuthorizedEditRoles, @OldRoleName, @RoleName) AS AuthorizedEditRoles,
    REPLACE(rb_Modules.AuthorizedAddRoles, @OldRoleName, @RoleName) AS AuthorizedAddRoles,
    REPLACE(rb_Modules.AuthorizedViewRoles, @OldRoleName, @RoleName) AS AuthorizedViewRoles,
    REPLACE(rb_Modules.AuthorizedDeleteRoles, @OldRoleName, @RoleName) AS AuthorizedDeleteRoles,
    REPLACE(rb_Modules.AuthorizedPropertiesRoles, @OldRoleName, @RoleName) AS AuthorizedPropertiesRoles
	INTO #MyTemp
	FROM rb_Modules
	INNER JOIN rb_Tabs ON rb_Tabs.TabID = rb_Modules.TabID
	INNER JOIN rb_Roles ON rb_Tabs.PortalID = rb_Roles.PortalID
	WHERE rb_Roles.RoleID = @RoleID
		AND rb_Tabs.TabID = rb_Modules.TabID

  --SELECT #MyTemp.ModuleID, #MyTemp.AuthorizedEditRoles FROM #MyTemp  --Used in testing


  /******Update the rb_Modules Table entries with the new Role name******/
  UPDATE rb
  SET 
    rb.AuthorizedEditRoles = #MyTemp.AuthorizedEditRoles, 
    rb.AuthorizedAddRoles = #MyTemp.AuthorizedAddRoles, 
    rb.AuthorizedViewRoles = #MyTemp.AuthorizedViewRoles, 
    rb.AuthorizedDeleteRoles = #MyTemp.AuthorizedDeleteRoles, 
    rb.AuthorizedPropertiesRoles = #MyTemp.AuthorizedPropertiesRoles

  FROM rb_Modules rb
  JOIN #MyTemp ON rb.ModuleID = #MyTemp.ModuleID

  /******Now update the rb_Roles table with the new Role name******/
  UPDATE rb_Roles
  SET rb_Roles.RoleName = @RoleName
  WHERE rb_Roles.RoleID = @RoleID

END
  
--We rename nothing if the user is messing with the 
--'Admins' role name because it is hard-coded
--in too many places and must not be changed.
GO
