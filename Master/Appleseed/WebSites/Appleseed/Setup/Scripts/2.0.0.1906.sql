IF NOT EXISTS(
    SELECT *
    FROM sys.columns 
    WHERE Name      = N'IsPage'
      AND Object_ID = Object_ID(N'rb_Recycler'))
BEGIN
	Alter table rb_Recycler 
	add IsPage bit default 0
END
GO


create PROCEDURE [dbo].[rb_DeleteTabToRecycler]
(
    @PageID int,
	@DeletedBy nvarchar(250),
    @DateDeleted datetime
)
AS
BEGIN

Declare @ParentId int;

Select @ParentId = [PortalID] from rb_Pages where PageID = @PageID

UPDATE rb_Pages SET PortalID = -1 where PageID = @PageID

INSERT INTO rb_Recycler (ModuleID, DateDeleted, DeletedBy, OriginalTab, IsPage)	VALUES	(@PageID, @DateDeleted, @DeletedBy,@ParentId,1)

END
GO


create PROCEDURE [dbo].[rb_RestoreTab]
(
    @PageID int,
	@ParentID int
)
AS
BEGIN

Declare @RPageId int;

Select @RPageId = OriginalTab from rb_Recycler where ModuleID = @PageID

IF(@ParentID >0)
BEGIN
	UPDATE rb_Pages SET PortalID = @RPageId,ParentPageID=@ParentID where PageID = @PageID
END
ELSE
BEGIN
	UPDATE rb_Pages SET PortalID = @RPageId where PageID = @PageID
END

Delete from rb_Recycler where ModuleID = @PageID

END
GO



create PROCEDURE [dbo].[rb_GetPagesInRecycler]
(
 @PortalID int,
 @SortField VarChar(50)
)
AS
SELECT     
	rb_Recycler.DateDeleted, 
	rb_Recycler.DeletedBy, 
	rb_Recycler.OriginalTab, 
	rb_Recycler.ModuleID, 
	rb_Pages.PageName, 
    rb_Pages.PageName as 'OriginalPageName'

FROM    rb_Recycler INNER JOIN
		rb_Pages ON rb_Recycler.ModuleID = rb_Pages.PageID 
        --rb_Modules ON rb_Recycler.ModuleID = rb_Modules.ModuleID INNER JOIN
        --rb_Pages ON rb_Recycler.OriginalTab = rb_Pages.PageID
WHERE rb_Recycler.OriginalTab = @PortalID and IsPage = 1
ORDER BY 
	CASE 
	 WHEN @SortField = 'DateDeleted' THEN CAST(DateDeleted AS VarChar(50))
         WHEN @SortField = 'DeletedBy' THEN CAST(DeletedBy AS VarChar(50))
         WHEN @SortField = 'OriginalTab' THEN CAST(OriginalTab AS VarChar(50))
         WHEN @SortField = 'ModuleID' THEN CAST(rb_Recycler.ModuleID AS VarChar(50)) 
         WHEN @SortField = 'PageTitle' THEN CAST(rb_Pages.PageName AS VarChar(50))
         WHEN @SortField = 'PageName' THEN CAST(rb_Pages.PageName AS VarChar(50))
         ELSE PageName
END
GO


alter PROCEDURE [dbo].[rb_RestoreTab]
(
    @PageID int,
	@ParentID int
)
AS
BEGIN

Declare @RPageId int;

Select @RPageId = OriginalTab from rb_Recycler where ModuleID = @PageID

IF(@ParentID >0)
BEGIN
	UPDATE rb_Pages SET PortalID = @RPageId,ParentPageID=@ParentID where PageID = @PageID
END
ELSE
BEGIN
	UPDATE rb_Pages SET PortalID = @RPageId,ParentPageID=NULL where PageID = @PageID
END

Delete from rb_Recycler where ModuleID = @PageID

END
GO



ALTER PROCEDURE [dbo].[rb_GetPageTree]
(
	@PortalID int
)
AS
--drop table #tree
-- Get the hierarchy
create table #tree (id int, sequence varchar(1000), levelNo int, PageOrder int)



-- insert top level (to get sub tree just insert relevent id here)
insert #tree 
select PageID, (convert(varchar(10),Len(rb_Pages.PageOrder)) + '.' + convert(varchar(10),rb_Pages.PageOrder)), 1, PageOrder
from rb_Pages 
where (PortalId = @PortalID) And (ParentPageID is null)
Order BY rb_Pages.PageOrder

declare @i int
select @i = 0
-- keep going until no more rows added
while @@rowcount > 0
begin
     select @i = @i + 1
     insert #tree
     -- Get all children of previous level
	
     select rb_Pages.PageID, 
	#tree.sequence + '.'+ convert(varchar(10),Len(rb_Pages.PageOrder)) + '.' + convert(varchar(10),rb_Pages.PageOrder), 
	@i + 1, 
	rb_Pages.PageOrder
     from rb_Pages, #tree 
     where #tree.levelNo = @i
	     and rb_Pages.ParentPageID = #tree.id and (PortalId = @PortalID)
     Order BY rb_Pages.PageOrder
end

-- output with hierarchy formatted
select rb_Pages.PageID, 
	rb_Pages.ParentPageID, 
	rb_Pages.PageOrder, 
	rb_Pages.PageName, 
	#tree.levelNo , 
	Replicate('-', (#tree.levelNo) * 2) + rb_Pages.PageName as PageOrder
	--, #tree.sequence
from #tree, rb_Pages
where #tree.id = rb_Pages.PageID
order by #tree.sequence--, rb_Pages.PageOrder

drop table #tree
--drop table #z
GO