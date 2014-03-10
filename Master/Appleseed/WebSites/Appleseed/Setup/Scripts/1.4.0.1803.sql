if exists (select * from dbo.sysobjects where id = object_id(N'[rb_MoveModuleToNewTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_MoveModuleToNewTab]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_DeleteModuleToRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_DeleteModuleToRecycler]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetModuleSettingsForIndividualModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModuleSettingsForIndividualModule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetModulesInRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModulesInRecycler]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_Recycler]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [rb_Recycler]
GO

CREATE TABLE [rb_Recycler] (
	[ModuleID] [int] NOT NULL ,
	[DateDeleted] [datetime] NOT NULL ,
	[DeletedBy] [nvarchar] (250) NULL ,
	[OriginalTab] [int] NOT NULL 
) ON [PRIMARY]
GO


--Simply changes a module's TabID value to the new value
--Used by recycler to restore modules to tabs
CREATE PROCEDURE rb_MoveModuleToNewTab
(
    @ModuleID  int,
    @TabID int
)
AS
BEGIN TRAN

UPDATE rb_Modules
SET TabID = @TabID
WHERE ModuleID = @ModuleID

DELETE FROM rb_Recycler
WHERE ModuleID = @ModuleID
COMMIT TRAN

GO

CREATE  PROCEDURE rb_DeleteModuleToRecycler
(
    @ModuleID       int,
    @DeletedBy nvarchar(250),
    @DateDeleted datetime
)
AS
DECLARE @intErrorCode INT
DECLARE @intOldTabId INT

SELECT @intOldTabId = (SELECT TOP 1 TabID FROM rb_Modules WHERE ModuleID = @ModuleID)
IF @intOldTabId <> 0  --IF IT'S NOT ALREADY IN THE RECYCLER
 BEGIN  --PUT IT THERE
  BEGIN TRAN
	UPDATE rb_Modules 
	SET TabID = 0
	WHERE ModuleID = @ModuleID

    SELECT @intErrorCode = @@ERROR
    IF (@intErrorCode <> 0) GOTO PROBLEM

    INSERT INTO rb_Recycler 
	(ModuleID, DateDeleted, DeletedBy, OriginalTab)
	VALUES	(@ModuleID, @DateDeleted, @DeletedBy, @intOldTabId)

    SELECT @intErrorCode = @@ERROR
    IF (@intErrorCode <> 0) GOTO PROBLEM
COMMIT TRAN
 END
ELSE  --IT WAS ALREADY IN THE RECYCLER, AND THE USER CALLED 'DELETE' FROM THE RECYCLER MODULE
 BEGIN
BEGIN TRAN
 	DELETE FROM rb_Modules WHERE ModuleID = @ModuleID
 	DELETE FROM rb_Recycler WHERE ModuleID = @ModuleID
COMMIT TRAN
 END


PROBLEM:
IF (@intErrorCode <> 0) BEGIN
PRINT 'Unexpected error occurred!'
    ROLLBACK TRAN
END

GO

CREATE PROCEDURE rb_GetModuleSettingsForIndividualModule
(
    @ModuleID  int
)
AS
/* Get the DataTable of module info */
SELECT     TOP 1 *
FROM	rb_Modules INNER JOIN
	rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.ModuleID = @ModuleID)


GO
CREATE PROCEDURE rb_GetModulesInRecycler
(
 @PortalID int,
 @SortField VarChar(50)
)
AS
SELECT     
	rb_Recycler.DateDeleted, 
	rb_Recycler.DeletedBy, 
	rb_Recycler.OriginalTab, 
	rb_Recycler.ModuleID, 
	rb_Modules.ModuleTitle, 
    rb_Tabs.TabName as 'OriginalTabName'

FROM    rb_Recycler INNER JOIN
        rb_Modules ON rb_Recycler.ModuleID = rb_Modules.ModuleID INNER JOIN
        rb_Tabs ON rb_Recycler.OriginalTab = rb_Tabs.TabID
WHERE PortalID = @PortalID
ORDER BY 
	CASE 
	 WHEN @SortField = 'DateDeleted' THEN CAST(DateDeleted AS VarChar(50))
         WHEN @SortField = 'DeletedBy' THEN CAST(DeletedBy AS VarChar(50))
         WHEN @SortField = 'OriginalTab' THEN CAST(OriginalTab AS VarChar(50))
         WHEN @SortField = 'ModuleID' THEN CAST(rb_Recycler.ModuleID AS VarChar(50)) 
         WHEN @SortField = 'ModuleTitle' THEN CAST(ModuleTitle AS VarChar(50))
         WHEN @SortField = 'TabName' THEN CAST(rb_Tabs.TabName AS VarChar(50))
         ELSE ModuleTitle
     END
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthDeleteRolesRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthDeleteRolesRecycler]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthViewRolesRecycler]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthViewRolesRecycler]
GO


CREATE  PROCEDURE rb_GetAuthDeleteRolesRecycler
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @DeleteRoles   = rb_Modules.AuthorizedDeleteRoles
    
FROM    
    rb_Modules
  INNER JOIN
	rb_Recycler ON rb_Modules.ModuleID = rb_Recycler.ModuleID
  INNER join
    rb_Tabs ON rb_Recycler.OriginalTab = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO



CREATE  PROCEDURE rb_GetAuthViewRolesRecycler
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @ViewRoles   = rb_Modules.AuthorizedViewRoles
    
FROM    
    rb_Modules
  INNER JOIN
	rb_Recycler ON rb_Modules.ModuleID = rb_Recycler.ModuleID
  INNER join
    rb_Tabs ON rb_Recycler.OriginalTab = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO



