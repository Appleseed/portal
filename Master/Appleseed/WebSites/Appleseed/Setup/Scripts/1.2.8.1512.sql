---------------------
--1.2.8.1512.sql
---------------------

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Versions]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
CREATE TABLE [rb_Versions] (
	[Release] [int] NOT NULL ,
	[Version] [nvarchar] (50) NULL ,
	[ReleaseDate] [datetime] NULL 
) ON [PRIMARY]
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1512','1.2.8.1512', CONVERT(datetime, '03/12/2003', 101))
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

-- Alter the modules tables so it contains the extra necessary columns for the target column
IF NOT EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] WHERE [TABLE_NAME]='Links' AND [COLUMN_NAME] = 'Target') 
Begin
	ALTER Table [Links]
		ADD [Target] [nvarchar](10) not null
		CONSTRAINT Target DEFAULT '_new' 
End
GO

-- By marcb@hotmail.com
-- lines removed because staging tables are created later with a different name "st_"
--
/* History: 11.3.2003 Cory Isakson Added Target column */

-- Sections changes by Manu
BEGIN TRANSACTION
ALTER TABLE Sections ADD
	SectionType nvarchar(150) NULL,
	ControlType nvarchar(150) NULL,
	ControlID nvarchar(150) NULL

ALTER TABLE Sections
	DROP COLUMN [Section]
COMMIT
GO

-- Remove obsolete records
DELETE FROM Sections
GO

-- Refresh procedure
ALTER PROCEDURE LocalizeManager
(
	@Key			nvarchar(50),
	@Translation	nvarchar(255) = null,
	@SectionType	varchar(150) = '',
	@ControlType	varchar(150) = '',
	@ControlId		varchar(150) = ''
)
AS

DECLARE @DefaultLanguage varchar(10)
SET @DefaultLanguage = 'en'

IF NOT EXISTS 
(
SELECT    TextKey
FROM      Localize
WHERE     (TextKey = @Key) AND (CultureCode = @DefaultLanguage)
)
INSERT Localize (TextKey, CultureCode, Description) Values (@Key, @DefaultLanguage, @Translation)

ELSE

UPDATE Localize
SET Description = @Translation
WHERE (TextKey = @Key) AND (CultureCode = @DefaultLanguage)

--Update locations
IF NOT EXISTS 
(
SELECT    TextKey
FROM      Sections
WHERE     (TextKey = @Key) AND (SectionType = @SectionType AND ControlType = @ControlType AND ControlID = @ControlId)
)
INSERT Sections (TextKey, SectionType, ControlType, ControlId) Values (@Key, @SectionType, @ControlType, @ControlId)
GO



