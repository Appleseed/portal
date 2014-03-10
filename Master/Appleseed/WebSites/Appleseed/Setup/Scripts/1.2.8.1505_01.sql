---------------------
--1.2.8.1505_01.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Localize_Cultures]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [Localize] DROP CONSTRAINT FK_Localize_Cultures
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Cultures]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Cultures]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Localize]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Localize]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[Sections]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [Sections]
GO

CREATE TABLE [Cultures] (
	[CultureCode] [varchar] (10) NOT NULL ,
	[NeutralCode] [char] (2) NULL ,
	[CountryID] [nchar] (2) NULL ,
	[Description] [nvarchar] (255) NULL ,
	[Identifier] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [Localize] (
	[TextKey] [nvarchar] (255) NOT NULL ,
	[CultureCode] [varchar] (10) NOT NULL ,
	[Description] [nvarchar] (1500) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [Sections] (
	[TextKey] [nvarchar] (255) NOT NULL ,
	[Section] [varchar] (255) NOT NULL 
) ON [PRIMARY]
GO

--Change countries table
BEGIN TRANSACTION
EXECUTE sp_rename N'Countries.PK_IDCountry', N'Tmp_CountryID', 'COLUMN'

EXECUTE sp_rename N'Countries.Tmp_CountryID', N'CountryID', 'COLUMN'

ALTER TABLE Countries ADD
	Code3 nchar(3) NULL,
	Number int NULL,
	Country nvarchar(255) NULL

ALTER TABLE Countries
	DROP COLUMN IT, EN, FR, ES, DE, PT

COMMIT
GO

-- Change states
BEGIN TRANSACTION
EXECUTE sp_rename N'States.PK_IDState', N'Tmp_StateID', 'COLUMN'
EXECUTE sp_rename N'States.Tmp_StateID', N'StateID', 'COLUMN'

EXECUTE sp_rename N'States.IDCountry_FK', N'Tmp_CountryID_1', 'COLUMN'
EXECUTE sp_rename N'States.Tmp_CountryID_1', N'CountryID', 'COLUMN'
COMMIT
GO

-- Change users
BEGIN TRANSACTION
EXECUTE sp_rename N'Users.IDCountry_FK', N'Tmp_CountryID_6', 'COLUMN'
EXECUTE sp_rename N'Users.IDState_FK', N'Tmp_StateID_7', 'COLUMN'
EXECUTE sp_rename N'Users.Tmp_CountryID_6', N'CountryID', 'COLUMN'
EXECUTE sp_rename N'Users.Tmp_StateID_7', N'StateID', 'COLUMN'
COMMIT
GO

BEGIN TRANSACTION
ALTER TABLE Cultures ADD CONSTRAINT
	PK_Cultures PRIMARY KEY CLUSTERED 
	(
	CultureCode
	) ON [PRIMARY]

ALTER TABLE Cultures ADD CONSTRAINT
	FK_Cultures_Countries FOREIGN KEY
	(
	CountryID
	) REFERENCES Countries
	(
	CountryID
	)
COMMIT
GO

ALTER TABLE Users
	DROP CONSTRAINT FK_Users_States
GO

ALTER TABLE Users
	DROP CONSTRAINT FK_Users_Countries
GO

ALTER TABLE Users ADD CONSTRAINT
	FK_Users_Countries FOREIGN KEY
	(
	CountryID
	) REFERENCES Countries
	(
	CountryID
	) ON UPDATE CASCADE
GO

ALTER TABLE Users ADD CONSTRAINT
	FK_Users_States FOREIGN KEY
	(
	StateID
	) REFERENCES States
	(
	StateID
	) ON UPDATE CASCADE
GO

