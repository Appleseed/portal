---------------------
--1.2.8.1708.sql
---------------------


-- Add new module: ComponentModule
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{2B113F51-FEA3-499A-98E7-7B83C192FDBC}'
SET @FriendlyName = 'ComponentModule'
SET @DesktopSrc = 'DesktopModules/ComponentModule/ComponentModule.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesComponentModule'
SET @Admin = 0
SET @Searchable = 1


IF NOT EXISTS (SELECT GeneralModDefID FROM rb_GeneralModuleDefinitions
WHERE GeneralModDefID = @GeneralModDefID)
BEGIN
    -- Installs module
    EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

    -- Install it for default portal
    EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
END
GO

if NOT exists (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ComponentModule]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    -- Create the table AND add constraints
    CREATE TABLE [rb_ComponentModule] (
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[Component] [nvarchar] (2000) NULL
    ) ON [PRIMARY]

    ALTER TABLE [rb_ComponentModule] WITH NOCHECK ADD 
    CONSTRAINT [PK_ComponentModule] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleID]
    )  ON [PRIMARY] 

    ALTER TABLE [rb_ComponentModule] ADD 
    CONSTRAINT [FK_ComponentModule_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [rb_Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
END
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'rb_GetComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_GetComponentModule
GO

/* PROCEDURE rb_GetComponentModule*/
CREATE PROCEDURE rb_GetComponentModule
@ModuleID int
AS
SELECT
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	Component
FROM
	rb_ComponentModule
WHERE
	ModuleID = @ModuleID
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'rb_UpdateComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_UpdateComponentModule
GO

/* PROCEDURE rb_UpdateComponentModule*/
CREATE PROCEDURE rb_UpdateComponentModule
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@Title nvarchar(100),
	@Component nvarchar(2000)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_ComponentModule
    WHERE 
        ModuleID = @ModuleID
)
    INSERT INTO rb_ComponentModule (
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	Component
    ) 
    VALUES (
	@ModuleID,
	@CreatedByUser,
	GetDate(),
	@Title,
	@Component
    )
ELSE
     UPDATE rb_ComponentModule
     SET
	CreatedByUser = @CreatedByUser,
	CreatedDate = GetDate(),
	Title = @Title,
	Component = @Component
     WHERE
	ModuleID = @ModuleID
GO



/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1708','1.2.8.1708', CONVERT(datetime, '05/09/2003', 101))
GO
