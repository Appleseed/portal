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

--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1739','1.2.8.1739', CONVERT(datetime, '02/10/2003', 101))
--GO

---------------------
-- Install script, updated Events
-----------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{EF9B29C5-E481-49A6-9383-8ED3AB42DDA0}'
SET @FriendlyName = 'Events'
SET @DesktopSrc = 'DesktopModules/Events/Events.ascx'
SET @MobileSrc = 'MobileModules/Events.aspx'
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesEvents'
SET @Admin = 0
SET @Searchable = 1

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1739','1.2.8.1739', CONVERT(datetime, '02/10/2003', 101))
GO