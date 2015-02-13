/*Add new module detail*/
GO 

--IF NOT EXISTS (SELECT * FROM [dbo].[rb_ModuleDefinitions] WHERE PortalID = 0 AND GeneralModDefID = 'C1EA4115-E7F2-4CBC-B1E7-DDA46791493C')
--BEGIN																																	

--EXEC	[dbo].[rb_AddGeneralModuleDefinitions]
--		@GeneralModDefID = 'C1EA4115-E7F2-4CBC-B1E7-DDA46791493C',
--		@FriendlyName = N'Page Friendly URL',
--		@DesktopSrc = N'DesktopModules/CoreModules/PageFriendlyURL/PageFriendlyURL.ascx',
--		@MobileSrc = N'',
--		@AssemblyName = N'Applesseed.Dll',
--		@ClassName = N'Appleseed.DesktopModules.CoreModules.PageFriendlyURL.PageFriendlyURL',
--		@Admin = 1,
--		@Searchable = 1
--END
--GO

IF NOT EXISTS (SELECT * FROM [dbo].[rb_ModuleDefinitions] WHERE PortalID = 0 AND GeneralModDefID = 'C1EA4115-E7F2-4CBC-B1E7-DDA46791493C')
BEGIN																																	
	insert into [dbo].[rb_ModuleDefinitions] (PortalID, GeneralModDefID) values (0,'C1EA4115-E7F2-4CBC-B1E7-DDA46791493C');
END
GO

/* Add new column FriendlyName into the rb_GeneralModuleDefinitions table*/
alter table dbo.rb_Pages add FriendlyURL[nvarchar](512) 
GO

/* PROCEDURE FOR ADD NEW FRIEDLY URL*/
CREATE PROCEDURE [dbo].[rb_UpdateFriendlyURL]
(
	@PageID int,
	@friendlyURL nvarchar(512),
	@result int output
)

AS
BEGIN
	DECLARE @pgID int,
			@frndURL nvarchar(512),
			@frndUrlResult nvarchar(512)

	set @pgID = @PageID 
	set @frndURL = @friendlyURL 
	SET @frndUrlResult = (select FriendlyURL from rb_Pages where FriendlyURL like @frndURL)
	IF  ((@frndUrlResult IS NULL) OR exists (select FriendlyURL from rb_Pages where FriendlyURL like @frndURL and PageID = @PageID))
	BEGIN
		UPDATE rb_Pages SET FriendlyURL = @frndURL  WHERE pageID = @pgID
		SET @result = 1
		return @result
	 END
	 ELSE
	 BEGIN
		SET @result = 0
		return @result
	 END
END
GO

/*Get FriendlyURL from pageID*/
create procedure rb_GetFriendlyURLbyPageID
(
	@pageID int
)

AS
BEGIN
	SELECT FriendlyURL FROM RB_PAGES WHERE PageID=@pageID 
END
GO