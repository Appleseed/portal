/* Install script, updated Discussion */

DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{2D86166C-4BDC-4A6F-A028-D17C2BB177C8}'
SET @FriendlyName = 'Discussion'
SET @DesktopSrc = 'DesktopModules/Discussion/Discussion.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = 'Appleseed.Content.Web.ModulesDiscussion'
SET @Admin = 0
SET @Searchable = 1

EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO


INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1738','1.2.8.1738', CONVERT(datetime, '09/26/2003', 101))
GO