---------------------
--1.1.7.1115_00.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_States_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [States] DROP CONSTRAINT FK_States_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Users] DROP CONSTRAINT FK_Users_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_ModuleDefinitions_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [ModuleDefinitions] DROP CONSTRAINT FK_ModuleDefinitions_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_SolutionModuleDefinitions_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [SolutionModuleDefinitions] DROP CONSTRAINT FK_SolutionModuleDefinitions_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Tabs_Layouts]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Tabs] DROP CONSTRAINT FK_Tabs_Layouts
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_PortalSettings_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [PortalSettings] DROP CONSTRAINT FK_PortalSettings_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Roles_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Roles] DROP CONSTRAINT FK_Roles_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Tabs_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Tabs] DROP CONSTRAINT FK_Tabs_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Users] DROP CONSTRAINT FK_Users_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_SolutionModuleDefintions_Solutions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [SolutionModuleDefinitions] DROP CONSTRAINT FK_SolutionModuleDefintions_Solutions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Modules_ModuleDefinitions1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Modules] DROP CONSTRAINT FK_Modules_ModuleDefinitions1
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_UserRoles_Roles]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [UserRoles] DROP CONSTRAINT FK_UserRoles_Roles
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_States]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Users] DROP CONSTRAINT FK_Users_States
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Modules_Tabs1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Modules] DROP CONSTRAINT FK_Modules_Tabs1
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Tabs_Tabs]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Tabs] DROP CONSTRAINT FK_Tabs_Tabs
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Announcements_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Announcements] DROP CONSTRAINT FK_Announcements_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Contacts_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Contacts] DROP CONSTRAINT FK_Contacts_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Discussion_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Discussion] DROP CONSTRAINT FK_Discussion_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Documents_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Documents] DROP CONSTRAINT FK_Documents_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Events_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Events] DROP CONSTRAINT FK_Events_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_HtmlText_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [HtmlText] DROP CONSTRAINT FK_HtmlText_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Links_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Links] DROP CONSTRAINT FK_Links_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_ModuleSettings_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [ModuleSettings] DROP CONSTRAINT FK_ModuleSettings_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_UserRoles_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [UserRoles] DROP CONSTRAINT FK_UserRoles_Users
GO

/****** Oggetto: stored procedure GetSingleMessage    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleMessage]
GO

/****** Oggetto: stored procedure AddContact    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddContact]
GO

/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddEvent]
GO

/****** Oggetto: stored procedure AddLink    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddLink]
GO

/****** Oggetto: stored procedure AddUserRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUserRole]
GO

/****** Oggetto: stored procedure DeleteContact    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteContact]
GO

/****** Oggetto: stored procedure DeleteDocument    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteDocument]
GO

/****** Oggetto: stored procedure DeleteEvent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteEvent]
GO

/****** Oggetto: stored procedure DeleteLink    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteLink]
GO

/****** Oggetto: stored procedure DeleteUserRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteUserRole]
GO

/****** Oggetto: stored procedure GetContacts    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetContacts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetContacts]
GO

/****** Oggetto: stored procedure GetDocumentContent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocumentContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocumentContent]
GO

/****** Oggetto: stored procedure GetDocuments    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocuments]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocuments]
GO

/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetEvents]
GO

/****** Oggetto: stored procedure GetHtmlText    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetHtmlText]
GO

/****** Oggetto: stored procedure GetLinks    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLinks]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetLinks]
GO

/****** Oggetto: stored procedure GetModuleSettings    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleSettings]
GO

/****** Oggetto: stored procedure GetNextMessageID    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetNextMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetNextMessageID]
GO

/****** Oggetto: stored procedure GetPrevMessageID    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPrevMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPrevMessageID]
GO

/****** Oggetto: stored procedure GetRoleMembership    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRoleMembership]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetRoleMembership]
GO

/****** Oggetto: stored procedure GetRolesByUser    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRolesByUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetRolesByUser]
GO

/****** Oggetto: stored procedure GetSingleContact    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleContact]
GO

/****** Oggetto: stored procedure GetSingleDocument    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleDocument]
GO

/****** Oggetto: stored procedure GetSingleEvent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleEvent]
GO

/****** Oggetto: stored procedure GetSingleLink    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleLink]
GO

/****** Oggetto: stored procedure GetThreadMessages    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetThreadMessages]
GO

/****** Oggetto: stored procedure GetTopLevelMessages    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTopLevelMessages]
GO

/****** Oggetto: stored procedure UpdateContact    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateContact]
GO

/****** Oggetto: stored procedure UpdateDocument    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateDocument]
GO

/****** Oggetto: stored procedure UpdateEvent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateEvent]
GO

/****** Oggetto: stored procedure UpdateHtmlText    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateHtmlText]
GO

/****** Oggetto: stored procedure UpdateLink    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateLink]
GO

/****** Oggetto: stored procedure GetModuleDefinitionByID    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitionByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitionByID]
GO

/****** Oggetto: stored procedure GetModulesAllPortals    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesAllPortals]
GO

/****** Oggetto: stored procedure GetModulesByName    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesByName]
GO

/****** Oggetto: stored procedure GetModulesSinglePortal    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesSinglePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesSinglePortal]
GO

/****** Oggetto: stored procedure GetPortalSettings    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalSettings]
GO

/****** Oggetto: stored procedure GetSingleUser    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleUser]
GO

/****** Oggetto: stored procedure GetUsersCount    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsersCount]
GO

/****** Oggetto: stored procedure UpdateModule    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModule]
GO

/****** Oggetto: stored procedure UpdateUser    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUser]
GO

/****** Oggetto: stored procedure AddRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddRole]
GO

/****** Oggetto: stored procedure DeleteRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteRole]
GO

/****** Oggetto: stored procedure GetModuleDefinitionByName    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitionByName]
GO

/****** Oggetto: stored procedure GetModuleInUse    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleInUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleInUse]
GO

/****** Oggetto: stored procedure GetPortalCustomSettings    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalCustomSettings]
GO

/****** Oggetto: stored procedure GetPortalRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalRoles]
GO

/****** Oggetto: stored procedure GetPortalsModules    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalsModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalsModules]
GO

/****** Oggetto: stored procedure GetSingleCountry    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleCountry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleCountry]
GO

/****** Oggetto: stored procedure GetSingleRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleRole]
GO

/****** Oggetto: stored procedure GetStates    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetStates]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetStates]
GO

/****** Oggetto: stored procedure GetTabsFlat    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsFlat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsFlat]
GO

/****** Oggetto: stored procedure GetTabsParent    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsParent]
GO

/****** Oggetto: stored procedure GetTabsinTab    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsinTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsinTab]
GO

/****** Oggetto: stored procedure UpdateModuleDefinitions    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleDefinitions]
GO

/****** Oggetto: stored procedure UpdatePortalSetting    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePortalSetting]
GO

/****** Oggetto: stored procedure UpdateRole    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateRole]
GO

/****** Oggetto: stored procedure UpdateTab    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateTab]
GO

/****** Oggetto: stored procedure AddPortal    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddPortal]
GO

/****** Oggetto: stored procedure DeletePortal    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeletePortal]
GO

/****** Oggetto: stored procedure GetCountries    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountries]
GO

/****** Oggetto: stored procedure GetCountriesFiltered    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountriesFiltered]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountriesFiltered]
GO

/****** Oggetto: stored procedure GetPortalSettingsPortalID    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettingsPortalID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalSettingsPortalID]
GO

/****** Oggetto: stored procedure GetSolutions    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSolutions]
GO

/****** Oggetto: stored procedure AddAnnouncement    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddAnnouncement]
GO

/****** Oggetto: stored procedure AddModule    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddModule]
GO

/****** Oggetto: stored procedure AddModuleDefinition    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddModuleDefinition]
GO

/****** Oggetto: stored procedure AddTab    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddTab]
GO

/****** Oggetto: stored procedure AddUser    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUser]
GO

/****** Oggetto: stored procedure AddUserFull    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUserFull]
GO

/****** Oggetto: stored procedure DeleteAnnouncement    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteAnnouncement]
GO

/****** Oggetto: stored procedure DeleteModule    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteModule]
GO

/****** Oggetto: stored procedure DeleteModuleDefinition    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteModuleDefinition]
GO

/****** Oggetto: stored procedure DeleteTab    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteTab]
GO

/****** Oggetto: stored procedure DeleteUser    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteUser]
GO

/****** Oggetto: stored procedure GetAnnouncements    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAnnouncements]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAnnouncements]
GO

/****** Oggetto: stored procedure GetAuthAddRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthAddRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthAddRoles]
GO

/****** Oggetto: stored procedure GetAuthDeleteRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthDeleteRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthDeleteRoles]
GO

/****** Oggetto: stored procedure GetAuthEditRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthEditRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthEditRoles]
GO

/****** Oggetto: stored procedure GetAuthPropertiesRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPropertiesRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthPropertiesRoles]
GO

/****** Oggetto: stored procedure GetAuthViewRoles    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthViewRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthViewRoles]
GO

/****** Oggetto: stored procedure GetCurrentModuleDefinitions    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCurrentModuleDefinitions]
GO

/****** Oggetto: stored procedure GetGeneralModuleDefinitionByName    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetGeneralModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetGeneralModuleDefinitionByName]
GO

/****** Oggetto: stored procedure GetModuleDefinitions    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitions]
GO

/****** Oggetto: stored procedure GetPortals    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortals]
GO

/****** Oggetto: stored procedure GetSingleAnnouncement    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleAnnouncement]
GO

/****** Oggetto: stored procedure GetSingleArticle    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleArticle]
GO

/****** Oggetto: stored procedure GetSingleArticleWithImages    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticleWithImages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleArticleWithImages]
GO

/****** Oggetto: stored procedure GetSingleModuleDefinition    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleModuleDefinition]
GO

/****** Oggetto: stored procedure GetSolutionModuleDefinitions    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSolutionModuleDefinitions]
GO

/****** Oggetto: stored procedure GetTabSettings    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabSettings]
GO

/****** Oggetto: stored procedure GetTabsByPortal    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsByPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsByPortal]
GO

/****** Oggetto: stored procedure GetUsers    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsers]
GO

/****** Oggetto: stored procedure UpdateAnnouncement    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateAnnouncement]
GO

/****** Oggetto: stored procedure UpdateModuleDefinition    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleDefinition]
GO

/****** Oggetto: stored procedure UpdateModuleOrder    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleOrder]
GO

/****** Oggetto: stored procedure UpdateModuleSetting    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleSetting]
GO

/****** Oggetto: stored procedure UpdatePortalInfo    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePortalInfo]
GO

/****** Oggetto: stored procedure UpdateTabOrder    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTabOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateTabOrder]
GO

/****** Oggetto: stored procedure UpdateUserFull    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUserFull]
GO

/****** Oggetto: stored procedure UserLogin    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UserLogin]
GO

/****** Oggetto: tabella [Announcements]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Announcements]
GO

/****** Oggetto: tabella [Contacts]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Contacts]
GO

/****** Oggetto: tabella [Discussion]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Discussion]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Discussion]
GO

/****** Oggetto: tabella [Documents]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Documents]
GO

/****** Oggetto: tabella [Events]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Events]
GO

/****** Oggetto: tabella [HtmlText]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [HtmlText]
GO

/****** Oggetto: tabella [Links]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Links]
GO

/****** Oggetto: tabella [ModuleSettings]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [ModuleSettings]
GO

/****** Oggetto: tabella [UserRoles]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UserRoles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [UserRoles]
GO

/****** Oggetto: tabella [Modules]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Modules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Modules]
GO

/****** Oggetto: tabella [Users]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Users]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Users]
GO

/****** Oggetto: tabella [ModuleDefinitions]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [ModuleDefinitions]
GO

/****** Oggetto: tabella [PortalSettings]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PortalSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [PortalSettings]
GO

/****** Oggetto: tabella [Roles]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Roles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Roles]
GO

/****** Oggetto: tabella [SolutionModuleDefinitions]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [SolutionModuleDefinitions]
GO

/****** Oggetto: tabella [States]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[States]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [States]
GO

/****** Oggetto: tabella [Tabs]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Tabs]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Tabs]
GO

/****** Oggetto: tabella [Countries]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Countries]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Countries]
GO

/****** Oggetto: tabella [GeneralModuleDefinitions]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [GeneralModuleDefinitions]
GO

/****** Oggetto: tabella [Layouts]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Layouts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Layouts]
GO

/****** Oggetto: tabella [Portals]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Portals]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Portals]
GO

/****** Oggetto: tabella [Solutions]    Data dello script: 07/11/2002 22.27.51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Solutions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Solutions]
GO

/****** Oggetto: tabella [Countries]    Data dello script: 07/11/2002 22.27.58 ******/
CREATE TABLE [Countries] (
    [PK_IDCountry] [nchar] (2) NOT NULL ,
    [IT] [nvarchar] (50) NULL ,
    [EN] [nvarchar] (50) NULL ,
    [FR] [nvarchar] (50) NULL ,
    [ES] [nvarchar] (50) NULL ,
    [DE] [nvarchar] (50) NULL ,
    [PT] [nvarchar] (50) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [GeneralModuleDefinitions]    Data dello script: 07/11/2002 22.27.58 ******/
CREATE TABLE [GeneralModuleDefinitions] (
    [GeneralModDefID]  uniqueidentifier ROWGUIDCOL  NOT NULL ,
    [ClassName] [nvarchar] (128) NULL ,
    [FriendlyName] [nvarchar] (128) NOT NULL ,
    [DesktopSrc] [nvarchar] (256) NOT NULL ,
    [MobileSrc] [nvarchar] (256) NOT NULL ,
    [Admin] [bit] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Layouts]    Data dello script: 07/11/2002 22.27.59 ******/
CREATE TABLE [Layouts] (
    [LayoutID] [int] IDENTITY (1, 1) NOT NULL ,
    [PortalID] [int] NULL ,
    [FriendlyName] [nvarchar] (128) NOT NULL ,
    [DesktopSrc] [nvarchar] (256) NOT NULL ,
    [MobileSrc] [nvarchar] (256) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Portals]    Data dello script: 07/11/2002 22.27.59 ******/
CREATE TABLE [Portals] (
    [PortalID] [int] IDENTITY (-1, 1) NOT NULL ,
    [PortalAlias] [nvarchar] (128) NOT NULL ,
    [PortalName] [nvarchar] (128) NOT NULL ,
    [PortalPath] [nvarchar] (128) NULL ,
    [AlwaysShowEditButton] [bit] NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Solutions]    Data dello script: 07/11/2002 22.28.00 ******/
CREATE TABLE [Solutions] (
    [SolutionsID] [int] IDENTITY (1, 1) NOT NULL ,
    [SolDescription] [nvarchar] (100) NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [ModuleDefinitions]    Data dello script: 07/11/2002 22.28.00 ******/
CREATE TABLE [ModuleDefinitions] (
    [ModuleDefID] [int] IDENTITY (1, 1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [GeneralModDefID] [uniqueidentifier] NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [PortalSettings]    Data dello script: 07/11/2002 22.28.00 ******/
CREATE TABLE [PortalSettings] (
    [PortalID] [int] NOT NULL ,
    [SettingName] [nvarchar] (50) NOT NULL ,
    [SettingValue] [nvarchar] (1500) NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Roles]    Data dello script: 07/11/2002 22.28.01 ******/
CREATE TABLE [Roles] (
    [RoleID] [int] IDENTITY (1,1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [RoleName] [nvarchar] (50) NOT NULL ,
    [Permission] [tinyint] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [SolutionModuleDefinitions]    Data dello script: 07/11/2002 22.28.01 ******/
CREATE TABLE [SolutionModuleDefinitions] (
    [SolutionModDefID] [int] IDENTITY (1, 1) NOT NULL ,
    [GeneralModDefID] [uniqueidentifier] NOT NULL ,
    [SolutionsID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [States]    Data dello script: 07/11/2002 22.28.02 ******/
CREATE TABLE [States] (
    [PK_IDState] [int] NOT NULL ,
    [Description] [nvarchar] (50) NULL ,
    [IDCountry_FK] [nchar] (2) NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Tabs]    Data dello script: 07/11/2002 22.28.02 ******/
CREATE TABLE [Tabs] (
    [TabID] [int] IDENTITY (1, 1) NOT NULL ,
    [ParentTabID] [int] NULL ,
    [TabOrder] [int] NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [TabName] [nvarchar] (50) NOT NULL ,
    [MobileTabName] [nvarchar] (50) NOT NULL ,
    [AuthorizedRoles] [nvarchar] (256) NULL ,
    [ShowMobile] [bit] NOT NULL ,
    [TabLayout] [int] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Modules]    Data dello script: 07/11/2002 22.28.03 ******/
CREATE TABLE [Modules] (
    [ModuleID] [int] IDENTITY (1, 1) NOT NULL ,
    [TabID] [int] NOT NULL ,
    [ModuleDefID] [int] NOT NULL ,
    [ModuleOrder] [int] NOT NULL ,
    [PaneName] [nvarchar] (50) NOT NULL ,
    [ModuleTitle] [nvarchar] (256) NULL ,
    [AuthorizedEditRoles] [nvarchar] (256) NULL ,
    [AuthorizedViewRoles] [nvarchar] (256) NULL ,
    [AuthorizedAddRoles] [nvarchar] (256) NULL ,
    [AuthorizedDeleteRoles] [nvarchar] (256) NULL ,
    [AuthorizedPropertiesRoles] [nvarchar] (256) NULL ,
    [CacheTime] [int] NOT NULL ,
    [ShowMobile] [bit] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Users]    Data dello script: 07/11/2002 22.28.03 ******/
CREATE TABLE [Users] (
    [UserID] [int] IDENTITY (1, 1) NOT NULL ,
    [PortalID] [int] NOT NULL ,
    [Name] [nvarchar] (50) NOT NULL ,
    [Company] [nvarchar] (50) NULL ,
    [Address] [nvarchar] (50) NULL ,
    [City] [nvarchar] (50) NULL ,
    [Zip] [nvarchar] (6) NULL ,
    [IDCountry_FK] [nchar] (2) NULL ,
    [IDState_FK] [int] NULL ,
    [PIva] [nvarchar] (11) NULL ,
    [CFiscale] [nvarchar] (16) NULL ,
    [Phone] [nvarchar] (50) NULL ,
    [Fax] [nvarchar] (50) NULL ,
    [Password] [nvarchar] (20) NULL ,
    [Email] [nvarchar] (100) NOT NULL ,
    [SendNewsletter] [bit] NULL ,
    [MailChecked] [tinyint] NULL ,
    [LastSend] [smalldatetime] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Announcements]    Data dello script: 07/11/2002 22.28.04 ******/
CREATE TABLE [Announcements] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [CreatedByUser] [varchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [Title] [varchar] (150) NULL ,
    [MoreLink] [varchar] (150) NULL ,
    [MobileMoreLink] [varchar] (150) NULL ,
    [ExpireDate] [datetime] NULL ,
    [Description] [varchar] (2000) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Contacts]    Data dello script: 07/11/2002 22.28.04 ******/
CREATE TABLE [Contacts] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [CreatedByUser] [nvarchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [Name] [nvarchar] (50) NULL ,
    [Role] [nvarchar] (100) NULL ,
    [Email] [nvarchar] (100) NULL ,
    [Contact1] [nvarchar] (250) NULL ,
    [Contact2] [nvarchar] (250) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Discussion]    Data dello script: 07/11/2002 22.28.05 ******/
CREATE TABLE [Discussion] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [Title] [nvarchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [Body] [nvarchar] (3000) NULL ,
    [DisplayOrder] [nvarchar] (750) NULL ,
    [CreatedByUser] [nvarchar] (100) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [Documents]    Data dello script: 07/11/2002 22.28.05 ******/
CREATE TABLE [Documents] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [CreatedByUser] [nvarchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [FileNameUrl] [nvarchar] (250) NULL ,
    [FileFriendlyName] [nvarchar] (150) NULL ,
    [Category] [nvarchar] (50) NULL ,
    [Content] [image] NULL ,
    [ContentType] [nvarchar] (50) NULL ,
    [ContentSize] [int] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Oggetto: tabella [Events]    Data dello script: 07/11/2002 22.28.06 ******/
CREATE TABLE [Events] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [CreatedByUser] [nvarchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [Title] [nvarchar] (150) NULL ,
    [WhereWhen] [nvarchar] (150) NULL ,
    [Description] [nvarchar] (2000) NULL ,
    [ExpireDate] [datetime] NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [HtmlText]    Data dello script: 07/11/2002 22.28.06 ******/
CREATE TABLE [HtmlText] (
    [ModuleID] [int] NOT NULL ,
    [DesktopHtml] [ntext] NOT NULL ,
    [MobileSummary] [ntext] NOT NULL ,
    [MobileDetails] [ntext] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Oggetto: tabella [Links]    Data dello script: 07/11/2002 22.28.07 ******/
CREATE TABLE [Links] (
    [ItemID] [int] IDENTITY (1,1) NOT NULL ,
    [ModuleID] [int] NOT NULL ,
    [CreatedByUser] [nvarchar] (100) NULL ,
    [CreatedDate] [datetime] NULL ,
    [Title] [nvarchar] (100) NULL ,
    [Url] [nvarchar] (250) NULL ,
    [MobileUrl] [nvarchar] (250) NULL ,
    [ViewOrder] [int] NULL ,
    [Description] [nvarchar] (2000) NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [ModuleSettings]    Data dello script: 07/11/2002 22.28.07 ******/
CREATE TABLE [ModuleSettings] (
    [ModuleID] [int] NOT NULL ,
    [SettingName] [nvarchar] (50) NOT NULL ,
    [SettingValue] [nvarchar] (1500) NOT NULL 
) ON [PRIMARY]
GO

/****** Oggetto: tabella [UserRoles]    Data dello script: 07/11/2002 22.28.07 ******/
CREATE TABLE [UserRoles] (
    [UserID] [int] NOT NULL ,
    [RoleID] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [Countries] WITH NOCHECK ADD 
    CONSTRAINT [PK_Countries] PRIMARY KEY  CLUSTERED 
    (
        [PK_IDCountry]
    )  ON [PRIMARY] 
GO

ALTER TABLE [GeneralModuleDefinitions] WITH NOCHECK ADD 
    CONSTRAINT [PK_GeneralModuleDefinitions] PRIMARY KEY  CLUSTERED 
    (
        [GeneralModDefID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Layouts] WITH NOCHECK ADD 
    CONSTRAINT [PK_Layouts] PRIMARY KEY  CLUSTERED 
    (
        [LayoutID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Solutions] WITH NOCHECK ADD 
    CONSTRAINT [PK_Solutions] PRIMARY KEY  CLUSTERED 
    (
        [SolutionsID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [PortalSettings] WITH NOCHECK ADD 
    CONSTRAINT [PK_PortalSettings] PRIMARY KEY  CLUSTERED 
    (
        [PortalID],
        [SettingName]
    )  ON [PRIMARY] 
GO

ALTER TABLE [SolutionModuleDefinitions] WITH NOCHECK ADD 
    CONSTRAINT [PK_SolutionModuleDefintions] PRIMARY KEY  CLUSTERED 
    (
        [SolutionModDefID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [States] WITH NOCHECK ADD 
    CONSTRAINT [PK_States] PRIMARY KEY  CLUSTERED 
    (
        [PK_IDState]
    )  ON [PRIMARY] 
GO

ALTER TABLE [GeneralModuleDefinitions] WITH NOCHECK ADD 
    CONSTRAINT [IX_GeneralModuleDefinitions] UNIQUE  NONCLUSTERED 
    (
        [FriendlyName]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Portals] WITH NOCHECK ADD 
    CONSTRAINT [PK_Portals] PRIMARY KEY  NONCLUSTERED 
    (
        [PortalID]
    )  ON [PRIMARY] ,
    CONSTRAINT [IX_Portals] UNIQUE  NONCLUSTERED 
    (
        [PortalAlias]
    )  ON [PRIMARY] 
GO

ALTER TABLE [ModuleDefinitions] WITH NOCHECK ADD 
    CONSTRAINT [PK_ModuleDefinitions] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleDefID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Roles] WITH NOCHECK ADD 
    CONSTRAINT [DF_Roles_Permission] DEFAULT (1) FOR [Permission],
    CONSTRAINT [PK_Roles] PRIMARY KEY  NONCLUSTERED 
    (
        [RoleID]
    )  ON [PRIMARY] ,
    CONSTRAINT [IX_Roles] UNIQUE  NONCLUSTERED 
    (
        [PortalID],
        [RoleName]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Tabs] WITH NOCHECK ADD 
    CONSTRAINT [PK_Tabs] PRIMARY KEY  NONCLUSTERED 
    (
        [TabID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Modules] WITH NOCHECK ADD 
    CONSTRAINT [PK_Modules] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Users] WITH NOCHECK ADD 
    CONSTRAINT [DF_Users_SendNewsletter] DEFAULT (1) FOR [SendNewsletter],
    CONSTRAINT [DF_Users_MailChecked] DEFAULT (0) FOR [MailChecked],
    CONSTRAINT [PK_Users] PRIMARY KEY  NONCLUSTERED 
    (
        [UserID]
    )  ON [PRIMARY] ,
    CONSTRAINT [IX_Users] UNIQUE  NONCLUSTERED 
    (
        [Email],
        [PortalID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Announcements] WITH NOCHECK ADD 
    CONSTRAINT [PK_Announcements] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Contacts] WITH NOCHECK ADD 
    CONSTRAINT [PK_Contacts] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Discussion] WITH NOCHECK ADD 
    CONSTRAINT [PK_Discussion] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Documents] WITH NOCHECK ADD 
    CONSTRAINT [PK_Documents] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Events] WITH NOCHECK ADD 
    CONSTRAINT [PK_Events] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [HtmlText] WITH NOCHECK ADD 
    CONSTRAINT [PK_HtmlText] PRIMARY KEY  NONCLUSTERED 
    (
        [ModuleID]
    )  ON [PRIMARY] 
GO

ALTER TABLE [Links] WITH NOCHECK ADD 
    CONSTRAINT [PK_Links] PRIMARY KEY  NONCLUSTERED 
    (
        [ItemID]
    )  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_PortalSettings] ON [PortalSettings]([PortalID], [SettingName]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ModuleSettings] ON [ModuleSettings]([ModuleID], [SettingName]) ON [PRIMARY]
GO

ALTER TABLE [ModuleDefinitions] ADD 
    CONSTRAINT [FK_ModuleDefinitions_GeneralModuleDefinitions] FOREIGN KEY 
    (
        [GeneralModDefID]
    ) REFERENCES [GeneralModuleDefinitions] (
        [GeneralModDefID]
    ) ON DELETE CASCADE 
GO

ALTER TABLE [PortalSettings] ADD 
    CONSTRAINT [FK_PortalSettings_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [Portals] (
        [PortalID]
    ) ON DELETE CASCADE 
GO

ALTER TABLE [Roles] ADD 
    CONSTRAINT [FK_Roles_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [Portals] (
        [PortalID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [SolutionModuleDefinitions] ADD 
    CONSTRAINT [FK_SolutionModuleDefinitions_GeneralModuleDefinitions] FOREIGN KEY 
    (
        [GeneralModDefID]
    ) REFERENCES [GeneralModuleDefinitions] (
        [GeneralModDefID]
    ),
    CONSTRAINT [FK_SolutionModuleDefintions_Solutions] FOREIGN KEY 
    (
        [SolutionsID]
    ) REFERENCES [Solutions] (
        [SolutionsID]
    )
GO

ALTER TABLE [States] ADD 
    CONSTRAINT [FK_States_Countries] FOREIGN KEY 
    (
        [IDCountry_FK]
    ) REFERENCES [Countries] (
        [PK_IDCountry]
    )
GO

ALTER TABLE [Tabs] ADD 
    CONSTRAINT [FK_Tabs_Layouts] FOREIGN KEY 
    (
        [TabLayout]
    ) REFERENCES [Layouts] (
        [LayoutID]
    ) ON UPDATE CASCADE ,
    CONSTRAINT [FK_Tabs_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [Portals] (
        [PortalID]
    ) ON DELETE CASCADE ,
    CONSTRAINT [FK_Tabs_Tabs] FOREIGN KEY 
    (
        [ParentTabID]
    ) REFERENCES [Tabs] (
        [TabID]
    )
GO

ALTER TABLE [Modules] ADD 
    CONSTRAINT [FK_Modules_ModuleDefinitions1] FOREIGN KEY 
    (
        [ModuleDefID]
    ) REFERENCES [ModuleDefinitions] (
        [ModuleDefID]
    ) ON DELETE CASCADE ,
    CONSTRAINT [FK_Modules_Tabs1] FOREIGN KEY 
    (
        [TabID]
    ) REFERENCES [Tabs] (
        [TabID]
    ) ON DELETE CASCADE 
GO

ALTER TABLE [Users] ADD 
    CONSTRAINT [FK_Users_Countries] FOREIGN KEY 
    (
        [IDCountry_FK]
    ) REFERENCES [Countries] (
        [PK_IDCountry]
    ),
    CONSTRAINT [FK_Users_Portals] FOREIGN KEY 
    (
        [PortalID]
    ) REFERENCES [Portals] (
        [PortalID]
    ) ON DELETE CASCADE ,
    CONSTRAINT [FK_Users_States] FOREIGN KEY 
    (
        [IDState_FK]
    ) REFERENCES [States] (
        [PK_IDState]
    )
GO

ALTER TABLE [Announcements] ADD 
    CONSTRAINT [FK_Announcements_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [Contacts] ADD 
    CONSTRAINT [FK_Contacts_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [Discussion] ADD 
    CONSTRAINT [FK_Discussion_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [Documents] ADD 
    CONSTRAINT [FK_Documents_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [Events] ADD 
    CONSTRAINT [FK_Events_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [HtmlText] ADD 
    CONSTRAINT [FK_HtmlText_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [Links] ADD 
    CONSTRAINT [FK_Links_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [ModuleSettings] ADD 
    CONSTRAINT [FK_ModuleSettings_Modules] FOREIGN KEY 
    (
        [ModuleID]
    ) REFERENCES [Modules] (
        [ModuleID]
    ) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [UserRoles] ADD 
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY 
    (
        [RoleID]
    ) REFERENCES [Roles] (
        [RoleID]
    ),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY 
    (
        [UserID]
    ) REFERENCES [Users] (
        [UserID]
    ) ON DELETE CASCADE 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddAnnouncement    Data dello script: 07/11/2002 22.28.08 ******/


-- =============================================================
-- create the stored procs
-- =============================================================
CREATE PROCEDURE AddAnnouncement
(
    @ModuleID       int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     DateTime,
    @Description    nvarchar(2000),
    @ItemID         int OUTPUT
)
AS

INSERT INTO Announcements
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    MoreLink,
    MobileMoreLink,
    ExpireDate,
    Description
)

VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @MoreLink,
    @MobileMoreLink,
    @ExpireDate,
    @Description
)

SELECT
    @ItemID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddModule    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE AddModule
(
    @TabID                  int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @ModuleDefID            int,
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles		nvarchar(256),
    @ShowMobile             bit,
    @ModuleID               int OUTPUT
)
AS
INSERT INTO Modules
(
    TabID,
    ModuleOrder,
    ModuleTitle,
    PaneName,
    ModuleDefID,
    CacheTime,
    AuthorizedEditRoles,
    AuthorizedAddRoles,
    AuthorizedViewRoles,
    AuthorizedDeleteRoles,
    AuthorizedPropertiesRoles,
    ShowMobile
) 
VALUES
(
    @TabID,
    @ModuleOrder,
    @ModuleTitle,
    @PaneName,
    @ModuleDefID,
    @CacheTime,
    @EditRoles,
    @AddRoles,
    @ViewRoles,
    @DeleteRoles,
    @PropertiesRoles,
    @ShowMobile
)
SELECT 
    @ModuleID = @@IDENTITY

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddModuleDefinition    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE AddModuleDefinition
(
    @GeneralModDefID	uniqueidentifier,
    @FriendlyName	    nvarchar(128),
    @DesktopSrc		    nvarchar(256),
    @MobileSrc		    nvarchar(256),
    @Admin			    bit
)
AS
INSERT INTO GeneralModuleDefinitions
(
    GeneralModDefID,
    FriendlyName,
    DesktopSrc,
    MobileSrc,
    Admin
)
VALUES
(
    @GeneralModDefID,
    @FriendlyName,
    @DesktopSrc,
    @MobileSrc,
    @Admin
)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddTab    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE AddTab
(
    @PortalID   int,
    @TabName    nvarchar(50),
    @TabOrder   int,
    @AuthorizedRoles nvarchar (256),
    @MobileTabName nvarchar(50),
    @TabID      int OUTPUT
)
AS

INSERT INTO Tabs
(
    PortalID,
    TabName,
    TabOrder,
    ShowMobile,
    MobileTabName,
    AuthorizedRoles
)

VALUES
(
    @PortalID,
    @TabName,
    @TabOrder,
    0, /* false */
    @MobileTabName,
    @AuthorizedRoles
)

SELECT
    @TabID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddUser    Data dello script: 07/11/2002 22.28.08 ******/



CREATE PROCEDURE AddUser
(
    @PortalID	int,
    @Name     nvarchar(50),
    @Email    nvarchar(100),
    @Password nvarchar(20),
    @UserID   int OUTPUT
)
AS

INSERT INTO Users
(
    Name,
    Email,
    Password,
    PortalID
)

VALUES
(
    @Name,
    @Email,
    @Password,
    @PortalID
)

SELECT
    @UserID = @@IDENTITY




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddUserFull    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE AddUserFull
(
    @PortalID	    int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address	    nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
    @IDCountry_FK	nchar(2),  
    @IDState_FK		int,
    @UserID		    int OUTPUT
)
AS

INSERT INTO Users
(
    PortalID,
    Name,
    Company,
    Address,		
    City,		
    Zip,		
    Phone,		
    Fax,		
    PIva,		
    CFiscale,	
    Email,		
    Password,
    SendNewsletter,
    IDCountry_FK,
    IDState_FK
)

VALUES
(
    @PortalID,
    @Name,
    @Company,
    @Address,	
    @City,	
    @Zip,	
    @Phone,	
    @Fax,	
    @PIva,	
    @CFiscale,
    @Email,
    @Password,
    @SendNewsletter,
    @IDCountry_FK,
    @IDState_FK
)

SELECT
    @UserID = @@IDENTITY




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteAnnouncement    Data dello script: 07/11/2002 22.28.08 ******/



CREATE PROCEDURE DeleteAnnouncement
(
    @ItemID int
)
AS

DELETE FROM
    Announcements

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteModule    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE DeleteModule
(
    @ModuleID       int
)
AS
DELETE FROM 
    Modules 
WHERE 
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteModuleDefinition    Data dello script: 07/11/2002 22.28.08 ******/
CREATE PROCEDURE DeleteModuleDefinition
(
    @ModuleDefID uniqueidentifier
)
AS
DELETE FROM
    GeneralModuleDefinitions
WHERE
    GeneralModDefID = @ModuleDefID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteTab    Data dello script: 07/11/2002 22.28.08 ******/



CREATE PROCEDURE DeleteTab
(
    @TabID int
)
AS

DELETE FROM
    Tabs

WHERE
    TabID = @TabID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteUser    Data dello script: 07/11/2002 22.28.08 ******/


CREATE PROCEDURE DeleteUser
(
    @UserID int
)
AS

DELETE FROM
    Users

WHERE
    UserID=@UserID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAnnouncements    Data dello script: 07/11/2002 22.28.08 ******/


CREATE PROCEDURE GetAnnouncements
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    CreatedByUser,
    CreatedDate,
    Title,
    MoreLink,
    MobileMoreLink,
    ExpireDate,
    Description

FROM 
    Announcements

WHERE
    ModuleID = @ModuleID
  AND
    ExpireDate > GetDate()



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAuthAddRoles    Data dello script: 07/11/2002 22.28.09 ******/


CREATE PROCEDURE GetAuthAddRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @AddRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = Tabs.AuthorizedRoles,
    @AddRoles   = Modules.AuthorizedAddRoles
    
FROM    
    Modules
  INNER JOIN
    Tabs ON Modules.TabID = Tabs.TabID
    
WHERE   
    Modules.ModuleID = @ModuleID
  AND
    Tabs.PortalID = @PortalID


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAuthDeleteRoles    Data dello script: 07/11/2002 22.28.09 ******/


CREATE PROCEDURE GetAuthDeleteRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = Tabs.AuthorizedRoles,
    @DeleteRoles   = Modules.AuthorizedDeleteRoles
    
FROM    
    Modules
  INNER JOIN
    Tabs ON Modules.TabID = Tabs.TabID
    
WHERE   
    Modules.ModuleID = @ModuleID
  AND
    Tabs.PortalID = @PortalID


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAuthEditRoles    Data dello script: 07/11/2002 22.28.09 ******/


CREATE PROCEDURE GetAuthEditRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = Tabs.AuthorizedRoles,
    @EditRoles   = Modules.AuthorizedEditRoles
    
FROM    
    Modules
  INNER JOIN
    Tabs ON Modules.TabID = Tabs.TabID
    
WHERE   
    Modules.ModuleID = @ModuleID
  AND
    Tabs.PortalID = @PortalID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAuthPropertiesRoles    Data dello script: 07/11/2002 22.28.09 ******/


CREATE PROCEDURE GetAuthPropertiesRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PropertiesRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = Tabs.AuthorizedRoles,
    @PropertiesRoles   = Modules.AuthorizedPropertiesRoles
    
FROM    
    Modules
  INNER JOIN
    Tabs ON Modules.TabID = Tabs.TabID
    
WHERE   
    Modules.ModuleID = @ModuleID
  AND
    Tabs.PortalID = @PortalID


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetAuthViewRoles    Data dello script: 07/11/2002 22.28.09 ******/


CREATE PROCEDURE GetAuthViewRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = Tabs.AuthorizedRoles,
    @ViewRoles   = Modules.AuthorizedViewRoles
    
FROM    
    Modules
  INNER JOIN
    Tabs ON Modules.TabID = Tabs.TabID
    
WHERE   
    Modules.ModuleID = @ModuleID
  AND
    Tabs.PortalID = @PortalID


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetCurrentModuleDefinitions    Data dello script: 07/11/2002 22.28.09 ******/
/* returns all module definitions for the specified portal */
CREATE PROCEDURE GetCurrentModuleDefinitions
(
    @PortalID  int
)
AS
SELECT  
    GeneralModuleDefinitions.FriendlyName,
    GeneralModuleDefinitions.DesktopSrc,
    GeneralModuleDefinitions.MobileSrc,
    ModuleDefinitions.ModuleDefID
FROM
    ModuleDefinitions
INNER JOIN
    GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
WHERE   
    ModuleDefinitions.PortalID = @PortalID
ORDER BY
GeneralModuleDefinitions.Admin, GeneralModuleDefinitions.FriendlyName

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetGeneralModuleDefinitionByName    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE
GetGeneralModuleDefinitionByName
(
    @FriendlyName nvarchar(128),
    @ModuleID uniqueidentifier OUTPUT
)
AS

SELECT @ModuleID =
(
SELECT  GeneralModuleDefinitions.GeneralModDefID
FROM    GeneralModuleDefinitions
WHERE   (GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModuleDefinitions    Data dello script: 07/11/2002 22.28.09 ******/
/* returns all module definitions for the specified portal */
CREATE PROCEDURE GetModuleDefinitions

AS
SELECT     GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc
FROM         GeneralModuleDefinitions
ORDER BY Admin, FriendlyName


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortals    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE GetPortals

AS

SELECT  Portals.PortalID, Portals.PortalAlias, Portals.PortalName, Portals.PortalPath, Portals.AlwaysShowEditButton
FROM    Portals
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleAnnouncement    Data dello script: 07/11/2002 22.28.09 ******/



CREATE PROCEDURE GetSingleAnnouncement
(
    @ItemID int
)
AS

SELECT
    CreatedByUser,
    CreatedDate,
    Title,
    MoreLink,
    MobileMoreLink,
    ExpireDate,
    Description

FROM
    Announcements

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleArticle    Data dello script: 07/11/2002 22.28.09 ******/

CREATE PROCEDURE GetSingleArticle
(
    @ItemID int
)
AS

SELECT		ItemID,
            ModuleID,
            CreatedByUser,
            CreatedDate,
            Title, 
            Subtitle, 
            Abstract, 
            Description, 
            StartDate, 
            ExpireDate, 
            IsInNewsletter, 
            MoreLink, 
            TemplateXSLT, 
            PhotoId1, 
            PhotoId2, 
            PhotoId3
FROM	Articles
WHERE   (ItemID = @ItemID)




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleArticleWithImages    Data dello script: 07/11/2002 22.28.09 ******/

CREATE PROCEDURE GetSingleArticleWithImages
(
    @ItemID int,
    @Variation varchar(50)
)
AS


SELECT *
FROM
(

SELECT		Articles.ItemID, 
            Articles.ModuleID, 
            Articles.CreatedByUser, 
            Articles.CreatedDate, 
            Articles.Title, 
            Articles.Subtitle, 
            Articles.Abstract, 
            Articles.Description, 
            Articles.StartDate, 
            Articles.ExpireDate, 
            Articles.IsInNewsletter, 
            Articles.MoreLink, 
            Articles.TemplateXSLT, 
            ImageVariations.ImageURL AS Image1, 
            ImageVariations_1.ImageURL AS Image2, 
            ImageVariations_2.ImageURL AS Image3, 
            ImageVariations.Variation AS Variation1, 
            ImageVariations_1.Variation AS Variation2, 
            ImageVariations_2.Variation AS Variation3
            
FROM        Images Images_2 
            INNER JOIN
            ImageVariations ImageVariations_2 ON Images_2.ItemID = ImageVariations_2.ImageID RIGHT OUTER JOIN
            Articles ON Images_2.ItemID = Articles.PhotoId3 LEFT OUTER JOIN
            ImageVariations INNER JOIN
            Images ON ImageVariations.ImageID = Images.ItemID ON Articles.PhotoId1 = Images.ItemID LEFT OUTER JOIN
            ImageVariations ImageVariations_1 INNER JOIN
            Images Images_1 ON ImageVariations_1.ImageID = Images_1.ItemID ON Articles.PhotoId2 = Images_1.ItemID
WHERE     (Articles.ItemID = @ItemID)

) Articles

WHERE (Variation1 IS null or Variation1 = @Variation) AND (Variation2 IS null or Variation2 = @Variation) AND (Variation3 IS null or Variation3 = @Variation)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleModuleDefinition    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE GetSingleModuleDefinition
(
    @GeneralModDefID uniqueidentifier
)
AS
SELECT
    GeneralModDefID, 
    FriendlyName,
    DesktopSrc,
    MobileSrc,
    Admin
FROM
    GeneralModuleDefinitions
WHERE
    GeneralModDefID = @GeneralModDefID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSolutionModuleDefinitions    Data dello script: 07/11/2002 22.28.09 ******/
/* returns all module definitions for a specified solution */
CREATE PROCEDURE GetSolutionModuleDefinitions
(
    @SolutionID  int
)
AS
SELECT *
 
FROM
    SolutionModuleDefinitions
WHERE   
    SolutionsID = @SolutionID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTabSettings    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE GetTabSettings
(
    @TabID   int
)
AS

IF (@TabID > 0)

/* Get Tabs list */
SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
FROM         Tabs
WHERE     (ParentTabID = @TabID)
ORDER BY TabOrder

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTabsByPortal    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE GetTabsByPortal
(
    @PortalID   int
)
AS

/* Get Tabs list */
SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
FROM         Tabs
WHERE     (PortalID = @PortalID)
ORDER BY TabOrder

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetUsers    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE GetUsers
(
@PortalID int
)
AS

SELECT     UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, IDCountry_FK, IDState_FK, PIva, CFiscale, Phone, Fax
FROM         Users
WHERE     (PortalID = @PortalID)
ORDER BY Email




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateAnnouncement    Data dello script: 07/11/2002 22.28.10 ******/



CREATE PROCEDURE UpdateAnnouncement
(
    @ItemID         int,
    @UserName       nvarchar(100),
    @Title          nvarchar(150),
    @MoreLink       nvarchar(150),
    @MobileMoreLink nvarchar(150),
    @ExpireDate     datetime,
    @Description    nvarchar(2000)
)
AS

UPDATE
    Announcements

SET
    CreatedByUser   = @UserName,
    CreatedDate     = GetDate(),
    Title           = @Title,
    MoreLink        = @MoreLink,
    MobileMoreLink  = @MobileMoreLink,
    ExpireDate      = @ExpireDate,
    Description     = @Description

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateModuleDefinition    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE UpdateModuleDefinition
(
    @GeneralModDefID	uniqueidentifier,
    @FriendlyName		nvarchar(128),
    @DesktopSrc			nvarchar(256),
    @Admin				bit,
    @MobileSrc			nvarchar(256)
)
AS
UPDATE
    GeneralModuleDefinitions
SET
    FriendlyName = @FriendlyName,
    DesktopSrc   = @DesktopSrc,
    MobileSrc    = @MobileSrc,
    Admin		 = @Admin
WHERE
    GeneralModDefID =  @GeneralModDefID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateModuleOrder    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE UpdateModuleOrder
(
    @ModuleID           int,
    @ModuleOrder        int,
    @PaneName           nvarchar(50)
)
AS
UPDATE
    Modules
SET
    ModuleOrder = @ModuleOrder,
    PaneName    = @PaneName
WHERE
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateModuleSetting    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE UpdateModuleSetting
(
    @ModuleID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(256)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        ModuleSettings 
    WHERE 
        ModuleID = @ModuleID
      AND
        SettingName = @SettingName
)
INSERT INTO ModuleSettings (
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
    ModuleSettings
SET
    SettingValue = @SettingValue
WHERE
    ModuleID = @ModuleID
  AND
    SettingName = @SettingName

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdatePortalInfo    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE UpdatePortalInfo
(
    @PortalID           int,
    @PortalName         nvarchar(128),
    @PortalPath         nvarchar(128),
    @AlwaysShowEditButton bit 
)
AS

UPDATE
    Portals

SET
    PortalName = @PortalName,
    PortalPath = @PortalPath,
    AlwaysShowEditButton = @AlwaysShowEditButton

WHERE
    PortalID = @PortalID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateTabOrder    Data dello script: 07/11/2002 22.28.10 ******/


CREATE PROCEDURE UpdateTabOrder
(
    @TabID           int,
    @TabOrder        int
)
AS

UPDATE
    Tabs

SET
    TabOrder = @TabOrder

WHERE
    TabID = @TabID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateUserFull    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE UpdateUserFull
(
    @UserID		    int,
    @PortalID       int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter	bit,
    @IDCountry_FK	nchar(2),  
    @IDState_FK		int
)
AS

UPDATE Users

SET

PortalID = @PortalID,
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
Email = @Email,		
Password = @Password,
SendNewsletter = @SendNewsletter,
IDCountry_FK = @IDCountry_FK,
IDState_FK = @IDState_FK

WHERE UserID = @UserID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UserLogin    Data dello script: 07/11/2002 22.28.10 ******/


CREATE PROCEDURE UserLogin
(
    @PortalID int,
    @Email    nvarchar(100),
    @Password nvarchar(20),
    @UserName nvarchar(100) OUTPUT
)
AS

SELECT     @UserName = Users.Name
FROM      Users
WHERE
    (
    Users.Email = @Email AND Users.Password = @Password AND Users.PortalID = @PortalID
    )



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddPortal    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE AddPortal
(
    @PortalAlias            nvarchar(128),
    @PortalName             nvarchar(128),
    @PortalPath             nvarchar(128),
    @AlwaysShowEditButton   bit,
    @PortalID               int OUTPUT
)
AS

INSERT INTO Portals
(
    PortalAlias,
    PortalName,
    PortalPath,
    AlwaysShowEditButton
)

VALUES
(
    @PortalAlias,
    @PortalName,
    @PortalPath,
    @AlwaysShowEditButton
)

SELECT
    @PortalID = @@IDENTITY

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeletePortal    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE DeletePortal
(
    @PortalID       int
)
AS
DELETE FROM 
    Portals 
WHERE 
    PortalID = @PortalID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetCountries    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE GetCountries
(
    @IDLang	nchar(2) = 'IT'
)

AS

SELECT
PK_IdCountry,
CASE @IDLang
    WHEN 'IT' THEN Countries.IT
    WHEN 'EN' THEN Countries.EN
    WHEN 'DE' THEN Countries.DE
    WHEN 'FR' THEN Countries.FR
    WHEN 'ES' THEN Countries.ES
    WHEN 'PT' THEN Countries.PT
    ELSE Countries.EN
END AS Description
FROM	countries
ORDER BY Description
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetCountriesFiltered    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE GetCountriesFiltered
(
    @IDLang	nchar(2) = 'IT',
    @Filter nvarchar(1000)
)

AS

IF (@IDLang = 'IT')
BEGIN
    SELECT	PK_IdCountry, IT AS Description
    FROM    countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END

IF (@IDLang = 'EN')
BEGIN
    SELECT	PK_IdCountry, EN AS Description
    FROM	countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END

IF (@IDLang = 'FR')
BEGIN
    SELECT	PK_IdCountry, FR AS Description
    FROM	countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END

IF (@IDLang = 'ES')
BEGIN
    SELECT	PK_IdCountry, ES AS Description
    FROM	countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END

IF (@IDLang = 'DE')
BEGIN
    SELECT	PK_IdCountry, DE AS Description
    FROM	countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END

IF (@IDLang = 'PT')
BEGIN
    SELECT	PK_IdCountry, PT AS Description
    FROM	countries
    WHERE  @Filter LIKE '%' + PK_IdCountry + '%'
    ORDER BY Description
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortalSettingsPortalID    Data dello script: 07/11/2002 22.28.10 ******/

CREATE PROCEDURE GetPortalSettingsPortalID
(
    @PortalID   nvarchar(50)
)
AS
    SELECT     TOP 1 PortalID, PortalName, PortalPath, AlwaysShowEditButton, PortalAlias
    FROM         Portals
    WHERE     (PortalID = @PortalID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Oggetto: stored procedure GetSolutions    Data dello script: 07/11/2002 22.28.10 ******/
CREATE PROCEDURE 

GetSolutions

AS

SELECT * FROM Solutions
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddRole    Data dello script: 07/11/2002 22.28.10 ******/



CREATE PROCEDURE AddRole
(
    @PortalID    int,
    @RoleName    nvarchar(50),
    @RoleID      int OUTPUT
)
AS

INSERT INTO Roles
(
    PortalID,
    RoleName
)

VALUES
(
    @PortalID,
    @RoleName
)

SELECT
    @RoleID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteRole    Data dello script: 07/11/2002 22.28.11 ******/


CREATE PROCEDURE DeleteRole
(
    @RoleID int
)
AS

DELETE FROM
    Roles

WHERE
    RoleID = @RoleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModuleDefinitionByName    Data dello script: 07/11/2002 22.28.11 ******/

CREATE PROCEDURE
GetModuleDefinitionByName
(
    @PortalID int,
    @FriendlyName nvarchar(128),
    @ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     ModuleDefinitions.ModuleDefID
    FROM       GeneralModuleDefinitions LEFT JOIN
                    ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID
    WHERE      (ModuleDefinitions.PortalID = @PortalID) AND (GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModuleInUse    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetModuleInUse
(
    @ModuleID uniqueidentifier
)
AS
SELECT     Portals.PortalID, Portals.PortalAlias, Portals.PortalName, '1' AS Checked
FROM         Portals LEFT OUTER JOIN
                      ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
WHERE     (ModuleDefinitions.GeneralModDefID = @ModuleID)

UNION

SELECT DISTINCT
    PortalID, PortalAlias, PortalName, '0' AS Checked
FROM   Portals
WHERE  
(
PortalID NOT IN
    (SELECT     Portals.PortalID
     FROM       Portals LEFT OUTER JOIN ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
     WHERE      (ModuleDefinitions.GeneralModDefID = @ModuleID)
    )
)

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortalCustomSettings    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetPortalCustomSettings
(
    @PortalID int
)
AS
SELECT
    SettingName,
    SettingValue
FROM
    PortalSettings
WHERE
    PortalID = @PortalID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortalRoles    Data dello script: 07/11/2002 22.28.11 ******/



/* returns all roles for the specified portal */
CREATE PROCEDURE GetPortalRoles
(
    @PortalID  int
)
AS

SELECT  
    RoleName,
    RoleID

FROM
    Roles

WHERE   
    PortalID = @PortalID

order by RoleID
/* questo assicura che l'ultimo inserito si infondo alla lista */





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortalsModules    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetPortalsModules
(
    @ModuleID  uniqueidentifier
)
AS
    SELECT     Portals.PortalID, Portals.PortalAlias, Portals.PortalName, ModuleDefinitions.ModuleDefID
    FROM         Portals LEFT OUTER JOIN
                          ModuleDefinitions ON Portals.PortalID = ModuleDefinitions.PortalID
    WHERE     (ModuleDefinitions.GeneralModDefID = @ModuleID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleCountry    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetSingleCountry
(
    @IDState int,
    @IDLang	nchar(2) = 'IT'
)

AS
SELECT
Countries.PK_IDCountry,
CASE @IDLang
    WHEN 'IT' THEN Countries.IT
    WHEN 'EN' THEN Countries.EN
    WHEN 'DE' THEN Countries.DE
    WHEN 'FR' THEN Countries.FR
    WHEN 'ES' THEN Countries.ES
    WHEN 'PT' THEN Countries.PT
    ELSE Countries.EN
END AS Description
FROM States INNER JOIN
     Countries ON States.IDCountry_FK = Countries.PK_IDCountry
WHERE     (States.PK_IDState = @IdState) 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleRole    Data dello script: 07/11/2002 22.28.11 ******/



CREATE PROCEDURE GetSingleRole
(
    @RoleID int
)
AS

SELECT
    RoleName

FROM
    Roles

WHERE
    RoleID = @RoleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetStates    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetStates
(
    @IDCountry_FK nchar(2)
)

AS
SELECT  Description, 
        PK_IDState
FROM    States
WHERE	IDCountry_FK = @IDCountry_FK
ORDER BY Description 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTabsFlat    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetTabsFlat
(
    @PortalID int
)
AS

SELECT    TabID, TabName, ParentTabID, TabOrder * 10000 TabOrder, 0 NestLevel
FROM      Tabs
WHERE     (PortalID = @PortalID) AND (ParentTabID IS NULL)

union

SELECT     Tabs.TabID, '--' + Tabs.TabName AS TabName, Tabs.ParentTabID, Tabs_1.TabOrder * 10000 + Tabs.TabOrder * 100 TabOrder, 1 NestLevel
FROM         Tabs INNER JOIN
                      Tabs Tabs_1 ON Tabs.ParentTabID = Tabs_1.TabID
WHERE     (Tabs.PortalID = @PortalID) AND (Tabs_1.ParentTabID IS NULL)

union

SELECT     Tabs.TabID, '----' + Tabs.TabName AS TabName, Tabs.ParentTabID, 
                      Tabs_2.TabOrder * 10000 + Tabs_1.TabOrder * 100 + Tabs.TabOrder AS TabOrder, 2 AS NestLevel
FROM         Tabs INNER JOIN
                      Tabs Tabs_1 ON Tabs.ParentTabID = Tabs_1.TabID INNER JOIN
                      Tabs Tabs_2 ON Tabs_1.ParentTabID = Tabs_2.TabID
WHERE     (Tabs.PortalID = @PortalID) AND (Tabs_2.ParentTabID IS NULL)


ORDER BY Tabs.TabOrder


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTabsParent    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetTabsParent
(
    @PortalID int,
    @TabID int
)
AS
SELECT 0 TabID, ' Livello principale' TabName

UNION

SELECT     Tabs.TabID, Tabs.TabName
FROM       Tabs
WHERE     (Tabs.TabID <> @TabID) AND (Tabs.PortalID = @PortalID)
ORDER BY Tabs.TabName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTabsinTab    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE GetTabsinTab
(
    @PortalID int,
    @TabID int
)
AS
SELECT     TabID, TabName, ParentTabID, TabOrder, AuthorizedRoles
FROM         Tabs
WHERE     (ParentTabID = @TabID) AND (PortalID = @PortalID)
ORDER BY TabOrder
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateModuleDefinitions    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE UpdateModuleDefinitions
(
    @GeneralModDefID	uniqueidentifier,
    @PortalID			int,
    @ischecked			bit
)
AS

IF (@ischecked = 0)
    /*DELETE IF CLEARED */
    DELETE FROM ModuleDefinitions WHERE ModuleDefinitions.GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID
    
ELSE
IF NOT (EXISTS (SELECT ModuleDefID FROM ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID))
    /* ADD IF CHECKED */
BEGIN
            INSERT INTO ModuleDefinitions
            (
                PortalID,
                GeneralModDefID
            )
            VALUES
            (
                @PortalID,
                @GeneralModDefID
            )
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdatePortalSetting    Data dello script: 07/11/2002 22.28.11 ******/
CREATE PROCEDURE UpdatePortalSetting
(
    @PortalID      int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(256)
)
AS
IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        PortalSettings 
    WHERE 
        PortalID = @PortalID
      AND
        SettingName = @SettingName
)
INSERT INTO PortalSettings (
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
    PortalSettings
SET
    SettingValue = @SettingValue
WHERE
    PortalID = @PortalID
  AND
    SettingName = @SettingName

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateRole    Data dello script: 07/11/2002 22.28.11 ******/


CREATE PROCEDURE UpdateRole
(
    @RoleID      int,
    @RoleName    nvarchar(50)
)
AS

UPDATE
    Roles

SET
    RoleName = @RoleName

WHERE
    RoleID = @RoleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateTab    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE UpdateTab
(
    @PortalID        int,
    @TabID           int,
    @ParentTabID     int,
    @TabOrder        int,
    @TabName         nvarchar(50),
    @AuthorizedRoles nvarchar(256),
    @MobileTabName   nvarchar(50),
    @ShowMobile      bit
)
AS

IF (@ParentTabID = 0) 
    SET @ParentTabID = NULL

IF NOT EXISTS
(
    SELECT 
        * 
    FROM 
        Tabs
    WHERE 
        TabID = @TabID
)
INSERT INTO Tabs (
    PortalID,
    ParentTabID,
    TabOrder,
    TabName,
    AuthorizedRoles,
    MobileTabName,
    ShowMobile
) 
VALUES (
    @PortalID,
    @TabOrder,
    @ParentTabID,
    @TabName,
    @AuthorizedRoles,
    @MobileTabName,
    @ShowMobile
   
)
ELSE
UPDATE
    Tabs
SET
    ParentTabID = @ParentTabID,
    TabOrder = @TabOrder,
    TabName = @TabName,
    AuthorizedRoles = @AuthorizedRoles,
    MobileTabName = @MobileTabName,
    ShowMobile = @ShowMobile
WHERE
    TabID = @TabID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModuleDefinitionByID    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE
GetModuleDefinitionByID
(
    @ModuleID int
)
AS


SELECT     ModuleDefinitions.ModuleDefID, ModuleDefinitions.PortalID, GeneralModuleDefinitions.FriendlyName, GeneralModuleDefinitions.DesktopSrc, 
                      GeneralModuleDefinitions.MobileSrc, GeneralModuleDefinitions.Admin, Modules.ModuleID
FROM         GeneralModuleDefinitions INNER JOIN
                      ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID INNER JOIN
                      Modules ON ModuleDefinitions.ModuleDefID = Modules.ModuleDefID
WHERE     (Modules.ModuleID = @ModuleID)


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModulesAllPortals    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE GetModulesAllPortals
AS

SELECT      0 AS ModuleID, 'Nessun modulo' AS ModuleTitle, '' as PortalAlias

UNION

    SELECT     Modules.ModuleID, Portals.PortalAlias + '\' + Modules.ModuleTitle AS ModuleTitle, PortalAlias
    FROM         Modules INNER JOIN
                          Tabs ON Modules.TabID = Tabs.TabID INNER JOIN
                          Portals ON Tabs.PortalID = Portals.PortalID INNER JOIN
                          ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
                          GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
    WHERE     (Modules.ModuleID > 0) AND (GeneralModuleDefinitions.Admin = 0)

ORDER BY PortalAlias, Modules.ModuleTitle


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModulesByName    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE GetModulesByName
(
    @ModuleName varchar(128),
    @PortalID int
)
AS

SELECT      0 ModuleID, ' Nessun modulo' ModuleTitle

UNION

SELECT     Modules.ModuleID, Modules.ModuleTitle
FROM         GeneralModuleDefinitions INNER JOIN
                      ModuleDefinitions ON GeneralModuleDefinitions.GeneralModDefID = ModuleDefinitions.GeneralModDefID INNER JOIN
                      Modules ON ModuleDefinitions.ModuleDefID = Modules.ModuleDefID
WHERE     (ModuleDefinitions.PortalID = @PortalID) AND (GeneralModuleDefinitions.FriendlyName = @ModuleName)

ORDER BY Modules.ModuleTitle
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModulesSinglePortal    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE GetModulesSinglePortal
(
    @PortalID  int
)
AS

SELECT      0 ModuleID, ' Nessun modulo' ModuleTitle

UNION

    SELECT     Modules.ModuleID, Modules.ModuleTitle
    FROM         Modules INNER JOIN
                          Tabs ON Modules.TabID = Tabs.TabID
    WHERE     (Tabs.PortalID = @PortalID)
    ORDER BY Modules.ModuleTitle

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPortalSettings    Data dello script: 07/11/2002 22.28.12 ******/

CREATE PROCEDURE GetPortalSettings
(
    @PortalAlias   nvarchar(50),
    @TabID         int,
    @PortalID      int OUTPUT,
    @PortalName    nvarchar(128) OUTPUT,
    @PortalPath    nvarchar(128) OUTPUT,
    @AlwaysShowEditButton bit OUTPUT,
    @TabName       nvarchar (50)  OUTPUT,
    @TabOrder      int OUTPUT,
    @ParentTabID      int OUTPUT,
    @MobileTabName nvarchar (50)  OUTPUT,
    @AuthRoles     nvarchar (256) OUTPUT,
    @ShowMobile    bit OUTPUT
)
AS
/* First, get Out Params */
IF @TabID = 0 
    SELECT TOP 1
        @PortalID      = Portals.PortalID,
        @PortalName    = Portals.PortalName,
        @PortalPath    = Portals.PortalPath,
        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
        @TabID         = Tabs.TabID,
        @TabOrder      = Tabs.TabOrder,
        @ParentTabID   = Tabs.ParentTabID,
        @TabName       = Tabs.TabName,
        @MobileTabName = Tabs.MobileTabName,
        @AuthRoles     = Tabs.AuthorizedRoles,
        @ShowMobile    = Tabs.ShowMobile
    FROM
        Tabs
    INNER JOIN
        Portals ON Tabs.PortalID = Portals.PortalID
        
    WHERE
        PortalAlias=@PortalAlias
        
    ORDER BY
        TabOrder
ELSE 
    SELECT
        @PortalID      = Portals.PortalID,
        @PortalName    = Portals.PortalName,
        @PortalPath    = Portals.PortalPath,
        @AlwaysShowEditButton = Portals.AlwaysShowEditButton,
        @TabName       = Tabs.TabName,
        @TabOrder      = Tabs.TabOrder,
        @ParentTabID   = Tabs.ParentTabID,
        @MobileTabName = Tabs.MobileTabName,
        @AuthRoles     = Tabs.AuthorizedRoles,
        @ShowMobile    = Tabs.ShowMobile
    FROM
        Tabs
    INNER JOIN
        Portals ON Tabs.PortalID = Portals.PortalID
        
    WHERE
        TabID=@TabID AND Portals.PortalAlias=@PortalAlias

/* Get Tabs list */
SELECT  
    TabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    TabOrder    
FROM    
    Tabs
    
WHERE   
    PortalID = @PortalID AND ParentTabID IS NULL
    
ORDER BY
    TabOrder
    
/* Get Mobile Tabs list */
SELECT  
    MobileTabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    ShowMobile  
FROM    
    Tabs
    
WHERE   
    PortalID = @PortalID
  AND
    ShowMobile = 1
    
ORDER BY
    TabOrder
/* Then, get the DataTable of module info */
SELECT     *
FROM         Modules INNER JOIN
                      ModuleDefinitions ON Modules.ModuleDefID = ModuleDefinitions.ModuleDefID INNER JOIN
                      GeneralModuleDefinitions ON ModuleDefinitions.GeneralModDefID = GeneralModuleDefinitions.GeneralModDefID
WHERE     (Modules.TabID = @TabID)
ORDER BY Modules.ModuleOrder

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleUser    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
    @IDLang	nchar(2) = 'IT'
)
AS

SELECT
    Users.UserID,
    Users.Email,
    Users.Password,
    Users.Name,
    Users.Company,
    Users.Address,
    Users.City,
    Users.Zip,
    Users.IDCountry_FK,
    Users.IDState_FK,
    Users.PIva,
    Users.CFiscale,
    Users.Phone,
    Users.Fax,
    Users.SendNewsletter,
    Users.MailChecked,
    Users.PortalID,
    States.Description AS State, 
    CASE @IDLang
        WHEN 'IT' THEN Countries.IT
        WHEN 'EN' THEN Countries.EN
        WHEN 'DE' THEN Countries.DE
        WHEN 'FR' THEN Countries.FR
        WHEN 'ES' THEN Countries.ES
        WHEN 'PT' THEN Countries.PT
        ELSE Countries.EN
    END AS Country
                      
FROM 
    Users LEFT OUTER JOIN
    Countries ON Users.IDCountry_FK = Countries.PK_IDCountry LEFT OUTER JOIN
    States ON Users.IDState_FK = States.PK_IDState
    
WHERE
(Users.Email = @Email) AND (Users.PortalID = @PortalID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetUsersCount    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE GetUsersCount
(
    @PortalID		int,
    @UsersCount		int OUTPUT
)
AS

SELECT TOP 1
@UsersCount = COUNT(DISTINCT Users.UserID)
FROM  Users
WHERE Users.PortalID = @PortalID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateModule    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE UpdateModule
(
    @ModuleID               int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles        nvarchar(256),
    @ShowMobile             bit
)
AS
UPDATE
    Modules
SET
    ModuleOrder             = @ModuleOrder,
    ModuleTitle             = @ModuleTitle,
    PaneName                = @PaneName,
    CacheTime               = @CacheTime,
    ShowMobile              = @ShowMobile,
    AuthorizedEditRoles     = @EditRoles,
    AuthorizedAddRoles      = @AddRoles,
    AuthorizedViewRoles     = @ViewRoles,
    AuthorizedDeleteRoles   = @DeleteRoles,
    AuthorizedPropertiesRoles = @PropertiesRoles
    
WHERE
    ModuleID = @ModuleID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateUser    Data dello script: 07/11/2002 22.28.12 ******/
CREATE PROCEDURE UpdateUser
(
    @PortalID		int,
    @UserID         int,
    @Name			nvarchar(50),
    @Email          nvarchar(100),
    @Password	    nvarchar(20),
    @SendNewsletter bit
)
AS

UPDATE
    Users

SET
    Name	 = @Name,
    Email    = @Email,
    Password = @Password,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddContact    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE AddContact
(
    @ModuleID int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250),
    @ItemID   int OUTPUT
)
AS

INSERT INTO Contacts
(
    CreatedByUser,
    CreatedDate,
    ModuleID,
    Name,
    Role,
    Email,
    Contact1,
    Contact2
)

VALUES
(
    @UserName,
    GetDate(),
    @ModuleID,
    @Name,
    @Role,
    @Email,
    @Contact1,
    @Contact2
)

SELECT
    @ItemID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE AddEvent
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  DateTime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100),
    @ItemID      int OUTPUT
)
AS

INSERT INTO Events
(
    ModuleID,
    CreatedByUser,
    Title,
    CreatedDate,
    ExpireDate,
    Description,
    WhereWhen
)

VALUES
(
    @ModuleID,
    @UserName,
    @Title,
    GetDate(),
    @ExpireDate,
    @Description,
    @WhereWhen
)

SELECT
    @ItemID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddLink    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE AddLink
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @ItemID      int OUTPUT
)
AS

INSERT INTO Links
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    MobileUrl,
    ViewOrder,
    Description
)
VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @Url,
    @MobileUrl,
    @ViewOrder,
    @Description
)

SELECT
    @ItemID = @@IDENTITY



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure AddUserRole    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE AddUserRole
(
    @UserID int,
    @RoleID int
)
AS

SELECT 
    *
FROM
    UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID

/* only insert if the record doesn't yet exist */
IF @@Rowcount < 1

    INSERT INTO UserRoles
    (
        UserID,
        RoleID
    )

    VALUES
    (
        @UserID,
        @RoleID
    )



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteContact    Data dello script: 07/11/2002 22.28.12 ******/


CREATE PROCEDURE DeleteContact
(
    @ItemID int
)
AS

DELETE FROM
    Contacts

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteDocument    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE DeleteDocument
(
    @ItemID int
)
AS

DELETE FROM
    Documents

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteEvent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE DeleteEvent
(
    @ItemID int
)
AS

DELETE FROM
    Events

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteLink    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE DeleteLink
(
    @ItemID int
)
AS

DELETE FROM
    Links

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure DeleteUserRole    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE DeleteUserRole
(
    @UserID int,
    @RoleID int
)
AS

DELETE FROM
    UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetContacts    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetContacts
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    CreatedDate,
    CreatedByUser,
    Name,
    Role,
    Email,
    Contact1,
    Contact2

FROM
    Contacts

WHERE
    ModuleID = @ModuleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetDocumentContent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetDocumentContent
(
    @ItemID int
)
AS

SELECT
    Content,
    ContentType,
    ContentSize,
    FileFriendlyName

FROM
    Documents

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetDocuments    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetDocuments
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    FileFriendlyName,
    FileNameUrl,
    CreatedByUser,
    CreatedDate,
    Category,
    ContentSize
    
FROM
    Documents

WHERE
    ModuleID = @ModuleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetEvents
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    Title,
    CreatedByUser,
    WhereWhen,
    CreatedDate,
    Title,
    ExpireDate,
    Description

FROM
    Events

WHERE
    ModuleID = @ModuleID
  AND
    ExpireDate > GetDate()



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetHtmlText    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetHtmlText
(
    @ModuleID int
)
AS

SELECT
    *

FROM
    HtmlText

WHERE
    ModuleID = @ModuleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetLinks    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetLinks
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    ViewOrder,
    Description

FROM
    Links

WHERE
    ModuleID = @ModuleID

ORDER BY
    ViewOrder



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetModuleSettings    Data dello script: 07/11/2002 22.28.13 ******/
CREATE PROCEDURE GetModuleSettings
(
    @ModuleID int
)
AS
SELECT     SettingName, SettingValue
FROM         ModuleSettings
WHERE     (ModuleID = @ModuleID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetNextMessageID    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetNextMessageID
(
    @ItemID int,
    @NextID int OUTPUT
)
AS

DECLARE @CurrentDisplayOrder as nvarchar(750)
DECLARE @CurrentModule as int

/* Find DisplayOrder of current item */
SELECT
    @CurrentDisplayOrder = DisplayOrder,
    @CurrentModule = ModuleID
FROM
    Discussion
WHERE
    ItemID = @ItemID

/* Get the next message in the same module */
SELECT Top 1
    @NextID = ItemID

FROM
    Discussion

WHERE
    DisplayOrder > @CurrentDisplayOrder
    AND
    ModuleID = @CurrentModule

ORDER BY
    DisplayOrder ASC

/* end of this thread? */
IF @@Rowcount < 1
    SET @NextID = null



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetPrevMessageID    Data dello script: 07/11/2002 22.28.13 ******/


CREATE PROCEDURE GetPrevMessageID
(
    @ItemID int,
    @PrevID int OUTPUT
)
AS

DECLARE @CurrentDisplayOrder as nvarchar(750)
DECLARE @CurrentModule as int

/* Find DisplayOrder of current item */
SELECT
    @CurrentDisplayOrder = DisplayOrder,
    @CurrentModule = ModuleID
FROM
    Discussion
WHERE
    ItemID = @ItemID

/* Get the previous message in the same module */
SELECT Top 1
    @PrevID = ItemID

FROM
    Discussion

WHERE
    DisplayOrder < @CurrentDisplayOrder
    AND
    ModuleID = @CurrentModule

ORDER BY
    DisplayOrder DESC

/* already at the beginning of this module? */
IF @@Rowcount < 1
    SET @PrevID = null



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetRoleMembership    Data dello script: 07/11/2002 22.28.13 ******/



/* returns all members for the specified role */
CREATE PROCEDURE GetRoleMembership
(
    @RoleID  int
)
AS

SELECT  
    UserRoles.UserID,
    Name,
    Email

FROM
    UserRoles
    
INNER JOIN 
    Users On Users.UserID = UserRoles.UserID

WHERE   
    UserRoles.RoleID = @RoleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetRolesByUser    Data dello script: 07/11/2002 22.28.13 ******/
/* returns all roles for the specified user */
CREATE PROCEDURE GetRolesByUser
(
    @PortalID		int,
    @Email         nvarchar(100)
)
AS

SELECT  
    Roles.RoleName,
    Roles.RoleID

FROM
    UserRoles
  INNER JOIN 
    Users ON UserRoles.UserID = Users.UserID
  INNER JOIN 
    Roles ON UserRoles.RoleID = Roles.RoleID

WHERE   
    Users.Email = @Email AND Users.PortalID = @PortalID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleContact    Data dello script: 07/11/2002 22.28.14 ******/



CREATE PROCEDURE GetSingleContact
(
    @ItemID int
)
AS

SELECT
    CreatedByUser,
    CreatedDate,
    Name,
    Role,
    Email,
    Contact1,
    Contact2

FROM
    Contacts

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleDocument    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE GetSingleDocument
(
    @ItemID int
)
AS

SELECT
    FileFriendlyName,
    FileNameUrl,
    CreatedByUser,
    CreatedDate,
    Category,
    ContentSize

FROM
    Documents

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleEvent    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE GetSingleEvent
(
    @ItemID int
)
AS

SELECT
    CreatedByUser,
    CreatedDate,
    Title,
    ExpireDate,
    Description,
    WhereWhen

FROM
    Events

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleLink    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE GetSingleLink
(
    @ItemID int
)
AS

SELECT
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    MobileUrl,
    ViewOrder,
    Description

FROM
    Links

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetThreadMessages    Data dello script: 07/11/2002 22.28.14 ******/



CREATE PROCEDURE GetThreadMessages
(
    @Parent nvarchar(750)
)
AS

SELECT
    ItemID,
    DisplayOrder,
    REPLICATE( '&#160;', ( ( LEN( DisplayOrder ) / 23 ) - 1 ) * 5 ) AS Indent,
    Title,  
    CreatedByUser,
    CreatedDate,
    Body

FROM 
    Discussion

WHERE
    LEFT(DisplayOrder, 23) = @Parent
  AND
    (LEN( DisplayOrder ) / 23 ) > 1

ORDER BY
    DisplayOrder



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetTopLevelMessages    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE GetTopLevelMessages
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    DisplayOrder,
    LEFT(DisplayOrder, 23) AS Parent,    
    (SELECT COUNT(*) -1  FROM Discussion Disc2 WHERE LEFT(Disc2.DisplayOrder,LEN(RTRIM(Disc.DisplayOrder))) = Disc.DisplayOrder) AS ChildCount,
    Title,  
    CreatedByUser,
    CreatedDate

FROM 
    Discussion Disc

WHERE 
    ModuleID=@ModuleID
  AND
    (LEN( DisplayOrder ) / 23 ) = 1

ORDER BY
    DisplayOrder



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateContact    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE UpdateContact
(
    @ItemID   int,
    @UserName nvarchar(100),
    @Name     nvarchar(50),
    @Role     nvarchar(100),
    @Email    nvarchar(100),
    @Contact1 nvarchar(250),
    @Contact2 nvarchar(250)
)
AS

UPDATE
    Contacts

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Name          = @Name,
    Role          = @Role,
    Email         = @Email,
    Contact1      = @Contact1,
    Contact2      = @Contact2

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateDocument    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE UpdateDocument
(
    @ItemID           int,
    @ModuleID         int,
    @FileFriendlyName nvarchar(150),
    @FileNameUrl      nvarchar(250),
    @UserName         nvarchar(100),
    @Category         nvarchar(50),
    @Content          image,
    @ContentType      nvarchar(50),
    @ContentSize      int
)
AS
IF (@ItemID=0) OR NOT EXISTS (
    SELECT 
        * 
    FROM 
        Documents 
    WHERE 
        ItemID = @ItemID
)
INSERT INTO Documents
(
    ModuleID,
    FileFriendlyName,
    FileNameUrl,
    CreatedByUser,
    CreatedDate,
    Category,
    Content,
    ContentType,
    ContentSize
)

VALUES
(
    @ModuleID,
    @FileFriendlyName,
    @FileNameUrl,
    @UserName,
    GetDate(),
    @Category,
    @Content,
    @ContentType,
    @ContentSize
)
ELSE

BEGIN

IF (@ContentSize=0)

UPDATE 
    Documents

SET 
    CreatedByUser    = @UserName,
    CreatedDate      = GetDate(),
    Category         = @Category,
    FileFriendlyName = @FileFriendlyName,
    FileNameUrl      = @FileNameUrl

WHERE
    ItemID = @ItemID
ELSE

UPDATE
    Documents

SET
    CreatedByUser     = @UserName,
    CreatedDate       = GetDate(),
    Category          = @Category,
    FileFriendlyName  = @FileFriendlyName,
    FileNameUrl       = @FileNameUrl,
    Content           = @Content,
    ContentType       = @ContentType,
    ContentSize       = @ContentSize

WHERE
    ItemID = @ItemID

END


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateEvent    Data dello script: 07/11/2002 22.28.14 ******/



CREATE PROCEDURE UpdateEvent
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ExpireDate  datetime,
    @Description nvarchar(2000),
    @WhereWhen   nvarchar(100)
)

AS

UPDATE
    Events

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    ExpireDate    = @ExpireDate,
    Description   = @Description,
    WhereWhen     = @WhereWhen

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateHtmlText    Data dello script: 07/11/2002 22.28.14 ******/



CREATE PROCEDURE UpdateHtmlText
(
    @ModuleID      int,
    @DesktopHtml   ntext,
    @MobileSummary ntext,
    @MobileDetails ntext
)
AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        HtmlText 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO HtmlText (
    ModuleID,
    DesktopHtml,
    MobileSummary,
    MobileDetails
) 
VALUES (
    @ModuleID,
    @DesktopHtml,
    @MobileSummary,
    @MobileDetails
)
ELSE
UPDATE
    HtmlText

SET
    DesktopHtml   = @DesktopHtml,
    MobileSummary = @MobileSummary,
    MobileDetails = @MobileDetails

WHERE
    ModuleID = @ModuleID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure UpdateLink    Data dello script: 07/11/2002 22.28.14 ******/



CREATE PROCEDURE UpdateLink
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000)
)
AS

UPDATE
    Links

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    Url           = @Url,
    MobileUrl     = @MobileUrl,
    ViewOrder     = @ViewOrder,
    Description   = @Description

WHERE
    ItemID = @ItemID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Oggetto: stored procedure GetSingleMessage    Data dello script: 07/11/2002 22.28.14 ******/


CREATE PROCEDURE GetSingleMessage
(
    @ItemID int
)
AS

DECLARE @nextMessageID int
EXECUTE GetNextMessageID @ItemID, @nextMessageID OUTPUT
DECLARE @prevMessageID int
EXECUTE GetPrevMessageID @ItemID, @prevMessageID OUTPUT

SELECT
    ItemID,
    Title,
    CreatedByUser,
    CreatedDate,
    Body,
    DisplayOrder,
    NextMessageID = @nextMessageID,
    PrevMessageID = @prevMessageID

FROM
    Discussion

WHERE
    ItemID = @ItemID
GO
