/* Install script, Newsletter by Manu 06/10/2003 */

---NEWSLETTER
IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetUsersNewsletter]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetUsersNewsletter]
GO

CREATE PROCEDURE rb_GetUsersNewsletter
(
@PortalID int,
@MaxUsers int = 250,
@MinSend float = 30, /* 1 = 1 day, users which send was made in x days will be ignored */    
@UserCount int = 0 OUTPUT
)
AS

/* 24 hours min delay */
IF @MinSend < 1 SELECT @MinSend = 1 

CREATE TABLE #TempItems
(
	ID int 		IDENTITY(1,1),
	UserID		int, 
	Name		nvarchar(250), 
	Password	varchar(40), 
	Email		nvarchar(100), 
	PortalID	int
)

INSERT INTO #TempItems(
	UserID, Name, Password, Email, PortalID
)
SELECT
	UserID, 
	Name, 
	Password, 
	Email, 
	PortalID
FROM
	rb_Users
WHERE
	(SendNewsletter = 1) AND 
	(CAST(COALESCE (LastSend, GETDATE() - @MinSend) AS float) <= CAST(GETDATE() - @MinSend AS float)) AND
        (PortalID = @PortalID) AND (NOT (Email IN (SELECT EMAIL FROM rb_BlackList WHERE PortalID = @PortalID)))
ORDER BY UserID

SELECT * FROM #TempItems WHERE ID <= @MaxUsers

SELECT @UserCount = @@ROWCOUNT
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_SendNewsletterTo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_SendNewsletterTo]
GO

CREATE PROCEDURE rb_SendNewsletterTo
(
@PortalID int,
@Email nvarchar(100)
)
AS 
UPDATE rb_Users SET LastSend = GETDATE() WHERE PortalID=@PortalID AND Email=@Email
SELECT 0
GO