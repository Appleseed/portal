---------------------
-- Install script, updated Appleseed Version 
---------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{72C6F60A-50C4-4F20-8F89-3E8A27820557}'
SET @FriendlyName = 'Appleseed Version'
SET @DesktopSrc = 'DesktopModules/Version/AppleseedVersion.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesAppleseedVersion'
SET @Admin = 0
SET @Searchable = 0

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated XmlModule
-----------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{BE224332-03DE-42B7-B127-AE1F1BD0FADC}'
SET @FriendlyName = 'XML/XSL'
SET @DesktopSrc = 'DesktopModules/XmlModule/XmlModule.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesXmlModule'
SET @Admin = 0
SET @Searchable = 0

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated XmlLangModule
---------------------

--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{E16DD121-267E-4268-A497-BDA6314E21A5}'
--SET @FriendlyName = 'XML/XSL Lang'
--SET @DesktopSrc = 'DesktopModules/XmlLang/XmlLangModule.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesXmlLangModule'
--SET @Admin = 0
--SET @Searchable = 0

---- update module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
--GO
