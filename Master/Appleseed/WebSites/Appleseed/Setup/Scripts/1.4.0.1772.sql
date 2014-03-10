/* Add AuthorizedMoveModuleRoles & AuthorizedDeleteModuleRoles in table rb_Modules */
IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedMoveModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
	[AuthorizedMoveModuleRoles] [nvarchar] (256) NULL 
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects SO INNER JOIN syscolumns SC ON SO.id=SC.id WHERE SO.id = OBJECT_ID(N'[rb_Modules]') AND OBJECTPROPERTY(SO.id, N'IsUserTable') = 1 AND SC.name=N'AuthorizedDeleteModuleRoles')
BEGIN
ALTER TABLE [rb_Modules] WITH NOCHECK ADD 
	[AuthorizedDeleteModuleRoles] [nvarchar] (256) NULL 
END
GO

/* Checks to see if rb_AddModule already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_AddModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_AddModule]
GO

CREATE PROCEDURE rb_AddModule
(
    @TabID                  int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @ModuleDefID            int,
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles	    nvarchar(256),
    @MoveModuleRoles	    nvarchar(256),
    @DeleteModuleRoles	    nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles        nvarchar(256),
    @SupportWorkflow	    bit,
    @ShowEveryWhere         bit,
    @SupportCollapsable     bit,
    @ModuleID               int OUTPUT
   
)
AS
INSERT INTO rb_Modules
(
    TabID,
    ModuleOrder,
    ModuleTitle,
    PaneName,
    ModuleDefID,
    CacheTime,
    AuthorizedEditRoles,
    AuthorizedAddRoles,
    AuthorizedViewRoles,
    AuthorizedDeleteRoles,
    AuthorizedPropertiesRoles,
    AuthorizedMoveModuleRoles,
    AuthorizedDeleteModuleRoles,
    ShowMobile,
    AuthorizedPublishingRoles,
    NewVersion, 
    SupportWorkflow,
    SupportCollapsable,
    ShowEveryWhere    
) 
VALUES
(
    @TabID,
    @ModuleOrder,
    @ModuleTitle,
    @PaneName,
    @ModuleDefID,
    @CacheTime,
    @EditRoles,
    @AddRoles,
    @ViewRoles,
    @DeleteRoles,
    @PropertiesRoles,
    @MoveModuleRoles,
    @DeleteModuleRoles,
    @ShowMobile,
    @PublishingRoles,
    1, -- False
    @SupportWorkflow,
    @SupportCollapsable,
    @ShowEveryWhere  
)
SELECT 
    @ModuleID = @@IDENTITY

GO

/* Checks to see if rb_UpdateModule already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_UpdateModule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateModule]
GO

CREATE PROCEDURE [rb_UpdateModule]
(
    @ModuleID               int,
    @TabID				    int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles        nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles	    nvarchar(256),
    @MoveModuleRoles	    nvarchar(256),
    @DeleteModuleRoles	    nvarchar(256),
    @SupportWorkflow	    bit,
    @ApprovalRoles			nvarchar(256),
    @ShowEveryWhere         bit,
    @SupportCollapsable     bit
)
AS
UPDATE
    rb_Modules
SET
   TabID			= @TabID,
    ModuleOrder               	= @ModuleOrder,
    ModuleTitle               	= @ModuleTitle,
    PaneName                  	= @PaneName,
    CacheTime                 	= @CacheTime,
    ShowMobile                	= @ShowMobile,
    AuthorizedEditRoles       	= @EditRoles,
    AuthorizedAddRoles        	= @AddRoles,
    AuthorizedViewRoles       	= @ViewRoles,
    AuthorizedDeleteRoles     	= @DeleteRoles,
    AuthorizedPropertiesRoles 	= @PropertiesRoles,
    AuthorizedMoveModuleRoles 	= @MoveModuleRoles,
    AuthorizedDeleteModuleRoles = @DeleteModuleRoles,
    AuthorizedPublishingRoles 	= @PublishingRoles,
    SupportWorkflow	        = @SupportWorkflow,
    AuthorizedApproveRoles    	= @ApprovalRoles,
    SupportCollapsable		= @SupportCollapsable,
    ShowEveryWhere              = @ShowEveryWhere
WHERE
    ModuleID = @ModuleID

GO

/* Checks to see if rb_GetAuthDeleteModuleRoles already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthDeleteModuleRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthDeleteModuleRoles]
GO

CREATE PROCEDURE rb_GetAuthDeleteModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @DeleteModuleRoles  = rb_Modules.AuthorizedDeleteModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO

/* Checks to see if rb_GetAuthMoveModuleRoles already exists and delete */
if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetAuthMoveModuleRoles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetAuthMoveModuleRoles]
GO

CREATE PROCEDURE rb_GetAuthMoveModuleRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @MoveModuleRoles   nvarchar (256) OUTPUT
)
AS
SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @MoveModuleRoles  = rb_Modules.AuthorizedMoveModuleRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO


