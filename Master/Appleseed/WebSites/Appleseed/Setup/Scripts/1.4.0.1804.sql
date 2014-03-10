-- Alter and Add these columns to the users table and alters procedures
ALTER TABLE [rb_Users]
	ALTER COLUMN Password varchar(40) NULL
GO
ALTER TABLE [rb_Users] 
	ADD [Salt] [varchar] (10)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUser]
GO

CREATE PROCEDURE rb_AddUser
(
    @PortalID int,
    @Name     nvarchar(50),
    @Email    nvarchar(100),
    @Password varchar(40),
    @Salt	  varchar(10),
    @UserID   int OUTPUT
)
AS
INSERT INTO rb_Users
(
    Name,
    Email,
    Password,
    Salt,
    PortalID
)
VALUES
(
    @Name,
    @Email,
    @Password,
    @Salt,
    @PortalID
)
SELECT
    @UserID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUserFull]
GO

CREATE PROCEDURE rb_AddUserFull
(
    @PortalID	    	    int,
    @Name		    nvarchar(50),
    @Company	            nvarchar(50),
    @Address	            nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	            nvarchar(16),
    @Email		    nvarchar(100),
    @Password	            varchar(40),
    @Salt					varchar(10),
    @SendNewsletter	    bit,
    @CountryID		    nchar(2),  
    @StateID	            int,
    @UserID		    int OUTPUT
)
AS
INSERT INTO rb_Users
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
	Salt,
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
    @Salt,
    @SendNewsletter,
	@CountryID,
	@StateID
)
SELECT
    @UserID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUser]
GO

CREATE PROCEDURE rb_UpdateUser
(
	@PortalID		int,
    @UserID         int,
    @Name			nvarchar(50),
    @Email          nvarchar(100),
    @Password	    varchar(40),
    @Salt			varchar(10),
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
    Salt	 = @Salt,
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFull]
GO

CREATE PROCEDURE rb_UpdateUserFull
(
    @UserID		    int,
    @OldUserID		int = @UserID,
    @PortalID       int,
    @Name		nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    varchar(40),
    @Salt			varchar(10),
    @SendNewsletter	bit,
    @CountryID		nchar(2),  
	@StateID		int
)
AS
-- 1774 fix
-- Get password if not set
If (len(@Password) <= 0)
	BEGIN
		SELECT @Password = (SELECT Password from rb_Users WHERE UserID = @OldUserID)
	END
	
	
DECLARE @err_message nvarchar(255)
	
-- Normal update
if (@OldUserID = @UserID)
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
		Salt = @Salt,
		SendNewsletter = @SendNewsletter,
		CountryID = @CountryID,
		StateID = @StateID

		WHERE UserID = @UserID
	END
ELSE
	BEGIN
		--if user found with same number return SEV 11 error to abort stored procedure
		IF EXISTS (SELECT UserID FROM rb_Users WHERE UserID = @UserID)
			BEGIN	
			SET @err_message = 'The number ' + Convert(varchar(20), @UserID) + ' is already in use!'
			RAISERROR (@err_message, 11,1)
			RETURN
			END
		
		-- Wrap on transaction
		BEGIN TRANSACTION

		-- Allow Identity change
		SET IDENTITY_INSERT rb_Users ON

		--Insert record as changed one
		INSERT INTO rb_Users
		(
			UserID,
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
			Salt,
			SendNewsletter,
			CountryID,
			StateID
		)

		VALUES
		(
			@UserID,
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
			@Email + CAST(RAND( (DATEPART(mm, GETDATE()) * 100000 ) + (DATEPART(ss, GETDATE()) * 1000 ) + DATEPART(ms, GETDATE()) ) AS VARCHAR(20)),
			@Password,
			@Salt,
			@SendNewsletter,
			@CountryID,
			@StateID
		)
		
		-- Restore identity off
		SET IDENTITY_INSERT rb_Users OFF

		--Migrate Roles
                INSERT INTO rb_UserRoles (UserID, RoleID) (SELECT @UserID UID, rb_UserRoles.RoleID FROM rb_UserRoles WHERE rb_UserRoles.UserID = @OldUserID)

		-- Remove the old one
		DELETE rb_Users  WHERE UserID = @OldUserID
	
		-- Update user with correct email
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
			Salt = @Salt,
			SendNewsletter = @SendNewsletter,
			CountryID = @CountryID,
			StateID = @StateID
	
			WHERE UserID = @UserID
		END

		COMMIT
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserLogin]
GO

CREATE PROCEDURE rb_UserLogin
(
    @PortalID int,
    @Email    nvarchar(100),
    @Password varchar(40),
    @Salt varchar(10)
)
AS
SELECT rb_Users.UserID, rb_Users.Name, rb_Users.Email, rb_Users.Password, rb_Users.Salt
FROM   rb_Users
WHERE  (Email = @Email) AND (PortalID = @PortalID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HashPassworda]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_HashPassworda]
GO

CREATE PROCEDURE rb_HashPassworda
(
	@Password 	varchar(40),
	@Salt		varchar(10)
)
AS
UPDATE  rb_Users
SET	Password = @Password,
	Salt = @Salt
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HashPasswordb]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_HashPasswordb]
GO

CREATE PROCEDURE rb_HashPasswordb
AS
SELECT rb_Users.Password, rb_Users.Salt
    FROM   rb_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HashPasswordc]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_HashPasswordc]
GO

CREATE PROCEDURE rb_HashPasswordc
(
	@Email		nvarchar(100),
	@Password 	varchar(40),
	@Salt		varchar(10)
)
AS
UPDATE  rb_Users
SET	Password = @Password,
	Salt = @Salt
	WHERE Email = @Email
GO
