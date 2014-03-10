/* Install script, Newsletter by Manu 06/10/2003 */

/*
Blacklist module, Manu
This script introduces the following changes to the db:
- Creates table Blacklist
- Creates sproc AddToBlackList, DeleteFromBlackList
*/

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_BlackList]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_BlackList] (
	[PortalID] [int] NOT NULL ,
	[Email] [nvarchar] (100) NOT NULL ,
	[Date] [smalldatetime] NULL ,
	[Reason] [nvarchar] (150) NULL ,
	CONSTRAINT [PK_rb_Blacklist] PRIMARY KEY  CLUSTERED 
	(
		[PortalID],
		[Email]
	)
)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddToBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddToBlackList]
GO

CREATE PROCEDURE rb_AddToBlackList
(
@PortalID int,
@Email nvarchar(100),
@Reason nvarchar(150)
)
AS 
IF NOT EXISTS (SELECT Email FROM rb_BlackList WHERE PortalID=@PortalID AND Email=@Email)
BEGIN
	INSERT INTO rb_BlackList
	(
		PortalID,
		Email,
		Date,
		Reason
	)
	VALUES
	(
		@PortalID,
		@Email,
		GETDATE(),
		@Reason
	)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteFromBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteFromBlackList]
GO

CREATE PROCEDURE rb_DeleteFromBlackList
(
@PortalID int,
@Email nvarchar(100)
)
AS 
DELETE FROM rb_BlackList WHERE PortalID=@PortalID AND Email=@Email
GO
