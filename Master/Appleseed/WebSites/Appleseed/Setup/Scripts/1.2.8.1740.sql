---------------------
-- Install script, updated Contacts 
---------------------

--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E5339EF}'
--SET @FriendlyName = 'Contacts'
--SET @DesktopSrc = 'DesktopModules/Contacts/Contacts.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesContacts'
--SET @Admin = 0
--SET @Searchable = 1

---- update module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
--GO

---------------------
-- Install script, updated BlackList
---------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531017}'
SET @FriendlyName = 'BlackList (Admin)'
SET @DesktopSrc = 'DesktopModules/NewsLetter/BlackList.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesBlackList'
SET @Admin = 1
SET @Searchable = 0

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated NewsLetter
---------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{B484D450-5D30-4C4B-817C-14A25D06577E}'
SET @FriendlyName = 'NewsLetter (Admin)'
SET @DesktopSrc = 'DesktopModules/NewsLetter/NewsLetter.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesNewsLetter'
SET @Admin = 0
SET @Searchable = 0

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1740','1.2.8.1740', CONVERT(datetime, '02/10/2003', 101))
GO