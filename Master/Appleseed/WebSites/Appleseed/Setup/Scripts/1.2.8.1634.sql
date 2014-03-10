-----------------------
----1.2.8.1634.sql
-----------------------


---- Add new module: User Defined Table
--DECLARE @GeneralModDefID uniqueidentifier
--DECLARE @FriendlyName nvarchar(128)
--DECLARE @DesktopSrc nvarchar(256)
--DECLARE @MobileSrc nvarchar(256)
--DECLARE @AssemblyName varchar(50)
--DECLARE @ClassName nvarchar(128)
--DECLARE @Admin bit
--DECLARE @Searchable bit

--SET @GeneralModDefID = '{2502DB18-B580-4F90-8CB4-C15E6E531021}'
--SET @FriendlyName = 'User Defined Table'
--SET @DesktopSrc = 'DesktopModules/UserDefinedTable/UserDefinedTable.ascx'
--SET @MobileSrc = ''
--SET @AssemblyName = 'Appleseed.DLL'
--SET @ClassName = 'Appleseed.Content.Web.ModulesUserDefinedTable'
--SET @Admin = 0
--SET @Searchable = 0


---- Installs module
--EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable

---- Install it for default portal
--EXEC [rb_UpdateModuleDefinitions] @GeneralModDefID, 0, 1
--GO


--if NOT exists (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UserDefinedData]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
--BEGIN

---- Create the table AND add constraints
--CREATE TABLE [rb_UserDefinedData] (
--	[UserDefinedDataId] [int] IDENTITY (1, 1) NOT NULL ,
--	[UserDefinedFieldID] [int] NOT NULL ,
--	[UserDefinedRowID] [int] NOT NULL ,
--	[FieldValue] [nvarchar] (2000) NOT NULL 
--) ON [PRIMARY]

--CREATE TABLE [rb_UserDefinedFields] (
--	[UserDefinedFieldID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL ,
--	[FieldTitle] [varchar] (50) NOT NULL ,
--	[Visible] [bit] NOT NULL ,
--	[FieldOrder] [int] NOT NULL ,
--	[FieldType] [varchar] (20) NOT NULL 
--) ON [PRIMARY]

--CREATE TABLE [rb_UserDefinedRows] (
--	[UserDefinedRowID] [int] IDENTITY (1, 1) NOT NULL ,
--	[ModuleID] [int] NOT NULL 
--) ON [PRIMARY]

--ALTER TABLE [rb_UserDefinedData] WITH NOCHECK ADD 
--	CONSTRAINT [PK_UserDefinedData] PRIMARY KEY  CLUSTERED 
--	(
--		[UserDefinedDataId]
--	)  ON [PRIMARY] 

--ALTER TABLE [rb_UserDefinedFields] WITH NOCHECK ADD 
--	CONSTRAINT [PK_UserDefinedTable] PRIMARY KEY  CLUSTERED 
--	(
--		[UserDefinedFieldID]
--	)  ON [PRIMARY] 

--ALTER TABLE [rb_UserDefinedRows] WITH NOCHECK ADD 
--	CONSTRAINT [PK_UserDefinedRows] PRIMARY KEY  CLUSTERED 
--	(
--		[UserDefinedRowID]
--	)  ON [PRIMARY] 

--ALTER TABLE [rb_UserDefinedFields] WITH NOCHECK ADD 
--	CONSTRAINT [DF_UserDefinedFields_FieldOrder] DEFAULT (0) FOR [FieldOrder]

--ALTER TABLE [rb_UserDefinedData] ADD 
--	CONSTRAINT [FK_UserDefinedData_UserDefinedFields] FOREIGN KEY 
--	(
--		[UserDefinedFieldID]
--	) REFERENCES [rb_UserDefinedFields] (
--		[UserDefinedFieldID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION ,
--	CONSTRAINT [FK_UserDefinedData_UserDefinedRows] FOREIGN KEY 
--	(
--		[UserDefinedRowID]
--	) REFERENCES [rb_UserDefinedRows] (
--		[UserDefinedRowID]
--	) NOT FOR REPLICATION 

--ALTER TABLE [rb_UserDefinedFields] ADD 
--	CONSTRAINT [FK_UserDefinedFields_Modules] FOREIGN KEY 
--	(
--		[ModuleID]
--	) REFERENCES [rb_Modules] (
--		[ModuleID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION 

--ALTER TABLE [rb_UserDefinedRows] ADD 
--	CONSTRAINT [FK_UserDefinedRows_Modules] FOREIGN KEY 
--	(
--		[ModuleID]
--	) REFERENCES [rb_Modules] (
--		[ModuleID]
--	) ON DELETE CASCADE  NOT FOR REPLICATION 
--END
--GO



--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddUserDefinedField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddUserDefinedField]
--GO

--create procedure rb_AddUserDefinedField

--@ModuleID     int,
--@FieldTitle   varchar(50),
--@Visible      bit,
--@FieldType    varchar(20)

--as

--declare @FieldOrder int

--select @FieldOrder = count(*) + 1
--from   rb_UserDefinedFields
--WHERE  ModuleID = @ModuleID

--insert rb_UserDefinedFields ( 
--  ModuleID,
--  FieldTitle,
--  Visible,
--  FieldOrder,
--  FieldType
--)
--values (
--  @ModuleID,
--  @FieldTitle,
--  @Visible,
--  @FieldOrder,
--  @FieldType
--)

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddUserDefinedRow]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_AddUserDefinedRow]
--GO

--create procedure rb_AddUserDefinedRow

--@ModuleID         int,
--@UserDefinedRowID int OUTPUT

--as

--insert rb_UserDefinedRows ( 
--  ModuleID
--)
--values (
--  @ModuleID
--)

--select @UserDefinedRowID = @@IDENTITY
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteUserDefinedField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DeleteUserDefinedField]
--GO

--create procedure rb_DeleteUserDefinedField

--@UserDefinedFieldID    int 

--as

--delete 
--from   rb_UserDefinedData
--WHERE  UserDefinedFieldID = @UserDefinedFieldID

--delete 
--from   rb_UserDefinedFields
--WHERE  UserDefinedFieldID = @UserDefinedFieldID

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteUserDefinedRow]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_DeleteUserDefinedRow]
--GO

--create procedure rb_DeleteUserDefinedRow

--@UserDefinedRowID    int 

--as

--delete 
--from   rb_UserDefinedData
--WHERE  UserDefinedRowID = @UserDefinedRowID

--delete 
--from   rb_UserDefinedRows
--WHERE  UserDefinedRowID = @UserDefinedRowID

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleUserDefinedField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSingleUserDefinedField]
--GO

--create procedure rb_GetSingleUserDefinedField

--@UserDefinedFieldID  int

--as

--select ModuleID,
--       FieldTitle,
--       Visible,
--       FieldOrder
--from   rb_UserDefinedFields
--WHERE  UserDefinedFieldID = @UserDefinedFieldID

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleUserDefinedRow]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetSingleUserDefinedRow]
--GO

--create procedure rb_GetSingleUserDefinedRow

--@UserDefinedRowID   int,
--@ModuleID           int

--as

--select rb_UserDefinedFields.FieldTitle,
--       rb_UserDefinedData.FieldValue
--from   rb_UserDefinedData
--inner join rb_UserDefinedFields on rb_UserDefinedData.UserDefinedFieldID = rb_UserDefinedFields.UserDefinedFieldID
--WHERE  rb_UserDefinedData.UserDefinedRowID = @UserDefinedRowID
--AND    rb_UserDefinedFields.ModuleID = @ModuleID
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_getuserdefinedfields]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_getuserdefinedfields]
--GO

--create procedure rb_GetUserDefinedFields

--@ModuleID  int

--as

--select UserDefinedFieldID,
--       FieldTitle,
--       Visible,
--       FieldType
--from   rb_UserDefinedFields
--WHERE  ModuleID = @ModuleID
--order by FieldOrder

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetUserDefinedRows]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_GetUserDefinedRows]
--GO

--create procedure rb_GetUserDefinedRows

--@ModuleID    int 

--as

--select rb_UserDefinedRows.UserDefinedRowID,
--       rb_UserDefinedFields.FieldTitle,
--       rb_UserDefinedData.FieldValue
--from   rb_UserDefinedRows
--left outer join rb_UserDefinedData on rb_UserDefinedRows.UserDefinedRowID = rb_UserDefinedData.UserDefinedRowID
--inner join rb_UserDefinedFields on rb_UserDefinedData.UserDefinedFieldID = rb_UserDefinedFields.UserDefinedFieldID 
--WHERE  rb_UserDefinedRows.ModuleID = @ModuleID

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserDefinedData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateUserDefinedData]
--GO

--create procedure rb_UpdateUserDefinedData

--@UserDefinedRowID    int,
--@UserDefinedFieldID  int,
--@FieldValue          nvarchar(2000) = null

--as

--if @FieldValue is null
--begin
--  IF EXISTS ( select 1 from rb_UserDefinedData WHERE UserDefinedFieldID = @UserDefinedFieldID AND UserDefinedRowID = @UserDefinedRowID )
--  begin
--    delete
--    from rb_UserDefinedData
--    WHERE UserDefinedFieldID = @UserDefinedFieldID
--    AND UserDefinedRowID = @UserDefinedRowID
--  end
--end
--else
--begin
--  IF NOT EXISTS ( select 1 from rb_UserDefinedData WHERE UserDefinedFieldID = @UserDefinedFieldID AND UserDefinedRowID = @UserDefinedRowID )
--  begin
--    insert rb_UserDefinedData ( 
--      UserDefinedFieldID,
--      UserDefinedRowID,
--      FieldValue
--    )
--    values (
--      @UserDefinedFieldID,
--      @UserDefinedRowID,
--      @FieldValue
--    )
--  end
--  else
--  begin
--    update rb_UserDefinedData
--    set    FieldValue = @FieldValue
--    WHERE UserDefinedFieldID = @UserDefinedFieldID
--    AND UserDefinedRowID = @UserDefinedRowID
--  end
--end

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserDefinedField]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateUserDefinedField]
--GO

--create procedure rb_UpdateUserDefinedField

--@UserDefinedFieldID   int,
--@FieldTitle           varchar(50),
--@Visible              bit,
--@FieldType            varchar(20)

--as

--update rb_UserDefinedFields
--set    FieldTitle = @FieldTitle,
--       Visible = @Visible,
--       FieldType = @FieldType
--WHERE  UserDefinedFieldID = @UserDefinedFieldID

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserDefinedFieldOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateUserDefinedFieldOrder]
--GO

--create procedure rb_UpdateUserDefinedFieldOrder

--@UserDefinedFieldID  int,
--@Direction           int

--as

--declare @ModuleID int
--declare @FieldOrder int

--select @ModuleID = ModuleID,
--       @FieldOrder = FieldOrder
--from   rb_UserDefinedFields
--WHERE  UserDefinedFieldID = @UserDefinedFieldID

--if (@Direction = -1 AND @FieldOrder > 0) or (@Direction = 1 AND @FieldOrder < ( select (count(*) - 1) from rb_UserDefinedFields WHERE ModuleID = @ModuleID ))
--begin
--  update rb_UserDefinedFields
--  set    FieldOrder = @FieldOrder
--  WHERE  ModuleID = @ModuleID
--  AND    FieldOrder = @FieldOrder + @Direction

--  update rb_UserDefinedFields
--  set    FieldOrder = @FieldOrder + @Direction
--  WHERE  UserDefinedFieldID = @UserDefinedFieldID
--end

--return 1
--GO


--IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserDefinedRow]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--DROP PROCEDURE [rb_UpdateUserDefinedRow]
--GO

--create procedure rb_UpdateUserDefinedRow

--@UserDefinedRowID int

--as

--IF NOT EXISTS ( select 1 from rb_UserDefinedData WHERE UserDefinedRowID = @UserDefinedRowID )
--begin
--  delete
--  from   rb_UserDefinedRows
--  WHERE  userDefinedRowID = @UserDefinedRowID
--end
--GO


--/* add version info */
--INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1634','1.2.8.1634', CONVERT(datetime, '05/01/2003', 101))
--GO
