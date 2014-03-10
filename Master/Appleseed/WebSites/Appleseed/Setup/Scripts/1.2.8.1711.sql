---------------------
--1.2.8.1711.sql
---------------------


-- Add new module: EventLogs
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531051}'
SET @FriendlyName = 'EventLogs'
SET @DesktopSrc = 'DesktopModules/EventLogs/EventLogs.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesEventLogs'
SET @Admin = 0
SET @Searchable = 0

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

-- Install it for default portal
EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
GO


/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1711','1.2.8.1711', CONVERT(datetime, '05/01/2003', 101))
GO
