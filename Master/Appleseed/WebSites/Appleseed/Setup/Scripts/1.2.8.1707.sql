-----------------------
----1.2.8.1707.sql
-----------------------


---- Add new module: Survey
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531018}'
--SET @FriendlyName = 'Survey'
--SET @DesktopSrc = 'DesktopModules/Survey/Survey.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesSurvey'
--SET @Admin = 0
--SET @Searchable = 0


--IF NOT EXISTS (SELECT GeneralModDefID FROM rb_GeneralModuleDefinitions
--WHERE GeneralModDefID = @GeneralModDefID)
--BEGIN
--    -- Installs module
--    EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

--    -- Install it for default portal
--    EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
--END
--GO

--if NOT exists (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Surveys]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--BEGIN
--    -- Create the table AND add constraints
--    CREATE TABLE [rb_Surveys] (
--	[SurveyID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL ,
--	[SurveyDesc] [nvarchar] (500) NOT NULL ,
--	[CreatedByUser] [nvarchar] (100) NOT NULL ,
--	[CreatedDate] [datetime] NOT NULL 
--    ) ON [PRIMARY]

--    CREATE TABLE [rb_SurveyAnswers] (
--	[AnswerID] [int] IDENTITY (1, 1) NOT NULL ,
--	[SurveyID] [int] NOT NULL ,
--	[QuestionID] [int] NOT NULL ,
--	[OptionID] [int] NOT NULL 
--    ) ON [PRIMARY]

--    CREATE TABLE [rb_SurveyOptions] (
--	[OptionID] [int] IDENTITY (1, 1) NOT NULL ,
--	[QuestionID] [int] NOT NULL ,
--	[ViewOrder] [int] NOT NULL ,
--	[OptionDesc] [nvarchar] (500) NOT NULL 
--    ) ON [PRIMARY]

--    CREATE TABLE [rb_SurveyQuestions] (
--	[QuestionID] [int] IDENTITY (1, 1) NOT NULL ,
--	[SurveyID] [int] NOT NULL ,
--	[Question] [nvarchar] (500) NOT NULL ,
--	[ViewOrder] [int] NOT NULL ,
--	[TypeOption] [nvarchar] (2) NOT NULL 
--    ) ON [PRIMARY]

--    ALTER TABLE [rb_Surveys] ADD 
--	CONSTRAINT [FK_rb_Surveys_Modules] FOREIGN KEY 
--	(
--		[ModuleID]
--	) REFERENCES [rb_Modules] (
--		[ModuleID]
--	) NOT FOR REPLICATION 

--    ALTER TABLE [rb_SurveyAnswers] WITH NOCHECK ADD 
--	CONSTRAINT [PK_SurveyAnswers] PRIMARY KEY  CLUSTERED 
--	(
--		[AnswerID]
--	)  ON [PRIMARY] 

--    ALTER TABLE [rb_SurveyOptions] WITH NOCHECK ADD 
--	CONSTRAINT [PK_rb_SurveyOptions] PRIMARY KEY  CLUSTERED 
--	(
--		[OptionID]
--	)  ON [PRIMARY] 

--    ALTER TABLE [rb_SurveyQuestions] WITH NOCHECK ADD 
--	CONSTRAINT [PK_rb_SurveyQuestions] PRIMARY KEY  CLUSTERED 
--	(
--		[QuestionID]
--	)  ON [PRIMARY] 

--    ALTER TABLE [rb_Surveys] WITH NOCHECK ADD 
--	CONSTRAINT [PK_rb_Surveys] PRIMARY KEY  CLUSTERED 
--	(
--		[SurveyID]
--	)  ON [PRIMARY] 

--    ALTER TABLE [rb_SurveyQuestions] WITH NOCHECK ADD 
--	CONSTRAINT [DF_rb_SurveyQuestions_TypeOption] DEFAULT ('RD') FOR [TypeOption]

--    ALTER TABLE [rb_SurveyAnswers] ADD 
--	CONSTRAINT [FK_rb_SurveyAnswers_rb_Surveys] FOREIGN KEY 
--	(
--		[SurveyID]
--	) REFERENCES [rb_Surveys] (
--		[SurveyID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION 

--    ALTER TABLE [rb_SurveyOptions] ADD 
--	CONSTRAINT [FK_rb_SurveyOptions_rb_SurveyQuestions] FOREIGN KEY 
--	(
--		[QuestionID]
--	) REFERENCES [rb_SurveyQuestions] (
--		[QuestionID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION 

--    ALTER TABLE [rb_SurveyQuestions] ADD 
--	CONSTRAINT [FK_rb_SurveyQuestions_rb_Surveys] FOREIGN KEY 
--	(
--		[SurveyID]
--	) REFERENCES [rb_Surveys] (
--		[SurveyID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION 


--    alter table [rb_Surveys] nocheck constraint [FK_rb_Surveys_Modules]
--END
--GO



--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddSurveyAnswer]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddSurveyAnswer]
--GO

--CREATE PROCEDURE rb_AddSurveyAnswer
--(
--    @SurveyID int,
--    @QuestionID int,
--    @OptionID int,
--    @ItemID   int OUTPUT
--)
--AS
--INSERT INTO rb_SurveyAnswers
--(
--    SurveyID,
--    QuestionID,
--    OptionID
--)
--VALUES
--(
--    @SurveyID,
--    @QuestionID,
--    @OptionID
--)
--SELECT
--    @ItemID = @@IDENTITY
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddSurveyOption]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddSurveyOption]
--GO

--CREATE PROCEDURE rb_AddSurveyOption
--(
--    @QuestionID int,
--    @OptionDesc    nvarchar(500),
--    @ViewOrder  int,
--    @OptionID      int OUTPUT
--)

--AS

--INSERT INTO rb_SurveyOptions
--(
--    QuestionID,
--    OptionDesc,
--    ViewOrder

--)
--VALUES
--(
--    @QuestionID,
--    @OptionDesc,
--    @ViewOrder

--)
--SELECT
--    @OptionID = @@IDENTITY
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddSurveyQuestion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddSurveyQuestion]
--GO

--CREATE PROCEDURE rb_AddSurveyQuestion
--(
--    @ModuleID int,
--    @Question    nvarchar(500),
--    @ViewOrder  int,
--    @TypeOption  nvarchar(2),
--    @QuestionID      int OUTPUT
--)
--AS
--Declare @SurveyID  int

--SELECT @SurveyID =  SurveyID 
--FROM rb_Surveys
--WHERE ModuleID = @ModuleID

--INSERT INTO rb_SurveyQuestions
--(
--    SurveyID,
--    Question,
--    ViewOrder,
--    TypeOption
--)
--VALUES
--(
--    @SurveyID,
--    @Question,
--    @ViewOrder,
--    @TypeOption
--)
--SELECT
--    @QuestionID = @@IDENTITY
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DelSurveyOption]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DelSurveyOption]
--GO

--CREATE PROCEDURE rb_DelSurveyOption
--(
--    @OptionID       int
--)
--AS
--DELETE FROM 
--    rb_SurveyOptions 
--WHERE 
--    OptionID = @optionID

--Delete from
--    rb_SurveyAnswers
--WHERE
--    OptionID = @optionID
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DelSurveyQuestion]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DelSurveyQuestion]
--GO

--CREATE PROCEDURE rb_DelSurveyQuestion
--(
--    @QuestionID       int
--)
--AS
--DELETE FROM 
--    rb_SurveyQuestions 
--WHERE 
--    QuestionID = @QuestionID
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ExistAddSurvey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_ExistAddSurvey]
--GO

--CREATE PROCEDURE rb_ExistAddSurvey
--(
--@ModuleID int,
--@CreatedByUser nvarchar(100),
--@SurveyDesc nvarchar(500) OUTPUT
--)

--AS

--SELECT @SurveyDesc = ModuleTitle
--From rb_Modules
--Where ModuleID = @ModuleID


--Select SurveyID 
--From rb_Surveys
--Where ModuleID = @ModuleID

--If   @@RowCount = 0
--	Insert into rb_Surveys (ModuleID, SurveyDesc, CreatedByUser, CreatedDate)
--             Values (@ModuleID,@SurveyDesc, @CreatedByUser, getdate())
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ExistSurvey]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_ExistSurvey]
--GO

--CREATE PROCEDURE rb_ExistSurvey
--(
--@ModuleID int,
--@RowCount int OUTPUT
--)

--AS

--Select SurveyID 
--From rb_Surveys
--Where ModuleID = @ModuleID

--Select  @RowCount = @@RowCount
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyAnswers]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyAnswers]
--GO

--CREATE PROCEDURE rb_GetSurveyAnswers
--(
--    @SurveyID int
--)
--AS

--SELECT

-- rb_SurveyAnswers.QuestionID, 
-- rb_SurveyQuestions.Question,
-- rb_SurveyAnswers.OptionID,
-- rb_SurveyOptions.OptionDesc, 
-- Count(rb_SurveyAnswers.OptionID) AS Num

--FROM rb_Surveys
-- JOIN rb_SurveyAnswers
--    ON rb_Surveys.SurveyID = rb_SurveyAnswers.SurveyID
-- JOIN rb_SurveyQuestions
--    ON rb_SurveyAnswers.QuestionID = rb_SurveyQuestions.QuestionID
-- JOIN rb_SurveyOptions
--    ON rb_SurveyAnswers.OptionID = rb_SurveyOptions.OptionID

--GROUP BY rb_Surveys.SurveyID, 
--         rb_SurveyAnswers.QuestionID,
--         rb_SurveyQuestions.Question, 
--         rb_SurveyAnswers.OptionID, 
--         rb_SurveyOptions.OptionDesc,
--	 rb_SurveyQuestions.ViewOrder
--HAVING
--   rb_Surveys.SurveyID = @SurveyID
--ORDER BY
--   rb_SurveyQuestions.ViewOrder
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyAnswersNum]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyAnswersNum]
--GO

--CREATE PROCEDURE rb_GetSurveyAnswersNum
--(
--    @SurveyID int,
--    @QuestionID int,
--    @NumAnswer int OUTPUT
--)
--AS

--SELECT

-- @NumAnswer = Count(rb_SurveyAnswers.QuestionID) 
 
--FROM rb_SurveyAnswers
     
--GROUP BY rb_SurveyAnswers.SurveyID, 
--         rb_SurveyAnswers.QuestionID

--HAVING
--   rb_SurveyAnswers.SurveyID = @SurveyID AND 
--   rb_SurveyAnswers.QuestionID = @QuestionID


--If @@RowCount = 0
--	Set @NumAnswer = 0
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyDimArray]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyDimArray]
--GO

--CREATE PROCEDURE rb_GetSurveyDimArray

--( 
--  @ModuleID as int,
--  @TypeOption as nvarchar(2),
--  @DimArray as int OUTPUT

--)
-- AS


--SELECT  @DimArray = count(OptionID) 
--FROM rb_SurveyOptions
--	JOIN rb_SurveyQuestions
--		ON rb_SurveyOptions.QuestionID = rb_SurveyQuestions.QuestionID
--	JOIN rb_Surveys
--		ON rb_SurveyQuestions.SurveyID = rb_Surveys.SurveyID

--WHERE rb_Surveys.ModuleID = @ModuleID  AND
--      rb_SurveyQuestions.TypeOption = @TypeOption
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyID]
--GO

--CREATE PROCEDURE rb_GetSurveyID
--(
--    @ModuleID int,
--    @SurveyID int OUTPUT
--)
--AS


--SELECT

-- SurveyID = @SurveyID

--FROM rb_Surveys

--WHERE
--    ModuleID = @ModuleID

--IF  (@SurveyID = NULL) 
--	Set @SurveyID = 0
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyOptionList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyOptionList]
--GO

--CREATE PROCEDURE rb_GetSurveyOptionList
--(
--    @QuestionID int

--)
--AS


--SELECT

--  OptionID,
--  OptionDesc, 
--  ViewOrder

--FROM rb_SurveyOptions

--WHERE
--    QuestionID = @QuestionID 

--ORDER BY  ViewOrder
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyOptions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyOptions]
--GO

--CREATE PROCEDURE rb_GetSurveyOptions
--(
--    @ModuleID int,
--   @TypeOption nvarchar(2)
--)
--AS


--SELECT


--  OptionDesc 

--FROM rb_Surveys
--	JOIN rb_SurveyQuestions 
--	ON rb_Surveys.SurveyID = rb_SurveyQuestions.SurveyID
--		JOIN rb_SurveyOptions
--		ON rb_SurveyQuestions.QuestionID = rb_SurveyOptions.QuestionID

--WHERE
--    ModuleID = @ModuleID AND TypeOption = @TypeOption

--ORDER BY  rb_SurveyOptions.ViewOrder
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyQuestionList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyQuestionList]
--GO

--CREATE PROCEDURE rb_GetSurveyQuestionList
--(
--    @ModuleID int
--)
--AS


--SELECT

-- rb_Surveys.SurveyID, 
-- rb_SurveyQuestions.QuestionID, 
-- Question,
-- ViewOrder,
-- TypeOption
 

--FROM rb_Surveys
--JOIN rb_SurveyQuestions 
--ON rb_Surveys.SurveyID = rb_SurveyQuestions.SurveyID

--WHERE
--    ModuleID = @ModuleID

--ORDER BY  rb_SurveyQuestions.ViewOrder
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSurveyQuestions]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSurveyQuestions]
--GO

--CREATE PROCEDURE rb_GetSurveyQuestions
--(
--    @ModuleID int
--)
--AS


--SELECT

-- rb_Surveys.SurveyID, 
-- SurveyDesc, 
-- CreatedByUser, 
-- CreatedDate, 
-- rb_SurveyQuestions.QuestionID, 
-- Question, 
-- TypeOption, 
-- OptionDesc, 
-- OptionID

--FROM rb_Surveys
--JOIN rb_SurveyQuestions 
--ON rb_Surveys.SurveyID = rb_SurveyQuestions.SurveyID
--JOIN rb_SurveyOptions
--ON rb_SurveyQuestions.QuestionID = rb_SurveyOptions.QuestionID

--WHERE
--    ModuleID = @ModuleID

--ORDER BY  rb_SurveyQuestions.ViewOrder, rb_SurveyOptions.ViewOrder
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateSurveyOptionOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateSurveyOptionOrder]
--GO

--CREATE PROCEDURE rb_UpdateSurveyOptionOrder
--(
--    @OptionID         int,
--    @Order int

--)
--AS
--UPDATE
--    rb_SurveyOptions

--SET
--     ViewOrder = @Order    

--WHERE
--   OptionID   = @OptionID
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateSurveyQuestionOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateSurveyQuestionOrder]
--GO

--CREATE PROCEDURE rb_UpdateSurveyQuestionOrder
--(
--    @QuestionID         int,
--    @Order int

--)
--AS
--UPDATE
--    rb_SurveyQuestions

--SET
--     ViewOrder = @Order    

--WHERE
--    QuestionID   = @QuestionID
--GO



--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1707','1.2.8.1707', CONVERT(datetime, '05/01/2003', 101))
--GO
