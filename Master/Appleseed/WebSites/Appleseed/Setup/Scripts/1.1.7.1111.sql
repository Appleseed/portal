---------------------
--1.1.7.1111.sql
---------------------

--by Manu on 27/10/2003
-- CLEAN INSTALL of system tables AND procedures
-- Modules structures will be created by module itself

--PORTALS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Portals]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Portals] (
    [PortalID] [int] IDENTITY (-1, 1) NOT NULL ,
    [PortalAlias] [nvarchar] (128) NOT NULL ,
    [PortalName] [nvarchar] (128) NOT NULL ,
    [PortalPath] [nvarchar] (128) NULL ,
    [AlwaysShowEditButton] [bit] NOT NULL ,
    CONSTRAINT [PK_rb_Portals] PRIMARY KEY  NONCLUSTERED 
    (
        [PortalID]
    ),
    CONSTRAINT [IX_rb_Portals] UNIQUE  NONCLUSTERED 
    (
        [PortalAlias]
    )
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_PortalSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_PortalSettings] (
    [PortalID] [int] NOT NULL ,
    [SettingName] [nvarchar] (50) NOT NULL ,
    [SettingValue] [nvarchar] (1500) NOT NULL ,
    CONSTRAINT [PK_rb_PortalSettings] PRIMARY KEY  CLUSTERED 
    (
        [PortalID],
        [SettingName]
    ),
    CONSTRAINT [FK_rb_PortalSettings_rb_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [rb_Portals] (
        [PortalID]
    ) ON DELETE CASCADE  ON UPDATE CASCADE 
)
 CREATE  INDEX [IX_rb_PortalSettings] ON [rb_PortalSettings]([PortalID], [SettingName])
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Tabs]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Tabs] (
    [TabID] [int] IDENTITY (1, 1) NOT NULL ,
    [ParentTabID] [int] NULL ,
    [TabOrder] [int] NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [TabName] [nvarchar] (50) NOT NULL ,
    [MobileTabName] [nvarchar] (50) NOT NULL ,
    [AuthorizedRoles] [nvarchar] (256) NULL ,
    [ShowMobile] [bit] NOT NULL ,
    [TabLayout] [int] NULL ,
    CONSTRAINT [PK_rb_Tabs] PRIMARY KEY  NONCLUSTERED 
    (
        [TabID]
    ),
    CONSTRAINT [FK_rb_Tabs_rb_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [rb_Portals] (
        [PortalID]
    ),
    CONSTRAINT [FK_rb_Tabs_rb_Tabs] FOREIGN KEY 
    (
        [ParentTabID]
    ) REFERENCES [rb_Tabs] (
        [TabID]
    )
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_TabSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_TabSettings] (
    [TabID] [int] NOT NULL ,
    [SettingName] [nvarchar] (50) NOT NULL ,
    [SettingValue] [nvarchar] (1500) NOT NULL ,
    CONSTRAINT [PK_rb_Tabsettings] PRIMARY KEY  CLUSTERED 
    (
        [TabID],
        [SettingName]
    ),
    CONSTRAINT [FK_rb_Tabsettings_rb_Tabs] FOREIGN KEY 
    (
        [TabID]
    ) REFERENCES [rb_Tabs] (
        [TabID]
    ) ON DELETE CASCADE 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_GeneralModuleDefinitions] (
    [GeneralModDefID]  uniqueidentifier ROWGUIDCOL  NOT NULL ,
    [FriendlyName] [nvarchar] (128) NOT NULL ,
    [DesktopSrc] [nvarchar] (256) NOT NULL ,
    [MobileSrc] [nvarchar] (256) NOT NULL ,
    [AssemblyName] [varchar] (50) NOT NULL CONSTRAINT [DF_rb_GeneralModuleDefinitions_AssemblyName] DEFAULT ('Appleseed.DLL'),
    [ClassName] [nvarchar] (128) NULL ,
    [Admin] [bit] NULL CONSTRAINT [DF_rb_GeneralModuleDefinitions_Admin] DEFAULT (0),
    [Searchable] [bit] NULL CONSTRAINT [DF_rb_GeneralModuleDefinitions_Searchable] DEFAULT (0),
    CONSTRAINT [PK_rb_GeneralModuleDefinitions] PRIMARY KEY  CLUSTERED 
    (
        [GeneralModDefID]
    ),
    CONSTRAINT [IX_rb_GeneralModuleDefinitions] UNIQUE  NONCLUSTERED 
    (
        [FriendlyName]
    )
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_ModuleDefinitions] (
    [ModuleDefID] [int] IDENTITY (1, 1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [GeneralModDefID] [uniqueidentifier] NOT NULL ,
    CONSTRAINT [PK_rb_ModuleDefinitions] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleDefID]
    ),
    CONSTRAINT [FK_rb_ModuleDefinitions_rb_GeneralModuleDefinitions] FOREIGN KEY 
    (
        [GeneralModDefID]
    ) REFERENCES [rb_GeneralModuleDefinitions] (
        [GeneralModDefID]
    ) ON DELETE CASCADE 
)
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Modules] (
    [ModuleID] [int] IDENTITY (1, 1) NOT NULL ,
    [TabID] [int] NOT NULL ,
    [ModuleDefID] [int] NOT NULL ,
    [ModuleOrder] [int] NOT NULL ,
    [PaneName] [nvarchar] (50) NOT NULL ,
    [ModuleTitle] [nvarchar] (256) NULL ,
    [AuthorizedEditRoles] [nvarchar] (256) NULL ,
    [AuthorizedViewRoles] [nvarchar] (256) NULL ,
    [AuthorizedAddRoles] [nvarchar] (256) NULL ,
    [AuthorizedDeleteRoles] [nvarchar] (256) NULL ,
    [AuthorizedPropertiesRoles] [nvarchar] (256) NULL ,
    [CacheTime] [int] NOT NULL ,
    [ShowMobile] [bit] NULL ,
    [AuthorizedPublishingRoles] [nvarchar] (256) NULL ,
    [NewVersion] [bit] NULL CONSTRAINT [DF_rb_Modules_NewVersion] DEFAULT (1),
    [SupportWorkflow] [bit] NULL ,
    [AuthorizedApproveRoles] [nvarchar] (256) NULL ,
    [WorkflowState] [tinyint] NULL CONSTRAINT [DF_rb_Modules_WorkflowState] DEFAULT (0),
    [LastModified] [datetime] NULL ,
    [LastEditor] [nvarchar] (256) NULL ,
    [StagingLastModified] [datetime] NULL ,
    [StagingLastEditor] [nvarchar] (256) NULL ,
    [SupportCollapsable] [bit] NULL CONSTRAINT [DF_rb_Modules_SupportCollapsable] DEFAULT (0),
    [ShowEveryWhere] [bit] NULL CONSTRAINT [DF_rb_Modules_ShowEveryWhere] DEFAULT (0),
    CONSTRAINT [PK_rb_Modules] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleID]
    ),
    CONSTRAINT [FK_rb_Modules_rb_ModuleDefinitions] FOREIGN KEY  --removed trailing 1
    (
        [ModuleDefID]
    ) REFERENCES [rb_ModuleDefinitions] (
        [ModuleDefID]
    ),
    CONSTRAINT [FK_rb_Modules_rb_Tabs] FOREIGN KEY --removed trailing 1
    (
        [TabID]
    ) REFERENCES [rb_Tabs] (
        [TabID]
    )
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ModuleSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_ModuleSettings] (
    [ModuleID] [int] NOT NULL ,
    [SettingName] [nvarchar] (50) NOT NULL ,
    [SettingValue] [nvarchar] (1500) NOT NULL ,
    CONSTRAINT [PK_rb_ModuleSettings] PRIMARY KEY  CLUSTERED 
    (
        [ModuleID],
        [SettingName]
    ),
    CONSTRAINT [FK_rb_ModuleSettings_rb_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [rb_Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  ON UPDATE CASCADE 
)
 CREATE  INDEX [IX_rb_ModuleSettings] ON [rb_ModuleSettings]([ModuleID], [SettingName])
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Users]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Users] (
    [UserID] [int] IDENTITY (1, 1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [Name] [nvarchar] (50) NOT NULL ,
    [Company] [nvarchar] (50) NULL ,
    [Address] [nvarchar] (50) NULL ,
    [City] [nvarchar] (50) NULL ,
    [Zip] [nvarchar] (6) NULL ,
    [CountryID] [nchar] (2) NULL ,
    [StateID] [int] NULL ,
    [PIva] [nvarchar] (11) NULL ,
    [CFiscale] [nvarchar] (16) NULL ,
    [Phone] [nvarchar] (50) NULL ,
    [Fax] [nvarchar] (50) NULL ,
    [Password] [nvarchar] (20) NULL ,
    [Email] [nvarchar] (100) NOT NULL ,
    [SendNewsletter] [bit] NULL CONSTRAINT [DF_rb_Users_SendNewsletter] DEFAULT (1),
    [MailChecked] [tinyint] NULL CONSTRAINT [DF_rb_Users_MailChecked] DEFAULT (0),
    [LastSend] [smalldatetime] NULL ,
    CONSTRAINT [PK_rb_Users] PRIMARY KEY  NONCLUSTERED 
    (
        [UserID]
    ),
    CONSTRAINT [IX_rb_Users] UNIQUE  NONCLUSTERED 
    (
        [Email],
        [PortalID]
    ),
    CONSTRAINT [FK_rb_Users_rb_Countries] FOREIGN KEY 
    (
        [CountryID]
    ) REFERENCES [rb_Countries] (
        [CountryID]
    ) ON UPDATE CASCADE ,
    CONSTRAINT [FK_rb_Users_rb_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [rb_Portals] (
        [PortalID]
    ) ON DELETE CASCADE ,
    CONSTRAINT [FK_rb_Users_rb_States] FOREIGN KEY 
    (
        [StateID]
    ) REFERENCES [rb_States] (
        [StateID]
    ) ON UPDATE CASCADE 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Roles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Roles] (
    [RoleID] [int] IDENTITY (1,1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [RoleName] [nvarchar] (50) NOT NULL ,
    [Permission] [tinyint] NULL CONSTRAINT [DF_rb_Roles_Permission] DEFAULT (1),
    CONSTRAINT [PK_rb_Roles] PRIMARY KEY  NONCLUSTERED 
    (
        [RoleID]
    ),
    CONSTRAINT [IX_rb_Roles] UNIQUE  NONCLUSTERED 
    (
        [PortalID],
        [RoleName]
    ),
    CONSTRAINT [FK_rb_Roles_rb_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [rb_Portals] (
        [PortalID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserRoles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_UserRoles] (
    [UserID] [int] NOT NULL ,
    [RoleID] [int] NOT NULL ,
    CONSTRAINT [FK_rb_UserRoles_rb_Roles] FOREIGN KEY 
    (
        [RoleID]
    ) REFERENCES [rb_Roles] (
        [RoleID]
    ),
    CONSTRAINT [FK_rb_UserRoles_rb_Users] FOREIGN KEY 
    (
        [UserID]
    ) REFERENCES [rb_Users] (
        [UserID]
    ) ON DELETE CASCADE  ON UPDATE CASCADE 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserDesktop]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_UserDesktop] (
    [UserID] [int] NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [TabID] [int] NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [State] [smallint] NOT NULL ,
    CONSTRAINT [PK_rb_UserDesktop] PRIMARY KEY  CLUSTERED 
    (
        [UserID],
        [ModuleID],
        [TabID],
        [PortalID]
    ),
    CONSTRAINT [FK_rb_UserDesktop_rb_Users] FOREIGN KEY 
    (
        [UserID]
    ) REFERENCES [rb_Users] (
        [UserID]
    ) ON DELETE CASCADE 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Versions] (
    [Release] [int] NOT NULL ,
    [Version] [nvarchar] (50) NULL ,
    [ReleaseDate] [datetime] NULL 
)
END
GO


--*********************************************
--**           STORED PROCEDURES             **
--*********************************************

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddGeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddGeneralModuleDefinitions]
GO

CREATE PROCEDURE rb_AddGeneralModuleDefinitions
    @GeneralModDefID uniqueidentifier,
    @FriendlyName nvarchar(128),
    @DesktopSrc nvarchar(256),
    @MobileSrc nvarchar(256),
    @AssemblyName varchar(50),
    @ClassName nvarchar(128),
    @Admin bit,
    @Searchable bit
AS
IF EXISTS (SELECT * FROM rb_GeneralModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID)
UPDATE rb_GeneralModuleDefinitions
SET
    FriendlyName = @FriendlyName,
    DesktopSrc = @DesktopSrc,
    MobileSrc = @MobileSrc,
    AssemblyName = @AssemblyName,
    ClassName = @ClassName,
    Admin = @Admin,
    Searchable = @Searchable
WHERE
    GeneralModDefID = @GeneralModDefID
ELSE
INSERT INTO rb_GeneralModuleDefinitions
(
    GeneralModDefID,
    FriendlyName,
    DesktopSrc,
    MobileSrc,
    AssemblyName,
    ClassName,
    Admin,
    Searchable
)
VALUES
(
    @GeneralModDefID,
    @FriendlyName,
    @DesktopSrc,
    @MobileSrc,
    @AssemblyName,
    @ClassName,
    @Admin,
    @Searchable
)
GO

/* Add AuthorizedMoveModuleRoles & AuthorizedDeleteModuleRoles in table rb_Modules */
IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedMoveModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
    [AuthorizedMoveModuleRoles] [nvarchar] (256) NULL 
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedDeleteModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
    [AuthorizedDeleteModuleRoles] [nvarchar] (256) NULL 
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddModule]
GO

CREATE PROCEDURE rb_AddModule
(
    @TabID                  int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @ModuleDefID            int,
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles	    nvarchar(256),
    @MoveModuleRoles	    nvarchar(256),
    @DeleteModuleRoles	    nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles        nvarchar(256),
    @SupportWorkflow	    bit,
    @ShowEveryWhere         bit,
    @SupportCollapsable     bit,
    @ModuleID               int OUTPUT
   
)
AS
--1772 update
INSERT INTO rb_Modules
(
    TabID,
    ModuleOrder,
    ModuleTitle,
    PaneName,
    ModuleDefID,
    CacheTime,
    AuthorizedEditRoles,
    AuthorizedAddRoles,
    AuthorizedViewRoles,
    AuthorizedDeleteRoles,
    AuthorizedPropertiesRoles,
    AuthorizedMoveModuleRoles,
    AuthorizedDeleteModuleRoles,
    ShowMobile,
    AuthorizedPublishingRoles,
    NewVersion, 
    SupportWorkflow,
    SupportCollapsable,
    ShowEveryWhere    
) 
VALUES
(
    @TabID,
    @ModuleOrder,
    @ModuleTitle,
    @PaneName,
    @ModuleDefID,
    @CacheTime,
    @EditRoles,
    @AddRoles,
    @ViewRoles,
    @DeleteRoles,
    @PropertiesRoles,
    @MoveModuleRoles,
    @DeleteModuleRoles,
    @ShowMobile,
    @PublishingRoles,
    1, -- False
    @SupportWorkflow,
    @SupportCollapsable,
    @ShowEveryWhere  
)
SELECT 
    @ModuleID = @@IDENTITY

GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteModule]
GO

CREATE PROCEDURE rb_DeleteModule
(
    @ModuleID       int
)
AS
DELETE FROM 
    rb_Modules 
WHERE 
    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteModuleDefinition]
GO

CREATE PROCEDURE rb_DeleteModuleDefinition
(
    @ModuleDefID uniqueidentifier
)
AS
DELETE FROM
    rb_GeneralModuleDefinitions
WHERE
    GeneralModDefID = @ModuleDefID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleDefinitions]
GO

CREATE PROCEDURE rb_UpdateModuleDefinitions
(
    @GeneralModDefID	uniqueidentifier,
    @PortalID			int = -2,
    @ischecked			bit
)
AS

-- Passing -2 AS @PortalID AND 0 AS @ischecked you will uninstall for all portal (if not in use)
-- Passing -2 AS @PortalID AND 1 AS @ischecked you will install for all portal

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
    /* DELETE IF CLEARED */
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddPortal]
GO

CREATE PROCEDURE rb_AddPortal
(
    @PortalAlias            nvarchar(128),
    @PortalName             nvarchar(128),
    @PortalPath             nvarchar(128),
    @AlwaysShowEditButton   bit,
    @PortalID               int OUTPUT
)
AS
INSERT INTO rb_Portals
(
    PortalAlias,
    PortalName,
    PortalPath,
    AlwaysShowEditButton
)
VALUES
(
    @PortalAlias,
    @PortalName,
    @PortalPath,
    @AlwaysShowEditButton
)
SELECT
    @PortalID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddRole]
GO

CREATE PROCEDURE rb_AddRole
(
    @PortalID    int,
    @RoleName    nvarchar(50),
    @RoleID      int OUTPUT
)
AS
INSERT INTO rb_Roles
(
    PortalID,
    RoleName
)
VALUES
(
    @PortalID,
    @RoleName
)
SELECT
    @RoleID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddTab]
GO

CREATE PROCEDURE rb_AddTab
(
    @PortalID   int,
    @TabName    nvarchar(50),
    @TabOrder   int,
    @AuthorizedRoles nvarchar (256),
    @MobileTabName nvarchar(50),
    @TabID      int OUTPUT
)
AS
INSERT INTO rb_Tabs
(
    PortalID,
    TabName,
    TabOrder,
    ShowMobile,
    MobileTabName,
    AuthorizedRoles
)
VALUES
(
    @PortalID,
    @TabName,
    @TabOrder,
    0, /* false */
    @MobileTabName,
    @AuthorizedRoles
)
SELECT
    @TabID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUser]
GO

CREATE PROCEDURE rb_AddUser
(
    @PortalID int,
    @Name     nvarchar(50),
    @Email    nvarchar(100),
    @Password nvarchar(20),
    @UserID   int OUTPUT
)
AS
INSERT INTO rb_Users
(
    Name,
    Email,
    Password,
    PortalID
)
VALUES
(
    @Name,
    @Email,
    @Password,
    @PortalID
)
SELECT
    @UserID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUserFull]
GO

CREATE PROCEDURE rb_AddUserFull
(
    @PortalID	    	    int,
    @Name		    nvarchar(50),
    @Company	            nvarchar(50),
    @Address	            nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	            nvarchar(16),
    @Email		    nvarchar(100),
    @Password	            nvarchar(20),
    @SendNewsletter	    bit,
    @CountryID		    nchar(2),  
    @StateID	            int,
    @UserID		    int OUTPUT
)
AS
INSERT INTO rb_Users
(
    PortalID,
    Name,
    Company,
    Address,		
    City,		
    Zip,		
    Phone,		
    Fax,		
    PIva,		
    CFiscale,	
    Email,		
    Password,
    SendNewsletter,
    CountryID,
    StateID
)
VALUES
(
    @PortalID,
    @Name,
    @Company,
    @Address,	
    @City,	
    @Zip,	
    @Phone,	
    @Fax,	
    @PIva,	
    @CFiscale,
    @Email,
    @Password,
    @SendNewsletter,
    @CountryID,
    @StateID
)
SELECT
    @UserID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUserRole]
GO

CREATE PROCEDURE rb_AddUserRole
(
    @UserID int,
    @RoleID int
)
AS
SELECT 
    *
FROM
    rb_UserRoles
WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID
/* only insert if the record doesn't yet exist */
IF @@Rowcount < 1
    INSERT INTO rb_UserRoles
    (
        UserID,
        RoleID
    )
    VALUES
    (
        @UserID,
        @RoleID
    )
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Approve]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Approve]
GO

CREATE PROCEDURE rb_Approve
    @moduleID	int
AS
    UPDATE	rb_Modules
    SET
        WorkflowState = 3 -- Approved
    WHERE
        [ModuleID] = @moduleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ChangeObjectOwner]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ChangeObjectOwner]
GO

CREATE   PROCEDURE rb_ChangeObjectOwner
/***********************************************************************
FileName:
    ChangeObjectOwner.sql
Description:
    Contains SQL used to change Appleseed database objects owner from 
    '<MACHINEName>\ASPNET' to 'dbo'.
    
    The script takes all user defined objects that are owned by user different than 'dbo'
    AND changes its ownership to 'dbo'. 
    
Assumptions:				
    The script assumes that Appleseed portal is installed with Appleseed AS a database Name.
    If the Name of the database is different, change the first line of the script. 
                    
    The script assumes that the new user Name is 'dbo'. If different user is required,
    change the 'dbo' Name in the script (it must be a valid user in the database).
History:
    06/30/2003 -- Marek Kepinski (mkepinski@impaq.com.pl)		
        Initial implementation.
    
Notes:
    Ignore warning displayed be SQL Server concerning possible breaking of stored procedures
    due to changes of object owner.
*************************************************************************/
(
    -- change the new owner Name if you need 
    @NewOwner nvarchar(128) = 'dbo'
)
AS
SET NOCOUNT ON
DECLARE @UserName sysname
DECLARE @ObjectName sysname
DECLARE @cmd nvarchar(255)
-- new owner must be a valid login
IF NOT EXISTS (SELECT * FROM sysusers WHERE islogin = 1 AND name = @NewOwner)
BEGIN
    DECLARE @Message varchar(200)
    SET @Message = 'User ' + @NewOwner + ' is not a valid user.'
    print @Message
END
ELSE
BEGIN
DECLARE C CURSOR FOR
    SELECT UserName = u.name, ObjectName = s.name
    FROM  sysobjects s INNER JOIN sysusers u ON s.uid = u.uid
        AND u.name <> @NewOwner
        AND s.xtype in ('V', 'P', 'U') 
        AND u.name not like 'INFORMATION%' 
    ORDER BY xtype, s.name
OPEN C
FETCH NEXT FROM C
    INTO @UserName, @ObjectName
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @cmd = 'sp_changeobjectowner @objName=''' + @UserName + '.' + @ObjectName + ''', @newowner=' + @NewOwner
    exec (@cmd)
    FETCH NEXT FROM C
    INTO @UserName, @ObjectName
END
CLOSE C
DEALLOCATE C
print 'Done.'
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeletePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteRole]
GO

CREATE PROCEDURE rb_DeleteRole
(
    @RoleID int
)
AS
DELETE FROM
    rb_Roles
WHERE
    RoleID = @RoleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteTab]
GO

CREATE PROCEDURE rb_DeleteTab
(
    @TabID int
)
AS
DELETE FROM
    rb_Tabs
WHERE
    TabID = @TabID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteUser]
GO

CREATE PROCEDURE rb_DeleteUser
(
    @UserID int
)
AS
DELETE FROM
    rb_Users
WHERE
    UserID=@UserID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteUserRole]
GO

CREATE PROCEDURE rb_DeleteUserRole
(
    @UserID int,
    @RoleID int
)
AS
DELETE FROM
    rb_UserRoles
WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthAddRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthAddRoles]
GO

CREATE PROCEDURE rb_GetAuthAddRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @AddRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @AddRoles   = rb_Modules.AuthorizedAddRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthApproveRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthApproveRoles]
GO

CREATE  PROCEDURE rb_GetAuthApproveRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ApproveRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @ApproveRoles   = rb_Modules.AuthorizedApproveRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthDeleteRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthDeleteRoles]
GO

CREATE PROCEDURE rb_GetAuthDeleteRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @DeleteRoles   = rb_Modules.AuthorizedDeleteRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthEditRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthEditRoles]
GO

CREATE PROCEDURE rb_GetAuthEditRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @EditRoles   = rb_Modules.AuthorizedEditRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthPropertiesRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthPropertiesRoles]
GO

CREATE PROCEDURE rb_GetAuthPropertiesRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PropertiesRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @PropertiesRoles   = rb_Modules.AuthorizedPropertiesRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthPublishingRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthPublishingRoles]
GO

CREATE PROCEDURE rb_GetAuthPublishingRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PublishingRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @PublishingRoles   = rb_Modules.AuthorizedPublishingRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetAuthViewRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAuthViewRoles]
GO

CREATE PROCEDURE rb_GetAuthViewRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @ViewRoles   = rb_Modules.AuthorizedViewRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentDbVersion]
GO

CREATE PROCEDURE [rb_GetCurrentDbVersion]
AS
SELECT     TOP 1 Release
FROM         rb_Versions
ORDER BY Release DESC
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetCurrentModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentModuleDefinitions]
GO

/* returns all module definitions for the specified portal */
CREATE PROCEDURE rb_GetCurrentModuleDefinitions
(
    @PortalID  int
)
AS
SELECT  
    rb_GeneralModuleDefinitions.FriendlyName,
    rb_GeneralModuleDefinitions.DesktopSrc,
    rb_GeneralModuleDefinitions.MobileSrc,
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetDefaultCulture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetDefaultCulture]
GO

CREATE PROCEDURE rb_GetDefaultCulture
(
    @CountryID nchar(2)
)
AS
SELECT    CultureCode, CountryID
FROM      rb_Cultures
WHERE     (CountryID = @CountryID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetDocumentContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetDocumentContent]
GO

CREATE   PROCEDURE rb_GetDocumentContent
(
    @ItemID int,
    @WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
    SELECT
        Content,
        ContentType,
        ContentSize,
        FileFriendlyName
    FROM
        rb_Documents
    WHERE
        ItemID = @ItemID
ELSE
    SELECT
        Content,
        ContentType,
        ContentSize,
        FileFriendlyName
    FROM
        rb_Documents_st
    WHERE
        ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetGeneralModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetGeneralModuleDefinitionByName]
GO

/****** Oggetto: stored procedure GetGeneralModuleDefinitionByName    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE
rb_GetGeneralModuleDefinitionByName
(
    @FriendlyName nvarchar(128),
    @ModuleID uniqueidentifier OUTPUT
)
AS
SELECT @ModuleID =
(
SELECT  rb_GeneralModuleDefinitions.GeneralModDefID
FROM    rb_GeneralModuleDefinitions
WHERE   (rb_GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetLastModified]
GO

CREATE PROCEDURE rb_GetLastModified
    (
        @ModuleID int,
        @WorkflowVersion int,
        @LastModifiedBy	nvarchar(256) OUTPUT,
        @LastModifiedDate datetime OUTPUT
    )
AS
    if ( @WorkflowVersion = 1 )
    begin
        select @LastModifiedDate = [LastModified], @LastModifiedBy = [LastEditor]
        from rb_Modules
        WHERE [ModuleID] = @ModuleID
    end
    else
    begin
        select @LastModifiedDate = [StagingLastModified], @LastModifiedBy = [StagingLastEditor]
        from rb_Modules
        WHERE [ModuleID] = @ModuleID
    end
    /* SET NOCOUNT ON */
    RETURN
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleDefinitionByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleDefinitionByID]
GO

CREATE PROCEDURE
rb_GetModuleDefinitionByID
(
    @ModuleID int
)
AS
SELECT     rb_ModuleDefinitions.ModuleDefID, rb_ModuleDefinitions.PortalID, rb_GeneralModuleDefinitions.FriendlyName, rb_GeneralModuleDefinitions.DesktopSrc, 
                      rb_GeneralModuleDefinitions.MobileSrc, rb_GeneralModuleDefinitions.Admin, rb_Modules.ModuleID
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
                      rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE     (rb_Modules.ModuleID = @ModuleID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleDefinitionByName]
GO

CREATE PROCEDURE rb_GetModuleDefinitionByName
(
    @PortalID int,
    @FriendlyName nvarchar(128),
    @ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     rb_ModuleDefinitions.ModuleDefID
    FROM       rb_GeneralModuleDefinitions LEFT JOIN
                    rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
    WHERE      (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleDefinitionByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleDefinitionByGuid]
GO

CREATE PROCEDURE rb_GetModuleDefinitionByGuid
(
    @PortalID int,
    @Guid uniqueidentifier,
    @ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     rb_ModuleDefinitions.ModuleDefID
    FROM         rb_GeneralModuleDefinitions LEFT OUTER JOIN
                          rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
    WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_ModuleDefinitions.GeneralModDefID = @Guid)
)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetGuid]
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleDefinitions]
GO

/* returns all module definitions for the specified portal */
CREATE PROCEDURE rb_GetModuleDefinitions
AS
SELECT     GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc
FROM         rb_GeneralModuleDefinitions
ORDER BY Admin, FriendlyName
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleInUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleInUse]
GO

CREATE PROCEDURE rb_GetModuleInUse
(
    @ModuleID uniqueidentifier
)
AS
SELECT     rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, '1' AS Checked
FROM         rb_Portals LEFT OUTER JOIN
                      rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
WHERE     (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)
UNION
SELECT DISTINCT
    PortalID, PortalAlias, PortalName, '0' AS Checked
FROM   rb_Portals
WHERE  
(
PortalID NOT IN
    (SELECT     rb_Portals.PortalID
     FROM       rb_Portals LEFT OUTER JOIN rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
     WHERE      (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)
    )
)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModulesAllPortals]
GO

--Fix on Shortuctall module, shortcuts should not be displayed on rb_GetModulesAllPortals list
CREATE PROCEDURE rb_GetModulesAllPortals
AS
SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' AS PortalAlias, -1 AS TabOrder
UNION
    SELECT     rb_Modules.ModuleID, rb_Portals.PortalAlias + '/' + rb_Tabs.TabName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, rb_Tabs.TabOrder
    FROM         rb_Modules INNER JOIN
                          rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
                          rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID INNER JOIN
                          rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                          rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
    WHERE     (rb_Modules.ModuleID > 0) AND (rb_GeneralModuleDefinitions.Admin = 0) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
                          rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
ORDER BY PortalAlias, ModuleTitle
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModulesByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModulesByName]
GO

CREATE PROCEDURE rb_GetModulesByName
(
    @ModuleName varchar(128),
    @PortalID int
)
AS
SELECT      0 ModuleID, ' Nessun modulo' ModuleTitle
UNION
SELECT     rb_Modules.ModuleID, rb_Modules.ModuleTitle
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
                      rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.FriendlyName = @ModuleName)
ORDER BY ModuleTitle
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModuleSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModuleSettings]
GO

CREATE PROCEDURE rb_GetModuleSettings
(
    @ModuleID int
)
AS
SELECT     SettingName, SettingValue
FROM         rb_ModuleSettings
WHERE     (ModuleID = @ModuleID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetModulesSinglePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetModulesSinglePortal]
GO

CREATE PROCEDURE rb_GetModulesSinglePortal
(
    @PortalID  int
)
AS
SELECT      0 ModuleID, 'NO_MODULE' ModuleTitle, -1 AS TabOrder
UNION
    SELECT     rb_Modules.ModuleID, rb_Tabs.TabName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')' AS ModTitle, rb_Tabs.TabOrder
    FROM         rb_Modules INNER JOIN
                          rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
                          rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                          rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
    WHERE     (rb_Tabs.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
                          rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
    ORDER BY TabOrder, ModuleTitle
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalCustomSettings]
GO

CREATE PROCEDURE rb_GetPortalCustomSettings
(
    @PortalID int
)
AS
SELECT
    SettingName,
    SettingValue
FROM
    rb_PortalSettings
WHERE
    PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalRoles]
GO

/* returns all roles for the specified portal */
CREATE PROCEDURE rb_GetPortalRoles
(
    @PortalID  int
)
AS
SELECT  
    RoleName,
    RoleID
FROM
    rb_Roles
WHERE   
    PortalID = @PortalID
order by RoleID
/* questo assicura che l'ultimo inserito si in fondo alla lista */
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortals]
GO

CREATE PROCEDURE rb_GetPortals
AS
SELECT  rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, rb_Portals.PortalPath, rb_Portals.AlwaysShowEditButton
FROM    rb_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettings]
GO

--Manu - Created 25/06/2003
--Manu fixed tab Name on active tab for localized items - 13/10/2203
CREATE PROCEDURE rb_GetPortalSettings
(
    @PortalAlias   nvarchar(50),
    @TabID         int,
    @PortalLanguage nvarchar(12),
    @PortalID      int OUTPUT,
    @PortalName    nvarchar(128) OUTPUT,
    @PortalPath    nvarchar(128) OUTPUT,
    @AlwaysShowEditButton bit OUTPUT,
    @TabName       nvarchar (50)  OUTPUT,
    @TabOrder      int OUTPUT,
    @ParentTabID      int OUTPUT,
    @MobileTabName nvarchar (50)  OUTPUT,
    @AuthRoles     nvarchar (256) OUTPUT,
    @ShowMobile    bit OUTPUT
)
AS
/* First, get Out Params */
IF @TabID = 0 
    SELECT TOP 1
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabID         = rb_Tabs.TabID,
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @TabName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName)  ,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
    WHERE
        PortalAlias=@PortalAlias
        
    ORDER BY
        TabOrder
ELSE 
    SELECT
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName),
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
        
    WHERE
        TabID=@TabID AND rb_Portals.PortalAlias=@PortalAlias
/* Get Tabs list */
SELECT     COALESCE ((SELECT     SettingValue
                              FROM         rb_TabSettings
                              WHERE     TabID = rb_Tabs.TabID AND SettingName = @PortalLanguage AND Len(SettingValue) > 0), TabName) AS TabName, AuthorizedRoles, TabID, ParentTabID, TabOrder, 
                      TabLayout
FROM         rb_Tabs
WHERE     (PortalID = @PortalID)
ORDER BY TabOrder
    
/* Get Mobile Tabs list */
SELECT  
    MobileTabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    ShowMobile  
FROM    
    rb_Tabs
WHERE   
    PortalID = @PortalID  AND  ShowMobile = 1
ORDER BY
    TabOrder
/* Then, get the DataTable of module info */
SELECT     *
FROM         rb_Modules INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.TabID = @TabID) OR
                      (rb_Modules.ShowEveryWhere = 1) AND (rb_ModuleDefinitions.PortalID = @PortalID)
ORDER BY rb_Modules.ModuleOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalSettingsLangList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettingsLangList]
GO

CREATE PROCEDURE rb_GetPortalSettingsLangList
(
    @PortalAlias   nvarchar(50)
)
AS
SELECT
    rb_PortalSettings.SettingValue
FROM
    rb_PortalSettings 
        INNER JOIN
    rb_Portals ON rb_PortalSettings.PortalID = rb_Portals.PortalID
WHERE
    (rb_Portals.PortalAlias = @PortalAlias) AND (rb_PortalSettings.SettingName = N'SITESETTINGS_LANGLIST')
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalSettingsPortalID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettingsPortalID]
GO

CREATE PROCEDURE rb_GetPortalSettingsPortalID
(
    @PortalID   nvarchar(50)
)
AS
    SELECT     TOP 1 PortalID, PortalName, PortalPath, AlwaysShowEditButton, PortalAlias
    FROM         rb_Portals
    WHERE     (PortalID = @PortalID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalsModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalsModules]
GO

CREATE PROCEDURE rb_GetPortalsModules
(
    @ModuleID  uniqueidentifier
)
AS
    SELECT     rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, rb_ModuleDefinitions.ModuleDefID
    FROM         rb_Portals LEFT OUTER JOIN
                          rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
    WHERE     (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetRoleMembership]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetRoleMembership]
GO

/* returns all members for the specified role */
CREATE PROCEDURE rb_GetRoleMembership
(
    @RoleID  int
)
AS
SELECT  
    rb_UserRoles.UserID,
    Name,
    Email
FROM
    rb_UserRoles
    
INNER JOIN 
    rb_Users On rb_Users.UserID = rb_UserRoles.UserID
WHERE   
    rb_UserRoles.RoleID = @RoleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetRolesByUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetRolesByUser]
GO

/* returns all roles for the specified user */
CREATE PROCEDURE rb_GetRolesByUser
(
    @PortalID		int,
    @Email         nvarchar(100)
)
AS
SELECT  
    rb_Roles.RoleName,
    rb_Roles.RoleID
FROM
    rb_UserRoles
  INNER JOIN 
    rb_Users ON rb_UserRoles.UserID = rb_Users.UserID
  INNER JOIN 
    rb_Roles ON rb_UserRoles.RoleID = rb_Roles.RoleID
WHERE   
    rb_Users.Email = @Email AND rb_Users.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSearchableModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSearchableModules]
GO

CREATE PROCEDURE rb_GetSearchableModules
(
    @PortalID int
)
AS
SELECT     rb_GeneralModuleDefinitions.GeneralModDefID, rb_GeneralModuleDefinitions.ClassName, rb_GeneralModuleDefinitions.FriendlyName, 
                      rb_GeneralModuleDefinitions.DesktopSrc, rb_GeneralModuleDefinitions.MobileSrc, rb_GeneralModuleDefinitions.Admin, rb_GeneralModuleDefinitions.Searchable, 
                      rb_GeneralModuleDefinitions.AssemblyName, rb_ModuleDefinitions.ModuleDefID
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
WHERE     (rb_GeneralModuleDefinitions.Searchable = 1) AND (rb_ModuleDefinitions.PortalID = @PortalID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSingleCountry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleCountry]
GO

CREATE PROCEDURE rb_GetSingleCountry
(
    @IDState int,
    @IDLang	nchar(2) = 'en'
)
AS
SELECT     rb_Countries.CountryID, rb_Localize.Description, rb_States.StateID
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID INNER JOIN
                      rb_States ON rb_Countries.CountryID = rb_States.CountryID
WHERE     (rb_Localize.CultureCode = @IDLang) AND (rb_States.StateID = @IDState) OR
                      (rb_States.StateID = @IDState) AND (rb_Cultures.NeutralCode = @IDLang)
ORDER BY rb_Localize.Description
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSingleModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleModuleDefinition]
GO

CREATE PROCEDURE rb_GetSingleModuleDefinition
(
    @GeneralModDefID uniqueidentifier
)
AS
SELECT
    GeneralModDefID, 
    FriendlyName,
    DesktopSrc,
    MobileSrc,
    Admin
FROM
    rb_GeneralModuleDefinitions
WHERE
    GeneralModDefID = @GeneralModDefID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSingleRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleRole]
GO

CREATE PROCEDURE rb_GetSingleRole
(
    @RoleID int
)
AS
SELECT
    RoleName
FROM
    rb_Roles
WHERE
    RoleID = @RoleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleUser]
GO

CREATE PROCEDURE rb_GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
    @IDLang	nchar(2) = 'IT'
)
AS
SELECT
    rb_Users.UserID,
    rb_Users.Email,
    rb_Users.Password,
    rb_Users.Name,
    rb_Users.Company,
    rb_Users.Address,
    rb_Users.City,
    rb_Users.Zip,
    rb_Users.CountryID,
    rb_Users.StateID,
    rb_Users.PIva,
    rb_Users.CFiscale,
    rb_Users.Phone,
    rb_Users.Fax,
    rb_Users.SendNewsletter,
    rb_Users.MailChecked,
    rb_Users.PortalID,
    
    
    (SELECT TOP 1 rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     ((rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)) AND (rb_Countries.CountryID = rb_Users.CountryID))
    
    
     AS Country
                      
FROM 
    rb_Users LEFT OUTER JOIN
    rb_States ON rb_Users.StateID = rb_States.StateID
    
WHERE
(rb_Users.Email = @Email) AND (rb_Users.PortalID = @PortalID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSolutions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSolutions]
GO

CREATE PROCEDURE rb_GetSolutions
AS
SELECT * FROM rb_Solutions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetStates]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetStates]
GO

CREATE PROCEDURE rb_GetStates
(
    @CountryID nchar(2)
)
AS
SELECT  StateID, 
        Description
FROM    rb_States
WHERE	CountryID = @CountryID
ORDER BY Description
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabCrumbs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabCrumbs]
GO

CREATE  proc rb_GetTabCrumbs
@TabID int,
@CrumbsXML nvarchar (4000) output
AS
--Variables used to build Crumb XML string
declare @ParentTabID int
declare @TabName AS nvarchar(50)
declare @Level int
--First Child in the branch is Crumb 20.  
set @Level =20
--Get First Parent Tab ID if there is one
set @ParentTabID = (select ParentTabID from rb_Tabs WHERE TabID=@TabID)
--Get TabName of Lowest Child
set @TabName = (select TabName from rb_Tabs WHERE TabID=@TabID)
--Build first Crumb
set @CrumbsXML = '<root><crumb TabID=''' + cast(@TabID AS varchar) + ''' Level=''' + cast(@Level AS varchar) + '''>' + @TabName + '</crumb>'
while @ParentTabID is not null
    begin
        set @Level=@Level - 1
        set @TabID=@ParentTabID
        set @ParentTabID=(select ParentTabID from rb_Tabs WHERE TabID=@TabID)
        set @TabName = (select TabName from rb_Tabs WHERE TabID=@TabID)
        set @CrumbsXML = @CrumbsXML + '<crumb TabID=''' + cast(@TabID AS varchar) + ''' Level=''' + cast(@Level AS varchar) + '''>' + @TabName + '</crumb>'
    end
set @CrumbsXML = @CrumbsXML + '</root>'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabCustomSettings]
GO

CREATE PROCEDURE rb_GetTabCustomSettings
(
    @TabID int
)
AS
SELECT
    SettingName,
    SettingValue
FROM
    rb_TabSettings
WHERE
    TabID = @TabID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabsByPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsByPortal]
GO

CREATE PROCEDURE rb_GetTabsByPortal
(
    @PortalID   int
)
AS
/* Get Tabs list */
SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
FROM         rb_Tabs
WHERE     (PortalID = @PortalID)
ORDER BY TabOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabSettings]
GO

--Fix by Onur Esnaf
CREATE PROCEDURE rb_GetTabSettings
(
    @TabID   int,
    @PortalLanguage nvarchar(12)
)
AS
 
IF (@TabID > 0)
 
/* Get Tabs list */
SELECT     
   COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName)  AS 
 TabName, 
 AuthorizedRoles, 
 TabID, 
 TabOrder, 
 ParentTabID, 
 MobileTabName, 
 ShowMobile, 
 PortalID
FROM         rb_Tabs
WHERE     (ParentTabID = @TabID)
ORDER BY TabOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabsFlat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsFlat]
GO

CREATE PROCEDURE rb_GetTabsFlat
(
        @PortalID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #TabTree
(
        [TabID] [int],
        [TabName] [nvarchar] (100),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO     #TabTree
SELECT  TabID,
        TabName,
        ParentTabID,
        TabOrder,
        0,
        cast(100000000 + TabOrder AS varchar)
FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder AS varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END
--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        '(Orphan)' + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    rb_Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.TabID)
                         AND PortalID =@PortalID
-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) AS ord , cast(TabID AS varchar) AS TabID into #tabs
from #TabTree
order by NestLevel, TreeOrder
-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID AS int)=#TabTree.TabID) 
-- Return Temporary Table
SELECT TabID, ParentTabID, TabName, TabOrder, NestLevel
FROM #TabTree 
order by TreeOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabsinTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsinTab]
GO

CREATE PROCEDURE rb_GetTabsinTab
(
    @PortalID int,
    @TabID int
)
AS
SELECT     TabID, TabName, ParentTabID, TabOrder, AuthorizedRoles
FROM         rb_Tabs
WHERE     (ParentTabID = @TabID) AND (PortalID = @PortalID)
ORDER BY TabOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsParent]
GO

CREATE PROCEDURE rb_GetTabsParent
(
    @PortalID int,
    @TabID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #TabTree
(
        [TabID] [int],
        [TabName] [nvarchar] (50),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO     #TabTree
SELECT  TabID,
        TabName,
        ParentTabID,
        TabOrder,
        0,
        cast(100000000 + TabOrder AS varchar)
FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder AS varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END
--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        '(Orphan)' + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    rb_Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.TabID)
                         AND PortalID =@PortalID
-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) AS ord , cast(TabID AS varchar) AS TabID into #tabs
from #TabTree
order by NestLevel, TreeOrder
-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID AS int)=#TabTree.TabID) 
-- Return Temporary Table
SELECT TabID, TabName, TreeOrder
FROM #TabTree 
UNION
SELECT 0 TabID, ' ROOT_LEVEL' TabName, '-1' AS TreeOrder
order by TreeOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetUsers]
GO

CREATE PROCEDURE rb_GetUsers
(
@PortalID int
)
AS
SELECT     UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, CountryID, StateID, PIva, CFiscale, Phone, Fax
FROM         rb_Users
WHERE     (PortalID = @PortalID)
ORDER BY Email
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetUsersCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetUsersCount]
GO

CREATE PROCEDURE rb_GetUsersCount
(
    @PortalID		int,
    @UsersCount		int OUTPUT
)
AS
SELECT TOP 1
@UsersCount = COUNT(DISTINCT rb_Users.UserID)
FROM  rb_Users
WHERE rb_Users.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetRelatedTables]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetRelatedTables]
GO

CREATE    PROCEDURE rb_GetRelatedTables
    @Name	nvarchar(128)
AS
    SELECT 
        [InnerResults].[ForeignKeyTableSchema],
        [InnerResults].[ForeignKeyTable], 
        [InnerResults].[ForeignKeyColumn], 
        [InnerResults].[KeyColumn],
        [InnerResults].[ForeignKeyTableID],
        [InnerResults].[KeyTableID],
        [InnerResults].[KeyTableSchema],
        [InnerResults].[KeyTable]
    FROM
        (
            SELECT     
                [FKeyTable].[TableName] AS ForeignKeyTable, 
                [FKeyTable].[TableSchema] AS ForeignKeyTableSchema,
                [KeyTable].[TableName] AS KeyTable, 
                [KeyTable].[TableSchema] AS KeyTableSchema,
                [FKeyColumns].[name] AS ForeignKeyColumn, 
                    [KeyColumns].[name] AS KeyColumn,
                [FKeyTable].[ID] AS ForeignKeyTableID,
                [KeyTable].[ID] AS KeyTableID
            FROM         sysforeignkeys INNER JOIN
                                  (
                            SELECT     
                                [sysobjects].[id] AS ID, 
                                [sysobjects].[name] AS TableName,
                                [INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] AS TableSchema
                            FROM    
                                [sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
                                    ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
                            WHERE   
                                ([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
                           ) FKeyTable ON sysforeignkeys.fkeyid = [FKeyTable].[ID] INNER JOIN
                           (
                            SELECT     
                                [sysobjects].[id] AS ID, 
                                [sysobjects].[name] AS TableName,
                                [INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] AS TableSchema
                            FROM    
                                [sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
                                    ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
                            WHERE   
                                ([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
                           ) KeyTable ON sysforeignkeys.rkeyid = [KeyTable].[ID] INNER JOIN
                                  syscolumns FKeyColumns ON [FKeyTable].[ID] = [FKeyColumns].[id] AND sysforeignkeys.fkey = [FKeyColumns].[colid] INNER JOIN
                                  syscolumns KeyColumns ON [KeyTable].[ID] = [KeyColumns].[id] AND sysforeignkeys.rkey = [KeyColumns].[colid]
        ) InnerResults
    WHERE			
        [InnerResults].[KeyTable] = @Name
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Publish]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Publish]
GO

-- Alter Publish stored procedure
CREATE   PROCEDURE rb_Publish
    @ModuleID	int
AS
    -- First get al list of tables which are related to the Modules table
    -- Create a temporary table
    CREATE TABLE #TMPResults
        (ForeignKeyTableSchema	nvarchar(128),
                 ForeignKeyTable	nvarchar(128),
         ForeignKeyColumn	nvarchar(128),
         KeyColumn		nvarchar(128),
         ForeignKeyTableID	int,
         KeyTableID		int,
         KeyTableSchema		nvarchar(128),
         KeyTable		nvarchar(128))
    INSERT INTO #TMPResults EXEC rb_GetRelatedTables 'rb_Modules'
    DECLARE RelatedTablesModules CURSOR FOR
        SELECT 	
            ForeignKeyTableSchema, 
            ForeignKeyTable,
            ForeignKeyColumn,
            KeyColumn,
            ForeignKeyTableID,
            KeyTableID,
            KeyTableSchema,
            KeyTable
        FROM
            #TMPResults
        WHERE 
            ForeignKeyTable <> 'ModuleSettings'
    -- Create temporary table for later use
    CREATE TABLE #TMPCount
        (ResultCount	int)
    -- Now search the table that hAS the related column
    OPEN RelatedTablesModules
    DECLARE @ForeignKeyTableSchema 	nvarchar(128)
    DECLARE @ForeignKeyTable	nvarchar(128)
    DECLARE @ForeignKeyColumn	nvarchar(128)
    DECLARE @KeyColumn		nvarchar(128)
    DECLARE @ForeignKeyTableID	int
    DECLARE @KeyTableID		int
    DECLARE @KeyTableSchema		nvarchar(128)
    DECLARE @KeyTable		nvarchar(128)
    DECLARE @SQLStatement		nvarchar(4000)
    DECLARE @Count			int
    DECLARE @TableHasIdentityCol	int
    FETCH NEXT FROM RelatedTablesModules 
    INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
        @KeyColumn, @ForeignKeyTableID, @KeyTableID,
        @KeyTableSchema, @KeyTable
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Check if this table hAS a corresponding staging table
        TRUNCATE TABLE #TMPCount
        SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = OBJECT_ID(N''[' + @ForeignKeyTable + '_st]'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
        -- PRINT @SQLStatement
        EXEC(@SQLStatement)
        SELECT @Count = ResultCount
            FROM #TMPCount		
        PRINT @ForeignKeyTable
        PRINT @Count
        IF (@Count = 1)
        BEGIN						
            -- Check if this table contains the related data
            TRUNCATE TABLE #TMPCount
            SET @SQLStatement = 
                'INSERT INTO #TMPCount ' +
                'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
                'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
                'UNION ' +
                'SELECT Count(*) FROM [' + @ForeignKeyTable + '_st] ' +
                'WHERE [' + @ForeignKeyTable + '_st].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 
            EXEC(@SQLStatement)
            SELECT @Count = ResultCount
                FROM #TMPCount		
            IF (@Count >= 1) 
            BEGIN
                -- This table contains the related data 
                -- Delete everything in the prod table
                SET @SQLStatement = 
                    'DELETE FROM [' + @ForeignKeyTable + '] ' +
                    'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                PRINT @SQLStatement
                EXEC(@SQLStatement)
                -- Copy everything from the staging table to the prod table
                SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableID, 'TableHasIdentity')
                IF (@TableHasIdentityCol = 1)
                BEGIN
                    -- The table contains a identity column
                    DECLARE TableColumns CURSOR FOR
                        SELECT [COLUMN_NAME]
                        FROM [INFORMATION_SCHEMA].[COLUMNS]
                        WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
                            AND [TABLE_NAME] = @ForeignKeyTable
                        ORDER BY [ORDINAL_POSITION]
    
                    OPEN TableColumns
    
                    DECLARE @ColumnList	nvarchar(4000)
                    SET @ColumnList = ''
                    DECLARE @ColName	nvarchar(128)
                    DECLARE @ColIsIdentity	int
    
                    FETCH NEXT FROM TableColumns
                    INTO @ColName
                    
                    SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
    
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        IF (@ColIsIdentity = 0)
                            SET @ColumnList = @ColumnList + '[' + @ColName + '] '
    
                        FETCH NEXT FROM TableColumns
                        INTO @ColName
    
                        IF (@@FETCH_STATUS = 0)
                        BEGIN
                            IF (@ColIsIdentity = 0)
                            BEGIN
                                SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
                                IF (@ColIsIdentity = 0)
                                    SET @ColumnList = @ColumnList + ', '		
                            END
                            ELSE
                                SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
                        END
                    END
                    
                    CLOSE TableColumns
                    DEALLOCATE TableColumns		
    
                    SET @SQLStatement = 	
                        'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
                        'SELECT ' + @ColumnList + ' FROM [' + @ForeignKeyTable + '_st] ' +
                        'WHERE [' + @ForeignKeyTable + '_st].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                    -- PRINT @SQLStatement
                    EXEC(@SQLStatement)
                END
                ELSE
                BEGIN
                    -- The table doens't contain a identitycolumn
                    SET @SQLStatement = 
                        'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
                        'SELECT * FROM [' + @ForeignKeyTable + '_st] ' +
                        'WHERE [' + @ForeignKeyTable + '_st].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                    EXEC(@SQLStatement)
                END
            END
        END
        FETCH NEXT FROM RelatedTablesModules 
        INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
            @KeyColumn, @ForeignKeyTableID, @KeyTableID,
            @KeyTableSchema, @KeyTable
    END
    CLOSE RelatedTablesModules
    DEALLOCATE RelatedTablesModules
            
    -- Set the module in the correct status
    UPDATE [rb_Modules]
    SET [WorkflowState] = 0, -- Original
        [LastModified] = [StagingLastModified],
            [LastEditor] = [StagingLastEditor]
    WHERE [ModuleID] = @ModuleID
    RETURN
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Reject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Reject]
GO

CREATE PROCEDURE rb_Reject
    @moduleID	int
AS
    UPDATE	rb_Modules
    SET
        WorkflowState = 1 -- Put status back to Working
    WHERE
        [ModuleID] = @moduleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_RequestApproval]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_RequestApproval]
GO

CREATE PROCEDURE rb_RequestApproval
    @moduleID	int
AS
    UPDATE	rb_Modules
    SET
        WorkflowState = 2 -- Request Approval
    WHERE
        [ModuleID] = @moduleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ModuleEdited]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModuleEdited]
GO

-- Alter ModuleEdited stored procedure
CREATE    PROCEDURE rb_ModuleEdited
    @ModuleID	int
AS

    -- Check if this module supports workflow
    DECLARE @support	bit

    SELECT @support = SupportWorkflow
    FROM rb_Modules
    WHERE ModuleID = @ModuleID

    IF ( @support = 1 )
    BEGIN
        -- It supports workflow
        UPDATE rb_Modules
        SET WorkflowState = 1 -- Working
        WHERE ModuleID = @ModuleID
    END
    ELSE
        -- It doesn't support workflow
        EXEC rb_Publish @ModuleID

    /* SET NOCOUNT ON */
    RETURN
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Revert]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Revert]
GO

-- Alter Publish stored procedure
CREATE    PROCEDURE rb_Revert
    @ModuleID	int
AS
    -- First get al list of tables which are related to the Modules table
    -- Create a temporary table
    CREATE TABLE #TMPResults
        (ForeignKeyTableSchema	nvarchar(128),
                 ForeignKeyTable	nvarchar(128),
         ForeignKeyColumn	nvarchar(128),
         KeyColumn		nvarchar(128),
         ForeignKeyTableID	int,
         KeyTableID		int,
         KeyTableSchema		nvarchar(128),
         KeyTable		nvarchar(128))
    INSERT INTO #TMPResults EXEC rb_GetRelatedTables 'rb_Modules'
    DECLARE RelatedTablesModules CURSOR FOR
        SELECT 	
            ForeignKeyTableSchema, 
            ForeignKeyTable,
            ForeignKeyColumn,
            KeyColumn,
            ForeignKeyTableID,
            KeyTableID,
            KeyTableSchema,
            KeyTable
        FROM
            #TMPResults
        WHERE 
            ForeignKeyTable <> 'ModuleSettings'
    -- Create temporary table for later use
    CREATE TABLE #TMPCount
        (ResultCount	int)
    -- Now search the table that hAS the related column
    OPEN RelatedTablesModules
    DECLARE @ForeignKeyTableSchema 	nvarchar(128)
    DECLARE @ForeignKeyTable	nvarchar(128)
    DECLARE @ForeignKeyColumn	nvarchar(128)
    DECLARE @KeyColumn		nvarchar(128)
    DECLARE @ForeignKeyTableID	int
    DECLARE @KeyTableID		int
    DECLARE @KeyTableSchema		nvarchar(128)
    DECLARE @KeyTable		nvarchar(128)
    DECLARE @SQLStatement		nvarchar(4000)
    DECLARE @Count			int
    DECLARE @TableHasIdentityCol	int
    FETCH NEXT FROM RelatedTablesModules 
    INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
        @KeyColumn, @ForeignKeyTableID, @KeyTableID,
        @KeyTableSchema, @KeyTable
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Check if this table hAS a corresponding staging table
        TRUNCATE TABLE #TMPCount
        SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = OBJECT_ID(N''[' + @ForeignKeyTable + '_st]'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
        -- PRINT @SQLStatement
        EXEC(@SQLStatement)
        SELECT @Count = ResultCount
            FROM #TMPCount		
        PRINT @ForeignKeyTable
        PRINT @Count
        IF (@Count = 1)
        BEGIN						
            -- Check if this table contains the related data
            TRUNCATE TABLE #TMPCount
            SET @SQLStatement = 
                'INSERT INTO #TMPCount ' +
                'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
                'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
                'UNION ' +
                'SELECT Count(*) FROM [' + @ForeignKeyTable + '_st] ' +
                'WHERE [' + @ForeignKeyTable + '_st].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 
            EXEC(@SQLStatement)
            SELECT @Count = ResultCount
                FROM #TMPCount		
            IF (@Count >= 1) 
            BEGIN
                -- This table contains the related data 
                -- Delete everything in the staging table
                SET @SQLStatement = 
                    'DELETE FROM [' + @ForeignKeyTable + '_st] ' +
                    'WHERE [' + @ForeignKeyTable + '_st].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                PRINT @SQLStatement
                EXEC(@SQLStatement)
                -- Copy everything from the staging table to the prod table
                SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableID, 'TableHasIdentity')
                IF (@TableHasIdentityCol = 1)
                BEGIN
                    -- The table contains a identity column
                    DECLARE TableColumns CURSOR FOR
                        SELECT [COLUMN_NAME]
                        FROM [INFORMATION_SCHEMA].[COLUMNS]
                        WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
                            AND [TABLE_NAME] = @ForeignKeyTable
                        ORDER BY [ORDINAL_POSITION]
    
                    OPEN TableColumns
    
                    DECLARE @ColumnList	nvarchar(4000)
                    SET @ColumnList = ''
                    DECLARE @ColName	nvarchar(128)
                    DECLARE @ColIsIdentity	int
    
                    FETCH NEXT FROM TableColumns
                    INTO @ColName
                    
                    SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
    
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        IF (@ColIsIdentity = 0)
                            SET @ColumnList = @ColumnList + '[' + @ColName + '] '
    
                        FETCH NEXT FROM TableColumns
                        INTO @ColName
    
                        IF (@@FETCH_STATUS = 0)
                        BEGIN
                            IF (@ColIsIdentity = 0)
                            BEGIN
                                SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
                                IF (@ColIsIdentity = 0)
                                    SET @ColumnList = @ColumnList + ', '		
                            END
                            ELSE
                                SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableID, @ColName, 'IsIdentity')
                        END
                    END
                    
                    CLOSE TableColumns
                    DEALLOCATE TableColumns		
    
                    SET @SQLStatement = 	
                        'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '_st] (' + @ColumnList + ') ' +
                        'SELECT ' + @ColumnList + ' FROM [' + @ForeignKeyTable + '] ' +
                        'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                    -- PRINT @SQLStatement
                    EXEC(@SQLStatement)
                END
                ELSE
                BEGIN
                    -- The table doens't contain a identitycolumn
                    SET @SQLStatement = 
                        'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '_st] ' +
                        'SELECT * FROM [' + @ForeignKeyTable + '] ' +
                        'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
                    EXEC(@SQLStatement)
                END
            END
        END
        FETCH NEXT FROM RelatedTablesModules 
        INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
            @KeyColumn, @ForeignKeyTableID, @KeyTableID,
            @KeyTableSchema, @KeyTable
    END
    CLOSE RelatedTablesModules
    DEALLOCATE RelatedTablesModules
            
    -- Set the module in the correct status
    UPDATE [rb_Modules]
    SET [WorkflowState] = 0, -- Original
        [LastModified] = [StagingLastModified],
            [LastEditor] = [StagingLastEditor]
    WHERE [ModuleID] = @ModuleID
    RETURN
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateGeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateGeneralModuleDefinitions]
GO

/* PROCEDURE rb_UpdateGeneralModuleDefinitions*/
CREATE PROCEDURE rb_UpdateGeneralModuleDefinitions
    @GeneralModDefID uniqueidentifier,
    @FriendlyName nvarchar(128),
    @DesktopSrc nvarchar(256),
    @MobileSrc nvarchar(256),
    @AssemblyName varchar(50),
    @ClassName nvarchar(128),
    @Admin bit,
    @Searchable bit
AS
UPDATE rb_GeneralModuleDefinitions
SET
    FriendlyName = @FriendlyName,
    DesktopSrc = @DesktopSrc,
    MobileSrc = @MobileSrc,
    AssemblyName = @AssemblyName,
    ClassName = @ClassName,
    Admin = @Admin,
    Searchable = @Searchable
WHERE
    GeneralModDefID = @GeneralModDefID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModule]
GO

CREATE PROCEDURE [rb_UpdateModule]
(
    @ModuleID               int,
    @TabID				    int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles        nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles	    nvarchar(256),
    @MoveModuleRoles	    nvarchar(256),
    @DeleteModuleRoles	    nvarchar(256),
    @SupportWorkflow	    bit,
    @ApprovalRoles			nvarchar(256),
    @ShowEveryWhere         bit,
    @SupportCollapsable     bit
)
AS
--From 1772
UPDATE
    rb_Modules
SET
   TabID			= @TabID,
    ModuleOrder               	= @ModuleOrder,
    ModuleTitle               	= @ModuleTitle,
    PaneName                  	= @PaneName,
    CacheTime                 	= @CacheTime,
    ShowMobile                	= @ShowMobile,
    AuthorizedEditRoles       	= @EditRoles,
    AuthorizedAddRoles        	= @AddRoles,
    AuthorizedViewRoles       	= @ViewRoles,
    AuthorizedDeleteRoles     	= @DeleteRoles,
    AuthorizedPropertiesRoles 	= @PropertiesRoles,
    AuthorizedMoveModuleRoles 	= @MoveModuleRoles,
    AuthorizedDeleteModuleRoles = @DeleteModuleRoles,
    AuthorizedPublishingRoles 	= @PublishingRoles,
    SupportWorkflow	        = @SupportWorkflow,
    AuthorizedApproveRoles    	= @ApprovalRoles,
    SupportCollapsable		= @SupportCollapsable,
    ShowEveryWhere              = @ShowEveryWhere
WHERE
    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleDefinitions]
GO

CREATE PROCEDURE rb_UpdateModuleDefinitions
(
    @GeneralModDefID	uniqueidentifier,
    @PortalID			int = -2,
    @ischecked			bit
)
AS
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModuleOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleOrder]
GO

CREATE PROCEDURE rb_UpdateModuleOrder
(
    @ModuleID           int,
    @ModuleOrder        int,
    @PaneName           nvarchar(50)
)
AS
UPDATE
    rb_Modules
SET
    ModuleOrder = @ModuleOrder,
    PaneName    = @PaneName
WHERE
    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateModuleSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleSetting]
GO

CREATE PROCEDURE rb_UpdateModuleSetting
(
    @ModuleID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_ModuleSettings 
    WHERE 
        ModuleID = @ModuleID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_ModuleSettings (
    ModuleID,
    SettingName,
    SettingValue
) 
VALUES (
    @ModuleID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_ModuleSettings
SET
    SettingValue = @SettingValue
WHERE
    ModuleID = @ModuleID
  AND
    SettingName = @SettingName
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdatePortalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdatePortalInfo]
GO

CREATE PROCEDURE rb_UpdatePortalInfo
(
    @PortalID           int,
    @PortalName         nvarchar(128),
    @PortalPath         nvarchar(128),
    @AlwaysShowEditButton bit 
)
AS
UPDATE
    rb_Portals
SET
    PortalName = @PortalName,
    PortalPath = @PortalPath,
    AlwaysShowEditButton = @AlwaysShowEditButton
WHERE
    PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdatePortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdatePortalSetting]
GO

CREATE PROCEDURE rb_UpdatePortalSetting
(
    @PortalID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_PortalSettings 
    WHERE 
        PortalID = @PortalID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_PortalSettings (
    PortalID,
    SettingName,
    SettingValue
) 
VALUES (
    @PortalID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_PortalSettings
SET
    SettingValue = @SettingValue
WHERE
    PortalID = @PortalID
  AND
    SettingName = @SettingName
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateRole]
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateTab]
GO

---------------------
--1.2.8.1522.sql
---------------------
--Update Stored PROCEDURE: rb_UpdateTab
--Prevents orphaning a tab or placing tabs in an infinte recursive loop
--26 dec 2002 - Cory Isakson
CREATE PROCEDURE rb_UpdateTab
(
    @PortalID        int,
    @TabID           int,
    @ParentTabID     int,
    @TabOrder        int,
    @TabName         nvarchar(50),
    @AuthorizedRoles nvarchar(256),
    @MobileTabName   nvarchar(50),
    @ShowMobile      bit
)
AS
IF (@ParentTabID = 0) 
    SET @ParentTabID = NULL
IF NOT EXISTS
(
    SELECT 
        * 
    FROM 
        rb_Tabs
    WHERE 
        TabID = @TabID
)
INSERT INTO rb_Tabs (
    PortalID,
    ParentTabID,
    TabOrder,
    TabName,
    AuthorizedRoles,
    MobileTabName,
    ShowMobile
) 
VALUES (
    @PortalID,
    @TabOrder,
    @ParentTabID,
    @TabName,
    @AuthorizedRoles,
    @MobileTabName,
    @ShowMobile
   
)
ELSE
--Updated 26.Dec.2002 Cory Isakson
--Check the Tab recursion so Tab is not placed into an infinate loop when building Tab Tree
BEGIN TRAN
--If the Update breaks the tab from having a path back to the root then do not Update
UPDATE
    rb_Tabs
SET
    ParentTabID = @ParentTabID,
    TabOrder = @TabOrder,
    TabName = @TabName,
    AuthorizedRoles = @AuthorizedRoles,
    MobileTabName = @MobileTabName,
    ShowMobile = @ShowMobile
WHERE
    TabID = @TabID
--Create a Temporary table to hold the tabs
CREATE TABLE #TabTree
(
    [TabID] [int],
    [TabName] [nvarchar] (50),
    [ParentTabID] [int],
    [TabOrder] [int],
    [NestLevel] [int],
    [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO	#TabTree
SELECT 	TabID,
    TabName,
    ParentTabID,
    TabOrder,
    0,
    cast(100000000 + TabOrder AS varchar)
FROM	rb_Tabs
WHERE	ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT 	#TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
        SELECT 	rb_Tabs.TabID,
            Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
            rb_Tabs.ParentTabID,
            rb_Tabs.TabOrder,
            @LastLevel,
            cast(#TabTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder AS varchar)
        FROM	rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
        WHERE	EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
         AND PortalID =@PortalID
        ORDER BY #TabTree.TabOrder
END
--Check that the Tab is found in the Tree.  If it is not then we abort the Update
IF NOT EXISTS (SELECT TabID from #TabTree WHERE TabID=@TabID)
BEGIN
    ROLLBACK TRAN
    --If we want to modify the TabLayout code then we can throw an error AND catch it.
    RAISERROR('Not allowed to choose that parent.',11,1)
END
ELSE
COMMIT TRAN
--End changes 26.Dec.2002 Cory Isakson
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateTabCustomSettings]
GO

CREATE PROCEDURE rb_UpdateTabCustomSettings
(
    @TabID int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_TabSettings 
    WHERE 
        TabID = @TabID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_TabSettings (
    TabID,
    SettingName,
    SettingValue
) 
VALUES (
    @TabID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_TabSettings
SET
    SettingValue = @SettingValue
WHERE
    TabID = @TabID
  AND
    SettingName = @SettingName
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateTabOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateTabOrder]
GO

CREATE PROCEDURE rb_UpdateTabOrder
(
    @TabID           int,
    @TabOrder        int
)
AS
UPDATE
    rb_Tabs
SET
    TabOrder = @TabOrder
WHERE
    TabID = @TabID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUserCheckEmail]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserCheckEmail]
GO

CREATE PROCEDURE rb_UpdateUserCheckEmail
(
    @UserID		    int,
    @CheckedEmail	tinyint
)
AS
UPDATE rb_Users
SET
    MailChecked = @CheckedEmail
WHERE UserID = @UserID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUserDeskTop]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserDeskTop]
GO

CREATE PROCEDURE rb_UpdateUserDeskTop
(
    @PortalID 		int,
    @UserID 		int,
    @ModuleID        	int,
    @TabID          	int,
    @WindowState 	smallint
)
AS
IF  (
    SELECT 
       count( *) 
    FROM 
       rb_UserDesktop(nolock)
    WHERE 
        UserID = @UserID AND  TabID = @TabID AND PortalID = @PortalID AND   ModuleID=@ModuleID
) = 0
BEGIN
    -- Transacted insert for download count
    BEGIN TRAN
    INSERT INTO rb_UserDesktop (
        UserID,
        ModuleID,
        TabID,
        PortalID,
        State
    ) 
    VALUES (
        @UserID,
        @ModuleID,
        @TabID,
        @PortalID,
        @WindowState
    
    )
    COMMIT TRAN
END
ELSE
BEGIN
    -- Transacted insert for download count
    BEGIN TRAN
    UPDATE
        rb_UserDesktop
    
    SET
        UserID=  @UserID,
        ModuleID=@ModuleID,
        TabID=@TabID,
        PortalID=@PortalID,
        State=@WindowState
    WHERE
            UserID = @UserID AND  TabID = @TabID AND PortalID = @PortalID AND   ModuleID=@ModuleID
     COMMIT TRAN
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFull]
GO

CREATE PROCEDURE rb_UpdateUserFull
(
    @UserID		    int,
    @OldUserID		int = @UserID,
    @PortalID       int,
    @Name		nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
    @CountryID		nchar(2),  
    @StateID		int
)
AS
-- 1774 fix
-- Get password if not set
If (len(@Password) <= 0)
    BEGIN
        SELECT @Password = (SELECT Password from rb_Users WHERE UserID = @OldUserID)
    END
    
    
DECLARE @err_message nvarchar(255)
    
-- Normal update
if (@OldUserID = @UserID)
    BEGIN
        UPDATE rb_Users

        SET
        PortalID = @PortalID,
        Name = @Name,
        Company = @Company,
        Address = @Address,		
        City = @City,		
        Zip = @Zip,		
        Phone = @Phone,		
        Fax = @Fax,		
        PIva = @PIva,		
        CFiscale = @CFiscale,	
        Email = @Email,		
        Password = @Password,
        SendNewsletter = @SendNewsletter,
        CountryID = @CountryID,
        StateID = @StateID

        WHERE UserID = @UserID
    END
ELSE
    BEGIN
        --if user found with same number return SEV 11 error to abort stored procedure
        IF EXISTS (SELECT UserID FROM rb_Users WHERE UserID = @UserID)
            BEGIN	
            SET @err_message = 'The number ' + Convert(varchar(20), @UserID) + ' is already in use!'
            RAISERROR (@err_message, 11,1)
            RETURN
            END
        
        -- Wrap on transaction
        BEGIN TRANSACTION

        -- Allow Identity change
        SET IDENTITY_INSERT rb_Users ON

        --Insert record as changed one
        INSERT INTO rb_Users
        (
            UserID,
            PortalID,
            Name,
            Company,
            Address,		
            City,		
            Zip,		
            Phone,		
            Fax,		
            PIva,		
            CFiscale,	
            Email,		
            Password,
            SendNewsletter,
            CountryID,
            StateID
        )

        VALUES
        (
            @UserID,
            @PortalID,
            @Name,
            @Company,
            @Address,	
            @City,	
            @Zip,	
            @Phone,	
            @Fax,	
            @PIva,	
            @CFiscale,
            @Email + CAST(RAND( (DATEPART(mm, GETDATE()) * 100000 ) + (DATEPART(ss, GETDATE()) * 1000 ) + DATEPART(ms, GETDATE()) ) AS VARCHAR(20)),
            @Password,
            @SendNewsletter,
            @CountryID,
            @StateID
        )
        
        -- Restore identity off
        SET IDENTITY_INSERT rb_Users OFF

        --Migrate Roles
                INSERT INTO rb_UserRoles (UserID, RoleID) (SELECT @UserID UID, rb_UserRoles.RoleID FROM rb_UserRoles WHERE rb_UserRoles.UserID = @OldUserID)

        -- Remove the old one
        DELETE rb_Users  WHERE UserID = @OldUserID
    
        -- Update user with correct email
        BEGIN
            UPDATE rb_Users
    
            SET
            PortalID = @PortalID,
            Name = @Name,
            Company = @Company,
            Address = @Address,		
            City = @City,		
            Zip = @Zip,		
            Phone = @Phone,		
            Fax = @Fax,		
            PIva = @PIva,		
            CFiscale = @CFiscale,	
            Email = @Email,		
            Password = @Password,
            SendNewsletter = @SendNewsletter,
            CountryID = @CountryID,
            StateID = @StateID
    
            WHERE UserID = @UserID
        END

        COMMIT
    END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteUserDeskTop]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteUserDeskTop]
GO

/* deletes all  user desktop values for the specified user */
CREATE PROCEDURE rb_DeleteUserDeskTop
(
    @UserID       int,
    @PortalID    int
)
AS

DELETE 
  
FROM
    rb_UserDesktop

WHERE   
    UserID = @UserID AND PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUserFullNoPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFullNoPassword]
GO

CREATE PROCEDURE rb_UpdateUserFullNoPassword
(
    @UserID		    int,
    @PortalID       int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @SendNewsletter	bit,
    @CountryID	nchar(2),  
    @StateID		int
)
AS
UPDATE rb_Users
SET
PortalID = @PortalID,
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
Email = @Email,		
SendNewsletter = @SendNewsletter,
CountryID = @CountryID,
StateID = @StateID
WHERE UserID = @UserID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLogin]
GO

CREATE PROCEDURE rb_UserLogin
(
    @PortalID int,
    @Email    nvarchar(100),
    @Password nvarchar(20)
)
AS
SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email
FROM   rb_Users
WHERE  (Email = @Email) AND (Password = @Password) AND (PortalID = @PortalID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserLoginByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLoginByID]
GO

CREATE PROCEDURE rb_UserLoginByID
(
    @PortalID int,
    @UserID   int,
    @Password nvarchar(20)
)
AS
--by Cemil on 24/05/2004
--Updated version of rb_UserLoginByID to change the UserID type from nvarchar to int
SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email
FROM   rb_Users
WHERE
    rb_Users.UserID = @UserID AND rb_Users.Password = @Password AND rb_Users.PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUser]
GO

CREATE PROCEDURE rb_UpdateUser
(
    @PortalID		int,
    @UserID         int,
    @Name			nvarchar(50),
    @Email          nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter bit
)
AS

IF (len(@Password)>0)
begin
UPDATE
    rb_Users

SET
    Name	 = @Name,
    Email    = @Email,
    Password = @Password,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID
end
else
begin
UPDATE
    rb_Users

SET
    Name	 = @Name,
    Email    = @Email,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID
end
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ModulesUpgradeOldToNew]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModulesUpgradeOldToNew]
GO

CREATE PROCEDURE rb_ModulesUpgradeOldToNew
(
    @OldModuleGuid uniqueidentifier,
    @NewModuleGuid uniqueidentifier
)
/* 
This procedure replaces all instances of the old module with the new one
Old module entires will be removed.
Module data are not affected.
by Manu 22/03/2002
*/ 

AS

--Get current ids
DECLARE @NewModuleDefID int
SET @NewModuleDefID = (SELECT TOP 1 ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @NewModuleGuid)
DECLARE @OldModuleDefID int
SET @OldModuleDefID = (SELECT TOP 1 ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @OldModuleGuid)

--Update modules
UPDATE Modules
SET ModuleDefID = @NewModuleDefID WHERE ModuleDefID = @OldModuleDefID

-- Drop old definition
DELETE ModuleDefinitions
WHERE ModuleDefID = @OldModuleDefID

DELETE GeneralModuleDefinitions
WHERE GeneralModDefID = @OldModuleGuid
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_SetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_SetLastModified]
GO

CREATE PROCEDURE rb_SetLastModified
    (
        @ModuleID int,
        @LastModifiedBy	nvarchar(256)
    )
AS

    -- Check if this module supports workflow
    DECLARE @support	bit

    SELECT @support = SupportWorkflow
    FROM rb_Modules
    WHERE ModuleID = @ModuleID

    IF ( @support = 1 )
    BEGIN
        -- It supports workflow
        UPDATE rb_Modules
        SET StagingLastModified = getdate(),
                    StagingLastEditor = @LastModifiedBy
        WHERE ModuleID = @ModuleID
    END
    ELSE
        -- It doesn't support workflow
        UPDATE rb_Modules
        SET LastModified = getdate(),
                    LastEditor = @LastModifiedBy,
            StagingLastModified = getdate(),
            StagingLastEditor = @LastModifiedBy
        WHERE ModuleID = @ModuleID

    /* SET NOCOUNT ON */
    RETURN
GO

--PORTALS
IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Portals_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Portals_IU]
GO

Create Proc rb_Portals_IU 	@PortalID	int,  	@PortalAlias	nvarchar(128),  	@PortalName	nvarchar(128),  	@PortalPath	nvarchar(128),  	@AlwaysShowEditButton	bit  AS  SET nocount ON  	UPDATE "rb_Portals" 	SET PortalAlias = @PortalAlias,  		PortalName = @PortalName,  		PortalPath = @PortalPath,  		AlwaysShowEditButton = @AlwaysShowEditButton 	WHERE PortalID	=	@PortalID  	IF @@rowcount = 0 	BEGIN 	SET IDENTITY_INSERT "rb_Portals" ON 	INSERT "rb_Portals" (PortalID,  		PortalAlias,  		PortalName,  		PortalPath,  		AlwaysShowEditButton) 	VALUES (@PortalID,  		@PortalAlias,  		@PortalName,  		@PortalPath,  		@AlwaysShowEditButton) 	SET IDENTITY_INSERT "rb_Portals" OFF 	END  Return 
GO

--	@PortalID	,	@PortalAlias	,	@PortalName	,	@PortalPath	,	@AlwaysShowEditButton
exec rb_Portals_IU 	-1	,	N'unused'	,	N'Unused Portal'	,	NULL	,	0
exec rb_Portals_IU 	0	,	N'Appleseed'	,	N'Appleseed Portal'	,	N'/_Appleseed'	,	0
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Portals_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Portals_IU]
GO
--End insert portals

--SOLUTIONS
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Solutions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Solutions] (
    [SolutionsID] [int] IDENTITY (1, 1) NOT NULL ,
    [SolDescription] [nvarchar] (100) COLLATE Latin1_General_CI_AS NOT NULL ,
    CONSTRAINT [PK_rb_Solutions] PRIMARY KEY  CLUSTERED 
    (
        [SolutionsID]
    )
)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Solutions_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Solutions_IU]
GO

Create Proc rb_Solutions_IU 	@SolutionsID	int,  	@SolDescription	nvarchar(100)  AS  SET nocount ON  	UPDATE "rb_Solutions" 	SET SolDescription = @SolDescription 	WHERE SolutionsID	=	@SolutionsID  	IF @@rowcount = 0 	BEGIN 	SET IDENTITY_INSERT "rb_Solutions" ON 	INSERT "rb_Solutions" (SolutionsID,  		SolDescription) 	VALUES (@SolutionsID,  		@SolDescription) 	SET IDENTITY_INSERT "rb_Solutions" OFF 	END  Return 
GO

--	@SolutionsID	,	@SolDescription

exec rb_Solutions_IU 	1	,	N'Appleseed'
GO

--1 rows scripted

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Solutions_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Solutions_IU]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSolutionModuleDefinitions]
GO

/* returns all module definitions for a specified solution */
CREATE PROCEDURE rb_GetSolutionModuleDefinitions
(
    @SolutionID  int
)
AS
SELECT *
FROM
    rb_SolutionModuleDefinitions
WHERE   
    SolutionsID = @SolutionID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSolutions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSolutions]
GO

CREATE PROCEDURE rb_GetSolutions
AS
SELECT * FROM rb_Solutions
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_SolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_SolutionModuleDefinitions] (
    [SolutionModDefID] [int] IDENTITY (1, 1) NOT NULL ,
    [GeneralModDefID] [uniqueidentifier] NOT NULL ,
    [SolutionsID] [int] NOT NULL ,
    CONSTRAINT [PK_rb_SolutionModuleDefintions] PRIMARY KEY  CLUSTERED 
    (
        [SolutionModDefID]
    ),
    CONSTRAINT [FK_rb_SolutionModuleDefinitions_rb_GeneralModuleDefinitions] FOREIGN KEY 
    (
        [GeneralModDefID]
    ) REFERENCES [rb_GeneralModuleDefinitions] (
        [GeneralModDefID]
    ),
    CONSTRAINT [FK_rb_SolutionModuleDefintions_rb_Solutions] FOREIGN KEY 
    (
        [SolutionsID]
    ) REFERENCES [rb_Solutions] (
        [SolutionsID]
    )
)
END
GO


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

--From1772
/* Add AuthorizedMoveModuleRoles & AuthorizedDeleteModuleRoles in table rb_Modules */
IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedMoveModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
    [AuthorizedMoveModuleRoles] [nvarchar] (256) NULL 
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedDeleteModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
    [AuthorizedDeleteModuleRoles] [nvarchar] (256) NULL 
END
GO

/* Checks to see if rb_GetAuthDeleteModuleRoles already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthDeleteModuleRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthDeleteModuleRoles]
GO

CREATE PROCEDURE rb_GetAuthDeleteModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @DeleteModuleRoles  = rb_Modules.AuthorizedDeleteModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO

/* Checks to see if rb_GetAuthMoveModuleRoles already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthMoveModuleRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthMoveModuleRoles]
GO

CREATE PROCEDURE rb_GetAuthMoveModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @MoveModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @MoveModuleRoles  = rb_Modules.AuthorizedMoveModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID
GO

--from 1773
/* Checks to see if rb_GetModulesInTab already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetModulesInTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModulesInTab]
GO

CREATE PROCEDURE rb_GetModulesInTab
(
 @PortalID int,
 @TabID int
)
AS
SELECT rb_Modules.ModuleID, rb_ModuleDefinitions.GeneralModDefID
FROM rb_Modules INNER JOIN
     rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
     rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE (rb_Modules.TabID = @TabID) AND (rb_ModuleDefinitions.PortalID = @PortalID)
GO

/* Set the correct roles properties to existing modules */
UPDATE rb_Modules
   SET AuthorizedMoveModuleRoles = 'Admins;'
   WHERE AuthorizedMoveModuleRoles is null
go

UPDATE rb_Modules
   SET AuthorizedDeleteModuleRoles = 'Admins;'
   WHERE AuthorizedDeleteModuleRoles is null
go
