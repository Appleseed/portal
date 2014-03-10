-------------------
--1.2.8.1715.sql
---------------------


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleDefinitions]
GO

CREATE PROCEDURE rb_UpdateModuleDefinitions
(
    @GeneralModDefID	uniqueidentifier,
    @PortalID			int = -2,
    @ischecked			bit
)
AS

-- Passing -2 as @PortalID AND 0 as @ischecked you will uninstall for all portal (if not in use)
-- Passing -2 as @PortalID AND 1 as @ischecked you will install for all portal

if (@PortalID = -2)
BEGIN
	IF (@ischecked = 0)
		DELETE FROM rb_ModuleDefinitions WHERE
                rb_ModuleDefinitions.GeneralModDefID = @GeneralModDefID
		AND rb_ModuleDefinitions.ModuleDefID NOT IN (SELECT ModuleDefID FROM rb_Modules) --module is not in use
	ELSE 
		
	INSERT INTO rb_ModuleDefinitions (PortalID, GeneralModDefID)
	       (
		SELECT rb_Portals.PortalID, rb_GeneralModuleDefinitions.GeneralModDefID
		FROM   rb_Portals CROSS JOIN rb_GeneralModuleDefinitions
		WHERE rb_GeneralModuleDefinitions.GeneralModDefID = @GeneralModDefID 
	              AND PortalID >= 0
	              AND rb_Portals.PortalID NOT IN (SELECT PortalID FROM rb_ModuleDefinitions WHERE rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID)
		)
END

ELSE --PortalID <> -2

BEGIN
IF (@ischecked = 0)
	/*DELETE IF CLEARED */
	DELETE FROM rb_ModuleDefinitions WHERE rb_ModuleDefinitions.GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID
	
ELSE
IF NOT (EXISTS (SELECT ModuleDefID FROM rb_ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID))
	/* ADD IF CHECKED */
BEGIN
			INSERT INTO rb_ModuleDefinitions
			(
				PortalID,
				GeneralModDefID
			)
			VALUES
			(
				@PortalID,
				@GeneralModDefID
			)
END
END
GO
-- Add LangSwitch - Now it is a placeable module
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{25E3290E-3B9A-4302-9384-9CA01243C00F}'
SET @FriendlyName = 'Language Switcher'
SET @DesktopSrc = 'DesktopModules/LanguageSwitcher.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesLanguageSwitcher'
SET @Admin = 0
SET @Searchable = 0

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

-- Install it for ALL portals
EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, -2, 1
GO

-- Install SIGNIN for ALL portals
EXEC [rb_UpdateModuleDefinitions] '{A0F1F62B-FDC7-4de5-BBAD-A5DAF31D960A}', -2, 1
GO

--Adds signin on first page (sorry for duplicates ;) )
INSERT INTO rb_Modules (TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles, AuthorizedDeleteRoles, AuthorizedPropertiesRoles, CacheTime) VALUES (1,868,-1,'leftPane','Login','Admins;','Unauthenticated Users;Admins;','Admins;','Admins;','Admins;',0)

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1715','1.2.8.1715', CONVERT(datetime, '05/24/2003', 101))
GO

/*
--TEST
DECLARE @GeneralModDefID uniqueidentifier
SET @GeneralModDefID = '{25E3290E-3B9A-4302-9384-9CA01243C00F}'
-- Install it for all portals
EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, -2, 0

SELECT * FROM rb_ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID
GO*/


