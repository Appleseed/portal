/* Install script, Monitoring, [paul@paulyarrow.com], 06/06/2003 */

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{3B8E3585-58B7-4f56-8AB6-C04A2BFA6589}'
SET @FriendlyName = 'Monitoring'
SET @DesktopSrc = 'DesktopModules/Monitoring/Monitoring.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesMonitoring'
SET @Admin = 0
SET @Searchable = 0

IF NOT EXISTS (SELECT GeneralModDefID FROM rb_GeneralModuleDefinitions
WHERE GeneralModDefID = @GeneralModDefID)
BEGIN
	-- Installs module
	EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

	-- Install it for default portal
	EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Monitoring]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
begin

CREATE TABLE [rb_Monitoring] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserID] [int] NULL ,
	[PortalID] [int] NULL ,
	[PageID] [int] NULL ,
	[ActivityTime] [datetime] NULL ,
	[ActivityType] [nvarchar] (50) NULL ,
	[Referrer] [nvarchar] (255) NULL ,
	[UserAgent] [nvarchar] (100) NULL ,
	[UserHostAddress] [nvarchar] (15) NULL ,
	[BrowserType] [nvarchar] (100) NULL ,
	[BrowserName] [nvarchar] (100) NULL ,
	[BrowserVersion] [nvarchar] (100) NULL ,
	[BrowserPlatform] [nvarchar] (100) NULL ,
	[BrowserIsAOL] [bit] NULL,
	[UserField] [nvarchar] (500) NULL
) ON [PRIMARY]

end
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddMonitoringEntry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddMonitoringEntry]
GO

CREATE PROCEDURE [rb_AddMonitoringEntry]
    @UserID int,
    @PortalID int,
    @PageID int,
    @ActivityType nvarchar(50),
    @Referrer nvarchar(255),
    @UserAgent nvarchar(100),
    @UserHostAddress nvarchar(15),
    @BrowserType nvarchar(100),
    @BrowserName nvarchar(100),
    @BrowserVersion nvarchar(100),
    @BrowserPlatform nvarchar(100),
    @BrowserIsAOL bit,
    @UserField nvarchar(500)
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
    GetDate(),
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
)
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetMonitoringEntries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetMonitoringEntries]
GO

CREATE PROCEDURE [rb_GetMonitoringEntries] 
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

	IF (UPPER(@ReportType) = 'DETAILED SITE LOG')
	BEGIN

		set @SQLSTRING = 'SELECT rbm.ActivityTime, rbm.ActivityType, rbt.TabName, rbu.[Name], rbm.BrowserName, rbm.BrowserVersion, rbm.BrowserPlatform, rbm.UserHostAddress, rbm.UserField ' +
					'FROM rb_Monitoring rbm ' + 
					'LEFT OUTER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID ' +
					'INNER JOIN rb_Portals rbp ON rbm.PortalID = rbp.PortalID ' +
					'LEFT OUTER JOIN rb_Tabs rbt ON rbm.PageID = rbt.TabID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS nvarchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS nvarchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS nvarchar)

		IF (@IncludeMonitorPage = 0) 
			set @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS nvarchar) + ' '
		IF (@IncludeAdminUser = 0)
			set @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			set @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		set @SQLSTRING = @SQLSTRING + ' ORDER BY ActivityTime DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE POPULARITY')
	BEGIN

		set @SQLSTRING = 'SELECT rbt.TabName, ''Requests'' = count(*), ''LastRequest'' = max(ActivityTime) ' +
					'FROM rb_Monitoring rbm ' +
					'INNER JOIN rb_Tabs rbt ON rbm.PageID = rbt.TabID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS nvarchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS nvarchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS nvarchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			set @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS nvarchar) + ' '
		IF (@IncludeAdminUser = 0)
			set @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			set @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		set @SQLSTRING = @SQLSTRING + ' GROUP BY rbt.TabName ORDER BY Requests DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'MOST ACTIVE USERS')
	BEGIN

		set @SQLSTRING = 'SELECT rbu.[Name], ''Actions'' = count(*), ''LastAction'' = max(ActivityTime) ' +
					'FROM rb_Monitoring rbm ' +
					'INNER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS nvarchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS nvarchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS nvarchar) + ' '

		IF (@IncludeMonitorPage = 0) 
			set @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS nvarchar) + ' '
		IF (@IncludeAdminUser = 0)
			set @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			set @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		set @SQLSTRING = @SQLSTRING + ' GROUP BY rbu.[Name] ORDER BY Actions DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE VIEWS BY DAY')
	BEGIN
		set @SQLSTRING = 'SELECT ''Date'' = convert(varchar,ActivityTime,102), ''Views'' = count(*), ''Visitors'' = count(distinct UserHostAddress), ''Users'' = count(distinct UserID) ' +
					'FROM rb_Monitoring rbm ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS nvarchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS nvarchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS nvarchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			set @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS nvarchar) + ' '
		IF (@IncludeAdminUser = 0)
			set @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			set @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		set @SQLSTRING = @SQLSTRING + 'GROUP BY convert(varchar,ActivityTime,102) ORDER BY [Date] ASC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE VIEWS BY BROWSER TYPE')
	BEGIN

		set @SQLSTRING = 'SELECT BrowserType, ''Views'' = count(*) ' +
					'FROM rb_Monitoring rbm ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS nvarchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS nvarchar) + '''  ' + 
					'AND rbm.PortalID = ' + CAST(@PortalID AS nvarchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			set @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS nvarchar) + ' '
		IF (@IncludeAdminUser = 0)
			set @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			set @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			set @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		set @SQLSTRING = @SQLSTRING + 'GROUP BY BrowserType ORDER BY [Views] DESC'

	END

	exec (@SQLSTRING)
GO

/* Install script, WhosLoggedOn module, [paul@paulyarrow.com], 16/07/2003 */


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetLoggedOnUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetLoggedOnUsers]
GO

CREATE PROCEDURE rb_GetLoggedOnUsers
(
    @PortalID    int,
    @MinutesToCheck int
)
AS

	select distinct rbm.UserHostAddress, rbu.[Name], (select top 1 ActivityType
		from rb_Monitoring
		WHERE (ActivityType = 'Logon' or ActivityType = 'Logoff')
		AND ActivityTime >= DATEADD(n, -@MinutesToCheck, GetDate())
		AND UserHostAddress = rbm.UserHostAddress
		AND UserID = rbm.UserID
		AND rbm.PortalID = @PortalID
		order by ActivityTime desc) as LastAction
		from rb_Monitoring rbm
		inner join rb_Users rbu ON rbm.UserID = rbu.UserID
		WHERE (rbm.ActivityType = 'Logon' or rbm.ActivityType = 'Logoff') 
		AND rbm.ActivityTime >= DATEADD(n, -@MinutesToCheck, GetDate())
		AND rbm.PortalID = @PortalID
GO



IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetNumberOfActiveUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetNumberOfActiveUsers]
GO

CREATE PROCEDURE rb_GetNumberOfActiveUsers
(
    @PortalID    int,
    @MinutesToCheck int,
    @NoOfUsers int output
)
AS

	select @NoOfUsers =  Count(distinct UserHostAddress)
	from rb_Monitoring
	WHERE ActivityTime >= DATEADD(n, -@MinutesToCheck, GetDate())
	AND PortalID = @PortalID
GO

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{52AD3A51-121D-48bc-9782-02076E0D6A69}'
SET @FriendlyName = 'Whos Logged On'
SET @DesktopSrc = 'DesktopModules/WhosLoggedOn/WhosLoggedOn.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesWhosLoggedOn'
SET @Admin = 0
SET @Searchable = 0

IF NOT EXISTS (SELECT GeneralModDefID FROM rb_GeneralModuleDefinitions
WHERE GeneralModDefID = @GeneralModDefID)
BEGIN
	-- Installs module
	EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

	-- Install it for default portal
	EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
END
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1732','1.2.8.1732', CONVERT(datetime, '07/31/2003', 101))
GO