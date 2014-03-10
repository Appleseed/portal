---------------------
--1.2.8.1626.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFull]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE rb_UpdateUser
(
	@PortalID		int,
    @UserID         int,
    @Name			nvarchar(50),
    @Email          nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter bit
)
AS

IF (len(@Password)>0)
begin
UPDATE
    rb_Users

SET
    Name	 = @Name,
    Email    = @Email,
    Password = @Password,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID
end
else
begin
UPDATE
    rb_Users

SET
    Name	 = @Name,
    Email    = @Email,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID
end
GO

CREATE PROCEDURE rb_UpdateUserFull
(
    @UserID		    int,
    @PortalID       int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
	@CountryID	nchar(2),  
	@StateID		int
)
AS

If (len(@Password) >0)
BEGIN

UPDATE rb_Users

SET

PortalID = @PortalID,
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
Email = @Email,		
Password = @Password,
SendNewsletter = @SendNewsletter,
CountryID = @CountryID,
StateID = @StateID

WHERE UserID = @UserID

END

ELSE

BEGIN

UPDATE rb_Users

SET

PortalID = @PortalID,
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
Email = @Email,		
SendNewsletter = @SendNewsletter,
CountryID = @CountryID,
StateID = @StateID

WHERE UserID = @UserID

END
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1626','1.2.8.1626', CONVERT(datetime, '04/24/2003', 101))
GO
