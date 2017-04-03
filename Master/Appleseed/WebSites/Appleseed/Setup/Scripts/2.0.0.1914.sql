-- Update Monitoring page set module

delete from [rb_Modules] where tabid = 170

DECLARE @PAGEID INT,
		@ModuleDefId INT,
		@ModuleID INT

Set @PAGEID = 170
select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='B484D450-5D30-4C4B-817C-14A25D06577E'

/* Add Monitoring page Module on new created page if not loaded */
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId and tabid = @pageid)
BEGIN
EXEC rb_addModule @PAGEID,1,'Monitoring','ContentPane',@ModuleDefId,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
END

select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='0B113F51-FEA3-499A-98E7-7B83C192FDBB'

/* Add Monitoring page Module on new created page if not loaded */
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId and tabid = @pageid)
BEGIN
EXEC rb_addModule @PAGEID,1,'Error Logs','ContentPane',@ModuleDefId,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
END
select @ModuleDefId=ModuleDefID from rb_ModuleDefinitions where GeneralModDefID='2502DB18-B580-4F90-8CB4-C15E6E531051'

/* Add Monitoring page Module on new created page if not loaded */
IF NOT EXISTS(SELECT * FROM rb_Modules WHERE ModuleDefID=@ModuleDefId and tabid = @pageid)
BEGIN
EXEC rb_addModule @PAGEID,1,'Event Logs','ContentPane',@ModuleDefId,0,'Admins','Admins;','Admins;','Admins;','Admins;','Admins;','Admins;',0,NULL,0,0,0,@ModuleID output
END