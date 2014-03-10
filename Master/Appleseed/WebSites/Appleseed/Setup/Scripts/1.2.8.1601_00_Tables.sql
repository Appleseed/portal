
/****** Object:  Table [Announcements]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Announcements', 'rb_Announcements'
GO

/****** Object:  Table [Articles]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Articles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Articles', 'rb_Articles'
GO

/****** Object:  Table [Blacklist]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Blacklist]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Blacklist', 'rb_Blacklist'
GO

/****** Object:  Table [Contacts]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Contacts', 'rb_Contacts'
GO

/****** Object:  Table [Countries]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Countries]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Countries', 'rb_Countries'
GO

/****** Object:  Table [Cultures]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Cultures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Cultures', 'rb_Cultures'
GO

/****** Object:  Table [Discussion]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Discussion]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Discussion', 'rb_Discussion'
GO

/****** Object:  Table [Documents]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Documents', 'rb_Documents'
GO

/****** Object:  Table [Events]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Events', 'rb_Events'
GO

/****** Object:  Table [GeneralModuleDefinitions]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[GeneralModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'GeneralModuleDefinitions', 'rb_GeneralModuleDefinitions'
GO

/****** Object:  Table [HtmlText]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'HtmlText', 'rb_HtmlText'
GO

/****** Object:  Table [Links]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Links', 'rb_Links'
GO

/****** Object:  Table [Localize]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Localize]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Localize', 'rb_Localize'
GO

/****** Object:  Table [Milestones]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Milestones]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Milestones', 'rb_Milestones'
GO

/****** Object:  Table [ModuleDefinitions]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'ModuleDefinitions', 'rb_ModuleDefinitions'
GO

/****** Object:  Table [ModuleSettings]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[ModuleSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'ModuleSettings', 'rb_ModuleSettings'
GO

/****** Object:  Table [Modules]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Modules]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Modules', 'rb_Modules'
GO

/****** Object:  Table [Pictures]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Pictures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Pictures', 'rb_Pictures'
GO

/****** Object:  Table [PortalSettings]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[PortalSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'PortalSettings', 'rb_PortalSettings'
GO

/****** Object:  Table [Portals]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Portals]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Portals', 'rb_Portals'
GO

/****** Object:  Table [Roles]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Roles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Roles', 'rb_Roles'
GO

/****** Object:  Table [Sections]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Sections]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Sections', 'rb_Sections'
GO

/****** Object:  Table [SolutionModuleDefinitions]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[SolutionModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'SolutionModuleDefinitions', 'rb_SolutionModuleDefinitions'
GO

/****** Object:  Table [Solutions]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Solutions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Solutions', 'rb_Solutions'
GO

/****** Object:  Table [States]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[States]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'States', 'rb_States'
GO

/****** Object:  Table [TabSettings]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[TabSettings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'TabSettings', 'rb_TabSettings'
GO

/****** Object:  Table [Tabs]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Tabs]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Tabs', 'rb_Tabs'
GO

/****** Object:  Table [UserRoles]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UserRoles]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'UserRoles', 'rb_UserRoles'
GO

/****** Object:  Table [Users]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Users]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'Users', 'rb_Users'
GO

/****** Object:  Table [st_Announcements]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_Announcements]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_Announcements', 'rb_st_Announcements'
GO

/****** Object:  Table [st_Contacts]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_Contacts]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_Contacts', 'rb_st_Contacts'
GO

/****** Object:  Table [st_Documents]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_Documents]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_Documents', 'rb_st_Documents'
GO

/****** Object:  Table [st_Events]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_Events]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_Events', 'rb_st_Events'
GO

/****** Object:  Table [st_HtmlText]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_HtmlText', 'rb_st_HtmlText'
GO

/****** Object:  Table [st_Links]    Script Date: 18/03/2003 14:18:51 ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[st_Links]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
EXEC sp_rename 'st_Links', 'rb_st_Links'
GO

