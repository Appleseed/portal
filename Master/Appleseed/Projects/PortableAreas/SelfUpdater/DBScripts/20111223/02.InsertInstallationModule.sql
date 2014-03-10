INSERT INTO [appleseed].[dbo].[rb_Modules]
           ([TabID]
           ,[ModuleDefID]
           ,[ModuleOrder]
           ,[PaneName]
           ,[ModuleTitle]
           ,[AuthorizedEditRoles]
           ,[AuthorizedViewRoles]
           ,[AuthorizedAddRoles]
           ,[AuthorizedDeleteRoles]
           ,[AuthorizedPropertiesRoles]
           ,[CacheTime]
           ,[ShowMobile]
           ,[AuthorizedPublishingRoles]
           ,[NewVersion]
           ,[SupportWorkflow]
           ,[AuthorizedApproveRoles]
           ,[WorkflowState]
           ,[LastModified]
           ,[LastEditor]
           ,[StagingLastModified]
           ,[StagingLastEditor]
           ,[SupportCollapsable]
           ,[ShowEveryWhere]
           ,[AuthorizedMoveModuleRoles]
           ,[AuthorizedDeleteModuleRoles])
     VALUES
           (5
           ,(SELECT [ModuleDefID]    
      
  FROM [appleseed].[dbo].[rb_ModuleDefinitions]
  where GeneralModDefID in (SELECT [GeneralModDefID]
      
  FROM [appleseed].[dbo].[rb_GeneralModuleDefinitions]
  where FriendlyName = 'SelfUpdater - Installation'))
           ,1
           ,'ContentPane'
           ,'Available Packages'
           ,'Admins'
           ,'All Users'
           ,'Admins'
           ,'Admins'
           ,'Admins'
           ,0
           ,0
           ,' '
           ,1
           ,1
           ,null
           ,0
           ,null
           ,null
           ,null
           ,null
           ,0
           ,0
           ,'Admins'
           ,'Admins')
GO