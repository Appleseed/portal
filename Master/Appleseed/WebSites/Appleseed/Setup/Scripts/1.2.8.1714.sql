-------------------
--1.2.8.1714.sql
---------------------

ALTER TABLE rb_Modules ADD
	ShowEveryWhere bit NULL
GO

ALTER TABLE rb_Modules ADD CONSTRAINT
	DF_rb_Modules_ShowEveryWhere DEFAULT 0 FOR ShowEveryWhere
GO

ALTER TABLE rb_Modules ADD CONSTRAINT
	DF_rb_Modules_SupportCollapsable DEFAULT 0 FOR SupportCollapsable
GO

UPDATE rb_Modules
SET ShowEveryWhere = 0
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModule]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddModule]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- End change marcb@hotmail.com
-- End Change Geert.Audenaert@Syntegra.Com

--Manu - Fix for Desktop tabs
CREATE PROCEDURE rb_GetPortalSettings
(
    @PortalAlias   nvarchar(50),
    @TabID         int,
    @PortalID      int OUTPUT,
    @PortalName    nvarchar(128) OUTPUT,
    @PortalPath    nvarchar(128) OUTPUT,
    @AlwaysShowEditButton bit OUTPUT,
    @TabName       nvarchar (50)  OUTPUT,
    @TabOrder      int OUTPUT,
    @ParentTabID      int OUTPUT,
    @MobileTabName nvarchar (50)  OUTPUT,
    @AuthRoles     nvarchar (256) OUTPUT,
    @ShowMobile    bit OUTPUT
)
AS
/* First, get Out Params */
IF @TabID = 0 
    SELECT TOP 1
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabID         = rb_Tabs.TabID,
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @TabName       = rb_Tabs.TabName,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
        
    WHERE
        PortalAlias=@PortalAlias
        
    ORDER BY
        TabOrder
ELSE 
    SELECT
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabName       = rb_Tabs.TabName,
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
        
    WHERE
        TabID=@TabID AND rb_Portals.PortalAlias=@PortalAlias

/* Get Tabs list */
SELECT  
    TabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    TabOrder    
FROM    
    rb_Tabs
    
WHERE   
    PortalID = @PortalID
    
ORDER BY
    TabOrder
    
/* Get Mobile Tabs list */
SELECT  
    MobileTabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    ShowMobile  
FROM    
    rb_Tabs
    
WHERE   
    PortalID = @PortalID
  AND
    ShowMobile = 1
    
ORDER BY
    TabOrder
/* Then, get the DataTable of module info */
SELECT     *
FROM         rb_Modules INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.TabID = @TabID OR ShowEveryWhere = 1)
ORDER BY rb_Modules.ModuleOrder
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
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
    @SupportWorkflow	    bit,
    @ApprovalRoles			nvarchar(256),
    @ShowEveryWhere         bit,
    @SupportCollapsable     bit
)
AS
UPDATE
    rb_Modules
SET
   TabID			  = @TabID,
    ModuleOrder               = @ModuleOrder,
    ModuleTitle               = @ModuleTitle,
    PaneName                  = @PaneName,
    CacheTime                 = @CacheTime,
    ShowMobile                = @ShowMobile,
    AuthorizedEditRoles       = @EditRoles,
    AuthorizedAddRoles        = @AddRoles,
    AuthorizedViewRoles       = @ViewRoles,
    AuthorizedDeleteRoles     = @DeleteRoles,
    AuthorizedPropertiesRoles = @PropertiesRoles,
    AuthorizedPublishingRoles = @PublishingRoles,
    SupportWorkflow	          = @SupportWorkflow,
    AuthorizedApproveRoles    = @ApprovalRoles,
    SupportCollapsable		= @SupportCollapsable,
    ShowEveryWhere              = @ShowEveryWhere

WHERE
    ModuleID = @ModuleID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE    PROCEDURE rb_AddModule
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

---------------------
-- Insert information for localization (English)
---------------------
INSERT INTO rb_localize  (TextKey,CultureCode,Description)
VALUES ('SHOWEVERYWHERE','en','Show on every page?')
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1714','1.2.8.1714', CONVERT(datetime, '05/24/2003', 101))
GO


