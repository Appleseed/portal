---------------------
--1.2.8.1622.sql
---------------------


-- Add new module: Database Table Edit
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{AB02A3F4-A0A4-45e0-96ED-8450C19166C5}'
SET @FriendlyName = 'Database Table Edit'
SET @DesktopSrc = 'DesktopModules/DatabaseTableEdit.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesDatabaseTableEdit'
SET @Admin = 0
SET @Searchable = 0

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

-- Install it for default portal
EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1622','1.2.8.1622', CONVERT(datetime, '04/22/2003', 101))
GO
