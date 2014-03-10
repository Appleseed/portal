---------------------
--1.2.8.1613.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Milestones_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Milestones_st] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[EstCompleteDate] [datetime] NULL ,
	[Status] [nvarchar] (100) NULL 
) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Milestones_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1 AND (deltrig <> 0 OR instrig <> 0 OR updtrig <> 0))
DROP TRIGGER [rb_Milestones_stModified]
GO

CREATE TRIGGER [rb_Milestones_stModified]
ON [rb_Milestones_st]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC rb_ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetMilestones]
GO

/* PROCEDURE rb_GetMilestones*/
CREATE PROCEDURE rb_GetMilestones
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones
WHERE
	ModuleID = @ModuleID
	ORDER by CreatedDate DESC
ELSE
	SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones_st
WHERE
	ModuleID = @ModuleID
	ORDER by CreatedDate DESC
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleMilestones]
GO

/* PROCEDURE rb_GetSingleMilestones*/
CREATE PROCEDURE rb_GetSingleMilestones
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones
	WHERE
	    ItemID = @ItemID
ELSE
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones_st
	WHERE
	    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateMilestones]
GO

/* PROCEDURE rb_UpdateMilestones*/
CREATE PROCEDURE rb_UpdateMilestones
	@ItemID int,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
UPDATE rb_Milestones_st
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddMilestones]
GO

/* PROCEDURE rb_AddMilestones*/
CREATE PROCEDURE rb_AddMilestones
	@ItemID int OUTPUT,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
INSERT INTO rb_Milestones_st
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetAnnouncements]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetAnnouncements]
GO

CREATE   PROCEDURE rb_GetAnnouncements
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM 
	    rb_Announcements
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
	ORDER by CreatedDate DESC
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM 
	    rb_Announcements_st
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
	ORDER by CreatedDate DESC
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddGeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
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

IF exists (SELECT * FROM rb_GeneralModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID)
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


---- Installs Flash module
---- this is the recommended way for install new modules
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{623EC4DD-BA40-421c-887D-D774ED8EBF02}'
--SET @FriendlyName = 'Flash Module'
--SET @DesktopSrc = 'DesktopModules/FlashModule/FlashModule.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = ''
--SET @Admin = 0
--SET @Searchable = 0
---- Installs module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

---- Install it for default portal
--EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
--GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_PicturesModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_PicturesModified_st]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_PicturesModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Pictures_stModified]
GO

CREATE  TRIGGER [rb_Pictures_stModified]
ON rb_Pictures_st
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC rb_ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules

END
-- Insert the trigger for the workflow
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ContactsModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_ContactsModified_st]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ContactsModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Contacts_stModified]
GO

CREATE  TRIGGER [rb_Contacts_stModified]
ON [rb_Contacts_st]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC rb_ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules

END
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1613','1.2.8.1613', CONVERT(datetime, '04/13/2003', 101))
GO
