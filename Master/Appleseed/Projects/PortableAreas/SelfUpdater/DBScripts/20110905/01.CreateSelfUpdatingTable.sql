/****** Object:  Table [dbo].[SelfUpdatingPackages]    Script Date: 09/05/2011 10:43:03 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelfUpdatingPackages]') AND type in (N'U'))
DROP TABLE [dbo].[SelfUpdatingPackages]
GO

/****** Object:  Table [dbo].[SelfUpdatingPackages]    Script Date: 09/05/2011 10:43:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SelfUpdatingPackages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PackageId] [nvarchar](max) NOT NULL,
	[PackageVersion] [nvarchar](max) NOT NULL,
	[Source] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SelfUpdatingPackages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


