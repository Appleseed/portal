if exists (select * from sysobjects where id = object_id(N'[rb_Monitoring]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table rb_Monitoring
go

create table rb_Monitoring(
  ID              int              not null identity,
  UserID          uniqueidentifier,
  PortalID        int,
  PageID          int,
  ActivityTime    datetime,
  ActivityType    varchar(50),
  Referrer        varchar(255),
  UserAgent       varchar(100),
  UserHostAddress varchar(15),
  BrowserType     varchar(100),
  BrowserName     varchar(100),
  BrowserVersion  varchar(100),
  BrowserPlatform varchar(100),
  BrowserIsAOL    bit,
  UserField       varchar(500)
)
go

create index IDX_rb_MonitoringGetOnlineUsers on rb_Monitoring(PortalID,ActivityTime,UserHostAddress)
go

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddMonitoringEntry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure rb_AddMonitoringEntry
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_AddMonitoringEntry
    @UserID uniqueidentifier,
    @PortalID int,
    @PageID int,
    @ActivityType varchar(50),
    @Referrer varchar(255),
    @UserAgent varchar(100),
    @UserHostAddress varchar(15),
    @BrowserType varchar(100),
    @BrowserName varchar(100),
    @BrowserVersion varchar(100),
    @BrowserPlatform varchar(100),
    @BrowserIsAOL bit,
    @UserField varchar(500)
AS

INSERT INTO rb_Monitoring
(
    UserID,
    PortalID,
    PageID,
    ActivityTime,
    ActivityType,
    Referrer,
    UserAgent,
    UserHostAddress,
    BrowserType,
    BrowserName,
    BrowserVersion,
    BrowserPlatform,
    BrowserIsAOL,
    UserField
)
VALUES
(
    @UserID,
    @PortalID,
    @PageID,
    GETDATE(),
    @ActivityType,
    @Referrer,
    @UserAgent,
    @UserHostAddress,
    @BrowserType,
    @BrowserName,
    @BrowserVersion,
    @BrowserPlatform,
    @BrowserIsAOL,
    @UserField
)'
go


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetLoggedOnUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure rb_GetLoggedOnUsers
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetLoggedOnUsers
(
    @PortalID    int,
    @MinutesToCheck int
)
AS
		SELECT DISTINCT rbm.UserHostAddress, rbm.UserID,
                          (
                          SELECT TOP 1 ActivityType
                          FROM rb_Monitoring
                          WHERE ActivityTime >= DATEADD(n, - @MinutesToCheck, GETDATE()) 
                          AND UserHostAddress = rbm.UserHostAddress 
                          AND UserID = rbm.UserID 
                          AND rbm.PortalID = @PortalID
                          ORDER BY ActivityTime DESC) 
                          AS LastAction
                          FROM  rb_Monitoring rbm
                          WHERE (rbm.ActivityTime >= DATEADD(n, - @MinutesToCheck, GETDATE())) 
                          AND (rbm.PortalID = @PortalID)'
go



create table rb_Pages(
  PageID          int           not null identity constraint PK_rb_Pages primary key nonclustered,
  ParentPageID    int,
  PageOrder       int           not null,
  PortalID        int           not null,
  PageName        nvarchar(50)  not null,
  MobilePageName  nvarchar(50)  not null,
  AuthorizedRoles nvarchar(512),
  ShowMobile      bit           not null,
  PageLayout      int,
  PageDescription nvarchar(512) not null constraint DF_rb_Tabs_PageDescription default ('')
)
go

alter table rb_Pages add
  constraint FK_rb_Pages_rb_Portals foreign key(PortalID) references rb_Portals(PortalID) on delete cascade,
  constraint FK_rb_Pages_rb_Pages foreign key(ParentPageID) references rb_Pages(PageID)
go

SET IDENTITY_INSERT rb_Pages ON
INSERT INTO rb_Pages([PageID]
		   ,[ParentPageID]
		   ,[PageOrder]
           ,[PortalID]
           ,[PageName]
           ,[MobilePageName]
           ,[AuthorizedRoles]
           ,[ShowMobile]
           ,[PageLayout])
SELECT 		[TabID]
		   ,[ParentTabID]
		   ,[TabOrder]
           ,[PortalID]
           ,[TabName]
           ,[MobileTabName]
           ,[AuthorizedRoles]
           ,[ShowMobile]
           ,[TabLayout] FROM rb_Tabs order by tabid

SET IDENTITY_INSERT rb_Pages OFF
go

alter table rb_Modules drop
  constraint FK_rb_Modules_rb_Tabs 
go

alter table rb_Modules add
  constraint FK_rb_Modules_rb_Pages foreign key(TabID) references rb_Pages(PageID) on delete cascade
go

alter table rb_TabSettings drop
  constraint FK_rb_Tabsettings_rb_Tabs 
go

drop table rb_Tabs
go

alter table rb_TabSettings add
  constraint FK_rb_Tabsettings_rb_Pages foreign key(TabID) references rb_Pages(PageID) on delete cascade
go


----- changes in stored procedures

drop procedure rb_AddTab
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_AddTab
(
   @PortalID		int,		/* Required Field  */
   @ParentPageID		int,            /*   New Parm - NULL Allowed    */
   @PageName		nvarchar(50),   /* Required Field  */
   @PageOrder		int,            /* Required Field  */ 
   @AuthorizedRoles	nvarchar (256), /* NULL Allowed    */
   @ShowMobile		bit = 0,        /*   New Parm - false by default */
   @MobilePageName	nvarchar(50),   /* Required Field  */
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
    @PageID = @@IDENTITY'
go


drop procedure rb_DeleteTab
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_DeleteTab
(
    @PageID int
)
AS
DELETE FROM
    rb_Pages
WHERE
    PageID = @PageID'
go

drop procedure rb_GetAuthAddRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthAddRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @AddRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @AddRoles   = rb_Modules.AuthorizedAddRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthApproveRoles
go

EXEC dbo.sp_executesql @statement = N'create  PROCEDURE rb_GetAuthApproveRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ApproveRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @ApproveRoles   = rb_Modules.AuthorizedApproveRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthDeleteModuleRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthDeleteModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @DeleteModuleRoles  = rb_Modules.AuthorizedDeleteModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthDeleteRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthDeleteRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @DeleteRoles   = rb_Modules.AuthorizedDeleteRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthDeleteRolesRecycler
go

drop procedure rb_GetAuthEditRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthEditRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @EditRoles   = rb_Modules.AuthorizedEditRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthMoveModuleRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthMoveModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @MoveModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @MoveModuleRoles  = rb_Modules.AuthorizedMoveModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthPropertiesRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthPropertiesRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PropertiesRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @PropertiesRoles   = rb_Modules.AuthorizedPropertiesRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthPublishingRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthPublishingRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PublishingRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @PublishingRoles   = rb_Modules.AuthorizedPublishingRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go

drop procedure rb_GetAuthViewRoles
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetAuthViewRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Pages.AuthorizedRoles,
    @ViewRoles   = rb_Modules.AuthorizedViewRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Pages ON rb_Modules.TabID = rb_Pages.PageID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Pages.PortalID = @PortalID'
go


drop procedure rb_GetModulesAllPortals
go

--Fix on Shortuctall module, shortcuts should not be displayed on rb_GetModulesAllPortals list
create PROCEDURE rb_GetModulesAllPortals
AS
SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' AS PortalAlias, -1 AS TabOrder
UNION
	SELECT     rb_Modules.ModuleID, rb_Portals.PortalAlias + '/' + rb_Pages.PageName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, rb_Pages.PageOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Pages ON rb_Modules.TabID = rb_Pages.PageID INNER JOIN
	                      rb_Portals ON rb_Pages.PortalID = rb_Portals.PortalID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Modules.ModuleID > 0) AND (rb_GeneralModuleDefinitions.Admin = 0) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
ORDER BY PortalAlias, ModuleTitle
go

drop procedure rb_GetModulesByName
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetModulesByName
(
	@ModuleName varchar(128),
	@PortalID int
)
AS
SELECT      0, '' Nessun modulo''
UNION
SELECT     rb_Modules.ModuleID, rb_Modules.ModuleTitle
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
                      rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.FriendlyName = @ModuleName)
ORDER BY 2'
go

drop procedure rb_GetModuleSettingsForIndividualModule
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetModuleSettingsForIndividualModule
    @ModuleID  int
AS
BEGIN
/* Get the DataTable of module info */
SELECT     TOP 1 *
FROM	rb_Modules INNER JOIN
	rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.ModuleID = @ModuleID)
END'
go

drop procedure rb_GetModulesInRecycler
go

drop procedure rb_GetModulesSinglePortal
go

create PROCEDURE rb_GetModulesSinglePortal
(
    @PortalID  int
)
AS
SELECT      0 ModuleID, 'NO_MODULE' ModuleTitle, -1 AS TabOrder
UNION
	SELECT     rb_Modules.ModuleID, rb_Tabs.TabName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')' AS ModTitle, rb_Tabs.TabOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Tabs.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
	ORDER BY TabOrder, rb_Modules.ModuleTitle
go

drop procedure rb_GetMonitoringEntries
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetMonitoringEntries 
(
    @PortalID			int,
    @StartDate			datetime,
    @EndDate			datetime,
    @ReportType			nvarchar(50),
    @CurrentTabID		bigint,
    @IPAddress			nvarchar(16),
    @IncludeMonitorPage		bit,
    @IncludeAdminUser		bit,
    @IncludePageRequests	bit,
    @IncludeLogon		bit,
    @IncludeLogoff		bit,
    @IncludeIPAddress		bit
)
AS

	SET NOCOUNT ON

	DECLARE @SQLSTRING nvarchar(2000)

	IF (UPPER(@ReportType) = ''DETAILED SITE LOG'')
	BEGIN

		SET @SQLSTRING = ''SELECT rbm.ActivityTime, rbm.ActivityType, rbt.PageName, rbu.[Name], rbm.BrowserName, rbm.BrowserVersion, rbm.BrowserPlatform, rbm.UserHostAddress, rbm.UserField '' +
					''FROM rb_Monitoring rbm '' + 
					''LEFT OUTER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID '' +
					''INNER JOIN rb_Portals rbp ON rbm.PortalID = rbp.PortalID '' +
					''LEFT OUTER JOIN rb_Pages rbt ON rbm.PageID = rbt.PageID '' +
					''WHERE ActivityTime >= '''''' + CAST(@StartDate AS nvarchar) + '''''' AND ActivityTime <= '''''' + CAST(@EndDate AS nvarchar) + ''''''  '' +
					''AND rbm.PortalID = '' + CAST(@PortalID AS nvarchar)

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.PageID != '' + CAST(@CurrentTabID AS nvarchar) + '' ''
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.UserID != 1 ''
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''PageRequest'''' ''
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logon'''' ''
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logoff'''' ''
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND UserHostAddress != '''''' + @IPAddress + '''''' ''

		SET @SQLSTRING = @SQLSTRING + '' ORDER BY ActivityTime DESC''

	END
	ELSE
	IF (UPPER(@ReportType) = ''PAGE POPULARITY'')
	BEGIN

		SET @SQLSTRING = ''SELECT rbt.PageName, ''''Requests'''' = COUNT(*), ''''LastRequest'''' = max(ActivityTime) '' +
					''FROM rb_Monitoring rbm '' +
					''INNER JOIN rb_Pages rbt ON rbm.PageID = rbt.PageID '' +
					''WHERE ActivityTime >= '''''' + CAST(@StartDate AS nvarchar) + '''''' AND ActivityTime <= '''''' + CAST(@EndDate AS nvarchar) + ''''''  '' +
					''AND rbm.PortalID = '' + CAST(@PortalID AS nvarchar) + '' AND rbm.ActivityType=''''PageRequest''''''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.PageID != '' + CAST(@CurrentTabID AS nvarchar) + '' ''
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.UserID != 1 ''
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''PageRequest'''' ''
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logon'''' ''
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logoff'''' ''
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND UserHostAddress != '''''' + @IPAddress + '''''' ''

		SET @SQLSTRING = @SQLSTRING + '' GROUP BY rbt.PageName ORDER BY Requests DESC''

	END
	ELSE
	IF (UPPER(@ReportType) = ''MOST ACTIVE USERS'')
	BEGIN

		SET @SQLSTRING = ''SELECT rbu.[Name], ''''Actions'''' = COUNT(*), ''''LastAction'''' = max(ActivityTime) '' +
					''FROM rb_Monitoring rbm '' +
					''INNER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID '' +
					''WHERE ActivityTime >= '''''' + CAST(@StartDate AS nvarchar) + '''''' AND ActivityTime <= '''''' + CAST(@EndDate AS nvarchar) + ''''''  '' +
					''AND rbm.PortalID = '' + CAST(@PortalID AS nvarchar) + '' ''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.PageID != '' + CAST(@CurrentTabID AS nvarchar) + '' ''
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.UserID != 1 ''
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''PageRequest'''' ''
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logon'''' ''
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logoff'''' ''
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND UserHostAddress != '''''' + @IPAddress + '''''' ''

		SET @SQLSTRING = @SQLSTRING + '' GROUP BY rbu.[Name] ORDER BY Actions DESC''

	END
	ELSE
	IF (UPPER(@ReportType) = ''PAGE VIEWS BY DAY'')
	BEGIN
		SET @SQLSTRING = ''SELECT ''''Date'''' = convert(varchar,ActivityTime,102), ''''Views'''' = COUNT(*), ''''Visitors'''' = COUNT(DISTINCT UserHostAddress), ''''Users'''' = COUNT(DISTINCT UserID) '' +
					''FROM rb_Monitoring rbm '' +
					''WHERE ActivityTime >= '''''' + CAST(@StartDate AS nvarchar) + '''''' AND ActivityTime <= '''''' + CAST(@EndDate AS nvarchar) + ''''''  '' +
					''AND rbm.PortalID = '' + CAST(@PortalID AS nvarchar) + '' AND rbm.ActivityType=''''PageRequest''''''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.PageID != '' + CAST(@CurrentTabID AS nvarchar) + '' ''
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.UserID != 1 ''
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''PageRequest'''' ''
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logon'''' ''
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logoff'''' ''
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND UserHostAddress != '''''' + @IPAddress + '''''' ''

		SET @SQLSTRING = @SQLSTRING + ''GROUP BY convert(varchar,ActivityTime,102) ORDER BY [Date] ASC''

	END
	ELSE
	IF (UPPER(@ReportType) = ''PAGE VIEWS BY BROWSER TYPE'')
	BEGIN

		SET @SQLSTRING = ''SELECT BrowserType, ''''Views'''' = COUNT(*) '' +
					''FROM rb_Monitoring rbm '' +
					''WHERE ActivityTime >= '''''' + CAST(@StartDate AS nvarchar) + '''''' AND ActivityTime <= '''''' + CAST(@EndDate AS nvarchar) + ''''''  '' + 
					''AND rbm.PortalID = '' + CAST(@PortalID AS nvarchar) + '' AND rbm.ActivityType=''''PageRequest''''''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.PageID != '' + CAST(@CurrentTabID AS nvarchar) + '' ''
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND rbm.UserID != 1 ''
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''PageRequest'''' ''
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logon'''' ''
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND ActivityType != ''''Logoff'''' ''
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + ''AND UserHostAddress != '''''' + @IPAddress + '''''' ''

		SET @SQLSTRING = @SQLSTRING + ''GROUP BY BrowserType ORDER BY [Views] DESC''

	END

	exec (@SQLSTRING)'
go

drop procedure rb_GetPagesParentTabID
go

EXEC dbo.sp_executesql @statement = N'create  PROCEDURE rb_GetPagesParentTabID
(
	@PortalID int,
	@PageID int
)
AS

Select ParentPageID
From rb_Pages
Where PageID = @PageID And PortalID = @PortalID'
go

drop procedure rb_GetPageTree
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetPageTree
(
	@PortalID int
)
AS
--drop table #tree
-- Get the hierarchy
create table #tree (id int, sequence varchar(1000), levelNo int, PageOrder int)



-- insert top level (to get sub tree just insert relevent id here)
insert #tree 
select PageID, (convert(varchar(10),Len(rb_Pages.PageOrder)) + ''.'' + convert(varchar(10),rb_Pages.PageOrder)), 1, PageOrder
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
	#tree.sequence + ''.''+ convert(varchar(10),Len(rb_Pages.PageOrder)) + ''.'' + convert(varchar(10),rb_Pages.PageOrder), 
	@i + 1, 
	rb_Pages.PageOrder
     from rb_Pages, #tree 
     where #tree.levelNo = @i
	     and rb_Pages.ParentPageID = #tree.id
     Order BY rb_Pages.PageOrder
end

-- output with hierarchy formatted
select rb_Pages.PageID, 
	rb_Pages.ParentPageID, 
	rb_Pages.PageOrder, 
	rb_Pages.PageName, 
	#tree.levelNo , 
	Replicate(''-'', (#tree.levelNo) * 2) + rb_Pages.PageName as PageOrder
	--, #tree.sequence
from #tree, rb_Pages
where #tree.id = rb_Pages.PageID
order by #tree.sequence--, rb_Pages.PageOrder

drop table #tree
--drop table #z'
go

--drop procedure rb_GetPicturesPaged
--go

drop procedure rb_GetPortalSettings
go

--Manu - Created 25/06/2003
--Manu fixed Page Name on active Page for localized items - 13/10/2203
EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetPortalSettings
(
    @PortalAlias   nvarchar(50),
    @PageID         int,
    @PortalLanguage nvarchar(12),
    @PortalID      int OUTPUT,
    @PortalName    nvarchar(128) OUTPUT,
    @PortalPath    nvarchar(128) OUTPUT,
    @AlwaysShowEditButton bit OUTPUT,
    @PageName       nvarchar (50)  OUTPUT,
    @PageOrder      int OUTPUT,
    @ParentPageID      int OUTPUT,
    @MobilePageName nvarchar (50)  OUTPUT,
    @AuthRoles     nvarchar (256) OUTPUT,
    @ShowMobile    bit OUTPUT
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
        @ShowMobile    = rb_Pages.ShowMobile
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
        @ShowMobile    = rb_Pages.ShowMobile
    FROM
        rb_Pages
    INNER JOIN
        rb_Portals ON rb_Pages.PortalID = rb_Portals.PortalID
        
    WHERE
        PageID=@PageID AND rb_Portals.PortalAlias=@PortalAlias
/* Get Pages list */
SELECT     COALESCE ((SELECT     SettingValue
                              FROM         rb_TabSettings
                              WHERE     TabID = rb_Pages.PageID AND SettingName = @PortalLanguage AND Len(SettingValue) > 0), PageName) AS PageName, AuthorizedRoles, PageID, ParentPageID, PageOrder, 
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
    ShowMobile  
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
ORDER BY rb_Modules.ModuleOrder'
go


drop procedure rb_GetTabCrumbs
go

create  proc rb_GetTabCrumbs
@PageID int,
@CrumbsXML nvarchar (4000) output
AS
--Variables used to build Crumb XML string
declare @ParentPageID int
declare @PageName AS nvarchar(50)
declare @Level int
--First Child in the branch is Crumb 20.  
set @Level =20
--Get First Parent Page ID if there is one
set @ParentPageID = (select ParentPageID from rb_Pages WHERE PageID=@PageID)
--Get PageName of Lowest Child
set @PageName = (select PageName from rb_Pages WHERE PageID=@PageID)
--Build first Crumb
set @CrumbsXML = '<root><crumb TabID=''' + cast(@PageID AS varchar) + ''' Level=''' + cast(@Level AS varchar) + '''>' + @PageName + '</crumb>'
while @ParentPageID is not null
	begin
		set @Level=@Level - 1
		set @PageID=@ParentPageID
		set @ParentPageID=(select ParentPageID from rb_Pages WHERE PageID=@PageID)
		set @PageName = (select PageName from rb_Pages WHERE PageID=@PageID)
		set @CrumbsXML = @CrumbsXML + '<crumb TabID=''' + cast(@PageID AS varchar) + ''' Level=''' + cast(@Level AS varchar) + '''>' + @PageName + '</crumb>'
	end
set @CrumbsXML = @CrumbsXML + '</root>'
go

drop procedure rb_GetTabsByPortal
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetTabsByPortal
(
    @PortalID   int
)
AS
/* Get Tabs list */
SELECT     PageName, AuthorizedRoles, PageID, PageOrder, ParentPageID, MobilePageName, ShowMobile, PortalID
FROM         rb_Pages
WHERE     (PortalID = @PortalID)
ORDER BY PageOrder'
go

drop procedure rb_GetTabSettings
go

--Fix by Onur Esnaf
EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetTabSettings
(
    @PageID   int,
    @PortalLanguage nvarchar(12)
)
AS
 
IF (@PageID > 0)
 
/* Get Pages list */
SELECT     
   COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Pages.PageID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   PageName)  AS 
 PageName, 
 AuthorizedRoles, 
 PageID, 
 PageOrder, 
 ParentPageID, 
 MobilePageName, 
 ShowMobile, 
 PortalID
FROM         rb_Pages
WHERE     (ParentPageID = @PageID)
ORDER BY PageOrder'
go

drop procedure rb_GetTabsFlat
go

create PROCEDURE rb_GetTabsFlat
(
        @PortalID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #PageTree
(
        [PageID] [int],
        [PageName] [nvarchar] (100),
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
go

drop procedure rb_GetTabsinTab
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_GetTabsinTab
(
	@PortalID int,
	@PageID int
)
AS
SELECT     PageID, PageName, ParentPageID, PageOrder, AuthorizedRoles
FROM         rb_Pages
WHERE     (ParentPageID = @PageID) AND (PortalID = @PortalID)
ORDER BY PageOrder'
go

drop procedure rb_GetTabsParent
go

create PROCEDURE rb_GetTabsParent
(
	@PortalID int,
	@PageID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #PageTree
(
        [PageID] [int],
        [PageName] [nvarchar] (100),
        [ParentPageID] [int],
        [PageOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO     #PageTree
SELECT  PageID,
        PageName,
        ParentPageID,
        PageOrder,
        0,
        cast(100000000 + PageOrder AS varchar)
FROM    rb_Pages
WHERE   ParentPageID IS NULL AND PortalID =@PortalID
ORDER BY PageOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #PageTree (PageID, PageName, ParentPageID, PageOrder, NestLevel, TreeOrder) 
                SELECT  rb_Pages.PageID,
                        Replicate('-', @LastLevel *2) + rb_Pages.PageName,
                        rb_Pages.ParentPageID,
                        rb_Pages.PageOrder,
                        @LastLevel,
                        cast(#PageTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Pages.PageOrder AS varchar)
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
-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) AS ord , cast(PageID AS varchar) AS PageID into #Pages
from #PageTree
order by NestLevel, TreeOrder
-- Change the PageOrder in the sirt temp table so that tabs are ordered in sequence
update #PageTree set PageOrder=(select ord from #Pages WHERE cast(#Pages.PageID AS int)=#PageTree.PageID) 
-- Return Temporary Table
SELECT PageID, PageName, TreeOrder
FROM #PageTree 
UNION
SELECT 0 PageID, ' ROOT_LEVEL' PageName, '-1' AS TreeOrder
order by TreeOrder
go



drop procedure rb_UpdateTab
go

--Update Stored PROCEDURE: rb_UpdateTab
--Prevents orphaning a Page or placing Pages in an infinte recursive loop
create PROCEDURE rb_UpdateTab
(
    @PortalID        int,
    @PageID           int,
    @ParentPageID     int,
    @PageOrder        int,
    @PageName         nvarchar(50),
    @AuthorizedRoles nvarchar(256),
    @MobilePageName   nvarchar(50),
    @ShowMobile      bit
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
    ShowMobile
) 
VALUES (
    @PortalID,
    @PageOrder,
    @ParentPageID,
    @PageName,
    @AuthorizedRoles,
    @MobilePageName,
    @ShowMobile
   
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
    ShowMobile = @ShowMobile
WHERE
    PageID = @PageID
--Create a Temporary table to hold the Pages
CREATE TABLE #PageTree
(
	[PageID] [int],
	[PageName] [nvarchar] (100),
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
go

drop procedure rb_UpdateTabOrder
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_UpdateTabOrder
(
    @PageID           int,
    @PageOrder        int
)
AS
UPDATE
    rb_Pages
SET
    PageOrder = @PageOrder
WHERE
    PageID = @PageID'
go

drop procedure rb_UpdateTabParent
go

EXEC dbo.sp_executesql @statement = N'create PROCEDURE rb_UpdateTabParent
(
    @PortalID        int,
    @PageID           int,
    @ParentPageID     int
)
AS
IF (@ParentPageID = 0) SET @ParentPageID = NULL

UPDATE
    rb_Pages
SET
    ParentPageID = @ParentPageID
WHERE
    PageID = @PageID'
go
