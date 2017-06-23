declare @pageid int
select @pageid = pageId from [dbo].[rb_Pages] where PageName = 'LANGUAGE SWITCHER' and ParentPageID = 100
if isnull(@pageid,0) > 0
BEGIN
    delete from [dbo].[rb_ModuleSettings] where ModuleID in (select ModuleId from [dbo].[rb_Modules] where tabId = @pageid)
    delete from [dbo].[rb_Modules] where tabId = @pageid
    delete from [dbo].[rb_TabSettings] where tabId = @pageid
    delete from [dbo].[rb_Pages] where PageId = @pageId
END

SET @pageid = 0
select @pageid = pageId from [dbo].[rb_Pages] where PageName = 'CONTENT MANAGER' and ParentPageID = 100
if isnull(@pageid,0) > 0
BEGIN
    delete from [dbo].[rb_ModuleSettings] where ModuleID in (select ModuleId from [dbo].[rb_Modules] where tabId = @pageid)
    delete from [dbo].[rb_Modules] where tabId = @pageid
    delete from [dbo].[rb_TabSettings] where tabId = @pageid
    delete from [dbo].[rb_Pages] where PageId = @pageId
END

SET @pageid = 0
select @pageid = pageId from [dbo].[rb_Pages] where PageName = 'Tasks' and ParentPageID = 100
if isnull(@pageid,0) > 0
BEGIN
    delete from [dbo].[rb_ModuleSettings] where ModuleID in (select ModuleId from [dbo].[rb_Modules] where tabId = @pageid)
    delete from [dbo].[rb_Modules] where tabId = @pageid
    delete from [dbo].[rb_TabSettings] where tabId = @pageid
    delete from [dbo].[rb_Pages] where PageId = @pageId
END

SET @pageid = 0
select @pageid = pageId from [dbo].[rb_Pages] where PageName = 'Version' and ParentPageID = 100
if isnull(@pageid,0) > 0
BEGIN
    delete from [dbo].[rb_ModuleSettings] where ModuleID in (select ModuleId from [dbo].[rb_Modules] where tabId = @pageid)
    delete from [dbo].[rb_Modules] where tabId = @pageid
    delete from [dbo].[rb_TabSettings] where tabId = @pageid
    delete from [dbo].[rb_Pages] where PageId = @pageId
END