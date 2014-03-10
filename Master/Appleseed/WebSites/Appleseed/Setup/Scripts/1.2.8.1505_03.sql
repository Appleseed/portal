---------------------
--1.2.8.1505_03.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1505','1.2.8.1505', CONVERT(datetime, '05/19/2003', 101))
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCulture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCulture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDefaultCulture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDefaultCulture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountries]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountriesFiltered]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountriesFiltered]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetStates]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetStates]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUserFull]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[LocalizeManager]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [LocalizeManager]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleCountry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleCountry]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUserFull]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersNoPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsersNoPassword]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserSetPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUserSetPassword]
GO

CREATE PROCEDURE GetCountries
(
	@IDLang	nchar(2) = 'en'
)

AS

SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)
ORDER BY Localize.Description

GO

CREATE PROCEDURE GetCountriesFiltered
(
	@IDLang	nchar(2) = 'en',
	@Filter nvarchar(1000)
)

AS


SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     ((Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)) AND (PATINDEX('%' + Countries.CountryID + '%', @Filter) > 0)
ORDER BY Localize.Description

GO

CREATE PROCEDURE GetStates
(
	@CountryID nchar(2)
)

AS
SELECT  StateID, 
		Description
FROM    States
WHERE	CountryID = @CountryID
ORDER BY Description

GO

CREATE PROCEDURE AddUserFull
(
	@PortalID	    int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address	    nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
	@CountryID		nchar(2),  
	@StateID		int,
    @UserID		    int OUTPUT
)
AS

INSERT INTO Users
(
    PortalID,
    Name,
    Company,
	Address,		
	City,		
	Zip,		
	Phone,		
	Fax,		
	PIva,		
	CFiscale,	
	Email,		
	Password,
	SendNewsletter,
	CountryID,
	StateID
)

VALUES
(
    @PortalID,
    @Name,
    @Company,
	@Address,	
	@City,	
	@Zip,	
	@Phone,	
	@Fax,	
	@PIva,	
	@CFiscale,
    @Email,
    @Password,
    @SendNewsletter,
	@CountryID,
	@StateID
)

SELECT
    @UserID = @@IDENTITY
GO

CREATE PROCEDURE GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
	@IDLang	nchar(2) = 'IT'
)
AS

SELECT
	Users.UserID,
	Users.Email,
	Users.Password,
	Users.Name,
	Users.Company,
	Users.Address,
	Users.City,
	Users.Zip,
	Users.CountryID,
	Users.StateID,
	Users.PIva,
	Users.CFiscale,
	Users.Phone,
	Users.Fax,
	Users.SendNewsletter,
	Users.MailChecked,
	Users.PortalID,
	
	
	(SELECT TOP 1 Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     ((Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)) AND (Countries.CountryID = Users.CountryID))
	
	
	 AS Country
					  
FROM 
	Users LEFT OUTER JOIN
	States ON Users.StateID = States.StateID
	
WHERE
(Users.Email = @Email) AND (Users.PortalID = @PortalID)
GO

CREATE PROCEDURE GetUsers
(
@PortalID int
)
AS

SELECT     UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, CountryID, StateID, PIva, CFiscale, Phone, Fax
FROM         Users
WHERE     (PortalID = @PortalID)
ORDER BY Email

GO


CREATE PROCEDURE LocalizeManager
(
	@Key         nvarchar(50),
	@Translation nvarchar(255) = null,
	@Section	 varchar(255) = ''
)
AS

DECLARE @DefaultLanguage varchar(10)
SET @DefaultLanguage = 'en'

IF NOT EXISTS 
(
SELECT    TextKey
FROM      Localize
WHERE     (TextKey = @Key) AND (CultureCode = @DefaultLanguage)
)
INSERT Localize (TextKey, CultureCode, Description) Values (@Key, @DefaultLanguage, @Translation)

ELSE

UPDATE Localize
SET Description = @Translation
WHERE (TextKey = @Key) AND (CultureCode = @DefaultLanguage)

--Update locations
IF NOT EXISTS 
(
SELECT    TextKey
FROM      Sections
WHERE     (TextKey = @Key) AND (Section = @Section)
)
INSERT Sections (TextKey, Section) Values (@Key, @Section)
GO

CREATE PROCEDURE GetSingleCountry
(
	@IDState int,
	@IDLang	nchar(2) = 'en'
)

AS
SELECT     Countries.CountryID, Localize.Description, States.StateID
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID INNER JOIN
                      States ON Countries.CountryID = States.CountryID
WHERE     (Localize.CultureCode = @IDLang) AND (States.StateID = @IdState) OR
                      (States.StateID = @IdState) AND (Cultures.NeutralCode = @IDLang)
ORDER BY Localize.Description
GO

CREATE PROCEDURE UpdateUserFull
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

UPDATE Users

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
GO

CREATE PROCEDURE GetCulture
(
	@CountryID nchar(2)
)
AS
SELECT    CultureCode, CountryID
FROM      Cultures
WHERE     (CountryID = @CountryID)
GO

CREATE PROCEDURE GetDefaultCulture
(
	@CountryID nchar(2)
)
AS
SELECT    CultureCode, CountryID
FROM      Cultures
WHERE     (CountryID = @CountryID)
GO

