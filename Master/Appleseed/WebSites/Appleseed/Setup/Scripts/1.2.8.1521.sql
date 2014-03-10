---------------------
--1.2.8.1521.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1521','1.2.8.1521', CONVERT(datetime, '03/21/2003', 101))
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCurrentDbVersion]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentDbVersion]
GO

CREATE PROCEDURE [rb_GetCurrentDbVersion]
AS
SELECT     TOP 1 Release
FROM         rb_Versions
ORDER BY Release DESC
GO

--Fixes the fact that you can't delete a tab 
--even though you've removed all the modules 
--from it. (Makes the tabsettings for that tab 
--cascade delete when the tab is deleted.
--John Bowen

BEGIN TRANSACTION 
ALTER TABLE TabSettings 
DROP CONSTRAINT FK_TabSettings_Tabs 

ALTER TABLE TabSettings WITH NOCHECK ADD CONSTRAINT 
FK_TabSettings_Tabs FOREIGN KEY 
( 
TabID 
) REFERENCES Tabs 
( 
TabID 
) ON DELETE CASCADE 
COMMIT
GO

