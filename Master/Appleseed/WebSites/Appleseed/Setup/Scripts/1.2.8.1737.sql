---------------------
--1.2.8.1737.sql  
---------------------

ALTER TABLE rb_Links_st
	ALTER COLUMN Url nvarchar(800) NULL
GO

ALTER TABLE rb_Links 
	ALTER COLUMN Url nvarchar(800) NULL
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateLink]
GO

CREATE   PROCEDURE rb_UpdateLink
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(800),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target		 nvarchar(10)
)
AS

UPDATE
    rb_Links_st

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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddLink]
GO

CREATE   PROCEDURE rb_AddLink
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(800),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target	 nvarchar(10),
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_Links_st
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ChangeObjectOwner]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ChangeObjectOwner]
GO

CREATE   PROCEDURE rb_ChangeObjectOwner

/***********************************************************************
FileName:
	ChangeObjectOwner.sql

Description:
	Contains SQL used to change Appleseed database objects owner from 
	'<MACHINENAME>\ASPNET' to 'dbo'.
	
	The script takes all user defined objects that are owned by user different than 'dbo'
	AND changes its ownership to 'dbo'. 
	
Assumptions:				
	The script assumes that Appleseed portal is installed with Appleseed as a database name.
	If the name of the database is different, change the first line of the script. 
					
	The script assumes that the new user name is 'dbo'. If different user is required,
	change the 'dbo' name in the script (it must be a valid user in the database).

History:
	06/30/2003 -- Marek Kepinski (mkepinski@impaq.com.pl)		
		Initial implementation.
	
Notes:
	Ignore warning displayed be SQL Server concerning possible breaking of stored procedures
	due to changes of object owner.

*************************************************************************/

(
	-- change the new owner name if you need 
    @NewOwner nvarchar(128) = 'dbo'
)
AS

SET NOCOUNT ON
DECLARE @UserName sysname
DECLARE @ObjectName sysname
DECLARE @cmd nvarchar(255)

-- new owner must be a valid login
IF NOT EXISTS (SELECT * FROM sysusers WHERE islogin = 1 AND name = @NewOwner)
BEGIN
    DECLARE @Message varchar(200)
    SET @Message = 'User ' + @NewOwner + ' is not a valid user.'
    print @Message
END
ELSE
BEGIN
DECLARE C CURSOR FOR
    SELECT UserName = u.name, ObjectName = s.name
	FROM  sysobjects s INNER JOIN sysusers u ON s.uid = u.uid
		AND u.name <> @NewOwner
		AND s.xtype in ('V', 'P', 'U') 
		AND u.name not like 'INFORMATION%' 
	ORDER BY xtype, s.name

OPEN C

FETCH NEXT FROM C
	INTO @UserName, @ObjectName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @cmd = 'sp_changeobjectowner @objname=''' + @UserName + '.' + @ObjectName + ''', @newowner=' + @NewOwner

    exec (@cmd)

    FETCH NEXT FROM C
	INTO @UserName, @ObjectName
END

CLOSE C
DEALLOCATE C
print 'Done.'

END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetEvents]
GO

/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/
/* devsolution 2003/6/17: Added items for calendar control */
/* fixed bug: http://sourceforge.net/tracker/index.php?func=detail&aid=798993&group_id=66837&atid=515929 */

CREATE PROCEDURE rb_GetEvents
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
	    AllDay,
	    StartDate,
	    StartTime,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    rb_Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
Order By StartDate, StartTime
ELSE
	SELECT
	    ItemID,
	    Title,
	    CreatedByUser,
	    WhereWhen,
	    AllDay,
	    StartDate,
	    StartTime,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description
	FROM
	    rb_Events_st
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
	Order By StartDate, StartTime
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1737','1.2.8.1737', CONVERT(datetime, '09/18/2003', 101))
GO

