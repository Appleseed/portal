-----------------------
----1.2.8.1623.sql
-----------------------


---- Add new module: Tasks
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531012}'
--SET @FriendlyName = 'Tasks'
--SET @DesktopSrc = 'DesktopModules/Tasks.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesTasks'
--SET @Admin = 0
--SET @Searchable = 1


--IF NOT EXISTS (SELECT GeneralModDefID FROM rb_GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN

---- Create the table AND add constraints
--CREATE TABLE [rb_Tasks] (
--    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
--    [ModuleID] [int] NOT NULL ,
--    [CreatedByUser] [nvarchar] (100) NULL ,
--    [CreatedDate] [datetime] NULL ,
--    [ModifiedByUser] [nvarchar] (100) NULL ,
--    [ModifiedDate] [datetime] NULL ,
--    [AssignedTo] [nvarchar] (50) NULL ,
--    [Title] [nvarchar] (100) NOT NULL ,
--    [Description] [nvarchar] (3000) NULL ,
--    [Status] [nvarchar] (20) NULL ,
--    [Priority] [nvarchar] (20) NULL ,
--    [PercentComplete] [int] NULL ,
--    [StartDate] [datetime] NULL ,
--    [DueDate] [datetime] NULL
--) ON [PRIMARY]

--ALTER TABLE [rb_Tasks] WITH NOCHECK ADD 
--    CONSTRAINT [PK_Tasks] PRIMARY KEY  NONCLUSTERED 
--    (
--        [ItemID]
--    )  ON [PRIMARY] 

--ALTER TABLE [rb_Tasks] ADD 
--    CONSTRAINT [FK_Tasks_Modules] FOREIGN KEY 
--    (
--        [ModuleID]
--    ) REFERENCES [rb_Modules] (
--        [ModuleID]
--    ) ON DELETE CASCADE  NOT FOR REPLICATION 


---- Insert templated records for tasks module
--INSERT INTO rb_TASKS (ModuleID,Title) VALUES (0,' ')

---- Installs module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

---- Install it for default portal
--EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
--END
--Go


---- Drop any existing procedures

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddTask]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddTask]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteTask]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DeleteTask]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetTasks]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetTasks]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleTask]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSingleTask]
--GO

--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateTask]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateTask]
--GO


---- =============================================================
---- create the stored procs
---- =============================================================

--CREATE PROCEDURE rb_AddTask
--(
--    @ModuleID int,
--    @UserName nvarchar(100),
--    @AssignedTo     nvarchar(50),
--    @Title     nvarchar(100),
--    @Description    nvarchar(3000),
--    @Status nvarchar(20),
--    @Priority nvarchar(20),
--    @PercentComplete int,
--    @StartDate datetime,
--    @DueDate datetime,
--    @ItemID   int OUTPUT
--)
--AS

--INSERT INTO rb_Tasks
--(
--    CreatedByUser,
--    CreatedDate,
--    ModifiedByUser,
--    ModifiedDate,
--    ModuleID,
--    AssignedTo,
--    Title,
--    Description,
--    Status,
--    Priority,
--    PercentComplete,
--    StartDate,
--    DueDate
--)

--VALUES
--(
--    @UserName,
--    GetDate(),
--    @UserName,
--    GetDate(),
--    @ModuleID,
--    @AssignedTo,
--    @Title,
--    @Description,
--    @Status,
--    @Priority,
--    @PercentComplete,
--    @StartDate,
--    @DueDate
--)

--SELECT
--    @ItemID = @@IDENTITY


--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO


--CREATE PROCEDURE rb_DeleteTask
--(
--    @ItemID int
--)
--AS

--DELETE FROM
--    rb_Tasks

--WHERE
--    ItemID = @ItemID


--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO


--CREATE PROCEDURE rb_GetTasks
--(
--    @ModuleID int
--)
--AS

--SELECT
--    ItemID,
--    CreatedByUser,
--    CreatedDate,
--    AssignedTo,
--    Title,
--    Description,
--    Status,
--    Priority,
--    PercentComplete,
--    StartDate,
--    DueDate

--FROM
--    rb_Tasks

--WHERE
--    ModuleID = @ModuleID


--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO



--CREATE PROCEDURE rb_GetSingleTask
--(
--    @ItemID int
--)
--AS

--SELECT
--    ItemID,
--    CreatedByUser,
--    CreatedDate,
--    ModifiedByUser,
--    ModifiedDate,
--    AssignedTo,
--    Title,
--    Description,
--    Status,
--    Priority,
--    PercentComplete,
--    StartDate,
--    DueDate

--FROM
--    rb_Tasks

--WHERE
--    ItemID = @ItemID


--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO


--CREATE PROCEDURE rb_UpdateTask
--(
--    @ItemID int,
--    @UserName nvarchar(100),
--    @AssignedTo     nvarchar(50),
--    @Title     nvarchar(100),
--    @Description    nvarchar(3000),
--    @Status nvarchar(20),
--    @Priority nvarchar(20),
--    @PercentComplete int,
--    @StartDate datetime,
--    @DueDate datetime
--)
--AS

--UPDATE
--    rb_Tasks

--SET
--    ModifiedByUser   = @UserName,
--    ModifiedDate     = GetDate(),
--    AssignedTo      = @AssignedTo,
--    Title           = @Title,
--    Description     = @Description,
--    Status          = @Status,
--    Priority        = @Priority,
--    PercentComplete = @PercentComplete,
--    StartDate       = @StartDate,
--    DueDate         = @DueDate

--WHERE
--    ItemID = @ItemID


--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

--SET QUOTED_IDENTIFIER ON 
--GO
--SET ANSI_NULLS ON 
--GO


--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1623','1.2.8.1623', CONVERT(datetime, '04/22/2003', 101))
--GO
