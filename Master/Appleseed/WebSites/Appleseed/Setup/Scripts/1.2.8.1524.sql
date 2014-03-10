---------------------
--1.2.8.1524.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1524','1.2.8.1524', CONVERT(datetime, '03/23/2003', 101))
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountries]
GO

CREATE PROCEDURE GetCountries
(
	@IDLang	nchar(2) = 'en'
)

AS

IF 
(
SELECT     COUNT(Countries.CountryID) AS CountryListCount
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)
) > 0

BEGIN
-- Country returns results
SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)
ORDER BY Localize.Description
END

else

BEGIN
-- Get English list
SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = 'en') OR
                      (Cultures.NeutralCode = 'en')
ORDER BY Localize.Description
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesAllPortals]
GO

--Fix on Shortuctall module, shortcuts should not be displayed on GetModulesAllPortals list
CREATE PROCEDURE GetModulesAllPortals
AS

SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' as PortalAlias, -1 as TabOrder

UNION

	SELECT     Modules.ModuleID, Portals.PortalAlias + '/' + Tabs.TabName + '/' + Modules.ModuleTitle + ' (' + GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, Tabs.TabOrder
	FROM         Modules INNER JOIN
	                      Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
	                      Portals ON Tabs.PortalID = Portals.PortalID INNER JOIN
	                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
	                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
	WHERE     (Modules.ModuleID > 0) AND (GeneralModuleDefinitions.Admin = 0) AND (GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
ORDER BY PortalAlias, Modules.ModuleTitle
GO

-- Alter the modules tables so it contains the extra necessary columns for the workflow implementation
IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_CATALOG] = 'Appleseed' AND [TABLE_NAME] = 'Modules' AND [COLUMN_NAME] = 'LastModified') 
BEGIN
	ALTER TABLE Modules ADD
		LastModified datetime NULL,
		LastEditor nvarchar(256) NULL,
		StagingLastModified datetime NULL,
		StagingLastEditor nvarchar(256) NULL
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetLastModified]
GO

CREATE PROCEDURE GetLastModified
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
		from Modules
		WHERE [ModuleID] = @ModuleID
	end
	else
	begin
		select @LastModifiedDate = [StagingLastModified], @LastModifiedBy = [StagingLastEditor]
		from Modules
		WHERE [ModuleID] = @ModuleID
	end

	/* SET NOCOUNT ON */
	RETURN 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [SetLastModified]
GO

CREATE PROCEDURE SetLastModified
	(
		@ModuleID int,
		@LastModifiedBy	nvarchar(256)
	)
AS

	-- Check if this module supports workflow
	DECLARE @support	bit

	SELECT @support = SupportWorkflow
	FROM Modules
	WHERE ModuleID = @ModuleID

	IF ( @support = 1 )
	BEGIN
		-- It supports workflow
		UPDATE Modules
		SET StagingLastModified = getdate(),
                    StagingLastEditor = @LastModifiedBy
		WHERE ModuleID = @ModuleID
	END
	ELSE
		-- It doesn't support workflow
		UPDATE Modules
		SET LastModified = getdate(),
                    LastEditor = @LastModifiedBy,
		    StagingLastModified = getdate(),
		    StagingLastEditor = @LastModifiedBy
		WHERE ModuleID = @ModuleID

	/* SET NOCOUNT ON */
	RETURN 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Publish]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [Publish]
GO

-- Alter Publish stored procedure

CREATE  PROCEDURE Publish
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

	INSERT INTO #TMPResults EXEC GetRelatedTables 'Modules', 'dbo'

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
			ForeignKeyTableSchema = 'dbo'
			AND ForeignKeyTable <> 'ModuleSettings'

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
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM dbo.[sysobjects] WHERE [id] = object_id(N''[st_' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
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
	UPDATE [Modules]
	SET [WorkflowState] = 0, -- Original
	    [LastModified] = [StagingLastModified],
            [LastEditor] = [StagingLastEditor]
	WHERE [ModuleID] = @ModuleID

	RETURN
GO

-- Installs AppleseedVersion
-- this is the recommended way for install new modules
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{72C6F60A-50C4-4f20-8F89-3E8A27820557}'
SET @FriendlyName = 'Appleseed Version'
SET @DesktopSrc = 'DesktopModules/AppleseedVersion.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = ''
SET @Admin = 0
SET @Searchable = 0
-- Installs module
EXEC [AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

-- Install it for default portal
EXEC [UpdateModuleDefinitions] @GeneralModDefID, 0, 1
GO
