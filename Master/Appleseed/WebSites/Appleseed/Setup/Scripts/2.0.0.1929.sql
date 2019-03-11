
ALTER PROCEDURE [dbo].[rb_GetPageTree]
(
	@PortalID int
)
AS
--drop table #tree
-- Get the hierarchy
--create table #tree (id int, sequence varchar(1000), levelNo int, PageOrder int)
declare @tree Table(id int, sequence1 varchar(1000), levelNo int, PageOrder int)



-- insert top level (to get sub tree just insert relevent id here)
insert @tree 
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
     insert @tree
     -- Get all children of previous level
	
     select rb_Pages.PageID, 
	sequence1 + '.'+ convert(varchar(10),Len(rb_Pages.PageOrder)) + '.' + convert(varchar(10),rb_Pages.PageOrder), 
	@i + 1, 
	rb_Pages.PageOrder
     from rb_Pages, @tree 
     where levelNo = @i
	     and rb_Pages.ParentPageID = id and (PortalId = @PortalID)
     Order BY rb_Pages.PageOrder
end

-- output with hierarchy formatted
select rb_Pages.PageID, 
	rb_Pages.ParentPageID, 
	rb_Pages.PageOrder, 
	rb_Pages.PageName, 
	rb_Pages.PortalID,
	rb_Pages.AuthorizedRoles,
	rb_Pages.PageLayout,
	rb_Pages.PageDescription,
	levelNo , 
	Replicate('-', (levelNo) * 2) + rb_Pages.PageName as PageOrder,
	rb_Pages.PageOrder as PageOrderInt
		--, #tree.sequence
from @tree, rb_Pages
where id = rb_Pages.PageID
order by sequence1--, rb_Pages.PageOrder

--drop table #tree
--drop table #z