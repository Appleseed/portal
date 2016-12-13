ALTER PROCEDURE [dbo].[rb_GetTabsFlat]
(
        @PortalID int
)
AS
BEGIN
	--Create a Temporary table to hold the tabs for this query
	declare @PageTree Table
	(
			[PageID] [int],
			[PageName] [nvarchar] (200),
			[ParentPageID] [int],
			[PageOrder] [int],
			[NestLevel] [int],
			[TreeOrder] [varchar] (1000)
	)
	SET NOCOUNT ON  --Turn off echo of "... row(s) affected"
	DECLARE @LastLevel smallint
	SET @LastLevel = 0
	-- First, the parent levels
	INSERT INTO     @PageTree
	SELECT  PageID,
			PageName,
			ParentPageID,
			PageOrder,
			0,
			cast(100000000 + PageOrder as varchar)
	FROM    rb_Pages
	WHERE   ParentPageID IS NULL AND PortalID =@PortalID
	ORDER BY PageOrder
	-- Next, the children levels
	WHILE (@@rowcount > 0)
	BEGIN
	  SET @LastLevel = @LastLevel + 1
	  INSERT        @PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
					SELECT  rb_Pages.PageID,
							Replicate('-', @LastLevel *2) + rb_Pages.PageName,
							rb_Pages.ParentPageID,
							rb_Pages.PageOrder,
							@LastLevel,
							cast(pt.TreeOrder as varchar) + '.' + cast(100000000 + rb_Pages.PageOrder as varchar)
					FROM    rb_Pages join @PageTree pt on rb_Pages.ParentPageID= pt.PageID
					WHERE   EXISTS (SELECT 'X' FROM @PageTree WHERE PageID = rb_Pages.ParentPageID AND NestLevel = @LastLevel - 1)
					 AND PortalID =@PortalID
					ORDER BY pt.PageOrder
	END
	--Get the Orphans
	  INSERT        @PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
					SELECT  rb_Pages.PageID,
							'(Orphan)' + rb_Pages.PageName,
							rb_Pages.ParentPageID,
							rb_Pages.PageOrder,
							999999999,
							'999999999'
					FROM    rb_Pages 
					WHERE   NOT EXISTS (SELECT 'X' FROM @PageTree WHERE PageID = rb_Pages.PageID)
							 AND PortalID =@PortalID
	---- Reorder the Pages by using a 2nd Temp table AND an identity field to keep them straight.
	--select IDENTITY(int,1,2) as ord , cast(PageID as varchar) as PageID into #Pages
	--from @PageTree
	--order by nestlevel, TreeOrder
	---- Change the PageOrder in the sirt temp table so that Pages are ordered in sequence
	--update pt set PageOrder=(select ord from @pageTree pt1 WHERE cast(pt1.PageID as int)=pt.PageID) from @pageTree pt
	-- Return Temporary Table
	SELECT PageID, parentPageID, Pagename, PageOrder, NestLevel
	FROM @PageTree 
	order by TreeOrder
END