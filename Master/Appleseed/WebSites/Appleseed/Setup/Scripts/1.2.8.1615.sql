---------------------
--1.2.8.1615.sql
---------------------

-- some modifs necessary to support unicode
-- see http://www.Appleseedportal.net/AspNetForums/ShowPost.aspx?PostID=2042
-- marcb_empco@hotmail.com  18/04/2003

-- rb_Announcements table
Alter table rb_Announcements ALTER COLUMN CreatedByUser NVARCHAR(100)
Alter table rb_Announcements ALTER COLUMN Title NVARCHAR(150)
Alter table rb_Announcements ALTER COLUMN MoreLink NVARCHAR(150)
Alter table rb_Announcements ALTER COLUMN MobileMoreLink NVARCHAR(150)
Alter table rb_Announcements ALTER COLUMN Description NVARCHAR(2000)
GO

Alter table rb_Announcements_st ALTER COLUMN CreatedByUser NVARCHAR(100)
Alter table rb_Announcements_st ALTER COLUMN Title NVARCHAR(150)
Alter table rb_Announcements_st ALTER COLUMN MoreLink NVARCHAR(150)
Alter table rb_Announcements_st ALTER COLUMN MobileMoreLink NVARCHAR(150)
Alter table rb_Announcements_st ALTER COLUMN Description NVARCHAR(2000)
GO

-- rb_Articles table
alter table rb_Articles add Description1 ntext
GO

update rb_Articles set rb_Articles.Description1=rb_Articles.Description
GO

alter table rb_Articles drop column Description
GO

EXECUTE sp_rename 'rb_Articles.Description1', 'Description', 'COLUMN'
GO


-- Proc rb_AddArticles
-- change Description data type to ntext
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'rb_AddArticle') AND (xtype = 'P')) 
DROP PROCEDURE [rb_AddArticle]
GO

CREATE PROCEDURE rb_AddArticle
(

    @ModuleID       int,

    @UserName       nvarchar(100),

    @Title          nvarchar(100),

    @Subtitle       nvarchar(200),

    @Abstract	    nvarchar(512),

    @Description    ntext,

    @StartDate      datetime,

    @ExpireDate     datetime,

    @IsInNewsletter bit,

    @MoreLink       nvarchar(150),

    @ItemID         int OUTPUT

)

AS



INSERT INTO rb_Articles

(

    ModuleID,

    CreatedByUser,

    CreatedDate,

    Title,

	Subtitle,

    Abstract,

	Description,

	StartDate,

	ExpireDate,

	IsInNewsletter,

	MoreLink

)

VALUES

(

    @ModuleID,

    @UserName,

    GetDate(),

    @Title,

    @Subtitle,

    @Abstract,

    @Description,

    @StartDate,

    @ExpireDate,

    @IsInNewsletter,

    @MoreLink

)



SELECT

    @ItemID = @@IDENTITY
-- End proc rb_Addarticles
GO

-- Proc rb_UpdateArticle
-- change Description data type to ntext
IF EXISTS (SELECT * FROM sysobjects WHERE (name = 'rb_UpdateArticle') AND (xtype = 'P')) 
DROP PROCEDURE [rb_UpdateArticle]

GO
CREATE PROCEDURE rb_UpdateArticle

(

    @ItemID         int,

    @ModuleID       int,

    @UserName       nvarchar(100),

    @Title          nvarchar(100),

    @Subtitle       nvarchar(200),

    @Abstract       nvarchar(512),

    @Description    ntext,

    @StartDate      datetime,

    @ExpireDate     datetime,

    @IsInNewsletter bit,

    @MoreLink       nvarchar(150)

)

AS



UPDATE rb_Articles



SET 

ModuleID = @ModuleID,

CreatedByUser = @UserName,

CreatedDate = GetDate(),

Title =@Title ,

Subtitle =  @Subtitle,

Abstract =@Abstract,

Description =@Description,

StartDate = @StartDate,

ExpireDate =@ExpireDate,

IsInNewsletter = @IsInNewsletter,

MoreLink =@MoreLink

WHERE 

ItemID = @ItemID
GO


-- End



IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddArticle]
GO


CREATE PROCEDURE rb_AddArticle
(

    @ModuleID       int,

    @UserName       nvarchar(100),

    @Title          nvarchar(100),

    @Subtitle       nvarchar(200),

    @Abstract	    nvarchar(512),

    @Description    ntext,

    @StartDate      datetime,

    @ExpireDate     datetime,

    @IsInNewsletter bit,

    @MoreLink       nvarchar(150),

    @ItemID         int OUTPUT

)

AS



INSERT INTO rb_Articles

(

    ModuleID,

    CreatedByUser,

    CreatedDate,

    Title,

	Subtitle,

    Abstract,

	Description,

	StartDate,

	ExpireDate,

	IsInNewsletter,

	MoreLink

)

VALUES

(

    @ModuleID,

    @UserName,

    GetDate(),

    @Title,

    @Subtitle,

    @Abstract,

    @Description,

    @StartDate,

    @ExpireDate,

    @IsInNewsletter,

    @MoreLink

)



SELECT

    @ItemID = @@IDENTITY
-- End proc rb_Addarticles

GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteArticle]
GO

CREATE PROCEDURE rb_DeleteArticle
(
    @ItemID int
)
AS

DELETE FROM
    rb_Articles

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetArticles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetArticles]
GO

CREATE PROCEDURE rb_GetArticles
(
    @ModuleID int
)
AS

SELECT		ItemID, 
			ModuleID, 
			CreatedByUser, 
			CreatedDate, 
			Title, 
			Subtitle, 
			Abstract, 
			Description, 
			StartDate, 
			ExpireDate, 
			IsInNewsletter, 
			MoreLink

FROM        rb_Articles

WHERE
    (ModuleID = @ModuleID) AND (GetDate() <= ExpireDate) AND (GetDate() >= StartDate)

ORDER BY
    StartDate DESC
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleArticle]
GO

CREATE PROCEDURE rb_GetSingleArticle
(
    @ItemID int
)
AS

SELECT		ItemID,
			ModuleID,
			CreatedByUser,
			CreatedDate,
			Title, 
			Subtitle, 
			Abstract, 
			Description, 
			StartDate, 
			ExpireDate, 
			IsInNewsletter, 
			MoreLink
FROM	rb_Articles
WHERE   (ItemID = @ItemID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleArticleWithImages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleArticleWithImages]
GO

CREATE PROCEDURE rb_GetSingleArticleWithImages
(
    @ItemID int,
    @Variation varchar(50)
)
AS

SELECT		rb_Articles.ItemID, 
			rb_Articles.ModuleID, 
			rb_Articles.CreatedByUser, 
			rb_Articles.CreatedDate, 
			rb_Articles.Title, 
			rb_Articles.Subtitle, 
			rb_Articles.Abstract, 
			rb_Articles.Description, 
            rb_Articles.StartDate, 
            rb_Articles.ExpireDate, 
            rb_Articles.IsInNewsletter, 
            rb_Articles.MoreLink
            
FROM        rb_Articles
WHERE     (ItemID = @ItemID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateArticle]
GO

CREATE PROCEDURE rb_UpdateArticle

(

    @ItemID         int,

    @ModuleID       int,

    @UserName       nvarchar(100),

    @Title          nvarchar(100),

    @Subtitle       nvarchar(200),

    @Abstract       nvarchar(512),

    @Description    ntext,

    @StartDate      datetime,

    @ExpireDate     datetime,

    @IsInNewsletter bit,

    @MoreLink       nvarchar(150)

)

AS



UPDATE rb_Articles



SET 

ModuleID = @ModuleID,

CreatedByUser = @UserName,

CreatedDate = GetDate(),

Title =@Title ,

Subtitle =  @Subtitle,

Abstract =@Abstract,

Description =@Description,

StartDate = @StartDate,

ExpireDate =@ExpireDate,

IsInNewsletter = @IsInNewsletter,

MoreLink =@MoreLink

WHERE 

ItemID = @ItemID

GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1615','1.2.8.1615', CONVERT(datetime, '04/18/2003', 101))
GO

