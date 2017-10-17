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
	@ModifiedByUserName nvarchar(256),
	@CWCSS   ntext,
    @CWJS ntext,
    @CWHTML ntext,
	@CWJSCSSREF ntext
)
AS

IF NOT EXISTS (SELECT * FROM rb_HtmlText_st WHERE ModuleID = @ModuleID AND VersionNo = @VersionNo)
BEGIN
	IF @Published = 1 
	BEGIN
		UPDATE rb_HtmlText_st SET Published = 0
		WHERE ModuleID = @ModuleID
	END

	INSERT INTO rb_HtmlText_st (
		ModuleID,
		DesktopHtml,
		MobileSummary,
		MobileDetails,
		VersionNo,
		CreatedDate,
		CreatedByUserName,
		Published,
		CWCSS,
		CWJS,
		CWHTML,
		CWJSCSSREF
	) 
	VALUES (
		@ModuleID,
		@DesktopHtml,
		@MobileSummary,
		@MobileDetails,
		@VersionNo,
		@CreatedDate,
		@CreatedByUserName,
		@Published,
		@CWCSS,
		@CWJS,
		@CWHTML,
		@CWJSCSSREF
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
		Published = @Published,
		CWCSS = @CWCSS,
		CWJS = @CWJS,
		CWHTML = @CWHTML,
		CWJSCSSREF = @CWJSCSSREF
	WHERE
		ModuleID = @ModuleID AND VersionNo = @VersionNo
END