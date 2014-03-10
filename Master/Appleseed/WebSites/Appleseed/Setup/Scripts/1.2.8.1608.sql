---------------------
--1.2.8.1608.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1608','1.2.8.1608', CONVERT(datetime, '04/08/2003', 101))
GO

/****** Object:  Table [st_rb_Announcements]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_Announcements', 'rb_Announcements_st'
GO

/****** Object:  Table [st_rb_Contacts]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_Contacts', 'rb_Contacts_st'
GO

/****** Object:  Table [st_rb_Documents]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_Documents', 'rb_Documents_st'
GO

/****** Object:  Table [st_rb_Events]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_Events', 'rb_Events_st'
GO

/****** Object:  Table [st_rb_HtmlText]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_HtmlText', 'rb_HtmlText_st'
GO

/****** Object:  Table [st_rb_Links]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_rb_Links', 'rb_Links_st'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- Alter Publish stored procedure
ALTER   PROCEDURE rb_Publish
	@ModuleID	int
AS
	-- First get al list of tables which are related to the Modules table

	-- Create a temporary table
	CREATE TABLE #TMPResults
		(ForeignKeyTableSchema	nvarchar(128),
                 ForeignKeyTable	nvarchar(128),
		 ForeignKeyColumn	nvarchar(128),
		 KeyColumn		nvarchar(128),
		 ForeignKeyTableId	int,
		 KeyTableId		int,
		 KeyTableSchema		nvarchar(128),
		 KeyTable		nvarchar(128))

	INSERT INTO #TMPResults EXEC rb_GetRelatedTables 'rb_Modules'

	DECLARE RelatedTablesModules CURSOR FOR
		SELECT 	
			ForeignKeyTableSchema, 
			ForeignKeyTable,
			ForeignKeyColumn,
			KeyColumn,
			ForeignKeyTableId,
			KeyTableId,
			KeyTableSchema,
			KeyTable
		FROM
			#TMPResults
		WHERE 
			ForeignKeyTable <> 'ModuleSettings'

	-- Create temporary table for later use
	CREATE TABLE #TMPCount
		(ResultCount	int)


	-- Now search the table that has the related column
	OPEN RelatedTablesModules

	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
	DECLARE @ForeignKeyTable	nvarchar(128)
	DECLARE @ForeignKeyColumn	nvarchar(128)
	DECLARE @KeyColumn		nvarchar(128)
	DECLARE @ForeignKeyTableId	int
	DECLARE @KeyTableId		int
	DECLARE @KeyTableSchema		nvarchar(128)
	DECLARE @KeyTable		nvarchar(128)
	DECLARE @SQLStatement		nvarchar(4000)
	DECLARE @Count			int
	DECLARE @TableHasIdentityCol	int

	FETCH NEXT FROM RelatedTablesModules 
	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
		@KeyTableSchema, @KeyTable
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Check if this table has a corresponding staging table
		TRUNCATE TABLE #TMPCount
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[' + @ForeignKeyTable + '_st]'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
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
				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
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
					
					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
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
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
								IF (@ColIsIdentity = 0)
									SET @ColumnList = @ColumnList + ', '		
							END
							ELSE
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
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
			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
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

ALTER   PROCEDURE rb_UpdateHtmlText
(
    @ModuleID      int,
    @DesktopHtml   ntext,
    @MobileSummary ntext,
    @MobileDetails ntext
)
AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_HtmlText_st 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO rb_HtmlText_st (
    ModuleID,
    DesktopHtml,
    MobileSummary,
    MobileDetails
) 
VALUES (
    @ModuleID,
    @DesktopHtml,
    @MobileSummary,
    @MobileDetails
)
ELSE
UPDATE
    rb_HtmlText_st

SET
    DesktopHtml   = @DesktopHtml,
    MobileSummary = @MobileSummary,
    MobileDetails = @MobileDetails

WHERE
    ModuleID = @ModuleID
GO


ALTER   PROCEDURE rb_GetHtmlText
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT *
	FROM
	    rb_HtmlText
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT *
	FROM
	    rb_HtmlText_st
	WHERE
	    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_HtmlTextModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_HtmlTextModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_HtmlText_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_HtmlText_stModified]
GO

CREATE    TRIGGER [rb_HtmlText_stModified]
ON [rb_HtmlText_st]
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

ALTER   PROCEDURE rb_AddAnnouncement
(
    @ModuleID       int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     DateTime,
    @Description    nvarchar(2000),
    @ItemID         int OUTPUT
)
AS

INSERT INTO rb_Announcements_st
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    MoreLink,
    MobileMoreLink,
    ExpireDate,
    Description
)

VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @MoreLink,
    @MobileMoreLink,
    @ExpireDate,
    @Description
)

SELECT
    @ItemID = @@IDENTITY
GO

ALTER   PROCEDURE rb_DeleteAnnouncement
(
    @ItemID int
)
AS

DELETE FROM
    rb_Announcements_st

WHERE
    ItemID = @ItemID
GO


ALTER   PROCEDURE rb_UpdateAnnouncement
(
    @ItemID         int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     datetime,
    @Description    nvarchar(2000)
)
AS

UPDATE
    rb_Announcements_st

SET
    CreatedByUser   = @UserName,
    CreatedDate     = GetDate(),
    Title           = @Title,
    MoreLink        = @MoreLink,
    MobileMoreLink  = @MobileMoreLink,
    ExpireDate      = @ExpireDate,
    Description     = @Description

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_AnnouncementsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_AnnouncementsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Announcements_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Announcements_stModified]
GO

CREATE TRIGGER [rb_Announcements_stModified]
ON [rb_Announcements_st]
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

ALTER   PROCEDURE rb_GetAnnouncements
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
GO

ALTER   PROCEDURE rb_AddContact
(
    @ModuleID int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250),
    @ItemID   int OUTPUT
)
AS

INSERT INTO rb_Contacts_st
(
    CreatedByUser,
    CreatedDate,
    ModuleID,
    Name,
    Role,
    Email,
    Contact1,
    Contact2
)

VALUES
(
    @UserName,
    GetDate(),
    @ModuleID,
    @Name,
    @Role,
    @Email,
    @Contact1,
    @Contact2
)

SELECT
    @ItemID = @@IDENTITY
GO

ALTER   PROCEDURE rb_DeleteContact
(
    @ItemID int
)
AS

DELETE FROM
    rb_Contacts_st

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetContacts    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_GetContacts
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedDate,
	    CreatedByUser,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_Contacts
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT
	    ItemID,
	    CreatedDate,
	    CreatedByUser,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_Contacts_st
	WHERE
	    ModuleID = @ModuleID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_UpdateContact    Data dello script: 07/11/2002 22.28.14 ******/


ALTER   PROCEDURE rb_UpdateContact
(
    @ItemID   int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250)
)
AS

UPDATE
    rb_Contacts_st

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Name          = @Name,
    Role          = @Role,
    Email         = @Email,
    Contact1      = @Contact1,
    Contact2      = @Contact2

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Revert]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
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
		 ForeignKeyTableId	int,
		 KeyTableId		int,
		 KeyTableSchema		nvarchar(128),
		 KeyTable		nvarchar(128))

	INSERT INTO #TMPResults EXEC rb_GetRelatedTables 'rb_Modules'

	DECLARE RelatedTablesModules CURSOR FOR
		SELECT 	
			ForeignKeyTableSchema, 
			ForeignKeyTable,
			ForeignKeyColumn,
			KeyColumn,
			ForeignKeyTableId,
			KeyTableId,
			KeyTableSchema,
			KeyTable
		FROM
			#TMPResults
		WHERE 
			ForeignKeyTable <> 'ModuleSettings'

	-- Create temporary table for later use
	CREATE TABLE #TMPCount
		(ResultCount	int)


	-- Now search the table that has the related column
	OPEN RelatedTablesModules

	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
	DECLARE @ForeignKeyTable	nvarchar(128)
	DECLARE @ForeignKeyColumn	nvarchar(128)
	DECLARE @KeyColumn		nvarchar(128)
	DECLARE @ForeignKeyTableId	int
	DECLARE @KeyTableId		int
	DECLARE @KeyTableSchema		nvarchar(128)
	DECLARE @KeyTable		nvarchar(128)
	DECLARE @SQLStatement		nvarchar(4000)
	DECLARE @Count			int
	DECLARE @TableHasIdentityCol	int

	FETCH NEXT FROM RelatedTablesModules 
	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
		@KeyTableSchema, @KeyTable
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Check if this table has a corresponding staging table
		TRUNCATE TABLE #TMPCount
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[' + @ForeignKeyTable + '_st]'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
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
				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
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
					
					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
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
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
								IF (@ColIsIdentity = 0)
									SET @ColumnList = @ColumnList + ', '		
							END
							ELSE
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
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
			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
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

ALTER   PROCEDURE rb_UpdateContact
(
    @ItemID   int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250)
)
AS

UPDATE
    rb_Contacts_st

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Name          = @Name,
    Role          = @Role,
    Email         = @Email,
    Contact1      = @Contact1,
    Contact2      = @Contact2

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_DocumentsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_DocumentsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Documents_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Documents_stModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [rb_Documents_stModified]
ON [rb_Documents_st]
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

ALTER   PROCEDURE rb_DeleteDocument
(
    @ItemID int
)
AS

DELETE FROM
    rb_Documents_st

WHERE
    ItemID = @ItemID
GO

ALTER   PROCEDURE rb_GetDocumentContent
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetDocuments    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_GetDocuments
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents_st
	WHERE
	    ModuleID = @ModuleID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleContact    Data dello script: 07/11/2002 22.28.14 ******/



ALTER   PROCEDURE rb_GetSingleContact
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_Contacts
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_Contacts_st
	WHERE
	    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleDocument    Data dello script: 07/11/2002 22.28.14 ******/


ALTER   PROCEDURE rb_GetSingleDocument
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents_st
	WHERE
	    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleAnnouncement    Data dello script: 07/11/2002 22.28.09 ******/



ALTER   PROCEDURE rb_GetSingleAnnouncement
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
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
	    ItemID = @ItemID
ELSE
	SELECT
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
	    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.28.12 ******/


ALTER   PROCEDURE rb_AddEvent
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  DateTime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100),
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_Events_st
(
    ModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    ExpireDate,
    Description,
    WhereWhen
)

VALUES
(
    @ModuleID,
    @UserName,
    @Title,
    GetDate(),
    @ExpireDate,
    @Description,
    @WhereWhen
)

SELECT
    @ItemID = @@IDENTITY
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteEvent    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_DeleteEvent
(
    @ItemID int
)
AS

DELETE FROM
    rb_Events_st

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_GetEvents
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    Title,
	    CreatedByUser,
	    WhereWhen,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    rb_Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
ELSE
	SELECT
	    ItemID,
	    Title,
	    CreatedByUser,
	    WhereWhen,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    rb_Events_st
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleEvent    Data dello script: 07/11/2002 22.28.14 ******/


ALTER   PROCEDURE rb_GetSingleEvent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_Events
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_Events_st
	WHERE
	    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_UpdateEvent    Data dello script: 07/11/2002 22.28.14 ******/



ALTER   PROCEDURE rb_UpdateEvent
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  datetime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100)
)

AS

UPDATE
    rb_Events_st

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    ExpireDate    = @ExpireDate,
    Description   = @Description,
    WhereWhen     = @WhereWhen

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_EventsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_EventsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Events_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Events_stModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [rb_Events_stModified]
ON [rb_Events_st]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER   PROCEDURE rb_AddLink
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target	 nvarchar(10),
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_Links_st
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    MobileUrl,
    ViewOrder,
    Description,
    Target
)
VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @Url,
    @MobileUrl,
    @ViewOrder,
    @Description,
    @Target
)

SELECT
    @ItemID = @@IDENTITY
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteLink    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_DeleteLink
(
    @ItemID int
)
AS

DELETE FROM
    rb_Links_st

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetLinks    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_GetLinks
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links_st
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
GO

ALTER   PROCEDURE rb_GetSingleLink
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links_st
	WHERE
	    ItemID = @ItemID
GO


ALTER   PROCEDURE rb_UpdateLink
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target		 nvarchar(10)
)
AS

UPDATE
    rb_Links_st

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    Url           = @Url,
    MobileUrl     = @MobileUrl,
    ViewOrder     = @ViewOrder,
    Description   = @Description,
    Target		  = @Target

WHERE
    ItemID = @ItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_LinksModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_LinksModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Links_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Links_stModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [rb_Links_stModified]
ON [rb_Links_st]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

ALTER    PROCEDURE rb_GetRelatedTables
	@Name	nvarchar(128)
AS
	SELECT 
		[InnerResults].[ForeignKeyTableSchema],
		[InnerResults].[ForeignKeyTable], 
		[InnerResults].[ForeignKeyColumn], 
		[InnerResults].[KeyColumn],
		[InnerResults].[ForeignKeyTableId],
		[InnerResults].[KeyTableId],
		[InnerResults].[KeyTableSchema],
		[InnerResults].[KeyTable]
	FROM
		(
			SELECT     
				[FKeyTable].[TableName] AS ForeignKeyTable, 
				[FKeyTable].[TableSchema] As ForeignKeyTableSchema,
				[KeyTable].[TableName] AS KeyTable, 
				[KeyTable].[TableSchema] As KeyTableSchema,
				[FKeyColumns].[name] AS ForeignKeyColumn, 
			        [KeyColumns].[name] AS KeyColumn,
				[FKeyTable].[id] AS ForeignKeyTableId,
				[KeyTable].[id] AS KeyTableId
			FROM         sysforeignkeys INNER JOIN
			                      (
							SELECT     
								[sysobjects].[id] As ID, 
								[sysobjects].[name] AS TableName,
								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
							FROM    
								[sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
									ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
							WHERE   
								([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
					       ) FKeyTable ON sysforeignkeys.fkeyid = [FKeyTable].[ID] INNER JOIN
					       (
							SELECT     
								[sysobjects].[id] As ID, 
								[sysobjects].[name] AS TableName,
								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_ContactsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_ContactsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_ContactsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_ContactsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ContactsModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_ContactsModified_st]
GO

CREATE  TRIGGER [rb_ContactsModified_st]
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
        rb_Documents_st 
    WHERE 
        ItemID = @ItemID
)
INSERT INTO rb_Documents_st
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
    rb_Documents_st

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
    rb_Documents_st

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Announcements_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Announcements_st] DROP CONSTRAINT FK_st_rb_Announcements_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Announcements_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Announcements_st] DROP CONSTRAINT FK_rb_Announcements_st_rb_Modules
GO

ALTER TABLE [rb_Announcements_st] ADD 
	CONSTRAINT [FK_rb_Announcements_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Announcements_st] DROP CONSTRAINT PK_st_rb_Announcements
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_Announcements_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Announcements_st] DROP CONSTRAINT PK_rb_Announcements_st
GO

ALTER TABLE [rb_Announcements_st] ADD 
	CONSTRAINT [PK_rb_Announcements_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Contacts_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Contacts_st] DROP CONSTRAINT FK_st_rb_Contacts_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Contacts_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Contacts_st] DROP CONSTRAINT FK_rb_Contacts_st_rb_Modules
GO

ALTER TABLE [rb_Contacts_st] ADD 
	CONSTRAINT [FK_rb_Contacts_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Contacts_st] DROP CONSTRAINT PK_st_rb_Contacts
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_Contacts_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Contacts_st] DROP CONSTRAINT PK_rb_Contacts_st
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Contacts_st] DROP CONSTRAINT PK_st_Contacts
GO

ALTER TABLE [rb_Contacts_st] ADD 
	CONSTRAINT [PK_rb_Contacts_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Documents_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Documents_st] DROP CONSTRAINT FK_st_rb_Documents_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Documents_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Documents_st] DROP CONSTRAINT FK_rb_Documents_st_rb_Modules
GO

ALTER TABLE [rb_Documents_st] ADD 
	CONSTRAINT [FK_rb_Documents_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Documents_st] DROP CONSTRAINT PK_st_rb_Documents
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_Documents_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Documents_st] DROP CONSTRAINT PK_rb_Documents_st
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Documents_st] DROP CONSTRAINT PK_st_Documents
GO

ALTER TABLE [rb_Documents_st] ADD 
	CONSTRAINT [PK_rb_Documents_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Events_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Events_st] DROP CONSTRAINT FK_st_rb_Events_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Events_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Events_st] DROP CONSTRAINT FK_rb_Events_st_rb_Modules
GO

ALTER TABLE [rb_Events_st] ADD 
	CONSTRAINT [FK_rb_Events_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Events_st] DROP CONSTRAINT PK_st_rb_Events
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_Events_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Events_st] DROP CONSTRAINT PK_rb_Events_st
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Events_st] DROP CONSTRAINT PK_st_Events
GO

ALTER TABLE [rb_Events_st] ADD 
	CONSTRAINT [PK_rb_Events_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_HtmlText_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_HtmlText_st] DROP CONSTRAINT FK_st_rb_HtmlText_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_HtmlText_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_HtmlText_st] DROP CONSTRAINT FK_rb_HtmlText_st_rb_Modules
GO

ALTER TABLE [rb_HtmlText_st] ADD 
	CONSTRAINT [FK_rb_HtmlText_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_HtmlText_st] DROP CONSTRAINT PK_st_rb_HtmlText
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_HtmlText_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_HtmlText_st] DROP CONSTRAINT PK_rb_HtmlText_st
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_HtmlText_st] DROP CONSTRAINT PK_st_HtmlText
GO

ALTER TABLE [rb_HtmlText_st] ADD 
	CONSTRAINT [PK_rb_HtmlText_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_rb_Links_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Links_st] DROP CONSTRAINT FK_st_rb_Links_rb_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Links_st_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Links_st] DROP CONSTRAINT FK_rb_Links_st_rb_Modules
GO

ALTER TABLE [rb_Links_st] ADD 
	CONSTRAINT [FK_rb_Links_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_rb_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Links_st] DROP CONSTRAINT PK_st_rb_Links
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_Links_st]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Links_st] DROP CONSTRAINT PK_rb_Links_st
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_Links_st] DROP CONSTRAINT PK_st_Links
GO

ALTER TABLE [rb_Links_st] ADD 
	CONSTRAINT [PK_rb_Links_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLogin]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserLoginById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLoginById]
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

CREATE PROCEDURE rb_UserLoginById
(
    @PortalID int,
    @UserID    nvarchar(100),
    @Password nvarchar(20)
)
AS

SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email
FROM   rb_Users
WHERE
	rb_Users.UserID = @UserID AND rb_Users.Password = @Password AND rb_Users.PortalID = @PortalID
GO

