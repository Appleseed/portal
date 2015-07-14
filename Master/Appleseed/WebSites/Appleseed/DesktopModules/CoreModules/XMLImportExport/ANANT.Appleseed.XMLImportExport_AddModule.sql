/*Create XMLImportExport module*/
GO 

EXEC	[dbo].[rb_AddGeneralModuleDefinitions]
		@GeneralModDefID = '6D0CA478-CEA8-4C32-B6E3-B8B5F9B64D4E',
		@FriendlyName = N'XMLImportExport',
		@DesktopSrc = N'/DesktopModules/CoreModules/XMLImportExport/XmlImportExport.ascx',
		@MobileSrc = N'',
		@AssemblyName = N'Applesseed.Dll',
		@ClassName = N'Appleseed.DesktopModules.CoreModules.XMLImportExport.XmlImportExport',
		@Admin = 1,
		@Searchable = 1

GO

insert into [dbo].[rb_ModuleDefinitions] (PortalID, GeneralModDefID) values (0,'6D0CA478-CEA8-4C32-B6E3-B8B5F9B64D4E');
GO

/*Get page detail by id*/
CREATE PROCEDURE [dbo].[rb_GetPageByID]
(
@PageID INT
)
AS
SELECT * FROM rb_pages 
WHERE pageid=@PageID
GO
