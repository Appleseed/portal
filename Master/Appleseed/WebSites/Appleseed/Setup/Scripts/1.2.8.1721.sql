---------------------
--1.2.8.1721.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetPortalSettingsLangList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetPortalSettingsLangList]
GO

CREATE PROCEDURE rb_GetPortalSettingsLangList
(
    @PortalAlias   nvarchar(50)
)
AS

SELECT
	rb_PortalSettings.SettingValue
FROM
	rb_PortalSettings 
		INNER JOIN
	rb_Portals ON rb_PortalSettings.PortalID = rb_Portals.PortalID
WHERE
	(rb_Portals.PortalAlias = @PortalAlias) AND (rb_PortalSettings.SettingName = N'LangList')
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1721','1.2.8.1721', CONVERT(datetime, '06/25/2003', 101))
GO