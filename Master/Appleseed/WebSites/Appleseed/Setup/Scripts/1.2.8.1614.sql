---------------------
--1.2.8.1614.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModulesUpgradeOldToNew]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModulesUpgradeOldToNew]
GO

/* 
This procedure replaces all instances of the old module with the new one
Old module entires will be removed.
Module data are not affected.
by Manu 22/03/2002
 */

CREATE PROCEDURE rb_ModulesUpgradeOldToNew
(
	@OldModuleGuid uniqueidentifier,
	@NewModuleGuid uniqueidentifier
)

AS

--Get current ids
DECLARE @NewModuleDefID int
SET @NewModuleDefID = (SELECT TOP 1 ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @NewModuleGuid)
DECLARE @OldModuleDefID int
SET @OldModuleDefID = (SELECT TOP 1 ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @OldModuleGuid)

--Update modules
UPDATE Modules
SET ModuleDefID = @NewModuleDefID WHERE ModuleDefID = @OldModuleDefID

-- Drop old definition
DELETE ModuleDefinitions
WHERE ModuleDefID = @OldModuleDefID

DELETE GeneralModuleDefinitions
WHERE GeneralModDefID = @OldModuleGuid
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ContactsModified_st]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_ContactsModified_st]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Contacts_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Contacts_stModified]
GO

CREATE  TRIGGER [rb_Contacts_stModified]
ON [rb_Contacts_st]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
	DECLARE ChangedModules CURSOR FOR
		SELECT ModuleID
		FROM inserted
		UNION
		SELECT ModuleID
		FROM deleted

	DECLARE @ModID	int

	OPEN ChangedModules	

	FETCH NEXT FROM ChangedModules
	INTO @ModID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC rb_ModuleEdited @ModID

		FETCH NEXT FROM ChangedModules
		INTO @ModID
	END

	CLOSE ChangedModules
	DEALLOCATE ChangedModules

END
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1614','1.2.8.1614', CONVERT(datetime, '04/17/2003', 101))
GO

