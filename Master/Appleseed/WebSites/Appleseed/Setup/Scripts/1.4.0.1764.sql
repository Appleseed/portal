ALTER TABLE [rb_ModuleSettings] 
	ALTER COLUMN SettingValue nvarchar(1500) NOT NULL
GO

ALTER TABLE [rb_PortalSettings] 
	ALTER COLUMN SettingValue nvarchar(1500) NOT NULL
GO

ALTER TABLE [rb_TabSettings] 
	ALTER COLUMN SettingValue nvarchar(1500) NOT NULL
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateModuleSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateModuleSetting]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdatePortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdatePortalSetting]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateTabCustomSettings]
GO


CREATE PROCEDURE rb_UpdateModuleSetting
(
    @ModuleID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_ModuleSettings 
    WHERE 
        ModuleID = @ModuleID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_ModuleSettings (
    ModuleID,
    SettingName,
    SettingValue
) 
VALUES (
    @ModuleID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_ModuleSettings
SET
    SettingValue = @SettingValue
WHERE
    ModuleID = @ModuleID
  AND
    SettingName = @SettingName

GO

CREATE PROCEDURE rb_UpdatePortalSetting
(
    @PortalID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_PortalSettings 
    WHERE 
        PortalID = @PortalID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_PortalSettings (
    PortalID,
    SettingName,
    SettingValue
) 
VALUES (
    @PortalID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_PortalSettings
SET
    SettingValue = @SettingValue
WHERE
    PortalID = @PortalID
  AND
    SettingName = @SettingName

GO
 
CREATE PROCEDURE rb_UpdateTabCustomSettings
(
    @TabID int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_TabSettings 
    WHERE 
        TabID = @TabID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_TabSettings (
    TabID,
    SettingName,
    SettingValue
) 
VALUES (
    @TabID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_TabSettings
SET
    SettingValue = @SettingValue
WHERE
    TabID = @TabID
  AND
    SettingName = @SettingName

GO
