---------------------
--1.2.8.1509.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1509','1.2.8.1509', CONVERT(datetime, '03/09/2003', 101))
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCurrentDbVersion]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentDbVersion]
GO

CREATE PROCEDURE [rb_GetCurrentDbVersion]
AS
SELECT     TOP 1 Release
FROM         rb_Versions
ORDER BY Release DESC
GO

ALTER TABLE TabSettings ADD CONSTRAINT
	FK_TabSettings_Tabs FOREIGN KEY
	(
	TabID
	) REFERENCES Tabs
	(
	TabID
	)
GO

BEGIN TRANSACTION
ALTER TABLE Tabs
	DROP CONSTRAINT FK_Tabs_Layouts

DROP TABLE Layouts
COMMIT
GO

ALTER TABLE TabSettings ADD CONSTRAINT
	PK_TabSettings PRIMARY KEY CLUSTERED 
	(
	TabID,
	SettingName
	) ON [PRIMARY]
GO

ALTER TABLE ModuleSettings ADD CONSTRAINT
	PK_ModuleSettings PRIMARY KEY CLUSTERED 
	(
	ModuleID,
	SettingName
	) ON [PRIMARY]
GO

--GeneralModulesDefinition
BEGIN TRANSACTION
--ALTER TABLE GeneralModuleDefinitions
--	DROP CONSTRAINT DF__GeneralMo__Searc__40C49C62

--ALTER TABLE GeneralModuleDefinitions
--	DROP CONSTRAINT DF__GeneralMo__Assem__41B8C09B

CREATE TABLE Tmp_GeneralModuleDefinitions
	(
	GeneralModDefID uniqueidentifier NOT NULL ROWGUIDCOL,
	FriendlyName nvarchar(128) NOT NULL,
	DesktopSrc nvarchar(256) NOT NULL,
	MobileSrc nvarchar(256) NOT NULL,
	AssemblyName varchar(50) NOT NULL,
	ClassName nvarchar(128) NULL,
	Admin bit NULL,
	Searchable bit NULL
	)  ON [PRIMARY]

ALTER TABLE Tmp_GeneralModuleDefinitions ADD CONSTRAINT
	DF__GeneralMo__Assem__41B8C09B DEFAULT ('Appleseed') FOR AssemblyName

ALTER TABLE Tmp_GeneralModuleDefinitions ADD CONSTRAINT
	DF_GeneralModuleDefinitions_Admin1 DEFAULT (0) FOR Admin

ALTER TABLE Tmp_GeneralModuleDefinitions ADD CONSTRAINT
	DF_GeneralModuleDefinitions_Searchable1 DEFAULT (0) FOR Searchable

IF EXISTS(SELECT * FROM GeneralModuleDefinitions)
	 EXEC('INSERT INTO Tmp_GeneralModuleDefinitions (GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc, AssemblyName, ClassName, Admin, Searchable)
		SELECT GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc, AssemblyName, ClassName, Admin, Searchable FROM GeneralModuleDefinitions TABLOCKX')

ALTER TABLE ModuleDefinitions
	DROP CONSTRAINT FK_ModuleDefinitions_GeneralModuleDefinitions

ALTER TABLE SolutionModuleDefinitions
	DROP CONSTRAINT FK_SolutionModuleDefinitions_GeneralModuleDefinitions

DROP TABLE GeneralModuleDefinitions

EXECUTE sp_rename N'Tmp_GeneralModuleDefinitions', N'GeneralModuleDefinitions', 'OBJECT'

ALTER TABLE GeneralModuleDefinitions ADD CONSTRAINT
	PK_GeneralModuleDefinitions PRIMARY KEY CLUSTERED 
	(
	GeneralModDefID
	) ON [PRIMARY]

ALTER TABLE GeneralModuleDefinitions ADD CONSTRAINT
	IX_GeneralModuleDefinitions UNIQUE NONCLUSTERED 
	(
	FriendlyName
	) ON [PRIMARY]

COMMIT
GO

ALTER TABLE SolutionModuleDefinitions WITH NOCHECK ADD CONSTRAINT
	FK_SolutionModuleDefinitions_GeneralModuleDefinitions FOREIGN KEY
	(
	GeneralModDefID
	) REFERENCES GeneralModuleDefinitions
	(
	GeneralModDefID
	)
GO

ALTER TABLE ModuleDefinitions WITH NOCHECK ADD CONSTRAINT
	FK_ModuleDefinitions_GeneralModuleDefinitions FOREIGN KEY
	(
	GeneralModDefID
	) REFERENCES GeneralModuleDefinitions
	(
	GeneralModDefID
	) ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'AddModuleDefinition') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE AddModuleDefinition
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'UpdateModuleDefinition') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE UpdateModuleDefinition
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'AddGeneralModuleDefinitions') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE AddGeneralModuleDefinitions
GO

/* PROCEDURE AddGeneralModuleDefinitions*/
CREATE PROCEDURE AddGeneralModuleDefinitions
	@GeneralModDefID uniqueidentifier,
	@FriendlyName nvarchar(128),
	@DesktopSrc nvarchar(256),
	@MobileSrc nvarchar(256),
	@AssemblyName varchar(50),
	@ClassName nvarchar(128),
	@Admin bit,
	@Searchable bit
AS
INSERT INTO GeneralModuleDefinitions
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'UpdateGeneralModuleDefinitions') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE UpdateGeneralModuleDefinitions
GO

/* PROCEDURE UpdateGeneralModuleDefinitions*/
CREATE PROCEDURE UpdateGeneralModuleDefinitions
	@GeneralModDefID uniqueidentifier,
	@FriendlyName nvarchar(128),
	@DesktopSrc nvarchar(256),
	@MobileSrc nvarchar(256),
	@AssemblyName varchar(50),
	@ClassName nvarchar(128),
	@Admin bit,
	@Searchable bit
AS
UPDATE GeneralModuleDefinitions
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

--UPDATE GeneralModuleDefinitions SET FriendlyName='Announcements', DesktopSrc='DesktopModules/Announcements.ascx', MobileSrc='MobileModules/Announcements.ascx', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesAnnouncements', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{CE55A821-2449-4903-BA1A-EC16DB93F8DB}'
--UPDATE GeneralModuleDefinitions SET FriendlyName='Articles', DesktopSrc='DesktopModules/Articles.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesArticles', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}'
--UPDATE GeneralModuleDefinitions SET FriendlyName='Contacts', DesktopSrc='DesktopModules/Contacts.ascx', MobileSrc='MobileModules/Contacts.ascx', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesContacts', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E5339EF}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Discussion', DesktopSrc='DesktopModules/Discussion/Discussion.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesDiscussion', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{2D86166C-4BDC-4A6F-A028-D17C2BB177C8}'
--UPDATE GeneralModuleDefinitions SET FriendlyName='Documents', DesktopSrc='DesktopModules/Document.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesDocuments', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{F9645B82-CB45-4C4C-BB2D-72FA42FE2B75}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Events', DesktopSrc='DesktopModules/Events.ascx', MobileSrc='MobileModules/Events.ascx', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesEvents', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{EF9B29C5-E481-49A6-9383-8ED3AB42DDA0}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Html Document', DesktopSrc='DesktopModules/HtmlModule.ascx', MobileSrc='MobileModules/Text.ascx', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesHtmlModule', Admin='0', Searchable='1'  WHERE GeneralModDefID = '{0B113F51-FEA3-499A-98E7-7B83C192FDBB}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Html Editor WYSIWYG', DesktopSrc='DesktopModules/HtmlModuleV2.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesHtmlModuleV2', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{2B113F51-FEA3-499A-98E7-7B83C192FDBB}'
UPDATE GeneralModuleDefinitions SET FriendlyName='IframeModule', DesktopSrc='DesktopModules/IframeModule.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesIframeModule', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531005}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Links', DesktopSrc='DesktopModules/Links.ascx', MobileSrc='MobileModules/Links.ascx', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesLinks', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{476CF1CC-8364-479D-9764-4B3ABD7FFABD}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Manage Portals (AdminAll)', DesktopSrc='AdminAll/Portals.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.Portals', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{366C247D-4CFB-451D-A7AE-649C83B05841}'
--UPDATE GeneralModuleDefinitions SET FriendlyName='Milestones', DesktopSrc='DesktopModules/Milestones.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesMilestones', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{B8784E32-688A-4B8A-87C4-DF108BF12DBE}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Newsletter', DesktopSrc='DesktopModules/SendNewsletter.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesSendNewsletter', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{B484D450-5D30-4C4B-817C-14A25D06577E}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Pictures', DesktopSrc='DesktopModules/Pictures.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesPictures', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{B29CB86B-AEA1-4E94-8B77-B4E4239258B0}'
--UPDATE GeneralModuleDefinitions SET FriendlyName='PortalSearch', DesktopSrc='DesktopModules/PortalSearch.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesPortalSearch', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531030}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Shortcut', DesktopSrc='DesktopModules/Shortcut.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesShortcut', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD2}'
UPDATE GeneralModuleDefinitions SET FriendlyName='ShortcutAll', DesktopSrc='DesktopModules/ShortcutAll.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesShortcutAll', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD0}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Signin', DesktopSrc='DesktopModules/Signin.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesSignin', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{A0F1F62B-FDC7-4DE5-BBAD-A5DAF31D960A}'
UPDATE GeneralModuleDefinitions SET FriendlyName='XML/XSL', DesktopSrc='DesktopModules/XmlModule.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesXmlModule', Admin='0', Searchable='0'  WHERE GeneralModDefID = '{BE224332-03DE-42B7-B127-AE1F1BD0FADC}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Blacklist (Admin)', DesktopSrc='Admin/Blacklist.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Content.Web.ModulesBlacklist', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531017}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Manage Users (Admin)', DesktopSrc='Admin/Users.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.Users', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{B6A48596-9047-4564-8555-61E3B31D7272}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Module Definitions (Admin)', DesktopSrc='Admin/ModuleDefs.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.ModuleDefs', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Module Types (AdminAll)', DesktopSrc='AdminAll/ModuleDefsAll.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.ModuleDefsAll', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{D04BB5EA-A792-4E87-BFC7-7D0ED3ADD582}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Roles (Admin)', DesktopSrc='Admin/Roles.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.Roles', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{A406A674-76EB-4BC1-BB35-50CD2C251F9C}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Site Settings (Admin)', DesktopSrc='Admin/SiteSettings.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.SiteSettings', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{EBBB01B1-FBB5-4E79-8FC4-59BCA1D0554E}'
UPDATE GeneralModuleDefinitions SET FriendlyName='Tabs (Admin)', DesktopSrc='Admin/Tabs.ascx', MobileSrc='', AssemblyName='Appleseed.DLL', ClassName='Appleseed.Admin.Tabs', Admin='1', Searchable='0'  WHERE GeneralModDefID = '{1C575D94-70FC-4A83-80C3-2087F726CBB3}'
GO

--Milestones

BEGIN TRANSACTION

if NOT exists (SELECT * FROM sysobjects WHERE id = object_id(N'[Milestones]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [Milestones] (
	[ItemID] [int] NOT NULL ,
	[ModuleID] [int] NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[EstCompleteDate] [datetime] NULL ,
	[Status] [nvarchar] (100) NULL 
) ON [PRIMARY]

ALTER TABLE Milestones ADD CONSTRAINT
	FK_Milestones_Modules FOREIGN KEY
	(
	ModuleID
	) REFERENCES Modules
	(
	ModuleID
	)

ALTER TABLE Milestones
	DROP CONSTRAINT FK_Milestones_Modules

CREATE TABLE Tmp_Milestones
	(
	ItemID int NOT NULL IDENTITY (1, 1),
	ModuleID int NULL,
	CreatedByUser nvarchar(100) NULL,
	CreatedDate datetime NULL,
	Title nvarchar(100) NULL,
	EstCompleteDate datetime NULL,
	Status nvarchar(100) NULL
	)  ON [PRIMARY]

SET IDENTITY_INSERT Tmp_Milestones ON
IF EXISTS(SELECT * FROM Milestones)
	 EXEC('INSERT INTO Tmp_Milestones (ItemID, ModuleID, CreatedByUser, CreatedDate, Title, EstCompleteDate, Status)
		SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, Title, EstCompleteDate, Status FROM Milestones TABLOCKX')
SET IDENTITY_INSERT Tmp_Milestones OFF

DROP TABLE Milestones

EXECUTE sp_rename N'Tmp_Milestones', N'Milestones', 'OBJECT'

ALTER TABLE Milestones ADD CONSTRAINT
	PK_Milestones PRIMARY KEY CLUSTERED 
	(
	ItemID
	) ON [PRIMARY]


ALTER TABLE Milestones WITH NOCHECK ADD CONSTRAINT
	FK_Milestones_Modules FOREIGN KEY
	(
	ModuleID
	) REFERENCES Modules
	(
	ModuleID
	)
COMMIT
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'GetSingleMilestones') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE GetSingleMilestones
GO

/* PROCEDURE GetSingleMilestones*/
CREATE PROCEDURE GetSingleMilestones
@ItemID int
AS
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	Milestones
WHERE
	ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'GetMilestones') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE GetMilestones
GO

/* PROCEDURE GetMilestones*/
CREATE PROCEDURE GetMilestones
@ModuleID int
AS
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	Milestones
WHERE
	ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'DeleteMilestones') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE DeleteMilestones
GO

/* PROCEDURE DeleteMilestones*/
CREATE PROCEDURE DeleteMilestones
@ItemID int
AS
DELETE
FROM
	Milestones
WHERE
	ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'AddMilestones') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE AddMilestones
GO

/* PROCEDURE AddMilestones*/
CREATE PROCEDURE AddMilestones
	@ItemID int OUTPUT,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
INSERT INTO Milestones
(
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
)
VALUES
(
	@ModuleID,
	@CreatedByUser,
	@CreatedDate,
	@Title,
	@EstCompleteDate,
	@Status
)
SELECT
	@ItemID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'UpdateMilestones') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE UpdateMilestones
GO

/* PROCEDURE UpdateMilestones*/
CREATE PROCEDURE UpdateMilestones
	@ItemID int,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
UPDATE Milestones
SET
	ModuleID = @ModuleID,
	CreatedByUser = @CreatedByUser,
	CreatedDate = @CreatedDate,
	Title = @Title,
	EstCompleteDate = @EstCompleteDate,
	Status = @Status
WHERE
	ItemID = @ItemID
GO

