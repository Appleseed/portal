---------------------
--1.2.8.1716.sql
---------------------


 --Add new module: BreadCrumbs as a module
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{D3182CD6-DAFF-4E72-AD9E-0B28CB44F007}'
SET @FriendlyName = 'BreadCrumbs'
SET @DesktopSrc = 'DesktopModules/BreadCrumbs/BreadCrumbs.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesBreadCrumbs'
SET @Admin = 0
SET @Searchable = 0

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

-- Install it for default portal
EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1

GO
----
---- Add new module: SimpleMenu as a module
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{D3182CD6-DAFF-4E72-AD9E-0B28CB44F006}'
--SET @FriendlyName = 'Simple Menu'
--SET @DesktopSrc = 'DesktopModules/SimpleMenu/SimpleMenu.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesSimpleMenu'
--SET @Admin = 0
--SET @Searchable = 0

---- Installs module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

---- Install it for default portal
--EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1



------ Fix path for module MileStones (was moved into own folder)
----UPDATE rb_GeneralModuleDefinitions SET DesktopSrc = 'DesktopModules/MileStones/MileStones.ascx' 
----WHERE GeneralModDefID = '{B8784E32-688A-4b8a-87C4-DF108BF12DBE}'
----GO


--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1716','1.2.8.1716', CONVERT(datetime, '05/27/2003', 101))
--GO
