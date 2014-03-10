--Manu: String or Binary Data would be truncated.
--fixed http://sourceforge.net/tracker/index.php?func=detail&aid=821928&group_id=66837&atid=515929

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetTabsFlat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsFlat]
GO

CREATE PROCEDURE rb_GetTabsFlat
(
        @PortalID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #TabTree
(
        [TabID] [int],
        [TabName] [nvarchar] (100),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent levels
INSERT INTO     #TabTree
SELECT  TabID,
        TabName,
        ParentTabID,
        TabOrder,
        0,
        cast(100000000 + TabOrder as varchar)
FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder as varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END
--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        '(Orphan)' + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    rb_Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.TabID)
                         AND PortalID =@PortalID
-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) as ord , cast(TabID as varchar) as TabID into #tabs
from #TabTree
order by nestlevel, TreeOrder
-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID as int)=#TabTree.TabID) 
-- Return Temporary Table
SELECT TabID, parenttabID, tabname, TabOrder, NestLevel
FROM #TabTree 
order by TreeOrder
GO