-----------------------
----1.2.8.1627.sql
-----------------------


---- Add new module: FAQs
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531000}'
--SET @FriendlyName = 'FAQs'
--SET @DesktopSrc = 'DesktopModules/FAQs/FAQs.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesFAQs'
--SET @Admin = 0
--SET @Searchable = 1

---- Installs module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

---- Install it for default portal
--EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1


--if Not exists (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_FAQs]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--BEGIN
--    CREATE TABLE [rb_FAQs] (
--	[ItemID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL ,
--	[CreatedByUser] [nvarchar] (100) NULL ,
--	[CreatedDate] [datetime] NULL ,
--	[Question] [nvarchar] (500) NULL ,
--	[Answer] [nvarchar] (4000) NULL 
--    ) ON [PRIMARY] 
--END
--GO



--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddFAQ]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddFAQ]
--GO

--CREATE PROCEDURE [rb_AddFAQ]
--	(@ItemID 	[int] OUTPUT,
--	 @ModuleID 	[int],
--	 @UserName	[nvarchar] (100),
--	 @Question 	[nvarchar] (500),
--	 @Answer 	[nvarchar] (4000))

--AS INSERT INTO [rb_FAQs]
--	([ModuleID],
--	 [CreatedByUser],
--	 [CreatedDate],
--	 [Question],
--	 [Answer]) 
 
--VALUES 
--	 (@ModuleID,
--	  @UserName,
--	  GetDate(),
--	  @Question,
--	  @Answer)

--SELECT 
--	@ItemID = @@IDENTITY

--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteFAQ]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DeleteFAQ]
--GO

--CREATE PROCEDURE [rb_DeleteFAQ]
--	(@ItemID 	[int])

--AS DELETE FROM [rb_FAQs]

--WHERE 
--	( [ItemID] = @ItemID)

--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetFAQ]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetFAQ]
--GO

--CREATE PROCEDURE rb_GetFAQ

--(@ModuleID int)

--AS

--SELECT ItemID, CreatedByUser, CreatedDate, Question, Answer
--FROM rb_FAQs 
--WHERE ModuleID = @ModuleID
--ORDER BY Question
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleFAQ]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSingleFAQ]
--GO

--CREATE PROCEDURE rb_GetSingleFAQ 
--(@ItemID int)

--AS

--SELECT CreatedByUser, CreatedDate, Question, Answer
--FROM rb_FAQs
--WHERE ItemID = @ItemID

--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateFAQ]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateFAQ]
--GO

--CREATE PROCEDURE [rb_UpdateFAQ]
--	(@ItemID 	[int],
--	 @UserName	[nvarchar] (100),
--	 @Question 	[nvarchar] (500),
--	 @Answer 	[nvarchar] (4000))

--AS UPDATE [rb_FAQs]

--SET  
--	 [CreatedByUser] = @UserName,
--	 [Question]	 = @Question,
--	 [Answer]	 = @Answer 

--WHERE 
--	( [ItemID]	 = @ItemID)

--GO


--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1627','1.2.8.1627', CONVERT(datetime, '04/25/2003', 101))
--GO
