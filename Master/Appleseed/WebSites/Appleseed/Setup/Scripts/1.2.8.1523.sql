---------------------
--1.2.8.1523.sql
---------------------

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1523','1.2.8.1523', CONVERT(datetime, '03/23/2003', 101))
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

ALTER PROCEDURE GetTabsParent
(
	@PortalID int,
	@TabID int
)
AS

--Create a Temporary table to hold the tabs for this query
CREATE TABLE #TabTree
(
        [TabID] [int],
        [TabName] [nvarchar] (50),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)

SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0

-- First, the parent levels
INSERT INTO     #TabTree
SELECT  TabID,
        TabName,
        ParentTabID,
        TabOrder,
        0,
        cast(100000000 + TabOrder as varchar)

FROM    Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  Tabs.TabID,
                        Replicate('-', @LastLevel *2) + Tabs.TabName,
                        Tabs.ParentTabID,
                        Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + Tabs.TabOrder as varchar)
                FROM    Tabs join #TabTree on Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END

--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  Tabs.TabID,
                        '(Orphan)' + Tabs.TabName,
                        Tabs.ParentTabID,
                        Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = Tabs.TabID)
                         AND PortalID =@PortalID

-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) as ord , cast(TabID as varchar) as TabID into #tabs
from #TabTree
order by nestlevel, TreeOrder

-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID as int)=#TabTree.TabID) 

-- Return Temporary Table


SELECT TabID, tabname, TreeOrder
FROM #TabTree 

UNION

SELECT 0 TabID, ' ROOT_LEVEL' TabName, '-1' as TreeOrder

order by TreeOrder
GO


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

--Upgrades old search to new
--exec rb_ModulesUpgradeOldToNew '{2502DB18-B580-4F90-8CB4-C15E6E531010}', '{2502DB18-B580-4F90-8CB4-C15E6E531030}'
--GO

--Upgrades old html to new
exec rb_ModulesUpgradeOldToNew '{2B113F51-FEA3-499A-98E7-7B83C192FDBB}', '{0B113F51-FEA3-499A-98E7-7B83C192FDBB}'
GO

ALTER PROCEDURE GetCountries
(
	@IDLang	nchar(2) = 'en'
)

AS

IF 
(
SELECT     COUNT(Countries.CountryID) AS CountryListCount
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)
) > 0

BEGIN
-- Country returns results
SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = @IDLang) OR
                      (Cultures.NeutralCode = @IDLang)
ORDER BY Localize.Description
END

else

BEGIN
-- Get English list
SELECT     Countries.CountryID, Localize.Description
FROM         Cultures INNER JOIN
                      Localize ON Cultures.CultureCode = Localize.CultureCode INNER JOIN
                      Countries ON Localize.TextKey = 'COUNTRY_' + Countries.CountryID
WHERE     (Localize.CultureCode = 'en') OR
                      (Cultures.NeutralCode = 'en')
ORDER BY Localize.Description
END
GO

