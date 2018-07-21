if exists (select * from dbo.sysobjects where id = object_id(N'[rb_ModuleUserSettings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [rb_ModuleUserSettings]
GO

CREATE TABLE [rb_ModuleUserSettings] (
	[ModuleID] [int] NOT NULL ,
	[UserID] [int] NOT NULL ,
	[SettingName] [nvarchar] (50) NOT NULL ,
	[SettingValue] [nvarchar] (1500) NOT NULL 
) ON [PRIMARY]
GO



if exists (select * from dbo.sysobjects where id = object_id(N'[rb_GetModuleUserSettings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetModuleUserSettings]
GO

CREATE  PROCEDURE rb_GetModuleUserSettings
(
    @ModuleID int,
    @UserID int
)
AS
SELECT     SettingName, SettingValue
FROM         rb_ModuleUserSettings
WHERE     (ModuleID = @ModuleID AND UserID = @UserID)
GO

IF NOT EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'rb_UpdateModuleUserSetting')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
	drop PROCEDURE rb_UpdateModuleUserSetting
END
GO

CREATE  PROCEDURE rb_UpdateModuleUserSetting
(
    @ModuleID      int,
    @UserID 	   int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(1500)
)
AS
BEGIN
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_ModuleUserSettings 
    WHERE 
        ModuleID = @ModuleID
      AND
	UserID = @UserID
      AND
        SettingName = @SettingName
)
INSERT INTO rb_ModuleUserSettings (
    ModuleID,
    UserID,
    SettingName,
    SettingValue
) 
VALUES (
    @ModuleID,
    @UserID,
    @SettingName,
    @SettingValue
)
ELSE
UPDATE
    rb_ModuleUserSettings
SET
    SettingValue = @SettingValue
WHERE
    ModuleID = @ModuleID
  AND
    UserID = @UserID
  AND
    SettingName = @SettingName

END
GO

