
if  exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentManager]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
    DROP TABLE [rb_ContentManager]
END

/*if NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rb_ContentManager]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)*/
BEGIN
	CREATE TABLE [rb_ContentManager] (
		[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
		[SourceGeneralModDefID] [uniqueidentifier] NOT NULL ,
		[DestGeneralModDefID] [uniqueidentifier] NULL ,
		[FriendlyName] [nvarchar] (128) NOT NULL ,
		[SourceSummarySproc] [nvarchar] (40) NOT NULL ,
		[DestSummarySproc] [nvarchar] (40) NOT NULL,
		[CopyItemSproc] [nvarchar] (40) NOT NULL ,
		[MoveItemLeftSproc] [nvarchar] (40) NOT NULL ,
		[MoveItemRightSproc] [nvarchar] (40) NOT NULL ,
		[CopyAllSproc] [nvarchar] (40)  NOT NULL ,
		[DeleteItemLeftSproc] [nvarchar] (40) NOT NULL,
		[DeleteItemRightSproc] [nvarchar] (40) NOT NULL 
	) ON [PRIMARY]

    ALTER TABLE [rb_ContentManager] WITH NOCHECK ADD
        CONSTRAINT [PK_rbContentManager] PRIMARY KEY  NONCLUSTERED
        (
            [ItemID]
        )  ON [PRIMARY]

    ALTER TABLE [rb_ContentManager] WITH NOCHECK ADD
        CONSTRAINT [FK_rbContentManager_SourceGenModDefs] FOREIGN KEY
        (
            [SourceGeneralModDefID]
        ) REFERENCES [rb_GeneralModuleDefinitions] (
            [GeneralModDefID]
        ) ON DELETE CASCADE NOT FOR REPLICATION
END
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleTypes]
GO

CREATE PROCEDURE rb_ContentMgr_GetModuleTypes AS
    SELECT
        ItemID,
        FriendlyName
    FROM
        rb_ContentManager
    ORDER BY FriendlyName ASC
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetPortals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetPortals]
GO

CREATE PROCEDURE rb_ContentMgr_GetPortals AS
    SELECT
        PortalID,
        PortalAlias
    FROM
        rb_Portals
    WHERE
        PortalID >= 0
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleInstances]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleInstances]
GO

CREATE PROCEDURE rb_ContentMgr_GetModuleInstances
(
    @ItemID     int,
    @PortalID   int
)
AS
    SELECT
        ModuleID,(TabName + '\' + ModuleTitle) AS TabModule
    FROM
        rb_ContentManager,rb_ModuleDefinitions,rb_Modules,rb_Pages
    WHERE
        rb_ContentManager.ItemID = @ItemID
            AND
        rb_ContentManager.SourceGeneralModDefID  = rb_ModuleDefinitions.GeneralModDefID
            AND
        rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
            AND
        rb_ModuleDefinitions.PortalID = @PortalID
            AND
        rb_Modules.TabID = rb_Pages.TabID

    ORDER BY
        rb_Pages.TabName,rb_Modules.ModuleTitle

GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetModuleInstancesExc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetModuleInstancesExc]
GO

CREATE PROCEDURE rb_ContentMgr_GetModuleInstancesExc
(
    @ItemID     int,
    @ExcludeItem  int,
    @PortalID   int
)
AS
    SELECT
        ModuleID,
        (TabName + '\' + ModuleTitle) AS TabModule
    FROM
        rb_ContentManager,rb_ModuleDefinitions,rb_Modules,rb_Pages
    WHERE
        rb_ContentManager.ItemID = @ItemID
            AND
        rb_Modules.ModuleID != @ExcludeItem
            AND
        rb_ContentManager.DestGeneralModDefID  = rb_ModuleDefinitions.GeneralModDefID
            AND
        rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
            AND
        rb_ModuleDefinitions.PortalID = @PortalID
            AND
        rb_Modules.TabID = rb_Pages.TabID
    ORDER BY
        rb_Pages.TabName,rb_Modules.ModuleTitle

GO

/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetSourceModuleData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetSourceModuleData]
GO

CREATE PROCEDURE rb_ContentMgr_GetSourceModuleData
(
    @ModuleID                   int,
    @ContentMgr_ItemID          int
)
AS
DECLARE @GetSummary     nvarchar(40)
SET @GetSummary = (SELECT SourceSummarySproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @GetSummary @ModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_GetDestModuleData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_GetDestModuleData]
GO

CREATE PROCEDURE rb_ContentMgr_GetDestModuleData
(
    @ModuleID                   int,
    @ContentMgr_ItemID          int
)
AS
DECLARE @GetSummary     nvarchar(40)
SET @GetSummary = (SELECT DestSummarySproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @GetSummary @ModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_MoveItemLeft]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_MoveItemLeft]
GO

CREATE PROCEDURE rb_ContentMgr_MoveItemLeft
(
    @ContentMgr_ItemID  int,
    @ItemID             int,
    @TargetModuleID     int
)
AS
DECLARE @MoveItemSproc     nvarchar(40)
SET @MoveItemSproc = (SELECT MoveItemLeftSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @MoveItemSproc @ItemID,@TargetModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_MoveItemRight]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_MoveItemRight]
GO

CREATE PROCEDURE rb_ContentMgr_MoveItemRight
(
    @ContentMgr_ItemID  int,
    @ItemID             int,
    @TargetModuleID     int
)
AS
DECLARE @MoveItemSproc     nvarchar(40)
SET @MoveItemSproc = (SELECT MoveItemRightSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @MoveItemSproc @ItemID,@TargetModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_CopyItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_CopyItem]
GO

CREATE PROCEDURE rb_ContentMgr_CopyItem
(
    @ContentMgr_ItemID  int,
    @ItemID             int,
    @TargetModuleID     int
)
AS
DECLARE @CopyItemSproc     nvarchar(40)
SET @CopyItemSproc = (SELECT CopyItemSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @CopyItemSproc @ItemID,@TargetModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_CopyAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_CopyAll]
GO

CREATE PROCEDURE rb_ContentMgr_CopyAll
(
    @ContentMgr_ItemID  int,
    @SourceModuleID     int,
    @TargetModuleID     int
)
AS
DECLARE @CopyAllSproc     nvarchar(40)
SET @CopyAllSproc = (SELECT CopyAllSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @CopyAllSproc @SourceModuleID,@TargetModuleID
GO
/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_DeleteItemLeft]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_DeleteItemLeft]
GO

CREATE PROCEDURE rb_ContentMgr_DeleteItemLeft
(
    @ContentMgr_ItemID  int,
    @ItemID             int
)
AS
DECLARE @DeleteItemSproc     nvarchar(40)
SET @DeleteItemSproc = (SELECT DeleteItemLeftSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @DeleteItemSproc @ItemID
GO

/*********************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ContentMgr_DeleteItemRight]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_ContentMgr_DeleteItemRight]
GO

CREATE PROCEDURE rb_ContentMgr_DeleteItemRight
(
    @ContentMgr_ItemID  int,
    @ItemID             int
)
AS
DECLARE @DeleteItemSproc     nvarchar(40)
SET @DeleteItemSproc = (SELECT DeleteItemRightSproc FROM rb_ContentManager WHERE ItemID = @ContentMgr_ItemID)

EXEC @DeleteItemSproc @ItemID
GO


