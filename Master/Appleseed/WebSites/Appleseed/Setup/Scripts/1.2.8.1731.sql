/* Update script Ecommerce - 1731 fixes, by Manu [manu-dea@hotmail dot it] */

SET NOCOUNT on
GO

UPDATE rb_PortalSettings SET SettingName = 'SITESETTINGS_LANGLIST' WHERE SettingName = 'LangList'
GO

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
	(rb_Portals.PortalAlias = @PortalAlias) AND (rb_PortalSettings.SettingName = N'SITESETTINGS_LANGLIST')
GO

-- Update Language switcher path
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{25E3290E-3B9A-4302-9384-9CA01243C00F}'
SET @FriendlyName = 'Language Switcher'
SET @DesktopSrc = 'DesktopModules/LanguageSwitcher/LanguageSwitcher.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesLanguageSwitcher'
SET @Admin = 0
SET @Searchable = 0

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

SET NOCOUNT off
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1731','1.2.8.1731', CONVERT(datetime, '07/28/2003', 101))
GO

--DELETE FROM [rb_Versions] WHERE [Version] = '1.2.8.1731'

