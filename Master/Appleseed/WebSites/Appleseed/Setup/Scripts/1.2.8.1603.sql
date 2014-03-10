---------------------
--1.2.8.1603.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1603','1.2.8.1603', CONVERT(datetime, '04/04/2003', 101))
GO


/****** Object:  Table [rb_st_Announcements]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_Announcements', 'st_rb_Announcements'
GO

/****** Object:  Table [rb_st_Contacts]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_Contacts', 'st_rb_Contacts'
GO

/****** Object:  Table [rb_st_Documents]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_Documents', 'st_rb_Documents'
GO

/****** Object:  Table [rb_st_Events]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_Events', 'st_rb_Events'
GO

/****** Object:  Table [rb_st_HtmlText]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_HtmlText', 'st_rb_HtmlText'
GO

/****** Object:  Table [rb_st_Links]    Script Date: 4/4/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'rb_st_Links', 'st_rb_Links'
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
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[st_' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
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
				'SELECT Count(*) FROM [st_' + @ForeignKeyTable + '] ' +
				'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

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
						'SELECT ' + @ColumnList + ' FROM [st_' + @ForeignKeyTable + '] ' +
						'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					-- PRINT @SQLStatement
					EXEC(@SQLStatement)
				END
				ELSE
				BEGIN
					-- The table doens't contain a identitycolumn
					SET @SQLStatement = 
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
						'SELECT * FROM [st_' + @ForeignKeyTable + '] ' +
						'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
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
        st_rb_HtmlText 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO st_rb_HtmlText (
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
    st_rb_HtmlText

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
	    st_rb_HtmlText
	WHERE
	    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_HtmlTextModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_HtmlTextModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_HtmlTextModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_HtmlTextModified]
GO

CREATE    TRIGGER [st_rb_HtmlTextModified]
ON [st_rb_HtmlText]
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

INSERT INTO st_rb_Announcements
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
    st_rb_Announcements

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
    st_rb_Announcements

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_AnnouncementsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_AnnouncementsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_AnnouncementsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_AnnouncementsModified]
GO

CREATE TRIGGER [st_rb_AnnouncementsModified]
ON [st_rb_Announcements]
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
	    st_rb_Announcements
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

INSERT INTO st_rb_Contacts
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
    st_rb_Contacts

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
	    st_rb_Contacts
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
    st_rb_Contacts

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Revert]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Revert]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[st_' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
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
				'SELECT Count(*) FROM [st_' + @ForeignKeyTable + '] ' +
				'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

			EXEC(@SQLStatement)
			SELECT @Count = ResultCount
				FROM #TMPCount		
			IF (@Count >= 1) 
			BEGIN
				-- This table contains the related data 
				-- Delete everything in the staging table
				SET @SQLStatement = 
					'DELETE FROM [st_' + @ForeignKeyTable + '] ' +
					'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
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
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[st_' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
						'SELECT ' + @ColumnList + ' FROM [' + @ForeignKeyTable + '] ' +
						'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					-- PRINT @SQLStatement
					EXEC(@SQLStatement)
				END
				ELSE
				BEGIN
					-- The table doens't contain a identitycolumn
					SET @SQLStatement = 
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[st_' + @ForeignKeyTable + '] ' +
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
    st_rb_Contacts

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_DocumentsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_DocumentsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_DocumentsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_DocumentsModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [st_rb_DocumentsModified]
ON [st_rb_Documents]
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

/****** Oggetto: stored procedure DeleteDocument    Data dello script: 07/11/2002 22.28.13 ******/


ALTER   PROCEDURE rb_DeleteDocument
(
    @ItemID int
)
AS

DELETE FROM
    st_rb_Documents

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

/****** Oggetto: stored procedure GetDocumentContent    Data dello script: 07/11/2002 22.28.13 ******/


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
	    st_rb_Documents
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
	    st_rb_Documents
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
	    st_rb_Contacts
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
	    st_rb_Documents
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
	    st_rb_Announcements
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

INSERT INTO st_rb_Events
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
    st_rb_Events

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
	    st_rb_Events
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
	    st_rb_Events
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
    st_rb_Events

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_EventsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_EventsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_EventsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_EventsModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [st_rb_EventsModified]
ON [st_rb_Events]
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

INSERT INTO st_rb_Links
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
    st_rb_Links

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
	    st_rb_Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleLink    Data dello script: 07/11/2002 22.28.14 ******/


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
	    st_rb_Links
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
    st_rb_Links

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_st_LinksModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_st_LinksModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_rb_LinksModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_rb_LinksModified]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE  TRIGGER [st_rb_LinksModified]
ON [st_rb_Links]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

