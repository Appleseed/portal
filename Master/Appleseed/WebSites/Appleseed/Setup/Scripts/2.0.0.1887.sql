IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalSettingsLangList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettingsLangList]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetPortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSetting]
GO

CREATE PROCEDURE [dbo].[rb_GetPortalSetting]
(
    @PortalAlias   nvarchar(50),
    @SettingName   nvarchar(50)
)
AS
SELECT
	rb_PortalSettings.SettingValue
FROM
	rb_PortalSettings 
		INNER JOIN
	rb_Portals ON rb_PortalSettings.PortalID = rb_Portals.PortalID
WHERE
	(rb_Portals.PortalAlias = @PortalAlias) AND (rb_PortalSettings.SettingName = @SettingName)
	
