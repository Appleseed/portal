/* Install script, Events module, manudea 27/10/2003  */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Events] (
	[ItemID] [int] IDENTITY (1,1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (150) NULL ,
	[WhereWhen] [nvarchar] (150) NULL ,
	[Description] [nvarchar] (2000) NULL ,
	[ExpireDate] [datetime] NULL ,
	[AllDay] [bit] NOT NULL CONSTRAINT [DF__rb_Events__AllDay] DEFAULT (0),
	[StartDate] [datetime] NULL ,
	[StartTime] [nvarchar] (8) NULL ,
	CONSTRAINT [PK_rb_Events] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	),
	CONSTRAINT [FK_rb_Events_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Events_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_Events_st] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (150) NULL ,
	[WhereWhen] [nvarchar] (150) NULL ,
	[Description] [nvarchar] (2000) NULL ,
	[ExpireDate] [datetime] NULL ,
	[AllDay] [bit] NOT NULL CONSTRAINT [DF__rb_Events_st__AllDay] DEFAULT (0),
	[StartDate] [datetime] NULL ,
	[StartTime] [nvarchar] (8) NULL ,
	CONSTRAINT [PK_rb_Events_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	),
	CONSTRAINT [FK_rb_Events_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_AddEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddEvent]
GO

/* devsolution 2003/6/17: Added items for calendar control */
CREATE    PROCEDURE rb_AddEvent
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @AllDay	 bit,
    @StartDate   datetime,
    @StartTime   nvarchar(8),
    @ExpireDate  DateTime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100),
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_Events_st
(
    ModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    AllDay,
    StartDate,
    StartTime,
    ExpireDate,
    Description,
    WhereWhen

)

VALUES
(
    @ModuleID,
    @UserName,
    @Title,
    GETDATE(),
    @AllDay,
    @StartDate,
    @StartTime,
    @ExpireDate,
    @Description,
    @WhereWhen
)

SELECT
    @ItemID = @@IDENTITY
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_DeleteEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteEvent]
GO

CREATE   PROCEDURE rb_DeleteEvent
(
    @ItemID int
)
AS

DELETE FROM
    rb_Events_st

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetEvents]
GO

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
	    ExpireDate > GETDATE()
ORDER BY StartDate, StartTime
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
	    ExpireDate > GETDATE()
	ORDER BY StartDate, StartTime
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetSingleEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleEvent]
GO

/* devsolution 2003/6/17: Added items for calendar control */
CREATE    PROCEDURE rb_GetSingleEvent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    AllDay,
	    StartDate,
	    StartTime,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_Events
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    AllDay,
	    StartDate,
	    StartTime,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_Events_st
	WHERE
	    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateEvent]
GO

CREATE    PROCEDURE rb_UpdateEvent
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @AllDay	 bit,
    @StartDate   datetime,
    @StartTime   nvarchar(8),
    @ExpireDate  datetime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100)
)

AS

UPDATE
    rb_Events_st

SET
    CreatedByUser = @UserName,
    CreatedDate   = GETDATE(),
    Title         = @Title,
    AllDay        = @AllDay,
    StartDate	  = @StartDate,
    StartTime     = @StartTime,
    ExpireDate    = @ExpireDate,
    Description   = @Description,
    WhereWhen     = @WhereWhen

WHERE
    ItemID = @ItemID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_Events_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Events_stModified]
GO

CREATE  TRIGGER [rb_Events_stModified]
ON [rb_Events_st]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC rb_ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules
END
GO
