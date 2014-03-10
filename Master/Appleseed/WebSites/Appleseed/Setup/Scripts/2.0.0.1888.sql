/****** Object:  StoredProcedure [dbo].[rb_GetPicturesPaged]    Script Date: 12/01/2010 14:36:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPicturesPaged]
GO


CREATE     PROCEDURE [dbo].[rb_GetPicturesPaged]
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
