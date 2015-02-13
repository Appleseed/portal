Alter table [dbo].[rb_HtmlText_st] ADD VersionNo int NOT NULL DEFAULT 1, Published bit NOT NULL DEFAULT 0, 
	CreatedDate datetime, CreatedByUserName nvarchar(256), 
	ModifiedDate datetime, ModifiedByUserName nvarchar(256)
GO

Alter table [dbo].[rb_HtmlText] ADD VersionNo int NOT NULL DEFAULT 1, Published bit NOT NULL DEFAULT 0, 
	CreatedDate datetime, CreatedByUserName nvarchar(256), 
	ModifiedDate datetime, ModifiedByUserName nvarchar(256)
GO

UPDATE [dbo].[rb_HtmlText_st] SET Published = 1, CreatedDate = GETDATE(), CreatedByUserName ='Admin'
GO
UPDATE [dbo].[rb_HtmlText] SET Published = 1, CreatedDate = GETDATE(), CreatedByUserName ='Admin'
GO

ALTER TABLE [dbo].[rb_HtmlText_st] DROP CONSTRAINT [PK_rb_HtmlText_st]
GO
ALTER TABLE [dbo].[rb_HtmlText] DROP CONSTRAINT [PK_rb_HtmlText]
GO

ALTER TABLE [dbo].[rb_HtmlText_st] ADD  CONSTRAINT [PK_rb_HtmlText_st] PRIMARY KEY ([ModuleID], [VersionNo])
GO

ALTER TABLE [dbo].[rb_HtmlText] ADD  CONSTRAINT [PK_rb_HtmlText] PRIMARY KEY ([ModuleID], [VersionNo])
GO

ALTER   PROCEDURE [dbo].[rb_GetHtmlText]
(
    @ModuleID int,
    @WorkflowVersion int,
	@VersionNo int
)
AS
BEGIN
	IF ( @WorkflowVersion = 1 )
		SELECT * FROM rb_HtmlText
		WHERE ModuleID = @ModuleID AND VersionNo = @VersionNo
	ELSE
		SELECT * FROM rb_HtmlText_st
		WHERE ModuleID = @ModuleID AND VersionNo = @VersionNo
END
GO

CREATE PROCEDURE [dbo].[rb_GetPublishedVersionHtmlText]
(
    @ModuleID int,
    @WorkflowVersion int
)
AS
BEGIN
	IF ( @WorkflowVersion = 1 )
		SELECT *
		FROM
			rb_HtmlText
		WHERE
			ModuleID = @ModuleID
			AND Published = 1
	ELSE
		SELECT *
		FROM
			rb_HtmlText_st
		WHERE
			ModuleID = @ModuleID
			AND Published = 1
END
GO


ALTER   PROCEDURE [dbo].[rb_UpdateHtmlText]
(
    @ModuleID      int,
    @DesktopHtml   ntext,
    @MobileSummary ntext,
    @MobileDetails ntext,
	@VersionNo int,
	@Published bit,
	@CreatedDate datetime,
	@CreatedByUserName nvarchar(256),
	@ModifiedDate datetime,
	@ModifiedByUserName nvarchar(256)
)
AS

IF NOT EXISTS (SELECT * FROM rb_HtmlText_st WHERE ModuleID = @ModuleID AND VersionNo = @VersionNo)
BEGIN
	INSERT INTO rb_HtmlText_st (
		ModuleID,
		DesktopHtml,
		MobileSummary,
		MobileDetails,
		VersionNo,
		CreatedDate,
		CreatedByUserName,
		Published
	) 
	VALUES (
		@ModuleID,
		@DesktopHtml,
		@MobileSummary,
		@MobileDetails,
		@VersionNo,
		@CreatedDate,
		@CreatedByUserName,
		@Published
	)
END
ELSE
BEGIN
	IF @Published = 1 
	BEGIN
		UPDATE rb_HtmlText_st SET Published = 0
		WHERE ModuleID = @ModuleID
	END

	UPDATE rb_HtmlText_st
	SET
		DesktopHtml		 = @DesktopHtml,
		MobileSummary	 = @MobileSummary,
		MobileDetails	 = @MobileDetails,
		VersionNo		 = @VersionNo,
		ModifiedDate	 = @ModifiedDate,
		ModifiedByUserName = @ModifiedByUserName,
		Published = @Published
	WHERE
		ModuleID = @ModuleID AND VersionNo = @VersionNo
END
GO



CREATE  PROCEDURE [dbo].[rb_HtmlTextVersionList]
(
    @ModuleID      int
)
AS
BEGIN
	SELECT ModuleID, VersionNo, Published FROM rb_HtmlText_st WHERE ModuleID = @ModuleID
END
GO

CREATE  PROCEDURE [dbo].[rb_GetHtmlTextVersionHistory]
(
    @ModuleID  int
)
AS
BEGIN
	SELECT VersionNo, CreatedByUserName, CreatedDate, ModifiedByUserName, ModifiedDate from rb_HtmlText_st where ModuleID = @ModuleID
END
GO

