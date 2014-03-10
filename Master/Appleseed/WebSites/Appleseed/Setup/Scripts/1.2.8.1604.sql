---------------------
--1.2.8.1604.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1604','1.2.8.1604', CONVERT(datetime, '04/05/2003', 101))
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_Announcements_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Announcements] DROP CONSTRAINT FK_rb_st_Announcements_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Announcements_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Announcements] DROP CONSTRAINT FK_st_rb_Announcements_rb_Modules
GO

ALTER TABLE [st_rb_Announcements] ADD 
	CONSTRAINT [FK_st_rb_Announcements_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Announcements] DROP CONSTRAINT PK_rb_st_Announcements
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Announcements] DROP CONSTRAINT PK_st_Announcements
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Announcements] DROP CONSTRAINT PK_st_rb_Announcements
GO

ALTER TABLE [st_rb_Announcements] ADD 
	CONSTRAINT [PK_st_rb_Announcements] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_Contacts_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Contacts] DROP CONSTRAINT FK_rb_st_Contacts_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Contacts_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Contacts] DROP CONSTRAINT FK_st_rb_Contacts_rb_Modules
GO

ALTER TABLE [st_rb_Contacts] ADD 
	CONSTRAINT [FK_st_rb_Contacts_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Contacts] DROP CONSTRAINT PK_rb_st_Contacts
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Contacts] DROP CONSTRAINT PK_st_rb_Contacts
GO

ALTER TABLE [st_rb_Contacts] ADD 
	CONSTRAINT [PK_st_rb_Contacts] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_Documents_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Documents] DROP CONSTRAINT FK_rb_st_Documents_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Documents_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Documents] DROP CONSTRAINT FK_st_rb_Documents_rb_Modules
GO

ALTER TABLE [st_rb_Documents] ADD 
	CONSTRAINT [FK_st_rb_Documents_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Documents] DROP CONSTRAINT PK_rb_st_Documents
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Documents] DROP CONSTRAINT PK_st_rb_Documents
GO

ALTER TABLE [st_rb_Documents] ADD 
	CONSTRAINT [PK_st_rb_Documents] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_Events_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Events] DROP CONSTRAINT FK_rb_st_Events_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Events_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Events] DROP CONSTRAINT FK_st_rb_Events_rb_Modules
GO

ALTER TABLE [st_rb_Events] ADD 
	CONSTRAINT [FK_st_rb_Events_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Events] DROP CONSTRAINT PK_rb_st_Events
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Events] DROP CONSTRAINT PK_st_rb_Events
GO

ALTER TABLE [st_rb_Events] ADD 
	CONSTRAINT [PK_st_rb_Events] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_HtmlText_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_HtmlText] DROP CONSTRAINT FK_rb_st_HtmlText_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_HtmlText_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_HtmlText] DROP CONSTRAINT FK_st_rb_HtmlText_rb_Modules
GO

ALTER TABLE [st_rb_HtmlText] ADD 
	CONSTRAINT [FK_st_rb_HtmlText_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_HtmlText] DROP CONSTRAINT PK_rb_st_HtmlText
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_HtmlText] DROP CONSTRAINT PK_st_rb_HtmlText
GO

ALTER TABLE [st_rb_HtmlText] ADD 
	CONSTRAINT [PK_st_rb_HtmlText] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_st_Links_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Links] DROP CONSTRAINT FK_rb_st_Links_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Links_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [st_rb_Links] DROP CONSTRAINT FK_st_rb_Links_rb_Modules
GO

ALTER TABLE [st_rb_Links] ADD 
	CONSTRAINT [FK_st_rb_Links_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_st_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Links] DROP CONSTRAINT PK_rb_st_Links
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [st_rb_Links] DROP CONSTRAINT PK_st_rb_Links
GO

ALTER TABLE [st_rb_Links] ADD 
	CONSTRAINT [PK_st_rb_Links] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_ContactsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_ContactsModified]
GO

CREATE  TRIGGER [rb_st_ContactsModified]
ON [st_rb_Contacts]
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateDocument]
GO

CREATE PROCEDURE rb_UpdateDocument
(
    @ItemID           int,
    @ModuleID         int,
    @FileFriendlyName nvarchar(150),
    @FileNameUrl      nvarchar(250),
    @UserName         nvarchar(100),
    @Category         nvarchar(50),
    @Content          image,
    @ContentType      nvarchar(50),
    @ContentSize      int
)
AS
IF (@ItemID=0) OR NOT EXISTS (
    SELECT 
        * 
    FROM 
        st_rb_Documents 
    WHERE 
        ItemID = @ItemID
)
INSERT INTO st_rb_Documents
(
    ModuleID,
    FileFriendlyName,
    FileNameUrl,
    CreatedByUser,
    CreatedDate,
    Category,
    Content,
    ContentType,
    ContentSize
)

VALUES
(
    @ModuleID,
    @FileFriendlyName,
    @FileNameUrl,
    @UserName,
    GetDate(),
    @Category,
    @Content,
    @ContentType,
    @ContentSize
)
ELSE

BEGIN

IF (@ContentSize=0)

UPDATE 
    st_rb_Documents

SET 
    CreatedByUser    = @UserName,
    CreatedDate      = GetDate(),
    Category         = @Category,
    FileFriendlyName = @FileFriendlyName,
    FileNameUrl      = @FileNameUrl

WHERE
    ItemID = @ItemID
ELSE

UPDATE
    st_rb_Documents

SET
    CreatedByUser     = @UserName,
    CreatedDate       = GetDate(),
    Category          = @Category,
    FileFriendlyName  = @FileFriendlyName,
    FileNameUrl       = @FileNameUrl,
    Content           = @Content,
    ContentType       = @ContentType,
    ContentSize       = @ContentSize

WHERE
    ItemID = @ItemID

END
GO




