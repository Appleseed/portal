/* Install script */

if not exists (select * from sysobjects where id = object_id(N'[rb_EnhancedHtml]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_EnhancedHtml] (
	[ItemID] [int] IDENTITY (0, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[ViewOrder] [int] NULL ,
	[CultureCode] [int] NULL,
	[DesktopHtml] [ntext] NOT NULL ,
	CONSTRAINT [PK_rb_EnhancedHtml] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	),
	CONSTRAINT [FK_rb_EnhancedHtml_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

if not exists (select * from sysobjects where id = object_id(N'[rb_EnhancedHtml_st]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [rb_EnhancedHtml_st] (
	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CreatedByUser] [nvarchar] (100) NULL ,
	[CreatedDate] [datetime] NULL ,
	[Title] [nvarchar] (100) NULL ,
	[ViewOrder] [int] NULL ,
	[CultureCode] [int] NULL,
	[DesktopHtml] [ntext] NOT NULL ,
	CONSTRAINT [PK_rb_EnhancedHtml_st] PRIMARY KEY  NONCLUSTERED 
	(
		[ItemID]
	),
	CONSTRAINT [FK_rb_EnhancedHtml_st_rb_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
)
END
GO

if exists (select * from sysobjects where id = object_id(N'[rb_EnhancedHtml_stModified]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_EnhancedHtml_stModified]
GO

CREATE  TRIGGER [rb_EnhancedHtml_stModified]
ON rb_EnhancedHtml_st
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

if exists (select * from sysobjects where id = object_id(N'[rb_AddEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_AddEnhancedHtml]
GO

CREATE PROCEDURE rb_AddEnhancedHtml
(
    @ModuleID    int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ViewOrder   int,
    @CultureCode int,
    @DesktopHtml ntext,
    @ItemID      int OUTPUT
)
AS

INSERT INTO rb_EnhancedHtml_st
(
    ModuleID,
    CreatedByUser,
    CreatedDate,
    Title,
    ViewOrder,
    CultureCode,
    DesktopHtml
)
VALUES
(
    @ModuleID,
    @UserName,
    GetDate(),
    @Title,
    @ViewOrder,
    @CultureCode,
    @DesktopHtml
)

SELECT
    @ItemID = SCOPE_IDENTITY()
GO

if exists (select * from sysobjects where id = object_id(N'[rb_DeleteEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_DeleteEnhancedHtml]
GO

CREATE   PROCEDURE rb_DeleteEnhancedHtml
(
    @ItemID int
)
AS

DELETE FROM
    rb_EnhancedHtml_st

WHERE
    ItemID = @ItemID
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEnhancedHtml]
GO

CREATE PROCEDURE rb_GetEnhancedHtml
(
    @ModuleID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ViewOrder,
	    CultureCode,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
ELSE
	SELECT
	    ItemID,
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ViewOrder,
	    CultureCode,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml_st
	WHERE
	    ModuleID = @ModuleID
	ORDER BY
	    ViewOrder
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetEnhancedLocalizedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetEnhancedLocalizedHtml]
GO

CREATE PROCEDURE rb_GetEnhancedLocalizedHtml
(
    @ModuleID int,
    @CultureCode int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    ItemID,
	    Title,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml
	WHERE
	    (ModuleID = @ModuleID) and (CultureCode = @CultureCode)
	ORDER BY
	    ViewOrder
ELSE
	SELECT
	    ItemID,
	    Title,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml_st
	WHERE
	    (ModuleID = @ModuleID) and (CultureCode = @CultureCode)
	ORDER BY
	    ViewOrder
GO

if exists (select * from sysobjects where id = object_id(N'[rb_GetSingleEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_GetSingleEnhancedHtml]
GO

CREATE PROCEDURE rb_GetSingleEnhancedHtml
(
    @ItemID int,
    @WorkflowVersion int
)
AS

IF (@WorkflowVersion = 1)
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ViewOrder,
	    CultureCode,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml
	WHERE
	    ItemID = @ItemID
ELSE
	SELECT
	    CreatedByUser,
	    CreatedDate,
	    Title,
	    ViewOrder,
	    CultureCode,
	    DesktopHtml
	FROM
	    rb_EnhancedHtml_st
	WHERE
	    ItemID = @ItemID
GO

if exists (select * from sysobjects where id = object_id(N'[rb_UpdateEnhancedHtml]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [rb_UpdateEnhancedHtml]
GO

CREATE PROCEDURE rb_UpdateEnhancedHtml
(
    @ItemID      int,
    @UserName    nvarchar(100),
    @Title       nvarchar(100),
    @ViewOrder   int,
    @CultureCode int,
    @DesktopHtml ntext
)
AS
UPDATE
    rb_EnhancedHtml_st
SET
    CreatedByUser = @UserName,
    CreatedDate   = GetDate(),
    Title         = @Title,
    ViewOrder     = @ViewOrder,
    CultureCode   = @CultureCode,
    DesktopHtml   = @DesktopHtml
WHERE
    ItemID = @ItemID
GO
