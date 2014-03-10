-------------------
--1.2.8.1774.sql
-------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
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
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
    @CountryID		nchar(2),  
	@StateID		int
)
AS

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
			SendNewsletter = @SendNewsletter,
			CountryID = @CountryID,
			StateID = @StateID
	
			WHERE UserID = @UserID
		END

		COMMIT
	END
GO