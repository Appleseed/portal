---------------------
--1.2.8.1719.sql
---------------------

/* devsolution 2003/6/17: Added items for calendar control */
/* Alter table to add columns for calendar */
/****** Object:  Table [EventCalendar]    Script Date: 5/18/2003 4:00:38 PM ******/
SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
ALTER TABLE [rb_Events] 
ADD 	[AllDay][bit] NOT NULL DEFAULT 0,
 	[StartDate] [datetime] NULL ,
	[StartTime] [nvarchar] (8) 
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
ALTER TABLE [rb_Events_st] 
ADD 	[AllDay][bit] NOT NULL DEFAULT 0,
 	[StartDate] [datetime] NULL ,
	[StartTime] [nvarchar] (8) 
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/
/* devsolution 2003/6/17: Added items for calendar control */
ALTER PROCEDURE rb_GetEvents
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

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_GetSingleEvent    Data dello script: 07/11/2002 22.28.14 ******/
/* devsolution 2003/6/17: Added items for calendar control */
ALTER    PROCEDURE rb_GetSingleEvent
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
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure rb_UpdateEvent    Data dello script: 07/11/2002 22.28.14 ******/
/* devsolution 2003/6/17: Added items for calendar control */
ALTER    PROCEDURE rb_UpdateEvent
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
    CreatedDate   = GetDate(),
    Title         = @Title,
    AllDay        = @AllDay,
    StartDate	  = @StartDate,
    StartTime     = @StartTime,
    ExpireDate    = @ExpireDate,
    Description   = @Description,
    WhereWhen     = @WhereWhen

WHERE
    ItemID = @ItemID

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.28.12 ******/
/* devsolution 2003/6/17: Added items for calendar control */
ALTER    PROCEDURE rb_AddEvent
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
    GetDate(),
    @AllDay,
    @StartDate,
    @StartTime,
    @ExpireDate,
    @Description,
    @WhereWhen
)

SELECT
    @ItemID = @@IDENTITY

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1719','1.2.8.1719', CONVERT(datetime, '06/20/2003', 101))
GO