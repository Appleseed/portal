---------------------
--1.1.7.1111Z.sql
---------------------
-- Updated by Eric Ramseur@anant.us 2/20/2016
-- by Manu and Uwe Lesta and Charles on 27 - feb - 2005
-- CLEAN INSTALL of system tables AND procedures
-- Modules structures will be created by module itself

SET NOCOUNT ON

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Users_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Users_IU]
GO
CREATE PROC rb_Users_IU 	@UserID	int,  	@PortalID	int,  	@Name	nvarchar(50),  	@Company	nvarchar(50),  	@Address	nvarchar(50),  	@City	nvarchar(50),  	@Zip	nvarchar(6),  	@CountryID	nchar(2),  	@StateID	int,  	@PIva	nvarchar(11),  	@CFiscale	nvarchar(16),  	@Phone	nvarchar(50),  	@Fax	nvarchar(50),  	@Password	nvarchar(20),  	@Email	nvarchar(100),  	@SendNewsletter	bit,  	@MailChecked	tinyint,  	@LastSend	smalldatetime  AS  SET NOCOUNT ON  	UPDATE [rb_Users] 	SET PortalID = @PortalID,  		Name = @Name,  		Company = @Company,  		Address = @Address,  		City = @City,  		Zip = @Zip,  		CountryID = @CountryID,  		StateID = @StateID,  		PIva = @PIva,  		CFiscale = @CFiscale,  		Phone = @Phone,  		Fax = @Fax,  		Password = @Password,  		Email = @Email,  		SendNewsletter = @SendNewsletter,  		MailChecked = @MailChecked,  		LastSend = @LastSend 	WHERE UserID	=	@UserID  	IF @@ROWCOUNT = 0 	BEGIN 	SET IDENTITY_INSERT [rb_Users] ON 	INSERT [rb_Users] (UserID,  		PortalID,  		Name,  		Company,  		Address,  		City,  		Zip,  		CountryID,  		StateID,  		PIva,  		CFiscale,  		Phone,  		Fax,  		Password,  		Email,  		SendNewsletter,  		MailChecked,  		LastSend) 	VALUES (@UserID,  		@PortalID,  		@Name,  		@Company,  		@Address,  		@City,  		@Zip,  		@CountryID,  		@StateID,  		@PIva,  		@CFiscale,  		@Phone,  		@Fax,  		@Password,  		@Email,  		@SendNewsletter,  		@MailChecked,  		@LastSend) 	SET IDENTITY_INSERT [rb_Users] OFF 	END  Return 
GO
--	@UserID	,	@PortalID	,	@Name	,	@Company	,	@Address	,	@City	,	@Zip	,	@CountryID	,	@StateID	,	@PIva	,	@CFiscale	,	@Phone	,	@Fax	,	@Password	,	@Email	,	@SendNewsletter	,	@MailChecked	,	@LastSend
EXEC rb_Users_IU 	1	,	0	,	N'Administrator'	,	NULL	,	NULL	,	NULL	,	NULL	,	N'IT'	,	NULL	,	NULL	,	NULL	,	N''	,	NULL	,	N'admin'	,	N'admin@Appleseedportal.net'	,	0	,	NULL	,	''
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Users_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Users_IU]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Roles_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Roles_IU]
GO
CREATE PROC rb_Roles_IU 	@RoleID	int,  	@PortalID	int,  	@RoleName	nvarchar(50),  	@Permission	tinyint  AS  SET NOCOUNT ON  	UPDATE [rb_Roles] 	SET PortalID = @PortalID,  		RoleName = @RoleName,  		Permission = @Permission 	WHERE RoleID	=	@RoleID  	IF @@ROWCOUNT = 0 	BEGIN 	SET IDENTITY_INSERT [rb_Roles] ON 	INSERT [rb_Roles] (RoleID,  		PortalID,  		RoleName,  		Permission) 	VALUES (@RoleID,  		@PortalID,  		@RoleName,  		@Permission) 	SET IDENTITY_INSERT [rb_Roles] OFF 	END  Return 
GO
--	@RoleID	,	@PortalID	,	@RoleName	,	@Permission
EXEC rb_Roles_IU 	0	,	0	,	N'Admins'	,	1
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Roles_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Roles_IU]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserRoles_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserRoles_IU]
GO
CREATE PROC rb_UserRoles_IU 	@UserID	int,  	@RoleID	int  AS  SET NOCOUNT ON  /***** WARNING CANNOT IMPLEMENT UPDATE BECAUSE TABLE HAS NO PRIMARY KEY  ***/  	INSERT [rb_UserRoles] (UserID,  		RoleID) 	VALUES (@UserID,  		@RoleID)  Return 
GO
--	@UserID	,	@RoleID
GO
EXEC rb_UserRoles_IU 	1	,	0
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserRoles_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UserRoles_IU]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_PortalSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_PortalSettings_IU]
GO
CREATE PROC rb_PortalSettings_IU 	@PortalID	int,  	@SettingName	nvarchar(50),  	@SettingValue	nvarchar(256)  AS  SET NOCOUNT ON  	UPDATE [rb_PortalSettings] 	SET SettingValue = @SettingValue 	WHERE PortalID	=	@PortalID AND SettingName	=	@SettingName  	IF @@ROWCOUNT = 0 	BEGIN 	INSERT [rb_PortalSettings] (PortalID,  		SettingName,  		SettingValue) 	VALUES (@PortalID,  		@SettingName,  		@SettingValue) 	END  Return 
GO
--	@PortalID	,	@SettingName	,	@SettingValue
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_PAGE_LAYOUT'	, 	N'Flatly.Bootstrap' -- Bootstrap Layout
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_REGISTER_TYPE'	,	N'~/desktopmodules/coremodules/register/registerfull.ascx'
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_THEME'	,	N'Flatly.Bootstrap' --Bootstrap Theme
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_ALT_THEME'	, N'Flatly.Bootstrap' 
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_LANGLIST'	, N'en;es;ar;bg;ca;cs;da;de;es-MX;fr;hr;is;it;ko;nl;nl-BE;no;pl;pt;ru;sl;sv;tr;uk;zh-CN;zh-TW;'
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_LOGO'	, N'appleseed-logo-white-160x33.png' 
GO
EXEC rb_PortalSettings_IU 	0	,	N'SITESETTINGS_DEFAULT_EDITOR'	, N'Syrinx CkEditor' 
GO
--Turn on Friendly URLS by default
EXEC rb_PortalSettings_IU 	0	,	N'ENABLE_PAGE_FRIENDLY_URL'	, N'True' 
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_PortalSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_PortalSettings_IU]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Tabs_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Tabs_IU]
GO
CREATE PROC rb_Tabs_IU 	@TabID	int,  	@ParentTabID	int,  	@TabOrder	int,  	@PortalID	int,  	@TabName	nvarchar(50),  	@MobileTabName	nvarchar(50),  	@AuthorizedRoles	nvarchar(256),  	@ShowMobile	bit,  	@TabLayout	int  AS  SET NOCOUNT ON  	UPDATE [rb_Tabs] 	SET ParentTabID = @ParentTabID,  		TabOrder = @TabOrder,  		PortalID = @PortalID,  		TabName = @TabName,  		MobileTabName = @MobileTabName,  		AuthorizedRoles = @AuthorizedRoles,  		ShowMobile = @ShowMobile,  		TabLayout = @TabLayout 	WHERE TabID	=	@TabID  	IF @@ROWCOUNT = 0 	BEGIN 	SET IDENTITY_INSERT [rb_Tabs] ON 	INSERT [rb_Tabs] (TabID,  		ParentTabID,  		TabOrder,  		PortalID,  		TabName,  		MobileTabName,  		AuthorizedRoles,  		ShowMobile,  		TabLayout) 	VALUES (@TabID,  		@ParentTabID,  		@TabOrder,  		@PortalID,  		@TabName,  		@MobileTabName,  		@AuthorizedRoles,  		@ShowMobile,  		@TabLayout) 	SET IDENTITY_INSERT [rb_Tabs] OFF 	END  Return 
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Modules_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Modules_IU]
GO
CREATE PROC rb_Modules_IU 	@ModuleID	int,
    @TabID	int,
    @GeneralModDefID	uniqueidentifier,
    @ModuleOrder	int,
    @PaneName	nvarchar(50),
    @ModuleTitle	nvarchar(256),
    @AuthorizedEditRoles	nvarchar(256),
    @AuthorizedViewRoles	nvarchar(256),
    @AuthorizedAddRoles	nvarchar(256),
    @AuthorizedDeleteRoles	nvarchar(256),
    @AuthorizedPropertiesRoles	nvarchar(256),
    @CacheTime	int,
    @ShowMobile	bit,
    @AuthorizedPublishingRoles	nvarchar(256),
    @NewVersion	bit,
    @SupportWorkflow	bit,
    @AuthorizedApproveRoles	nvarchar(256),
    @WorkflowState	tinyint,
    @LastModified	datetime,
    @LastEditor	nvarchar(256),
    @StagingLastModified	datetime,
    @StagingLastEditor	nvarchar(256),
    @SupportCollapsable	bit,
    @ShowEveryWhere		bit ,
    @AuthorizedMoveModuleRoles	nvarchar(256),
    @AuthorizedDeleteModuleRoles	nvarchar(256)

AS  
IF NOT exists (SELECT ModuleDefID FROM  rb_ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID)
BEGIN
    RAISERROR('Something gone wrong. Cannot find the module.', 16, 1)
    RETURN
END

Declare @ModuleDefID int
SET @ModuleDefID = 
    (SELECT ModuleDefID FROM  rb_ModuleDefinitions WHERE GeneralModDefID = @GeneralModDefID)   

--RAISERROR('Something gone wrong. Cannot find module: ' + CONVERT(varchar(50), @GeneralModDefID) + '''', 16, 1)
    
SET NOCOUNT ON
    UPDATE [rb_Modules] 	

SET TabID = @TabID,
        ModuleDefID = @ModuleDefID,
        ModuleOrder = @ModuleOrder,
        PaneName = @PaneName,
        ModuleTitle = @ModuleTitle,
        AuthorizedEditRoles = @AuthorizedEditRoles,
        AuthorizedViewRoles = @AuthorizedViewRoles,
        AuthorizedAddRoles = @AuthorizedAddRoles,
        AuthorizedDeleteRoles = @AuthorizedDeleteRoles,
        AuthorizedPropertiesRoles = @AuthorizedPropertiesRoles,
        CacheTime = @CacheTime,
        ShowMobile = @ShowMobile,
        AuthorizedPublishingRoles = @AuthorizedPublishingRoles,
        NewVersion = @NewVersion,
        SupportWorkflow = @SupportWorkflow,
        AuthorizedApproveRoles = @AuthorizedApproveRoles,
        WorkflowState = @WorkflowState,
        LastModified = @LastModified,
        LastEditor = @LastEditor,
        StagingLastModified = @StagingLastModified,
        StagingLastEditor = @StagingLastEditor,
        SupportCollapsable = @SupportCollapsable,
        ShowEveryWhere = @ShowEveryWhere,
        AuthorizedMoveModuleRoles = @AuthorizedMoveModuleRoles,
        AuthorizedDeleteModuleRoles = @AuthorizedDeleteModuleRoles

WHERE ModuleID	=	@ModuleID

IF @@ROWCOUNT = 0 
BEGIN 	
SET IDENTITY_INSERT [rb_Modules] ON 	

    INSERT [rb_Modules] 
        (ModuleID,
        TabID,
        ModuleDefID,
        ModuleOrder,
        PaneName,
        ModuleTitle,
        AuthorizedEditRoles,
        AuthorizedViewRoles,
        AuthorizedAddRoles,
        AuthorizedDeleteRoles,
        AuthorizedPropertiesRoles,
        CacheTime,
        ShowMobile,
        AuthorizedPublishingRoles,
        NewVersion,
        SupportWorkflow,
        AuthorizedApproveRoles,
        WorkflowState,
        LastModified,
        LastEditor,
        StagingLastModified,
        StagingLastEditor,
        SupportCollapsable,
        ShowEveryWhere,
        AuthorizedMoveModuleRoles,
        AuthorizedDeleteModuleRoles
) 	

VALUES (@ModuleID,
        @TabID,
        @ModuleDefID,
        @ModuleOrder,
        @PaneName,
        @ModuleTitle,
        @AuthorizedEditRoles,
        @AuthorizedViewRoles,
        @AuthorizedAddRoles,
        @AuthorizedDeleteRoles,
        @AuthorizedPropertiesRoles,
        @CacheTime,
        @ShowMobile,
        @AuthorizedPublishingRoles,
        @NewVersion,
        @SupportWorkflow,
        @AuthorizedApproveRoles,
        @WorkflowState,
        @LastModified,
        @LastEditor,
        @StagingLastModified,
        @StagingLastEditor,
        @SupportCollapsable,
        @ShowEveryWhere,
        @AuthorizedMoveModuleRoles,
        @AuthorizedDeleteModuleRoles
) 	SET IDENTITY_INSERT [rb_Modules] OFF 	END  Return 
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModuleSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModuleSettings_IU]
GO
CREATE PROC rb_ModuleSettings_IU 	@ModuleID	int,  	@SettingName	nvarchar(50),  	@SettingValue	nvarchar(256)  AS  SET NOCOUNT ON  	UPDATE [rb_ModuleSettings] 	SET SettingValue = @SettingValue 	WHERE ModuleID	=	@ModuleID AND SettingName	=	@SettingName  	IF @@ROWCOUNT = 0 	BEGIN 	INSERT [rb_ModuleSettings] (ModuleID,  		SettingName,  		SettingValue) 	VALUES (@ModuleID,  		@SettingName,  		@SettingValue) 	END  Return 
GO

-- @TabID	,	@ParentTabID	,	@TabOrder	,	@PortalID	,	@TabName	,	@MobileTabName	,	@AuthorizedRoles	,	@ShowMobile	,	@TabLayout
EXEC rb_Tabs_IU 	0	,	NULL	,	0	,	-1	,	N'Unused Tab'	,	N'Unused Tab'	,	N'All Users;'	,	0	,	NULL
GO
EXEC rb_Tabs_IU 	1	,	NULL	,	1	,	0	,	N'Home'	,	N'Home'	,	N'All Users;'	,	1	,	NULL
GO

--	@ModuleID, @TabID, @GeneralModDefID, @ModuleOrder, @PaneName, @ModuleTitle, @AuthorizedEditRoles, @AuthorizedViewRoles, @AuthorizedAddRoles, @AuthorizedDeleteRoles, @AuthorizedPropertiesRoles, @CacheTime, @ShowMobile, @AuthorizedPublishingRoles, @NewVersion, @SupportWorkflow, @AuthorizedApproveRoles, @WorkflowState, @LastModified, @LastEditor, @StagingLastModified, @StagingLastEditor, @SupportCollapsable, @ShowEveryWhere
EXEC rb_Modules_IU 	5,	1,	'{0B113F51-FEA3-499A-98E7-7B83C192FDBB}', 1, N'TopPane', N'Banner', N'Admins', N'All Users', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	1, 1, '{A0F1F62B-FDC7-4DE5-BBAD-A5DAF31D960A}', -1, N'LeftPane', N'Login', N'Admins;', N'Unauthenticated Users;Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 1, 0, NULL, 0, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	2,	1,	'{0B113F51-FEA3-499A-98E7-7B83C192FDBB}', 1, N'ContentPane', N'Welcome to Appleseed Portal', N'Admins', N'All Users', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	7,	1,	'{2502DB18-B580-4F90-8CB4-C15E6E531020}', 2, N'ContentPane', N'Appleseed Portal Blog Feed', N'Admins', N'All Users', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	4,	1,	'{350CED6F-6739-43f3-8BF1-1D95187CA0BF}', 3, N'ContentPane', N'Start adding modules now!', N'Admins', N'Admins', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	6,	1,	'{0B113F51-FEA3-499A-98E7-7B83C192FDBB}', 1, N'BottomPane', N'Footer', N'Admins', N'All Users', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 1, N'Admins;', N'Admins;'
GO
-- admins tab
EXEC rb_Tabs_IU 	100	,	NULL	,	1000	,	0	,	N'Administration'	,	N'Admin'	,	N'Admins;'	,	0	,	NULL
GO
-- replace #++# by
-- N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
EXEC rb_Modules_IU 	100, 100, '{A1E37A0F-4EE9-4B83-9482-43466FC21E08}', 1, N'ContentPane', N'Add New Page', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	1001, 100, '{72C6F60A-50C4-4F20-8F89-3E8A27820557}', 1, N'ContentPane', N'Appleseed Version', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	1002, 100, '{52AD3A51-121D-48BC-9782-02076E0D6A69}', 1, N'ContentPane', N'Who''s Logged On?', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	110	,	100	,	1010	,	0	,	N'Page Manager'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	110, 110, '{1C575D94-70FC-4A83-80C3-2087F726CBB3}', 1, N'ContentPane', N'Manage All Pages', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

--- Separator, intentionally left blank
EXEC rb_Tabs_IU 	120	,	100	,	1020	,	0	,	N' '	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	130	,	100	,	1030	,	0	,	N'Cache Viewer'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	130, 130, '{33F254F8-2537-4486-A91D-E8544D407200}', 1, N'ContentPane', N'Cache Viewer', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	140	,	100	,	1040	,	0	,	N'Content Manager'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	140, 140, '{EDDD32E0-2135-4276-9157-3478995CCCD2}', 1, N'ContentPane', N'Content manager', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	150	,	100	,	1050	,	0	,	N'Database'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	151	,	150	,	1051	,	0	,	N'Database Editor'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	151, 151, '{AB02A3F4-A0A4-45E0-96ED-8450C19166C5}', 1, N'ContentPane', N'Database Table Edit', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	152	,	150	,	1052	,	0	,	N'Database Tool'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	152, 152, '{2502DB18-B580-4F90-8CB4-C15E6E531032}', 1, N'ContentPane', N'Database Tool', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	155	,	100	,	1055	,	0	,	N'File Manager'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	155, 155, '{DE97F04D-FB0A-445d-829A-61E4FA69ADB2}', 1, N'ContentPane', N'File Manager', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	160	,	100	,	1060	,	0	,	N'Language Switcher'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	160, 160, '{25E3290E-3B9A-4302-9384-9CA01243C00F}', 1, N'ContentPane', N'Language Switcher', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	170	,	100	,	1070	,	0	,	N'Logs'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	171	,	170	,	1071	,	0	,	N'Monitoring / Logs'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	171, 171, '{52AD3A51-121D-48BC-9782-02076E0D6A69}', 1, N'ContentPane', N'Who''s Logged On?', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	1711,171, '{3B8E3585-58B7-4F56-8AB6-C04A2BFA6589}', 2, N'ContentPane', N'Monitoring', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	172	,	170	,	1072	,	0	,	N'Error Logs'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	172, 172, '{2502DB18-B580-4F90-8CB4-C15E6E53100B}', 1, N'ContentPane', N'Error Logs', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_ModuleSettings_IU 	172	,	N'Directory'	,	N'~/rb_logs' --TODO
GO

EXEC rb_Tabs_IU 	173	,	170	,	1073	,	0	,	N'EventLogs'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	173, 173, '{2502DB18-B580-4F90-8CB4-C15E6E531051}', 1, N'ContentPane', N'EventLogs', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	180	,	100	,	1080	,	0	,	N'Modules'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	181	,	180	,	1081	,	0	,	N'Add Module To Page'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	181, 181, '{350CED6F-6739-43F3-8BF1-1D95187CA0BF}', 1, N'ContentPane', N'Add Module To Page', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	182	,	180	,	1082	,	0	,	N'Modules'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	182, 182, '{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}', 1, N'ContentPane', N'Modules', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	183	,	180	,	1083	,	0	,	N'Modules Definitions'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	183, 183, '{D04BB5EA-A792-4E87-BFC7-7D0ED3ADD582}', 1, N'ContentPane', N'Add/Remove Modules', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	190	,	100	,	1090	,	0	,	N'Newsletter'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	190, 190, '{B484D450-5D30-4C4B-817C-14A25D06577E}', 1, N'ContentPane', N'Newsletter', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	200	,	100	,	1200	,	0	,	N'Pages'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	201	,	200	,	1201	,	0	,	N'Add New Page'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	201, 201, '{A1E37A0F-4EE9-4B83-9482-43466FC21E08}', 1, N'ContentPane', N'Add New Page', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	202	,	200	,	1202	,	0	,	N'Add Module To Page'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	202,	202,	'{350CED6F-6739-43f3-8BF1-1D95187CA0BF}', 1, N'ContentPane', N'Add Module To Page', N'Admins', N'Admins', N'Admins', N'Admins', N'Admins', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	203	,	200	,	1203	,	0	,	N'Page Manager'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	203, 203, '{1C575D94-70FC-4A83-80C3-2087F726CBB3}', 1, N'ContentPane', N'Page Manager', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	210	,	100	,	1210	,	0	,	N'Portal Management'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	210, 210, '{366C247D-4CFB-451D-A7AE-649C83B05841}', 1, N'ContentPane', N'Portal Management', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	215	,	100	,	1215	,	0	,	N'Recycler'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	215, 215, '{E928F47B-A131-4a33-88D5-D5D6E7A94B36}', 1, N'ContentPane', N'Recycler', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

--EXEC rb_Tabs_IU 	220	,	100	,	1220	,	0	,	N'Search Portal'	,	N''	,	N'Admins;'	,	0	,	NULL
--GO
--EXEC rb_Modules_IU 	220, 220, '{2502DB18-B580-4F90-8CB4-C15E6E531030}', 1, N'ContentPane', N'Search Portal', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
--GO

--EXEC rb_Tabs_IU 	230	,	100	,	1230	,	0	,	N'ServiceItemList'	,	N''	,	N'Admins;'	,	0	,	NULL
--GO
--EXEC rb_Modules_IU 	230, 230, '{2502DB18-B580-4F90-8CB4-C15E6E531052}', 1, N'ContentPane', N'ServiceItemList', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
--GO

EXEC rb_Tabs_IU 	240	,	100	,	1240	,	0	,	N'Site Settings'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	240, 240, '{EBBB01B1-FBB5-4E79-8FC4-59BCA1D0554E}', 1, N'ContentPane', N'Site Settings', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

--EXEC rb_Tabs_IU 	250	,	100	,	1250	,	0	,	N'SiteMap'	,	N''	,	N'Admins;'	,	0	,	NULL
--GO
--EXEC rb_Modules_IU 	250, 250, '{429A98E3-7A07-4D9A-A578-3ED8DD158306}', 1, N'ContentPane', N'SiteMap', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
--GO

EXEC rb_Tabs_IU 	260	,	100	,	1260	,	0	,	N'Tasks'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

--EXEC rb_Tabs_IU 	261	,	260	,	1261	,	0	,	N'Milestones'	,	N''	,	N'Admins;'	,	0	,	NULL
--GO
--EXEC rb_Modules_IU 	261, 261, '{B8784E32-688A-4B8A-87C4-DF108BF12DBE}', 1, N'ContentPane', N'Milestones', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
--GO

--EXEC rb_Tabs_IU 	262	,	260	,	1262	,	0	,	N'Tasks Add/Edit/Delete'	,	N''	,	N'Admins;'	,	0	,	NULL
--GO
--EXEC rb_Modules_IU 	262, 262, '{2502DB18-B580-4F90-8CB4-C15E6E531012}', 1, N'ContentPane', N'Task', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
--GO

EXEC rb_Tabs_IU 	280	,	100	,	1280	,	0	,	N'Users'	,	N''	,	N'Admins;'	,	0	,	NULL
GO

EXEC rb_Tabs_IU 	281	,	280	,	1281	,	0	,	N'User Add/Delete/Edit'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	281, 281, '{B6A48596-9047-4564-8555-61E3B31D7272}', 1, N'ContentPane', N'Manage Users', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO
EXEC rb_Modules_IU 	2811,281, '{A406A674-76EB-4BC1-BB35-50CD2C251F9C}', 2, N'ContentPane', N'Manage Roles', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	282	,	280	,	1282	,	0	,	N'User BlackList'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	282, 282, '{2502DB18-B580-4F90-8CB4-C15E6E531017}', 1, N'ContentPane', N'BlackList', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	283	,	280	,	1283	,	0	,	N'User Roles'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	283, 283, '{A406A674-76EB-4BC1-BB35-50CD2C251F9C}', 1, N'ContentPane', N'User Roles', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	284	,	280	,	1284	,	0	,	N'Who''s Logged On?'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	284, 284, '{52AD3A51-121D-48BC-9782-02076E0D6A69}', 1, N'ContentPane', N'Who''s Logged On?', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

EXEC rb_Tabs_IU 	290	,	100	,	1290	,	0	,	N'Version'	,	N''	,	N'Admins;'	,	0	,	NULL
GO
EXEC rb_Modules_IU 	290, 290, '{72C6F60A-50C4-4F20-8F89-3E8A27820557}', 1, N'ContentPane', N'Appleseed Version', N'Admins', N'Admins;', N'Admins;', N'Admins;', N'Admins;', 0, 0, NULL, 0, 0, NULL, NULL, '', NULL, '', NULL, 0, 0, N'Admins;', N'Admins;'
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Modules_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Modules_IU]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModuleSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModuleSettings_IU]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Tabs_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_Tabs_IU]
GO

DELETE FROM rb_HtmlText_st WHERE ModuleID = 2
GO

INSERT rb_HtmlText_st
(ModuleID, DesktopHtml, MobileSummary, MobileDetails)
VALUES (2, '<p>Appleseed Portal is your solution to help you run your company on the Internet. Use it as a platform to first manage the contents of your site. Soon we will release modules to help you connect your existing online software services that run different parts of your company.</p><p>If this is your first time using Appleseed Portal, you may want to take some steps to manage your content.</p><h2><strong>1. Login </strong></h2><p>Use the login form on the left side. You can add users to let others manage your site once you&#39;re in.</p><h2><strong>2. Play with Pages </strong></h2><p>We have a few pages setup for you to play around with. Take a look. There are three &quot;Content&quot; pages setup for &quot;Grow&quot;, &quot;Connect&quot;, and &quot;Trade&quot; which some suggestions on how to run your company.</p><h2><strong>3. Play with Modules</strong></h2><p>Add some basic modules, or edit the ones that are there to learn what they do. Most of the content is in &quot;HTML Module&quot;, but there are many more that are more specific such as the &quot;Books&quot; or the &quot;RSS Feed&quot; Module.</p>', '', '')
GO

DELETE FROM rb_HtmlText_st WHERE ModuleID = 5
GO
-- default home slider 
INSERT rb_HtmlText_st
(ModuleID, DesktopHtml, MobileSummary, MobileDetails)
VALUES (5, N'&lt;style type=&quot;text/css&quot;&gt;
        /****************/
        /****SLIDER*****/
        /***************/
/*container */  
.slideshow{
    margin-top:10px;
    overflow:hidden;
    position:relative;
    list-style-type: none;}

li.content{
    list-style-type: none;}
    
/* JPG */  
.slider_image{
    width:100%;
    height:auto;
    display:inline-block;}  
    
/* TEXT */
.wrap_text{

}  

/* LINKS */
.continue{
    float:right;
    color:#fff!important;
    font-family:Helvetica, Arial;
    font-size:14px;
}
.continue:hover{
    float:right;
    color:#ccc!important;
    font-family:Helvetica, Arial;
    font-size:14px;
}
#fsn {
    display:block;
    height:25px;
    margin:10px auto 15px;
    position:relative;
    text-align:center;
    width:100%;
    z-index:0;
}
#fsn ul {
    display:block;
    height:20px;
    list-style:none outside none;
    margin:0 auto;
    overflow:hidden;
    padding:5px 0 0;
    position:relative;
    width:125px;
}
#fs_pagination a{
    display:block;
    float:left;
    height:17px;
    margin:0 10px 0 0;
    padding:0;
    width:17px;
    background-color:#ededed;
    color:#22C0FD;
    font-size:10px;
    text-indent:-9999px;
    font-weight:bold;
}
#fs_pagination a:hover,
#fs_pagination a:active,
 a.activeSlide{ 
    background-color:#333!important;
    text-indent:-9999px;
}

#Content_moduleType{
    color:#666;}
&lt;/style&gt;
&lt;!-- include Cycle plugin --&gt;
&lt;script type=&quot;text/javascript&quot; src=&quot;~/aspnet_client/js/jquery.cycle.min.js&quot;&gt;&lt;/script&gt;
&lt;!--  initialize the slideshow when the DOM is ready --&gt;
&lt;script type=&quot;text/javascript&quot;&gt;
$(document).ready(function () {
    $(&#39;.slideshow&#39;).after(&#39;&lt;div id=&quot;fsn&quot;&gt;&lt;ul id=&quot;fs_pagination&quot;&gt;&#39;).cycle({
        timeout: 5000, // milliseconds between slide transitions (0 to disable auto advance)
        fx: &#39;fade&#39;, // choose a transition type, ex: fade, scrollUp, shuffle, etc...            
        pager: &#39;#fs_pagination&#39;, // selector for element to use as pager container
        pause: true, // true to enable &quot;pause on hover&quot;
        pauseOnPagerHover: 0 // true to pause when hovering over pager link
   });
});
&lt;/script&gt;
    &lt;!-- Jquery Container--&gt;
    &lt;ul class=&quot;slideshow&quot;&gt;
        &lt;!-- IMAGE &amp; TEXT Number (1) --&gt;
        &lt;li class=&quot;content&quot;&gt;
            &lt;div class=&quot;col-sm-5&quot;&gt;
                &lt;img class=&quot;slider_image&quot; src=&quot;~/Portals/_Appleseed/images/default/slider/Appleseed.slider.business.jpg&quot; /&gt;
            &lt;/div&gt;
            &lt;div class=&quot;wrap_text col-sm-7&quot;&gt;
                &lt;h1&gt;Grow your business.&lt;/h1&gt;
                &lt;h3&gt;You downloaded Appleseed Portal to help you run your company online. As we work to make this easier for you in upcoming versions, you still have some basic things you can do. You should consider taking these steps as soon as you can if you haven&#39;t.&lt;/h3&gt;
                &lt;br/&gt;
                &lt;a class=&quot;continue&quot; href=&quot;&quot;&gt;Continue Reading &#187; &lt;/a&gt;      
            &lt;/div&gt;
        &lt;/li&gt;
        &lt;!-- IMAGE &amp; TEXT Number (2) --&gt;
        &lt;li class=&quot;content&quot;&gt;
            &lt;div class=&quot;col-sm-5&quot;&gt;
                &lt;img  class=&quot;slider_image&quot; src=&quot;~/Portals/_Appleseed/images/default/slider/Appleseed.slider.coffee.jpg&quot; /&gt;
            &lt;/div&gt;
            &lt;div class=&quot;wrap_text col-sm-7&quot;&gt;
                &lt;h1&gt;Connect with others.&lt;/h1&gt;
                &lt;h3&gt;There are several ways for you to connect to other entrepreneurs, prospective customers and business partners. Here are a few suggestions that can get you started in your quest to build a great company.&lt;/h3&gt;
                &lt;br/&gt;
            &lt;a class=&quot;continue&quot; href=&quot;&quot;&gt;Continue Reading &#187; &lt;/a&gt;
            &lt;/div&gt;    
        &lt;/li&gt;
        &lt;!-- IMAGE &amp; TEXT Number (3) --&gt;
        &lt;li class=&quot;content&quot;&gt;
            &lt;div class=&quot;col-sm-5&quot;&gt;
                &lt;img class=&quot;slider_image&quot; src=&quot;~/Portals/_Appleseed/images/default/slider/Appleseed.slider.trade.jpg&quot; /&gt;
            &lt;/div&gt;
            &lt;div class=&quot;wrap_text col-sm-7&quot;&gt;       
                &lt;h1&gt;Trade with the world.&lt;/h1&gt;
                &lt;h3&gt;Once you have your business setup, and have a network of businesses to help you deliver your products and services, you are ready to trade. Start small, and get bigger. The sky is the limit, until you surpass it.&lt;/h3&gt;
                &lt;br/&gt;
            &lt;a class=&quot;continue&quot; href=&quot;&quot;&gt;Continue Reading &#187; &lt;/a&gt;
            &lt;/div&gt;      
        &lt;/li&gt;
    &lt;/ul&gt;
    &lt;!-- End Jquery Container--&gt;', '', '')
DELETE FROM rb_HtmlText_st WHERE ModuleID = 6
GO
-- default footer module 
INSERT rb_HtmlText_st
(ModuleID, DesktopHtml, MobileSummary, MobileDetails)
VALUES (6, '<style type="text/css">
#sitemap a.no_deco:link,
#sitemap a.no_deco:visited,
#sitemap a.no_deco:hover{
    padding-right: 10px;
    width: 80px;
    display: inline-block;
}
.center_footer_content{
    width: 940px;
    margin: 0 auto;
    padding: 0px 10px;}
.footer_box{
    float: left;
    width: 280px;
    padding: 10px;
    display: inline-block;
    height: 160px;
}
.footer_box#left{
    padding-right: 30px;
}
.footer_box#last{
    padding-right: 0px;
    padding-left: 30px;
}
</style>
<div class="footer_box" id="left"> 
      <span>Our Services</span> 
      <hr>
        <ul> 
            <li><a href="#" class="no_deco">Lorem ipsum dolor</a></li> 
            <li><a href="#" class="no_deco">Lorem ipsum dolor sit amet</a></li> 
            <li><a href="#" class="no_deco">Praesent et eros</a></li> 
            <li><a href="#" class="no_deco">Lorem ipsum dolor</a></li> 
            <li><a href="#" class="no_deco">Suspendisse in neque</a></li> 
            <li><a href="#" class="no_deco">Phasellus tempor vestibulum</a></li> 
        </ul> 
    </div> 
    <div class="footer_box" id="sitemap"> 
        <span>Site Map</span>
        <hr> 
        <ul> 
            <li><a href="#" class="no_deco">HOME        </a>  <a href="#">Who we are</a> | <a href="#">Work Team</a>                   </li> 
            <li><a href="#" class="no_deco">SERVICES    </a>  <a href="#">Web Design</a> | <a href="#">CMS</a> | <a href="#">CRM</a>   </li> 
            <li><a href="#" class="no_deco">PROJECTS    </a>  <a href="#">Open Source</a> | <a href="#">Partners</a>                   </li> 
            <li><a href="#" class="no_deco">MEMBERS     </a>  <a href="#">Log in</a> | <a href="#">Register</a>                        </li> 
            <li><a href="#" class="no_deco">PORTFOLIO   </a>  <a href="#">Web Design</a> | <a href="#">App</a>                         </li> 
            <li><a href="#" class="no_deco">CONTACT US  </a>  <a href="#">Our Offices</a> | <a href="#">Request a quote</a>            </li> 
        </ul> 
    </div> 
    <div class="footer_box" id="last"> 
        <span>Testimonials</span>
        <hr> 
        <p>
            Elitpha sellus enim rutrum orna ac met quis risus sus sed metus. 
            Ipsumm aecenas sempor tincidunt feugiat tur aenec a integet rhoncus 
            eger et. Semnisse fauctor in ut convalli citudin vivamus curabitur tinci dunt nam vestique pretium.
        </p> 
    </div> 
    <br class="clear">', '', '')
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModuleSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModuleSettings_IU]
GO
CREATE PROC rb_ModuleSettings_IU 
(	
@ModuleID	int,  	
@SettingName	nvarchar(50),  	
@SettingValue	nvarchar(256)
)  
AS   
SET NOCOUNT ON  	 
UPDATE [rb_ModuleSettings] 	
SET SettingValue = @SettingValue 	
WHERE ModuleID = @ModuleID AND SettingName = @SettingName
IF @@ROWCOUNT = 0
BEGIN
    INSERT [rb_ModuleSettings] (ModuleID, SettingName, SettingValue)
    VALUES (@ModuleID, @SettingName, @SettingValue)
END
GO
--	@ModuleID	,	@SettingName	,	@SettingValue
EXEC rb_ModuleSettings_IU 	4	,	N'LANGUAGESWITCHER_TYPES'	,	1
GO
EXEC rb_ModuleSettings_IU 	5	,	N'Editor'	,	N'Plain Text'
GO
EXEC rb_ModuleSettings_IU 	5	,	N'MODULESETTINGS_MODULE_THEME'	,	N'Module-No-Style'
GO
EXEC rb_ModuleSettings_IU 	6	,	N'Editor'	,	N'Plain Text'
GO
EXEC rb_ModuleSettings_IU 	6	,	N'MODULESETTINGS_MODULE_THEME'	,	N'Module-No-Style'
GO
--EXEC rb_ModuleSettings_IU 	7	,	N'XML URL'	,	N'http://feeds.feedburner.com/appleseedapp'
--GO
EXEC rb_ModuleSettings_IU 	2	,	N'SHARE_MODULE'	,	N'False'
GO
EXEC rb_ModuleSettings_IU 	2	,	N'Editor'	,	N'Syrinx CkEditor'
GO
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ModuleSettings_IU]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ModuleSettings_IU]
GO

--FROM 1779
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_PortalSettings_rb_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_PortalSettings
    DROP CONSTRAINT FK_rb_PortalSettings_rb_Portals
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_PortalSettings_rb_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_PortalSettings WITH NOCHECK ADD CONSTRAINT
    FK_rb_PortalSettings_rb_Portals FOREIGN KEY
    (
    PortalID
    ) REFERENCES rb_Portals
    (
    PortalID
    ) ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_Tabs1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules
    DROP CONSTRAINT FK_rb_Modules_rb_Tabs1
GO

--Dropping 1 at the end
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_Tabs]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules
    DROP CONSTRAINT FK_rb_Modules_rb_Tabs
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_Tabs]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules WITH NOCHECK ADD CONSTRAINT
    FK_rb_Modules_rb_Tabs FOREIGN KEY
    (
    TabID
    ) REFERENCES rb_Tabs
    (
    TabID
    ) ON DELETE CASCADE
    
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Tabs_rb_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Tabs
    DROP CONSTRAINT FK_rb_Tabs_rb_Portals
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Tabs_rb_Portals]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Tabs WITH NOCHECK ADD CONSTRAINT
    FK_rb_Tabs_rb_Portals FOREIGN KEY
    (
    PortalID
    ) REFERENCES rb_Portals
    (
    PortalID
    ) ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_ModuleSettings_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_ModuleSettings
    DROP CONSTRAINT FK_rb_ModuleSettings_rb_Modules
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_ModuleSettings_rb_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_ModuleSettings WITH NOCHECK ADD CONSTRAINT
    FK_rb_ModuleSettings_rb_Modules FOREIGN KEY
    (
    ModuleID
    ) REFERENCES rb_Modules
    (
    ModuleID
    ) ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_ModuleDefinitions1]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules
    DROP CONSTRAINT FK_rb_Modules_rb_ModuleDefinitions1
GO

--Dropping 1
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules
    DROP CONSTRAINT FK_rb_Modules_rb_ModuleDefinitions
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_Modules_rb_ModuleDefinitions]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE rb_Modules WITH NOCHECK ADD CONSTRAINT
    FK_rb_Modules_rb_ModuleDefinitions FOREIGN KEY
    (
    ModuleDefID
    ) REFERENCES rb_ModuleDefinitions
    (
    ModuleDefID
    ) ON DELETE CASCADE
    
GO

-------------------
--1.4.0.1780.sql
-- Updates rb_users table to include the user_last_visit and user_current_visit fields for determining the user's last visit date
-- Recreates the rb_GetSingleUser procedure to return all fields from the rb_users table, so any additional fields can be returned when this procedure is called
-- Creates the rb_UpdateLastVisit to update the specified user's last visit date, only updates once during a day
-------------------
if exists (select * from sysobjects where id = object_id(N'[rb_GetSingleUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetSingleUser]
GO

if exists (select * from sysobjects where id = object_id(N'[rb_UpdateLastVisit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateLastVisit]
GO

-- Add these columns to the users table, if they do not exist 
if exists (select * from sysobjects where id = object_id(N'[rb_Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) begin
    if not exists (select * from syscolumns C inner join sysobjects O on O.id = C.id where O.id = object_id(N'[rb_Users]') and OBJECTPROPERTY(O.id, N'IsUserTable') = 1 and C.name = 'user_last_visit') 
        ALTER TABLE rb_Users ADD user_last_visit datetime NOT NULL DEFAULT(getdate())
    if not exists (select * from syscolumns C inner join sysobjects O on O.id = C.id where O.id = object_id(N'[rb_Users]') and OBJECTPROPERTY(O.id, N'IsUserTable') = 1 and C.name = 'user_current_visit') 
        ALTER TABLE rb_Users ADD user_current_visit datetime NOT NULL DEFAULT(getdate())
end
GO

CREATE   PROCEDURE rb_GetSingleUser
(
    @Email nvarchar(100),
    @PortalID int,
    @IDLang	nchar(2) = 'IT'
)
AS
SELECT
    rb_Users.*,
    
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

CREATE       procedure rb_UpdateLastVisit (@Email varchar(100), @PortalID int)


AS
BEGIN
   DECLARE
    @CurDate as datetime,
    @LastCurDate as datetime,
    @LastVisitDate as datetime

   SET NOCOUNT ON
   --Call this procedure BEFORE you call procedures to obtain data based on user_last_visit date field 
   --Get current dates stored in DB	
   SELECT @LastVisitDate = user_last_visit, @LastCurDate = user_current_visit
   FROM rb_users
   WHERE Email = @Email AND PortalID = @PortalID
   
   IF @@ROWCOUNT = 1
   BEGIN
     SET @CurDate = getdate()
     --Check to see if current visit is at least a day old
     IF DATEADD(hour, 12, @LastCurDate) < (@CurDate)
     BEGIN
       IF @LastCurDate <> @LastVisitDate
       BEGIN
         --if current visit date and last visit date are different 
         --then set current visit date and Last Visit date to current date
         --This allows for the user to view the pages based on last visit date,
         --for 1 day after they first visit the site before it sets a new last
         --visit date
            UPDATE rb_users 
            SET 
                user_current_visit = @CurDate,
                user_last_visit = @LastCurDate
            WHERE Email = @Email AND PortalID = @PortalID
       END
       ELSE
       BEGIN
         --else only set current visit date to current date
         UPDATE rb_users
         SET 
            user_current_visit = @CurDate    
         WHERE Email = @Email AND PortalID = @PortalID
       END
     END
   END
END
GO


--1785 rb_FindModulesByGuid
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModulesByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModulesByGuid]
GO

CREATE PROCEDURE rb_FindModulesByGuid
(
    @PortalID int,
    @Guid uniqueidentifier
)
AS

    SELECT     rb_Modules.ModuleID
    FROM         rb_Modules 
    INNER JOIN rb_ModuleDefinitions ON rb_Modules.ModuleDefID = rb_ModuleDefinitions.ModuleDefID
    WHERE     (rb_ModuleDefinitions.PortalID = @PortalID) AND (rb_ModuleDefinitions.GeneralModDefID = @Guid)
GO

--1787
-- for remove old rb_FindModuleByGuid procedure
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FindModuleByGuid]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_FindModuleByGuid]
GO
-- change DesktopSrc in RoleAssignment module
UPDATE    rb_GeneralModuleDefinitions
SET       DesktopSrc = 'DesktopModules/RoleAssignment/RoleAssignment.ascx',
          ClassName = 'Appleseed.Content.Web.ModulesRoleAssignment'
WHERE     (GeneralModDefID = '{5EEE69A2-35BA-4B54-8451-E13B0CD24E99}')
GO

--1788
if exists (select * from dbo.sysobjects where id = object_id(N'rb_AddTab') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure rb_AddTab
GO


CREATE PROCEDURE rb_AddTab
(
   @PortalID		int,		/* Required Field  */
   @ParentTabID		int,            /*   New Parm - NULL Allowed    */
   @TabName		nvarchar(50),   /* Required Field  */
   @TabOrder		int,            /* Required Field  */ 
   @AuthorizedRoles	nvarchar (256), /* NULL Allowed    */
   @ShowMobile		bit = 0,        /*   New Parm - false by default */
   @MobileTabName	nvarchar(50),   /* Required Field  */
   @TabID		int OUTPUT      /* Returned value */

)
 
AS

  IF (@ParentTabID = 0)
  BEGIN  
    set @ParentTabID = NULL
  END


INSERT INTO rb_Tabs
(
   ParentTabID,  /* New parm */
    TabOrder,
    PortalID,
    TabName,
    MobileTabName,
    AuthorizedRoles,
    ShowMobile
    
)


VALUES
(
   @ParentTabID, 
   @TabOrder,
   @PortalID,
   @TabName,
   @MobileTabName,
   @AuthorizedRoles,
   @ShowMobile
    
)

SELECT
    @TabID = @@IDENTITY
GO

--1789
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetTabsParent]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetTabsParent]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateTab]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateTab]
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
        [TabName] [nvarchar] (100),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO     #TabTree
SELECT  TabID,
        TabName,
        ParentTabID,
        TabOrder,
        0,
        cast(100000000 + TabOrder AS varchar)
FROM    rb_Tabs
WHERE   ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder AS varchar)
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
select IDENTITY(int,1,2) AS ord , cast(TabID AS varchar) AS TabID into #tabs
from #TabTree
order by NestLevel, TreeOrder
-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID AS int)=#TabTree.TabID) 
-- Return Temporary Table
SELECT TabID, TabName, TreeOrder
FROM #TabTree 
UNION
SELECT 0 TabID, ' ROOT_LEVEL' TabName, '-1' AS TreeOrder
order by TreeOrder
GO


--Update Stored PROCEDURE: rb_UpdateTab
--Prevents orphaning a tab or placing tabs in an infinte recursive loop
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
    [TabName] [nvarchar] (100),
    [ParentTabID] [int],
    [TabOrder] [int],
    [NestLevel] [int],
    [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON	-- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent Levels
INSERT INTO	#TabTree
SELECT 	TabID,
    TabName,
    ParentTabID,
    TabOrder,
    0,
    cast(100000000 + TabOrder AS varchar)
FROM	rb_Tabs
WHERE	ParentTabID IS NULL AND PortalID =@PortalID
ORDER BY TabOrder
-- Next, the children Levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT 	#TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
        SELECT 	rb_Tabs.TabID,
            Replicate('-', @LastLevel *2) + rb_Tabs.TabName,
            rb_Tabs.ParentTabID,
            rb_Tabs.TabOrder,
            @LastLevel,
            cast(#TabTree.TreeOrder AS varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder AS varchar)
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
GO

--UPDATERS!!!!
--This version MUST MATCH THE version created by this script
--BE AWARE THAT IF YOU CHANGE THIS YOU MUST ENSURE THAT THIS SCRIPT EFFECTIVELY CREATES
--THE VERSION YOU STATE OUT FROM A CLEAN DB. IF YOU ARE UNSURE PLEASE DO NOT CHANGE THIS.
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1789','1.5.0.1789', CONVERT(datetime, '12/2/2005', 101))
GO


