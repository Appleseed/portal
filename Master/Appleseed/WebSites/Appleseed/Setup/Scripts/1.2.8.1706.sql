/* Delete all of the original IBuySpy discussion thread stored procedures */
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetThreadMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTopLevelMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetNextMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetNextMessageID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPrevMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPrevMessageID]
GO

/* Delete any beta cpmDiscussion stored prcedures just in case user installed the download */
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_cpmDiscussionAddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_cpmDiscussionAddMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_cpmDiscussionGetMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_cpmDiscussionGetMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_cpmDiscussionGetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_cpmDiscussionGetThreadMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_cpmDiscussionGetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_cpmDiscussionGetTopLevelMessages]
GO

/* Delete any existing cpmDiscussion stored procedures */
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionDeleteMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionDeleteMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionAddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionAddMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionGetMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionGetMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionGetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionGetThreadMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionGetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionGetTopLevelMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionGetNextMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionGetNextMessageID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionGetPrevMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionGetPrevMessageID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionDeleteMessageAndChildren]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionDeleteMessageAndChildren]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DiscussionDeleteChildren]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DiscussionDeleteChildren]
GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionDeleteChildren
(
   @ItemID int,  /* delete this item AND all its children */
   @NumDeletedMessages int OUTPUT
)
AS

DECLARE @CurrentDisplayOrder as nvarchar(750)
DECLARE @CurrentModule as int

/* Find DisplayOrder of item to delete */
SELECT
    @CurrentDisplayOrder = DisplayOrder,
    @CurrentModule = ModuleID
FROM
    rb_Discussion
WHERE
    ItemID = @ItemID


/* Delete this item AND all of its children */
DELETE
FROM
    rb_Discussion
WHERE
    LEFT(DisplayOrder,LEN(RTRIM(@CurrentDisplayOrder))) = @CurrentDisplayOrder
    /* DisplayOrder LIKE @CurrentDisplayOrder + '%' */
    AND
    ModuleID = @CurrentModule

SET @NumDeletedMessages = @@Rowcount

GO


/***********************************************************/
CREATE PROCEDURE rb_DiscussionDeleteMessage
(
    @ItemID int
)
AS

DELETE FROM
    rb_Discussion
WHERE
    ItemID = @ItemID
GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionAddMessage
(
    @Mode      text,
    @ItemID    int OUTPUT,
    @Title     nvarchar(100),
    @Body      nvarchar(3000),
    @ParentID  int,
    @UserName  nvarchar(100),
    @ModuleID  int
)   

AS 

IF (@Mode LIKE 'EDIT')
BEGIN
UPDATE 
    rb_Discussion
SET
    Title = @Title,
    Body  = @Body,
    CreatedByUser = @UserName   /* SHOULD WE LET USER CHANGE ? */
WHERE
    ItemID = @ParentID

SET @ItemID = @ParentID
END
ELSE
BEGIN
/* Find DisplayOrder of parent item */
DECLARE @ParentDisplayOrder as nvarchar(750)
SET @ParentDisplayOrder = ''

SELECT 
    @ParentDisplayOrder = DisplayOrder
FROM 
    rb_Discussion 
WHERE 
    ItemID = @ParentID

INSERT INTO rb_Discussion
(
    Title,
    Body,
    DisplayOrder,
    CreatedDate, 
    CreatedByUser,
    ModuleID
)

VALUES
(
    @Title,
    @Body,
    @ParentDisplayOrder + CONVERT( nvarchar(24), GetDate(), 21 ),
    GetDate(),
    @UserName,
    @ModuleID
)
SELECT 
@ItemID = @@IDENTITY
END


/* update the top most parent's created date with the current date 
This is old logic WHERE I changed the CreatedDate of the parent to get TopLevelMessage
sorts always showing the recent activity at top.  Now TopLevelMEssages auto
sorts by threads with recent activity

DECLARE @TopParent as nvarchar(24)
SET @TopParent = "" 
SET @TopParent = LEFT(@ParentDisplayOrder, 23)

make sure we are not at a top level parent already !
IF @ParentID != 0 
BEGIN
UPDATE Discussion
SET CreatedDate = GetDate()
WHERE DisplayOrder = @TopParent
END
*/

GO

/***********************************************************/


CREATE PROCEDURE rb_DiscussionGetMessage
(
    @ItemID int
)
AS

/*
DECLARE @nextMessageID int
EXECUTE DiscussionGetNextMessageID @ItemID, @nextMessageID OUTPUT
DECLARE @prevMessageID int
EXECUTE DiscussionGetPrevMessageID @ItemID, @prevMessageID OUTPUT
*/

SELECT
    ItemID,
    Title,
    CreatedByUser,
    CreatedDate,
    Body,
    DisplayOrder
/*
    ,
    NextMessageID = @nextMessageID,
    PrevMessageID = @prevMessageID
*/

FROM
    rb_Discussion

WHERE
    ItemID = @ItemID


GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionGetThreadMessages
(
    @Parent nvarchar(750)
)
AS

SELECT
    ItemID,
    DisplayOrder,
    REPLICATE( '&nbsp;', ( ( LEN( DisplayOrder ) / 23 ) - 2 ) * 5 ) AS Indent,
    REPLICATE( '<blockquote>', ( ( LEN( DisplayOrder ) / 23 ) - 1 )) AS BlockQuoteStart,
    REPLICATE( '</blockquote>', ( ( LEN( DisplayOrder ) / 23 ) - 1)) AS BlockQuoteEnd,
    Title,  
    CreatedByUser,
    CreatedDate,
    Body

FROM 
    rb_Discussion

WHERE
    LEFT(DisplayOrder, 23) = @Parent
  AND
    (LEN( DisplayOrder ) / 23 ) > 1

ORDER BY
    DisplayOrder


GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionGetTopLevelMessages
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    DisplayOrder,
    LEFT(DisplayOrder, 23) AS Parent,    
    (SELECT COUNT(*) -1  FROM rb_Discussion Disc2 WHERE LEFT(Disc2.DisplayOrder,LEN(RTRIM(Disc.DisplayOrder))) = Disc.DisplayOrder) AS ChildCount,
    Title,  
    CreatedByUser,
    CreatedDate,
    Body,
    (SELECT MAX(CreatedDate) FROM rb_Discussion Disc3 WHERE LEFT(Disc3.DisplayOrder, 23) = Disc.DisplayOrder)
    AS DateofLastReply
FROM rb_Discussion Disc
WHERE ModuleID=@ModuleID
AND (LEN( DisplayOrder ) / 23 ) = 1

ORDER BY
    DateofLastReply DESC

GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionGetNextMessageID
(
    @ItemID int,
    @NextID int OUTPUT
)
AS

DECLARE @CurrentDisplayOrder as nvarchar(750)
DECLARE @CurrentModule as int

/* Find DisplayOrder of current item */
SELECT
    @CurrentDisplayOrder = DisplayOrder,
    @CurrentModule = ModuleID
FROM
    rb_Discussion
WHERE
    ItemID = @ItemID

/* Get the next message in the same module */
SELECT Top 1
    @NextID = ItemID

FROM
    rb_Discussion

WHERE
    DisplayOrder > @CurrentDisplayOrder
    AND
    ModuleID = @CurrentModule

ORDER BY
    DisplayOrder ASC

/* end of this thread? */
IF @@Rowcount < 1
    SET @NextID = null
GO

/***********************************************************/
CREATE PROCEDURE rb_DiscussionGetPrevMessageID
(
    @ItemID int,
    @PrevID int OUTPUT
)
AS

DECLARE @CurrentDisplayOrder as nvarchar(750)
DECLARE @CurrentModule as int

/* Find DisplayOrder of current item */
SELECT
    @CurrentDisplayOrder = DisplayOrder,
    @CurrentModule = ModuleID
FROM
    rb_Discussion
WHERE
    ItemID = @ItemID

/* Get the previous message in the same module */
SELECT Top 1
    @PrevID = ItemID

FROM
    rb_Discussion

WHERE
    DisplayOrder < @CurrentDisplayOrder
    AND
    ModuleID = @CurrentModule

ORDER BY
    DisplayOrder DESC

/* already at the beginning of this module? */
IF @@Rowcount < 1
    SET @PrevID = null
GO

/***********************************************************/
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1706','1.2.8.1706', CONVERT(datetime, '05/07/2003', 101))
GO