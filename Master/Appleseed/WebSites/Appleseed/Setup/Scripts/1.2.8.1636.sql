---------------------
--1.2.8.1636.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_UserRoles_rb_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_UserRoles
	DROP CONSTRAINT FK_rb_UserRoles_rb_Users
GO

ALTER TABLE rb_UserRoles WITH NOCHECK ADD CONSTRAINT
	FK_rb_UserRoles_rb_Users FOREIGN KEY
	(
	UserID
	) REFERENCES rb_Users
	(
	UserID
	) ON UPDATE CASCADE
	 ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFullNewId]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFullNewId]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddUserFullNewId]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddUserFullNewId]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFull]
GO

CREATE PROCEDURE rb_UpdateUserFull
(
    @UserID		    int,
    @OldUserID		int = @UserID,
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
    @CountryID		nchar(2),  
	@StateID		int
)
AS

-- Get password if not set
If (len(@Password) <= 0)
	BEGIN
		SELECT @Password = (SELECT Password from rb_Users WHERE UserID = @OldUserID)
	END
	
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
		-- Wrap on transaction
		BEGIN TRANSACTION

		-- Allow Identity change
		SET IDENTITY_INSERT rb_Users ON

		-- Remove the old one or we get error for duplicate values
		DELETE rb_Users  WHERE UserID = @OldUserID

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
			@Email,
			@Password,
			@SendNewsletter,
			@CountryID,
			@StateID
		)
		
		-- Restore identity off
		SET IDENTITY_INSERT rb_Users OFF

		COMMIT
	END
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1636','1.2.8.1636', CONVERT(datetime, '04/30/2003', 101))
GO
