---------------------
--1.2.8.1720.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettings]
GO

--Manu - Created 20/06/2003

CREATE PROCEDURE rb_GetPortalSettings
(
    @PortalAlias   nvarchar(50),
    @TabID         int,
    @PortalLanguage nvarchar(12),
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
SELECT     COALESCE ((SELECT     SettingValue
                              FROM         rb_TabSettings
                              WHERE     TabID = rb_Tabs.TabID AND SettingName = @PortalLanguage), TabName) AS TabName, AuthorizedRoles, TabID, ParentTabID, TabOrder, 
                      TabLayout
FROM         rb_Tabs
WHERE     (PortalID = @PortalID)
ORDER BY TabOrder
    
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
    PortalID = @PortalID  AND  ShowMobile = 1
ORDER BY
    TabOrder

/* Then, get the DataTable of module info */
SELECT     *
FROM         rb_Modules INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.TabID = @TabID) OR
                      (rb_Modules.ShowEveryWhere = 1) AND (rb_ModuleDefinitions.PortalID = @PortalID)
ORDER BY rb_Modules.ModuleOrder
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1720','1.2.8.1720', CONVERT(datetime, '06/20/2003', 101))
GO