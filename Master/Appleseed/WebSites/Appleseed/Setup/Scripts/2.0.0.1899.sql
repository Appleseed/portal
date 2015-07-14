
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[rb_Sliders]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[rb_Sliders](
	[SliderID] [int] IDENTITY(1,1) NOT NULL,
	[SliderName] [varchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserName] [varchar](500) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUserName] [varchar](500) NULL,
 CONSTRAINT [PK_rb_Sliders] PRIMARY KEY CLUSTERED 
(
	[SliderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

ALTER TABLE [dbo].[rb_Sliders] ADD  CONSTRAINT [DF_rb_Sliders_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[rb_SliderImages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[rb_SliderImages](
	[SliderImageID] [int] IDENTITY(1,1) NOT NULL,
	[SliderID] [int] NOT NULL,
	[SliderCaption] [varchar](200) NULL,
	[SliderImageExt] [varchar](5) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedUserName] [varchar](500) NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedUserName] [varchar](500) NULL,
	[Active] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_SliderImages] PRIMARY KEY CLUSTERED 
(
	[SliderImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF
ALTER TABLE [dbo].[rb_SliderImages] ADD  CONSTRAINT [DF_SliderImages_Active]  DEFAULT ((1)) FOR [Active]

ALTER TABLE [dbo].[rb_SliderImages] ADD  DEFAULT ((1)) FOR [DisplayOrder]

ALTER TABLE [dbo].[rb_SliderImages]  WITH CHECK ADD  CONSTRAINT [fk_deleteSlider] FOREIGN KEY([SliderID])
REFERENCES [dbo].[rb_Sliders] ([SliderID])
ON DELETE CASCADE

ALTER TABLE [dbo].[rb_SliderImages] CHECK CONSTRAINT [fk_deleteSlider]
END 
GO



