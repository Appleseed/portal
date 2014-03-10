IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddArticle]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddGeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddGeneralModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddMilestones]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddModule]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddPicture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddPortal]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddTab]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddToBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddToBlackList]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUserFull]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[AddUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [AddUserRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Approve]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [Approve]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteArticle]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteFromBlackList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteFromBlackList]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteMilestones]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteModule]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteModuleDefinition]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeletePicture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeletePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeletePortal]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteTab]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DeleteUserRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [DeleteUserRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAnnouncements]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAnnouncements]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetArticles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetArticles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthAddRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthAddRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthApproveRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthApproveRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthDeleteRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthDeleteRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthEditRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthEditRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPropertiesRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthPropertiesRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPublishRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthPublishRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthPublishingRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthPublishingRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetAuthViewRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetAuthViewRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetContacts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetContacts]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountries]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountries]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCountriesFiltered]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCountriesFiltered]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCulture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCulture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetCurrentModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetCurrentModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDefaultCulture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDefaultCulture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocumentContent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocumentContent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetDocuments]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetDocuments]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetEvents]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetEvents]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetGeneralModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetGeneralModuleDefinitionByName]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetHtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetLastModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetLinks]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetLinks]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetMilestones]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitionByID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitionByID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitionByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitionByName]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleInUse]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleInUse]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModuleSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModuleSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesAllPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesAllPortals]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesByName]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesByName]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetModulesSinglePortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetModulesSinglePortal]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetNextMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetNextMessageID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPicture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPicturesPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPicturesPaged]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalCustomSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalRoles]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalRoles]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalSettingsPortalID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalSettingsPortalID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortals]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortals]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPortalsModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPortalsModules]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetPrevMessageID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetPrevMessageID]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRelatedTables]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetRelatedTables]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRoleMembership]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetRoleMembership]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetRolesByUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetRolesByUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSearchableModules]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSearchableModules]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleArticle]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleArticleWithImages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleArticleWithImages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleCountry]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleCountry]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleMessage]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleMilestones]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleModuleDefinition]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleModuleDefinition]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSinglePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSinglePicture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSingleUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSingleUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSolutionModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetSolutions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetSolutions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetStates]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetStates]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabCrumbs]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabCrumbs]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabCustomSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsByPortal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsByPortal]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsFlat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsFlat]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsParent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTabsinTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTabsinTab]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetThreadMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetThreadMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetTopLevelMessages]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetTopLevelMessages]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsers]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsersCount]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GetUsersNewsletter]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [GetUsersNewsletter]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[LocalizeManager]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [LocalizeManager]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleEdited]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [ModuleEdited]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Publish]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [Publish]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Reject]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [Reject]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[RequestApproval]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [RequestApproval]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SendNewsletterTo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [SendNewsletterTo]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SetLastModified]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [SetLastModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateAnnouncement]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateAnnouncement]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateArticle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateArticle]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateContact]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateContact]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateDocument]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateDocument]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateEvent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateEvent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateGeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateGeneralModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateHtmlText]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateLink]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateLink]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateMilestones]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateMilestones]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModule]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleDefinitions]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleOrder]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModuleSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModuleSetting]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePicture]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePicture]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalInfo]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePortalInfo]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdatePortalSetting]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdatePortalSetting]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateRole]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateRole]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateTab]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTabCustomSettings]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateTabCustomSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateTabOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateTabOrder]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUser]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserCheckEmail]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUserCheckEmail]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateUserFull]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateUserFull]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UserLogin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UserLogin]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetCurrentDbVersion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetCurrentDbVersion]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModulesUpgradeOldToNew]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModulesUpgradeOldToNew]
GO

-- =============================================================
-- ALTER  the stored procs
-- =============================================================
CREATE  PROCEDURE rb_AddAnnouncement
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

INSERT INTO rb_st_Announcements
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


CREATE PROCEDURE rb_AddArticle
(
    @ModuleID       int,
    @UserName       nvarchar(100),
    @Title          nvarchar(100),
    @Subtitle       nvarchar(200),
    @Abstract	    nvarchar(512),
    @Description    text,
    @StartDate      datetime,
    @ExpireDate     datetime,
    @IsInNewsletter bit,
    @MoreLink       nvarchar(150),
    @ItemID         int OUTPUT
)
AS

INSERT INTO rb_Articles
(
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
	MoreLink
)
VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @Subtitle,
    @Abstract,
    @Description,
    @StartDate,
    @ExpireDate,
    @IsInNewsletter,
    @MoreLink
)

SELECT
    @ItemID = @@IDENTITY

GO



/****** Oggetto: stored procedure AddContact    Data dello script: 07/11/2002 22.28.12 ******/

CREATE  PROCEDURE rb_AddContact
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

INSERT INTO rb_st_Contacts
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





/****** Oggetto: stored procedure AddEvent    Data dello script: 07/11/2002 22.28.12 ******/


CREATE  PROCEDURE rb_AddEvent
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

INSERT INTO rb_st_Events
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



/* PROCEDURE rb_AddGeneralModuleDefinitions*/
CREATE PROCEDURE rb_AddGeneralModuleDefinitions
	@GeneralModDefID uniqueidentifier,
	@FriendlyName nvarchar(128),
	@DesktopSrc nvarchar(256),
	@MobileSrc nvarchar(256),
	@AssemblyName varchar(50),
	@ClassName nvarchar(128),
	@Admin bit,
	@Searchable bit
AS
INSERT INTO rb_GeneralModuleDefinitions
(
	GeneralModDefID,
	FriendlyName,
	DesktopSrc,
	MobileSrc,
	AssemblyName,
	ClassName,
	Admin,
	Searchable
)
VALUES
(
	@GeneralModDefID,
	@FriendlyName,
	@DesktopSrc,
	@MobileSrc,
	@AssemblyName,
	@ClassName,
	@Admin,
	@Searchable
)

GO



CREATE  PROCEDURE rb_AddLink
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target	 nvarchar(10),
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_st_Links
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    Url,
    MobileUrl,
    ViewOrder,
    Description,
    Target
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
    @Description,
    @Target
)

SELECT
    @ItemID = @@IDENTITY


GO



CREATE     PROCEDURE rb_AddMessage
(
    @ItemID int OUTPUT,
    @Title nvarchar(100),
    @Body nvarchar(3000),
    @ParentID int,
    @UserName nvarchar(100),
    @ModuleID int
)   

AS 

/* Find DisplayOrder of parent item */
DECLARE @ParentDisplayOrder as nvarchar(750)

SET @ParentDisplayOrder = ''

SELECT 
    @ParentDisplayOrder = DisplayOrder
FROM 
    rb_Discussion 
WHERE 
    ItemID = @ParentID

INSERT INTO rb_Discussion
(
    Title,
    Body,
    DisplayOrder,
    CreatedDate, 
    CreatedByUser,
    ModuleID
)

VALUES
(
    @Title,
    @Body,
    @ParentDisplayOrder + CONVERT( nvarchar(24), GetDate(), 21 ),
    GetDate(),
    @UserName,
    @ModuleID
)

SELECT 
    @ItemID = @@IDENTITY

GO



/* PROCEDURE rb_AddMilestones*/
CREATE PROCEDURE rb_AddMilestones
	@ItemID int OUTPUT,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
INSERT INTO rb_Milestones
(
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
)
VALUES
(
	@ModuleID,
	@CreatedByUser,
	@CreatedDate,
	@Title,
	@EstCompleteDate,
	@Status
)
SELECT
	@ItemID = @@IDENTITY

GO



CREATE    PROCEDURE rb_AddModule
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
    @PropertiesRoles	    nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles        nvarchar(256),
    @SupportWorkflow	    bit,
    @ModuleID               int OUTPUT
)
AS
INSERT INTO rb_Modules
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
    ShowMobile,
    AuthorizedPublishingRoles,
    NewVersion, 
    SupportWorkflow
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
    @ShowMobile,
    @PublishingRoles,
    1, -- False
    @SupportWorkflow
)

SELECT 
    @ModuleID = @@IDENTITY

GO



-- =============================================================
-- create the stored procs
-- =============================================================
CREATE PROCEDURE [rb_AddPicture]
	(@ItemID 	[int ]OUTPUT,
	 @ModuleID 	[int],
	 @DisplayOrder	[int],
	 @MetadataXml VARCHAR(6000),
	 @ShortDescription VARCHAR(256),
	 @Keywords VARCHAR(256)
)
AS 
INSERT INTO [rb_Pictures]
	([ModuleID],
	[DisplayOrder],
	[MetadataXml],
	[ShortDescription],
	[Keywords]
) 
VALUES 
	(@ModuleID,
	 @DisplayOrder,
	 @MetadataXml,
	 @ShortDescription,
	 @Keywords)
SELECT 
	@ItemID = @@IDENTITY

GO



CREATE PROCEDURE rb_AddPortal
(
    @PortalAlias            nvarchar(128),
    @PortalName             nvarchar(128),
    @PortalPath             nvarchar(128),
    @AlwaysShowEditButton   bit,
    @PortalID               int OUTPUT
)
AS

INSERT INTO rb_Portals
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



CREATE PROCEDURE rb_AddRole
(
    @PortalID    int,
    @RoleName    nvarchar(50),
    @RoleID      int OUTPUT
)
AS

INSERT INTO rb_Roles
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



CREATE PROCEDURE rb_AddTab
(
    @PortalID   int,
    @TabName    nvarchar(50),
    @TabOrder   int,
    @AuthorizedRoles nvarchar (256),
    @MobileTabName nvarchar(50),
    @TabID      int OUTPUT
)
AS

INSERT INTO rb_Tabs
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



CREATE PROCEDURE rb_AddToBlackList
(
@PortalID int,
@Email nvarchar(100),
@Reason nvarchar(150)
)
AS 
IF NOT Exists (SELECT Email FROM rb_Blacklist WHERE PortalID=@PortalID AND Email=@Email)
BEGIN
	INSERT INTO rb_BlackList
	(
		PortalID,
		Email,
		Date,
		Reason
	)
	VALUES
	(
		@PortalID,
		@Email,
		GetDate(),
		@Reason
	)
END

GO



CREATE PROCEDURE rb_AddUser
(
    @PortalID int,
    @Name     nvarchar(50),
    @Email    nvarchar(100),
    @Password nvarchar(20),
    @UserID   int OUTPUT
)
AS

INSERT INTO rb_Users
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



CREATE PROCEDURE rb_AddUserFull
(
    @PortalID	    	    int,
    @Name		    nvarchar(50),
    @Company	            nvarchar(50),
    @Address	            nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	            nvarchar(16),
    @Email		    nvarchar(100),
    @Password	            nvarchar(20),
    @SendNewsletter	    bit,
    @CountryID		    nchar(2),  
    @StateID	            int,
    @UserID		    int OUTPUT
)
AS

INSERT INTO rb_Users
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
	CountryID,
	StateID
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
	@CountryID,
	@StateID
)

SELECT
    @UserID = @@IDENTITY

GO



CREATE PROCEDURE rb_AddUserRole
(
    @UserID int,
    @RoleID int
)
AS

SELECT 
    *
FROM
    rb_UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID

/* only insert if the record doesn't yet exist */
IF @@Rowcount < 1

    INSERT INTO rb_UserRoles
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



CREATE PROCEDURE rb_Approve
	@moduleID	int
AS
	UPDATE	rb_Modules
	SET
		WorkflowState = 3 -- Approved
	WHERE
		[ModuleID] = @moduleID

GO



CREATE  PROCEDURE rb_DeleteAnnouncement
(
    @ItemID int
)
AS

DELETE FROM
    rb_st_Announcements

WHERE
    ItemID = @ItemID

GO



CREATE PROCEDURE rb_DeleteArticle
(
    @ItemID int
)
AS

DELETE FROM
    rb_Articles

WHERE
    ItemID = @ItemID

GO



CREATE  PROCEDURE rb_DeleteContact
(
    @ItemID int
)
AS

DELETE FROM
    rb_st_Contacts

WHERE
    ItemID = @ItemID







GO






/****** Oggetto: stored procedure DeleteDocument    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_DeleteDocument
(
    @ItemID int
)
AS

DELETE FROM
    rb_st_Documents

WHERE
    ItemID = @ItemID







GO
 






/****** Oggetto: stored procedure DeleteEvent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_DeleteEvent
(
    @ItemID int
)
AS

DELETE FROM
    rb_st_Events

WHERE
    ItemID = @ItemID







GO
 



CREATE PROCEDURE rb_DeleteFromBlackList
(
@PortalID int,
@Email nvarchar(100)

)
AS 
DELETE FROM rb_Blacklist WHERE PortalID=@PortalID AND Email=@Email

GO
 






/****** Oggetto: stored procedure DeleteLink    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_DeleteLink
(
    @ItemID int
)
AS

DELETE FROM
    rb_st_Links

WHERE
    ItemID = @ItemID







GO
 



/* PROCEDURE rb_DeleteMilestones*/
CREATE PROCEDURE rb_DeleteMilestones
@ItemID int
AS
DELETE
FROM
	rb_Milestones
WHERE
	ItemID = @ItemID

GO
 



CREATE PROCEDURE rb_DeleteModule
(
    @ModuleID       int
)
AS
DELETE FROM 
    rb_Modules 
WHERE 
    ModuleID = @ModuleID

GO
 



CREATE PROCEDURE rb_DeleteModuleDefinition
(
    @ModuleDefID uniqueidentifier
)
AS
DELETE FROM
    rb_GeneralModuleDefinitions
WHERE
    GeneralModDefID = @ModuleDefID

GO
 



--************************************************************************
CREATE PROCEDURE [rb_DeletePicture]
	(@ItemID 	[int])
AS DELETE FROM [rb_Pictures]
WHERE 
	( [ItemID] = @ItemID)
GO
 
CREATE PROCEDURE rb_DeletePortal
(
    @PortalID       int
)
AS
DELETE FROM 
    rb_Portals 
WHERE 
    PortalID = @PortalID

DELETE FROM 
    rb_Tabs 
WHERE 
    PortalID = @PortalID
GO
 



CREATE PROCEDURE rb_DeleteRole
(
    @RoleID int
)
AS

DELETE FROM
    rb_Roles

WHERE
    RoleID = @RoleID

GO
 



CREATE PROCEDURE rb_DeleteTab
(
    @TabID int
)
AS

DELETE FROM
    rb_Tabs

WHERE
    TabID = @TabID

GO
 



CREATE PROCEDURE rb_DeleteUser
(
    @UserID int
)
AS

DELETE FROM
    rb_Users

WHERE
    UserID=@UserID

GO
 



CREATE PROCEDURE rb_DeleteUserRole
(
    @UserID int,
    @RoleID int
)
AS

DELETE FROM
    rb_UserRoles

WHERE
    UserID=@UserID
    AND
    RoleID=@RoleID

GO
 






/****** Oggetto: stored procedure GetAnnouncements    Data dello script: 07/11/2002 22.28.08 ******/


CREATE  PROCEDURE rb_GetAnnouncements
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
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
	    rb_Announcements
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
ELSE
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
	    rb_st_Announcements
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
GO

CREATE PROCEDURE rb_GetArticles
(
    @ModuleID int
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
			MoreLink

FROM        rb_Articles

WHERE
    (ModuleID = @ModuleID) AND (GetDate() <= ExpireDate) AND (GetDate() >= StartDate)

ORDER BY
    StartDate DESC
GO

CREATE PROCEDURE rb_GetAuthAddRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @AddRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @AddRoles   = rb_Modules.AuthorizedAddRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE  PROCEDURE rb_GetAuthApproveRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ApproveRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @ApproveRoles   = rb_Modules.AuthorizedApproveRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetAuthDeleteRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @DeleteRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @DeleteRoles   = rb_Modules.AuthorizedDeleteRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetAuthEditRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @EditRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @EditRoles   = rb_Modules.AuthorizedEditRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetAuthPropertiesRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PropertiesRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @PropertiesRoles   = rb_Modules.AuthorizedPropertiesRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE  PROCEDURE rb_GetAuthPublishRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PublishRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @PublishRoles   = rb_Modules.AuthorizedPublishingRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID


GO
 



CREATE PROCEDURE rb_GetAuthPublishingRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @PublishingRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @PublishingRoles   = rb_Modules.AuthorizedPublishingRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetAuthViewRoles
(
    @PortalID    int,
    @ModuleID    int,
    @AccessRoles nvarchar (256) OUTPUT,
    @ViewRoles   nvarchar (256) OUTPUT
)
AS

SELECT  
    @AccessRoles = rb_Tabs.AuthorizedRoles,
    @ViewRoles   = rb_Modules.AuthorizedViewRoles
    
FROM    
    rb_Modules
  INNER JOIN
    rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
    
WHERE   
    rb_Modules.ModuleID = @ModuleID
  AND
    rb_Tabs.PortalID = @PortalID

GO
 






/****** Oggetto: stored procedure GetContacts    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetContacts
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
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
	    rb_Contacts
	WHERE
	    ModuleID = @ModuleID
ELSE
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
	    rb_st_Contacts
	WHERE
	    ModuleID = @ModuleID







GO
 



CREATE PROCEDURE rb_GetCountries
(
	@IDLang	nchar(2) = 'en'
)

AS

IF 
(
SELECT     COUNT(rb_Countries.CountryID) AS CountryListCount
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     (rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)
) > 0

BEGIN
-- Country returns results
SELECT     rb_Countries.CountryID, rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     (rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)
ORDER BY rb_Localize.Description
END

else

BEGIN
-- Get English list
SELECT     rb_Countries.CountryID, rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     (rb_Localize.CultureCode = 'en') OR
                      (rb_Cultures.NeutralCode = 'en')
ORDER BY rb_Localize.Description
END

GO
 



CREATE PROCEDURE rb_GetCountriesFiltered
(
	@IDLang	nchar(2) = 'en',
	@Filter nvarchar(1000)
)

AS


SELECT     rb_Countries.CountryID, rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     ((rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)) AND (PATINDEX('%' + rb_Countries.CountryID + '%', @Filter) > 0)
ORDER BY rb_Localize.Description


GO
 



CREATE PROCEDURE rb_GetCulture
(
	@CountryID nchar(2)
)
AS
SELECT    CultureCode, CountryID
FROM      rb_Cultures
WHERE     (CountryID = @CountryID)

GO
 



/* returns all module definitions for the specified portal */
CREATE PROCEDURE rb_GetCurrentModuleDefinitions
(
    @PortalID  int
)
AS
SELECT  
    rb_GeneralModuleDefinitions.FriendlyName,
    rb_GeneralModuleDefinitions.DesktopSrc,
    rb_GeneralModuleDefinitions.MobileSrc,
    rb_ModuleDefinitions.ModuleDefID
FROM
    rb_ModuleDefinitions
INNER JOIN
	rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE   
    rb_ModuleDefinitions.PortalID = @PortalID
ORDER BY
rb_GeneralModuleDefinitions.Admin, rb_GeneralModuleDefinitions.FriendlyName

GO
 



CREATE PROCEDURE rb_GetDefaultCulture
(
	@CountryID nchar(2)
)
AS
SELECT    CultureCode, CountryID
FROM      rb_Cultures
WHERE     (CountryID = @CountryID)

GO
 






/****** Oggetto: stored procedure GetDocumentContent    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetDocumentContent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    Content,
	    ContentType,
	    ContentSize,
	    FileFriendlyName
	FROM
	    rb_Documents
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    Content,
	    ContentType,
	    ContentSize,
	    FileFriendlyName
	FROM
	    rb_st_Documents
	WHERE
	    ItemID = @ItemID
	






GO
 






/****** Oggetto: stored procedure GetDocuments    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetDocuments
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT
	    ItemID,
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_st_Documents
	WHERE
	    ModuleID = @ModuleID





GO
 






/****** Oggetto: stored procedure GetEvents    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetEvents
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
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
	    rb_Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()
ELSE
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
	    rb_st_Events
	WHERE
	    ModuleID = @ModuleID
	  AND
	    ExpireDate > GetDate()








GO
 



/****** Oggetto: stored procedure GetGeneralModuleDefinitionByName    Data dello script: 07/11/2002 22.28.09 ******/
CREATE PROCEDURE
rb_GetGeneralModuleDefinitionByName
(
	@FriendlyName nvarchar(128),
	@ModuleID uniqueidentifier OUTPUT
)
AS

SELECT @ModuleID =
(
SELECT  rb_GeneralModuleDefinitions.GeneralModDefID
FROM    rb_GeneralModuleDefinitions
WHERE   (rb_GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)

GO
 






/****** Oggetto: stored procedure GetHtmlText    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetHtmlText
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT *
	FROM
	    rb_HtmlText
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT *
	FROM
	    rb_st_HtmlText
	WHERE
	    ModuleID = @ModuleID








GO
 



CREATE PROCEDURE rb_GetLastModified
	(
		@ModuleID int,
		@WorkflowVersion int,
		@LastModifiedBy	nvarchar(256) OUTPUT,
		@LastModifiedDate datetime OUTPUT
	)
AS

	if ( @WorkflowVersion = 1 )
	begin
		select @LastModifiedDate = [LastModified], @LastModifiedBy = [LastEditor]
		from rb_Modules
		WHERE [ModuleID] = @ModuleID
	end
	else
	begin
		select @LastModifiedDate = [StagingLastModified], @LastModifiedBy = [StagingLastEditor]
		from rb_Modules
		WHERE [ModuleID] = @ModuleID
	end

	/* SET NOCOUNT ON */
	RETURN 

GO
 






/****** Oggetto: stored procedure GetLinks    Data dello script: 07/11/2002 22.28.13 ******/


CREATE  PROCEDURE rb_GetLinks
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_st_Links
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder







GO
 



/* PROCEDURE rb_GetMilestones*/
CREATE PROCEDURE rb_GetMilestones
@ModuleID int
AS
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones
WHERE
	ModuleID = @ModuleID

GO
 



CREATE PROCEDURE
rb_GetModuleDefinitionByID
(
	@ModuleID int
)
AS


SELECT     rb_ModuleDefinitions.ModuleDefID, rb_ModuleDefinitions.PortalID, rb_GeneralModuleDefinitions.FriendlyName, rb_GeneralModuleDefinitions.DesktopSrc, 
                      rb_GeneralModuleDefinitions.MobileSrc, rb_GeneralModuleDefinitions.Admin, rb_Modules.ModuleID
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
                      rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE     (rb_Modules.ModuleID = @ModuleID)

GO
 



/****** Oggetto: stored procedure GetModuleDefinitionByName    Data dello script: 07/11/2002 22.28.11 ******/

CREATE PROCEDURE
rb_GetModuleDefinitionByName
(
	@PortalID int,
	@FriendlyName nvarchar(128),
	@ModuleID int OUTPUT
)
AS

SELECT
    @ModuleID =
(
    SELECT     rb_ModuleDefinitions.ModuleDefID
    FROM       rb_GeneralModuleDefinitions LEFT JOIN
                    rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
    WHERE      (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.FriendlyName = @FriendlyName)
)


GO
 



/* returns all module definitions for the specified portal */
CREATE PROCEDURE rb_GetModuleDefinitions

AS
SELECT     GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc
FROM         rb_GeneralModuleDefinitions
ORDER BY Admin, FriendlyName

GO
 



CREATE PROCEDURE rb_GetModuleInUse
(
    @ModuleID uniqueidentifier
)
AS
SELECT     rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, '1' AS Checked
FROM         rb_Portals LEFT OUTER JOIN
                      rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
WHERE     (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)

UNION

SELECT DISTINCT
    PortalID, PortalAlias, PortalName, '0' AS Checked
FROM   rb_Portals
WHERE  
(
PortalID NOT IN
    (SELECT     rb_Portals.PortalID
     FROM       rb_Portals LEFT OUTER JOIN rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
     WHERE      (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)
    )
)

GO
 



CREATE PROCEDURE rb_GetModuleSettings
(
    @ModuleID int
)
AS
SELECT     SettingName, SettingValue
FROM         rb_ModuleSettings
WHERE     (ModuleID = @ModuleID)

GO
 



--Fix on Shortuctall module, shortcuts should not be displayed on rb_GetModulesAllPortals list
CREATE PROCEDURE rb_GetModulesAllPortals
AS

SELECT      0 AS ModuleID, 'NO_MODULE' AS ModuleTitle, '' as PortalAlias, -1 as TabOrder

UNION

	SELECT     rb_Modules.ModuleID, rb_Portals.PortalAlias + '/' + rb_Tabs.TabName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')'  AS ModuleTitle, PortalAlias, rb_Tabs.TabOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
	                      rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Modules.ModuleID > 0) AND (rb_GeneralModuleDefinitions.Admin = 0) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
ORDER BY PortalAlias, rb_Modules.ModuleTitle

GO
 



CREATE PROCEDURE rb_GetModulesByName
(
	@ModuleName varchar(128),
	@PortalID int
)
AS

SELECT      0 ModuleID, ' Nessun modulo' ModuleTitle

UNION

SELECT     rb_Modules.ModuleID, rb_Modules.ModuleTitle
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID INNER JOIN
                      rb_Modules ON rb_ModuleDefinitions.ModuleDefID = rb_Modules.ModuleDefID
WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.FriendlyName = @ModuleName)

ORDER BY rb_Modules.ModuleTitle

GO
 



CREATE PROCEDURE rb_GetModulesSinglePortal
(
    @PortalID  int
)
AS

SELECT      0 ModuleID, 'NO_MODULE' ModuleTitle, -1 as TabOrder

UNION

	SELECT     rb_Modules.ModuleID, rb_Tabs.TabName + '/' + rb_Modules.ModuleTitle + ' (' + rb_GeneralModuleDefinitions.FriendlyName + ')' AS ModTitle, rb_Tabs.TabOrder
	FROM         rb_Modules INNER JOIN
	                      rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID INNER JOIN
	                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
	                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
	WHERE     (rb_Tabs.PortalID = @PortalID) AND (rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD2' AND 
	                      rb_GeneralModuleDefinitions.GeneralModDefID <> 'F9F9C3A4-6E16-43b4-B540-984DDB5F1CD0')
	ORDER BY TabOrder, rb_Modules.ModuleTitle

GO
 



CREATE PROCEDURE rb_GetNextMessageID
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
    rb_Discussion
WHERE
    ItemID = @ItemID

/* Get the next message in the same module */
SELECT Top 1
    @NextID = ItemID

FROM
    rb_Discussion

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
 



--************************************************************************
CREATE PROCEDURE rb_GetPicture
(@ModuleID int)
AS
SELECT ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords
FROM rb_Pictures 
WHERE ModuleID = @ModuleID
ORDER BY DisplayOrder

GO
 



--************************************************************************
CREATE     PROCEDURE rb_GetPicturesPaged
(
	@ModuleID int,
	@Page int = 1,
	@RecordsPerPage int = 10
)
AS

-- We don't want to return the # of rows inserted
-- into our temporary table, so turn NOCOUNT ON
SET NOCOUNT ON

--Create a temporary table
CREATE TABLE #TempItems
(
	ID				int IDENTITY,
 	ItemID 			int,
 	ModuleID 		int,
	DisplayOrder		int,
 	MetadataXml		varchar(6000),
 	ShortDescription	varchar(256),
 	Keywords		varchar(256)
)

-- Insert the rows from tblItems into the temp. table
INSERT INTO
#TempItems
(
	ItemID, DisplayOrder, MetadataXml, ShortDescription, Keywords
)
SELECT
	rb_Pictures.ItemID, 
	rb_Pictures.DisplayOrder, 
	rb_Pictures.MetadataXml, 
	rb_Pictures.ShortDescription, 
	rb_Pictures.Keywords
FROM
	rb_Pictures
WHERE
	rb_Pictures.ModuleID = @ModuleID
ORDER BY 
	DisplayOrder

-- Find out the first AND last record we want
DECLARE @FirstRec int, @LastRec int
SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
SELECT @LastRec = (@Page * @RecordsPerPage + 1)

-- Now, return the set of paged records, plus, an indiciation of we
-- have more records or not!
SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
FROM #TempItems
WHERE ID > @FirstRec AND ID < @LastRec

-- Turn NOCOUNT back OFF
SET NOCOUNT OFF

GO 



CREATE PROCEDURE rb_GetPortalCustomSettings
(
    @PortalID int
)
AS
SELECT
    SettingName,
    SettingValue
FROM
    rb_PortalSettings
WHERE
    PortalID = @PortalID

GO
 



/* returns all roles for the specified portal */
CREATE PROCEDURE rb_GetPortalRoles
(
    @PortalID  int
)
AS

SELECT  
    RoleName,
    RoleID

FROM
    rb_Roles

WHERE   
    PortalID = @PortalID

order by RoleID
/* questo assicura che l'ultimo inserito si in fondo alla lista */

GO
 



-- End change marcb@hotmail.com
-- End Change Geert.Audenaert@Syntegra.Com

--Manu - Fix for Desktop tabs
CREATE PROCEDURE rb_GetPortalSettings
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
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabID         = rb_Tabs.TabID,
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @TabName       = rb_Tabs.TabName,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
        
    WHERE
        PortalAlias=@PortalAlias
        
    ORDER BY
        TabOrder
ELSE 
    SELECT
        @PortalID      = rb_Portals.PortalID,
        @PortalName    = rb_Portals.PortalName,
        @PortalPath    = rb_Portals.PortalPath,
        @AlwaysShowEditButton = rb_Portals.AlwaysShowEditButton,
        @TabName       = rb_Tabs.TabName,
        @TabOrder      = rb_Tabs.TabOrder,
        @ParentTabID   = rb_Tabs.ParentTabID,
        @MobileTabName = rb_Tabs.MobileTabName,
        @AuthRoles     = rb_Tabs.AuthorizedRoles,
        @ShowMobile    = rb_Tabs.ShowMobile
    FROM
        rb_Tabs
    INNER JOIN
        rb_Portals ON rb_Tabs.PortalID = rb_Portals.PortalID
        
    WHERE
        TabID=@TabID AND rb_Portals.PortalAlias=@PortalAlias

/* Get Tabs list */
SELECT  
    TabName,
    AuthorizedRoles,
    TabID,
    ParentTabID,
    TabOrder    
FROM    
    rb_Tabs
    
WHERE   
    PortalID = @PortalID
    
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
    rb_Tabs
    
WHERE   
    PortalID = @PortalID
  AND
    ShowMobile = 1
    
ORDER BY
    TabOrder
/* Then, get the DataTable of module info */
SELECT     *
FROM         rb_Modules INNER JOIN
                      rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID INNER JOIN
                      rb_GeneralModuleDefinitions ON rb_ModuleDefinitions.GeneralModDefID = rb_GeneralModuleDefinitions.GeneralModDefID
WHERE     (rb_Modules.TabID = @TabID)
ORDER BY rb_Modules.ModuleOrder

GO
 



CREATE PROCEDURE rb_GetPortalSettingsPortalID
(
    @PortalID   nvarchar(50)
)
AS
    SELECT     TOP 1 PortalID, PortalName, PortalPath, AlwaysShowEditButton, PortalAlias
    FROM         rb_Portals
    WHERE     (PortalID = @PortalID)


GO
 



CREATE PROCEDURE rb_GetPortals
AS
SELECT  rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, rb_Portals.PortalPath, rb_Portals.AlwaysShowEditButton
FROM    rb_Portals

GO
 



CREATE PROCEDURE rb_GetPortalsModules
(
    @ModuleID  uniqueidentifier
)
AS
	SELECT     rb_Portals.PortalID, rb_Portals.PortalAlias, rb_Portals.PortalName, rb_ModuleDefinitions.ModuleDefID
	FROM         rb_Portals LEFT OUTER JOIN
	                      rb_ModuleDefinitions ON rb_Portals.PortalID = rb_ModuleDefinitions.PortalID
	WHERE     (rb_ModuleDefinitions.GeneralModDefID = @ModuleID)


GO
 



CREATE PROCEDURE rb_GetPrevMessageID
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
    rb_Discussion
WHERE
    ItemID = @ItemID

/* Get the previous message in the same module */
SELECT Top 1
    @PrevID = ItemID

FROM
    rb_Discussion

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
 




CREATE   PROCEDURE rb_GetRelatedTables
	@Name	nvarchar(128),
	@Schema	nvarchar(128)
AS
	SELECT 
		[InnerResults].[ForeignKeyTableSchema],
		[InnerResults].[ForeignKeyTable], 
		[InnerResults].[ForeignKeyColumn], 
		[InnerResults].[KeyColumn],
		[InnerResults].[ForeignKeyTableId],
		[InnerResults].[KeyTableId],
		[InnerResults].[KeyTableSchema],
		[InnerResults].[KeyTable]
	FROM
		(
			SELECT     
				[FKeyTable].[TableName] AS ForeignKeyTable, 
				[FKeyTable].[TableSchema] As ForeignKeyTableSchema,
				[KeyTable].[TableName] AS KeyTable, 
				[KeyTable].[TableSchema] As KeyTableSchema,
				[FKeyColumns].[name] AS ForeignKeyColumn, 
			        [KeyColumns].[name] AS KeyColumn,
				[FKeyTable].[id] AS ForeignKeyTableId,
				[KeyTable].[id] AS KeyTableId
			FROM         sysforeignkeys INNER JOIN
			                      (
							SELECT     
								[sysobjects].[id] As ID, 
								[sysobjects].[name] AS TableName,
								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
							FROM    
								[sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
									ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
							WHERE   
								([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
					       ) FKeyTable ON sysforeignkeys.fkeyid = [FKeyTable].[ID] INNER JOIN
					       (
							SELECT     
								[sysobjects].[id] As ID, 
								[sysobjects].[name] AS TableName,
								[INFORMATION_SCHEMA].[TABLES].[TABLE_SCHEMA] As TableSchema
							FROM    
								[sysobjects] INNER JOIN [INFORMATION_SCHEMA].[TABLES] 
									ON [sysobjects].[name] = [INFORMATION_SCHEMA].[TABLES].[TABLE_NAME] 
							WHERE   
								([INFORMATION_SCHEMA].[TABLES].[TABLE_TYPE] = 'BASE TABLE')
					       ) KeyTable ON sysforeignkeys.rkeyid = [KeyTable].[ID] INNER JOIN
			                      syscolumns FKeyColumns ON [FKeyTable].[ID] = [FKeyColumns].[id] AND sysforeignkeys.fkey = [FKeyColumns].[colid] INNER JOIN
			                      syscolumns KeyColumns ON [KeyTable].[ID] = [KeyColumns].[id] AND sysforeignkeys.rkey = [KeyColumns].[colid]
		) InnerResults
	WHERE			
		[InnerResults].[KeyTable] = @Name

GO
 



/* returns all members for the specified role */
CREATE PROCEDURE rb_GetRoleMembership
(
    @RoleID  int
)
AS

SELECT  
    rb_UserRoles.UserID,
    Name,
    Email

FROM
    rb_UserRoles
    
INNER JOIN 
    rb_Users On rb_Users.UserID = rb_UserRoles.UserID

WHERE   
    rb_UserRoles.RoleID = @RoleID

GO
 



/* returns all roles for the specified user */
CREATE PROCEDURE rb_GetRolesByUser
(
    @PortalID		int,
    @Email         nvarchar(100)
)
AS

SELECT  
    rb_Roles.RoleName,
    rb_Roles.RoleID

FROM
    rb_UserRoles
  INNER JOIN 
    rb_Users ON rb_UserRoles.UserID = rb_Users.UserID
  INNER JOIN 
    rb_Roles ON rb_UserRoles.RoleID = rb_Roles.RoleID

WHERE   
    rb_Users.Email = @Email AND rb_Users.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetSearchableModules
(
	@PortalID int
)
AS
SELECT     rb_GeneralModuleDefinitions.GeneralModDefID, rb_GeneralModuleDefinitions.ClassName, rb_GeneralModuleDefinitions.FriendlyName, 
                      rb_GeneralModuleDefinitions.DesktopSrc, rb_GeneralModuleDefinitions.MobileSrc, rb_GeneralModuleDefinitions.Admin, rb_GeneralModuleDefinitions.Searchable, 
                      rb_GeneralModuleDefinitions.AssemblyName, rb_ModuleDefinitions.ModuleDefID
FROM         rb_GeneralModuleDefinitions INNER JOIN
                      rb_ModuleDefinitions ON rb_GeneralModuleDefinitions.GeneralModDefID = rb_ModuleDefinitions.GeneralModDefID
WHERE     (rb_GeneralModuleDefinitions.Searchable = 1) AND (rb_ModuleDefinitions.PortalID = @PortalID)

GO
 






/****** Oggetto: stored procedure rb_GetSingleAnnouncement    Data dello script: 07/11/2002 22.28.09 ******/



CREATE  PROCEDURE rb_GetSingleAnnouncement
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM
	    rb_Announcements
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    MoreLink,
	    MobileMoreLink,
	    ExpireDate,
	    Description
	FROM
	    rb_st_Announcements
	WHERE
	    ItemID = @ItemID







GO
 



CREATE PROCEDURE rb_GetSingleArticle
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
			MoreLink
FROM	rb_Articles
WHERE   (ItemID = @ItemID)

GO
 



CREATE PROCEDURE rb_GetSingleArticleWithImages
(
    @ItemID int,
    @Variation varchar(50)
)
AS

SELECT		rb_Articles.ItemID, 
			rb_Articles.ModuleID, 
			rb_Articles.CreatedByUser, 
			rb_Articles.CreatedDate, 
			rb_Articles.Title, 
			rb_Articles.Subtitle, 
			rb_Articles.Abstract, 
			rb_Articles.Description, 
            rb_Articles.StartDate, 
            rb_Articles.ExpireDate, 
            rb_Articles.IsInNewsletter, 
            rb_Articles.MoreLink
            
FROM        rb_Articles
WHERE     (ItemID = @ItemID)

GO
 






/****** Oggetto: stored procedure rb_GetSingleContact    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE rb_GetSingleContact
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_Contacts
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Name,
	    Role,
	    Email,
	    Contact1,
	    Contact2
	FROM
	    rb_st_Contacts
	WHERE
	    ItemID = @ItemID







GO
 



CREATE PROCEDURE rb_GetSingleCountry
(
	@IDState int,
	@IDLang	nchar(2) = 'en'
)

AS
SELECT     rb_Countries.CountryID, rb_Localize.Description, rb_States.StateID
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID INNER JOIN
                      rb_States ON rb_Countries.CountryID = rb_States.CountryID
WHERE     (rb_Localize.CultureCode = @IDLang) AND (rb_States.StateID = @IdState) OR
                      (rb_States.StateID = @IdState) AND (rb_Cultures.NeutralCode = @IDLang)
ORDER BY rb_Localize.Description

GO
 






/****** Oggetto: stored procedure rb_GetSingleDocument    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE rb_GetSingleDocument
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_Documents
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    FileFriendlyName,
	    FileNameUrl,
	    CreatedByUser,
	    CreatedDate,
	    Category,
	    ContentSize
	FROM
	    rb_st_Documents
	WHERE
	    ItemID = @ItemID








GO
 






/****** Oggetto: stored procedure rb_GetSingleEvent    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE rb_GetSingleEvent
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_Events
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ExpireDate,
	    Description,
	    WhereWhen	
	FROM
	    rb_st_Events
	WHERE
	    ItemID = @ItemID








GO
 






/****** Oggetto: stored procedure rb_GetSingleLink    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE rb_GetSingleLink
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_Links
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    Url,
	    MobileUrl,
	    ViewOrder,
	    Description,
	    Target
	FROM
	    rb_st_Links
	WHERE
	    ItemID = @ItemID





GO
 



CREATE PROCEDURE rb_GetSingleMessage
(
    @ItemID int
)
AS

DECLARE @nextMessageID int
EXECUTE rb_GetNextMessageID @ItemID, @nextMessageID OUTPUT
DECLARE @prevMessageID int
EXECUTE rb_GetPrevMessageID @ItemID, @prevMessageID OUTPUT

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
    rb_Discussion

WHERE
    ItemID = @ItemID

GO
 



/* PROCEDURE rb_GetSingleMilestones*/
CREATE PROCEDURE rb_GetSingleMilestones
@ItemID int
AS
SELECT
	ItemID,
	ModuleID,
	CreatedByUser,
	CreatedDate,
	Title,
	EstCompleteDate,
	Status
FROM
	rb_Milestones
WHERE
	ItemID = @ItemID

GO
 



CREATE PROCEDURE rb_GetSingleModuleDefinition
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
    rb_GeneralModuleDefinitions
WHERE
    GeneralModDefID = @GeneralModDefID

GO
 



--************************************************************************
CREATE PROCEDURE rb_GetSinglePicture 
(@ItemID int)
AS
SELECT 
	rb_OriginalPictures.ItemID, 
	(
		SELECT TOP 1
			ItemID
		FROM 
			rb_Pictures
		WHERE 
			ModuleID = (SELECT ModuleID FROM rb_Pictures WHERE ItemID = rb_OriginalPictures.ItemID)
			AND ItemID <> rb_OriginalPictures.ItemID
			AND (DisplayOrder < rb_OriginalPictures.DisplayOrder OR (DisplayOrder = rb_OriginalPictures.DisplayOrder AND ItemID < rb_OriginalPictures.ItemID))
		ORDER BY
			rb_OriginalPictures.DisplayOrder - DisplayOrder, rb_OriginalPictures.ItemID - ItemID
	) AS PreviousItemID,
	(
		SELECT TOP 1
			ItemID
		FROM 
			rb_Pictures
		WHERE 
			ModuleID = (SELECT ModuleID FROM rb_Pictures WHERE ItemID = rb_OriginalPictures.ItemID)
			AND ItemID <> rb_OriginalPictures.ItemID
			AND (DisplayOrder > rb_OriginalPictures.DisplayOrder OR (DisplayOrder = rb_OriginalPictures.DisplayOrder AND ItemID > rb_OriginalPictures.ItemID))
		ORDER BY
			DisplayOrder - rb_OriginalPictures.DisplayOrder ,	ItemID - rb_OriginalPictures.ItemID 
	) AS NextItemID,
	rb_OriginalPictures.ModuleID, 
	rb_OriginalPictures.DisplayOrder, 
	rb_OriginalPictures.MetadataXml, 
	rb_OriginalPictures.ShortDescription, 
	rb_OriginalPictures.Keywords
FROM 
	rb_Pictures As rb_OriginalPictures
WHERE 
	ItemID = @ItemID

GO
 



CREATE PROCEDURE rb_GetSingleRole
(
    @RoleID int
)
AS

SELECT
    RoleName

FROM
    rb_Roles

WHERE
    RoleID = @RoleID

GO
 



CREATE PROCEDURE rb_GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
	@IDLang	nchar(2) = 'IT'
)
AS

SELECT
	rb_Users.UserID,
	rb_Users.Email,
	rb_Users.Password,
	rb_Users.Name,
	rb_Users.Company,
	rb_Users.Address,
	rb_Users.City,
	rb_Users.Zip,
	rb_Users.CountryID,
	rb_Users.StateID,
	rb_Users.PIva,
	rb_Users.CFiscale,
	rb_Users.Phone,
	rb_Users.Fax,
	rb_Users.SendNewsletter,
	rb_Users.MailChecked,
	rb_Users.PortalID,
	
	
	(SELECT TOP 1 rb_Localize.Description
FROM         rb_Cultures INNER JOIN
                      rb_Localize ON rb_Cultures.CultureCode = rb_Localize.CultureCode INNER JOIN
                      rb_Countries ON rb_Localize.TextKey = 'COUNTRY_' + rb_Countries.CountryID
WHERE     ((rb_Localize.CultureCode = @IDLang) OR
                      (rb_Cultures.NeutralCode = @IDLang)) AND (rb_Countries.CountryID = rb_Users.CountryID))
	
	
	 AS Country
					  
FROM 
	rb_Users LEFT OUTER JOIN
	rb_States ON rb_Users.StateID = rb_States.StateID
	
WHERE
(rb_Users.Email = @Email) AND (rb_Users.PortalID = @PortalID)

GO
 



/* returns all module definitions for a specified solution */
CREATE PROCEDURE rb_GetSolutionModuleDefinitions
(
    @SolutionID  int
)
AS
SELECT *
 
FROM
    rb_SolutionModuleDefinitions
WHERE   
    SolutionsID = @SolutionID

GO
 



CREATE PROCEDURE rb_GetSolutions
AS
SELECT * FROM rb_Solutions

GO
 



CREATE PROCEDURE rb_GetStates
(
	@CountryID nchar(2)
)

AS
SELECT  StateID, 
		Description
FROM    rb_States
WHERE	CountryID = @CountryID
ORDER BY Description


GO
 



CREATE  proc rb_GetTabCrumbs

@TabID int,
@CrumbsXML nvarchar (4000) output

AS

--Variables used to build Crumb XML string
declare @ParentTabID int
declare @TabName as nvarchar(50)
declare @Level int

--First Child in the branch is Crumb 20.  
set @Level =20

--Get First Parent Tab ID if there is one
set @ParentTabID = (select parenttabID from rb_tabs WHERE TabID=@TabID)
--Get TabName of Lowest Child
set @TabName = (select tabname from rb_tabs WHERE TabID=@TabID)
--Build first Crumb
set @CrumbsXML = '<root><crumb TabID=''' + cast(@TabID as varchar) + ''' level=''' + cast(@Level as varchar) + '''>' + @TabName + '</crumb>'

while @ParentTabID is not null
	begin
		set @level=@level - 1
		set @TabID=@parentTabID
		set @ParentTabID=(select ParentTabID from rb_tabs WHERE TabID=@TabID)
		set @tabname = (select tabname from rb_tabs WHERE TabID=@TabID)
		set @CrumbsXML = @CrumbsXML + '<crumb TabID=''' + cast(@TabID as varchar) + ''' level=''' + cast(@Level as varchar) + '''>' + @TabName + '</crumb>'
	end

set @CrumbsXML = @CrumbsXML + '</root>'

GO
 



CREATE PROCEDURE rb_GetTabCustomSettings
(
    @TabID int
)
AS
SELECT
    SettingName,
    SettingValue
FROM
    rb_TabSettings
WHERE
    TabID = @TabID

GO
 



CREATE PROCEDURE rb_GetTabSettings
(
    @TabID   int
)
AS

IF (@TabID > 0)

/* Get Tabs list */
SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
FROM         rb_Tabs
WHERE     (ParentTabID = @TabID)
ORDER BY TabOrder

GO
 



CREATE PROCEDURE rb_GetTabsByPortal
(
    @PortalID   int
)
AS

/* Get Tabs list */
SELECT     TabName, AuthorizedRoles, TabID, TabOrder, ParentTabID, MobileTabName, ShowMobile, PortalID
FROM         rb_Tabs
WHERE     (PortalID = @PortalID)
ORDER BY TabOrder

GO
 



CREATE PROCEDURE rb_GetTabsFlat
(
        @PortalID int
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

FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder as varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END

--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        '(Orphan)' + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    rb_Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.TabID)
                         AND PortalID =@PortalID

-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
select IDENTITY(int,1,2) as ord , cast(TabID as varchar) as TabID into #tabs
from #TabTree
order by nestlevel, TreeOrder

-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID as int)=#TabTree.TabID) 

-- Return Temporary Table
SELECT TabID, parenttabID, tabname, TabOrder, NestLevel
FROM #TabTree 
order by TreeOrder

GO
 



CREATE PROCEDURE rb_GetTabsParent
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

FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder as varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                 AND PortalID =@PortalID
                ORDER BY #TabTree.TabOrder
END

--Get the Orphans
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        '(Orphan)' + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        999999999,
                        '999999999'
                FROM    rb_Tabs 
                WHERE   NOT EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.TabID)
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
 



CREATE PROCEDURE rb_GetTabsinTab
(
	@PortalID int,
	@TabID int
)
AS
SELECT     TabID, TabName, ParentTabID, TabOrder, AuthorizedRoles
FROM         rb_Tabs
WHERE     (ParentTabID = @TabID) AND (PortalID = @PortalID)
ORDER BY TabOrder

GO
 



CREATE PROCEDURE rb_GetThreadMessages
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
    rb_Discussion

WHERE
    LEFT(DisplayOrder, 23) = @Parent
  AND
    (LEN( DisplayOrder ) / 23 ) > 1

ORDER BY
    DisplayOrder

GO
 



CREATE PROCEDURE rb_GetTopLevelMessages
(
    @ModuleID int
)
AS

SELECT
    ItemID,
    DisplayOrder,
    LEFT(DisplayOrder, 23) AS Parent,    
    (SELECT COUNT(*) -1  FROM rb_Discussion Disc2 WHERE LEFT(Disc2.DisplayOrder,LEN(RTRIM(Disc.DisplayOrder))) = Disc.DisplayOrder) AS ChildCount,
    Title,  
    CreatedByUser,
    CreatedDate

FROM 
    rb_Discussion Disc

WHERE 
    ModuleID=@ModuleID
  AND
    (LEN( DisplayOrder ) / 23 ) = 1

ORDER BY
    DisplayOrder

GO
 



CREATE PROCEDURE rb_GetUsers
(
@PortalID int
)
AS

SELECT     UserID, Name, Password, Email, PortalID, Company, Address, City, Zip, CountryID, StateID, PIva, CFiscale, Phone, Fax
FROM         rb_Users
WHERE     (PortalID = @PortalID)
ORDER BY Email


GO
 




CREATE PROCEDURE rb_GetUsersCount
(
    @PortalID		int,
    @UsersCount		int OUTPUT
)
AS

SELECT TOP 1
@UsersCount = COUNT(DISTINCT rb_Users.UserID)
FROM  rb_Users
WHERE rb_Users.PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_GetUsersNewsletter
(
@PortalID int,
@MaxUsers int = 250,
@MinSend float = 30, /* 1 = 1 day, users which send was made in x days will be ignored */    
@UserCount int = 0 OUTPUT
)
AS

/* 24 hours min delay */
IF @MinSend < 1 SELECT @MinSend = 1 

SELECT
TOP 250
	UserID, 
	Name, 
	Password, 
	Email, 
	PortalID
FROM
	rb_Users
WHERE
	(SendNewsletter = 1) AND 
	(CAST(COALESCE (LastSend, GETDATE() - @MinSend) AS float) <= CAST(GETDATE() - @MinSend AS float)) AND
        (PortalID = @PortalID) AND (NOT (Email IN (SELECT EMAIL FROM rb_BlackList WHERE PortalID = @PortalID)))
ORDER BY UserID

SELECT @UserCount = @@ROWCOUNT

GO
 



-- Refresh procedure
CREATE PROCEDURE rb_LocalizeManager
(
	@Key			nvarchar(50),
	@Translation	nvarchar(255) = null,
	@SectionType	varchar(150) = '',
	@ControlType	varchar(150) = '',
	@ControlId		varchar(150) = ''
)
AS

DECLARE @DefaultLanguage varchar(10)
SET @DefaultLanguage = 'en'

IF NOT EXISTS 
(
SELECT    TextKey
FROM      rb_Localize
WHERE     (TextKey = @Key) AND (CultureCode = @DefaultLanguage)
)
INSERT rb_Localize (TextKey, CultureCode, Description) Values (@Key, @DefaultLanguage, @Translation)

ELSE

UPDATE rb_Localize
SET Description = @Translation
WHERE (TextKey = @Key) AND (CultureCode = @DefaultLanguage)

--Update locations
IF NOT EXISTS 
(
SELECT    TextKey
FROM      rb_Sections
WHERE     (TextKey = @Key) AND (SectionType = @SectionType AND ControlType = @ControlType AND ControlID = @ControlId)
)
INSERT rb_Sections (TextKey, SectionType, ControlType, ControlId) Values (@Key, @SectionType, @ControlType, @ControlId)

GO
 



-- Alter Publish stored procedure

CREATE  PROCEDURE rb_Publish
	@ModuleID	int
AS

	-- First get al list of tables which are related to the Modules table

	-- Create a temporary table
	CREATE TABLE #TMPResults
		(ForeignKeyTableSchema	nvarchar(128),
                 ForeignKeyTable	nvarchar(128),
		 ForeignKeyColumn	nvarchar(128),
		 KeyColumn		nvarchar(128),
		 ForeignKeyTableId	int,
		 KeyTableId		int,
		 KeyTableSchema		nvarchar(128),
		 KeyTable		nvarchar(128))

	INSERT INTO #TMPResults EXEC rb_GetRelatedTables 'Modules'

	DECLARE RelatedTablesModules CURSOR FOR
		SELECT 	
			ForeignKeyTableSchema, 
			ForeignKeyTable,
			ForeignKeyColumn,
			KeyColumn,
			ForeignKeyTableId,
			KeyTableId,
			KeyTableSchema,
			KeyTable
		FROM
			#TMPResults
		WHERE 
			ForeignKeyTable <> 'ModuleSettings'

	-- Create temporary table for later use
	CREATE TABLE #TMPCount
		(ResultCount	int)


	-- Now search the table that has the related column
	OPEN RelatedTablesModules

	DECLARE @ForeignKeyTableSchema 	nvarchar(128)
	DECLARE @ForeignKeyTable	nvarchar(128)
	DECLARE @ForeignKeyColumn	nvarchar(128)
	DECLARE @KeyColumn		nvarchar(128)
	DECLARE @ForeignKeyTableId	int
	DECLARE @KeyTableId		int
	DECLARE @KeyTableSchema		nvarchar(128)
	DECLARE @KeyTable		nvarchar(128)
	DECLARE @SQLStatement		nvarchar(4000)
	DECLARE @Count			int
	DECLARE @TableHasIdentityCol	int

	FETCH NEXT FROM RelatedTablesModules 
	INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
		@KeyColumn, @ForeignKeyTableId, @KeyTableId,
		@KeyTableSchema, @KeyTable
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Check if this table has a corresponding staging table
		TRUNCATE TABLE #TMPCount
		SET @SQLStatement = 'INSERT INTO #TMPCount SELECT Count(*) FROM [sysobjects] WHERE [id] = object_id(N''[rb_st_' + @ForeignKeyTable + ']'') AND OBJECTPROPERTY([id], N''IsUserTable'') = 1'
		-- PRINT @SQLStatement
		EXEC(@SQLStatement)
		SELECT @Count = ResultCount
			FROM #TMPCount		
		PRINT @ForeignKeyTable
		PRINT @Count
		IF (@Count = 1)
		BEGIN						
			-- Check if this table contains the related data
			TRUNCATE TABLE #TMPCount
			SET @SQLStatement = 
				'INSERT INTO #TMPCount ' +
				'SELECT Count(*) FROM [' + @ForeignKeyTable + '] ' +
				'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) +
				'UNION ' +
				'SELECT Count(*) FROM [rb_st_' + @ForeignKeyTable + '] ' +
				'WHERE [rb_st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID) 

			EXEC(@SQLStatement)
			SELECT @Count = ResultCount
				FROM #TMPCount		
			IF (@Count >= 1) 
			BEGIN
				-- This table contains the related data 
				-- Delete everything in the prod table
				SET @SQLStatement = 
					'DELETE FROM [' + @ForeignKeyTable + '] ' +
					'WHERE [' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
				PRINT @SQLStatement
				EXEC(@SQLStatement)
				-- Copy everything from the staging table to the prod table
				SET @TableHasIdentityCol = OBJECTPROPERTY(@ForeignKeyTableId, 'TableHasIdentity')
				IF (@TableHasIdentityCol = 1)
				BEGIN
					-- The table contains a identity column
					DECLARE TableColumns CURSOR FOR
						SELECT [COLUMN_NAME]
						FROM [INFORMATION_SCHEMA].[COLUMNS]
						WHERE [TABLE_SCHEMA] = @ForeignKeyTableSchema 
							AND [TABLE_NAME] = @ForeignKeyTable
						ORDER BY [ORDINAL_POSITION]
	
					OPEN TableColumns
	
					DECLARE @ColumnList	nvarchar(4000)
					SET @ColumnList = ''
					DECLARE @ColName	nvarchar(128)
					DECLARE @ColIsIdentity	int
	
					FETCH NEXT FROM TableColumns
					INTO @ColName
					
					SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
	
					WHILE @@FETCH_STATUS = 0
					BEGIN
						IF (@ColIsIdentity = 0)
							SET @ColumnList = @ColumnList + '[' + @ColName + '] '
	
						FETCH NEXT FROM TableColumns
						INTO @ColName
	
						IF (@@FETCH_STATUS = 0)
						BEGIN
							IF (@ColIsIdentity = 0)
							BEGIN
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
								IF (@ColIsIdentity = 0)
									SET @ColumnList = @ColumnList + ', '		
							END
							ELSE
								SET @ColIsIdentity = COLUMNPROPERTY(@ForeignKeyTableId, @ColName, 'IsIdentity')
						END
					END
					
					CLOSE TableColumns
					DEALLOCATE TableColumns		
	
					SET @SQLStatement = 	
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] (' + @ColumnList + ') ' +
						'SELECT ' + @ColumnList + ' FROM [rb_st_' + @ForeignKeyTable + '] ' +
						'WHERE [rb_st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					-- PRINT @SQLStatement
					EXEC(@SQLStatement)
				END
				ELSE
				BEGIN
					-- The table doens't contain a identitycolumn
					SET @SQLStatement = 
						'INSERT INTO [' + @ForeignKeyTableSchema + '].[' + @ForeignKeyTable + '] ' +
						'SELECT * FROM [rb_st_' + @ForeignKeyTable + '] ' +
						'WHERE [rb_st_' + @ForeignKeyTable + '].[' + @ForeignKeyColumn + '] = ' + CONVERT(varchar(20), @ModuleID)
					EXEC(@SQLStatement)
				END
			END
		END

		FETCH NEXT FROM RelatedTablesModules 
		INTO @ForeignKeyTableSchema, @ForeignKeyTable, @ForeignKeyColumn, 
			@KeyColumn, @ForeignKeyTableId, @KeyTableId,
			@KeyTableSchema, @KeyTable
	END


	CLOSE RelatedTablesModules
	DEALLOCATE RelatedTablesModules
			
	-- Set the module in the correct status
	UPDATE [rb_Modules]
	SET [WorkflowState] = 0, -- Original
	    [LastModified] = [StagingLastModified],
            [LastEditor] = [StagingLastEditor]
	WHERE [ModuleID] = @ModuleID

	RETURN

GO
 



-- Alter ModuleEdited stored procedure
CREATE    PROCEDURE rb_ModuleEdited
	@ModuleID	int
AS

	-- Check if this module supports workflow
	DECLARE @support	bit

	SELECT @support = SupportWorkflow
	FROM rb_Modules
	WHERE ModuleID = @ModuleID

	IF ( @support = 1 )
	BEGIN
		-- It supports workflow
		UPDATE rb_Modules
		SET WorkflowState = 1 -- Working
		WHERE ModuleID = @ModuleID
	END
	ELSE
		-- It doesn't support workflow
		EXEC rb_Publish @ModuleID

	/* SET NOCOUNT ON */
	RETURN 

GO
 



CREATE PROCEDURE rb_Reject
	@moduleID	int
AS
	UPDATE	rb_Modules
	SET
		WorkflowState = 1 -- Put status back to Working
	WHERE
		[ModuleID] = @moduleID

GO
 



CREATE PROCEDURE rb_RequestApproval
	@moduleID	int
AS
	UPDATE	rb_Modules
	SET
		WorkflowState = 2 -- Request Approval
	WHERE
		[ModuleID] = @moduleID

GO
 



CREATE PROCEDURE rb_SendNewsletterTo
(
@PortalID int,
@Email nvarchar(100)
)
AS 
UPDATE rb_Users SET LastSend = GETDATE() WHERE PortalID=@PortalID AND Email=@Email
select 0

GO
 



CREATE PROCEDURE rb_SetLastModified
	(
		@ModuleID int,
		@LastModifiedBy	nvarchar(256)
	)
AS

	-- Check if this module supports workflow
	DECLARE @support	bit

	SELECT @support = SupportWorkflow
	FROM rb_Modules
	WHERE ModuleID = @ModuleID

	IF ( @support = 1 )
	BEGIN
		-- It supports workflow
		UPDATE rb_Modules
		SET StagingLastModified = getdate(),
                    StagingLastEditor = @LastModifiedBy
		WHERE ModuleID = @ModuleID
	END
	ELSE
		-- It doesn't support workflow
		UPDATE rb_Modules
		SET LastModified = getdate(),
                    LastEditor = @LastModifiedBy,
		    StagingLastModified = getdate(),
		    StagingLastEditor = @LastModifiedBy
		WHERE ModuleID = @ModuleID

	/* SET NOCOUNT ON */
	RETURN 

GO
 






/****** Oggetto: stored procedure rb_UpdateAnnouncement    Data dello script: 07/11/2002 22.28.10 ******/



CREATE  PROCEDURE rb_UpdateAnnouncement
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
    rb_st_Announcements

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
 



CREATE PROCEDURE rb_UpdateArticle
(
    @ItemID         int,
    @ModuleID       int,
    @UserName       nvarchar(100),
    @Title          nvarchar(100),
    @Subtitle       nvarchar(200),
    @Abstract       nvarchar(512),
    @Description    text,
    @StartDate      datetime,
    @ExpireDate     datetime,
    @IsInNewsletter bit,
    @MoreLink       nvarchar(150)
)
AS

UPDATE rb_Articles

SET 
ModuleID = @ModuleID,
CreatedByUser = @UserName,
CreatedDate = GetDate(),
Title =@Title ,
Subtitle =  @Subtitle,
Abstract =@Abstract,
Description =@Description,
StartDate = @StartDate,
ExpireDate =@ExpireDate,
IsInNewsletter = @IsInNewsletter,
MoreLink =@MoreLink
WHERE 
ItemID = @ItemID

GO
 






/****** Oggetto: stored procedure rb_UpdateContact    Data dello script: 07/11/2002 22.28.14 ******/


CREATE  PROCEDURE rb_UpdateContact
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
    rb_st_Contacts

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

CREATE   PROCEDURE rb_UpdateDocument
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
        rb_st_Documents 
    WHERE 
        ItemID = @ItemID
)
INSERT INTO rb_st_Documents
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
    rb_st_Documents

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
    rb_st_Documents

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
 






/****** Oggetto: stored procedure rb_UpdateEvent    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE rb_UpdateEvent
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
    rb_st_Events

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
 



/* PROCEDURE rb_UpdateGeneralModuleDefinitions*/
CREATE PROCEDURE rb_UpdateGeneralModuleDefinitions
	@GeneralModDefID uniqueidentifier,
	@FriendlyName nvarchar(128),
	@DesktopSrc nvarchar(256),
	@MobileSrc nvarchar(256),
	@AssemblyName varchar(50),
	@ClassName nvarchar(128),
	@Admin bit,
	@Searchable bit
AS
UPDATE rb_GeneralModuleDefinitions
SET
	FriendlyName = @FriendlyName,
	DesktopSrc = @DesktopSrc,
	MobileSrc = @MobileSrc,
	AssemblyName = @AssemblyName,
	ClassName = @ClassName,
	Admin = @Admin,
	Searchable = @Searchable
WHERE
	GeneralModDefID = @GeneralModDefID

GO
 






/****** Oggetto: stored procedure rb_UpdateHtmlText    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE rb_UpdateHtmlText
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
        rb_st_HtmlText 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO rb_st_HtmlText (
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
    rb_st_HtmlText

SET
    DesktopHtml   = @DesktopHtml,
    MobileSummary = @MobileSummary,
    MobileDetails = @MobileDetails

WHERE
    ModuleID = @ModuleID







GO
 






/****** Oggetto: stored procedure rb_UpdateLink    Data dello script: 07/11/2002 22.28.14 ******/



CREATE  PROCEDURE rb_UpdateLink
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @Url         nvarchar(250),
    @MobileUrl   nvarchar(250),
    @ViewOrder   int,
    @Description nvarchar(2000),
    @Target		 nvarchar(10)
)
AS

UPDATE
    rb_st_Links

SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    Url           = @Url,
    MobileUrl     = @MobileUrl,
    ViewOrder     = @ViewOrder,
    Description   = @Description,
    Target		  = @Target

WHERE
    ItemID = @ItemID







GO
 



/* PROCEDURE rb_UpdateMilestones*/
CREATE PROCEDURE rb_UpdateMilestones
	@ItemID int,
	@ModuleID int,
	@CreatedByUser nvarchar(100),
	@CreatedDate datetime,
	@Title nvarchar(100),
	@EstCompleteDate datetime,
	@Status nvarchar(100)
AS
UPDATE rb_Milestones
SET
	ModuleID = @ModuleID,
	CreatedByUser = @CreatedByUser,
	CreatedDate = @CreatedDate,
	Title = @Title,
	EstCompleteDate = @EstCompleteDate,
	Status = @Status
WHERE
	ItemID = @ItemID

GO
 



CREATE PROCEDURE [rb_UpdateModule]
(
    @ModuleID               int,
    @TabID				    int,
    @ModuleOrder            int,
    @ModuleTitle            nvarchar(256),
    @PaneName               nvarchar(50),
    @CacheTime              int,
    @EditRoles              nvarchar(256),
    @AddRoles               nvarchar(256),
    @ViewRoles              nvarchar(256),
    @DeleteRoles            nvarchar(256),
    @PropertiesRoles        nvarchar(256),
    @ShowMobile             bit,
    @PublishingRoles	    nvarchar(256),
    @SupportWorkflow	    bit,
    @ApprovalRoles			nvarchar(256)
)
AS
UPDATE
    rb_Modules
SET
	TabID					  = @TabID,
    ModuleOrder               = @ModuleOrder,
    ModuleTitle               = @ModuleTitle,
    PaneName                  = @PaneName,
    CacheTime                 = @CacheTime,
    ShowMobile                = @ShowMobile,
    AuthorizedEditRoles       = @EditRoles,
    AuthorizedAddRoles        = @AddRoles,
    AuthorizedViewRoles       = @ViewRoles,
    AuthorizedDeleteRoles     = @DeleteRoles,
    AuthorizedPropertiesRoles = @PropertiesRoles,
    AuthorizedPublishingRoles = @PublishingRoles,
    SupportWorkflow	          = @SupportWorkflow,
    AuthorizedApproveRoles    = @ApprovalRoles
    
WHERE
    ModuleID = @ModuleID

GO
 



CREATE PROCEDURE rb_UpdateModuleDefinitions
(
    @GeneralModDefID	uniqueidentifier,
    @PortalID			int,
    @ischecked			bit
)
AS

IF (@ischecked = 0)
	/*DELETE IF CLEARED */
	DELETE FROM rb_ModuleDefinitions WHERE rb_ModuleDefinitions.GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID
	
ELSE
IF NOT (EXISTS (SELECT ModuleDefID FROM rb_ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID AND PortalID = @PortalID))
	/* ADD IF CHECKED */
BEGIN
			INSERT INTO rb_ModuleDefinitions
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
 



CREATE PROCEDURE rb_UpdateModuleOrder
(
    @ModuleID           int,
    @ModuleOrder        int,
    @PaneName           nvarchar(50)
)
AS
UPDATE
    rb_Modules
SET
    ModuleOrder = @ModuleOrder,
    PaneName    = @PaneName
WHERE
    ModuleID = @ModuleID

GO
 



CREATE PROCEDURE rb_UpdateModuleSetting
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
 



--************************************************************************
CREATE PROCEDURE [rb_UpdatePicture]
	(@ItemID 	[int ],
	 @ModuleID 	[int],
	 @DisplayOrder	[int],
	 @MetadataXml VARCHAR(6000),
	 @ShortDescription VARCHAR(256),
	 @Keywords VARCHAR(256)
)
AS 
UPDATE [rb_Pictures]
SET  
	 [DisplayOrder] 		= @DisplayOrder,
	 [MetadataXml]		= @MetadataXml,
	 [ShortDescription]	= @ShortDescription,
	 [Keywords]		= @Keywords
WHERE 
	( [ItemID]	 = @ItemID)
/* end PictureAlbum 2.0 mods */

GO
 



CREATE PROCEDURE rb_UpdatePortalInfo
(
    @PortalID           int,
    @PortalName         nvarchar(128),
    @PortalPath         nvarchar(128),
    @AlwaysShowEditButton bit 
)
AS

UPDATE
    rb_Portals

SET
    PortalName = @PortalName,
    PortalPath = @PortalPath,
    AlwaysShowEditButton = @AlwaysShowEditButton

WHERE
    PortalID = @PortalID

GO
 



CREATE PROCEDURE rb_UpdatePortalSetting
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
 



CREATE PROCEDURE rb_UpdateRole
(
    @RoleID      int,
    @RoleName    nvarchar(50)
)
AS

UPDATE
    rb_Roles

SET
    RoleName = @RoleName

WHERE
    RoleID = @RoleID

GO
 

---------------------
--1.2.8.1522.sql
---------------------

--Update Stored PROCEDURE: rb_UpdateTab
--Prevents orphaning a tab or placing tabs in an infinte recursive loop
--26 dec 2002 - Cory Isakson
CREATE PROCEDURE rb_UpdateTab
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
        rb_Tabs
    WHERE 
        TabID = @TabID
)
INSERT INTO rb_Tabs (
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
--Updated 26.Dec.2002 Cory Isakson
--Check the Tab recursion so Tab is not placed into an infinate loop when building Tab Tree
BEGIN TRAN
--If the Update breaks the tab from having a path back to the root then do not Update
UPDATE
    rb_Tabs
SET
    ParentTabID = @ParentTabID,
    TabOrder = @TabOrder,
    TabName = @TabName,
    AuthorizedRoles = @AuthorizedRoles,
    MobileTabName = @MobileTabName,
    ShowMobile = @ShowMobile
WHERE
    TabID = @TabID

--Create a Temporary table to hold the tabs
CREATE TABLE #TabTree
(
	[TabID] [int],
	[TabName] [nvarchar] (50),
	[ParentTabID] [int],
	[TabOrder] [int],
	[NestLevel] [int],
	[TreeOrder] [varchar] (1000)
)

SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0

-- First, the parent levels
INSERT INTO	#TabTree
SELECT 	TabID,
	TabName,
	ParentTabID,
	TabOrder,
	0,
	cast(100000000 + TabOrder as varchar)
FROM	rb_Tabs
WHERE	ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder

-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT 	#TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
		SELECT 	rb_Tabs.TabID,
			Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
			rb_Tabs.ParentTabID,
			rb_Tabs.TabOrder,
			@LastLevel,
			cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder as varchar)
		FROM	rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
		WHERE	EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
		 AND PortalID =@PortalID
		ORDER BY #TabTree.TabOrder
END

--Check that the Tab is found in the Tree.  If it is not then we abort the Update
IF NOT EXISTS (SELECT TabID from #TabTree WHERE TabID=@TabID)
BEGIN
	ROLLBACK TRAN
	--If we want to modify the TabLayout code then we can throw an error AND catch it.
	RAISERROR('Not allowed to choose that parent.',11,1)
END
ELSE
COMMIT TRAN
--End changes 26.Dec.2002 Cory Isakson

GO
 



CREATE PROCEDURE rb_UpdateTabCustomSettings
(
    @TabID int,
    @SettingName   nvarchar(50),
    @SettingValue  nvarchar(256)
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
 



CREATE PROCEDURE rb_UpdateTabOrder
(
    @TabID           int,
    @TabOrder        int
)
AS

UPDATE
    rb_Tabs

SET
    TabOrder = @TabOrder

WHERE
    TabID = @TabID

GO
 



CREATE PROCEDURE rb_UpdateUser
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
    rb_Users

SET
    Name	 = @Name,
    Email    = @Email,
    Password = @Password,
    PortalID = @PortalID,
    SendNewsletter = @SendNewsletter

WHERE
    UserID    = @UserID

GO
 




CREATE PROCEDURE rb_UpdateUserCheckEmail
(
    @UserID		    int,
    @CheckedEmail	tinyint
)
AS

UPDATE rb_Users

SET

    MailChecked = @CheckedEmail

WHERE UserID = @UserID

GO
 



CREATE PROCEDURE rb_UpdateUserFull
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
	@CountryID	nchar(2),  
	@StateID		int
)
AS

UPDATE rb_Users

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
CountryID = @CountryID,
StateID = @StateID

WHERE UserID = @UserID

GO
 



CREATE PROCEDURE rb_UserLogin
(
    @PortalID int,
    @Email    nvarchar(100),
    @Password nvarchar(20),
    @UserName nvarchar(100) OUTPUT
)
AS

SELECT     @UserName = rb_Users.Name
FROM      rb_Users
WHERE
	(
	rb_Users.Email = @Email AND rb_Users.Password = @Password AND rb_Users.PortalID = @PortalID
	)

GO
 



CREATE PROCEDURE [rb_GetCurrentDbVersion]
AS
SELECT     TOP 1 Release
FROM         rb_Versions
ORDER BY Release DESC

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
SET @NewModuleDefID = (SELECT TOP 1 ModuleDefID FROM rb_ModuleDefinitions WHERE GeneralModDefID = @NewModuleGuid)
DECLARE @OldModuleDefID int
SET @OldModuleDefID = (SELECT TOP 1 ModuleDefID FROM rb_ModuleDefinitions WHERE GeneralModDefID = @OldModuleGuid)

--Update modules
UPDATE rb_Modules
SET ModuleDefID = @NewModuleDefID WHERE ModuleDefID = @OldModuleDefID

-- Drop old definition
DELETE rb_ModuleDefinitions
WHERE ModuleDefID = @OldModuleDefID

DELETE rb_GeneralModuleDefinitions
WHERE GeneralModDefID = @OldModuleGuid
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO


