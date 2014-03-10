/* Install script, ComponentModule, Jakob Hansen, 9 may 2003 */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_ComponentModule]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
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


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'rb_GetComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_GetComponentModule
GO

/* Procedure rb_GetComponentModule*/
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


IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'rb_UpdateComponentModule') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE rb_UpdateComponentModule
GO

/* Procedure rb_UpdateComponentModule*/
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
	GETDATE(),
	@Title,
	@Component
    )
ELSE
     UPDATE rb_ComponentModule
     SET
	CreatedByUser = @CreatedByUser,
	CreatedDate = GETDATE(),
	Title = @Title,
	Component = @Component
     WHERE
	ModuleID = @ModuleID
GO
