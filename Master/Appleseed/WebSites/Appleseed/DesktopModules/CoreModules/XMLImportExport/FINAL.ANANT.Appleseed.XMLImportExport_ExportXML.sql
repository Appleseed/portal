/*Get page detail by id*/
CREATE PROCEDURE [dbo].[rb_XMLExportData]
(
	@PageID INT,
	@PortalLanguage nvarchar(12)
)
AS
BEGIN
	/*get Page Data*/
	SELECT	PageID, ParentPageID, PageOrder,PortalID, PageName, MobilePageName,
			AuthorizedRoles, ShowMobile, PageLayout, PageDescription 
		FROM rb_pages 
	WHERE pageid=@PageID

	/*Get page tab settings*/
	SELECT * FROM [dbo].[rb_TabSettings] 
	WHERE TabID=@PageID

	/*Get Module list render on page as well as module defination id */
	SELECT * FROM [dbo].[rb_Modules] 
			INNER JOIN [dbo].[rb_ModuleDefinitions] ON rb_modules.ModuleDefID = rb_moduleDefinitions.ModuleDefID
			INNER JOIN [dbo].[rb_GeneralModuleDefinitions] ON rb_GeneralModuleDefinitions.GeneralModDefID=rb_moduleDefinitions.GeneralModDefID
	where rb_Modules.TabID=@PageID


	/*Get Module Settings*/
	SELECT	SettingName, SettingValue
		FROM	rb_ModuleSettings
		INNER JOIN rb_Modules on rb_Modules.ModuleID = rb_ModuleSettings.ModuleID
	WHERE	rb_Modules.TabID=@PageID

END
GO
