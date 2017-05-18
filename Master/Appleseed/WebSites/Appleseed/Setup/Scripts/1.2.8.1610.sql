---------------------
--1.2.8.1610.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddPicture]
-- Drop AddPicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeletePicture]
-- Drop DeletePicture stored proc
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPicturesPaged]
-- Drop GetPicturesPaged stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSinglePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSinglePicture]
-- Drop GetSinglePicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPicture]
-- Drop GetPicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePicture]
-- Drop UpdatePicture stored proc
GO

-- NOW Let's drop the rb_ Versions as well

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddPicture]
-- Drop rb_AddPicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeletePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeletePicture]
-- Drop rb_DeletePicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPicturesPaged]
-- Drop rb_GetPicturesPaged stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSinglePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSinglePicture]
-- Drop rb_GetSinglePicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPicture]
-- Drop rb_GetPicture stored proc
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdatePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdatePicture]
-- Drop rb_UpdatePicture stored proc
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Pictures_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Pictures_st]
--Drop the staging table IF EXISTS
GO


CREATE TABLE [rb_Pictures_st] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[MetadataXml] [nvarchar] (3000),
	[ShortDescription] [nvarchar] (256) ,
	[Keywords] [nvarchar] (256),
	[CreatedByUser] [nvarchar] (100),
	[CreatedDate] [datetime] NULL 
) ON [PRIMARY]
--Create the new staging table for the Picture workflow
GO

ALTER TABLE rb_Pictures_st ADD CONSTRAINT
	PK_rb_Pictures_st PRIMARY KEY CLUSTERED 
	(
	ItemID
	) ON [PRIMARY]
--Add a constraint for our new table
GO


-- =============================================================
-- create the stored procs
-- =============================================================
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
-- Create rb_AddPicture stored proc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



--************************************************************************
CREATE PROCEDURE [rb_DeletePicture]
	(@ItemID 	[int])
AS DELETE FROM [rb_Pictures_st]
WHERE 
	( [ItemID] = @ItemID)
-- Create rb_DeletePicture stored proc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



--************************************************************************
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

	-- We don't want to return the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON

	-- Insert the rows from tblItems into the temp. table
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
	
	-- Now, return the set of paged records, plus, an indiciation of we
	-- have more records or not!
	SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
	FROM #TempItems
	WHERE ID > @FirstRec AND ID < @LastRec
	
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
END
ELSE
BEGIN
	-- We don't want to return the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON
	
	-- Insert the rows from tblItems into the temp. table
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
	
	-- Now, return the set of paged records, plus, an indiciation of we
	-- have more records or not!
	SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
	FROM #TempItems
	WHERE ID > @FirstRec AND ID < @LastRec
	
	-- Turn NOCOUNT back OFF
	SET NOCOUNT OFF
END
-- Create rb_GetPicturesPaged stored proc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


--************************************************************************
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
		rb_Pictures As OriginalPictures
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
		rb_Pictures_st As OriginalPictures
	WHERE 
		ItemID = @ItemID
-- Create rb_GetSinglePicture stored proc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


--************************************************************************
CREATE PROCEDURE [rb_UpdatePicture]
	(@ItemID 	[int],
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
-- Create rb_UpdatePicture stored proc
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



ALTER TABLE rb_Pictures ALTER COLUMN MetadataXml nvarchar(3000)
ALTER TABLE rb_Pictures ALTER COLUMN ShortDescription nvarchar(256)
ALTER TABLE rb_Pictures ALTER COLUMN Keywords nvarchar(256)
ALTER TABLE rb_Pictures ADD CreatedByUser nvarchar(100) NULL
ALTER TABLE rb_Pictures ADD CreatedDate datetime NULL

-- Modify the rb_Pictures table so that it complies with new standards
GO


INSERT INTO rb_Pictures_st
   SELECT ModuleID, DisplayOrder, MetadataXml, ShortDescription, Keywords, CreatedByUser, CreatedDate	
   FROM rb_Pictures
-- Let's copy old data to the staging table

GO

CREATE  TRIGGER [rb_PicturesModified_st]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


UPDATE rb_GeneralModuleDefinitions
SET Searchable = 1
WHERE GeneralModDefID = '{B29CB86B-AEA1-4E94-8B77-B4E4239258B0}'
-- Update the GeneralModuleDefinitions table so that picture module is now searchable
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1610','1.2.8.1610', CONVERT(datetime, '04/10/2003', 101))
GO
