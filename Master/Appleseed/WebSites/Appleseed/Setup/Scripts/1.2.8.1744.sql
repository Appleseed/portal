IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetTabSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabSettings]
GO

--Fix by Onur Esnaf
CREATE PROCEDURE rb_GetTabSettings
(
    @TabID   int,
    @PortalLanguage nvarchar(12)
)
AS
 
IF (@TabID > 0)
 
/* Get Tabs list */
SELECT     
   COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName)  AS 
 TabName, 
 AuthorizedRoles, 
 TabID, 
 TabOrder, 
 ParentTabID, 
 MobileTabName, 
 ShowMobile, 
 PortalID
FROM         rb_Tabs
WHERE     (ParentTabID = @TabID)
ORDER BY TabOrder
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettings]
GO

--Manu - Created 25/06/2003
--Manu fixed tab name on active tab for localized items - 13/10/2203
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
        @TabName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName)  ,
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
        @TabName       = COALESCE (
   (SELECT SettingValue
    FROM   rb_TabSettings
    WHERE  TabID = rb_Tabs.TabID 
       AND SettingName = @PortalLanguage 
       AND Len(SettingValue) > 0), 
   TabName),
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
                              WHERE     TabID = rb_Tabs.TabID AND SettingName = @PortalLanguage AND Len(SettingValue) > 0), TabName) AS TabName, AuthorizedRoles, TabID, ParentTabID, TabOrder, 
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
