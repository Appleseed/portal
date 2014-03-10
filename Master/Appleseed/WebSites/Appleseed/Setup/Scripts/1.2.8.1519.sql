---------------------
--1.2.8.1519.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1519','1.2.8.1519', CONVERT(datetime, '03/19/2003', 101))
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[UpdateModule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [UpdateModule]
GO

CREATE PROCEDURE [UpdateModule]
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
    Modules
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

