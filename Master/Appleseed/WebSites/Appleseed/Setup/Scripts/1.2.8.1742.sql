---------------------
-- Install script, updated SignIn module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{A0F1F62B-FDC7-4de5-BBAD-A5DAF31D960A}'
SET @FriendlyName = 'SignIn'
SET @DesktopSrc = 'DesktopModules/SignIn/SignIn.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesSignIn'
SET @Admin = 0
SET @Searchable = 0
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated SignInCool module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{99F3511F-737C-4b57-87C0-9A010AF40A9C}'
SET @FriendlyName = 'SignInCool'
SET @DesktopSrc = 'DesktopModules/SignIn/SignInCool.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesSignInCool'
SET @Admin = 0
SET @Searchable = 0
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated SignInLink module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{E2AE1D7E-E2FE-466f-A2F4-EB9465BC8966}'
SET @FriendlyName = 'SignInLink'
SET @DesktopSrc = 'DesktopModules/SignIn/SignInLink.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesSignInLink'
SET @Admin = 0
SET @Searchable = 0
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated Shortcut module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2}'
SET @FriendlyName = 'Shortcut'
SET @DesktopSrc = 'DesktopModules/Shortcut/Shortcut.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesShortcut'
SET @Admin = 0
SET @Searchable = 0
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated ShortcutAll module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0}'
SET @FriendlyName = 'ShortcutAll'
SET @DesktopSrc = 'DesktopModules/Shortcut/ShortcutAll.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesShortcutAll'
SET @Admin = 0
SET @Searchable = 0
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated Pictures module 
---------------------
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{B29CB86B-AEA1-4E94-8B77-B4E4239258B0}'
SET @FriendlyName = 'Pictures'
SET @DesktopSrc = 'DesktopModules/Pictures/Pictures.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesPictures'
SET @Admin = 0
SET @Searchable = 1
-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

--------------------------------
---------------------
-- Install script, updated Documents module 
---------------------

--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{F9645B82-CB45-4C4C-BB2D-72FA42FE2B75}'
--SET @FriendlyName = 'Documents'
--SET @DesktopSrc = 'DesktopModules/Documents/Documents.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesDocuments'
--SET @Admin = 0
--SET @Searchable = 1

---- update module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
--GO

---------------------
-- Install script, updated HTML Content module 
---------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{0B113F51-FEA3-499A-98E7-7B83C192FDBB}'
SET @FriendlyName = 'HTML Document'
SET @DesktopSrc = 'DesktopModules/HTMLDocument/HTMLModule.ascx'
SET @MobileSrc = 'MobileModules/Text.ascx'
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesHtmlModule'
SET @Admin = 0
SET @Searchable = 1

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

---------------------
-- Install script, updated PortalSearch module 
-----------------------

--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531030}'
--SET @FriendlyName = 'PortalSearch'
--SET @DesktopSrc = 'DesktopModules/PortalSearch/PortalSearch.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesPortalSearch'
--SET @Admin = 0
--SET @Searchable = 0

---- update module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
--GO
