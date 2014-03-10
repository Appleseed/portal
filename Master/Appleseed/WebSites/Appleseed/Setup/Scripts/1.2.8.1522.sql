---------------------
--1.2.8.1522.sql
---------------------

--Update Stored PROCEDURE: UpdateTab
--Prevents orphaning a tab or placing tabs in an infinte recursive loop
--26 dec 2002 - Cory Isakson
ALTER PROCEDURE UpdateTab
(
    @PortalID        int,
    @TabID           int,
    @ParentTabID     int,
    @TabOrder        int,
    @TabName         nvarchar(50),
    @AuthorizedRoles nvarchar(256),
    @MobileTabName   nvarchar(50),
    @ShowMobile      bit
)
AS
IF (@ParentTabID = 0) 
    SET @ParentTabID = NULL
IF NOT EXISTS
(
    SELECT 
        * 
    FROM 
        Tabs
    WHERE 
        TabID = @TabID
)
INSERT INTO Tabs (
    PortalID,
    ParentTabID,
    TabOrder,
    TabName,
    AuthorizedRoles,
    MobileTabName,
    ShowMobile
) 
VALUES (
    @PortalID,
    @TabOrder,
    @ParentTabID,
    @TabName,
    @AuthorizedRoles,
    @MobileTabName,
    @ShowMobile
   
)
ELSE
--Updated 26.Dec.2002 Cory Isakson
--Check the Tab recursion so Tab is not placed into an infinate loop when building Tab Tree
BEGIN TRAN
--If the Update breaks the tab from having a path back to the root then do not Update
UPDATE
    Tabs
SET
    ParentTabID = @ParentTabID,
    TabOrder = @TabOrder,
    TabName = @TabName,
    AuthorizedRoles = @AuthorizedRoles,
    MobileTabName = @MobileTabName,
    ShowMobile = @ShowMobile
WHERE
    TabID = @TabID

--Create a Temporary table to hold the tabs
CREATE TABLE #TabTree
(
	[TabID] [int],
	[TabName] [nvarchar] (50),
	[ParentTabID] [int],
	[TabOrder] [int],
	[NestLevel] [int],
	[TreeOrder] [varchar] (1000)
)

SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0

-- First, the parent levels
INSERT INTO	#TabTree
SELECT 	TabID,
	TabName,
	ParentTabID,
	TabOrder,
	0,
	cast(100000000 + TabOrder as varchar)
FROM	Tabs
WHERE	ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT 	#TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
		SELECT 	Tabs.TabID,
			Replicate('-', @LastLevel *2) + Tabs.TabName,
			Tabs.ParentTabID,
			Tabs.TabOrder,
			@LastLevel,
			cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + Tabs.TabOrder as varchar)
		FROM	Tabs join #TabTree on Tabs.ParentTabID= #TabTree.TabID
		WHERE	EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
		 AND PortalID =@PortalID
		ORDER BY #TabTree.TabOrder
END

--Check that the Tab is found in the Tree.  If it is not then we abort the Update
IF NOT EXISTS (SELECT TabID from #TabTree WHERE TabID=@TabID)
BEGIN
	ROLLBACK TRAN
	--If we want to modify the TabLayout code then we can throw an error AND catch it.
	RAISERROR('Not allowed to choose that parent.',11,1)
END
ELSE
COMMIT TRAN
--End changes 26.Dec.2002 Cory Isakson
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1522','1.2.8.1522', CONVERT(datetime, '03/22/2003', 101))
GO

