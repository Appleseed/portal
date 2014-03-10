---------------------
--1.2.8.1736.sql  
---------------------

---------------------
-- (moving Announcements AND Articles)
---------------------

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{CE55A821-2449-4903-BA1A-EC16DB93F8DB}'
SET @FriendlyName = 'Announcements'
SET @DesktopSrc = 'DesktopModules/Announcements/Announcements.ascx'
SET @MobileSrc = 'MobileModules/Announcements.ascx'
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesAnnouncements'
SET @Admin = 0
SET @Searchable = 1

-- update module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

-----------------------
---- (moving Articles)
-----------------------
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{87303CF7-76D0-49B1-A7E7-A5C8E26415BA}'
--SET @FriendlyName = 'Articles'
--SET @DesktopSrc = 'DesktopModules/Articles/Articles.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesArticles'
--SET @Admin = 0
--SET @Searchable = 1

---- update module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
--GO


--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1736','1.2.8.1736', CONVERT(datetime, '09/11/2003', 101))
--GO