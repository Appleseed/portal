---------------------
--1.2.8.1516.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1516','1.2.8.1516', CONVERT(datetime, '03/16/2003', 101))
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCurrentDbVersion]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentDbVersion]
GO

CREATE PROCEDURE [rb_GetCurrentDbVersion]
AS
SELECT     TOP 1 Release
FROM         rb_Versions
ORDER BY Release DESC
GO

/*

John Bowen - March 13, 2003

SCRIPT TO CHANGE ALL 'STAGING' OWNED OBJECTS OVER TO 'DBO' OWNERSHIP.  
  ** CREATES NEW TABLES ([st_tableName])
  ** COPIES DATA FROM OLD STAGING TABLES TO NEW
  ** MODIFIES (28) AFFECTED STORED PROCEDURES TO USE NEW TABLES
  ** DROPS OLD STAGING TABLES
  ** DROPS STAGING USER FROM DB
*/

/* CREATE ALL THE [DBO] TABLES EXACTLY LIKE THE CURRENT [STAGING] TABLES */

CREATE TABLE [st_Announcements] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [varchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [varchar] (150) NULL ,
	[MoreLink] [varchar] (150) NULL ,
	[MobileMoreLink] [varchar] (150) NULL ,
	[ExpireDate] [datetime] NULL ,
	[Description] [varchar] (2000) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [st_Contacts] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Name] [nvarchar] (50) NULL ,
	[Role] [nvarchar] (100) NULL ,
	[Email] [nvarchar] (100) NULL ,
	[Contact1] [nvarchar] (250) NULL ,
	[Contact2] [nvarchar] (250) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [st_Documents] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[FileNameUrl] [nvarchar] (250) NULL ,
	[FileFriendlyName] [nvarchar] (150) NULL ,
	[Category] [nvarchar] (50) NULL ,
	[Content] [image] NULL ,
	[ContentType] [nvarchar] (50) NULL ,
	[ContentSize] [int] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [st_Events] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (150) NULL ,
	[WhereWhen] [nvarchar] (150) NULL ,
	[Description] [nvarchar] (2000) NULL ,
	[ExpireDate] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [st_HtmlText] (
	[ModuleID] [int] NOT NULL ,
	[DesktopHtml] [ntext] NOT NULL ,
	[MobileSummary] [ntext] NOT NULL ,
	[MobileDetails] [ntext] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [st_Links] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[Url] [nvarchar] (250) NULL ,
	[MobileUrl] [nvarchar] (250) NULL ,
	[ViewOrder] [int] NULL ,
	[Description] [nvarchar] (2000) NULL,
	[Target] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO

------------------------------------------------------------
/*COPY ALL THE DATA FROM THE STAGING TABLES TO THE NEW DBO TABLES*/
------------------------------------------------------------
-- lines removed by marcb@hotmail.com to avoid errors because staging.xxx tables do not exist
-- lines restored by manudea to correctly migrate data if present

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
INSERT INTO [st_HtmlText] (ModuleID, DesktopHtml, MobileSummary, MobileDetails) SELECT ModuleID, DesktopHtml, MobileSummary, MobileDetails FROM [staging].[HtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	SET IDENTITY_INSERT [st_Announcements] ON
	INSERT INTO [st_Announcements] (ItemID, ModuleID, CreatedByUser, CreatedDate, Title, MoreLink, MobileMoreLink, ExpireDate, Description) SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, Title, MoreLink, MobileMoreLink, ExpireDate, Description FROM [staging].[Announcements]
	SET IDENTITY_INSERT [st_Announcements] OFF
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	SET IDENTITY_INSERT [st_Contacts] ON
	INSERT INTO [st_Contacts] (ItemID, ModuleID, CreatedByUser, CreatedDate, Name, Role, Email, Contact1, Contact2) SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, Name, Role, Email, Contact1, Contact2 FROM [staging].[Contacts]
	SET IDENTITY_INSERT [st_Contacts] OFF
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	SET IDENTITY_INSERT [st_Documents] ON
	INSERT INTO [st_Documents] (ItemID, ModuleID, CreatedByUser, CreatedDate, FileNameUrl, FileFriendlyName, Category, Content, ContentType, ContentSize) SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, FileNameUrl, FileFriendlyName, Category, Content, ContentType, ContentSize FROM [staging].[Documents]
	SET IDENTITY_INSERT [st_Documents] OFF
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	SET IDENTITY_INSERT [st_Events] ON
	INSERT INTO [st_Events] (ItemID, ModuleID, CreatedByUser, CreatedDate, Title, WhereWhen, Description, ExpireDate) SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, Title, WhereWhen, Description, ExpireDate FROM [staging].[Events]
	SET IDENTITY_INSERT [st_Events] OFF
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	SET IDENTITY_INSERT [st_Links] ON
	INSERT INTO [st_Links] (ItemID, ModuleID, CreatedByUser, CreatedDate, Title, Url, MobileUrl, ViewOrder, Description, Target) SELECT ItemID, ModuleID, CreatedByUser, CreatedDate, Title, Url, MobileUrl, ViewOrder, Description, Target FROM [staging].[Links]
	SET IDENTITY_INSERT [st_Links] OFF
END
GO

------------------------------------------------------------
/*UPDATE ALL AFFECTED SPROCS IN THE DB*/
------------------------------------------------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAnnouncements]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAnnouncements]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetContacts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetContacts]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocumentContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocumentContent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocuments]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocuments]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetEvents]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetHtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLinks]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetLinks]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateHtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateLink]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/****** Oggetto: stored procedure AddAnnouncement    Data dello script: 07/11/2002 22.28.08 ******/


-- =============================================================
-- ALTER  the stored procs
-- =============================================================
CREATE  PROCEDURE AddAnnouncement
(
    @ModuleID       int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     DateTime,
    @Description    nvarchar(2000),
    @ItemID         int OUTPUT
)
AS

INSERT INTO st_Announcements
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    MoreLink,
    MobileMoreLink,
    ExpireDate,
    Description
)

VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @MoreLink,
    @MobileMoreLink,
    @ExpireDate,
    @Description
)

SELECT
    @ItemID = @@IDENTITY







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/****** Oggetto: stored procedure AddContact    Data dello script: 07/11/2002 22.28.12 ******/

CREATE  PROCEDURE AddContact
(
    @ModuleID int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250),
    @ItemID   int OUTPUT
)
AS

INSERT INTO st_Contacts
(
    CreatedByUser,
    CreatedDate,
    ModuleID,
    Name,
    Role,
    Email,
    Contact1,
    Contact2
)

VALUES
(
    @UserName,
    GetDate(),
    @ModuleID,
    @Name,
    @Role,
    @Email,
    @Contact1,
    @Contact2
)

SELECT
    @ItemID = @@IDENTITY







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.28.12 ******/


CREATE  PROCEDURE AddEvent
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  DateTime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100),
    @ItemID      int OUTPUT
)
AS

INSERT INTO st_Events
(
    ModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    ExpireDate,
    Description,
    WhereWhen
)

VALUES
(
    @ModuleID,
    @UserName,
    @Title,
    GetDate(),
    @ExpireDate,
    @Description,
    @WhereWhen
)

SELECT
    @ItemID = @@IDENTITY

GO

CREATE  PROCEDURE AddLink
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target		 nvarchar(10),
    @ItemID      int OUTPUT
)
AS

INSERT INTO st_Links
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    MobileUrl,
    ViewOrder,
    Description,
    Target
)
VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @Url,
    @MobileUrl,
    @ViewOrder,
    @Description,
    @Target
)

SELECT
    @ItemID = @@IDENTITY

GO

CREATE  PROCEDURE DeleteAnnouncement
(
    @ItemID int
)
AS

DELETE FROM
    st_Announcements

WHERE
    ItemID = @ItemID
GO

CREATE  PROCEDURE DeleteContact
(
    @ItemID int
)
AS

DELETE FROM
    st_Contacts

WHERE
    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure DeleteDocument    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE DeleteDocument
(
    @ItemID int
)
AS

DELETE FROM
    st_Documents

WHERE
    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure DeleteEvent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE DeleteEvent
(
    @ItemID int
)
AS

DELETE FROM
    st_Events

WHERE
    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure DeleteLink    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE DeleteLink
(
    @ItemID int
)
AS

DELETE FROM
    st_Links

WHERE
    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetAnnouncements    Data dello script: 07/11/2002 22.28.08 ******/


CREATE  PROCEDURE GetAnnouncements
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM 
	    Announcements
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM 
	    st_Announcements
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetContacts    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetContacts
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedDate,
	    CreatedByUser,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    Contacts
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT
	    ItemID,
	    CreatedDate,
	    CreatedByUser,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    st_Contacts
	WHERE
	    ModuleID = @ModuleID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetDocumentContent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetDocumentContent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    Content,
	    ContentType,
	    ContentSize,
	    FileFriendlyName
	FROM
	    Documents
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    Content,
	    ContentType,
	    ContentSize,
	    FileFriendlyName
	FROM
	    st_Documents
	WHERE
	    ItemID = @ItemID
	





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetDocuments    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetDocuments
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    Documents
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    st_Documents
	WHERE
	    ModuleID = @ModuleID




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetEvents
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    Title,
	    CreatedByUser,
	    WhereWhen,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
ELSE
	SELECT
	    ItemID,
	    Title,
	    CreatedByUser,
	    WhereWhen,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    st_Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetHtmlText    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetHtmlText
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT *
	FROM
	    HtmlText
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT *
	FROM
	    st_HtmlText
	WHERE
	    ModuleID = @ModuleID







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetLinks    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE GetLinks
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    st_Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetSingleAnnouncement    Data dello script: 07/11/2002 22.28.09 ******/



CREATE  PROCEDURE GetSingleAnnouncement
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM
	    Announcements
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM
	    st_Announcements
	WHERE
	    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetSingleContact    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE GetSingleContact
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    Contacts
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    st_Contacts
	WHERE
	    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetSingleDocument    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE GetSingleDocument
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    Documents
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    st_Documents
	WHERE
	    ItemID = @ItemID







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetSingleEvent    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE GetSingleEvent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    Events
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    st_Events
	WHERE
	    ItemID = @ItemID







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure GetSingleLink    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE GetSingleLink
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    Links
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    st_Links
	WHERE
	    ItemID = @ItemID




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure UpdateAnnouncement    Data dello script: 07/11/2002 22.28.10 ******/



CREATE  PROCEDURE UpdateAnnouncement
(
    @ItemID         int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     datetime,
    @Description    nvarchar(2000)
)
AS

UPDATE
    st_Announcements

SET
    CreatedByUser   = @UserName,
    CreatedDate     = GetDate(),
    Title           = @Title,
    MoreLink        = @MoreLink,
    MobileMoreLink  = @MobileMoreLink,
    ExpireDate      = @ExpireDate,
    Description     = @Description

WHERE
    ItemID = @ItemID
GO

CREATE  PROCEDURE UpdateContact
(
    @ItemID   int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250)
)
AS

UPDATE
    st_Contacts

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Name          = @Name,
    Role          = @Role,
    Email         = @Email,
    Contact1      = @Contact1,
    Contact2      = @Contact2

WHERE
    ItemID = @ItemID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





/****** Oggetto: stored procedure UpdateDocument    Data dello script: 07/11/2002 22.28.14 ******/


CREATE   PROCEDURE UpdateDocument
(
    @ItemID           int,
    @ModuleID         int,
    @FileFriendlyName nvarchar(150),
    @FileNameUrl      nvarchar(250),
    @UserName         nvarchar(100),
    @Category         nvarchar(50),
    @Content          image,
    @ContentType      nvarchar(50),
    @ContentSize      int
)
AS
IF (@ItemID=0) OR NOT EXISTS (
    SELECT 
        * 
    FROM 
        st_Documents 
    WHERE 
        ItemID = @ItemID
)
INSERT INTO st_Documents
(
    ModuleID,
    FileFriendlyName,
    FileNameUrl,
    CreatedByUser,
    CreatedDate,
    Category,
    Content,
    ContentType,
    ContentSize
)

VALUES
(
    @ModuleID,
    @FileFriendlyName,
    @FileNameUrl,
    @UserName,
    GetDate(),
    @Category,
    @Content,
    @ContentType,
    @ContentSize
)
ELSE

BEGIN

IF (@ContentSize=0)

UPDATE 
    st_Documents

SET 
    CreatedByUser    = @UserName,
    CreatedDate      = GetDate(),
    Category         = @Category,
    FileFriendlyName = @FileFriendlyName,
    FileNameUrl      = @FileNameUrl

WHERE
    ItemID = @ItemID
ELSE

UPDATE
    st_Documents

SET
    CreatedByUser     = @UserName,
    CreatedDate       = GetDate(),
    Category          = @Category,
    FileFriendlyName  = @FileFriendlyName,
    FileNameUrl       = @FileNameUrl,
    Content           = @Content,
    ContentType       = @ContentType,
    ContentSize       = @ContentSize

WHERE
    ItemID = @ItemID

END






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




/****** Oggetto: stored procedure UpdateEvent    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE UpdateEvent
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  datetime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100)
)

AS

UPDATE
    st_Events

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    ExpireDate    = @ExpireDate,
    Description   = @Description,
    WhereWhen     = @WhereWhen

WHERE
    ItemID = @ItemID
GO

CREATE  PROCEDURE UpdateHtmlText
(
    @ModuleID      int,
    @DesktopHtml   ntext,
    @MobileSummary ntext,
    @MobileDetails ntext
)
AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        st_HtmlText 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO st_HtmlText (
    ModuleID,
    DesktopHtml,
    MobileSummary,
    MobileDetails
) 
VALUES (
    @ModuleID,
    @DesktopHtml,
    @MobileSummary,
    @MobileDetails
)
ELSE
UPDATE
    st_HtmlText

SET
    DesktopHtml   = @DesktopHtml,
    MobileSummary = @MobileSummary,
    MobileDetails = @MobileDetails

WHERE
    ModuleID = @ModuleID
GO

CREATE  PROCEDURE UpdateLink
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target		 nvarchar(10)
)
AS

UPDATE
    st_Links

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    Url           = @Url,
    MobileUrl     = @MobileUrl,
    ViewOrder     = @ViewOrder,
    Description   = @Description,
    Target		  = @Target

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Publish]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [Publish]
GO

-- Alter Publish stored procedure

CREATE PROCEDURE Publish
	@ModuleID	int
AS

	-- First get al list of tables which are related to the Modules table

	-- Create a temporary table
	CREATE TABLE #TMPResults
		(ForeignKeyTableSchema	nvarchar(128),
                 ForeignKeyTable	nvarchar(128),
		 ForeignKeyColumn	nvarchar(128),
		 KeyColumn		nvarchar(128),
		 ForeignKeyTableId	int,
		 KeyTableId		int,
		 KeyTableSchema		nvarchar(128),
		 KeyTable		nvarchar(128))

	INSERT INTO #TMPResults EXEC GetRelatedTables 'Modules', 'dbo'

	DECLARE RelatedTablesModules CURSOR FOR
		SELECT 	
			ForeignKeyTableSchema, 
			ForeignKeyTable,
			ForeignKeyColumn,
			KeyColumn,
			ForeignKeyTableId,
			KeyTableId,
			KeyTableSchema,
			KeyTable
		FROM
			#TMPResults
		WHERE 
			ForeignKeyTableSchema = 'dbo'
			AND ForeignKeyTable <> 'ModuleSettings'

	-- Create temporary table for later use
	CREATE TABLE #TMPCount
		(ResultCount	int)


	-- Now search the table that has the related column
	OPEN RelatedTablesModules

	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
	DECLARE @ForeignKeyTable	nvarchar(128)
	DECLARE @ForeignKeyColumn	nvarchar(128)
	DECLARE @KeyColumn		nvarchar(128)
	DECLARE @ForeignKeyTableId	int
	DECLARE @KeyTableId		int
	DECLARE @KeyTableSchema		nvarchar(128)
	DECLARE @KeyTable		nvarchar(128)
	DECLARE @SQLStatement		nvarchar(4000)
	DECLARE @Count			int
	DECLARE @TableHasIdentityCol	int

	FETCH NEXT FROM RelatedTablesModules 
	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
		@KeyTableSchema, @KeyTable
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Check if this table has a corresponding staging table
		TRUNCATE TABLE #TMPCount
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[st_' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
		-- PRINT @SQLStatement
		EXEC(@SQLStatement)
		SELECT @Count = ResultCount
			FROM #TMPCount		
		PRINT @ForeignKeyTable
		PRINT @Count
		IF (@Count = 1)
		BEGIN						
			-- Check if this table contains the related data
			TRUNCATE TABLE #TMPCount
			SET @SQLStatement = 
				'INSERT INTO #TMPCount ' +
				'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
				'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
				'UNION ' +
				'SELECT Count(*) FROM [st_' + @ForeignKeyTable + '] ' +
				'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

			EXEC(@SQLStatement)
			SELECT @Count = ResultCount
				FROM #TMPCount		
			IF (@Count >= 1) 
			BEGIN
				-- This table contains the related data 
				-- Delete everything in the prod table
				SET @SQLStatement = 
					'DELETE FROM [' + @ForeignKeyTable + '] ' +
					'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
				PRINT @SQLStatement
				EXEC(@SQLStatement)
				-- Copy everything from the staging table to the prod table
				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
				IF (@TableHasIdentityCol = 1)
				BEGIN
					-- The table contains a identity column
					DECLARE TableColumns CURSOR FOR
						SELECT [COLUMN_NAME]
						FROM [INFORMATION_SCHEMA].[COLUMNS]
						WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
							AND [TABLE_NAME] = @ForeignKeyTable
						ORDER BY [ORDINAL_POSITION]
	
					OPEN TableColumns
	
					DECLARE @ColumnList	nvarchar(4000)
					SET @ColumnList = ''
					DECLARE @ColName	nvarchar(128)
					DECLARE @ColIsIdentity	int
	
					FETCH NEXT FROM TableColumns
					INTO @ColName
					
					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
						IF (@ColIsIdentity = 0)
							SET @ColumnList = @ColumnList + '[' + @ColName + '] '
	
						FETCH NEXT FROM TableColumns
						INTO @ColName
	
						IF (@@FETCH_STATUS = 0)
						BEGIN
							IF (@ColIsIdentity = 0)
							BEGIN
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
								IF (@ColIsIdentity = 0)
									SET @ColumnList = @ColumnList + ', '		
							END
							ELSE
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
						END
					END
					
					CLOSE TableColumns
					DEALLOCATE TableColumns		
	
					SET @SQLStatement = 	
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
						'SELECT ' + @ColumnList + ' FROM [st_' + @ForeignKeyTable + '] ' +
						'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					-- PRINT @SQLStatement
					EXEC(@SQLStatement)
				END
				ELSE
				BEGIN
					-- The table doens't contain a identitycolumn
					SET @SQLStatement = 
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
						'SELECT * FROM [st_' + @ForeignKeyTable + '] ' +
						'WHERE [st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					EXEC(@SQLStatement)
				END
			END
		END

		FETCH NEXT FROM RelatedTablesModules 
		INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
			@KeyTableSchema, @KeyTable
	END


	CLOSE RelatedTablesModules
	DEALLOCATE RelatedTablesModules
			
	-- Set the module in the correct status
	UPDATE [Modules]
	SET [WorkflowState] = 0 -- Original
	WHERE [ModuleID] = @ModuleID

	RETURN
GO

------------------------------------------------------------
/*  THIS SECTION GETS RID OF THE STAGING USER AND HIS TABLES */
------------------------------------------------------------

-- setuser N'staging'

/* DROP ALL THE TRIGGERS FROM THE EXISTING STAGING TABLES */
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[AnnouncementsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[AnnouncementsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[ContactsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[ContactsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[DocumentsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[DocumentsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[EventsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[EventsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[HtmlTextModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[HtmlTextModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[LinksModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [staging].[LinksModified]
GO


/* DROP ALL THE STAGING TABLES FROM THE DB */
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[Announcements]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[Contacts]

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[Documents]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[Events]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[HtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[staging].[Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [staging].[Links]
GO

------------------------------------------------------------
/*ADD ALL THE CONSTRAINTS AND TRIGGERS FOR THE NEW TABLES*/
------------------------------------------------------------
ALTER TABLE [st_Announcements] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_Announcements] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_Contacts] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_Contacts] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_Documents] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_Documents] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_Events] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_Events] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_HtmlText] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_HtmlText] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_Links] WITH NOCHECK ADD 
	CONSTRAINT [PK_st_Links] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [st_Announcements] ADD 
	CONSTRAINT [FK_st_Announcements_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [st_Contacts] ADD 
	CONSTRAINT [FK_st_Contacts_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [st_Documents] ADD 
	CONSTRAINT [FK_st_Documents_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [st_Events] ADD 
	CONSTRAINT [FK_st_Events_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [st_HtmlText] ADD 
	CONSTRAINT [FK_st_HtmlText_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [st_Links] ADD 
	CONSTRAINT [FK_st_Links_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

CREATE  TRIGGER [st_AnnouncementsModified]
ON [st_Announcements]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules

END
GO

CREATE  TRIGGER [st_ContactsModified]
ON [st_Contacts]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules

END
GO

CREATE  TRIGGER [st_DocumentsModified]
ON [st_Documents]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO

CREATE  TRIGGER [st_EventsModified]
ON [st_Events]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO

CREATE   TRIGGER [st_HtmlTextModified]
ON [st_HtmlText]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO

CREATE  TRIGGER [st_LinksModified]
ON [st_Links]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO

