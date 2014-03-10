-----------------------
----1.2.8.1505_00.sql
-----------------------

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--DROP TABLE [rb_Versions]
--GO

--CREATE TABLE [rb_Versions] (
--	[Release] [int] NOT NULL ,
--	[Version] [nvarchar] (50) NULL ,
--	[ReleaseDate] [datetime] NULL 
--) ON [PRIMARY]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddMessage]
--GO

--CREATE PROCEDURE AddMessage
--(
--    @ItemID int OUTPUT,
--    @Title nvarchar(100),
--    @Body nvarchar(3000),
--    @ParentID int,
--    @UserName nvarchar(100),
--    @ModuleID int
--)   

--AS 

--/* Find DisplayOrder of parent item */
--DECLARE @ParentDisplayOrder as nvarchar(750)

--SET @ParentDisplayOrder = ''

--SELECT 
--    @ParentDisplayOrder = DisplayOrder
--FROM 
--    Discussion 
--WHERE 
--    ItemID = @ParentID

--INSERT INTO Discussion
--(
--    Title,
--    Body,
--    DisplayOrder,
--    CreatedDate, 
--    CreatedByUser,
--    ModuleID
--)
--VALUES
--(
--    @Title,
--    @Body,
--    @ParentDisplayOrder + CONVERT( nvarchar(24), GetDate(), 21 ),
--    GetDate(),
--    @UserName,
--    @ModuleID
--)

--SELECT 
--    @ItemID = @@IDENTITY
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabsParent]
--GO

--CREATE PROCEDURE GetTabsParent
--(
--	@PortalID int,
--	@TabID int
--)
--AS
--SELECT 0 TabID, ' ROOT_LEVEL' TabName

--UNION

--SELECT     Tabs.TabID, Tabs.TabName
--FROM       Tabs
--WHERE     (Tabs.TabID <> @TabID) AND (Tabs.PortalID = @PortalID)
--ORDER BY Tabs.TabName
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesSinglePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModulesSinglePortal]
--GO

--CREATE PROCEDURE GetModulesSinglePortal
--(
--    @PortalID  int
--)
--AS

--SELECT      0 ModuleID, 'NO_MODULE' ModuleTitle, -1 as TabOrder

--UNION

--	SELECT     Modules.ModuleID, Tabs.TabName + '/' + Modules.ModuleTitle + ' (' + GeneralModuleDefinitions.FriendlyName + ')' AS ModTitle, Tabs.TabOrder
--	FROM         Modules INNER JOIN
--	                      Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
--	                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--	                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--	WHERE     (Tabs.PortalID = @PortalID) AND (GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
--	                      GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
--	ORDER BY TabOrder, Modules.ModuleTitle
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModulesAllPortals]
--GO

--CREATE PROCEDURE GetModulesAllPortals
--AS

--SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' as PortalAlias, -1 as TabOrder

--UNION

--	SELECT     Modules.ModuleID, Portals.PortalAlias + '/' + Tabs.TabName + '/' + Modules.ModuleTitle + ' (' + GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, Tabs.TabOrder
--	FROM         Modules INNER JOIN
--	                      Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
--	                      Portals ON Tabs.PortalID = Portals.PortalID INNER JOIN
--	                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--	                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--	WHERE     (Modules.ModuleID > 0) AND (GeneralModuleDefinitions.Admin = 0)

--ORDER BY PortalAlias, Modules.ModuleTitle
--GO

----Search module patch, 16 dec. 2002
----Jakob Hansen, hansen3000@hotmail.com
----Modified by manu
----This patch add entries on db for SEARCH module
--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531010}')
--BEGIN
----Insert data into GeneralModuleDefinitions
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{2502DB18-B580-4F90-8CB4-C15E6E531010}',NULL,'Search','DesktopModules/Search.ascx','',0)
----Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0','{2502DB18-B580-4F90-8CB4-C15E6E531010}')
--END
--GO

----This patch add entries on db for HTML v2 module
----Jakob Hansen, hansen3000@hotmail.com
----Modified by manu
--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions WHERE GeneralModDefID = '{2B113F51-FEA3-499A-98E7-7B83C192FDBB}')
--BEGIN
----Insert data into GeneralModuleDefinitions
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{2B113F51-FEA3-499A-98E7-7B83C192FDBB}',NULL,'Html Editor WYSIWYG','DesktopModules/HtmlModuleV2.ascx','',0)
----Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0','{2B113F51-FEA3-499A-98E7-7B83C192FDBB}')
--END
--GO

----SQL Update for GetTabsFlat Stored PROCEDURE
---- by Cory Isakson on 17/12/2002
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsFlat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabsFlat]
--GO

--CREATE PROCEDURE GetTabsFlat
--(
--        @PortalID int
--)

--AS
----Create a Temporary table to hold the tabs for this query
--CREATE TABLE #TabTree
--(
--        [TabID] [int],
--        [TabName] [nvarchar] (50),
--        [ParentTabID] [int],
--        [TabOrder] [int],
--        [NestLevel] [int],
--        [TreeOrder] [varchar] (1000)
--)

--SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
--DECLARE @LastLevel smallint
--SET @LastLevel = 0

---- First, the parent levels
--INSERT INTO     #TabTree
--SELECT  TabID,
--        TabName,
--        ParentTabID,
--        TabOrder,
--        0,
--        cast(100000000 + TabOrder as varchar)

--FROM    Tabs
--WHERE   ParentTabID IS NULL AND PortalID =@PortalID
--ORDER BY TabOrder

---- Next, the children levels
--WHILE (@@rowcount > 0)
--BEGIN
--  SET @LastLevel = @LastLevel + 1
--  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
--                SELECT  Tabs.TabID,
--                        Replicate('-', @LastLevel *2) + Tabs.TabName,
--                        Tabs.ParentTabID,
--                        Tabs.TabOrder,
--                        @LastLevel,
--                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + Tabs.TabOrder as varchar)
--                FROM    Tabs join #TabTree on Tabs.ParentTabID= #TabTree.TabID
--                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
--                 AND PortalID =@PortalID
--                ORDER BY #TabTree.TabOrder
--END

----Get the Orphans
--  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
--                SELECT  Tabs.TabID,
--                        '(Orphan)' + Tabs.TabName,
--                        Tabs.ParentTabID,
--                        Tabs.TabOrder,
--                        999999999,
--                        '999999999'
--                FROM    Tabs 
--                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.TabID)
--                         AND PortalID =@PortalID

---- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
--select IDENTITY(int,1,2) as ord , cast(TabID as varchar) as TabID into #tabs
--from #TabTree
--order by nestlevel, TreeOrder

---- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
--update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID as int)=#TabTree.TabID) 

---- Return Temporary Table
--SELECT TabID, parenttabID, tabname, TabOrder, NestLevel
--FROM #TabTree 
--order by TreeOrder
--GO

----Breadcrumbs
----Ver. 1.0 - 24. dec 2002 - First realase by Cory Isakson
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabCrumbs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabCrumbs]
--GO

--CREATE  proc GetTabCrumbs

--@TabID int,
--@CrumbsXML nvarchar (4000) output

--AS

----Variables used to build Crumb XML string
--declare @ParentTabID int
--declare @TabName as nvarchar(50)
--declare @Level int

----First Child in the branch is Crumb 20.  
--set @Level =20

----Get First Parent Tab ID if there is one
--set @ParentTabID = (select parenttabID from tabs WHERE TabID=@TabID)
----Get TabName of Lowest Child
--set @TabName = (select tabname from tabs WHERE TabID=@TabID)
----Build first Crumb
--set @CrumbsXML = '<root><crumb TabID=''' + cast(@TabID as varchar) + ''' level=''' + cast(@Level as varchar) + '''>' + @TabName + '</crumb>'

--while @ParentTabID is not null
--	begin
--		set @level=@level - 1
--		set @TabID=@parentTabID
--		set @ParentTabID=(select ParentTabID from tabs WHERE TabID=@TabID)
--		set @tabname = (select tabname from tabs WHERE TabID=@TabID)
--		set @CrumbsXML = @CrumbsXML + '<crumb TabID=''' + cast(@TabID as varchar) + ''' level=''' + cast(@Level as varchar) + '''>' + @TabName + '</crumb>'
--	end

--set @CrumbsXML = @CrumbsXML + '</root>'
--GO

----Update Stored PROCEDURE: UpdateTab
----Prevents orphaning a tab or placing tabs in an infinte recursive loop
----26 dec 2002 - Cory Isakson

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO

--ALTER PROCEDURE UpdateTab
--(
--    @PortalID        int,
--    @TabID           int,
--    @ParentTabID     int,
--    @TabOrder        int,
--    @TabName         nvarchar(50),
--    @AuthorizedRoles nvarchar(256),
--    @MobileTabName   nvarchar(50),
--    @ShowMobile      bit
--)
--AS
--IF (@ParentTabID = 0) 
--    SET @ParentTabID = NULL
--IF NOT EXISTS
--(
--    SELECT 
--        * 
--    FROM 
--        Tabs
--    WHERE 
--        TabID = @TabID
--)
--INSERT INTO Tabs (
--    PortalID,
--    ParentTabID,
--    TabOrder,
--    TabName,
--    AuthorizedRoles,
--    MobileTabName,
--    ShowMobile
--) 
--VALUES (
--    @PortalID,
--    @TabOrder,
--    @ParentTabID,
--    @TabName,
--    @AuthorizedRoles,
--    @MobileTabName,
--    @ShowMobile
   
--)
--ELSE
----Updated 26.Dec.2002 Cory Isakson
----Check the Tab recursion so Tab is not placed into an infinate loop when building Tab Tree
--BEGIN TRAN
----If the Update breaks the tab from having a path back to the root then do not Update
--UPDATE
--    Tabs
--SET
--    ParentTabID = @ParentTabID,
--    TabOrder = @TabOrder,
--    TabName = @TabName,
--    AuthorizedRoles = @AuthorizedRoles,
--    MobileTabName = @MobileTabName,
--    ShowMobile = @ShowMobile
--WHERE
--    TabID = @TabID

----Create a Temporary table to hold the tabs
--CREATE TABLE #TabTree
--(
--	[TabID] [int],
--	[TabName] [nvarchar] (50),
--	[ParentTabID] [int],
--	[TabOrder] [int],
--	[NestLevel] [int],
--	[TreeOrder] [varchar] (1000)
--)

--SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
--DECLARE @LastLevel smallint
--SET @LastLevel = 0

---- First, the parent levels
--INSERT INTO	#TabTree
--SELECT 	TabID,
--	TabName,
--	ParentTabID,
--	TabOrder,
--	0,
--	cast(100000000 + TabOrder as varchar)
--FROM	Tabs
--WHERE	ParentTabID IS NULL AND PortalID =@PortalID
--ORDER BY TabOrder

---- Next, the children levels
--WHILE (@@rowcount > 0)
--BEGIN
--  SET @LastLevel = @LastLevel + 1
--  INSERT 	#TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
--		SELECT 	Tabs.TabID,
--			Replicate('-', @LastLevel *2) + Tabs.TabName,
--			Tabs.ParentTabID,
--			Tabs.TabOrder,
--			@LastLevel,
--			cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + Tabs.TabOrder as varchar)
--		FROM	Tabs join #TabTree on Tabs.ParentTabID= #TabTree.TabID
--		WHERE	EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
--		 AND PortalID =@PortalID
--		ORDER BY #TabTree.TabOrder
--END

----Check that the Tab is found in the Tree.  If it is not then we abort the Update
--IF NOT EXISTS (SELECT TabID from #TabTree WHERE TabID=@TabID)
--BEGIN
--	ROLLBACK TRAN
--	--If we want to modify the TabLayout code then we can throw an error AND catch it.
--	--RAISERROR('Not allowed to chose that parent.',11,1)
--END
--ELSE
--COMMIT TRAN
----End changes 26.Dec.2002 Cory Isakson
--GO

--/*
--Blacklist module patch, Manu

--This patch introduces the following changes to the db:
--- Creates table Blacklist
--- Creates sproc AddToBlackList, DeleteFromBlackList
--- Inserts entry in table GeneralModuleDefinitions
--- Inserts entry in table ModuleDefinitions
--*/

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Blacklist]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--DROP TABLE [Blacklist]
--GO

---- Create the table AND add constraints
--CREATE TABLE [Blacklist] (
--	[PortalID] [int] NOT NULL ,
--	[Email] [nvarchar] (100) NOT NULL ,
--	[Date] [smalldatetime] NULL ,
--	[Reason] [nvarchar] (150) NULL 
--) ON [PRIMARY]

--ALTER TABLE [Blacklist] WITH NOCHECK ADD 
--	CONSTRAINT [PK_Blacklist] PRIMARY KEY  CLUSTERED 
--	(
--		[PortalID],
--		[Email]
--	)  ON [PRIMARY] 
--GO

--DECLARE @FriendlyName AS nvarchar(128)
--DECLARE @DesktopSrc AS nvarchar(128)
--DECLARE @GeneralModDefID as uniqueidentifier

--SET @FriendlyName = 'Blacklist (Admin)'      -- You enter the module UI name here
--SET @DesktopSrc = 'Admin/Blacklist.ascx'     -- You enter actual filename here
--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531017}'

--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN
---- Insert data into GeneralModuleDefinitions
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES(@GeneralModDefID,NULL,@FriendlyName,@DesktopSrc,'',1)
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF

---- Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0',@GeneralModDefID)
--END
--GO

---- Procedures
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddToBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddToBlackList]
--GO

--CREATE PROCEDURE AddToBlackList
--(
--@PortalID int,
--@Email nvarchar(100),
--@Reason nvarchar(150)
--)
--AS 
--IF NOT Exists (SELECT Email FROM Blacklist WHERE PortalID=@PortalID AND Email=@Email)
--BEGIN
--	INSERT INTO BlackList
--	(
--		PortalID,
--		Email,
--		Date,
--		Reason
--	)
--	VALUES
--	(
--		@PortalID,
--		@Email,
--		GetDate(),
--		@Reason
--	)
--END
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteFromBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteFromBlackList]
--GO

--CREATE PROCEDURE DeleteFromBlackList
--(
--@PortalID int,
--@Email nvarchar(100)

--)
--AS 
--DELETE FROM Blacklist WHERE PortalID=@PortalID AND Email=@Email
--GO
----end Blacklist module patch

----This patch add entries on db
----IframeModule - Display URL source in an IFRAME
----Credits: Jakob Hansen, hansen3000@hotmail.com
----Modified by manu
--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions WHERE GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531005}')
--BEGIN
----Insert data into GeneralModuleDefinitions
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{2502DB18-B580-4F90-8CB4-C15E6E531005}',NULL,'IframeModule','DesktopModules/IframeModule.ascx','',0)
----Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0','{2502DB18-B580-4F90-8CB4-C15E6E531005}')
--END
--GO

--/*
--Newsletter module patch, Manu AND Jakob

--This patch introduces the following changes to the db:
--- Creates sproc GetSingleUser, GetUsersNewsletter, GetUsersCount, GetUsersNoPassword
--  UpdateUserCheckEmail, UpdateUserFull, UpdateUserSetPassword, SendNewsletterTo
--- Inserts entry in table GeneralModuleDefinitions
--- Inserts entry in table ModuleDefinitions
--*/

--DECLARE @FriendlyName AS nvarchar(128)
--DECLARE @DesktopSrc AS nvarchar(128)
--DECLARE @GeneralModDefID as uniqueidentifier

--SET @FriendlyName = 'Newsletter'                         -- You enter the module UI name here
--SET @DesktopSrc = 'DesktopModules/SendNewsletter.ascx'   -- You enter actual filename here
--SET @GeneralModDefID = '{B484D450-5D30-4C4B-817C-14A25D06577E}'

--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN

---- Insert data into GeneralModuleDefinitions
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES(@GeneralModDefID,NULL,@FriendlyName,@DesktopSrc,'',0)
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF

---- Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0',@GeneralModDefID)

--END
--GO

---- Drop any existing procedures
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleUser]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersNewsletter]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetUsersNewsletter]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetUsersCount]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersNoPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetUsersNoPassword]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserCheckEmail]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateUserCheckEmail]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateUserFull]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserSetPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateUserSetPassword]
--GO
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SendNewsletterTo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [SendNewsletterTo]
--GO

---- =============================================================
---- create the stored procs
---- =============================================================
--CREATE PROCEDURE GetSingleUser
--(
--    @Email nvarchar(100),
--    @PortalID int,
--	@IDLang	nchar(2) = 'IT'
--)
--AS

--SELECT
--	Users.UserID,
--	Users.Email,
--	Users.Password,
--	Users.Name,
--	Users.Company,
--	Users.Address,
--	Users.City,
--	Users.Zip,
--	Users.IDCountry_FK,
--	Users.IDState_FK,
--	Users.PIva,
--	Users.CFiscale,
--	Users.Phone,
--	Users.Fax,
--	Users.SendNewsletter,
--	Users.MailChecked,
--	Users.PortalID,
--	States.Description AS State, 
--	CASE @IDLang
--		WHEN 'IT' THEN Countries.IT
--		WHEN 'EN' THEN Countries.EN
--		WHEN 'DE' THEN Countries.DE
--		WHEN 'FR' THEN Countries.FR
--		WHEN 'ES' THEN Countries.ES
--		WHEN 'PT' THEN Countries.PT
--		ELSE Countries.EN
--	END AS Country
					  
--FROM 
--	Users LEFT OUTER JOIN
--	Countries ON Users.IDCountry_FK = Countries.PK_IDCountry LEFT OUTER JOIN
--	States ON Users.IDState_FK = States.PK_IDState
	
--WHERE
--(Users.Email = @Email) AND (Users.PortalID = @PortalID)
--GO

--CREATE PROCEDURE GetUsersNewsletter
--(
--@PortalID int,
--@MaxUsers int = 250,
--@MinSend float = 30, /* 1 = 1 day, users which send was made in x days will be ignored */    
--@UserCount int = 0 OUTPUT
--)
--AS

--/* 24 hours min delay */
--IF @MinSend < 1 SELECT @MinSend = 1 

--SELECT
--TOP 250
--	UserID, 
--	Name, 
--	Password, 
--	Email, 
--	PortalID
--FROM
--	Users
--WHERE
--	(SendNewsletter = 1) AND 
--	(CAST(COALESCE (LastSend, GETDATE() - @MinSend) AS float) <= CAST(GETDATE() - @MinSend AS float)) AND
--        (PortalID = @PortalID) AND (NOT (Email IN (SELECT Email FROM BlackList WHERE PortalID = @PortalID)))
--ORDER BY UserID

--SELECT @UserCount = @@ROWCOUNT
--GO


--CREATE PROCEDURE GetUsersCount
--(
--    @PortalID		int,
--    @UsersCount		int OUTPUT
--)
--AS

--SELECT TOP 1
--@UsersCount = COUNT(DISTINCT Users.UserID)
--FROM  Users
--WHERE Users.PortalID = @PortalID
--GO


--CREATE PROCEDURE GetUsersNoPassword
--(
--    @PortalID int
--)
--AS

--SELECT TOP 200   UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, IDCountry_FK, IDState_FK, PIva, CFiscale, Phone, Fax
--FROM         Users
--WHERE     (PortalID = @PortalID) AND (Password is null or Password = '')
--ORDER BY Email
--GO


--CREATE PROCEDURE UpdateUserCheckEmail
--(
--    @UserID		    int,
--    @CheckedEmail	tinyint
--)
--AS

--UPDATE Users

--SET

--    MailChecked = @CheckedEmail

--WHERE UserID = @UserID
--GO

--CREATE PROCEDURE UpdateUserFull
--(
--    @UserID		    int,
--    @PortalID       int,
--    @Name		    nvarchar(50),
--    @Company	    nvarchar(50),
--    @Address		nvarchar(50),
--    @City		    nvarchar(50),
--    @Zip		    nvarchar(6),
--    @Phone		    nvarchar(50),
--    @Fax		    nvarchar(50),
--    @PIva		    nvarchar(11),
--    @CFiscale	    nvarchar(16),
--    @Email		    nvarchar(100),
--    @Password	    nvarchar(20),
--    @SendNewsletter	bit,
--	@IDCountry_FK	nchar(2),  
--	@IDState_FK		int
--)
--AS

--UPDATE Users

--SET

--PortalID = @PortalID,
--Name = @Name,
--Company = @Company,
--Address = @Address,		
--City = @City,		
--Zip = @Zip,		
--Phone = @Phone,		
--Fax = @Fax,		
--PIva = @PIva,		
--CFiscale = @CFiscale,	
--Email = @Email,		
--Password = @Password,
--SendNewsletter = @SendNewsletter,
--IDCountry_FK = @IDCountry_FK,
--IDState_FK = @IDState_FK

--WHERE UserID = @UserID
--GO

--CREATE PROCEDURE UpdateUserSetPassword
--(
--    @UserID		    int,
--    @Password	    varchar(20)
--)
--AS

--UPDATE Users
--SET
--    Password = @Password

--WHERE UserID = @UserID
--GO

--CREATE PROCEDURE SendNewsletterTo
--(
--@PortalID int,
--@Email nvarchar(100)
--)
--AS 
--UPDATE Users SET LastSend = GETDATE() WHERE PortalID=@PortalID AND Email=@Email
--select 0
--GO
--/* end Newsletter module patch */

--/* Remove cascade delete */

--ALTER TABLE Tabs
--	DROP CONSTRAINT FK_Tabs_Portals
--GO
--ALTER TABLE Modules
--	DROP CONSTRAINT FK_Modules_Tabs1
--GO
--ALTER TABLE Modules
--	DROP CONSTRAINT FK_Modules_ModuleDefinitions1
--GO
--ALTER TABLE PortalSettings
--	DROP CONSTRAINT FK_PortalSettings_Portals
--GO
--/* BEGIN TRANSACTION */
--ALTER TABLE Tabs WITH NOCHECK ADD CONSTRAINT
--	FK_Tabs_Portals FOREIGN KEY
--	(
--	PortalID
--	) REFERENCES Portals
--	(
--	PortalID
--	)
--GO
--ALTER TABLE PortalSettings WITH NOCHECK ADD CONSTRAINT
--	FK_PortalSettings_Portals FOREIGN KEY
--	(
--	PortalID
--	) REFERENCES Portals
--	(
--	PortalID
--	) ON UPDATE CASCADE
--	 ON DELETE CASCADE
--GO
--ALTER TABLE Modules WITH NOCHECK ADD CONSTRAINT
--	FK_Modules_ModuleDefinitions1 FOREIGN KEY
--	(
--	ModuleDefID
--	) REFERENCES ModuleDefinitions
--	(
--	ModuleDefID
--	)
--GO
--ALTER TABLE Modules WITH NOCHECK ADD CONSTRAINT
--	FK_Modules_Tabs1 FOREIGN KEY
--	(
--	TabID
--	) REFERENCES Tabs
--	(
--	TabID
--	)
--GO
--/* end remove cascade delete */

--/*
--Pictures module patch, Ender Malkoc, endermalkoc@hotmail.com

--This patch introduces the following changes to the db:
--- Creates table Picture
--- Creates sproc AddPicture, DeletePicture, GetPicture, GetSinglePicture, UpdatePicture
--- Inserts entry in table GeneralModuleDefinitions
--- Inserts entry in table ModuleDefinitions
--*/
--DECLARE @FriendlyName AS nvarchar(128)
--DECLARE @DesktopSrc AS nvarchar(128)
--DECLARE @GeneralModDefID as uniqueidentifier

--SET @FriendlyName = 'Pictures'                     -- You enter the module UI name here
--SET @DesktopSrc = 'DesktopModules/Pictures.ascx'   -- You enter actual filename here
--SET @GeneralModDefID = '{B29CB86B-AEA1-4E94-8B77-B4E4239258B0}'

--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN

---- Create the table AND add constraints
--CREATE TABLE [Pictures] (
--	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL ,
--	[DisplayOrder] [int] NOT NULL ,
--	[MetadataXml] [varchar] (6000) NULL ,
--	[ShortDescription] [varchar] (256) NULL ,
--	[Keywords] [varchar] (256) NULL 
--) ON [PRIMARY]

--ALTER TABLE [Pictures] WITH NOCHECK ADD 
--    CONSTRAINT [PK_Pictures] PRIMARY KEY  NONCLUSTERED 
--    (
--        [ItemID]
--    )  ON [PRIMARY] 

--ALTER TABLE [Pictures] ADD 
--    CONSTRAINT [FK_Pictures_Modules] FOREIGN KEY 
--    (
--        [ModuleID]
--    ) REFERENCES [Modules] (
--        [ModuleID]
--    ) ON DELETE CASCADE  NOT FOR REPLICATION 

---- Insert data into GeneralModuleDefinitions
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES(@GeneralModDefID,NULL,@FriendlyName,@DesktopSrc,'',0)
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF

---- Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0',@GeneralModDefID)

--END

---- Drop any existing procedures

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddPicture]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeletePicture]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPicture]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSinglePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSinglePicture]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPicturesPaged]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdatePicture]
--GO

---- =============================================================
---- create the stored procs
---- =============================================================
--CREATE PROCEDURE [AddPicture]
--	(@ItemID 	[int ]OUTPUT,
--	 @ModuleID 	[int],
--	 @DisplayOrder	[int],
--	 @MetadataXml VARCHAR(6000),
--	 @ShortDescription VARCHAR(256),
--	 @Keywords VARCHAR(256)
--)
--AS 
--INSERT INTO [Pictures]
--	([ModuleID],
--	[DisplayOrder],
--	[MetadataXml],
--	[ShortDescription],
--	[Keywords]
--) 
--VALUES 
--	(@ModuleID,
--	 @DisplayOrder,
--	 @MetadataXml,
--	 @ShortDescription,
--	 @Keywords)
--SELECT 
--	@ItemID = @@IDENTITY
--GO

----************************************************************************
--CREATE PROCEDURE [DeletePicture]
--	(@ItemID 	[int])
--AS DELETE FROM [Pictures]
--WHERE 
--	( [ItemID] = @ItemID)
--GO

----************************************************************************
--CREATE PROCEDURE GetPicture
--(@ModuleID int)
--AS
--SELECT ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords
--FROM Pictures 
--WHERE ModuleID = @ModuleID
--ORDER BY DisplayOrder
--GO

----************************************************************************
--CREATE PROCEDURE GetSinglePicture 
--(@ItemID int)
--AS
--SELECT 
--	OriginalPictures.ItemID, 
--	(
--		SELECT TOP 1
--			ItemID
--		FROM 
--			Pictures
--		WHERE 
--			ModuleID = (SELECT ModuleID FROM Pictures WHERE ItemID = OriginalPictures.ItemID)
--			AND ItemID <> OriginalPictures.ItemID
--			AND (DisplayOrder < OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID < OriginalPictures.ItemID))
--		ORDER BY
--			OriginalPictures.DisplayOrder - DisplayOrder, OriginalPictures.ItemID - ItemID
--	) AS PreviousItemID,
--	(
--		SELECT TOP 1
--			ItemID
--		FROM 
--			Pictures
--		WHERE 
--			ModuleID = (SELECT ModuleID FROM Pictures WHERE ItemID = OriginalPictures.ItemID)
--			AND ItemID <> OriginalPictures.ItemID
--			AND (DisplayOrder > OriginalPictures.DisplayOrder OR (DisplayOrder = OriginalPictures.DisplayOrder AND ItemID > OriginalPictures.ItemID))
--		ORDER BY
--			DisplayOrder - OriginalPictures.DisplayOrder ,	ItemID - OriginalPictures.ItemID 
--	) AS NextItemID,
--	OriginalPictures.ModuleID, 
--	OriginalPictures.DisplayOrder, 
--	OriginalPictures.MetadataXml, 
--	OriginalPictures.ShortDescription, 
--	OriginalPictures.Keywords
--FROM 
--	Pictures As OriginalPictures
--WHERE 
--	ItemID = @ItemID
--GO

----************************************************************************
--CREATE     PROCEDURE GetPicturesPaged
--(
--	@ModuleID int,
--	@Page int = 1,
--	@RecordsPerPage int = 10
--)
--AS

---- We don't want to return the # of rows inserted
---- into our temporary table, so turn NOCOUNT ON
--SET NOCOUNT ON

----Create a temporary table
--CREATE TABLE #TempItems
--(
--	ID				int IDENTITY,
-- 	ItemID 			int,
-- 	ModuleID 		int,
--	DisplayOrder		int,
-- 	MetadataXml		varchar(6000),
-- 	ShortDescription	varchar(256),
-- 	Keywords		varchar(256)
--)

---- Insert the rows from tblItems into the temp. table
--INSERT INTO
--#TempItems
--(
--	ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords
--)
--SELECT
--	Pictures.ItemID, 
--	Pictures.DisplayOrder, 
--	Pictures.MetadataXml, 
--	Pictures.ShortDescription, 
--	Pictures.Keywords
--FROM
--	Pictures
--WHERE
--	Pictures.ModuleID = @ModuleID
--ORDER BY 
--	DisplayOrder

---- Find out the first AND last record we want
--DECLARE @FirstRec int, @LastRec int
--SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
--SELECT @LastRec = (@Page * @RecordsPerPage + 1)

---- Now, return the set of paged records, plus, an indiciation of we
---- have more records or not!
--SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
--FROM #TempItems
--WHERE ID > @FirstRec AND ID < @LastRec

---- Turn NOCOUNT back OFF
--SET NOCOUNT OFF
--GO

----************************************************************************
--CREATE PROCEDURE [UpdatePicture]
--	(@ItemID 	[int ],
--	 @ModuleID 	[int],
--	 @DisplayOrder	[int],
--	 @MetadataXml VARCHAR(6000),
--	 @ShortDescription VARCHAR(256),
--	 @Keywords VARCHAR(256)
--)
--AS 
--UPDATE [Pictures]
--SET  
--	 [DisplayOrder] 		= @DisplayOrder,
--	 [MetadataXml]		= @MetadataXml,
--	 [ShortDescription]	= @ShortDescription,
--	 [Keywords]		= @Keywords
--WHERE 
--	( [ItemID]	 = @ItemID)
--/* end PictureAlbum 2.0 mods */
--GO

--/* Register signin */
--DECLARE @FriendlyName AS nvarchar(128)
--DECLARE @DesktopSrc AS nvarchar(128)
--DECLARE @GeneralModDefID as uniqueidentifier

--SET @FriendlyName = 'Signin'                     -- You enter the module UI name here
--SET @DesktopSrc = 'DesktopModules/Signin.ascx'   -- You enter actual filename here
--SET @GeneralModDefID = '{A0F1F62B-FDC7-4de5-BBAD-A5DAF31D960A}'

--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN

---- Insert data into GeneralModuleDefinitions
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES(@GeneralModDefID,NULL,@FriendlyName,@DesktopSrc,'',0)
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF

---- Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0',@GeneralModDefID)

--END
--/* end Register signin */
--GO

---- Change by Geert.Audenaert@Syntegra.Com
---- Date: 6/2/2003
		
---- Alter the modules tables so it contains the extra necessary columns for the workflow implementation
--IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_NAME] = 'Modules' AND [COLUMN_NAME] = 'AuthorizedPublishingRoles') 
--BEGIN
--	ALTER TABLE [Modules] ADD 
--		[AuthorizedPublishingRoles] [nvarchar] (256) NULL ,
--		[NewVersion] [bit] NULL CONSTRAINT [DF_Modules_NewVersion] DEFAULT (1),
--		[SupportWorkflow] [bit] NULL
--END
--GO
---- marcb@hotmail.com
---- I have removed all Geert lines concerning the staging user because all tables are recreated later using ptrefix "st_" instead of "staging" user name

---- Recreate all stored procedures 
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddAnnouncement]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddContact]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddEvent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddLink]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddMessage]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddModule]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddModuleDefinition]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddPortal]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddTab]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddUser]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddUserFull]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddUserRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteAnnouncement]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteContact]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteDocument]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteEvent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteLink]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteModule]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteModuleDefinition]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeletePortal]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteTab]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteUser]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteUserRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAnnouncements]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAnnouncements]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthAddRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthAddRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthDeleteRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthDeleteRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthEditRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthEditRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPropertiesRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthPropertiesRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPublishingRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthPublishingRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthViewRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthViewRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetContacts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetContacts]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetCountries]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountriesFiltered]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetCountriesFiltered]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetCurrentModuleDefinitions]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocumentContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetDocumentContent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocuments]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetDocuments]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetEvents]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetHtmlText]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLinks]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetLinks]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitionByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModuleDefinitionByID]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModuleDefinitions]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleInUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModuleInUse]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModuleSettings]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModulesAllPortals]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModulesByName]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesSinglePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetModulesSinglePortal]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetNextMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetNextMessageID]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortalCustomSettings]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortalRoles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortalSettings]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettingsPortalID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortalSettingsPortalID]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortals]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalsModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPortalsModules]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPrevMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetPrevMessageID]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRelatedTables]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetRelatedTables]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRoleMembership]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetRoleMembership]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRolesByUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetRolesByUser]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleAnnouncement]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleContact]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleDocument]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleEvent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleLink]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleMessage]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleModuleDefinition]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleUser]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSolutionModuleDefinitions]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSolutions]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetStates]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetStates]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabSettings]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsByPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabsByPortal]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabsParent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsinTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabsinTab]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetThreadMessages]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTopLevelMessages]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetUsers]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleEdited]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [ModuleEdited]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Publish]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [Publish]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateAnnouncement]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateContact]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateDocument]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateEvent]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateHtmlText]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateLink]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateModule]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateModuleDefinition]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateModuleDefinitions]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateModuleOrder]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateModuleSetting]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdatePortalInfo]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdatePortalSetting]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateRole]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateTab]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTabOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateTabOrder]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateUser]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UserLogin]
--GO

---- =============================================================
---- ALTER  the stored procs
---- =============================================================
--CREATE  PROCEDURE AddAnnouncement
--(
--    @ModuleID       int,
--    @UserName       nvarchar(100),
--    @Title          nvarchar(150),
--    @MoreLink       nvarchar(150),
--    @MobileMoreLink nvarchar(150),
--    @ExpireDate     DateTime,
--    @Description    nvarchar(2000),
--    @ItemID         int OUTPUT
--)
--AS

--INSERT INTO Staging.Announcements
--(
--    ModuleID,
--    CreatedByUser,
--    CreatedDate,
--    Title,
--    MoreLink,
--    MobileMoreLink,
--    ExpireDate,
--    Description
--)

--VALUES
--(
--    @ModuleID,
--    @UserName,
--    GetDate(),
--    @Title,
--    @MoreLink,
--    @MobileMoreLink,
--    @ExpireDate,
--    @Description
--)

--SELECT
--    @ItemID = @@IDENTITY
--GO

--CREATE  PROCEDURE AddContact
--(
--    @ModuleID int,
--    @UserName nvarchar(100),
--    @Name     nvarchar(50),
--    @Role     nvarchar(100),
--    @Email    nvarchar(100),
--    @Contact1 nvarchar(250),
--    @Contact2 nvarchar(250),
--    @ItemID   int OUTPUT
--)
--AS

--INSERT INTO Staging.Contacts
--(
--    CreatedByUser,
--    CreatedDate,
--    ModuleID,
--    Name,
--    Role,
--    Email,
--    Contact1,
--    Contact2
--)

--VALUES
--(
--    @UserName,
--    GetDate(),
--    @ModuleID,
--    @Name,
--    @Role,
--    @Email,
--    @Contact1,
--    @Contact2
--)

--SELECT
--    @ItemID = @@IDENTITY
--GO

--CREATE  PROCEDURE AddEvent
--(
--    @ModuleID    int,
--    @UserName    nvarchar(100),
--    @Title       nvarchar(100),
--    @ExpireDate  DateTime,
--    @Description nvarchar(2000),
--    @WhereWhen   nvarchar(100),
--    @ItemID      int OUTPUT
--)
--AS

--INSERT INTO Staging.Events
--(
--    ModuleID,
--    CreatedByUser,
--    Title,
--    CreatedDate,
--    ExpireDate,
--    Description,
--    WhereWhen
--)

--VALUES
--(
--    @ModuleID,
--    @UserName,
--    @Title,
--    GetDate(),
--    @ExpireDate,
--    @Description,
--    @WhereWhen
--)

--SELECT
--    @ItemID = @@IDENTITY
--GO

--CREATE  PROCEDURE AddLink
--(
--    @ModuleID    int,
--    @UserName    nvarchar(100),
--    @Title       nvarchar(100),
--    @Url         nvarchar(250),
--    @MobileUrl   nvarchar(250),
--    @ViewOrder   int,
--    @Description nvarchar(2000),
--    @ItemID      int OUTPUT
--)
--AS

--INSERT INTO Staging.Links
--(
--    ModuleID,
--    CreatedByUser,
--    CreatedDate,
--    Title,
--    Url,
--    MobileUrl,
--    ViewOrder,
--    Description
--)
--VALUES
--(
--    @ModuleID,
--    @UserName,
--    GetDate(),
--    @Title,
--    @Url,
--    @MobileUrl,
--    @ViewOrder,
--    @Description
--)

--SELECT
--    @ItemID = @@IDENTITY
--GO

--CREATE     PROCEDURE AddMessage
--(
--    @ItemID int OUTPUT,
--    @Title nvarchar(100),
--    @Body nvarchar(3000),
--    @ParentID int,
--    @UserName nvarchar(100),
--    @ModuleID int
--)   

--AS 

--/* Find DisplayOrder of parent item */
--DECLARE @ParentDisplayOrder as nvarchar(750)

--SET @ParentDisplayOrder = ''

--SELECT 
--    @ParentDisplayOrder = DisplayOrder
--FROM 
--    Discussion 
--WHERE 
--    ItemID = @ParentID

--INSERT INTO Discussion
--(
--    Title,
--    Body,
--    DisplayOrder,
--    CreatedDate, 
--    CreatedByUser,
--    ModuleID
--)

--VALUES
--(
--    @Title,
--    @Body,
--    @ParentDisplayOrder + CONVERT( nvarchar(24), GetDate(), 21 ),
--    GetDate(),
--    @UserName,
--    @ModuleID
--)

--SELECT 
--    @ItemID = @@IDENTITY
--GO

--CREATE    PROCEDURE AddModule
--(
--    @TabID                  int,
--    @ModuleOrder            int,
--    @ModuleTitle            nvarchar(256),
--    @PaneName               nvarchar(50),
--    @ModuleDefID            int,
--    @CacheTime              int,
--    @EditRoles              nvarchar(256),
--    @AddRoles               nvarchar(256),
--    @ViewRoles              nvarchar(256),
--    @DeleteRoles            nvarchar(256),
--    @PropertiesRoles	    nvarchar(256),
--    @ShowMobile             bit,
--    @PublishingRoles        nvarchar(256),
--    @SupportWorkflow	    bit,
--    @ModuleID               int OUTPUT
--)
--AS
--INSERT INTO Modules
--(
--    TabID,
--    ModuleOrder,
--    ModuleTitle,
--    PaneName,
--    ModuleDefID,
--    CacheTime,
--    AuthorizedEditRoles,
--    AuthorizedAddRoles,
--    AuthorizedViewRoles,
--    AuthorizedDeleteRoles,
--    AuthorizedPropertiesRoles,
--    ShowMobile,
--    AuthorizedPublishingRoles,
--    NewVersion, 
--    SupportWorkflow
--) 
--VALUES
--(
--    @TabID,
--    @ModuleOrder,
--    @ModuleTitle,
--    @PaneName,
--    @ModuleDefID,
--    @CacheTime,
--    @EditRoles,
--    @AddRoles,
--    @ViewRoles,
--    @DeleteRoles,
--    @PropertiesRoles,
--    @ShowMobile,
--    @PublishingRoles,
--    1, -- False
--    @SupportWorkflow
--)

--SELECT 
--    @ModuleID = @@IDENTITY
--GO

--CREATE PROCEDURE AddModuleDefinition
--(
--    @GeneralModDefID	uniqueidentifier,
--    @FriendlyName	    nvarchar(128),
--    @DesktopSrc		    nvarchar(256),
--    @MobileSrc		    nvarchar(256),
--    @Admin			    bit
--)
--AS
--INSERT INTO GeneralModuleDefinitions
--(
--    GeneralModDefID,
--    FriendlyName,
--    DesktopSrc,
--    MobileSrc,
--    Admin
--)
--VALUES
--(
--    @GeneralModDefID,
--    @FriendlyName,
--    @DesktopSrc,
--    @MobileSrc,
--    @Admin
--)
--GO

--CREATE PROCEDURE AddPortal
--(
--    @PortalAlias            nvarchar(128),
--    @PortalName             nvarchar(128),
--    @PortalPath             nvarchar(128),
--    @AlwaysShowEditButton   bit,
--    @PortalID               int OUTPUT
--)
--AS

--INSERT INTO Portals
--(
--    PortalAlias,
--    PortalName,
--    PortalPath,
--    AlwaysShowEditButton
--)

--VALUES
--(
--    @PortalAlias,
--    @PortalName,
--    @PortalPath,
--    @AlwaysShowEditButton
--)

--SELECT
--    @PortalID = @@IDENTITY
--GO

--CREATE PROCEDURE AddRole
--(
--    @PortalID    int,
--    @RoleName    nvarchar(50),
--    @RoleID      int OUTPUT
--)
--AS

--INSERT INTO Roles
--(
--    PortalID,
--    RoleName
--)

--VALUES
--(
--    @PortalID,
--    @RoleName
--)

--SELECT
--    @RoleID = @@IDENTITY
--GO

--CREATE PROCEDURE AddTab
--(
--    @PortalID   int,
--    @TabName    nvarchar(50),
--    @TabOrder   int,
--    @AuthorizedRoles nvarchar (256),
--    @MobileTabName nvarchar(50),
--    @TabID      int OUTPUT
--)
--AS

--INSERT INTO Tabs
--(
--    PortalID,
--    TabName,
--    TabOrder,
--    ShowMobile,
--    MobileTabName,
--    AuthorizedRoles
--)

--VALUES
--(
--    @PortalID,
--    @TabName,
--    @TabOrder,
--    0, /* false */
--    @MobileTabName,
--    @AuthorizedRoles
--)

--SELECT
--    @TabID = @@IDENTITY
--GO

--CREATE PROCEDURE AddUser
--(
--	@PortalID	int,
--    @Name     nvarchar(50),
--    @Email    nvarchar(100),
--    @Password nvarchar(20),
--    @UserID   int OUTPUT
--)
--AS

--INSERT INTO Users
--(
--    Name,
--    Email,
--    Password,
--    PortalID
--)

--VALUES
--(
--    @Name,
--    @Email,
--    @Password,
--    @PortalID
--)

--SELECT
--    @UserID = @@IDENTITY
--GO

--CREATE PROCEDURE AddUserFull
--(
--	@PortalID	    int,
--    @Name		    nvarchar(50),
--    @Company	    nvarchar(50),
--    @Address	    nvarchar(50),
--    @City		    nvarchar(50),
--    @Zip		    nvarchar(6),
--    @Phone		    nvarchar(50),
--    @Fax		    nvarchar(50),
--    @PIva		    nvarchar(11),
--    @CFiscale	    nvarchar(16),
--    @Email		    nvarchar(100),
--    @Password	    nvarchar(20),
--    @SendNewsletter	bit,
--	@IDCountry_FK	nchar(2),  
--	@IDState_FK		int,
--    @UserID		    int OUTPUT
--)
--AS

--INSERT INTO Users
--(
--    PortalID,
--    Name,
--    Company,
--	Address,		
--	City,		
--	Zip,		
--	Phone,		
--	Fax,		
--	PIva,		
--	CFiscale,	
--	Email,		
--	Password,
--	SendNewsletter,
--	IDCountry_FK,
--	IDState_FK
--)

--VALUES
--(
--    @PortalID,
--    @Name,
--    @Company,
--	@Address,	
--	@City,	
--	@Zip,	
--	@Phone,	
--	@Fax,	
--	@PIva,	
--	@CFiscale,
--    @Email,
--    @Password,
--    @SendNewsletter,
--	@IDCountry_FK,
--	@IDState_FK
--)

--SELECT
--    @UserID = @@IDENTITY
--GO

--CREATE PROCEDURE AddUserRole
--(
--    @UserID int,
--    @RoleID int
--)
--AS

--SELECT 
--    *
--FROM
--    UserRoles

--WHERE
--    UserID=@UserID
--    AND
--    RoleID=@RoleID

--/* only insert if the record doesn't yet exist */
--IF @@Rowcount < 1

--    INSERT INTO UserRoles
--    (
--        UserID,
--        RoleID
--    )

--    VALUES
--    (
--        @UserID,
--        @RoleID
--    )
--GO

--CREATE  PROCEDURE DeleteAnnouncement
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Staging.Announcements

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE DeleteContact
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Staging.Contacts

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE DeleteDocument
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Staging.Documents

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE DeleteEvent
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Staging.Events

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE DeleteLink
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Staging.Links

--WHERE
--    ItemID = @ItemID
--GO

--CREATE PROCEDURE DeleteModule
--(
--    @ModuleID       int
--)
--AS
--DELETE FROM 
--    Modules 
--WHERE 
--    ModuleID = @ModuleID
--GO

--CREATE PROCEDURE DeleteModuleDefinition
--(
--    @ModuleDefID uniqueidentifier
--)
--AS
--DELETE FROM
--    GeneralModuleDefinitions
--WHERE
--    GeneralModDefID = @ModuleDefID
--GO

--CREATE PROCEDURE DeletePortal
--(
--    @PortalID       int
--)
--AS
--DELETE FROM 
--    Portals 
--WHERE 
--    PortalID = @PortalID
--GO

--CREATE PROCEDURE DeleteRole
--(
--    @RoleID int
--)
--AS

--DELETE FROM
--    Roles

--WHERE
--    RoleID = @RoleID
--GO

--CREATE PROCEDURE DeleteTab
--(
--    @TabID int
--)
--AS

--DELETE FROM
--    Tabs

--WHERE
--    TabID = @TabID
--GO

--CREATE PROCEDURE DeleteUser
--(
--    @UserID int
--)
--AS

--DELETE FROM
--    Users

--WHERE
--    UserID=@UserID
--GO

--CREATE PROCEDURE DeleteUserRole
--(
--    @UserID int,
--    @RoleID int
--)
--AS

--DELETE FROM
--    UserRoles

--WHERE
--    UserID=@UserID
--    AND
--    RoleID=@RoleID
--GO

--CREATE  PROCEDURE GetAnnouncements
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    ItemID,
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    MoreLink,
--	    MobileMoreLink,
--	    ExpireDate,
--	    Description
--	FROM 
--	    Announcements
--	WHERE
--	    ModuleID = @ModuleID
--	  AND
--	    ExpireDate > GetDate()
--ELSE
--	SELECT
--	    ItemID,
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    MoreLink,
--	    MobileMoreLink,
--	    ExpireDate,
--	    Description
--	FROM 
--	    staging.Announcements
--	WHERE
--	    ModuleID = @ModuleID
--	  AND
--	    ExpireDate > GetDate()
--GO

--CREATE PROCEDURE GetAuthAddRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @AddRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @AddRoles   = Modules.AuthorizedAddRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE PROCEDURE GetAuthDeleteRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @DeleteRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @DeleteRoles   = Modules.AuthorizedDeleteRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE PROCEDURE GetAuthEditRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @EditRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @EditRoles   = Modules.AuthorizedEditRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE PROCEDURE GetAuthPropertiesRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @PropertiesRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @PropertiesRoles   = Modules.AuthorizedPropertiesRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE PROCEDURE GetAuthPublishingRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @PublishingRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @PublishingRoles   = Modules.AuthorizedPublishingRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE PROCEDURE GetAuthViewRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @ViewRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @ViewRoles   = Modules.AuthorizedViewRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--CREATE  PROCEDURE GetContacts
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF (@WorkflowVersion = 1)
--	SELECT
--	    ItemID,
--	    CreatedDate,
--	    CreatedByUser,
--	    Name,
--	    Role,
--	    Email,
--	    Contact1,
--	    Contact2
--	FROM
--	    Contacts
--	WHERE
--	    ModuleID = @ModuleID
--ELSE
--	SELECT
--	    ItemID,
--	    CreatedDate,
--	    CreatedByUser,
--	    Name,
--	    Role,
--	    Email,
--	    Contact1,
--	    Contact2
--	FROM
--	    staging.Contacts
--	WHERE
--	    ModuleID = @ModuleID
--GO

--CREATE PROCEDURE GetCountries
--(
--	@IDLang	nchar(2) = 'IT'
--)

--AS

--SELECT
--PK_IdCountry,
--CASE @IDLang
--	WHEN 'IT' THEN Countries.IT
--	WHEN 'EN' THEN Countries.EN
--	WHEN 'DE' THEN Countries.DE
--	WHEN 'FR' THEN Countries.FR
--	WHEN 'ES' THEN Countries.ES
--	WHEN 'PT' THEN Countries.PT
--	ELSE Countries.EN
--END AS Description
--FROM	countries
--ORDER BY Description
--GO

--CREATE PROCEDURE GetCountriesFiltered
--(
--	@IDLang	nchar(2) = 'IT',
--	@Filter nvarchar(1000)
--)

--AS

--IF (@IDLang = 'IT')
--BEGIN
--	SELECT	PK_IdCountry, IT AS Description
--	FROM    countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END

--IF (@IDLang = 'EN')
--BEGIN
--	SELECT	PK_IdCountry, EN AS Description
--	FROM	countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END

--IF (@IDLang = 'FR')
--BEGIN
--	SELECT	PK_IdCountry, FR AS Description
--	FROM	countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END

--IF (@IDLang = 'ES')
--BEGIN
--	SELECT	PK_IdCountry, ES AS Description
--	FROM	countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END

--IF (@IDLang = 'DE')
--BEGIN
--	SELECT	PK_IdCountry, DE AS Description
--	FROM	countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END

--IF (@IDLang = 'PT')
--BEGIN
--	SELECT	PK_IdCountry, PT AS Description
--	FROM	countries
--	WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
--	ORDER BY Description
--END
--GO

--/* returns all module definitions for the specified portal */
--CREATE PROCEDURE GetCurrentModuleDefinitions
--(
--    @PortalID  int
--)
--AS
--SELECT  
--    GeneralModuleDefinitions.FriendlyName,
--    GeneralModuleDefinitions.DesktopSrc,
--    GeneralModuleDefinitions.MobileSrc,
--    ModuleDefinitions.ModuleDefID
--FROM
--    ModuleDefinitions
--INNER JOIN
--	GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--WHERE   
--    ModuleDefinitions.PortalID = @PortalID
--ORDER BY
--GeneralModuleDefinitions.Admin, GeneralModuleDefinitions.FriendlyName
--GO

--CREATE  PROCEDURE GetDocumentContent
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    Content,
--	    ContentType,
--	    ContentSize,
--	    FileFriendlyName
--	FROM
--	    Documents
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    Content,
--	    ContentType,
--	    ContentSize,
--	    FileFriendlyName
--	FROM
--	    staging.Documents
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE  PROCEDURE GetDocuments
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    ItemID,
--	    FileFriendlyName,
--	    FileNameUrl,
--	    CreatedByUser,
--	    CreatedDate,
--	    Category,
--	    ContentSize
--	FROM
--	    Documents
--	WHERE
--	    ModuleID = @ModuleID
--ELSE
--	SELECT
--	    ItemID,
--	    FileFriendlyName,
--	    FileNameUrl,
--	    CreatedByUser,
--	    CreatedDate,
--	    Category,
--	    ContentSize
--	FROM
--	    staging.Documents
--	WHERE
--	    ModuleID = @ModuleID
--GO

--CREATE  PROCEDURE GetEvents
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF (@WorkflowVersion = 1)
--	SELECT
--	    ItemID,
--	    Title,
--	    CreatedByUser,
--	    WhereWhen,
--	    CreatedDate,
--	    Title,
--	    ExpireDate,
--	    Description
--	FROM
--	    Events
--	WHERE
--	    ModuleID = @ModuleID
--	  AND
--	    ExpireDate > GetDate()
--ELSE
--	SELECT
--	    ItemID,
--	    Title,
--	    CreatedByUser,
--	    WhereWhen,
--	    CreatedDate,
--	    Title,
--	    ExpireDate,
--	    Description
--	FROM
--	    staging.Events
--	WHERE
--	    ModuleID = @ModuleID
--	  AND
--	    ExpireDate > GetDate()
--GO

--CREATE  PROCEDURE GetHtmlText
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT *
--	FROM
--	    HtmlText
--	WHERE
--	    ModuleID = @ModuleID
--ELSE
--	SELECT *
--	FROM
--	    staging.HtmlText
--	WHERE
--	    ModuleID = @ModuleID
--GO

--CREATE  PROCEDURE GetLinks
--(
--    @ModuleID int,
--    @WorkflowVersion int
--)
--AS

--IF (@WorkflowVersion = 1)
--	SELECT
--	    ItemID,
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    Url,
--	    ViewOrder,
--	    Description
--	FROM
--	    Links
--	WHERE
--	    ModuleID = @ModuleID
--	ORDER BY
--	    ViewOrder
--ELSE
--	SELECT
--	    ItemID,
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    Url,
--	    ViewOrder,
--	    Description
--	FROM
--	    staging.Links
--	WHERE
--	    ModuleID = @ModuleID
--	ORDER BY
--	    ViewOrder
--GO

--CREATE PROCEDURE
--GetModuleDefinitionByID
--(
--	@ModuleID int
--)
--AS


--SELECT     ModuleDefinitions.ModuleDefID, ModuleDefinitions.PortalID, GeneralModuleDefinitions.FriendlyName, GeneralModuleDefinitions.DesktopSrc, 
--                      GeneralModuleDefinitions.MobileSrc, GeneralModuleDefinitions.Admin, Modules.ModuleID
--FROM         GeneralModuleDefinitions INNER JOIN
--                      ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID INNER JOIN
--                      Modules ON ModuleDefinitions.ModuleDefID = Modules.ModuleDefID
--WHERE     (Modules.ModuleID = @ModuleID)
--GO

--/* returns all module definitions for the specified portal */
--CREATE PROCEDURE GetModuleDefinitions

--AS
--SELECT     GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc
--FROM         GeneralModuleDefinitions
--ORDER BY Admin, FriendlyName
--GO

--CREATE PROCEDURE GetModuleInUse
--(
--    @ModuleID uniqueidentifier
--)
--AS
--SELECT     Portals.PortalID, Portals.PortalAlias, Portals.PortalName, '1' AS Checked
--FROM         Portals LEFT OUTER JOIN
--                      ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
--WHERE     (ModuleDefinitions.GeneralModDefID = @ModuleID)

--UNION

--SELECT DISTINCT
--    PortalID, PortalAlias, PortalName, '0' AS Checked
--FROM   Portals
--WHERE  
--(
--PortalID NOT IN
--    (SELECT     Portals.PortalID
--     FROM       Portals LEFT OUTER JOIN ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
--     WHERE      (ModuleDefinitions.GeneralModDefID = @ModuleID)
--    )
--)
--GO

--CREATE PROCEDURE GetModuleSettings
--(
--    @ModuleID int
--)
--AS
--SELECT     SettingName, SettingValue
--FROM         ModuleSettings
--WHERE     (ModuleID = @ModuleID)
--GO

--CREATE PROCEDURE GetModulesAllPortals
--AS

--SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' as PortalAlias, -1 as TabOrder

--UNION

--	SELECT     Modules.ModuleID, Portals.PortalAlias + '/' + Tabs.TabName + '/' + Modules.ModuleTitle + ' (' + GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, Tabs.TabOrder
--	FROM         Modules INNER JOIN
--	                      Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
--	                      Portals ON Tabs.PortalID = Portals.PortalID INNER JOIN
--	                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--	                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--	WHERE     (Modules.ModuleID > 0) AND (GeneralModuleDefinitions.Admin = 0)

--ORDER BY PortalAlias, Modules.ModuleTitle
--GO

--CREATE PROCEDURE GetModulesByName
--(
--	@ModuleName varchar(128),
--	@PortalID int
--)
--AS

--SELECT      0 ModuleID, ' Nessun modulo' ModuleTitle

--UNION

--SELECT     Modules.ModuleID, Modules.ModuleTitle
--FROM         GeneralModuleDefinitions INNER JOIN
--                      ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID INNER JOIN
--                      Modules ON ModuleDefinitions.ModuleDefID = Modules.ModuleDefID
--WHERE     (ModuleDefinitions.PortalID = @PortalID) AND (GeneralModuleDefinitions.FriendlyName = @ModuleName)

--ORDER BY Modules.ModuleTitle
--GO

--CREATE PROCEDURE GetModulesSinglePortal
--(
--    @PortalID  int
--)
--AS

--SELECT      0 ModuleID, 'NO_MODULE' ModuleTitle, -1 as TabOrder

--UNION

--	SELECT     Modules.ModuleID, Tabs.TabName + '/' + Modules.ModuleTitle + ' (' + GeneralModuleDefinitions.FriendlyName + ')' AS ModTitle, Tabs.TabOrder
--	FROM         Modules INNER JOIN
--	                      Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
--	                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--	                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--	WHERE     (Tabs.PortalID = @PortalID) AND (GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
--	                      GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
--	ORDER BY TabOrder, Modules.ModuleTitle
--GO

--CREATE PROCEDURE GetNextMessageID
--(
--    @ItemID int,
--    @NextID int OUTPUT
--)
--AS

--DECLARE @CurrentDisplayOrder as nvarchar(750)
--DECLARE @CurrentModule as int

--/* Find DisplayOrder of current item */
--SELECT
--    @CurrentDisplayOrder = DisplayOrder,
--    @CurrentModule = ModuleID
--FROM
--    Discussion
--WHERE
--    ItemID = @ItemID

--/* Get the next message in the same module */
--SELECT Top 1
--    @NextID = ItemID

--FROM
--    Discussion

--WHERE
--    DisplayOrder > @CurrentDisplayOrder
--    AND
--    ModuleID = @CurrentModule

--ORDER BY
--    DisplayOrder ASC

--/* end of this thread? */
--IF @@Rowcount < 1
--    SET @NextID = null
--GO

--CREATE PROCEDURE GetPortalCustomSettings
--(
--    @PortalID int
--)
--AS
--SELECT
--    SettingName,
--    SettingValue
--FROM
--    PortalSettings
--WHERE
--    PortalID = @PortalID
--GO

--/* returns all roles for the specified portal */
--CREATE PROCEDURE GetPortalRoles
--(
--    @PortalID  int
--)
--AS

--SELECT  
--    RoleName,
--    RoleID

--FROM
--    Roles

--WHERE   
--    PortalID = @PortalID

--order by RoleID
--/* questo assicura che l'ultimo inserito si in fondo alla lista */
--GO

--CREATE PROCEDURE GetPortalSettings
--(
--    @PortalAlias   nvarchar(50),
--    @TabID         int,
--    @PortalID      int OUTPUT,
--    @PortalName    nvarchar(128) OUTPUT,
--    @PortalPath    nvarchar(128) OUTPUT,
--    @AlwaysShowEditButton bit OUTPUT,
--    @TabName       nvarchar (50)  OUTPUT,
--    @TabOrder      int OUTPUT,
--    @ParentTabID      int OUTPUT,
--    @MobileTabName nvarchar (50)  OUTPUT,
--    @AuthRoles     nvarchar (256) OUTPUT,
--    @ShowMobile    bit OUTPUT
--)
--AS
--/* First, get Out Params */
--IF @TabID = 0 
--    SELECT TOP 1
--        @PortalID      = Portals.PortalID,
--        @PortalName    = Portals.PortalName,
--        @PortalPath    = Portals.PortalPath,
--        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
--        @TabID         = Tabs.TabID,
--        @TabOrder      = Tabs.TabOrder,
--        @ParentTabID   = Tabs.ParentTabID,
--        @TabName       = Tabs.TabName,
--        @MobileTabName = Tabs.MobileTabName,
--        @AuthRoles     = Tabs.AuthorizedRoles,
--        @ShowMobile    = Tabs.ShowMobile
--    FROM
--        Tabs
--    INNER JOIN
--        Portals ON Tabs.PortalID = Portals.PortalID
        
--    WHERE
--        PortalAlias=@PortalAlias
        
--    ORDER BY
--        TabOrder
--ELSE 
--    SELECT
--        @PortalID      = Portals.PortalID,
--        @PortalName    = Portals.PortalName,
--        @PortalPath    = Portals.PortalPath,
--        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
--        @TabName       = Tabs.TabName,
--        @TabOrder      = Tabs.TabOrder,
--        @ParentTabID   = Tabs.ParentTabID,
--        @MobileTabName = Tabs.MobileTabName,
--        @AuthRoles     = Tabs.AuthorizedRoles,
--        @ShowMobile    = Tabs.ShowMobile
--    FROM
--        Tabs
--    INNER JOIN
--        Portals ON Tabs.PortalID = Portals.PortalID
        
--    WHERE
--        TabID=@TabID AND Portals.PortalAlias=@PortalAlias

--/* Get Tabs list */
--SELECT  
--    TabName,
--    AuthorizedRoles,
--    TabID,
--    ParentTabID,
--    TabOrder    
--FROM    
--    Tabs
    
--WHERE   
--    PortalID = @PortalID AND ParentTabID IS NULL
    
--ORDER BY
--    TabOrder
    
--/* Get Mobile Tabs list */
--SELECT  
--    MobileTabName,
--    AuthorizedRoles,
--    TabID,
--    ParentTabID,
--    ShowMobile  
--FROM    
--    Tabs
    
--WHERE   
--    PortalID = @PortalID
--  AND
--    ShowMobile = 1
    
--ORDER BY
--    TabOrder
--/* Then, get the DataTable of module info */
--SELECT     *
--FROM         Modules INNER JOIN
--                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--WHERE     (Modules.TabID = @TabID)
--ORDER BY Modules.ModuleOrder
--GO

--CREATE PROCEDURE GetPortalSettingsPortalID
--(
--    @PortalID   nvarchar(50)
--)
--AS
--    SELECT     TOP 1 PortalID, PortalName, PortalPath, AlwaysShowEditButton, PortalAlias
--    FROM         Portals
--    WHERE     (PortalID = @PortalID)

--GO

--CREATE PROCEDURE GetPortals
--AS
--SELECT  Portals.PortalID, Portals.PortalAlias, Portals.PortalName, Portals.PortalPath, Portals.AlwaysShowEditButton
--FROM    Portals
--GO

--CREATE PROCEDURE GetPortalsModules
--(
--    @ModuleID  uniqueidentifier
--)
--AS
--	SELECT     Portals.PortalID, Portals.PortalAlias, Portals.PortalName, ModuleDefinitions.ModuleDefID
--	FROM         Portals LEFT OUTER JOIN
--	                      ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
--	WHERE     (ModuleDefinitions.GeneralModDefID = @ModuleID)

--GO

--CREATE PROCEDURE GetPrevMessageID
--(
--    @ItemID int,
--    @PrevID int OUTPUT
--)
--AS

--DECLARE @CurrentDisplayOrder as nvarchar(750)
--DECLARE @CurrentModule as int

--/* Find DisplayOrder of current item */
--SELECT
--    @CurrentDisplayOrder = DisplayOrder,
--    @CurrentModule = ModuleID
--FROM
--    Discussion
--WHERE
--    ItemID = @ItemID

--/* Get the previous message in the same module */
--SELECT Top 1
--    @PrevID = ItemID

--FROM
--    Discussion

--WHERE
--    DisplayOrder < @CurrentDisplayOrder
--    AND
--    ModuleID = @CurrentModule

--ORDER BY
--    DisplayOrder DESC

--/* already at the beginning of this module? */
--IF @@Rowcount < 1
--    SET @PrevID = null
--GO


--CREATE   PROCEDURE GetRelatedTables
--	@Name	nvarchar(128),
--	@Schema	nvarchar(128)
--AS
--	SELECT 
--		[InnerResults].[ForeignKeyTableSchema],
--		[InnerResults].[ForeignKeyTable], 
--		[InnerResults].[ForeignKeyColumn], 
--		[InnerResults].[KeyColumn],
--		[InnerResults].[ForeignKeyTableId],
--		[InnerResults].[KeyTableId],
--		[InnerResults].[KeyTableSchema],
--		[InnerResults].[KeyTable]
--	FROM
--		(
--			SELECT     
--				[FKeyTable].[TableName] AS ForeignKeyTable, 
--				[FKeyTable].[TableSchema] As ForeignKeyTableSchema,
--				[KeyTable].[TableName] AS KeyTable, 
--				[KeyTable].[TableSchema] As KeyTableSchema,
--				[FKeyColumns].[name] AS ForeignKeyColumn, 
--			        [KeyColumns].[name] AS KeyColumn,
--				[FKeyTable].[id] AS ForeignKeyTableId,
--				[KeyTable].[id] AS KeyTableId
--			FROM         sysforeignkeys INNER JOIN
--			                      (
--							SELECT     
--								[sysobjects].[id] As ID, 
--								[sysobjects].[name] AS TableName,
--								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
--							FROM    
--								[sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
--									ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
--							WHERE   
--								([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
--					       ) FKeyTable ON sysforeignkeys.fkeyid = [FKeyTable].[ID] INNER JOIN
--					       (
--							SELECT     
--								[sysobjects].[id] As ID, 
--								[sysobjects].[name] AS TableName,
--								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
--							FROM    
--								[sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
--									ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
--							WHERE   
--								([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
--					       ) KeyTable ON sysforeignkeys.rkeyid = [KeyTable].[ID] INNER JOIN
--			                      syscolumns FKeyColumns ON [FKeyTable].[ID] = [FKeyColumns].[id] AND sysforeignkeys.fkey = [FKeyColumns].[colid] INNER JOIN
--			                      syscolumns KeyColumns ON [KeyTable].[ID] = [KeyColumns].[id] AND sysforeignkeys.rkey = [KeyColumns].[colid]
--		) InnerResults
--	WHERE			
--		[InnerResults].[KeyTable] = @Name
--GO

--/* returns all members for the specified role */
--CREATE PROCEDURE GetRoleMembership
--(
--    @RoleID  int
--)
--AS

--SELECT  
--    UserRoles.UserID,
--    Name,
--    Email

--FROM
--    UserRoles
    
--INNER JOIN 
--    Users On Users.UserID = UserRoles.UserID

--WHERE   
--    UserRoles.RoleID = @RoleID
--GO

--/* returns all roles for the specified user */
--CREATE PROCEDURE GetRolesByUser
--(
--	@PortalID		int,
--    @Email         nvarchar(100)
--)
--AS

--SELECT  
--    Roles.RoleName,
--    Roles.RoleID

--FROM
--    UserRoles
--  INNER JOIN 
--    Users ON UserRoles.UserID = Users.UserID
--  INNER JOIN 
--    Roles ON UserRoles.RoleID = Roles.RoleID

--WHERE   
--    Users.Email = @Email AND Users.PortalID = @PortalID
--GO

--CREATE  PROCEDURE GetSingleAnnouncement
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    MoreLink,
--	    MobileMoreLink,
--	    ExpireDate,
--	    Description
--	FROM
--	    Announcements
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    MoreLink,
--	    MobileMoreLink,
--	    ExpireDate,
--	    Description
--	FROM
--	    staging.Announcements
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE  PROCEDURE GetSingleContact
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF (@WorkflowVersion = 1)
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Name,
--	    Role,
--	    Email,
--	    Contact1,
--	    Contact2
--	FROM
--	    Contacts
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Name,
--	    Role,
--	    Email,
--	    Contact1,
--	    Contact2
--	FROM
--	    staging.Contacts
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE  PROCEDURE GetSingleDocument
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    FileFriendlyName,
--	    FileNameUrl,
--	    CreatedByUser,
--	    CreatedDate,
--	    Category,
--	    ContentSize
--	FROM
--	    Documents
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    FileFriendlyName,
--	    FileNameUrl,
--	    CreatedByUser,
--	    CreatedDate,
--	    Category,
--	    ContentSize
--	FROM
--	    staging.Documents
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE  PROCEDURE GetSingleEvent
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF ( @WorkflowVersion = 1 )
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    ExpireDate,
--	    Description,
--	    WhereWhen	
--	FROM
--	    Events
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    ExpireDate,
--	    Description,
--	    WhereWhen	
--	FROM
--	    staging.Events
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE  PROCEDURE GetSingleLink
--(
--    @ItemID int,
--    @WorkflowVersion int
--)
--AS

--IF (@WorkflowVersion = 1)
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    Url,
--	    MobileUrl,
--	    ViewOrder,
--	    Description
--	FROM
--	    Links
--	WHERE
--	    ItemID = @ItemID
--ELSE
--	SELECT
--	    CreatedByUser,
--	    CreatedDate,
--	    Title,
--	    Url,
--	    MobileUrl,
--	    ViewOrder,
--	    Description
--	FROM
--	    staging.Links
--	WHERE
--	    ItemID = @ItemID
--GO

--CREATE PROCEDURE GetSingleMessage
--(
--    @ItemID int
--)
--AS

--DECLARE @nextMessageID int
--EXECUTE GetNextMessageID @ItemID, @nextMessageID OUTPUT
--DECLARE @prevMessageID int
--EXECUTE GetPrevMessageID @ItemID, @prevMessageID OUTPUT

--SELECT
--    ItemID,
--    Title,
--    CreatedByUser,
--    CreatedDate,
--    Body,
--    DisplayOrder,
--    NextMessageID = @nextMessageID,
--    PrevMessageID = @prevMessageID

--FROM
--    Discussion

--WHERE
--    ItemID = @ItemID
--GO

--CREATE PROCEDURE GetSingleModuleDefinition
--(
--    @GeneralModDefID uniqueidentifier
--)
--AS
--SELECT
--	GeneralModDefID, 
--    FriendlyName,
--    DesktopSrc,
--    MobileSrc,
--    Admin
--FROM
--    GeneralModuleDefinitions
--WHERE
--    GeneralModDefID = @GeneralModDefID
--GO

--CREATE PROCEDURE GetSingleRole
--(
--    @RoleID int
--)
--AS

--SELECT
--    RoleName

--FROM
--    Roles

--WHERE
--    RoleID = @RoleID
--GO

--CREATE PROCEDURE GetSingleUser
--(
--    @Email nvarchar(100),
--    @PortalID int,
--	@IDLang	nchar(2) = 'IT'
--)
--AS

--SELECT
--	Users.UserID,
--	Users.Email,
--	Users.Password,
--	Users.Name,
--	Users.Company,
--	Users.Address,
--	Users.City,
--	Users.Zip,
--	Users.IDCountry_FK,
--	Users.IDState_FK,
--	Users.PIva,
--	Users.CFiscale,
--	Users.Phone,
--	Users.Fax,
--	Users.SendNewsletter,
--	Users.MailChecked,
--	Users.PortalID,
--	States.Description AS State, 
--	CASE @IDLang
--		WHEN 'IT' THEN Countries.IT
--		WHEN 'EN' THEN Countries.EN
--		WHEN 'DE' THEN Countries.DE
--		WHEN 'FR' THEN Countries.FR
--		WHEN 'ES' THEN Countries.ES
--		WHEN 'PT' THEN Countries.PT
--		ELSE Countries.EN
--	END AS Country
					  
--FROM 
--	Users LEFT OUTER JOIN
--	Countries ON Users.IDCountry_FK = Countries.PK_IDCountry LEFT OUTER JOIN
--	States ON Users.IDState_FK = States.PK_IDState
	
--WHERE
--(Users.Email = @Email) AND (Users.PortalID = @PortalID)
--GO

--/* returns all module definitions for a specified solution */
--CREATE PROCEDURE GetSolutionModuleDefinitions
--(
--    @SolutionID  int
--)
--AS
--SELECT *
 
--FROM
--    SolutionModuleDefinitions
--WHERE   
--    SolutionsID = @SolutionID
--GO

--CREATE PROCEDURE GetSolutions
--AS
--SELECT * FROM Solutions
--GO

--CREATE PROCEDURE GetStates
--(
--	@IDCountry_FK nchar(2)
--)

--AS
--SELECT  Description, 
--		PK_IDState
--FROM    States
--WHERE	IDCountry_FK = @IDCountry_FK
--ORDER BY Description 
--GO

--CREATE PROCEDURE GetTabSettings
--(
--    @TabID   int
--)
--AS

--IF (@TabID > 0)

--/* Get Tabs list */
--SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
--FROM         Tabs
--WHERE     (ParentTabID = @TabID)
--ORDER BY TabOrder
--GO

--CREATE PROCEDURE GetTabsByPortal
--(
--    @PortalID   int
--)
--AS

--/* Get Tabs list */
--SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
--FROM         Tabs
--WHERE     (PortalID = @PortalID)
--ORDER BY TabOrder
--GO

--CREATE PROCEDURE GetTabsParent
--(
--	@PortalID int,
--	@TabID int
--)
--AS
--SELECT 0 TabID, ' ROOT_LEVEL' TabName

--UNION

--SELECT     Tabs.TabID, Tabs.TabName
--FROM       Tabs
--WHERE     (Tabs.TabID <> @TabID) AND (Tabs.PortalID = @PortalID)
--ORDER BY Tabs.TabName
--GO

--CREATE PROCEDURE GetTabsinTab
--(
--	@PortalID int,
--	@TabID int
--)
--AS
--SELECT     TabID, TabName, ParentTabID, TabOrder, AuthorizedRoles
--FROM         Tabs
--WHERE     (ParentTabID = @TabID) AND (PortalID = @PortalID)
--ORDER BY TabOrder
--GO

--CREATE PROCEDURE GetThreadMessages
--(
--    @Parent nvarchar(750)
--)
--AS

--SELECT
--    ItemID,
--    DisplayOrder,
--    REPLICATE( '&#160;', ( ( LEN( DisplayOrder ) / 23 ) - 1 ) * 5 ) AS Indent,
--    Title,  
--    CreatedByUser,
--    CreatedDate,
--    Body

--FROM 
--    Discussion

--WHERE
--    LEFT(DisplayOrder, 23) = @Parent
--  AND
--    (LEN( DisplayOrder ) / 23 ) > 1

--ORDER BY
--    DisplayOrder
--GO

--CREATE PROCEDURE GetTopLevelMessages
--(
--    @ModuleID int
--)
--AS

--SELECT
--    ItemID,
--    DisplayOrder,
--    LEFT(DisplayOrder, 23) AS Parent,    
--    (SELECT COUNT(*) -1  FROM Discussion Disc2 WHERE LEFT(Disc2.DisplayOrder,LEN(RTRIM(Disc.DisplayOrder))) = Disc.DisplayOrder) AS ChildCount,
--    Title,  
--    CreatedByUser,
--    CreatedDate

--FROM 
--    Discussion Disc

--WHERE 
--    ModuleID=@ModuleID
--  AND
--    (LEN( DisplayOrder ) / 23 ) = 1

--ORDER BY
--    DisplayOrder
--GO

--CREATE PROCEDURE GetUsers
--(
--@PortalID int
--)
--AS

--SELECT     UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, IDCountry_FK, IDState_FK, PIva, CFiscale, Phone, Fax
--FROM         Users
--WHERE     (PortalID = @PortalID)
--ORDER BY Email
--GO

--CREATE      PROCEDURE Publish
--	@ModuleID	int
--AS

--	-- First get al list of tables which are related to the Modules table

--	-- Create a temporary table
--	CREATE TABLE #TMPResults
--		(ForeignKeyTableSchema	nvarchar(128),
--                 ForeignKeyTable	nvarchar(128),
--		 ForeignKeyColumn	nvarchar(128),
--		 KeyColumn		nvarchar(128),
--		 ForeignKeyTableId	int,
--		 KeyTableId		int,
--		 KeyTableSchema		nvarchar(128),
--		 KeyTable		nvarchar(128))

--	INSERT INTO #TMPResults EXEC GetRelatedTables 'Modules', 'dbo'

--	DECLARE RelatedTablesModules CURSOR FOR
--		SELECT 	
--			ForeignKeyTableSchema, 
--			ForeignKeyTable,
--			ForeignKeyColumn,
--			KeyColumn,
--			ForeignKeyTableId,
--			KeyTableId,
--			KeyTableSchema,
--			KeyTable
--		FROM
--			#TMPResults
--		WHERE 
--			ForeignKeyTableSchema = 'dbo'
--			AND ForeignKeyTable <> 'ModuleSettings'

--	-- Create temporary table for later use
--	CREATE TABLE #TMPCount
--		(ResultCount	int)


--	-- Now search the table that has the related column
--	OPEN RelatedTablesModules

--	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
--	DECLARE @ForeignKeyTable	nvarchar(128)
--	DECLARE @ForeignKeyColumn	nvarchar(128)
--	DECLARE @KeyColumn		nvarchar(128)
--	DECLARE @ForeignKeyTableId	int
--	DECLARE @KeyTableId		int
--	DECLARE @KeyTableSchema		nvarchar(128)
--	DECLARE @KeyTable		nvarchar(128)
--	DECLARE @SQLStatement		nvarchar(4000)
--	DECLARE @Count			int
--	DECLARE @TableHasIdentityCol	int

--	FETCH NEXT FROM RelatedTablesModules 
--	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
--		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
--		@KeyTableSchema, @KeyTable
	
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--		-- Check if this table has a corresponding staging table
--		TRUNCATE TABLE #TMPCount
--		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[staging].[' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
--		-- PRINT @SQLStatement
--		EXEC(@SQLStatement)
--		SELECT @Count = ResultCount
--			FROM #TMPCount		
--		PRINT @ForeignKeyTable
--		PRINT @Count
--		IF (@Count = 1)
--		BEGIN						
--			-- Check if this table contains the related data
--			TRUNCATE TABLE #TMPCount
--			SET @SQLStatement = 
--				'INSERT INTO #TMPCount ' +
--				'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
--				'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
--				'UNION ' +
--				'SELECT Count(*) FROM [staging].[' + @ForeignKeyTable + '] ' +
--				'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

--			EXEC(@SQLStatement)
--			SELECT @Count = ResultCount
--				FROM #TMPCount		
--			IF (@Count >= 1) 
--			BEGIN
--				-- This table contains the related data 
--				-- Delete everything in the prod table
--				SET @SQLStatement = 
--					'DELETE FROM [' + @ForeignKeyTable + '] ' +
--					'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--				PRINT @SQLStatement
--				EXEC(@SQLStatement)
--				-- Copy everything from the staging table to the prod table
--				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
--				IF (@TableHasIdentityCol = 1)
--				BEGIN
--					-- The table contains a identity column
--					DECLARE TableColumns CURSOR FOR
--						SELECT [COLUMN_NAME]
--						FROM [INFORMATION_SCHEMA].[COLUMNS]
--						WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
--							AND [TABLE_NAME] = @ForeignKeyTable
--						ORDER BY [ORDINAL_POSITION]
	
--					OPEN TableColumns
	
--					DECLARE @ColumnList	nvarchar(4000)
--					SET @ColumnList = ''
--					DECLARE @ColName	nvarchar(128)
--					DECLARE @ColIsIdentity	int
	
--					FETCH NEXT FROM TableColumns
--					INTO @ColName
					
--					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
--					WHILE @@FETCH_STATUS = 0
--					BEGIN
--						IF (@ColIsIdentity = 0)
--							SET @ColumnList = @ColumnList + '[' + @ColName + '] '
	
--						FETCH NEXT FROM TableColumns
--						INTO @ColName
	
--						IF (@@FETCH_STATUS = 0)
--						BEGIN
--							IF (@ColIsIdentity = 0)
--							BEGIN
--								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
--								IF (@ColIsIdentity = 0)
--									SET @ColumnList = @ColumnList + ', '		
--							END
--							ELSE
--								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
--						END
--					END
					
--					CLOSE TableColumns
--					DEALLOCATE TableColumns		
	
--					SET @SQLStatement = 	
--						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
--						'SELECT ' + @ColumnList + ' FROM [staging].[' + @ForeignKeyTable + '] ' +
--						'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--					-- PRINT @SQLStatement
--					EXEC(@SQLStatement)
--				END
--				ELSE
--				BEGIN
--					-- The table doens't contain a identitycolumn
--					SET @SQLStatement = 
--						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
--						'SELECT * FROM [staging].[' + @ForeignKeyTable + '] ' +
--						'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--					EXEC(@SQLStatement)
--				END
--			END
--		END

--		FETCH NEXT FROM RelatedTablesModules 
--		INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
--			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
--			@KeyTableSchema, @KeyTable
--	END


--	CLOSE RelatedTablesModules
--	DEALLOCATE RelatedTablesModules
			
--	-- Set the NewVersion parameter of the module to false
--	UPDATE [Modules]
--	SET [NewVersion] = 1
--	WHERE [ModuleID] = @ModuleID

--	RETURN
--GO

--CREATE   PROCEDURE ModuleEdited
--	@ModuleID	int
--AS

--	-- Check if this module supports workflow
--	DECLARE @support	bit

--	SELECT @support = SupportWorkflow
--	FROM Modules
--	WHERE ModuleID = @ModuleID

--	IF ( @support = 1 )
--	BEGIN
--		-- It supports workflow
--		UPDATE Modules
--		SET NewVersion = 0
--		WHERE ModuleID = @ModuleID
--	END
--	ELSE
--		-- It doesn't support workflow
--		EXEC Publish @ModuleID

--	/* SET NOCOUNT ON */
--	RETURN 
--GO

--CREATE  PROCEDURE UpdateAnnouncement
--(
--    @ItemID         int,
--    @UserName       nvarchar(100),
--    @Title          nvarchar(150),
--    @MoreLink       nvarchar(150),
--    @MobileMoreLink nvarchar(150),
--    @ExpireDate     datetime,
--    @Description    nvarchar(2000)
--)
--AS

--UPDATE
--    Staging.Announcements

--SET
--    CreatedByUser   = @UserName,
--    CreatedDate     = GetDate(),
--    Title           = @Title,
--    MoreLink        = @MoreLink,
--    MobileMoreLink  = @MobileMoreLink,
--    ExpireDate      = @ExpireDate,
--    Description     = @Description

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE UpdateContact
--(
--    @ItemID   int,
--    @UserName nvarchar(100),
--    @Name     nvarchar(50),
--    @Role     nvarchar(100),
--    @Email    nvarchar(100),
--    @Contact1 nvarchar(250),
--    @Contact2 nvarchar(250)
--)
--AS

--UPDATE
--    Staging.Contacts

--SET
--    CreatedByUser = @UserName,
--    CreatedDate   = GetDate(),
--    Name          = @Name,
--    Role          = @Role,
--    Email         = @Email,
--    Contact1      = @Contact1,
--    Contact2      = @Contact2

--WHERE
--    ItemID = @ItemID
--GO

--CREATE   PROCEDURE UpdateDocument
--(
--    @ItemID           int,
--    @ModuleID         int,
--    @FileFriendlyName nvarchar(150),
--    @FileNameUrl      nvarchar(250),
--    @UserName         nvarchar(100),
--    @Category         nvarchar(50),
--    @Content          image,
--    @ContentType      nvarchar(50),
--    @ContentSize      int
--)
--AS
--IF (@ItemID=0) OR NOT EXISTS (
--    SELECT 
--        * 
--    FROM 
--        staging.Documents 
--    WHERE 
--        ItemID = @ItemID
--)
--INSERT INTO Staging.Documents
--(
--    ModuleID,
--    FileFriendlyName,
--    FileNameUrl,
--    CreatedByUser,
--    CreatedDate,
--    Category,
--    Content,
--    ContentType,
--    ContentSize
--)

--VALUES
--(
--    @ModuleID,
--    @FileFriendlyName,
--    @FileNameUrl,
--    @UserName,
--    GetDate(),
--    @Category,
--    @Content,
--    @ContentType,
--    @ContentSize
--)
--ELSE

--BEGIN

--IF (@ContentSize=0)

--UPDATE 
--    Staging.Documents

--SET 
--    CreatedByUser    = @UserName,
--    CreatedDate      = GetDate(),
--    Category         = @Category,
--    FileFriendlyName = @FileFriendlyName,
--    FileNameUrl      = @FileNameUrl

--WHERE
--    ItemID = @ItemID
--ELSE

--UPDATE
--    Staging.Documents

--SET
--    CreatedByUser     = @UserName,
--    CreatedDate       = GetDate(),
--    Category          = @Category,
--    FileFriendlyName  = @FileFriendlyName,
--    FileNameUrl       = @FileNameUrl,
--    Content           = @Content,
--    ContentType       = @ContentType,
--    ContentSize       = @ContentSize

--WHERE
--    ItemID = @ItemID

--END
--GO

--CREATE  PROCEDURE UpdateEvent
--(
--    @ItemID      int,
--    @UserName    nvarchar(100),
--    @Title       nvarchar(100),
--    @ExpireDate  datetime,
--    @Description nvarchar(2000),
--    @WhereWhen   nvarchar(100)
--)

--AS

--UPDATE
--    Staging.Events

--SET
--    CreatedByUser = @UserName,
--    CreatedDate   = GetDate(),
--    Title         = @Title,
--    ExpireDate    = @ExpireDate,
--    Description   = @Description,
--    WhereWhen     = @WhereWhen

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE UpdateHtmlText
--(
--    @ModuleID      int,
--    @DesktopHtml   ntext,
--    @MobileSummary ntext,
--    @MobileDetails ntext
--)
--AS

--IF NOT EXISTS (
--    SELECT 
--        * 
--    FROM 
--        Staging.HtmlText 
--    WHERE 
--        ModuleID = @ModuleID
--)
--INSERT INTO Staging.HtmlText (
--    ModuleID,
--    DesktopHtml,
--    MobileSummary,
--    MobileDetails
--) 
--VALUES (
--    @ModuleID,
--    @DesktopHtml,
--    @MobileSummary,
--    @MobileDetails
--)
--ELSE
--UPDATE
--    Staging.HtmlText

--SET
--    DesktopHtml   = @DesktopHtml,
--    MobileSummary = @MobileSummary,
--    MobileDetails = @MobileDetails

--WHERE
--    ModuleID = @ModuleID
--GO

--CREATE  PROCEDURE UpdateLink
--(
--    @ItemID      int,
--    @UserName    nvarchar(100),
--    @Title       nvarchar(100),
--    @Url         nvarchar(250),
--    @MobileUrl   nvarchar(250),
--    @ViewOrder   int,
--    @Description nvarchar(2000)
--)
--AS

--UPDATE
--    Staging.Links

--SET
--    CreatedByUser = @UserName,
--    CreatedDate   = GetDate(),
--    Title         = @Title,
--    Url           = @Url,
--    MobileUrl     = @MobileUrl,
--    ViewOrder     = @ViewOrder,
--    Description   = @Description

--WHERE
--    ItemID = @ItemID
--GO

--CREATE  PROCEDURE UpdateModule
--(
--    @ModuleID               int,
--    @ModuleOrder            int,
--    @ModuleTitle            nvarchar(256),
--    @PaneName               nvarchar(50),
--    @CacheTime              int,
--    @EditRoles              nvarchar(256),
--    @AddRoles               nvarchar(256),
--    @ViewRoles              nvarchar(256),
--    @DeleteRoles            nvarchar(256),
--    @PropertiesRoles        nvarchar(256),
--    @ShowMobile             bit,
--    @PublishingRoles	    nvarchar(256),
--    @SupportWorkflow	    bit
--)
--AS
--UPDATE
--    Modules
--SET
--    ModuleOrder               = @ModuleOrder,
--    ModuleTitle               = @ModuleTitle,
--    PaneName                  = @PaneName,
--    CacheTime                 = @CacheTime,
--    ShowMobile                = @ShowMobile,
--    AuthorizedEditRoles       = @EditRoles,
--    AuthorizedAddRoles        = @AddRoles,
--    AuthorizedViewRoles       = @ViewRoles,
--    AuthorizedDeleteRoles     = @DeleteRoles,
--    AuthorizedPropertiesRoles = @PropertiesRoles,
--    AuthorizedPublishingRoles = @PublishingRoles,
--    SupportWorkflow	      = @SupportWorkflow
    
--WHERE
--    ModuleID = @ModuleID
--GO

--CREATE PROCEDURE UpdateModuleDefinition
--(
--    @GeneralModDefID	uniqueidentifier,
--    @FriendlyName		nvarchar(128),
--    @DesktopSrc			nvarchar(256),
--    @Admin				bit,
--    @MobileSrc			nvarchar(256)
--)
--AS
--UPDATE
--    GeneralModuleDefinitions
--SET
--    FriendlyName = @FriendlyName,
--    DesktopSrc   = @DesktopSrc,
--    MobileSrc    = @MobileSrc,
--    Admin		 = @Admin
--WHERE
--    GeneralModDefID =  @GeneralModDefID
--GO

--CREATE PROCEDURE UpdateModuleDefinitions
--(
--    @GeneralModDefID	uniqueidentifier,
--    @PortalID			int,
--    @ischecked			bit
--)
--AS

--IF (@ischecked = 0)
--	/*DELETE IF CLEARED */
--	DELETE FROM ModuleDefinitions WHERE ModuleDefinitions.GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID
	
--ELSE
--IF NOT (EXISTS (SELECT ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID))
--	/* ADD IF CHECKED */
--BEGIN
--			INSERT INTO ModuleDefinitions
--			(
--				PortalID,
--				GeneralModDefID
--			)
--			VALUES
--			(
--				@PortalID,
--				@GeneralModDefID
--			)
--END
--GO

--CREATE PROCEDURE UpdateModuleOrder
--(
--    @ModuleID           int,
--    @ModuleOrder        int,
--    @PaneName           nvarchar(50)
--)
--AS
--UPDATE
--    Modules
--SET
--    ModuleOrder = @ModuleOrder,
--    PaneName    = @PaneName
--WHERE
--    ModuleID = @ModuleID
--GO

--CREATE PROCEDURE UpdateModuleSetting
--(
--    @ModuleID      int,
--    @SettingName   nvarchar(50),
--    @SettingValue  nvarchar(256)
--)
--AS
--IF NOT EXISTS (
--    SELECT 
--        * 
--    FROM 
--        ModuleSettings 
--    WHERE 
--        ModuleID = @ModuleID
--      AND
--        SettingName = @SettingName
--)
--INSERT INTO ModuleSettings (
--    ModuleID,
--    SettingName,
--    SettingValue
--) 
--VALUES (
--    @ModuleID,
--    @SettingName,
--    @SettingValue
--)
--ELSE
--UPDATE
--    ModuleSettings
--SET
--    SettingValue = @SettingValue
--WHERE
--    ModuleID = @ModuleID
--  AND
--    SettingName = @SettingName
--GO

--CREATE PROCEDURE UpdatePortalInfo
--(
--    @PortalID           int,
--    @PortalName         nvarchar(128),
--    @PortalPath         nvarchar(128),
--    @AlwaysShowEditButton bit 
--)
--AS

--UPDATE
--    Portals

--SET
--    PortalName = @PortalName,
--    PortalPath = @PortalPath,
--    AlwaysShowEditButton = @AlwaysShowEditButton

--WHERE
--    PortalID = @PortalID
--GO

--CREATE PROCEDURE UpdatePortalSetting
--(
--    @PortalID      int,
--    @SettingName   nvarchar(50),
--    @SettingValue  nvarchar(256)
--)
--AS
--IF NOT EXISTS (
--    SELECT 
--        * 
--    FROM 
--        PortalSettings 
--    WHERE 
--        PortalID = @PortalID
--      AND
--        SettingName = @SettingName
--)
--INSERT INTO PortalSettings (
--    PortalID,
--    SettingName,
--    SettingValue
--) 
--VALUES (
--    @PortalID,
--    @SettingName,
--    @SettingValue
--)
--ELSE
--UPDATE
--    PortalSettings
--SET
--    SettingValue = @SettingValue
--WHERE
--    PortalID = @PortalID
--  AND
--    SettingName = @SettingName
--GO

--CREATE PROCEDURE UpdateRole
--(
--    @RoleID      int,
--    @RoleName    nvarchar(50)
--)
--AS

--UPDATE
--    Roles

--SET
--    RoleName = @RoleName

--WHERE
--    RoleID = @RoleID
--GO

--CREATE PROCEDURE UpdateTab
--(
--    @PortalID        int,
--    @TabID           int,
--    @ParentTabID     int,
--    @TabOrder        int,
--    @TabName         nvarchar(50),
--    @AuthorizedRoles nvarchar(256),
--    @MobileTabName   nvarchar(50),
--    @ShowMobile      bit
--)
--AS

--IF (@ParentTabID = 0) 
--    SET @ParentTabID = NULL

--IF NOT EXISTS
--(
--    SELECT 
--        * 
--    FROM 
--        Tabs
--    WHERE 
--        TabID = @TabID
--)
--INSERT INTO Tabs (
--    PortalID,
--    ParentTabID,
--    TabOrder,
--    TabName,
--    AuthorizedRoles,
--    MobileTabName,
--    ShowMobile
--) 
--VALUES (
--    @PortalID,
--    @TabOrder,
--    @ParentTabID,
--    @TabName,
--    @AuthorizedRoles,
--    @MobileTabName,
--    @ShowMobile
   
--)
--ELSE
--UPDATE
--    Tabs
--SET
--    ParentTabID = @ParentTabID,
--    TabOrder = @TabOrder,
--    TabName = @TabName,
--    AuthorizedRoles = @AuthorizedRoles,
--    MobileTabName = @MobileTabName,
--    ShowMobile = @ShowMobile
--WHERE
--    TabID = @TabID
--GO

--CREATE PROCEDURE UpdateTabOrder
--(
--    @TabID           int,
--    @TabOrder        int
--)
--AS

--UPDATE
--    Tabs

--SET
--    TabOrder = @TabOrder

--WHERE
--    TabID = @TabID
--GO

--CREATE PROCEDURE UpdateUser
--(
--	@PortalID		int,
--    @UserID         int,
--    @Name			nvarchar(50),
--    @Email          nvarchar(100),
--    @Password	    nvarchar(20),
--    @SendNewsletter bit
--)
--AS

--UPDATE
--    Users

--SET
--    Name	 = @Name,
--    Email    = @Email,
--    Password = @Password,
--    PortalID = @PortalID,
--    SendNewsletter = @SendNewsletter

--WHERE
--    UserID    = @UserID
--GO

--CREATE PROCEDURE UserLogin
--(
--    @PortalID int,
--    @Email    nvarchar(100),
--    @Password nvarchar(20),
--    @UserName nvarchar(100) OUTPUT
--)
--AS

--SELECT     @UserName = Users.Name
--FROM      Users
--WHERE
--	(
--	Users.Email = @Email AND Users.Password = @Password AND Users.PortalID = @PortalID
--	)
--GO

---- End change marcb@hotmail.com
---- End Change Geert.Audenaert@Syntegra.Com

----Manu - Fix for Desktop tabs
--ALTER PROCEDURE GetPortalSettings
--(
--    @PortalAlias   nvarchar(50),
--    @TabID         int,
--    @PortalID      int OUTPUT,
--    @PortalName    nvarchar(128) OUTPUT,
--    @PortalPath    nvarchar(128) OUTPUT,
--    @AlwaysShowEditButton bit OUTPUT,
--    @TabName       nvarchar (50)  OUTPUT,
--    @TabOrder      int OUTPUT,
--    @ParentTabID      int OUTPUT,
--    @MobileTabName nvarchar (50)  OUTPUT,
--    @AuthRoles     nvarchar (256) OUTPUT,
--    @ShowMobile    bit OUTPUT
--)
--AS
--/* First, get Out Params */
--IF @TabID = 0 
--    SELECT TOP 1
--        @PortalID      = Portals.PortalID,
--        @PortalName    = Portals.PortalName,
--        @PortalPath    = Portals.PortalPath,
--        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
--        @TabID         = Tabs.TabID,
--        @TabOrder      = Tabs.TabOrder,
--        @ParentTabID   = Tabs.ParentTabID,
--        @TabName       = Tabs.TabName,
--        @MobileTabName = Tabs.MobileTabName,
--        @AuthRoles     = Tabs.AuthorizedRoles,
--        @ShowMobile    = Tabs.ShowMobile
--    FROM
--        Tabs
--    INNER JOIN
--        Portals ON Tabs.PortalID = Portals.PortalID
        
--    WHERE
--        PortalAlias=@PortalAlias
        
--    ORDER BY
--        TabOrder
--ELSE 
--    SELECT
--        @PortalID      = Portals.PortalID,
--        @PortalName    = Portals.PortalName,
--        @PortalPath    = Portals.PortalPath,
--        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
--        @TabName       = Tabs.TabName,
--        @TabOrder      = Tabs.TabOrder,
--        @ParentTabID   = Tabs.ParentTabID,
--        @MobileTabName = Tabs.MobileTabName,
--        @AuthRoles     = Tabs.AuthorizedRoles,
--        @ShowMobile    = Tabs.ShowMobile
--    FROM
--        Tabs
--    INNER JOIN
--        Portals ON Tabs.PortalID = Portals.PortalID
        
--    WHERE
--        TabID=@TabID AND Portals.PortalAlias=@PortalAlias

--/* Get Tabs list */
--SELECT  
--    TabName,
--    AuthorizedRoles,
--    TabID,
--    ParentTabID,
--    TabOrder    
--FROM    
--    Tabs
    
--WHERE   
--    PortalID = @PortalID
    
--ORDER BY
--    TabOrder
    
--/* Get Mobile Tabs list */
--SELECT  
--    MobileTabName,
--    AuthorizedRoles,
--    TabID,
--    ParentTabID,
--    ShowMobile  
--FROM    
--    Tabs
    
--WHERE   
--    PortalID = @PortalID
--  AND
--    ShowMobile = 1
    
--ORDER BY
--    TabOrder
--/* Then, get the DataTable of module info */
--SELECT     *
--FROM         Modules INNER JOIN
--                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
--                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
--WHERE     (Modules.TabID = @TabID)
--ORDER BY Modules.ModuleOrder
--GO
----end manu - fix desktop tab

--/* Crea nuova tabella "Cultures".                                                             */
--/* "Cultures" : Table of Cultures.                                                            */
--/* 	This is almost fixed.                                                                     */
--/* 	You can find here:                                                                        */
--/* 	http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpref/html/frlrfsystemglobalizationcultureinfoclasstopic.asp */
--/* 	"NeutralCode" : CultureCode identifies Cultures.                                          */
--/* 	Culture Name follow the RFC 1766 standard in the format "<languagecode2>-<country/regioncode2>", WHERE <languagecode2> is a lowercase two-letter code derived from ISO 639-1 AND <country/regioncode2> is an uppercase two-letter code derived from ISO 3166. For example, U.S. English is "en-US". Some culture names have prefixes that specify the script; for example, "Cy-" specifies the Cyrillic script, "Lt-" specifies the Latin script. */
--/* 	"LanguageCode" : Language is the neutral culture of the current culture.                  */
--/* 	A neutral culture is a culture that is associated with a language but not with a country/region.  */
--/* 	"CountryRegionCode" : CountryRegionCode is the country name (official short names in ENGLISH) in as given in ISO 3166-1 AND the corresponding ISO 3166-1-Alpha-2 code elements. */
--/* 	"Description" : Localizable description                                                   */
--/* 	"Identifier" : The culture identifier is mapped to the corresponding National Language Support (NLS) locale identifier. */  
--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Cultures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--create table [Cultures] ( 
--	[CultureCode] char(10) not null,
--	[NeutralCode] char(2) null,
--	[CountryRegionCode] char(2) null,
--	[Description] nvarchar(64) null,
--	[Identifier] int null)
--GO

--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Cultures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--alter table [Cultures]
--	add constraint [Cultures_PK] primary key clustered ([CultureCode])   
--GO
----end cultures table=======

--/****** 
--john.mandia@whitelightsolutions.com : 
--TabCustomSettings 
--******/

--/****** Object:  Table [TabSettings]    Script Date: 24/02/2003 12:27:45 ******/
--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[TabSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--begin
--CREATE TABLE [TabSettings] (
--	[TabID] [int] NOT NULL ,
--	[SettingName] [nvarchar] (50) NOT NULL ,
--	[SettingValue] [nvarchar] (256) NOT NULL 
--) ON [PRIMARY]
--END
--GO

--/****** Object:  Stored PROCEDURE GetTabCustomSettings    Script Date: 24/02/2003 12:29:43 ******/
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetTabCustomSettings]
--GO

--CREATE PROCEDURE GetTabCustomSettings
--(
--    @TabID int
--)
--AS
--SELECT
--    SettingName,
--    SettingValue
--FROM
--    TabSettings
--WHERE
--    TabID = @TabID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateTabCustomSettings]
--GO

--CREATE PROCEDURE UpdateTabCustomSettings
--(
--    @TabID int,
--    @SettingName   nvarchar(50),
--    @SettingValue  nvarchar(256)
--)
--AS
--IF NOT EXISTS (
--    SELECT 
--        * 
--    FROM 
--        TabSettings 
--    WHERE 
--        TabID = @TabID
--      AND
--        SettingName = @SettingName
--)
--INSERT INTO TabSettings (
--    TabID,
--    SettingName,
--    SettingValue
--) 
--VALUES (
--    @TabID,
--    @SettingName,
--    @SettingValue
--)
--ELSE
--UPDATE
--    TabSettings
--SET
--    SettingValue = @SettingValue
--WHERE
--    TabID = @TabID
--  AND
--    SettingName = @SettingName
--GO

----end john.mandia@whitelightsolutions.com changes

---- DB changes for extended workflow
---- By Geert.Audenaert@Syntegra.Com
--IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_NAME] = 'Modules'  AND [COLUMN_NAME] = 'AuthorizedApproveRoles') 
--BEGIN
--	-- Alter modules table
--	ALTER TABLE Modules ADD
--		AuthorizedApproveRoles nvarchar(256) NULL,
--		WorkflowState tinyint NULL
--END
--GO

--IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS] WHERE [TABLE_NAME] = 'Modules' AND [CONSTRAINT_NAME] = 'DF_Modules_NewVersion')
--BEGIN
--	ALTER TABLE Modules
--		DROP CONSTRAINT DF_Modules_NewVersion
--END
--GO

--IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[TABLE_CONSTRAINTS] WHERE [TABLE_NAME] = 'Modules' AND [CONSTRAINT_NAME] = 'DF_Modules_WorkflowState')
--BEGIN
--	ALTER TABLE Modules ADD CONSTRAINT
--		DF_Modules_WorkflowState DEFAULT 0 FOR WorkflowState
--END
--GO

--IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_NAME] = 'Modules' AND [COLUMN_NAME] = 'NewVersion') 
--BEGIN
--	ALTER TABLE Modules
--		DROP COLUMN NewVersion
--END 
--GO

---- Alter ModuleEdited stored procedure
--ALTER    PROCEDURE ModuleEdited
--	@ModuleID	int
--AS

--	-- Check if this module supports workflow
--	DECLARE @support	bit

--	SELECT @support = SupportWorkflow
--	FROM Modules
--	WHERE ModuleID = @ModuleID

--	IF ( @support = 1 )
--	BEGIN
--		-- It supports workflow
--		UPDATE Modules
--		SET WorkflowState = 1 -- Working
--		WHERE ModuleID = @ModuleID
--	END
--	ELSE
--		-- It doesn't support workflow
--		EXEC Publish @ModuleID

--	/* SET NOCOUNT ON */
--	RETURN 
--GO

---- Alter Publish stored procedure

--ALTER PROCEDURE Publish
--	@ModuleID	int
--AS

--	-- First get al list of tables which are related to the Modules table

--	-- Create a temporary table
--	CREATE TABLE #TMPResults
--		(ForeignKeyTableSchema	nvarchar(128),
--                 ForeignKeyTable	nvarchar(128),
--		 ForeignKeyColumn	nvarchar(128),
--		 KeyColumn		nvarchar(128),
--		 ForeignKeyTableId	int,
--		 KeyTableId		int,
--		 KeyTableSchema		nvarchar(128),
--		 KeyTable		nvarchar(128))

--	INSERT INTO #TMPResults EXEC GetRelatedTables 'Modules', 'dbo'

--	DECLARE RelatedTablesModules CURSOR FOR
--		SELECT 	
--			ForeignKeyTableSchema, 
--			ForeignKeyTable,
--			ForeignKeyColumn,
--			KeyColumn,
--			ForeignKeyTableId,
--			KeyTableId,
--			KeyTableSchema,
--			KeyTable
--		FROM
--			#TMPResults
--		WHERE 
--			ForeignKeyTableSchema = 'dbo'
--			AND ForeignKeyTable <> 'ModuleSettings'

--	-- Create temporary table for later use
--	CREATE TABLE #TMPCount
--		(ResultCount	int)


--	-- Now search the table that has the related column
--	OPEN RelatedTablesModules

--	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
--	DECLARE @ForeignKeyTable	nvarchar(128)
--	DECLARE @ForeignKeyColumn	nvarchar(128)
--	DECLARE @KeyColumn		nvarchar(128)
--	DECLARE @ForeignKeyTableId	int
--	DECLARE @KeyTableId		int
--	DECLARE @KeyTableSchema		nvarchar(128)
--	DECLARE @KeyTable		nvarchar(128)
--	DECLARE @SQLStatement		nvarchar(4000)
--	DECLARE @Count			int
--	DECLARE @TableHasIdentityCol	int

--	FETCH NEXT FROM RelatedTablesModules 
--	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
--		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
--		@KeyTableSchema, @KeyTable
	
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
--		-- Check if this table has a corresponding staging table
--		TRUNCATE TABLE #TMPCount
--		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[staging].[' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
--		-- PRINT @SQLStatement
--		EXEC(@SQLStatement)
--		SELECT @Count = ResultCount
--			FROM #TMPCount		
--		PRINT @ForeignKeyTable
--		PRINT @Count
--		IF (@Count = 1)
--		BEGIN						
--			-- Check if this table contains the related data
--			TRUNCATE TABLE #TMPCount
--			SET @SQLStatement = 
--				'INSERT INTO #TMPCount ' +
--				'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
--				'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
--				'UNION ' +
--				'SELECT Count(*) FROM [staging].[' + @ForeignKeyTable + '] ' +
--				'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

--			EXEC(@SQLStatement)
--			SELECT @Count = ResultCount
--				FROM #TMPCount		
--			IF (@Count >= 1) 
--			BEGIN
--				-- This table contains the related data 
--				-- Delete everything in the prod table
--				SET @SQLStatement = 
--					'DELETE FROM [' + @ForeignKeyTable + '] ' +
--					'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--				PRINT @SQLStatement
--				EXEC(@SQLStatement)
--				-- Copy everything from the staging table to the prod table
--				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
--				IF (@TableHasIdentityCol = 1)
--				BEGIN
--					-- The table contains a identity column
--					DECLARE TableColumns CURSOR FOR
--						SELECT [COLUMN_NAME]
--						FROM [INFORMATION_SCHEMA].[COLUMNS]
--						WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
--							AND [TABLE_NAME] = @ForeignKeyTable
--						ORDER BY [ORDINAL_POSITION]
	
--					OPEN TableColumns
	
--					DECLARE @ColumnList	nvarchar(4000)
--					SET @ColumnList = ''
--					DECLARE @ColName	nvarchar(128)
--					DECLARE @ColIsIdentity	int
	
--					FETCH NEXT FROM TableColumns
--					INTO @ColName
					
--					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
--					WHILE @@FETCH_STATUS = 0
--					BEGIN
--						IF (@ColIsIdentity = 0)
--							SET @ColumnList = @ColumnList + '[' + @ColName + '] '
	
--						FETCH NEXT FROM TableColumns
--						INTO @ColName
	
--						IF (@@FETCH_STATUS = 0)
--						BEGIN
--							IF (@ColIsIdentity = 0)
--							BEGIN
--								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
--								IF (@ColIsIdentity = 0)
--									SET @ColumnList = @ColumnList + ', '		
--							END
--							ELSE
--								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
--						END
--					END
					
--					CLOSE TableColumns
--					DEALLOCATE TableColumns		
	
--					SET @SQLStatement = 	
--						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
--						'SELECT ' + @ColumnList + ' FROM [staging].[' + @ForeignKeyTable + '] ' +
--						'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--					-- PRINT @SQLStatement
--					EXEC(@SQLStatement)
--				END
--				ELSE
--				BEGIN
--					-- The table doens't contain a identitycolumn
--					SET @SQLStatement = 
--						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
--						'SELECT * FROM [staging].[' + @ForeignKeyTable + '] ' +
--						'WHERE [staging].[' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
--					EXEC(@SQLStatement)
--				END
--			END
--		END

--		FETCH NEXT FROM RelatedTablesModules 
--		INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
--			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
--			@KeyTableSchema, @KeyTable
--	END


--	CLOSE RelatedTablesModules
--	DEALLOCATE RelatedTablesModules
			
--	-- Set the NewVersion parameter of the module to false
--	UPDATE [Modules]
--	SET [WorkflowState] = 0 -- Original
--	WHERE [ModuleID] = @ModuleID

--	RETURN
--GO

--/****** Oggetto: stored procedure UpdateModule    Data dello script: 07/11/2002 22.28.12 ******/
--ALTER   PROCEDURE UpdateModule
--(
--    @ModuleID               int,
--    @ModuleOrder            int,
--    @ModuleTitle            nvarchar(256),
--    @PaneName               nvarchar(50),
--    @CacheTime              int,
--    @EditRoles              nvarchar(256),
--    @AddRoles               nvarchar(256),
--    @ViewRoles              nvarchar(256),
--    @DeleteRoles            nvarchar(256),
--    @PropertiesRoles        nvarchar(256),
--    @ShowMobile             bit,
--    @PublishingRoles	    nvarchar(256),
--    @SupportWorkflow	    bit,
--    @ApprovalRoles	    nvarchar(256)
--)
--AS
--UPDATE
--    Modules
--SET
--    ModuleOrder               = @ModuleOrder,
--    ModuleTitle               = @ModuleTitle,
--    PaneName                  = @PaneName,
--    CacheTime                 = @CacheTime,
--    ShowMobile                = @ShowMobile,
--    AuthorizedEditRoles       = @EditRoles,
--    AuthorizedAddRoles        = @AddRoles,
--    AuthorizedViewRoles       = @ViewRoles,
--    AuthorizedDeleteRoles     = @DeleteRoles,
--    AuthorizedPropertiesRoles = @PropertiesRoles,
--    AuthorizedPublishingRoles = @PublishingRoles,
--    SupportWorkflow	      = @SupportWorkflow,
--    AuthorizedApproveRoles    = @ApprovalRoles
    
--WHERE
--    ModuleID = @ModuleID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[RequestApproval]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [RequestApproval]
--GO

--CREATE PROCEDURE RequestApproval
--	@moduleID	int
--AS
--	UPDATE	Modules
--	SET
--		WorkflowState = 2 -- Request Approval
--	WHERE
--		[ModuleID] = @moduleID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Approve]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [Approve]
--GO

--CREATE PROCEDURE Approve
--	@moduleID	int
--AS
--	UPDATE	Modules
--	SET
--		WorkflowState = 3 -- Approved
--	WHERE
--		[ModuleID] = @moduleID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Reject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [Reject]
--GO

--CREATE PROCEDURE Reject
--	@moduleID	int
--AS
--	UPDATE	Modules
--	SET
--		WorkflowState = 1 -- Put status back to Working
--	WHERE
--		[ModuleID] = @moduleID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthApproveRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthApproveRoles]
--GO

--CREATE  PROCEDURE GetAuthApproveRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @ApproveRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @ApproveRoles   = Modules.AuthorizedApproveRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPublishRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetAuthPublishRoles]
--GO

--CREATE  PROCEDURE GetAuthPublishRoles
--(
--    @PortalID    int,
--    @ModuleID    int,
--    @AccessRoles nvarchar (256) OUTPUT,
--    @PublishRoles   nvarchar (256) OUTPUT
--)
--AS

--SELECT  
--    @AccessRoles = Tabs.AuthorizedRoles,
--    @PublishRoles   = Modules.AuthorizedPublishingRoles
    
--FROM    
--    Modules
--  INNER JOIN
--    Tabs ON Modules.TabID = Tabs.TabID
    
--WHERE   
--    Modules.ModuleID = @ModuleID
--  AND
--    Tabs.PortalID = @PortalID

--GO

---- end changes Geert Audenaert

----This patch add entries on db for Articles module
----by manu
----IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions WHERE GeneralModDefID = '{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}')
----BEGIN
------Insert data into GeneralModuleDefinitions
----INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}',NULL,'Articles','DesktopModules/Articles.ascx','',0)
------Insert data into ModuleDefinitions
----INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0','{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}')
----END
----GO

--IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Articles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
-- BEGIN
--CREATE TABLE [Articles] (
--	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL ,
--	[CreatedByUser] [nvarchar] (100) NULL ,
--	[CreatedDate] [datetime] NULL ,
--	[Title] [nvarchar] (100) NULL ,
--	[Subtitle] [nvarchar] (200) NULL ,
--	[Abstract] [nvarchar] (512) NULL ,
--	[Description] [text] NULL ,
--	[StartDate] [datetime] NULL ,
--	[ExpireDate] [datetime] NULL ,
--	[IsInNewsletter] [bit] NULL ,
--	[MoreLink] [nvarchar] (150) NULL ,
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


--ALTER TABLE [Articles] WITH NOCHECK ADD 
--	CONSTRAINT [PK_Articles] PRIMARY KEY  CLUSTERED 
--	(
--		[ItemID]
--	)  ON [PRIMARY] 


--ALTER TABLE [Articles] ADD 
--	CONSTRAINT [FK_Articles_Modules] FOREIGN KEY 
--	(
--		[ModuleID]
--	) REFERENCES [Modules] (
--		[ModuleID]
--	) ON DELETE CASCADE 
--END

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [AddArticle]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [DeleteArticle]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetArticles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetArticles]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleArticle]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticleWithImages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSingleArticleWithImages]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [UpdateArticle]
--GO

--CREATE PROCEDURE AddArticle
--(
--    @ModuleID       int,
--    @UserName       nvarchar(100),
--    @Title          nvarchar(100),
--    @Subtitle       nvarchar(200),
--    @Abstract		nvarchar(512),
--    @Description    text,
--    @StartDate      datetime,
--    @ExpireDate     datetime,
--    @IsInNewsletter bit,
--    @MoreLink       nvarchar(150),
--    @ItemID         int OUTPUT
--)
--AS

--INSERT INTO Articles
--(
--    ModuleID,
--    CreatedByUser,
--    CreatedDate,
--    Title,
--	Subtitle,
--    Abstract,
--	Description,
--	StartDate,
--	ExpireDate,
--	IsInNewsletter,
--	MoreLink
--)
--VALUES
--(
--    @ModuleID,
--    @UserName,
--    GetDate(),
--    @Title,
--    @Subtitle,
--    @Abstract,
--    @Description,
--    @StartDate,
--    @ExpireDate,
--    @IsInNewsletter,
--    @MoreLink
--)

--SELECT
--    @ItemID = @@IDENTITY
--GO

--CREATE PROCEDURE DeleteArticle
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    Articles

--WHERE
--    ItemID = @ItemID
--GO

--CREATE PROCEDURE GetArticles
--(
--    @ModuleID int
--)
--AS

--SELECT		ItemID, 
--			ModuleID, 
--			CreatedByUser, 
--			CreatedDate, 
--			Title, 
--			Subtitle, 
--			Abstract, 
--			Description, 
--			StartDate, 
--			ExpireDate, 
--			IsInNewsletter, 
--			MoreLink
            
--FROM        Articles

--WHERE
--    (ModuleID = @ModuleID) AND (GetDate() <= ExpireDate)

--ORDER BY
--    StartDate DESC
--GO

--CREATE PROCEDURE GetSingleArticle
--(
--    @ItemID int
--)
--AS

--SELECT		ItemID,
--			ModuleID,
--			CreatedByUser,
--			CreatedDate,
--			Title, 
--			Subtitle, 
--			Abstract, 
--			Description, 
--			StartDate, 
--			ExpireDate, 
--			IsInNewsletter, 
--			MoreLink
--FROM	Articles
--WHERE   (ItemID = @ItemID)
--GO

--CREATE PROCEDURE GetSingleArticleWithImages
--(
--    @ItemID int,
--    @Variation varchar(50)
--)
--AS

--SELECT		Articles.ItemID, 
--			Articles.ModuleID, 
--			Articles.CreatedByUser, 
--			Articles.CreatedDate, 
--			Articles.Title, 
--			Articles.Subtitle, 
--			Articles.Abstract, 
--			Articles.Description, 
--            Articles.StartDate, 
--            Articles.ExpireDate, 
--            Articles.IsInNewsletter, 
--            Articles.MoreLink
            
--FROM        Articles
--WHERE     (ItemID = @ItemID)
--GO

--CREATE PROCEDURE UpdateArticle
--(
--    @ItemID         int,
--    @ModuleID       int,
--    @UserName       nvarchar(100),
--    @Title          nvarchar(100),
--    @Subtitle       nvarchar(200),
--    @Abstract       nvarchar(512),
--    @Description    text,
--    @StartDate      datetime,
--    @ExpireDate     datetime,
--    @IsInNewsletter bit,
--    @MoreLink       nvarchar(150)
--)
--AS

--UPDATE Articles

--SET 
--ModuleID = @ModuleID,
--CreatedByUser = @UserName,
--CreatedDate = GetDate(),
--Title =@Title ,
--Subtitle =  @Subtitle,
--Abstract =@Abstract,
--Description =@Description,
--StartDate = @StartDate,
--ExpireDate =@ExpireDate,
--IsInNewsletter = @IsInNewsletter,
--MoreLink =@MoreLink
--WHERE 
--ItemID = @ItemID
--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO
----end 
----end Articles patch 

--/*
--Search module patch, Jakob Hansen, hansen3000@hotmail.com

--This patch introduces the following changes to the db:
--- Inserts entry in table GeneralModuleDefinitions
--- Inserts entry in table ModuleDefinitions
--*/

--DECLARE @FriendlyName AS nvarchar(128)
--DECLARE @DesktopSrc AS nvarchar(128)
--DECLARE @GeneralModDefID as uniqueidentifier

--SET @FriendlyName = 'Portal Search'                     -- You enter the module UI name here
--SET @DesktopSrc = 'DesktopModules/PortalSearch.ascx'    -- You enter actual filename here
--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531030}'

--IF NOT EXISTS (SELECT GeneralModDefID FROM GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN

---- Insert data into GeneralModuleDefinitions
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES(@GeneralModDefID,NULL,@FriendlyName,@DesktopSrc,'',0)
--IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF

---- Insert data into ModuleDefinitions
--INSERT INTO ModuleDefinitions (PortalID, GeneralModDefID) VALUES ('0',@GeneralModDefID)

--END
--GO

---- Update module table
--IF NOT EXISTS 
--(SELECT * FROM sysobjects O INNER JOIN SysColumns C ON O.ID=C.ID
--WHERE ObjectProperty(O.ID,'IsUserTable')=1 
--AND O.Name='GeneralModuleDefinitions' AND C.Name='Searchable')
--BEGIN
--	ALTER TABLE [GeneralModuleDefinitions]
--		ADD Searchable bit NULL DEFAULT (0)
--END
--GO

--IF NOT EXISTS 
--(SELECT * FROM sysobjects O INNER JOIN SysColumns C ON O.ID=C.ID
--WHERE ObjectProperty(O.ID,'IsUserTable')=1 
--AND O.Name='GeneralModuleDefinitions' AND C.Name='AssemblyName')
--	ALTER TABLE [GeneralModuleDefinitions]
--		ADD AssemblyName varchar(50) NOT NULL DEFAULT 'Appleseed'

--GO

----Get searchable modules procedure
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSearchableModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [GetSearchableModules]
--GO

--CREATE PROCEDURE GetSearchableModules
--(
--	@PortalID int
--)
--AS
--SELECT     GeneralModuleDefinitions.GeneralModDefID, GeneralModuleDefinitions.ClassName, GeneralModuleDefinitions.FriendlyName, 
--                      GeneralModuleDefinitions.DesktopSrc, GeneralModuleDefinitions.MobileSrc, GeneralModuleDefinitions.Admin, GeneralModuleDefinitions.Searchable, 
--                      GeneralModuleDefinitions.AssemblyName, ModuleDefinitions.ModuleDefID
--FROM         GeneralModuleDefinitions INNER JOIN
--                      ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID
--WHERE     (GeneralModuleDefinitions.Searchable = 1) AND (ModuleDefinitions.PortalID = @PortalID)
--GO

--SET nocount ON
---- Update serchable modules
--UPDATE GeneralModuleDefinitions
--SET Searchable = 1, ClassName = 'Appleseed.Content.Web.ModulesDiscussion'
--WHERE GeneralModDefid = '{2D86166C-4BDC-4A6F-A028-D17C2BB177C8}'
--GO

--UPDATE GeneralModuleDefinitions
--SET Searchable = 1, ClassName = 'Appleseed.Content.Web.ModulesAnnouncements'
--WHERE GeneralModDefid = '{CE55A821-2449-4903-BA1A-EC16DB93F8DB}'
--GO

----UPDATE GeneralModuleDefinitions
----SET Searchable = 1, ClassName = 'Appleseed.Content.Web.ModulesArticles'
----WHERE GeneralModDefid = '{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}'
----GO

----UPDATE GeneralModuleDefinitions
----SET Searchable = 1, ClassName = 'Appleseed.Content.Web.ModulesContacts'
----WHERE GeneralModDefid = '{2502DB18-B580-4F90-8CB4-C15E6E5339EF}'
----GO

----UPDATE GeneralModuleDefinitions
----SET Searchable = 1, ClassName = 'Appleseed.Content.Web.ModulesDocuments'
----WHERE GeneralModDefid = '{F9645B82-CB45-4C4C-BB2D-72FA42FE2B75}'
----GO

--SET nocount OFF

----end Search module patch, Manu AND Jakob Hansen, hansen3000@hotmail.com

----New localization
--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rblang_Language]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--DROP TABLE [rblang_Language]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[LocalizeManager]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [LocalizeManager]
--GO

--CREATE PROCEDURE LocalizeManager
--(
--	@Key         nvarchar(50),
--	@Translation nvarchar(255) = null
--)
--AS

--IF NOT EXISTS 
--(
--SELECT    [Key]
--FROM      rblang_Language
--WHERE     ([Key] = @Key)
--)
--INSERT rblang_Language ([Key], ItemID, en) Values (@Key, 0, @Translation)

--ELSE

--UPDATE rblang_Language
--SET [en] = @Translation
--WHERE ([Key] = @Key) AND ([en] = '' or [en] IS NULL)
--GO
----end localization

