---------------------
--1.1.7.1115_01.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1115','1.2.8.1115', CONVERT(datetime, '11/15/2002', 101))
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

-----------------------------------------------------------
--Insert data into GeneralModuleDefinitions
-----------------------------------------------------------
IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions ON
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{1C575D94-70FC-4A83-80C3-2087F726CBB3}',NULL,'Tabs (Admin)','Admin/Tabs.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{476CF1CC-8364-479D-9764-4B3ABD7FFABD}',NULL,'Links','DesktopModules/Links.ascx','MobileModules/Links.ascx',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{A406A674-76EB-4BC1-BB35-50CD2C251F9C}',NULL,'Roles (Admin)','Admin/Roles.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{EBBB01B1-FBB5-4E79-8FC4-59BCA1D0554E}',NULL,'Site Settings (Admin)','Admin/SiteSettings.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{B6A48596-9047-4564-8555-61E3B31D7272}',NULL,'Manage Users (Admin)','Admin/Users.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{366C247D-4CFB-451D-A7AE-649C83B05841}',NULL,'Manage Portals (AdminAll)','AdminAll/Portals.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}',NULL,'Module Definitions (Admin)','Admin/ModuleDefs.ascx','',1)
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{F9645B82-CB45-4C4C-BB2D-72FA42FE2B75}',NULL,'Documents','DesktopModules/Document.ascx','',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{0B113F51-FEA3-499A-98E7-7B83C192FDBB}',NULL,'Html Document','DesktopModules/HtmlModule.ascx','MobileModules/Text.ascx',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{D04BB5EA-A792-4E87-BFC7-7D0ED3ADD582}',NULL,'Module Types (AdminAll)','AdminAll/ModuleDefsAll.ascx','',1)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{EF9B29C5-E481-49A6-9383-8ED3AB42DDA0}',NULL,'Events','DesktopModules/Events.ascx','MobileModules/Events.ascx',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD0}',NULL,'ShortcutAll','DesktopModules/ShortcutAll.ascx','',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD2}',NULL,'Shortcut','DesktopModules/Shortcut.ascx','',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{BE224332-03DE-42B7-B127-AE1F1BD0FADC}',NULL,'XML/XSL','DesktopModules/XmlModule.ascx','',0)
--INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{2502DB18-B580-4F90-8CB4-C15E6E5339EF}',NULL,'Contacts','DesktopModules/Contacts.ascx','MobileModules/Contacts.ascx',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{2D86166C-4BDC-4A6F-A028-D17C2BB177C8}',NULL,'Discussion','DesktopModules/Discussion/Discussion.ascx','',0)
INSERT INTO GeneralModuleDefinitions (GeneralModDefID,ClassName,FriendlyName,DesktopSrc,MobileSrc,Admin) VALUES('{CE55A821-2449-4903-BA1A-EC16DB93F8DB}',NULL,'Announcements','DesktopModules/Announcements.ascx','MobileModules/Announcements.ascx',0)
IF(	IDENT_INCR( 'GeneralModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('GeneralModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT GeneralModuleDefinitions OFF
GO

-----------------------------------------------------------
--Insert data into ModuleDefinitions
-----------------------------------------------------------
IF(	IDENT_INCR( 'ModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('ModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT ModuleDefinitions ON
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('1','0','{CE55A821-2449-4903-BA1A-EC16DB93F8DB}')
--INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('2','0','{2502DB18-B580-4F90-8CB4-C15E6E5339EF}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('3','0','{2D86166C-4BDC-4A6F-A028-D17C2BB177C8}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('4','0','{EF9B29C5-E481-49A6-9383-8ED3AB42DDA0}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('5','0','{0B113F51-FEA3-499A-98E7-7B83C192FDBB}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('847','0','{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD2}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('7','0','{476CF1CC-8364-479D-9764-4B3ABD7FFABD}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('861','0','{F9F9C3A4-6E16-43B4-B540-984DDB5F1CD0}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('9','0','{BE224332-03DE-42B7-B127-AE1F1BD0FADC}')
--INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('10','0','{F9645B82-CB45-4C4C-BB2D-72FA42FE2B75}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('11','0','{D04BB5EA-A792-4E87-BFC7-7D0ED3ADD582}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('12','0','{A406A674-76EB-4BC1-BB35-50CD2C251F9C}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('13','0','{1C575D94-70FC-4A83-80C3-2087F726CBB3}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('14','0','{EBBB01B1-FBB5-4E79-8FC4-59BCA1D0554E}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('15','0','{B6A48596-9047-4564-8555-61E3B31D7272}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('21','0','{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}')
INSERT INTO ModuleDefinitions (ModuleDefID,PortalID,GeneralModDefID) VALUES('73','0','{366C247D-4CFB-451D-A7AE-649C83B05841}')
IF(	IDENT_INCR( 'ModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('ModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT ModuleDefinitions OFF
GO

-----------------------------------------------------------
--Insert data into Portals
-----------------------------------------------------------
IF(	IDENT_INCR( 'Portals' ) IS NOT NULL OR IDENT_SEED('Portals') IS NOT NULL ) SET IDENTITY_INSERT Portals ON
INSERT INTO Portals (PortalID,PortalAlias,PortalName,PortalPath,AlwaysShowEditButton) VALUES('-1','unused','Unused Portal',NULL,0)
INSERT INTO Portals (PortalID,PortalAlias,PortalName,PortalPath,AlwaysShowEditButton) VALUES('0','Appleseed','Appleseed Portal','_Appleseed',0)
IF(	IDENT_INCR( 'Portals' ) IS NOT NULL OR IDENT_SEED('Portals') IS NOT NULL ) SET IDENTITY_INSERT Portals OFF
GO

-----------------------------------------------------------
--Insert data into PortalSettings
-----------------------------------------------------------
IF(	IDENT_INCR( 'PortalSettings' ) IS NOT NULL OR IDENT_SEED('PortalSettings') IS NOT NULL ) SET IDENTITY_INSERT PortalSettings ON
INSERT INTO PortalSettings (PortalID,SettingName,SettingValue) VALUES('0','Register','RegisterFull')
INSERT INTO PortalSettings (PortalID,SettingName,SettingValue) VALUES('0','TabLayout','Default')
INSERT INTO PortalSettings (PortalID,SettingName,SettingValue) VALUES('0','Theme','Default')
INSERT INTO PortalSettings (PortalID,SettingName,SettingValue) VALUES('0','ThemeAlt','DefaultAlternate')
IF(	IDENT_INCR( 'PortalSettings' ) IS NOT NULL OR IDENT_SEED('PortalSettings') IS NOT NULL ) SET IDENTITY_INSERT PortalSettings OFF
GO

-----------------------------------------------------------
--Insert data into Tabs
-----------------------------------------------------------
IF(	IDENT_INCR( 'Tabs' ) IS NOT NULL OR IDENT_SEED('Tabs') IS NOT NULL ) SET IDENTITY_INSERT Tabs ON
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('0',NULL,'0','-1','Unused Tab','Unused Tab','All Users;',0,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('1',NULL,'1','0','Home','Home','All Users;',1,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('3039','1','3','0','Documents','','All Users;',0,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('6',NULL,'9','0','Admin this','Admin','Admins;',0,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('11','6','11','0','Admin all','','Admins;',0,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('3325','1','5','0','Documents2','','All Users;',0,NULL)
INSERT INTO Tabs (TabID,ParentTabID,TabOrder,PortalID,TabName,MobileTabName,AuthorizedRoles,ShowMobile,TabLayout) VALUES('3176','3325','7','0','SubMenu','','All Users;',0,NULL)
IF(	IDENT_INCR( 'Tabs' ) IS NOT NULL OR IDENT_SEED('Tabs') IS NOT NULL ) SET IDENTITY_INSERT Tabs OFF
GO

-----------------------------------------------------------
--Insert data into Modules
-----------------------------------------------------------
IF(	IDENT_INCR( 'Modules' ) IS NOT NULL OR IDENT_SEED('Modules') IS NOT NULL ) SET IDENTITY_INSERT Modules ON
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('0','0','1','1','','Unused Module','All Users;','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('10235','1','5','999','ContentPane','Appleseed portal released!','Admins','All Users','Admins','Admins','Admins','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('10120','3039','10','5','contentPane','documents','Admins;','Admins;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('10199','3325','10','3','ContentPane','Documenti','Admins;','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('43','6','21','1','RightPane','Modules','Admins;','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('91','11','73','3','contentPane','Portals','Admins','All Users','Admins','Admins','Admins','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('28','11','11','1','rightPane','General Module Def.','Admins;','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('29','6','14','1','ContentPane','Site Settings','Admins','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('30','6','13','2','ContentPane','Tabs','Admins','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('31','6','12','3','ContentPane','Security Roles','Admins','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('32','6','15','4','ContentPane','Manage Users','Admins','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('10200','3176','7','1','leftPane','Custom links','Admins;','All Users;','Admins;','Admins;','Admins;','0',0)
INSERT INTO Modules (ModuleID,TabID,ModuleDefID,ModuleOrder,PaneName,ModuleTitle,AuthorizedEditRoles,AuthorizedViewRoles,AuthorizedAddRoles,AuthorizedDeleteRoles,AuthorizedPropertiesRoles,CacheTime,ShowMobile) VALUES('10234','3325','7','5','leftPane','Links','Admins','All Users','Admins','Admins','Admins','0',0)
IF(	IDENT_INCR( 'Modules' ) IS NOT NULL OR IDENT_SEED('Modules') IS NOT NULL ) SET IDENTITY_INSERT Modules OFF
GO

-----------------------------------------------------------
--Insert data into ModuleSettings
-----------------------------------------------------------
IF(	IDENT_INCR( 'ModuleSettings' ) IS NOT NULL OR IDENT_SEED('ModuleSettings') IS NOT NULL ) SET IDENTITY_INSERT ModuleSettings ON
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('28','ApplyTheme','True')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('28','Theme','Alt')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('43','ApplyTheme','True')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('43','Theme','Alt')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10234','ApplyTheme','True')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10120','DocumentPath','Documents')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10120','ThemeVariant','Variatio2')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10120','ApplyTheme','True')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10234','Theme','Alt')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10200','ApplyTheme','True')
INSERT INTO ModuleSettings (ModuleID,SettingName,SettingValue) VALUES('10200','Theme','Alt')
IF(	IDENT_INCR( 'ModuleSettings' ) IS NOT NULL OR IDENT_SEED('ModuleSettings') IS NOT NULL ) SET IDENTITY_INSERT ModuleSettings OFF
GO

-----------------------------------------------------------
--Insert data into Discussion
-----------------------------------------------------------
IF(	IDENT_INCR( 'Discussion' ) IS NOT NULL OR IDENT_SEED('Discussion') IS NOT NULL ) SET IDENTITY_INSERT Discussion ON
INSERT INTO Discussion (ItemID,ModuleID,Title,CreatedDate,Body,DisplayOrder,CreatedByUser) VALUES('0','0','',NULL,'','','')
IF(	IDENT_INCR( 'Discussion' ) IS NOT NULL OR IDENT_SEED('Discussion') IS NOT NULL ) SET IDENTITY_INSERT Discussion OFF
GO

-----------------------------------------------------------
--Insert data into Documents
-----------------------------------------------------------
IF(	IDENT_INCR( 'Documents' ) IS NOT NULL OR IDENT_SEED('Documents') IS NOT NULL ) SET IDENTITY_INSERT Documents ON
INSERT INTO Documents (ItemID,ModuleID,CreatedByUser,CreatedDate,FileNameUrl,FileFriendlyName,Category,Content,ContentType,ContentSize) VALUES('0','0',NULL,NULL,NULL,NULL,NULL,'',NULL,NULL)
IF(	IDENT_INCR( 'Documents' ) IS NOT NULL OR IDENT_SEED('Documents') IS NOT NULL ) SET IDENTITY_INSERT Documents OFF
GO

-----------------------------------------------------------
--Insert data into Events
-----------------------------------------------------------
IF(	IDENT_INCR( 'Events' ) IS NOT NULL OR IDENT_SEED('Events') IS NOT NULL ) SET IDENTITY_INSERT Events ON
INSERT INTO Events (ItemID,ModuleID,CreatedByUser,CreatedDate,Title,WhereWhen,Description,ExpireDate) VALUES('0','0',NULL,NULL,NULL,NULL,NULL,NULL)
IF(	IDENT_INCR( 'Events' ) IS NOT NULL OR IDENT_SEED('Events') IS NOT NULL ) SET IDENTITY_INSERT Events OFF
GO

-----------------------------------------------------------
--Insert data into HtmlText
-----------------------------------------------------------
IF(	IDENT_INCR( 'HtmlText' ) IS NOT NULL OR IDENT_SEED('HtmlText') IS NOT NULL ) SET IDENTITY_INSERT HtmlText ON
IF(	IDENT_INCR( 'HtmlText' ) IS NOT NULL OR IDENT_SEED('HtmlText') IS NOT NULL ) SET IDENTITY_INSERT HtmlText OFF
GO

-----------------------------------------------------------
--Insert data into Announcements
-----------------------------------------------------------
IF(	IDENT_INCR( 'Announcements' ) IS NOT NULL OR IDENT_SEED('Announcements') IS NOT NULL ) SET IDENTITY_INSERT Announcements ON
INSERT INTO Announcements (ItemID,ModuleID,CreatedByUser,CreatedDate,Title,MoreLink,MobileMoreLink,ExpireDate,Description) VALUES('0','0',NULL,NULL,NULL,NULL,NULL,NULL,NULL)
IF(	IDENT_INCR( 'Announcements' ) IS NOT NULL OR IDENT_SEED('Announcements') IS NOT NULL ) SET IDENTITY_INSERT Announcements OFF
GO

-----------------------------------------------------------
--Insert data into Contacts
-----------------------------------------------------------
IF(	IDENT_INCR( 'Contacts' ) IS NOT NULL OR IDENT_SEED('Contacts') IS NOT NULL ) SET IDENTITY_INSERT Contacts ON
INSERT INTO Contacts (ItemID,ModuleID,CreatedByUser,CreatedDate,Name,Role,Email,Contact1,Contact2) VALUES('0','0',NULL,NULL,NULL,NULL,NULL,NULL,NULL)
IF(	IDENT_INCR( 'Contacts' ) IS NOT NULL OR IDENT_SEED('Contacts') IS NOT NULL ) SET IDENTITY_INSERT Contacts OFF
GO

-----------------------------------------------------------
--Insert data into Layouts
-----------------------------------------------------------
IF(	IDENT_INCR( 'Layouts' ) IS NOT NULL OR IDENT_SEED('Layouts') IS NOT NULL ) SET IDENTITY_INSERT Layouts ON
INSERT INTO Layouts (LayoutID,PortalID,FriendlyName,DesktopSrc,MobileSrc) VALUES('1',NULL,'Default','Default/DesktopPortalBanner.ascx',NULL)
IF(	IDENT_INCR( 'Layouts' ) IS NOT NULL OR IDENT_SEED('Layouts') IS NOT NULL ) SET IDENTITY_INSERT Layouts OFF
GO

-----------------------------------------------------------
--Insert data into Links
-----------------------------------------------------------
IF(	IDENT_INCR( 'Links' ) IS NOT NULL OR IDENT_SEED('Links') IS NOT NULL ) SET IDENTITY_INSERT Links ON
INSERT INTO Links (ItemID,ModuleID,CreatedByUser,CreatedDate,Title,Url,MobileUrl,ViewOrder,Description) VALUES('0','0',NULL,NULL,NULL,NULL,NULL,NULL,NULL)
IF(	IDENT_INCR( 'Links' ) IS NOT NULL OR IDENT_SEED('Links') IS NOT NULL ) SET IDENTITY_INSERT Links OFF
GO

-----------------------------------------------------------
--Insert data into Roles
-----------------------------------------------------------
IF(	IDENT_INCR( 'Roles' ) IS NOT NULL OR IDENT_SEED('Roles') IS NOT NULL ) SET IDENTITY_INSERT Roles ON
INSERT INTO Roles (RoleID,PortalID,RoleName,Permission) VALUES('0','0','Admins','1')
IF(	IDENT_INCR( 'Roles' ) IS NOT NULL OR IDENT_SEED('Roles') IS NOT NULL ) SET IDENTITY_INSERT Roles OFF
GO

-----------------------------------------------------------
--Insert data into SolutionModuleDefinitions
-----------------------------------------------------------
IF(	IDENT_INCR( 'SolutionModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('SolutionModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT SolutionModuleDefinitions ON
IF(	IDENT_INCR( 'SolutionModuleDefinitions' ) IS NOT NULL OR IDENT_SEED('SolutionModuleDefinitions') IS NOT NULL ) SET IDENTITY_INSERT SolutionModuleDefinitions OFF
GO

-----------------------------------------------------------
--Insert data into Solutions
-----------------------------------------------------------
IF(	IDENT_INCR( 'Solutions' ) IS NOT NULL OR IDENT_SEED('Solutions') IS NOT NULL ) SET IDENTITY_INSERT Solutions ON
INSERT INTO Solutions (SolutionsID,SolDescription) VALUES('1','Appleseed')
IF(	IDENT_INCR( 'Solutions' ) IS NOT NULL OR IDENT_SEED('Solutions') IS NOT NULL ) SET IDENTITY_INSERT Solutions OFF
GO

-----------------------------------------------------------
--Insert data into Countries
-----------------------------------------------------------
IF(	IDENT_INCR( 'Countries' ) IS NOT NULL OR IDENT_SEED('Countries') IS NOT NULL ) SET IDENTITY_INSERT Countries ON
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AD','Andorra','Andorra','Andorre','Andorra','Andorra','Andorra')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AE','Emirati Arabi','United Arab Emirates','Émirats Arabes Unis','Emiratos Árabes Unidos','Vereinigte Arabische Emirate','Emirados Árabes Unidos')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AF','Afghanistan','Afghanistan','Afghanistan','Afganistán','Afghanistan','Afeganistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AG','Antigua e Barbuda','Antigua And Barbuda','Antigua-et-Barbuda','Antigua y Barbuda','Antigua und Barbuda','Antígua e Barbuda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AI','Anguilla','Anguilla','Anguilla','Anguilla','Anguilla','Anguilla')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AL','Albania','Albania','Albanie','Albania','Albanien','Albânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AM','Armenia','Armenia','Arménie','Armenia','Armenien','Armênia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AN','Antille Olandesi','Netherlands Antilles','Antilles néerlandaises','Antillas Holandesas','Niederländisch-Antillen','Antilhas Holandesas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AO','Angola','Angola','Angola','Angola','Angola','Angola')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AQ','Antartide','Antarctica','Antarctique','Antártida','Antarktis','Antártida')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AR','Argentina','Argentina','Argentine','Argentina','Argentinien','Argentina')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AS','Samoa Americane','American Samoa','Samoa américaines','Samoa Americana','Amerikanisch-Samoa','Samoa Americana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AT','Austria','Austria','Autriche','Austria','Österreich','Áustria')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AU','Australia','Australia','Australie','Australia','Australien','Austrália')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AW','Aruba','Aruba','Aruba','Aruba','Aruba','Aruba')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('AZ','Azerbaigian','Azerbaijan','Azerbaïdjan','Azerbaiyán','Aserbaidschan','Azerbaijão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BA','Bosnia-Erzegovina','Bosnia AND Herzegovina','Bosnie et Herzégovine','Bosnia y Herzegovina','Bosnien und Herzegowina','Bósnia e Herzegovina')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BB','Barbados','Barbados','Barbade (la)','Barbados','Barbados','Barbados')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BD','Bangladesh','Bangladesh','Bangladesh','Bangladesh','Bangladesch','Bangladesh')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BE','Belgio','Belgium','Belgique','Bélgica','Belgien','Bélgica')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BF','Burkina Faso','Burkina Faso','Burkina-Faso','Burkina Faso','Burkina Faso','Burkina Faso')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BG','Bulgaria','Bulgaria','Bulgarie','Bulgaria','Bulgarien','Bulgária')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BH','Bahrein','Bahrain','Bahreïn','Bahrein','Bahrain','Bahrein')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BI','Burundi','Burundi','Burundi','Burundi','Burundi','Burundi')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BJ','Benin','Benin','Bénin','Benin','Benin','Benin')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BM','Bermuda','Bermuda','Bermudes','Bermudas','Bermuda','Bermuda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BN','Brunei','Brunei','Brunei','Brunei','Brunei Darussalam','Brunei')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BO','Bolivia','Bolivia','Bolivie','Bolivia','Bolivien','Bolívia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BR','Brasile','Brazil','Brésil','Brasil','Brasilien','Brasil')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BS','Bahamas','Bahamas, The','Bahamas','Bahamas','Bahamas','Bahamas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BT','Bhutan','Bhutan','Bhoutan','Bután','Bhutan','Butão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BV','Isola Bouvet','Bouvet Island','Îles Bouvet','Isla Bouvet','Bouvet-Insel','Ilha Bouvet')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BW','Botswana','Botswana','Botswana','Botswana','Botswana','Botsuana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BY','Bielorussia','Belarus','Biélorussie','Bielorrusia','Weissrussland','Belarus')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('BZ','Belize','Belize','Belize','Belice','Belize','Belize')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CA','Canada','Canada','Canada','Canadá','Kanada','Canadá')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CC','Isole Cocos (Keeling)','Cocos (Keeling) Islands','Îles Cocos-Keeling','Islas de Cocos o Keeling','Kokosinseln','Ilhas Cocos (Keeling)')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CD','Congo, Repubblica Dem. del','Congo, Democractic Republic of the','Congo, République du','Congo, República Democrática del','Kongo, Demokratische Republik','Congo, República Popular do')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CF','Repubblica Centrafricana','Central African Republic','République Centrafricaine','República Centroafricana','Zentralafrikanische Republik','República Centro-Africana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CG','Congo','Congo','Congo','Congo','Kongo','Congo')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CH','Svizzera','Switzerland','Suisse','Suiza','Schweiz','Suíça')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CI','Costa d''Avorio','Cote D''Ivoire (Ivory Coast)','Côte D''Ivoire','Costa de Marfíl','Côte d''Ivoire','Costa do Marfim')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CK','Isole Cook','Cook Islands','Îles Cook','Islas Cook','Cookinseln','Ilhas Cook')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CL','Cile','Chile','Chili','Chile','Chile','Chile')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CM','Camerun','Cameroon','Cameroun','Camerún','Kamerun','Camarões')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CN','Cina','China','Chine','China','China','China')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CO','Colombia','Colombia','Colombie','Colombia','Kolumbien','Colômbia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CR','Costa Rica','Costa Rica','Costa Rica','Costa Rica','Costa Rica','Costa Rica')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CU','Cuba','Cuba','Cuba','Cuba','Kuba','Cuba')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CV','Capo Verde','Cape Verde','Cap-Vert','Cabo Verde','Kap Verde','Cabo Verde')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CX','Isole Christmas','Christmas Island','Île Christmas','Isla de Christmas','Weihnachtsinseln','Ilhas Christmas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CY','Cipro','Cyprus','Chypre','Chipre','Zypern','Chipre')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('CZ','Repubblica Ceca','Czech Republic','République tchèque','República Checa','Tschechische Republik','República Tcheca')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DE','Germania','Germany','Allemagne','Alemania','Deutschland','Alemanha')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DJ','Gibuti','Djibouti','Djibouti','Djibouti','Djibouti','Djibuti')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DK','Danimarca','Denmark','Danemark','Dinamarca','Dänemark','Dinamarca')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DM','Dominica','Dominica','Dominique (la)','Dominica','Dominica','Dominica')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DO','Repubblica Dominicana','Dominican Republic','République Dominicaine','República Dominicana','Dominikanische Republik','República Dominicana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('DZ','Algeria','Algeria','Algérie','Argelia','Algerien','Argélia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('EC','Ecuador','Ecuador','Équateur (République de l'')','Ecuador','Ecuador','Equador')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('EE','Estonia','Estonia','Estonie','Estonia','Estland','Estônia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('EG','Egitto','Egypt','Égypte','Egipto','Ägypten','Egito')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ER','Eritrea','Eritrea','Érythrée','Eritrea','Eritrea','Eritréia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ES','Spagna','Spain','Espagne','España','Spanien','Espanha')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ET','Etiopia','Ethiopia','Éthiopie','Etiopía','Äthiopien','Etiópia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FI','Finlandia','Finland','Finlande','Finlandia','Finnland','Finlândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FJ','Fiji','Fiji Islands','Fidji','Fiji','Fidschi','Fiji')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FK','Isole Falkland (Islas Malvinas)','Falkland Islands (Islas Malvinas)','Îles Malouines','Islas Malvinas','Falklandinseln','Ilhas Malvinas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FM','Micronesia','Micronesia','Micronésie','Micronesia','Mikronesien','Micronésia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FO','Isole Faroe','Faroe Islands','Îles Féroé','Islas Faroe','Färöer','Ilhas Faroés')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('FR','Francia','France','France','Francia','Frankreich','França')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GA','Gabon','Gabon','Gabon','Gabón','Gabun','Gabão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GD','Grenada','Grenada','Grenade','Granada','Grenada','Grenada')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GE','Georgia','Georgia','Géorgie','Georgia','Georgien','Geórgia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GF','Guiana francese','French Guiana','Guyane française','Guayana Francesa','Französisch-Guyana','Guiana Francesa')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GH','Ghana','Ghana','Ghana','Ghana','Ghana','Gana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GI','Gibilterra','Gibraltar','Gibraltar','Gibraltar','Gibraltar','Gibraltar')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GL','Groenlandia','Greenland','Groenland','Groenlandia','Grönland','Groenlândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GM','Gambia','Gambia, The','Gambie','Gambia','Gambia','Gâmbia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GN','Guinea','Guinea','Guinée','Guinea','Guinea','Guiné')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GP','Guadalupe','Guadeloupe','Guadeloupe (France DOM-TOM)','Guadalupe','Guadeloupe','Guadalupe')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GQ','Guinea equatoriale','Equatorial Guinea','Guinée Équatoriale','Guinea Ecuatorial','Äquatorial-Guinea','Guiné Equatorial')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GR','Grecia','Greece','Grèce','Grecia','Griechenland','Grécia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GS','Georgia del Sud e Isole Sandwich Meridionali','South Georgia And The South Sandwich Islands','Géorgie du Sud et Sandwich du Sud (ÎIes)','Georgia del Sur y las islas Sandwich del Sur','Südgeorgien und Sandwich-Inseln','Ilhas Geórgia do Sul e Sandwich do Sul')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GT','Guatemala','Guatemala','Guatemala','Guatemala','Guatemala','Guatemala')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GU','Guam','Guam','Guam','Guam','Guam','Guam')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GW','Guinea-Bissau','Guinea-Bissau','Guinée-Bissau','Guinea-Bissau','Guinea-Bissau','Guiné-Bissau')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('GY','Guyana','Guyana','Guyane','Guayana','Guyana','Guiana')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HK','Hong Kong SAR','Hong Kong S.A.R.','Hong Kong, Région administrative spéciale','Hong Kong, ZAE de la RPC','Hong Kong SAR','Hong Kong RAE, República Popular da China')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HM','Isole Heard e Mcdonald','Heard AND McDonald Islands','Îles Heard et Mc Donald','Islas Heard y McDonald','Heard- und Mcdonald-Inseln','Ilhas Heard e McDonald')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HN','Honduras','Honduras','Honduras (le)','Honduras','Honduras','Honduras')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HR','Croazia','Croatia (Hrvatska)','Croatie','Croacia (Hrvatska)','Kroatien','Croácia (Hrvatska)')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HT','Haiti','Haiti','Haïti','Haití','Haiti','Haiti')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('HU','Ungheria','Hungary','Hongrie','Hungría','Ungarn','Hungria')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ID','Indonesia','Indonesia','Indonésie','Indonesia','Indonesien','Indonésia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IE','Irlanda','Ireland','Irlande','Irlanda','Irland','Irlanda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IL','Israele','Israel','Israël','Israel','Israel','Israel')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IN','India','India','Inde','India','Indien','Índia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IO','Territori inglesi dell''Oceano Indiano','British Indian Ocean Territory','Territoires Britanniques de l''océan Indien','Territorios británicos del océano Índico','British Indian Ocean Territories','Território Britânico do Oceano Índico')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IQ','Iraq','Iraq','Irak','Irak','Irak','Iraque')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IR','Iran','Iran','Iran','Irán','Iran','Irã')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IS','Islanda','Iceland','Islande','Islandia','Island','Islândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('IT','Italia','Italy','Italie','Italia','Italien','Itália')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('JM','Giamaica','Jamaica','Jamaïque','Jamaica','Jamaika','Jamaica')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('JO','Giordania','Jordan','Jordanie','Jordania','Jordanien','Jordânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('JP','Giappone','Japan','Japon','Japón','Japan','Japão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KE','Kenya','Kenya','Kenya','Kenia','Kenia','Quênia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KG','Kirghizistan','Kyrgyzstan','Kirghizistan','Kirguizistán','Kirgisistan','Quirguistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KH','Cambogia','Cambodia','Cambodge','Camboya','Kambodscha','Camboja')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KI','Kiribati','Kiribati','Kiribati','Kiribati','Kiribati','Kiribati')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KM','Comore','Comoros','Comores','Comores','Kolumbien','Comores')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KN','Saint Kitts e Nevis','Saint Kitts And Nevis','St Christopher et Nevis (Îles)','Saint Kitts y Nevis','Saint Kitts AND Nevis','Saint Kitts e Névis')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KP','Corea del Nord','Korea, North','Corée du Nord','Corea del Norte','Korea (Dem. Volksrepublik Nordkorea)','Coréia do Norte')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KR','Corea','Korea','Corée','Corea','Korea','Coréia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KW','Kuwait','Kuwait','Koweït','Kuwait','Kuwait','Kuwait')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KY','Isole Cayman','Cayman Islands','Îles Caïmans','Islas Caimán','Cayman-Inseln','Ilhas Cayman')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('KZ','Kazakistan','Kazakhstan','Kazakhstan','Kazajistán','Kasachstan','Cazaquistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LA','Laos','Laos','République démocratique populaire du Laos','Laos','Laos','Laos')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LB','Libano','Lebanon','Liban','Líbano','Libanon','Líbano')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LC','Saint Lucia','Saint Lucia','Saint-Lucie','Santa Lucía','Saint Lucia','Santa Lúcia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LI','Liechtenstein','Liechtenstein','Liechtenstein','Liechtenstein','Liechtenstein','Liechtenstein')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LK','Sri Lanka','Sri Lanka','Sri Lanka','Sri Lanka','Sri Lanka','Sri Lanka')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LR','Liberia','Liberia','Liberia','Liberia','Liberia','Libéria')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LS','Lesotho','Lesotho','Lesotho','Lesotho','Lesotho','Lesoto')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LT','Lituania','Lithuania','Lituanie','Lituania','Litauen','Lituânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LU','Lussemburgo','Luxembourg','Luxembourg','Luxemburgo','Luxemburg','Luxemburgo')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LV','Lettonia','Latvia','Lettonie','Letonia','Lettland','Letônia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('LY','Libia','Libya','Jamahiriya arabe libyenne (Lybie)','Libia','Libyen (Volksrepublik)','Líbia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MA','Marocco','Morocco','Maroc','Marruecos','Marokko','Marrocos')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MC','Monaco','Monaco','Monaco','Mónaco','Monaco','Mônaco')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MD','Moldavia','Moldova','Moldavie','Moldavia','Moldawien','Moldávia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MG','Madagascar','Madagascar','Madagascar','Madagascar','Madagaskar','Madagascar')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MH','Isole Marshall','Marshall Islands','Îles Marshall','Islas Marshall','Marshall-Inseln','Ilhas Marshall')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MK','Macedonia, Ex Repubblica Iugoslava di','Macedonia, Former Yugoslav Republic of','Macédoine, Ex-République yougoslave de','Macedonia, Ex-República Yugoslava de','Mazedonien, Ehemalige jugoslawische Republik','Macedônia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ML','Mali','Mali','Mali','Malí','Mali','Mali')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MM','Myanmar','Myanmar','Myanmar (Union de)','Birmania','Myanmar','Myanma')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MN','Mongolia','Mongolia','Mongolie','Mongolia','Mongolei','Mongólia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MO','Macao','Macau S.A.R.','Macao','Macao','Makao','Macau')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MP','Isole Marianne Settentrionali','Northern Mariana Islands','Mariannes du Nord (Îles du Commonwealth)','Islas Marianas del Norte','Nördliche Marianen','Ilhas Marianas do Norte')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MQ','Martinica','Martinique','Martinique (France DOM-TOM)','Martinica','Martinique','Martinica')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MR','Mauritania','Mauritania','Mauritanie','Mauritania','Mauretanien','Mauritânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MS','Montserrat','Montserrat','Montserrat','Montserrat','Montserrat','Montserrat')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MT','Malta','Malta','Malte','Malta','Malta','Malta')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MU','Mauritius','Mauritius','Île Maurice','Mauricio','Mauritius','Maurício')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MV','Maldive','Maldives','Maldives','Maldivas','Malediven','Maldivas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MW','Malawi','Malawi','Malawi','Malawi','Malawi','Malaui')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MX','Messico','Mexico','Mexique','México','Mexiko','México')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MY','Malesia','Malaysia','Malaisie','Malasia','Malaysia','Malásia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('MZ','Mozambico','Mozambique','Mozambique','Mozambique','Mosambik','Moçambique')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NA','Namibia','Namibia','Namibie','Namibia','Namibia','Namíbia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NC','Nuova Caledonia','New Caledonia','Nouvelle Calédonie','Nueva Caledonia','Neukaledonien','Nova Caledônia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NE','Niger','Niger','Niger','Níger','Niger','Níger')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NF','Isole Norfolk','Norfolk Island','Île de Norfolk','Norfolk','Norfolk Island','Ilha Norfolk')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NG','Nigeria','Nigeria','Nigéria','Nigeria','Nigeria','Nigéria')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NI','Nicaragua','Nicaragua','Nicaragua','Nicaragua','Nicaragua','Nicarágua')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NL','Paesi Bassi','Netherlands, The','Pays-Bas','Países Bajos','Niederlande','Holanda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NN','Sconosciuto','Unknown','Unknown','Unknown','Unknown','Unknown')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NO','Norvegia','Norway','Norvège','Noruega','Norwegen','Noruega')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NP','Nepal','Nepal','Népal','Nepal','Nepal','Nepal')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NR','Nauru','Nauru','Nauru (République de)','Nauru','Nauru','Nauru')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NU','Niue','Niue','Niue','Niue','Niue','Niue')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('NZ','Nuova Zelanda','New Zealand','Nouvelle Zélande','Nueva Zelanda','Neuseeland','Nova Zelândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('OM','Oman','Oman','Oman','Omán','Oman','Omã')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PA','Panama','Panama','Panama','Panamá','Panama','Panamá')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PE','Perù','Peru','Pérou','Perú','Peru','Peru')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PF','Polinesia francese','French Polynesia','Polynésie française (DOM-TOM)','Polinesia Francesa','Französisch-Polynesien','Polinésia Francesa')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PG','Papua Nuova Guinea','Papua new Guinea','Papouasie Nouvelle-Guinée','Papúa Nueva Guinea','Papua-Neuguinea','Papua-Nova Guiné')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PH','Filippine','Philippines','Philippines','Filipinas','Philippinen','Filipinas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PK','Pakistan','Pakistan','Pakistan','Paquistán','Pakistan','Paquistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PL','Polonia','Poland','Pologne','Polonia','Polen','Polônia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PM','Saint Pierre et Miquelon','Saint Pierre AND Miquelon','Saint-Pierre-et-Miquelon (France DOM-TOM)','St. Pierre y Miquelon','Saint-Pierre-et-Miquelon','St. Pierre e Miquelon')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PN','Pitcairn','Pitcairn Island','Pitcairn (Îles)','Pitcairn','Pitcairn','Pitcairn')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PR','Puerto Rico','Puerto Rico','Porto Rico','Puerto Rico','Puerto Rico','Porto Rico')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PT','Portogallo','Portugal','Portugal','Portugal','Portugal','Portugal')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PW','Palau','Palau','Palau','Islas Palau','Palau','Palau')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('PY','Paraguay','Paraguay','Paraguay','Paraguay','Paraguay','Paraguai')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('QA','Qatar','Qatar','Qatar','Qatar','Katar','Catar')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('RE','Reunion','Reunion','Réunion (Île de la)','Reunión','Réunion','Reunião')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('RO','Romania','Romania','Roumanie','Rumania','Rumänien','Romênia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('RU','Russia','Russia','Fédération de Russie','Rusia','Russische Föderation','Rússia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('RW','Ruanda','Rwanda','Rwanda','Ruanda','Ruanda','Ruanda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SA','Arabia Saudita','Saudi Arabia','Arabie Saoudite','Arabia Saudí','Saudi-Arabien','Arábia Saudita')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SB','Isole Salomone','Solomon Islands','Îles Salomon','Islas Salomón','Salomonen','Ilhas Salomão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SC','Seychelles','Seychelles','Seychelles','Seychelles','Seychellen','Seychelles')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SD','Sudan','Sudan','Soudan','Sudán','Sudan','Sudão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SE','Svezia','Sweden','Suède','Suecia','Schweden','Suécia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SG','Singapore','Singapore','Singapore','Singapore','Singapore','Singapore')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SH','Sant''Elena','Saint Helena','Sainte Hélène','Santa Helena','Sankt Helena','Santa Helena')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SI','Slovenia','Slovenia','Slovénie','Eslovenia','Slowenien','Eslovênia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SJ','Isole Svalbard e Jan Mayen','Svalbard And Jan Mayen Islands','Île Svalbard et Jan Mayen','Islas Svalbard y Jan Mayen','Svalbard und Jan Mayen','Ilhas Svalbard e Jan Mayen')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SK','Slovacchia','Slovakia','Slovaquie','República Eslovaca','Slowakei','Eslováquia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SL','Sierra Leone','Sierra Leone','Sierra Leone','Sierra Leona','Sierra Leone','Serra Leoa')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SM','San Marino','San Marino','Saint-Marin','San Marino','San Marino','San Marino')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SN','Senegal','Senegal','Sénégal','Senegal','Senegal','Senegal')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SO','Somalia','Somalia','Somalie','Somalia','Somalia','Somália')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SR','Suriname','Suriname','Suriname','Surinam','Surinam','Suriname')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ST','Sao Tome e Principe','Sao Tome AND Principe','Sâo Tomé et Prince','Santo Tomé y Príncipe','São Tomé und Príncipe','São Tomé e Príncipe')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SV','El Salvador','El Salvador','Salvador','El Salvador','El Salvador','El Salvador')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SY','Siria','Syria','République arabe syrienne','Siria','Syrien','Síria')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('SZ','Swaziland','Swaziland','Swaziland','Suazilandia','Swasiland','Suazilândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TC','Isole Turks e Caicos','Turks And Caicos Islands','Îles Turks et Caïcos','Islas Turks y Caicos','Turks- und Caicosinseln','Ilhas Turks e Caicos')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TD','Ciad','Chad','Tchad','Chad','Tschad','Chade')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TF','Territori australi francesi','French Southern Territories','Terres Australes françaises (DOM-TOM)','Territorios franceses del Sur','Französische Südgebiete','Territórios Franceses do Sul')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TG','Togo','Togo','Togo','Togo','Togo','Togo')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TH','Tailandia','Thailand','Thaïlande','Tailandia','Thailand','Tailândia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TJ','Tagikistan','Tajikistan','Tajikistan','Tayikistán','Tadschikistan','Tadjiquistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TK','Tokelau','Tokelau','Îles Tokelau','Islas Tokelau','Tokelau','Tokelau')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TM','Turkmenistan','Turkmenistan','Turkménistan','Turkmenistán','Turkmenistan','Turcomenistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TN','Tunisia','Tunisia','Tunisie','Túnez','Tunesien','Tunísia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TO','Tonga','Tonga','Tonga','Tonga','Tonga','Tonga')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TP','Timor Est','East Timor','Timor oriental (partie orientale)','Timor Oriental','Ost-Timor','Timor Oriental')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TR','Turchia','Turkey','Turquie','Turquía','Türkei','Turquia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TT','Trinidad e Tobago','Trinidad And Tobago','Trinité-et-Tobago','Trinidad y Tobago','Trinidad und Tobago','Trinidad e Tobago')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TV','Tuvalu','Tuvalu','Tuvalu (Îles)','Tuvalu','Tuvalu','Tuvalu')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TW','Taiwan','Taiwan','Taiwan','Taiwán','Taiwan','Taiwan')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('TZ','Tanzania','Tanzania','Tanzanie','Tanzania','Tansania','Tanzânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UA','Ucraina','Ukraine','Ukraine','Ucrania','Ukraine','Ucrânia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UG','Uganda','Uganda','Ouganda','Uganda','Uganda','Uganda')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UK','Gran Bretagna','United Kingdom','Royaume-Uni','Reino Unido','Vereinigtes Königreich','Reino Unido')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UM','Isole minori degli Stati Uniti','United States Minor Outlying Islands','Dépendances américaines du Pacifique','Islas menores de Estados Unidos','Vereinigte Staaten von Amerika, vorgelagerte Insel','Territórios Insulares dos EUA')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('US','Stati Uniti','United States','États-Unis','Estados Unidos','Vereinigte Staaten von Amerika','Estados Unidos')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UY','Uruguay','Uruguay','Uruguay','Uruguay','Uruguay','Uruguai')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('UZ','Uzbekistan','Uzbekistan','Ouzbékistän','Uzbekistán','Usbekistan','Uzbequistão')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VA','Città del Vaticano','Vatican City State (Holy See)','État de la cité du Vatican','Ciudad del Vaticano (Santa Sede)','Vatikanstadt','Cidade do Vaticano (Santa Sé)')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VC','Saint Vincent e Grenadine','Saint Vincent And The Grenadines','Saint-Vincent et les Grenadines','San Vicente y Granadinas','Saint Vincent und die Grenadines','São Vicente e Granadinas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VE','Venezuela','Venezuela','Venezuela','Venezuela','Venezuela','Venezuela')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VG','Isole Vergini (GB)','Virgin Islands (British)','Îles Vierges britanniques','Islas Vírgenes (Reino Unido)','Jungferninseln (Britisch)','Ilhas Virgens Britânicas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VI','Isole Vergini (USA)','Virgin Islands (US)','Îles Vierges américaines','Islas Vírgenes (EE.UU.)','Jungferninseln (U.S.)','Ilhas Virgens Americanas')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VN','Vietnam','Vietnam','Vietnam','Vietnam','Vietnam','Vietnã')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('VU','Vanuatu','Vanuatu','Vanuatu (République de)','Vanuatu','Vanuatu','Vanuatu')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('WF','Isole Wallis e Futuna','Wallis And Futuna Islands','Wallis et Futuna','Islas Wallis y Futuna','Wallis und Futuna','Ilhas Wallis e Futuna')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('WS','Samoa','Samoa','Samoa','Samoa','Samoa','Samoa')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('YE','Yemen','Yemen','Yémen','Yemen','Jemen','Iêmen')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('YT','Mayotte','Mayotte','Mayotte','Mayotte','Mayotte','Mayotte')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('YU','Iugoslavia','Yugoslavia','Yougoslavie','Yugoslavia','Jugoslawien','Iugoslávia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ZA','Sudafrica','South Africa','Afrique du Sud','República de Sudáfrica','Republik Südafrika','África do Sul')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ZM','Zambia','Zambia','Zambie','Zambia','Sambia','Zâmbia')
INSERT INTO Countries (PK_IDCountry,IT,EN,FR,ES,DE,PT) VALUES('ZW','Zimbabwe','Zimbabwe','Zimbabwe','Zimbabue','Zimbabwe','Zimbábue')
IF(	IDENT_INCR( 'Countries' ) IS NOT NULL OR IDENT_SEED('Countries') IS NOT NULL ) SET IDENTITY_INSERT Countries OFF
GO

-----------------------------------------------------------
--Insert data into States
-----------------------------------------------------------
IF(	IDENT_INCR( 'States' ) IS NOT NULL OR IDENT_SEED('States') IS NOT NULL ) SET IDENTITY_INSERT States ON
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('0','Unspecified','NN')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('599','Abu Dhabi','AE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('1003','Alabama','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('1040','Alaska','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('1076','Alberta','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('1945','Arizona','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('1951','Arkansas','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('2082','Ash Shariqah','AE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('2537','Baden-Württemberg','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('4934','British Columbia','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('5155','Buenos Aires','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('5599','California','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('6427','Chaco','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('7636','Colorado','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('7798','Connecticut','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('7915','Cordoba','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('7990','Corrientes','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8831','Delaware','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9130','District of Columbia','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9470','Dubai','AE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10313','Entre Rios','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('11032','Florida','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('12004','Georgia','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('13339','Hamburg','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('13656','Hawaii','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('13852','Herat','AF')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('14713','Idaho','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('14808','Illinois','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('14882','Indiana','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('14987','Iowa','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('15851','Kabul','AF')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('16074','Kandahar','AF')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('16121','Kansas','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('16480','Kentucky','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('19283','Louisiana','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('19840','Maine','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('20110','Manitoba','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('20487','Maryland','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('20543','Massachusetts','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('20696','Mazar-e Sharif','AF')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('20974','Mendoza','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('21196','Michigan','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('21412','Minnesota','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('21502','Mississippi','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('21512','Missouri','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('21789','Montana','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('22869','Nebraska','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23013','Neuquen','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23035','Nevada','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23074','New Brunswick','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23097','New Hampshire','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23117','New Jersey','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23132','New Mexico','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23161','New York','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23204','Newfoundland','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23331','Niedersachsen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23611','North Carolina','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23624','North Dakota','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23766','Northwest Territories','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('23841','Nova Scotia','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('24230','Ohio','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('24293','Oklahoma','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('24465','Ontario','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('24561','Oregon','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('25623','Pennsylvania','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('26704','Prince Edward Island','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('27068','Quebec','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('27654','Rheinland-Pfalz','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('27664','Rhode Island','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('28479','Saarland','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29054','Salta','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29330','San Juan','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29642','Santa Cruz','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29657','Santa Fe','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29744','Santiago del Estero','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('29926','Saskatchewan','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('31410','South Carolina','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('31418','South Dakota','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('33025','Tennessee','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('33145','Texas','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('34062','Tucuman','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('34626','Utah','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('35022','Vermont','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('35364','Virginia','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('35841','Washington','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('36208','West Virginia','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('36684','Wisconsin','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('36927','Wyoming','US')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('37348','Yukon Territory','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('41760','Bremen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('42096','Nunavut','CA')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8045746','Berlin','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8125502','Thuringen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8141652','Schleswig-Holstein','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8145597','Sachsen-Anhalt','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8145598','Sachsen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8164991','Nordrhein-Westfalen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8175722','Mecklenburg-Vorpommern','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8208586','Hessen','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8248738','Brandenburg','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8255245','Bayern','DE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8521702','Yvelines','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8521729','Yonne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8522188','Vosges','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8523323','Paris','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8523790','Vienne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8524650','Vendee','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8525064','Vaucluse','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8525260','Var','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8525582','Val-drOise','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8525601','Val-De-Marne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8527877','Tarn-et-Garonne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8527882','Tarn','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8528825','Somme','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8529775','Seine-Saint-Denis','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8529777','Seine-Maritime','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8529778','Seine-et-Marne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8530048','Savoie','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8530458','Sarthe','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8530614','Saone-et-Loire','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8539434','Pyrenees-Orientales','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8539437','Pyrenees-Atlantiques','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8539537','Puy-de-Dome','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8542995','Pas-de-Calais','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8543814','Orne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8544230','Oise','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8544697','Nord','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8544940','Nievre','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8546201','Moselle','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8546453','Morbihan','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8548685','Meuse','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8548691','Meurthe-et-Moselle','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8549516','Mayenne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8550187','Marne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8550855','Manche','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8551250','Maine-et-Loire','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8551878','Lozere','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552113','Lot-et-Garonne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552114','Lot','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552449','Loir-et-Cher','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552450','Loiret','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552454','Loire-Atlantique','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8552463','Loire','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8562476','Landes','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8566665','Jura','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8567332','Isere','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8567422','Indre-et-Loire','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8567423','Indre','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8567467','Ille-et-Vilaine','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568121','Herault','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568278','Hauts-de-Seine','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568284','Haut-Rhin','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568340','Haute-Vienne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568347','Hautes-Pyrenees','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568357','Haute-Savoie','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568358','Haute-Saone','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568359','Hautes-Alpes','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568379','Haute-Marne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568382','Haute-Loire','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568389','Haute-Garonne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8568416','Haute-Corse','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8570578','Gironde','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8570825','Gers','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8571302','Gard','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8573109','Finistere','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8573955','Eure-et-Loir','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8573956','Eure','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8574239','Essonne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8575427','Drome','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8575635','Doubs','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8575688','Dordogne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8576149','Deux-Sevres','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8577167','Creuse','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8578067','Cotes-d''Armor','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8578076','Cote-d''Or','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8578168','Corse-du-Sud','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8578186','Correze','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8580134','Cher','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8581298','Charente-Maritime','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8581300','Charente','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8583450','Cantal','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8583754','Calvados','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8586025','Bouches-du-Rhone','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8588470','Territoire-de-Belfort','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8589403','Bas-Rhin','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8590379','Aveyron','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8590953','Aude','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8591109','Aube','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8591658','Ariege','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8591830','Ardennes','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8591842','Ardeche','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8592753','Alpes-Maritimes','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8592754','Alpes-de-Haute-Provence','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8592815','Allier','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8593082','Aisne','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8593129','Ain','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('8983108','Tierra del Fuego Antartida e Islas del Atlantico','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9217877','Al l''Ayn','AE')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9590886','Zamora','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9591071','Vizcaya','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9593798','Valladolid','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9593864','Valencia','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9595641','Toledo','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9596097','Tarragona','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9596689','Soria','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9597150','Sevilla','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9597450','Segovia','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9598313','Santa Cruz de Tenerife','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9599894','Salamanca','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9602882','Provincia de Pontevedra','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9604804','Palencia','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9605029','Asturias','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9605329','Orense','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9606207','Navarra','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9609211','Malaga','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9609494','Provincia de Lugo','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9610406','La Rioja','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9610903','Lleida','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9610926','Leon','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9611473','Las Palmas','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9613207','La Coruna','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9613931','Jaen','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9614354','Huesca','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9614436','Huelva','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9615112','Guipuzcoa','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9615419','Guadalajara','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9615670','Granada','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9620542','Cuenca','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9621233','Cordoba','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9621755','Ciudad Real','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9625113','Cadiz','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9625152','Caceres','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9625584','Burgos','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9627509','Barcelona','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9627875','Badajoz','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9628030','Avila','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9629623','Almeria','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9629788','Alicante','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9630410','Albacete','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9630421','Alava','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9867354','Viterbo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9867790','Vicenza','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9867907','Verona','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9867950','Vercelli','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9867990','Venezia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868095','Varese','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868517','Udine','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868653','Trieste','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868672','Treviso','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868716','Trento','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9868810','Trapani','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9869061','Torino','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9869351','Terni','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9869522','Taranto','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9870080','Sondrio','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9870214','Siracusa','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9870289','Siena','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9870868','Savona','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9870948','Sassari','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9872871','Salerno','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873060','Rovigo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873306','Roma','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873679','Rieti','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873807','Reggio nella Emilia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873808','Reggio di Calabria','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9873853','Ravenna','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9874381','Potenza','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9874523','Pordenone','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875048','Pistoia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875081','Pisa','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875535','Piacenza','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875659','Pescara','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875668','Pesaro e Urbino','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875675','Perugia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9875893','Pavia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9876000','Parma','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9876200','Palermo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9876337','Padova','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9876861','Novara','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9877095','Napoli','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9878134','Modena','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9878273','Milano','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9878383','Messina','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9878616','Matera','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9878692','Massa Carrara','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9879034','Mantova','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9879594','Lucca','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9879741','Livorno','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880079','Lecce','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880201','Latina','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880226','La Spezia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880275','L''Aquila','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880657','Isernia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9880762','Imperia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9881066','Grosseto','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9881297','Gorizia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9881598','Genova','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9881951','Frosinone','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9882212','Forli-Cesena','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9882361','Foggia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9882456','Firenze','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9882611','Ferrara','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9883330','Cuneo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9883496','Cremona','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9883595','Cosenza','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9883946','Como','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9885203','Catanzaro','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9885210','Catania','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9885780','Caserta','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9887202','Campobasso','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9887534','Cagliari','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9887815','Brindisi','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9887844','Brescia','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888241','Bolzano','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888257','Bologna','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888525','Bergamo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888540','Benevento','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888582','Belluno','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9888744','Bari','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889092','Avellino','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889164','Asti','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889362','Arezzo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889497','Aosta','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889853','Alessandria','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('9889982','Agrigento','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10026439','Rhone','FR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106904','Chubut','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106905','Río Negro','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106906','La Pampa','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106907','Distrito Federal','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106909','San Luis','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106910','La Rioja','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106911','Misiones','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106912','Catamarca','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106913','Formosa','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10106914','Jujuy','AR')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111296','Castellon','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111297','Gerona','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111298','Teruel','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111299','Zaragoza','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111300','Santander','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111301','Madrid','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111302','Murcia','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10111303','Baleares','ES')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126883','Ancona','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126884','Macerata','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126885','Ascoli Piceno','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126886','Chieti','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126887','Teramo','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126888','Ragusa','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126889','Caltanissetta','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126890','Enna','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126891','Oristano','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10126892','Nuoro','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209554','Biella','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209555','Verbano-Cusio-Ossola','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209556','Lodi','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209557','Lecco','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209558','Rimini','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209559','Prato','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209560','Crotone','IT')
INSERT INTO States (PK_IDState,Description,IDCountry_FK) VALUES('10209561','Vibo Valentia','IT')
IF(	IDENT_INCR( 'States' ) IS NOT NULL OR IDENT_SEED('States') IS NOT NULL ) SET IDENTITY_INSERT States OFF
GO

-----------------------------------------------------------
--Insert data into Users
-----------------------------------------------------------
IF(	IDENT_INCR( 'Users' ) IS NOT NULL OR IDENT_SEED('Users') IS NOT NULL ) SET IDENTITY_INSERT Users ON
INSERT INTO Users (UserID,PortalID,Name,Company,Address,City,Zip,IDCountry_FK,IDState_FK,PIva,CFiscale,Phone,Fax,Password,Email,SendNewsletter,MailChecked,LastSend) VALUES('1','0','admin',NULL,NULL,NULL,NULL,'IT',NULL,NULL,NULL,'',NULL,'admin','admin',0,NULL,NULL)
IF(	IDENT_INCR( 'Users' ) IS NOT NULL OR IDENT_SEED('Users') IS NOT NULL ) SET IDENTITY_INSERT Users OFF
GO

-----------------------------------------------------------
--Insert data into UserRoles
-----------------------------------------------------------
IF(	IDENT_INCR( 'UserRoles' ) IS NOT NULL OR IDENT_SEED('UserRoles') IS NOT NULL ) SET IDENTITY_INSERT UserRoles ON
INSERT INTO UserRoles (UserID,RoleID) VALUES('1','0')
IF(	IDENT_INCR( 'UserRoles' ) IS NOT NULL OR IDENT_SEED('UserRoles') IS NOT NULL ) SET IDENTITY_INSERT UserRoles OFF
GO
