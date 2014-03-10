/* Install script */

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HtmlText]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_HtmlText] (
	[ModuleID] [int] NOT NULL ,
	[DesktopHtml] [ntext] NOT NULL ,
	[MobileSummary] [ntext] NOT NULL ,
	[MobileDetails] [ntext] NOT NULL
	CONSTRAINT [PK_rb_HtmlText] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	),
	CONSTRAINT [FK_rb_HtmlText_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HtmlText_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_HtmlText_st] (
	[ModuleID] [int] NOT NULL ,
	[DesktopHtml] [ntext] NOT NULL ,
	[MobileSummary] [ntext] NOT NULL ,
	[MobileDetails] [ntext] NOT NULL
	CONSTRAINT [PK_rb_HtmlText_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ModuleID]
	),
	CONSTRAINT [FK_rb_HtmlText_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_HtmlText_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_HtmlText_stModified]
GO

CREATE    TRIGGER [rb_HtmlText_stModified]
ON [rb_HtmlText_st]
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

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_GetHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetHtmlText]
GO

CREATE   PROCEDURE rb_GetHtmlText
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF ( @WorkflowVersion = 1 )
	SELECT *
	FROM
	    rb_HtmlText
	WHERE
	    ModuleID = @ModuleID
ELSE
	SELECT *
	FROM
	    rb_HtmlText_st
	WHERE
	    ModuleID = @ModuleID
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID(N'[rb_UpdateHtmlText]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateHtmlText]
GO

CREATE   PROCEDURE rb_UpdateHtmlText
(
    @ModuleID      int,
    @DesktopHtml   ntext,
    @MobileSummary ntext,
    @MobileDetails ntext
)
AS

IF NOT EXISTS (
    SELECT 
        * 
    FROM 
        rb_HtmlText_st 
    WHERE 
        ModuleID = @ModuleID
)
INSERT INTO rb_HtmlText_st (
    ModuleID,
    DesktopHtml,
    MobileSummary,
    MobileDetails
) 
VALUES (
    @ModuleID,
    @DesktopHtml,
    @MobileSummary,
    @MobileDetails
)
ELSE
UPDATE
    rb_HtmlText_st

SET
    DesktopHtml   = @DesktopHtml,
    MobileSummary = @MobileSummary,
    MobileDetails = @MobileDetails

WHERE
    ModuleID = @ModuleID
GO


