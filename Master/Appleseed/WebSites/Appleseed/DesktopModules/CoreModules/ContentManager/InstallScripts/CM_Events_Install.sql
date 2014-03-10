/*
SUGGESTIONS:
    -if the module has workflow, use the workflow table!!!!
    -you can copy the list of fields for the _CopyItem sproc from
        the Add method of that module.  Ex get the fields for
        rb_Announcements_CopyItem from rb_AddAnnouncement.  Just
        copy+paste the list twice, add the rest and use the
        @ModuleID field.
    -the _Summary function MUST, MUST,MUST return both the
        ItemID field(named ItemID, you can alias it for others)
        ItemDesc(alias + concatenate the fields you want into this one)

    -Add a record into the rb_ContentManager for the sprocs you made.
*/

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetEvents_Summary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEvents_Summary]
GO

CREATE PROCEDURE rb_GetEvents_Summary
(
    @ModuleID   int
)
AS
SELECT ItemID, Title + '::    ' + WhereWhen + LEFT(Description,200) As ItemDesc
    FROM rb_Events_st WHERE ModuleID = @ModuleID
GO
/***************************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_CopyItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_CopyItem]
GO

CREATE PROCEDURE rb_Events_CopyItem
(
    @ItemID          int,        /*Item that will be copied*/
    @TargetModuleID  int         /*Where to copy it to*/
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
SELECT
    @TargetModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    AllDay,
    StartDate,
    StartTime,
    ExpireDate,
    Description,
    WhereWhen
FROM
    rb_Events_st WHERE ItemID = @ItemID
GO
/***************************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_MoveItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_MoveItem]
GO

CREATE PROCEDURE rb_Events_MoveItem
(
    @ItemID         int,
    @TargetModuleID int
)
AS
UPDATE rb_Events_st
    SET ModuleID = @TargetModuleID
        WHERE ItemID = @ItemID
GO
/***************************************************************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Events_CopyAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_Events_CopyAll]
GO

CREATE PROCEDURE rb_Events_CopyAll
(
    @SourceModuleID  int,        /*Item that will be copied*/
    @TargetModuleID  int         /*Where to copy it to*/
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
SELECT
    @TargetModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    AllDay,
    StartDate,
    StartTime,
    ExpireDate,
    Description,
    WhereWhen
FROM
    rb_Events_st WHERE ModuleID = @SourceModuleID
GO
/***************************************************************************************/
INSERT INTO rb_ContentManager(
    SourceGeneralModDefID,
    DestGeneralModDefID,
    FriendlyName,
    SourceSummarySproc,
    DestSummarySproc,
    CopyItemSproc,
    MoveItemLeftSproc,
    MoveItemRightSproc,
    CopyAllSproc,
    DeleteItemLeftSproc,
    DeleteItemRightSproc
)
VALUES('ef9b29c5-e481-49a6-9383-8ed3ab42dda0',
	'ef9b29c5-e481-49a6-9383-8ed3ab42dda0',
    'Events',
    'rb_GetEvents_Summary',
    'rb_GetEvents_Summary',
    'rb_Events_CopyItem',
    'rb_Events_MoveItem',
    'rb_Events_MoveItem',
    'rb_Events_CopyAll',
    'rb_DeleteEvent',
    'rb_DeleteEvent')
GO
