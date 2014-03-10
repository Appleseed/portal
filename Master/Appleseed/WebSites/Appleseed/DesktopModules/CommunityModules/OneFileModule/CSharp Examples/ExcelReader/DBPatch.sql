/*
OneFileModule module : ExcelReader by Thierry (Tiptopweb)
Updated 13. sep 2005

This patch introduces the following changes to the db:
- Inserts entry in table rb_GeneralModuleDefinitions
- Inserts entry in table rb_ModuleDefinitions

Deinstalling: First remove the module from all pages then run this script (in module Database Tool):
DELETE FROM rb_GeneralModuleDefinitions WHERE FriendlyName = 'ExcelReader (OneFileModule)'
*/


USE [Appleseed]
GO

-- If you install using module "Admin - Database Tool" then copy all lines from below:
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = NEWID()   -- Or if you want full control: = '{1E03D793-5A01-4f35-811D-5D23EF66618D}'
SET @FriendlyName = 'ExcelReader (OneFileModule)'    -- You enter the module UI name here
SET @DesktopSrc = 'DesktopModules/ExcelReader.ascx'  -- You enter actual filename here
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.Modules.OneFileModule.dll'
SET @ClassName = 'Appleseed.Content.Web.ModulesOneFileModule'
SET @Admin = 0
SET @Searchable = 0

IF NOT EXISTS (SELECT DesktopSrc FROM rb_GeneralModuleDefinitions WHERE DesktopSrc = @DesktopSrc)
BEGIN
	-- Installs module
	EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

	-- Install it for default portal
	EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
END
