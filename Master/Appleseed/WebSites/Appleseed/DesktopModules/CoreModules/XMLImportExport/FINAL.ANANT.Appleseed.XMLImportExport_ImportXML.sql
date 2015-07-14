ALTER PROCEDURE [rb_XMLImportData]
(
   -- @ParentPageID int,
	@XMLString XML
)
AS
BEGIN
	DECLARE @pageID INT
	DECLARE @TestPageID INT, @PPageID INT
	DECLARE @newPageID INT
	DECLARE @NewModuleID int
	DECLARE @SettingName nvarchar(50),
			@SettingValue nvarchar(1500),
			@getModuleID int

	DECLARE @ModuleID int ,
		@TabID int ,
		@ModuleDefID int ,
		@ModuleOrder int ,
		@PaneName nvarchar(50) ,
		@ModuleTitle nvarchar(256) ,
		@AuthorizedEditRoles nvarchar(256) ,
		@AuthorizedViewRoles nvarchar(256) ,
		@AuthorizedAddRoles nvarchar(256) ,
		@AuthorizedDeleteRoles nvarchar(256) ,
		@AuthorizedPropertiesRoles nvarchar(256) ,
		@CacheTime int ,
		@ShowMobile bit ,
		@AuthorizedPublishingRoles nvarchar(256) ,
		@NewVersion bit ,
		@SupportWorkflow bit ,
		@AuthorizedApproveRoles nvarchar(256) ,
		@WorkflowState tinyint ,
		@SupportCollapsable bit ,
		@ShowEveryWhere bit ,
		@AuthorizedMoveModuleRoles nvarchar(256) ,
		@AuthorizedDeleteModuleRoles nvarchar(256)

	/*FOR PAGES*/
	DECLARE @PageDetail Table
	(
		PageID int,
		ParentPageID int,
		PageOrder int,
		PortalID int,
		PageName NVARCHAR(100),
		MobilePageName NVARCHAR(100),
		ShowMobile bit,
		PageLayout int,
		PageDescription NVARCHAR(1024),
		AuthorizedRoles NVARCHAR(1024)
	)
	/*For PageSettings*/
	DECLARE @TabSettings Table
	(
		SettingName NVARCHAR(50),
		SettingValue NVARCHAR(1500)
	)
	/*For Modules*/
	DECLARE @ModuleDetail Table
	(
		ModuleID int ,
		ModuleDefID int ,
		ModuleOrder int ,
		PaneName nvarchar(50) ,
		ModuleTitle nvarchar(256) ,
		AuthorizedEditRoles nvarchar(256) ,
		AuthorizedViewRoles nvarchar(256) ,
		AuthorizedAddRoles nvarchar(256) ,
		AuthorizedDeleteRoles nvarchar(256) ,
		AuthorizedPropertiesRoles nvarchar(256) ,
		CacheTime int ,
		ShowMobile bit ,
		AuthorizedPublishingRoles nvarchar(256) ,
		NewVersion bit ,
		SupportWorkflow bit ,
		AuthorizedApproveRoles nvarchar(256) ,
		WorkflowState tinyint ,
		SupportCollapsable bit ,
		ShowEveryWhere bit ,
		AuthorizedMoveModuleRoles nvarchar(256) ,
		AuthorizedDeleteModuleRoles nvarchar(256)
	)
	/*For ModuleSettings*/
	DECLARE @ModuleSettings Table
	(
	SettingName varchar (50),
	SettingValue nvarchar(1500)
	)

	/*Create Log Table*/
	DECLARE  @LogTableXMLImportExport table
	(
		LogMessage nvarchar(2000)
	)

	/*Insert page detail by pageid*/
	INSERT INTO @PageDetail 
		SELECT 
			PageAttr.value('@PageID', 'int') AS PageID,
			PageAttr.value('@ParentPageID', 'int') as ParentPageID,
			PageAttr.value('@PageOrder', 'int') as PageOrder,
			PageAttr.value('@PortalID', 'int') as PortalID,
			PageAttr.value('@PageName', 'NVARCHAR(100)') as PageName,
			PageAttr.value('@MobilePageName', 'NVARCHAR(100)') as MobilePageName,
			PageAttr.value('@ShowMobile', 'bit') as ShowMobile,
			PageAttr.value('@PageLayout', 'int') as PageLayout,
			PageAttr.value('PageDescription[1]', 'NVARCHAR(1024)') as PageDescription,
			PageAttr.value('AuthorizedRoles[1]', 'NVARCHAR(1024)') as AuthorizedRoles
		FROM 
			@XMLString.nodes('Page') AS XD(PageAttr)
		
	/*Set pageID from Dumy table*/	
	SET @pageID = (SELECT pageid from @pagedetail)

	/*Set TestPageID from rb_Papges*/
	--SET @TestPageID = (SELECT PageID FROM rb_Pages WHERE PageID = @pageID)
	
	/*Set @PPageID from rb_Papges*/
	--SET @PPageID = (SELECT ParentPageID FROM rb_Pages WHERE PageID = @pageID)

	/*Check in Page is numeric or -1 or page existing into table */
	IF (ISNUMERIC(@pageID) = 1 OR @pageID=-1 OR exists(SELECT PageID FROM rb_Pages WHERE PageID = @pageID))
	BEGIN
		/*Check ParentPageID for further process*/
		--IF EXISTS(SELECT a.ParentPageID FROM @PageDetail A INNER JOIN rb_pages b on a.PageID = b.PageID)
		IF EXISTS(SELECT a.ParentPageID FROM @PageDetail A INNER JOIN rb_pages b on a.ParentPageID =B.PageID)
		BEGIN
			IF EXISTS(SELECT * FROM [dbo].[rb_Pages] WHERE PageID = @pageID) 
				AND @pageID <> -1
			BEGIN
				UPDATE Main
					SET Main.ParentPageID = Other.ParentPageID,
					Main.PageOrder = Other.PageOrder,
					Main.PortalID = Other.PortalID,
					Main.PageName = Other.PageName,
					Main.MobilePageName = Other.MobilePageName,
					Main.ShowMobile = Other.ShowMobile,
					Main.PageLayout = Other.PageLayout,
					Main.PageDescription = Other.PageDescription,
					Main.AuthorizedRoles = Other.AuthorizedRoles
				FROM [dbo].[rb_Pages] Main
				INNER JOIN @PageDetail Other ON
					Main.PageID = Other.PageID
				INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('In tabel rb_Pages:'+ convert(varchar(50),@pageID) + ', Page Updated Sucessfully.!!')
				SET @newPageID = @pageID
			END
			ELSE
			BEGIN
				INSERT INTO [dbo].[rb_Pages] (ParentPageID,PageOrder, PortalID,PageName,MobilePageName,ShowMobile,PageLayout,PageDescription,AuthorizedRoles)
				SELECT ParentPageID,PageOrder, PortalID,PageName,MobilePageName,ShowMobile,PageLayout,PageDescription,AuthorizedRoles 
				FROM @PageDetail

				/*Set Newly created pageID*/
				SET @newPageID = @@IDENTITY
				INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('In tabel rb_Pages, New Page:'+ convert(varchar(50),@newPageID) + ' added Sucessfully..!!')
			END

			/*FOR PAGE SETTINGS (TAB SETTINGS)*/

			INSERT INTO @TabSettings 
				SELECT
					PageSettings.value('@Name', 'NVARCHAR(50)') as SettingName,
					PageSettings.value('.', 'NVARCHAR(1500)') as SettingValue
				FROM @XMLString.nodes('Page/PageSettings/PageSetting') as Catalog(PageSettings)
	
		
			DECLARE curTabSettings CURSOR FOR SELECT * FROM @TabSettings
			OPEN curTabSettings
			FETCH NEXT FROM curTabSettings INTO @SettingName,@SettingValue
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF EXISTS(SELECT * FROM [dbo].[rb_TabSettings] WHERE SettingName = @SettingName AND TabID=@newPageID)
				BEGIN
					UPDATE rb_TabSettings SET SettingName = @SettingName,SettingValue = @SettingValue 
					WHERE SettingName = @SettingName AND TabID = @newPageID
					--INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('New Page Setting:'+ @SettingName +' and Value:'+@SettingValue+' of Page:'+ convert(varchar(50),@newPageID) + ' Update Sucessfully..!!')
				END
				ELSE
				BEGIN
					INSERT INTO [dbo].[rb_TabSettings] (TabID, SettingName, SettingValue) VALUES (@newPageID, @SettingName, @SettingValue)

					INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('New Page Setting:'+ @SettingName +' and Value:'+@SettingValue+' of Page:'+ convert(varchar(50),@newPageID) + ' added Sucessfully..!!')
				END
				FETCH NEXT FROM curTabSettings INTO @SettingName,@SettingValue
			END
			CLOSE curTabSettings
			DEALLOCATE  curTabSettings

			/*Insert Module data by TabID*/
			INSERT INTO @ModuleDetail 
				SELECT 
					ModuleDt.value('@ModuleID', 'int') AS ModuleID,
					ModuleDt.value('@ModuleDefID', 'int') as ModuleDefID,
					ModuleDt.value('@ModuleOrder', 'int') as ModuleOrder,
					ModuleDt.value('PaneName[1]', 'NVARCHAR(50)') as PaneName,
					ModuleDt.value('ModuleTitle[1]', 'NVARCHAR(256)') as ModuleTitle,
					ModuleDt.value('AuthorizedEditRoles[1]', 'NVARCHAR(256)') as AuthorizedEditRoles,
					ModuleDt.value('AuthorizedViewRoles[1]', 'NVARCHAR(256)') as AuthorizedViewRoles,
					ModuleDt.value('AuthorizedAddRoles[1]', 'NVARCHAR(256)') as AuthorizedAddRoles,
					ModuleDt.value('AuthorizedDeleteRoles[1]', 'NVARCHAR(256)') as AuthorizedDeleteRoles,
					ModuleDt.value('AuthorizedPropertiesRoles[1]', 'NVARCHAR(256)') as AuthorizedPropertiesRoles,
					ModuleDt.value('@CacheTime', 'int') as CacheTime,
					ModuleDt.value('@ShowMobile', 'bit') as ShowMobile,
					ModuleDt.value('AuthorizedPublishingRoles[1]', 'NVARCHAR(256)') as AuthorizedPublishingRoles,
					ModuleDt.value('@NewVersion', 'bit') as NewVersion,
					ModuleDt.value('@SupportWorkflow', 'bit') as SupportWorkflow,
					ModuleDt.value('AuthorizedApproveRoles[1]', 'NVARCHAR(256)') as AuthorizedApproveRoles,
					ModuleDt.value('@WorkflowState', 'tinyint') as WorkflowState,
					ModuleDt.value('@SupportCollapsable', 'bit') as SupportCollapsable,
					ModuleDt.value('@ShowEveryWhere', 'bit') as ShowEveryWhere,
					ModuleDt.value('AuthorizedMoveModuleRoles[1]', 'NVARCHAR(256)') as AuthorizedMoveModuleRoles,
					ModuleDt.value('AuthorizedDeleteModuleRoles[1]', 'NVARCHAR(256)') as AuthorizedDeleteModuleRoles
				FROM 
					@XMLString.nodes('Page/Modules/Module') AS XD(ModuleDt)

			/*Set ModuleID*/
			--SELECT @ModuleID = @ModuleId from @ModuleDetail

			DECLARE curModule CURSOR FOR SELECT * FROM @ModuleDetail
			OPEN curModule
			FETCH NEXT FROM curModule into @ModuleID,@ModuleDefID,@ModuleOrder,@PaneName,@ModuleTitle,
				@AuthorizedEditRoles,@AuthorizedViewRoles,@AuthorizedAddRoles,@AuthorizedDeleteRoles,
				@AuthorizedPropertiesRoles,@CacheTime,@ShowMobile,@AuthorizedPublishingRoles,@NewVersion,
				@SupportWorkflow,@AuthorizedApproveRoles,@WorkflowState,@SupportCollapsable,@ShowEveryWhere,
				@AuthorizedMoveModuleRoles,@AuthorizedDeleteModuleRoles

			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @NewModuleID = 0
				IF EXISTS(SELECT M.ModuleDefID FROM rb_Modules M INNER JOIN rb_ModuleDefinitions MD ON M.ModuleDefID = MD.ModuleDefID
							WHERE M.ModuleDefID = @ModuleDefID) AND ISNUMERIC(@ModuleDefID) = 1
				BEGIN
					/* VERIFY MODULE ID FOR INSERT/ADD or UPDATE */					
					IF EXISTS(SELECT * FROM rb_Modules WHERE ModuleID = @ModuleID)
					BEGIN
						UPDATE Main SET
							Main.ModuleOrder = Other.ModuleOrder ,
							Main.PaneName = Other.PaneName ,
							Main.ModuleTitle = Other.ModuleTitle ,
							Main.AuthorizedEditRoles = Other.AuthorizedEditRoles ,
							Main.AuthorizedViewRoles = Other.AuthorizedViewRoles ,
							Main.AuthorizedAddRoles = Other.AuthorizedAddRoles ,
							Main.AuthorizedDeleteRoles = Other.AuthorizedDeleteRoles ,
							Main.AuthorizedPropertiesRoles = Other.AuthorizedPropertiesRoles ,
							Main.CacheTime = Other.CacheTime ,
							Main.ShowMobile = Other.ShowMobile ,
							Main.AuthorizedPublishingRoles = Other.AuthorizedPublishingRoles ,
							Main.NewVersion = Other.NewVersion ,
							Main.SupportWorkflow  = Other.SupportWorkflow ,
							Main.AuthorizedApproveRoles  = Other.AuthorizedApproveRoles ,
							Main.WorkflowState = Other.WorkflowState,
							Main.SupportCollapsable  = Other.SupportCollapsable ,
							Main.ShowEveryWhere  = Other.ShowEveryWhere ,
							Main.AuthorizedMoveModuleRoles  = Other.AuthorizedMoveModuleRoles ,
							Main.AuthorizedDeleteModuleRoles = Other.AuthorizedDeleteModuleRoles,
							Main.TabID = @newPageID
						FROM [dbo].[rb_Modules] Main
							INNER JOIN @ModuleDetail Other ON Main.ModuleID = Other.ModuleID 
						WHERE Other.ModuleID = @ModuleID
						
						SET @NewModuleID = @ModuleID
						INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module:'+convert(varchar(50),@NewModuleID) +' With Title:'+@ModuleTitle+' of Page:'+ convert(varchar(50),@newPageID) + ' Updated Sucessfully..!!')
					END
					ELSE IF @ModuleID = -1
					BEGIN
						INSERT INTO [dbo].[rb_Modules] (TabID ,ModuleDefID ,ModuleOrder ,PaneName ,ModuleTitle ,AuthorizedEditRoles,
							AuthorizedViewRoles ,AuthorizedAddRoles ,AuthorizedDeleteRoles ,AuthorizedPropertiesRoles ,CacheTime ,ShowMobile ,
							AuthorizedPublishingRoles ,NewVersion ,SupportWorkflow ,AuthorizedApproveRoles ,WorkflowState,SupportCollapsable ,
							ShowEveryWhere ,AuthorizedMoveModuleRoles ,AuthorizedDeleteModuleRoles)
						SELECT @newPageID ,ModuleDefID ,ModuleOrder ,PaneName ,ModuleTitle ,AuthorizedEditRoles ,AuthorizedViewRoles,
							AuthorizedAddRoles ,AuthorizedDeleteRoles ,AuthorizedPropertiesRoles ,CacheTime ,ShowMobile ,
							AuthorizedPublishingRoles ,NewVersion ,SupportWorkflow ,AuthorizedApproveRoles ,WorkflowState,SupportCollapsable,
							ShowEveryWhere ,AuthorizedMoveModuleRoles ,AuthorizedDeleteModuleRoles 
						FROM @ModuleDetail
						WHERE ModuleID = @ModuleID

						SET @NewModuleID = @@IDENTITY
						INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module:'+convert(varchar(50),@NewModuleID) +' With Title:'+@ModuleTitle+' on Page:'+ convert(varchar(50),@newPageID) + ' Added Sucessfully..!!')
					END
					ELSE
					BEGIN
						INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('ModuleID: '+CONVERT(nvarchar(50),@ModuleID) +' is invalid or does not exits')
					END

					/*Populate into the @ModuleSettings for processing*/
					IF @NewModuleID > 0
					BEGIN
						INSERT INTO @ModuleSettings 
							SELECT
								PageSettings.value('@SettingName', 'NVARCHAR(50)') as SettingName,
								PageSettings.value('.', 'NVARCHAR(1500)') as SettingValue
							FROM 
								@XMLString.nodes('Page/Modules/Module[@ModuleID=sql:variable("@ModuleID")]/ModuleSettings/ModuleSetting') XD(PageSettings)
		
						DECLARE curModuleSettings CURSOR FOR SELECT * FROM @ModuleSettings 
						OPEN curModuleSettings
						FETCH NEXT FROM curModuleSettings INTO @SettingName, @SettingValue

						INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module Settings: '+convert(varchar(50),@NewModuleID))
						WHILE @@FETCH_STATUS = 0
						BEGIN
							IF EXISTS (SELECT  *  FROM  rb_ModuleSettings WHERE ModuleId = @NewModuleID AND SettingName = @SettingName)
							BEGIN
								--UPDATE Main SET
								--	Main.SettingValue = @SettingValue 
								--FROM rb_ModuleSettings Main 
								--where Main.SettingName = @SettingName AND Main.ModuleID = @NewModuleID
								UPDATE rb_ModuleSettings SET SettingName= @SettingName, SettingValue = @SettingValue 
								WHERE SettingName = @SettingName AND ModuleID = @NewModuleID
								--INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module:'+Convert(varchar(50),@ModuleID) +' Setting Name:'+@SettingName +' and Value:'+@SettingValue+' of Page:'+ convert(varchar(50),@newPageID) + ' Updated Sucessfully..!!')
							END
							ELSE
							BEGIN
								INSERT INTO rb_ModuleSettings (ModuleID, SettingName, SettingValue) 
									values(@NewModuleID, @SettingName, @SettingValue)
					
								INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module:'+Convert(varchar(50),@NewModuleID) +' Setting Name:'+@SettingName +' and Value:'+@SettingValue+' of Page:'+ convert(varchar(50),@newPageID) + ' Added Sucessfully..!!')
							END
							FETCH NEXT FROM curModuleSettings INTO @SettingName, @SettingValue
						END
						CLOSE curModuleSettings
						DEALLOCATE curModuleSettings
					END	
				END	
				ELSE
				BEGIN
					INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Module is not created due to invalid ModuleDefID:'+CONVERT(nvarchar(100),@ModuleDefID)+' or does not exist ModuleDefID in rb_ModuleDefinitions table')
				END
				FETCH NEXT FROM curModule into @ModuleID,@ModuleDefID,@ModuleOrder,@PaneName,@ModuleTitle,
					@AuthorizedEditRoles,@AuthorizedViewRoles,@AuthorizedAddRoles,@AuthorizedDeleteRoles,
					@AuthorizedPropertiesRoles,@CacheTime,@ShowMobile,@AuthorizedPublishingRoles,@NewVersion,
					@SupportWorkflow,@AuthorizedApproveRoles,@WorkflowState,@SupportCollapsable,@ShowEveryWhere,
					@AuthorizedMoveModuleRoles,@AuthorizedDeleteModuleRoles
			END
			CLOSE curModule
			DEALLOCATE curModule
			
		END
		ELSE
		BEGIN
			/*For Invalid ParentPageID or does not exist*/
			INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Invalid ParentPageID:'+Convert(nvarchar(20),@PPageID)+' or does not exist into the rb_Pages table')
		END
	END
	ELSE
	/*For Invalid page or does not exist*/
	BEGIN
		INSERT INTO @LogTableXMLImportExport (LogMessage) VALUES ('Invalid PageID:'+Convert(nvarchar(20),@pageID)+' or does not exist into the rb_Pages table')
	END
	SELECT LogMessage FROM @LogTableXMLImportExport
END
