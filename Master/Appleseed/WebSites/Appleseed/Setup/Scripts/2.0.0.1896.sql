
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