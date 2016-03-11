
ALTER PROCEDURE [dbo].[rb_GetPortalSettings]
(
    @PortalAlias   nvarchar(50),
    @PageID         int,
    @PortalLanguage nvarchar(12),
    @PortalID      int OUTPUT,
    @PortalName    nvarchar(128) OUTPUT,
    @PortalPath    nvarchar(128) OUTPUT,
    @AlwaysShowEditButton bit OUTPUT,
    @PageName       nvarchar (200)  OUTPUT,
    @PageOrder      int OUTPUT,
    @ParentPageID      int OUTPUT,
    @MobilePageName nvarchar (200)  OUTPUT,
    @AuthRoles     nvarchar (256) OUTPUT,
    @ShowMobile    bit OUTPUT,
	@FriendURL nvarchar (1024) OUTPUT
)
AS
/* First, get Out Params */
IF @PageID = 0 
    SELECT TOP 1
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @PageID         = rb_Pages.PageID,
        @PageOrder      = rb_Pages.PageOrder,
        @ParentPageID   = rb_Pages.ParentPageID,
        @PageName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Pages.PageID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   PageName)  ,
        @MobilePageName = rb_Pages.MobilePageName,
        @AuthRoles     = rb_Pages.AuthorizedRoles,
        @ShowMobile    = rb_Pages.ShowMobile,
		@FriendURL	   = ISNULL(rb_pages.FriendlyURL,'')
    FROM
        rb_Pages
    INNER JOIN
        rb_Portals ON rb_Pages.PortalID = rb_Portals.PortalID
    WHERE
        PortalAlias=@PortalAlias
        
    ORDER BY
        PageOrder
ELSE 
    SELECT
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @PageName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Pages.PageID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   PageName),
        @PageOrder      = rb_Pages.PageOrder,
        @ParentPageID   = rb_Pages.ParentPageID,
        @MobilePageName = rb_Pages.MobilePageName,
        @AuthRoles     = rb_Pages.AuthorizedRoles,
        @ShowMobile    = rb_Pages.ShowMobile,
		@FriendURL	   = ISNULL(rb_pages.FriendlyURL,'')
    FROM
        rb_Pages
    INNER JOIN
        rb_Portals ON rb_Pages.PortalID = rb_Portals.PortalID
        
    WHERE
        PageID=@PageID AND rb_Portals.PortalAlias=@PortalAlias
/* Get Pages list */
SELECT     
COALESCE ((SELECT     SettingValue
                              FROM         rb_TabSettings
                              WHERE     TabID = rb_Pages.PageID AND SettingName = @PortalLanguage AND Len(SettingValue) > 0), PageName) AS PageName, 
							  ISNULL(FriendlyURL,'') AS FriendlyURL,
							  AuthorizedRoles, 
							  PageID, 
							  ParentPageID, 
							  PageOrder, 
                      PageLayout
FROM         rb_Pages
WHERE     (PortalID = @PortalID)
ORDER BY PageOrder
    
/* Get Mobile Tabs list */
SELECT  
    MobilePageName,
    AuthorizedRoles,
    PageID,
    ParentPageID,
    ShowMobile,
	ISNULL(FriendlyURL,'') AS FriendlyURL
FROM    
    rb_Pages
WHERE   
    PortalID = @PortalID  AND  ShowMobile = 1
ORDER BY
    PageOrder
/* Then, get the DataTable of module info */
SELECT     *
FROM         rb_Modules INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.TabID = @PageID) OR
                      (rb_Modules.ShowEveryWhere = 1) AND (rb_ModuleDefinitions.PortalID = @PortalID)
ORDER BY rb_Modules.ModuleOrder
GO

ALTER PROCEDURE [dbo].[rb_GetTabsFlat]
(
        @PortalID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #PageTree
(
        [PageID] [int],
        [PageName] [nvarchar] (200),
        [ParentPageID] [int],
        [PageOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent levels
INSERT INTO     #PageTree
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
  INSERT        #PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
                SELECT  rb_Pages.PageID,
                        Replicate('-', @LastLevel *2) + rb_Pages.PageName,
                        rb_Pages.ParentPageID,
                        rb_Pages.PageOrder,
                        @LastLevel,
                        cast(#PageTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Pages.PageOrder as varchar)
                FROM    rb_Pages join #PageTree on rb_Pages.ParentPageID= #PageTree.PageID
                WHERE   EXISTS (SELECT 'X' FROM #PageTree WHERE PageID = rb_Pages.ParentPageID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #PageTree.PageOrder
END
--Get the Orphans
  INSERT        #PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
                SELECT  rb_Pages.PageID,
                        '(Orphan)' + rb_Pages.PageName,
                        rb_Pages.ParentPageID,
                        rb_Pages.PageOrder,
                        999999999,
                        '999999999'
                FROM    rb_Pages 
                WHERE   NOT EXISTS (SELECT 'X' FROM #PageTree WHERE PageID = rb_Pages.PageID)
                         AND PortalID =@PortalID
-- Reorder the Pages by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) as ord , cast(PageID as varchar) as PageID into #Pages
from #PageTree
order by nestlevel, TreeOrder
-- Change the PageOrder in the sirt temp table so that Pages are ordered in sequence
update #PageTree set PageOrder=(select ord from #Pages WHERE cast(#Pages.PageID as int)=#PageTree.PageID) 
-- Return Temporary Table
SELECT PageID, parentPageID, Pagename, PageOrder, NestLevel
FROM #PageTree 
order by TreeOrder
GO

--Update Stored PROCEDURE: rb_UpdateTab
--Prevents orphaning a Page or placing Pages in an infinte recursive loop
ALTER PROCEDURE [dbo].[rb_UpdateTab]
(
    @PortalID        int,
    @PageID           int,
    @ParentPageID     int,
    @PageOrder        int,
    @PageName         nvarchar(200),
    @AuthorizedRoles nvarchar(256),
    @MobilePageName   nvarchar(200),
    @ShowMobile      bit,
	@FriendlyURL	nvarchar(1024)
)
AS
IF (@ParentPageID = 0) 
    SET @ParentPageID = NULL
IF NOT EXISTS
(
    SELECT 
        * 
    FROM 
        rb_Pages
    WHERE 
        PageID = @PageID
)
INSERT INTO rb_Pages (
    PortalID,
    ParentPageID,
    PageOrder,
    PageName,
    AuthorizedRoles,
    MobilePageName,
    ShowMobile,
	FriendlyURL
) 
VALUES (
    @PortalID,
    @PageOrder,
    @ParentPageID,
    @PageName,
    @AuthorizedRoles,
    @MobilePageName,
    @ShowMobile,
	@FriendlyURL
)
ELSE
--Updated 26.Dec.2002 Cory Isakson
--Check the Page recursion so Page is not placed into an infinate loop when building Page Tree
BEGIN TRAN
--If the Update breaks the Page from having a path back to the root then do not Update
UPDATE
    rb_Pages
SET
    ParentPageID = @ParentPageID,
    PageOrder = @PageOrder,
    PageName = @PageName,
    AuthorizedRoles = @AuthorizedRoles,
    MobilePageName = @MobilePageName,
    ShowMobile = @ShowMobile,
	FriendlyURL = @FriendlyURL WHERE
    PageID = @PageID
--Create a Temporary table to hold the Pages
CREATE TABLE #PageTree
(
	[PageID] [int],
	[PageName] [nvarchar] (200),
	[ParentPageID] [int],
	[PageOrder] [int],
	[NestLevel] [int],
	[TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO	#PageTree
SELECT 	PageID,
	PageName,
	ParentPageID,
	PageOrder,
	0,
	cast(100000000 + PageOrder AS varchar)
FROM	rb_Pages
WHERE	ParentPageID IS NULL AND PortalID =@PortalID
ORDER BY PageOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT 	#PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
		SELECT 	rb_Pages.PageID,
			Replicate('-', @LastLevel *2) + rb_Pages.PageName,
			rb_Pages.ParentPageID,
			rb_Pages.PageOrder,
			@LastLevel,
			cast(#PageTree.TreeOrder AS varchar(8000)) + '.' + cast(100000000 + rb_Pages.PageOrder AS varchar)
		FROM	rb_Pages join #PageTree on rb_Pages.ParentPageID= #PageTree.PageID
		WHERE	EXISTS (SELECT 'X' FROM #PageTree WHERE PageID = rb_Pages.ParentPageID AND NestLevel = @LastLevel - 1)
		 AND PortalID =@PortalID
		ORDER BY #PageTree.PageOrder
END
--Check that the Page is found in the Tree.  If it is not then we abort the Update
IF NOT EXISTS (SELECT PageID from #PageTree WHERE PageID=@PageID)
BEGIN
	ROLLBACK TRAN
	--If we want to modify the PageLayout code then we can throw an error AND catch it.
	RAISERROR('Not allowed to choose that parent.',11,1)
END
ELSE
COMMIT TRAN
GO	

ALTER PROCEDURE [dbo].[rb_AddTab]
(
   @PortalID		int,		/* Required Field  */
   @ParentPageID		int,            /*   New Parm - NULL Allowed    */
   @PageName		nvarchar(200),   /* Required Field  */
   @PageOrder		int,            /* Required Field  */ 
   @AuthorizedRoles	nvarchar (256), /* NULL Allowed    */
   @ShowMobile		bit = 0,        /*   New Parm - false by default */
   @MobilePageName	nvarchar(200),   /* Required Field  */
   @PageID		int OUTPUT      /* Returned value */

)
 
AS

  IF (@ParentPageID = 0)
  BEGIN  
    set @ParentPageID = NULL
  END


INSERT INTO rb_Pages
(
   ParentPageID,  /* New parm */
    PageOrder,
    PortalID,
    PageName,
    MobilePageName,
    AuthorizedRoles,
    ShowMobile
    
)


VALUES
(
   @ParentPageID, 
   @PageOrder,
   @PortalID,
   @PageName,
   @MobilePageName,
   @AuthorizedRoles,
   @ShowMobile
    
)

SELECT
    @PageID = @@IDENTITY
GO

ALTER TABLE rb_pages alter column PageName nvarchar(200)
ALTER TABLE rb_pages alter column MobilePageName nvarchar(200)
GO

if EXISTS(select * from rb_PortalSettings where SettingName='SITESETTINGS_DEFAULT_EDITOR' AND SettingValue='Syrinx CkEditor')
BEGIN
update [rb_PortalSettings] set SettingValue='CKeditor' where SettingName='SITESETTINGS_DEFAULT_EDITOR'
END
GO

if EXISTS(select * from rb_ModuleSettings where ModuleID=2)
BEGIN
UPDATE [rb_ModuleSettings] SEt SettingValue='CKeditor' where ModuleID=2
END
GO