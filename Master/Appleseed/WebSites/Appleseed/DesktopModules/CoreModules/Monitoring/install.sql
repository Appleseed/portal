/* Install script, Monitoring, [paul@paulyarrow.com], 06/06/2003 */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Monitoring]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [rb_Monitoring] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserID] [int] NULL ,
	[PortalID] [int] NULL ,
	[PageID] [int] NULL ,
	[ActivityTime] [datetime] NULL ,
	[ActivityType] [varchar] (50) NULL ,
	[Referrer] [varchar] (255) NULL ,
	[UserAgent] [varchar] (100) NULL ,
	[UserHostAddress] [varchar] (15) NULL ,
	[BrowserType] [varchar] (100) NULL ,
	[BrowserName] [varchar] (100) NULL ,
	[BrowserVersion] [varchar] (100) NULL ,
	[BrowserPlatform] [varchar] (100) NULL ,
	[BrowserIsAOL] [bit] NULL,
	[UserField] [varchar] (500) NULL
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT name FROM sysindexes WHERE name = 'IDX_rb_MonitoringGetOnlineUsers')
   DROP INDEX [rb_Monitoring].[IDX_rb_MonitoringGetOnlineUsers]
GO

CREATE NONCLUSTERED INDEX [IDX_rb_MonitoringGetOnlineUsers] ON [rb_Monitoring] ([PortalID] ASC, [ActivityTime] ASC, [UserHostAddress] ASC )
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddMonitoringEntry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddMonitoringEntry]
GO

CREATE PROCEDURE [rb_AddMonitoringEntry]
    @UserID int,
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
)
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetMonitoringEntries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetMonitoringEntries]
GO

CREATE PROCEDURE [rb_GetMonitoringEntries] 
(
    @PortalID			int,
    @StartDate			datetime,
    @EndDate			datetime,
    @ReportType			varchar(50),
    @CurrentTabID		bigint,
    @IPAddress			varchar(16),
    @IncludeMonitorPage		bit,
    @IncludeAdminUser		bit,
    @IncludePageRequests	bit,
    @IncludeLogon		bit,
    @IncludeLogoff		bit,
    @IncludeIPAddress		bit
)
AS

	SET NOCOUNT ON

	DECLARE @SQLSTRING varchar(2000)

	IF (UPPER(@ReportType) = 'DETAILED SITE LOG')
	BEGIN

		SET @SQLSTRING = 'SELECT rbm.ActivityTime, rbm.ActivityType, rbt.TabName, rbu.[Name], rbm.BrowserName, rbm.BrowserVersion, rbm.BrowserPlatform, rbm.UserHostAddress, rbm.UserField ' +
					'FROM rb_Monitoring rbm ' + 
					'LEFT OUTER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID ' +
					'INNER JOIN rb_Portals rbp ON rbm.PortalID = rbp.PortalID ' +
					'LEFT OUTER JOIN rb_Pages rbt ON rbm.PageID = rbt.TabID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS varchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS varchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS varchar)

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS varchar) + ' '
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		SET @SQLSTRING = @SQLSTRING + ' ORDER BY ActivityTime DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE POPULARITY')
	BEGIN

		SET @SQLSTRING = 'SELECT rbt.TabName, ''Requests'' = COUNT(*), ''LastRequest'' = max(ActivityTime) ' +
					'FROM rb_Monitoring rbm ' +
					'INNER JOIN rb_Pages rbt ON rbm.PageID = rbt.TabID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS varchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS varchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS varchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS varchar) + ' '
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		SET @SQLSTRING = @SQLSTRING + ' GROUP BY rbt.TabName ORDER BY Requests DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'MOST ACTIVE USERS')
	BEGIN

		SET @SQLSTRING = 'SELECT rbu.[Name], ''Actions'' = COUNT(*), ''LastAction'' = max(ActivityTime) ' +
					'FROM rb_Monitoring rbm ' +
					'INNER JOIN rb_Users rbu ON rbm.UserID = rbu.UserID ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS varchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS varchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS varchar) + ' '

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS varchar) + ' '
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		SET @SQLSTRING = @SQLSTRING + ' GROUP BY rbu.[Name] ORDER BY Actions DESC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE VIEWS BY DAY')
	BEGIN
		SET @SQLSTRING = 'SELECT ''Date'' = convert(varchar,ActivityTime,102), ''Views'' = COUNT(*), ''Visitors'' = COUNT(DISTINCT UserHostAddress), ''Users'' = COUNT(DISTINCT UserID) ' +
					'FROM rb_Monitoring rbm ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS varchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS varchar) + '''  ' +
					'AND rbm.PortalID = ' + CAST(@PortalID AS varchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS varchar) + ' '
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		SET @SQLSTRING = @SQLSTRING + 'GROUP BY convert(varchar,ActivityTime,102) ORDER BY [Date] ASC'

	END
	ELSE
	IF (UPPER(@ReportType) = 'PAGE VIEWS BY BROWSER TYPE')
	BEGIN

		SET @SQLSTRING = 'SELECT BrowserType, ''Views'' = COUNT(*) ' +
					'FROM rb_Monitoring rbm ' +
					'WHERE ActivityTime >= ''' + CAST(@StartDate AS varchar) + ''' AND ActivityTime <= ''' + CAST(@EndDate AS varchar) + '''  ' + 
					'AND rbm.PortalID = ' + CAST(@PortalID AS varchar) + ' AND rbm.ActivityType=''PageRequest'''

		IF (@IncludeMonitorPage = 0) 
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.PageID != ' + CAST(@CurrentTabID AS varchar) + ' '
		IF (@IncludeAdminUser = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND rbm.UserID != 1 '
		IF (@IncludePageRequests = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''PageRequest'' '
		IF (@IncludeLogon = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logon'' '
		IF (@IncludeLogoff = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND ActivityType != ''Logoff'' '
		IF (@IncludeIPAddress = 0)
			SET @SQLSTRING = @SQLSTRING + 'AND UserHostAddress != ''' + @IPAddress + ''' '

		SET @SQLSTRING = @SQLSTRING + 'GROUP BY BrowserType ORDER BY [Views] DESC'

	END

	exec (@SQLSTRING)
GO
