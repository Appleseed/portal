
declare @pageid int
select @pageid = pageid from rb_Pages where pagename = 'Users'

update  rb_Modules set ModuleOrder = 2 where tabId = @pageid and ModuleTitle = 'Manage Users'
update  rb_Modules set ModuleOrder = 1 where tabId = @pageid and ModuleTitle = 'Manage Roles'
update rb_Pages set PageName = 'Manage Roles & Users' where pageid = @pageid