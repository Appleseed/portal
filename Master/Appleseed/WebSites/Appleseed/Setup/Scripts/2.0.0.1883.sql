set xact_abort on
go

begin transaction
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [UserCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CategoryId] [uniqueidentifier] NOT NULL,
	[PortalId] [int] NOT NULL,
 CONSTRAINT [PK_UserCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [aspnet_CustomProfile](
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CategoryId] [int] NULL,
	[Company] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[Zip] [nvarchar](10) NULL,
	[City] [nvarchar](50) NULL,
	[CountryCode] [nchar](2) NULL,
	[StateId] [int] NULL,
	[Fax] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[SendNewsletter] [bit] NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[BirthDate] [datetime] NULL,
 CONSTRAINT [PK__aspnet_CustomPro__1475118B] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [aspnet_CustomProfile]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_CustomProfile_aspnet_Users] FOREIGN KEY([UserId])
REFERENCES [aspnet_Users] ([UserId])
GO

ALTER TABLE [aspnet_CustomProfile] CHECK CONSTRAINT [FK_aspnet_CustomProfile_aspnet_Users]
GO

ALTER TABLE [aspnet_CustomProfile]  WITH CHECK ADD  CONSTRAINT [FK_aspnet_CustomProfile_UserCategory] FOREIGN KEY([CategoryId])
REFERENCES [UserCategory] ([Id])
GO

ALTER TABLE [aspnet_CustomProfile] CHECK CONSTRAINT [FK_aspnet_CustomProfile_UserCategory]
GO



commit
go

