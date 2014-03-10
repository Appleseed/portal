INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1601','1.2.8.1601', CONVERT(datetime, '04/01/2003', 101))
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Tabs_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Tabs] DROP CONSTRAINT FK_Tabs_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Cultures_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Cultures] DROP CONSTRAINT FK_Cultures_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_States_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_States] DROP CONSTRAINT FK_States_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Users] DROP CONSTRAINT FK_Users_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_ModuleDefinitions_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_ModuleDefinitions] DROP CONSTRAINT FK_ModuleDefinitions_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_SolutionModuleDefinitions_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_SolutionModuleDefinitions] DROP CONSTRAINT FK_SolutionModuleDefinitions_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Modules_ModuleDefinitions1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Modules] DROP CONSTRAINT FK_Modules_ModuleDefinitions1
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Announcements_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Announcements] DROP CONSTRAINT FK_Announcements_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Articles_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Articles] DROP CONSTRAINT FK_Articles_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Contacts_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Contacts] DROP CONSTRAINT FK_Contacts_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Discussion_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Discussion] DROP CONSTRAINT FK_Discussion_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Documents_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Documents] DROP CONSTRAINT FK_Documents_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Events_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Events] DROP CONSTRAINT FK_Events_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_HtmlText_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_HtmlText] DROP CONSTRAINT FK_HtmlText_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Links_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Links] DROP CONSTRAINT FK_Links_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Milestones_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Milestones] DROP CONSTRAINT FK_Milestones_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_ModuleSettings_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_ModuleSettings] DROP CONSTRAINT FK_ModuleSettings_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Pictures_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Pictures] DROP CONSTRAINT FK_Pictures_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_Announcements_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_Announcements] DROP CONSTRAINT FK_st_Announcements_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_Contacts_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_Contacts] DROP CONSTRAINT FK_st_Contacts_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_Documents_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_Documents] DROP CONSTRAINT FK_st_Documents_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_Events_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_Events] DROP CONSTRAINT FK_st_Events_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_HtmlText_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_HtmlText] DROP CONSTRAINT FK_st_HtmlText_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_st_Links_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_st_Links] DROP CONSTRAINT FK_st_Links_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_PortalSettings_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_PortalSettings] DROP CONSTRAINT FK_PortalSettings_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Roles_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Roles] DROP CONSTRAINT FK_Roles_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Users] DROP CONSTRAINT FK_Users_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_UserRoles_Roles]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_UserRoles] DROP CONSTRAINT FK_UserRoles_Roles
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_SolutionModuleDefintions_Solutions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_SolutionModuleDefinitions] DROP CONSTRAINT FK_SolutionModuleDefintions_Solutions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Users_States]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Users] DROP CONSTRAINT FK_Users_States
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Modules_Tabs1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Modules] DROP CONSTRAINT FK_Modules_Tabs1
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Tabs_Tabs]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Tabs] DROP CONSTRAINT FK_Tabs_Tabs
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_TabSettings_Tabs]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_TabSettings] DROP CONSTRAINT FK_TabSettings_Tabs
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_UserRoles_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_UserRoles] DROP CONSTRAINT FK_UserRoles_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_AnnouncementsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_AnnouncementsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_ContactsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_ContactsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_DocumentsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_DocumentsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_EventsModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_EventsModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_HtmlTextModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_HtmlTextModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_LinksModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [st_LinksModified]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Articles]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Articles] DROP CONSTRAINT PK_Articles
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Blacklist]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Blacklist] DROP CONSTRAINT PK_Blacklist
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Countries]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Countries] DROP CONSTRAINT PK_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Cultures]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Cultures] DROP CONSTRAINT PK_Cultures
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_GeneralModuleDefinitions] DROP CONSTRAINT PK_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Milestones]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Milestones] DROP CONSTRAINT PK_Milestones
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_ModuleSettings]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_ModuleSettings] DROP CONSTRAINT PK_ModuleSettings
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_PortalSettings]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_PortalSettings] DROP CONSTRAINT PK_PortalSettings
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_SolutionModuleDefintions]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_SolutionModuleDefinitions] DROP CONSTRAINT PK_SolutionModuleDefintions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Solutions]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Solutions] DROP CONSTRAINT PK_Solutions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_States]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_States] DROP CONSTRAINT PK_States
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_TabSettings]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_TabSettings] DROP CONSTRAINT PK_TabSettings
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Announcements] DROP CONSTRAINT PK_Announcements
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Contacts] DROP CONSTRAINT PK_Contacts
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Discussion]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Discussion] DROP CONSTRAINT PK_Discussion
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Documents] DROP CONSTRAINT PK_Documents
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Events] DROP CONSTRAINT PK_Events
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF__GeneralMo__Assem__41B8C09B]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_GeneralModuleDefinitions] DROP CONSTRAINT DF__GeneralMo__Assem__41B8C09B
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_GeneralModuleDefinitions_Admin1]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_GeneralModuleDefinitions] DROP CONSTRAINT DF_GeneralModuleDefinitions_Admin1
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_GeneralModuleDefinitions_Searchable1]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_GeneralModuleDefinitions] DROP CONSTRAINT DF_GeneralModuleDefinitions_Searchable1
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[IX_GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_GeneralModuleDefinitions] DROP CONSTRAINT IX_GeneralModuleDefinitions
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_HtmlText] DROP CONSTRAINT PK_HtmlText
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Target]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Links] DROP CONSTRAINT Target
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Links] DROP CONSTRAINT PK_Links
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_ModuleDefinitions] DROP CONSTRAINT PK_ModuleDefinitions
GO

DROP INDEX [rb_ModuleSettings].[IX_ModuleSettings]

GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_Modules_NewVersion]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Modules] DROP CONSTRAINT DF_Modules_NewVersion
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_Modules_WorkflowState]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Modules] DROP CONSTRAINT DF_Modules_WorkflowState
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Modules]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Modules] DROP CONSTRAINT PK_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Pictures]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Pictures] DROP CONSTRAINT PK_Pictures
GO

DROP INDEX [rb_PortalSettings].[IX_PortalSettings]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Portals]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Portals] DROP CONSTRAINT PK_Portals
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[IX_Portals]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Portals] DROP CONSTRAINT IX_Portals
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_Roles_Permission]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Roles] DROP CONSTRAINT DF_Roles_Permission
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Roles]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Roles] DROP CONSTRAINT PK_Roles
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[IX_Roles]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Roles] DROP CONSTRAINT IX_Roles
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Tabs]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Tabs] DROP CONSTRAINT PK_Tabs
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_Users_SendNewsletter]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Users] DROP CONSTRAINT DF_Users_SendNewsletter
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[DF_Users_MailChecked]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Users] DROP CONSTRAINT DF_Users_MailChecked
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_Users]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_Users] DROP CONSTRAINT PK_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[IX_Users]') AND OBJECTPROPERTY(id, N'IsConstraint') = 1)
ALTER TABLE  [rb_Users] DROP CONSTRAINT IX_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Announcements]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_Announcements] DROP CONSTRAINT PK_st_Announcements
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Contacts]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_Contacts] DROP CONSTRAINT PK_st_Contacts
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Documents]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_Documents] DROP CONSTRAINT PK_st_Documents
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Events]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_Events] DROP CONSTRAINT PK_st_Events
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_HtmlText]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_HtmlText] DROP CONSTRAINT PK_st_HtmlText
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PK_st_Links]') AND OBJECTPROPERTY(id, N'IsPrimaryKey') = 1)
ALTER TABLE  [rb_st_Links] DROP CONSTRAINT PK_st_Links
GO

ALTER TABLE [rb_Articles] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Articles] PRIMARY KEY  CLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Blacklist] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Blacklist] PRIMARY KEY  CLUSTERED 
	(
		[PortalID],
		[Email]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Countries] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Countries] PRIMARY KEY  CLUSTERED 
	(
		[CountryID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Cultures] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Cultures] PRIMARY KEY  CLUSTERED 
	(
		[CultureCode]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_GeneralModuleDefinitions] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_GeneralModuleDefinitions] PRIMARY KEY  CLUSTERED 
	(
		[GeneralModDefID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Milestones] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Milestones] PRIMARY KEY  CLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_ModuleSettings] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_ModuleSettings] PRIMARY KEY  CLUSTERED 
	(
		[ModuleID],
		[SettingName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_PortalSettings] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_PortalSettings] PRIMARY KEY  CLUSTERED 
	(
		[PortalID],
		[SettingName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_SolutionModuleDefinitions] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_SolutionModuleDefintions] PRIMARY KEY  CLUSTERED 
	(
		[SolutionModDefID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Solutions] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_Solutions] PRIMARY KEY  CLUSTERED 
	(
		[SolutionsID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_States] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_States] PRIMARY KEY  CLUSTERED 
	(
		[StateID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_TabSettings] WITH NOCHECK ADD 
	CONSTRAINT [PK_rb_TabSettings] PRIMARY KEY  CLUSTERED 
	(
		[TabID],
		[SettingName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Announcements] ADD 
	CONSTRAINT [PK_rb_Announcements] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Contacts] ADD 
	CONSTRAINT [PK_rb_Contacts] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Discussion] ADD 
	CONSTRAINT [PK_rb_Discussion] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Documents] ADD 
	CONSTRAINT [PK_rb_Documents] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Events] ADD 
	CONSTRAINT [PK_rb_Events] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_GeneralModuleDefinitions] ADD 
	CONSTRAINT [DF__rb_GeneralMo__Assem__41B8C09B] DEFAULT ('Appleseed.DLL') FOR [AssemblyName],
	CONSTRAINT [DF_rb_GeneralModuleDefinitions_Admin1] DEFAULT (0) FOR [Admin],
	CONSTRAINT [DF_rb_GeneralModuleDefinitions_Searchable1] DEFAULT (0) FOR [Searchable],
	CONSTRAINT [IX_rb_GeneralModuleDefinitions] UNIQUE  NONCLUSTERED 
	(
		[FriendlyName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_HtmlText] ADD 
	CONSTRAINT [PK_rb_HtmlText] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Links] ADD 
	CONSTRAINT [Target] DEFAULT ('_new') FOR [Target],
	CONSTRAINT [PK_rb_Links] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_ModuleDefinitions] ADD 
	CONSTRAINT [PK_rb_ModuleDefinitions] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleDefID]
	)  ON [PRIMARY] 
GO


 CREATE  INDEX [IX_rb_ModuleSettings] ON [rb_ModuleSettings]([ModuleID], [SettingName]) ON [PRIMARY]
GO

ALTER TABLE [rb_Modules] ADD 
	CONSTRAINT [DF_rb_Modules_NewVersion] DEFAULT (1) FOR [NewVersion],
	CONSTRAINT [DF_rb_Modules_WorkflowState] DEFAULT (0) FOR [WorkflowState],
	CONSTRAINT [PK_rb_Modules] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Pictures] ADD 
	CONSTRAINT [PK_rb_Pictures] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_rb_PortalSettings] ON [rb_PortalSettings]([PortalID], [SettingName]) ON [PRIMARY]
GO

ALTER TABLE [rb_Portals] ADD 
	CONSTRAINT [PK_rb_Portals] PRIMARY KEY  NONCLUSTERED 
	(
		[PortalID]
	)  ON [PRIMARY] ,
	CONSTRAINT [IX_rb_Portals] UNIQUE  NONCLUSTERED 
	(
		[PortalAlias]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Roles] ADD 
	CONSTRAINT [DF_rb_Roles_Permission] DEFAULT (1) FOR [Permission],
	CONSTRAINT [PK_rb_Roles] PRIMARY KEY  NONCLUSTERED 
	(
		[RoleID]
	)  ON [PRIMARY] ,
	CONSTRAINT [IX_rb_Roles] UNIQUE  NONCLUSTERED 
	(
		[PortalID],
		[RoleName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Tabs] ADD 
	CONSTRAINT [PK_rb_Tabs] PRIMARY KEY  NONCLUSTERED 
	(
		[TabID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Users] ADD 
	CONSTRAINT [DF_rb_Users_SendNewsletter] DEFAULT (1) FOR [SendNewsletter],
	CONSTRAINT [DF_rb_Users_MailChecked] DEFAULT (0) FOR [MailChecked],
	CONSTRAINT [PK_rb_Users] PRIMARY KEY  NONCLUSTERED 
	(
		[UserID]
	)  ON [PRIMARY] ,
	CONSTRAINT [IX_rb_Users] UNIQUE  NONCLUSTERED 
	(
		[Email],
		[PortalID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_Announcements] ADD 
	CONSTRAINT [PK_rb_st_Announcements] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_Contacts] ADD 
	CONSTRAINT [PK_rb_st_Contacts] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_Documents] ADD 
	CONSTRAINT [PK_rb_st_Documents] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_Events] ADD 
	CONSTRAINT [PK_rb_st_Events] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_HtmlText] ADD 
	CONSTRAINT [PK_rb_st_HtmlText] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_st_Links] ADD 
	CONSTRAINT [PK_rb_st_Links] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Announcements] ADD 
	CONSTRAINT [FK_rb_Announcements_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Articles] ADD 
	CONSTRAINT [FK_rb_Articles_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE 
GO

ALTER TABLE [rb_Contacts] ADD 
	CONSTRAINT [FK_rb_Contacts_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Cultures] ADD 
	CONSTRAINT [FK_rb_Cultures_rb_Countries] FOREIGN KEY 
	(
		[CountryID]
	) REFERENCES [rb_Countries] (
		[CountryID]
	)
GO

ALTER TABLE [rb_Discussion] ADD 
	CONSTRAINT [FK_rb_Discussion_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Documents] ADD 
	CONSTRAINT [FK_rb_Documents_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Events] ADD 
	CONSTRAINT [FK_rb_Events_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_HtmlText] ADD 
	CONSTRAINT [FK_rb_HtmlText_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Links] ADD 
	CONSTRAINT [FK_rb_Links_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Milestones] ADD 
	CONSTRAINT [FK_rb_Milestones_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	)
GO

ALTER TABLE [rb_ModuleDefinitions] ADD 
	CONSTRAINT [FK_rb_ModuleDefinitions_rb_GeneralModuleDefinitions] FOREIGN KEY 
	(
		[GeneralModDefID]
	) REFERENCES [rb_GeneralModuleDefinitions] (
		[GeneralModDefID]
	) ON DELETE CASCADE 
GO

ALTER TABLE [rb_ModuleSettings] ADD 
	CONSTRAINT [FK_rb_ModuleSettings_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [rb_Modules] ADD 
	CONSTRAINT [FK_rb_Modules_rb_ModuleDefinitions1] FOREIGN KEY 
	(
		[ModuleDefID]
	) REFERENCES [rb_ModuleDefinitions] (
		[ModuleDefID]
	),
	CONSTRAINT [FK_rb_Modules_rb_Tabs1] FOREIGN KEY 
	(
		[TabID]
	) REFERENCES [rb_Tabs] (
		[TabID]
	)
GO

ALTER TABLE [rb_Pictures] ADD 
	CONSTRAINT [FK_rb_Pictures_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_PortalSettings] ADD 
	CONSTRAINT [FK_rb_PortalSettings_rb_Portals] FOREIGN KEY 
	(
		[PortalID]
	) REFERENCES [rb_Portals] (
		[PortalID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [rb_Roles] ADD 
	CONSTRAINT [FK_rb_Roles_rb_Portals] FOREIGN KEY 
	(
		[PortalID]
	) REFERENCES [rb_Portals] (
		[PortalID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_SolutionModuleDefinitions] ADD 
	CONSTRAINT [FK_rb_SolutionModuleDefinitions_rb_GeneralModuleDefinitions] FOREIGN KEY 
	(
		[GeneralModDefID]
	) REFERENCES [rb_GeneralModuleDefinitions] (
		[GeneralModDefID]
	),
	CONSTRAINT [FK_rb_SolutionModuleDefintions_rb_Solutions] FOREIGN KEY 
	(
		[SolutionsID]
	) REFERENCES [rb_Solutions] (
		[SolutionsID]
	)
GO

ALTER TABLE [rb_States] ADD 
	CONSTRAINT [FK_rb_States_rb_Countries] FOREIGN KEY 
	(
		[CountryID]
	) REFERENCES [rb_Countries] (
		[CountryID]
	)
GO

ALTER TABLE [rb_TabSettings] ADD 
	CONSTRAINT [FK_rb_TabSettings_rb_Tabs] FOREIGN KEY 
	(
		[TabID]
	) REFERENCES [rb_Tabs] (
		[TabID]
	) ON DELETE CASCADE 
GO

ALTER TABLE [rb_Tabs] ADD 
	CONSTRAINT [FK_rb_Tabs_rb_Tabs] FOREIGN KEY 
	(
		[ParentTabID]
	) REFERENCES [rb_Tabs] (
		[TabID]
	)
GO

ALTER TABLE [rb_Tabs] WITH NOCHECK ADD CONSTRAINT
	[FK_rb_Tabs_rb_Portals] FOREIGN KEY
	(
	[PortalID]
	) REFERENCES [rb_Portals]
	(
	[PortalID]
	)
GO

ALTER TABLE [rb_UserRoles] ADD 
	CONSTRAINT [FK_rb_UserRoles_rb_Roles] FOREIGN KEY 
	(
		[RoleID]
	) REFERENCES [rb_Roles] (
		[RoleID]
	),
	CONSTRAINT [FK_rb_UserRoles_rb_Users] FOREIGN KEY 
	(
		[UserID]
	) REFERENCES [rb_Users] (
		[UserID]
	) ON DELETE CASCADE 
GO

ALTER TABLE [rb_Users] ADD 
	CONSTRAINT [FK_rb_Users_rb_Countries] FOREIGN KEY 
	(
		[CountryID]
	) REFERENCES [rb_Countries] (
		[CountryID]
	) ON UPDATE CASCADE ,
	CONSTRAINT [FK_rb_Users_rb_Portals] FOREIGN KEY 
	(
		[PortalID]
	) REFERENCES [rb_Portals] (
		[PortalID]
	) ON DELETE CASCADE ,
	CONSTRAINT [FK_rb_Users_rb_States] FOREIGN KEY 
	(
		[StateID]
	) REFERENCES [rb_States] (
		[StateID]
	) ON UPDATE CASCADE 
GO

ALTER TABLE [rb_st_Announcements] ADD 
	CONSTRAINT [FK_rb_st_Announcements_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_st_Contacts] ADD 
	CONSTRAINT [FK_rb_st_Contacts_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_st_Documents] ADD 
	CONSTRAINT [FK_rb_st_Documents_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_st_Events] ADD 
	CONSTRAINT [FK_rb_st_Events_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_st_HtmlText] ADD 
	CONSTRAINT [FK_rb_st_HtmlText_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_st_Links] ADD 
	CONSTRAINT [FK_rb_st_Links_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  TRIGGER [rb_st_AnnouncementsModified]
ON [rb_st_Announcements]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  TRIGGER [rb_st_ContactsModified]
ON [rb_st_Contacts]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  TRIGGER [rb_st_DocumentsModified]
ON [rb_st_Documents]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  TRIGGER [rb_st_EventsModified]
ON [rb_st_Events]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE   TRIGGER [rb_st_HtmlTextModified]
ON [rb_st_HtmlText]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE  TRIGGER [rb_st_LinksModified]
ON [rb_st_Links]
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

