/* Install script, Pictures module, manudea 27/10/2003  */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Pictures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Pictures] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[MetadataXml] [nvarchar] (3000) NULL ,
	[ShortDescription] [nvarchar] (256) NULL ,
	[Keywords] [nvarchar] (256) NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	CONSTRAINT [PK_rb_Pictures] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	),
	CONSTRAINT [FK_rb_Pictures_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Pictures_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Pictures_st] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[MetadataXml] [nvarchar] (3000) NULL ,
	[ShortDescription] [nvarchar] (256) NULL ,
	[Keywords] [nvarchar] (256) NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	CONSTRAINT [PK_rb_Pictures_st] PRIMARY KEY  CLUSTERED 
	(
		[ItemID]
	)
)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddPicture]
GO

CREATE PROCEDURE [rb_AddPicture]
	(@ItemID 	[int ] OUTPUT,
	 @ModuleID 	[int],
	 @DisplayOrder	[int],
	 @MetadataXml nvarchar(3000),
	 @ShortDescription nvarchar(256),
	 @Keywords nvarchar(256),
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime
)
AS 
INSERT INTO [rb_Pictures_st]
	([ModuleID],
	[DisplayOrder],
	[MetadataXml],
	[ShortDescription],
	[Keywords],
	[CreatedByUser],
	[CreatedDate]
) 
VALUES 
	(@ModuleID,
	 @DisplayOrder,
	 @MetadataXml,
	 @ShortDescription,
	 @Keywords,
	 @CreatedByUser,
	 @CreatedDate)
SELECT 
	@ItemID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeletePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeletePicture]
GO

CREATE PROCEDURE [rb_DeletePicture]
	(@ItemID 	[int])
AS DELETE FROM [rb_Pictures_st]
WHERE 
	( [ItemID] = @ItemID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPicturesPaged]
GO

CREATE     PROCEDURE rb_GetPicturesPaged
(
	@ModuleID int,
	@Page int = 1,
	@RecordsPerPage int = 10,
	@WorkflowVersion int
)
AS

-- Find out the first AND last record we want
DECLARE @FirstRec int, @LastRec int

--Create a temporary table
CREATE TABLE #TempItems
(
	ID				int IDENTITY,
 	ItemID 			int,
 	ModuleID 		int,
	DisplayOrder		int,
 	MetadataXml		nvarchar(3000),
 	ShortDescription		nvarchar(256),
 	Keywords		nvarchar(256),
	CreatedByUser		nvarchar(100),
	CreatedDate		datetime
)

IF ( @WorkflowVersion = 1 )
BEGIN

	-- We don't want to RETURN the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON

	-- INSERT the rows FROM tblItems into the temp. table
	INSERT INTO
	#TempItems
	(
		ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords, CreatedByUser, CreatedDate
	)
	SELECT
		rb_Pictures.ItemID, 
		rb_Pictures.DisplayOrder, 
		rb_Pictures.MetadataXml, 
		rb_Pictures.ShortDescription, 
		rb_Pictures.Keywords,
		rb_Pictures.CreatedByUser,
		rb_Pictures.CreatedDate
	FROM
		rb_Pictures
	WHERE
		rb_Pictures.ModuleID = @ModuleID
	ORDER BY 
		DisplayOrder
	
	SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
	SELECT @LastRec = (@Page * @RecordsPerPage + 1)
	
	-- Now, RETURN the SET of paged records, plus, an indiciation of we
	-- have more records or not!
	SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
	FROM #TempItems
	WHERE ID > @FirstRec AND ID < @LastRec
	ORDER BY ID
	
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
END
ELSE
BEGIN
	-- We don't want to RETURN the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON
	
	-- INSERT the rows FROM tblItems into the temp. table
	INSERT INTO
	#TempItems
	(
		ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords, CreatedByUser, CreatedDate
	)
	SELECT
		rb_Pictures_st.ItemID, 
		rb_Pictures_st.DisplayOrder, 
		rb_Pictures_st.MetadataXml, 
		rb_Pictures_st.ShortDescription, 
		rb_Pictures_st.Keywords,
		rb_Pictures_st.CreatedByUser,
		rb_Pictures_st.CreatedDate
	FROM
		rb_Pictures_st
	WHERE
		rb_Pictures_st.ModuleID = @ModuleID
	ORDER BY 
		DisplayOrder
	
	SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
	SELECT @LastRec = (@Page * @RecordsPerPage + 1)
	
	-- Now, RETURN the SET of paged records, plus, an indiciation of we
	-- have more records or not!
	SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
	FROM #TempItems
	WHERE ID > @FirstRec AND ID < @LastRec
	ORDER BY ID
	
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSinglePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSinglePicture]
GO

CREATE PROCEDURE rb_GetSinglePicture 
(
	@ItemID int,
	@WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
	SELECT 
		OriginalPictures.ItemID, 
		(
			SELECT TOP 1
				ItemID
			FROM 
				rb_Pictures
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Pictures WHERE ItemID = OriginalPictures.ItemID)
				AND ItemID <> OriginalPictures.ItemID
				AND (DisplayOrder < OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID < OriginalPictures.ItemID))
			ORDER BY
				OriginalPictures.DisplayOrder - DisplayOrder, OriginalPictures.ItemID - ItemID
		) AS PreviousItemID,
		(
			SELECT TOP 1
				ItemID
			FROM 
				rb_Pictures
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Pictures WHERE ItemID = OriginalPictures.ItemID)
				AND ItemID <> OriginalPictures.ItemID
				AND (DisplayOrder > OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID > OriginalPictures.ItemID))
			ORDER BY
				DisplayOrder - OriginalPictures.DisplayOrder ,	ItemID - OriginalPictures.ItemID 
		) AS NextItemID,
		OriginalPictures.ModuleID, 
		OriginalPictures.DisplayOrder, 
		OriginalPictures.MetadataXml, 
		OriginalPictures.ShortDescription, 
		OriginalPictures.Keywords
	FROM 
		rb_Pictures AS OriginalPictures
	WHERE 
		ItemID = @ItemID
ELSE
	SELECT 
		OriginalPictures.ItemID, 
		(
			SELECT TOP 1
				ItemID
			FROM 
				rb_Pictures_st
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Pictures_st WHERE ItemID = OriginalPictures.ItemID)
				AND ItemID <> OriginalPictures.ItemID
				AND (DisplayOrder < OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID < OriginalPictures.ItemID))
			ORDER BY
				OriginalPictures.DisplayOrder - DisplayOrder, OriginalPictures.ItemID - ItemID
		) AS PreviousItemID,
		(
			SELECT TOP 1
				ItemID
			FROM 
				rb_Pictures_st
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Pictures_st WHERE ItemID = OriginalPictures.ItemID)
				AND ItemID <> OriginalPictures.ItemID
				AND (DisplayOrder > OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID > OriginalPictures.ItemID))
			ORDER BY
				DisplayOrder - OriginalPictures.DisplayOrder ,	ItemID - OriginalPictures.ItemID 
		) AS NextItemID,
		OriginalPictures.ModuleID, 
		OriginalPictures.DisplayOrder, 
		OriginalPictures.MetadataXml, 
		OriginalPictures.ShortDescription, 
		OriginalPictures.Keywords
	FROM 
		rb_Pictures_st AS OriginalPictures
	WHERE 
		ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdatePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdatePicture]
GO

CREATE PROCEDURE [rb_UpdatePicture]
	(@ItemID 	[int ],
	 @ModuleID 	[int],
	 @DisplayOrder	[int],
	 @MetadataXml nvarchar(3000),
	 @ShortDescription nvarchar(256),
	 @Keywords nvarchar(256),
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime
)
AS 
UPDATE [rb_Pictures_st]
SET  
	 [DisplayOrder] 		= @DisplayOrder,
	 [MetadataXml]		= @MetadataXml,
	 [ShortDescription]	= @ShortDescription,
	 [Keywords]		= @Keywords,
	 [CreatedByUser]	= @CreatedByUser,
	 [CreatedDate]		= @CreatedDate
WHERE 
	( [ItemID]	 = @ItemID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Pictures_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
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
GO
