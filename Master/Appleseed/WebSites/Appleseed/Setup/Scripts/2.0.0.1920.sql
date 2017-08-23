/* CREATE PAGE FOR Page DASHBOARD if not exist */
DECLARE @PAGEID INT,
		@ModuleDefId INT,
		@ModuleID INT
if not exists(select * from rb_pages where PageName='Dashboard')
BEGIN
	EXEC rb_AddTab 0,NULL,'Dashboard',100,'All Users;',1,'Dashboard', @PAGEID output
END
ELSE
BEGIN
	select @PAGEID = PageId from rb_pages where PageName='Dashboard'
END

/* Add XML Feeds Module on new created page if not loaded */
select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='2502DB18-B580-4F90-8CB4-C15E6E531020'
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId and TabId = @PAGEID)
BEGIN
	EXEC rb_addModule @PAGEID,1,'News','ContentPane',@ModuleDefId,0,'All Users;','All Users;','All Users;','All Users;','All Users;','All Users;','All Users;',0,NULL,0,0,0,@ModuleID output

	EXEC [rb_UpdateModuleSetting] @ModuleID, 'XML URL', 'http://www.zdnet.com/news/rss.xml'
END
GO
