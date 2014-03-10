--------------------------------
--1.2.8.1713.sql bja@reedtek.com
-- Changes to support collapsable AND save modules
-- some fixes by Manu - 18/06/2003
--------------------------------

---------------------
-- Insert information for localization (English)
---------------------
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SWI_BUTTON_MAX','en','Maximize Window')
GO
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SWI_BUTTON_MIN','en','Minimize Window')
GO
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SWI_BUTTON_CLOSE','en','Close Window')
GO
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SHOWCOLLAPSABLE','en','Can collapse window?')
GO
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SAVE_DESKTOP','en','Save Desktop')
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserDesktop]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
---------------------
-- Create user window desktop-table
---------------------
CREATE TABLE [rb_UserDesktop] (
	[UserID] [int] NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[TabID] [int] NOT NULL ,
	[PortalID] [int] NOT NULL ,
	[State] [smallint] NOT NULL 
) ON [PRIMARY]

---------------------
-- Set constraints/indexes
---------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_rb_UserDesktop]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE [rb_UserDesktop] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_UserDesktop] PRIMARY KEY  CLUSTERED 
	(
		[UserID],
		[ModuleID],
		[TabID],
		[PortalID]
	)  ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_UserDesktop_rb_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_UserDesktop] ADD 
	CONSTRAINT [FK_rb_UserDesktop_rb_Users] FOREIGN KEY 
	(
		[UserID]
	) REFERENCES [rb_Users] (
		[UserID]
	) ON DELETE CASCADE 
GO

--------------------------------------
-- Add support for collapsable/modules
--------------------------------------
ALTER TABLE rb_Modules ADD SupportCollapsable bit NULL
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteUserDeskTop]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteUserDeskTop]
GO

/* deletes all  user desktop values for the specified user */
CREATE PROCEDURE rb_DeleteUserDeskTop
(
    @UserID       int,
    @PortalID    int
)
AS

DELETE 
  
FROM
    rb_UserDeskTop

WHERE   
    UserID = @UserID AND PortalID = @PortalID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserDeskTop]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserDeskTop]
GO

CREATE PROCEDURE rb_UpdateUserDeskTop
(
    @PortalID 		int,
    @UserID 		int,
    @ModuleID        	int,
    @TabID          	int,
    @WindowState 	smallint


)
AS
IF  (
    SELECT 
       count( *) 
    FROM 
       rb_UserDeskTop(nolock)
    WHERE 
        UserID = @UserID AND  TabID = @TabID AND PortalID = @PortalID AND   ModuleID=@ModuleID
) = 0

BEGIN
    -- Transacted insert for download count
    BEGIN TRAN
	INSERT INTO rb_UserDeskTop (
	    UserID,
	    ModuleID,
	    TabID,
	    PortalID,
	    State

	) 
	VALUES (
	    @UserID,
	    @ModuleID,
	    @TabID,
	    @PortalID,
	    @WindowState
	
	)
    COMMIT TRAN
END
ELSE

BEGIN
    -- Transacted insert for download count
    BEGIN TRAN
	UPDATE
	    rb_UserDeskTop
	
	SET
	    UserID=  @UserID,
	    ModuleID=@ModuleID,
	    TabID=@TabID,
	    PortalID=@PortalID,
	    State=@WindowState
	WHERE
	        UserID = @UserID AND  TabID = @TabID AND PortalID = @PortalID AND   ModuleID=@ModuleID
     COMMIT TRAN
END
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1713','1.2.8.1713', CONVERT(datetime, '05/22/2003', 101))
GO

