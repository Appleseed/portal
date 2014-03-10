CREATE TABLE [dbo].[UnitTest] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[CHRVAL] [char] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[INTVAL] [int] NULL ,
	[VCHRVAL] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BITVAL] [bit] NULL ,
	[DATEVAL] [datetime] NULL 
) ON [PRIMARY]
GO
