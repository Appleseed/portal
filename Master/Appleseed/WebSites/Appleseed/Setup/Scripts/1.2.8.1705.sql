---------------------
--1.2.8.1705.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_ShipZones_rb_ShipPrices]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_ShipZones] DROP CONSTRAINT FK_rb_ShipZones_rb_ShipPrices
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_ShipZones_rb_Countries]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_ShipZones] DROP CONSTRAINT FK_rb_ShipZones_rb_Countries
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Products_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Products] DROP CONSTRAINT FK_Products_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Products_st_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Products_st] DROP CONSTRAINT FK_Products_st_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Orders_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Orders] DROP CONSTRAINT FK_Orders_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_rb_OrderDetails_rb_Orders]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_OrderDetails] DROP CONSTRAINT FK_rb_OrderDetails_rb_Orders
GO

/****** Object:  Trigger dbo.rb_Products_stModified    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Products_stModified]
GO

/****** Object:  Stored PROCEDURE dbo.rb_AddOrderDetails    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrderDetails]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrderDetails    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetOrderDetails]
GO

/****** Object:  Stored PROCEDURE dbo.rb_AddOrder    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrder]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartList    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartList]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartTotal    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartTotal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartTotal]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartTotalShipping    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartTotalShipping]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartTotalShipping]
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateProduct    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateProduct]
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateOrder    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateOrder]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrdersByUser    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetOrdersByUser]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetOrdersByUser]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrders    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetOrders]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetOrders]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleOrder    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleOrder]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleProduct    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleProduct]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProducts    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProducts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProducts]
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateUserFullNoPassword    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFullNoPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFullNoPassword]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartRemoveAllItems    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartRemoveAllItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartRemoveAllItems]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProductsPaged    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProductsPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProductsPaged]
GO

/****** Object:  Table [rb_OrderDetails]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_OrderDetails]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_OrderDetails]
GO

/****** Object:  Table [rb_Orders]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Orders]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Orders]
GO

/****** Object:  Table [rb_Products]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Products]
GO

/****** Object:  Table [rb_Products_st]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Products_st]
GO

/****** Object:  Table [rb_ShipZones]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ShipZones]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_ShipZones]
GO

/****** Object:  Table [rb_ShipPrices]    Script Date: 5/5/2003 7:07:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ShipPrices]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_ShipPrices]
GO

/****** Object:  Table [rb_ShipPrices]    Script Date: 5/5/2003 7:08:16 PM ******/
CREATE TABLE [rb_ShipPrices] (
	[ShipPriceID] [int] IDENTITY (1, 1) NOT NULL ,
	[Weight] [float] NOT NULL ,
	[Price] [money] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [rb_ShipZones]    Script Date: 5/5/2003 7:08:21 PM ******/
CREATE TABLE [rb_ShipZones] (
	[CountryID] [nchar] (2) NOT NULL ,
	[ShipPriceID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [rb_Orders]    Script Date: 5/5/2003 7:08:22 PM ******/
CREATE TABLE [rb_Orders] (
	[OrderID] [char] (24) NOT NULL ,
	[UserID] [int] NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[TotalGoods] [money] NULL ,
	[TotalShipping] [money] NULL ,
	[TotalTaxes] [money] NULL ,
	[TotalExpenses] [money] NULL ,
	[TotalWeight] [real] NULL ,
	[DateCreated] [datetime] NULL ,
	[DateModified] [datetime] NULL ,
	[Status] [int] NULL ,
	[PaymentMethod] [nvarchar] (50) NULL ,
	[ShippingMethod] [nvarchar] (50) NULL ,
	[ShippingData] [ntext] NULL ,
	[BillingData] [ntext] NULL ,
	[TransactionID] [nvarchar] (50) NULL ,
	[AuthCode] [nvarchar] (50) NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [rb_Products]    Script Date: 5/5/2003 7:08:23 PM ******/
CREATE TABLE [rb_Products] (
	[ProductID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CategoryID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[ModelNumber] [nvarchar] (256) NULL ,
	[ModelName] [nvarchar] (256) NULL ,
	[UnitPrice] [money] NOT NULL ,
	[FeaturedItem] [bit] NOT NULL ,
	[LongDescription] [ntext] NULL ,
	[ShortDescription] [ntext] NULL ,
	[MetadataXml] [ntext] NULL ,
	[TaxRate] [float] NULL ,
	[Weight] [float] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [rb_Products_st]    Script Date: 5/5/2003 7:08:23 PM ******/
CREATE TABLE [rb_Products_st] (
	[ProductID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CategoryID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[ModelNumber] [nvarchar] (256) NULL ,
	[ModelName] [nvarchar] (256) NULL ,
	[UnitPrice] [money] NOT NULL ,
	[FeaturedItem] [bit] NOT NULL ,
	[LongDescription] [ntext] NULL ,
	[ShortDescription] [ntext] NULL ,
	[MetadataXml] [ntext] NULL ,
	[TaxRate] [float] NULL ,
	[Weight] [float] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [rb_OrderDetails]    Script Date: 5/5/2003 7:08:23 PM ******/
CREATE TABLE [rb_OrderDetails] (
	[OrderID] [char] (24) NOT NULL ,
	[ProductID] [int] NOT NULL ,
	[Quantity] [int] NOT NULL ,
	[ModelName] [nvarchar] (256) NOT NULL ,
	[ModelNumber] [nvarchar] (256) NOT NULL ,
	[UnitPrice] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [rb_ShipPrices] WITH NOCHECK ADD 
	CONSTRAINT [PK_ShipPrices] PRIMARY KEY  CLUSTERED 
	(
		[ShipPriceID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [rb_Orders] WITH NOCHECK ADD 
	CONSTRAINT [PK_Orders] PRIMARY KEY  CLUSTERED 
	(
		[OrderID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Products] WITH NOCHECK ADD 
	CONSTRAINT [PK_Products] PRIMARY KEY  CLUSTERED 
	(
		[ProductID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Products_st] WITH NOCHECK ADD 
	CONSTRAINT [PK_Products_st] PRIMARY KEY  CLUSTERED 
	(
		[ProductID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Orders] WITH NOCHECK ADD 
	CONSTRAINT [DF_rb_Orders_TotalGoods] DEFAULT (0) FOR [TotalGoods],
	CONSTRAINT [DF_rb_Orders_TotalShipping] DEFAULT (0) FOR [TotalShipping],
	CONSTRAINT [DF_rb_Orders_TotalTaxes] DEFAULT (0) FOR [TotalTaxes],
	CONSTRAINT [DF_rb_Orders_TotalExpenses] DEFAULT (0) FOR [TotalExpenses],
	CONSTRAINT [DF_rb_Orders_TotalWeight] DEFAULT (0) FOR [TotalWeight]
GO

ALTER TABLE [rb_Products] WITH NOCHECK ADD 
	CONSTRAINT [DF_Products_DisplayOrder] DEFAULT (0) FOR [DisplayOrder],
	CONSTRAINT [DF_Products_SalePrice] DEFAULT (0) FOR [UnitPrice],
	CONSTRAINT [DF_Products_FeaturedItem] DEFAULT (0) FOR [FeaturedItem]
GO

ALTER TABLE [rb_Products_st] WITH NOCHECK ADD 
	CONSTRAINT [DF_Products_st_DisplayOrder] DEFAULT (0) FOR [DisplayOrder],
	CONSTRAINT [DF_Products_st_SalePrice] DEFAULT (0) FOR [UnitPrice],
	CONSTRAINT [DF_Products_st_FeaturedItem] DEFAULT (0) FOR [FeaturedItem],
	CONSTRAINT [DF_rb_Products_st_TaxRate] DEFAULT (10) FOR [TaxRate],
	CONSTRAINT [DF_rb_Products_st_Weight] DEFAULT (0) FOR [Weight]
GO

ALTER TABLE [rb_ShipZones] ADD 
	CONSTRAINT [FK_rb_ShipZones_rb_Countries] FOREIGN KEY 
	(
		[CountryID]
	) REFERENCES [rb_Countries] (
		[CountryID]
	),
	CONSTRAINT [FK_rb_ShipZones_rb_ShipPrices] FOREIGN KEY 
	(
		[ShipPriceID]
	) REFERENCES [rb_ShipPrices] (
		[ShipPriceID]
	)
GO

ALTER TABLE [rb_Orders] ADD 
	CONSTRAINT [FK_Orders_Users] FOREIGN KEY 
	(
		[UserID]
	) REFERENCES [rb_Users] (
		[UserID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [rb_Products] ADD 
	CONSTRAINT [FK_Products_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_Products_st] ADD 
	CONSTRAINT [FK_Products_st_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_OrderDetails] ADD 
	CONSTRAINT [FK_rb_OrderDetails_rb_Orders] FOREIGN KEY 
	(
		[OrderID]
	) REFERENCES [rb_Orders] (
		[OrderID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProductsPaged    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE     PROCEDURE rb_GetProductsPaged
(
	@ModuleID int,
        	@CategoryID int,
	@Page int = 1,
	@RecordsPerPage int = 10,
	@WorkflowVersion int
)
AS
-- We don't want to return the # of rows inserted
-- into our temporary table, so turn NOCOUNT ON
SET NOCOUNT ON
--Create a temporary table
CREATE TABLE #TempItems
(
	ID		int IDENTITY,
        	ProductID	int,
	DisplayOrder	int,
	ModelNumber	nvarchar(256),
	ModelName	nvarchar(256),
	UnitPrice	money,
	FeaturedItem	bit,
	LongDescription	ntext,
	ShortDescription ntext,
	MetadataXml	ntext,
              Weight		float,
              TaxRate            float
)
IF ( @WorkflowVersion = 1 )
BEGIN
	-- We don't want to return the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON
	-- Insert the rows from tblItems into the temp. table
	
	IF @CategoryID = -1
  	BEGIN
		INSERT INTO
		#TempItems
		(
			ProductID,
			DisplayOrder,
			ModelNumber,
			ModelName,
			UnitPrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml,
                                        Weight,
			TaxRate
		)
		SELECT
		    	rb_Products.ProductID,
			rb_Products.DisplayOrder,
			rb_Products.ModelNumber,
			rb_Products.ModelName,
			rb_Products.UnitPrice,
			rb_Products.FeaturedItem,
			rb_Products.LongDescription,
			rb_Products.ShortDescription,
			rb_Products.MetadataXml,
			rb_Products.Weight,
			rb_Products.TaxRate
		FROM
			rb_Products
		WHERE 
		    rb_Products.FeaturedItem = 1 AND rb_Products.ModuleID = @ModuleID
		ORDER BY 
		    CategoryID, DisplayOrder, ModelName, ModelNumber
	END
	ELSE
	BEGIN
		INSERT INTO
		#TempItems
		(
			ProductID,
			DisplayOrder,
			ModelNumber,
			ModelName,
			UnitPrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml,
                                        Weight,
			TaxRate
		)
		SELECT
		    	rb_Products.ProductID,
			rb_Products.DisplayOrder,
			rb_Products.ModelNumber,
			rb_Products.ModelName,
			rb_Products.UnitPrice,
			rb_Products.FeaturedItem,
			rb_Products.LongDescription,
			rb_Products.ShortDescription,
			rb_Products.MetadataXml,
			rb_Products.Weight,
			rb_Products.TaxRate
	FROM
			rb_Products
		WHERE 
		    rb_Products.CategoryID = @CategoryID AND rb_Products.ModuleID = @ModuleID
		ORDER BY 
		    DisplayOrder, ModelName, ModelNumber
	END
END
ELSE
BEGIN
	-- We don't want to return the # of rows inserted
	-- into our temporary table, so turn NOCOUNT ON
	SET NOCOUNT ON
	-- Insert the rows from tblItems into the temp. table
	
	IF @CategoryID = -1
  	BEGIN
		INSERT INTO
		#TempItems
		(
			ProductID,
			DisplayOrder,
			ModelNumber,
			ModelName,
			UnitPrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml,
                                        Weight,
			TaxRate
		)
		SELECT
		    	rb_Products_st.ProductID,
			rb_Products_st.DisplayOrder,
			rb_Products_st.ModelNumber,
			rb_Products_st.ModelName,
			rb_Products_st.UnitPrice,
			rb_Products_st.FeaturedItem,
			rb_Products_st.LongDescription,
			rb_Products_st.ShortDescription,
			rb_Products_st.MetadataXml,
			rb_Products_st.Weight,
			rb_Products_st.TaxRate
		FROM
			rb_Products_st
		WHERE 
		    rb_Products_st.FeaturedItem = 1 AND rb_Products_st.ModuleID = @ModuleID
		ORDER BY 
		    CategoryID, DisplayOrder, ModelName, ModelNumber
	END
	ELSE
	BEGIN
		INSERT INTO
		#TempItems
		(
			ProductID,
			DisplayOrder,
			ModelNumber,
			ModelName,
			UnitPrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml,
                                        Weight,
			TaxRate
		)
		SELECT
		    	rb_Products_st.ProductID,
			rb_Products_st.DisplayOrder,
			rb_Products_st.ModelNumber,
			rb_Products_st.ModelName,
			rb_Products_st.UnitPrice,
			rb_Products_st.FeaturedItem,
			rb_Products_st.LongDescription,
			rb_Products_st.ShortDescription,
			rb_Products_st.MetadataXml,
			rb_Products_st.Weight,
			rb_Products_st.TaxRate
		FROM
			rb_Products_st
		WHERE 
		    rb_Products_st.CategoryID = @CategoryID AND rb_Products_st.ModuleID = @ModuleID
		ORDER BY 
		    DisplayOrder, ModelName, ModelNumber
	END
END
-- Find out the first AND last record we want
DECLARE @FirstRec int, @LastRec int
SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
SELECT @LastRec = (@Page * @RecordsPerPage + 1)
-- Now, return the set of paged records, plus, an indiciation of we
-- have more records or not!
SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
FROM #TempItems
WHERE ID > @FirstRec AND ID < @LastRec
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartRemoveAllItems    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_CartRemoveAllItems
(
    @CartID nvarchar(50),
    @ModuleID int
)
AS
DELETE FROM rb_Cart
WHERE 
    CartID = @CartID
AND
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateUserFullNoPassword    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_UpdateUserFullNoPassword
(
    @UserID		    int,
    @PortalID       int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @Email		    nvarchar(100),
    @SendNewsletter	bit,
	@CountryID	nchar(2),  
	@StateID		int
)
AS
UPDATE rb_Users
SET
PortalID = @PortalID,
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
Email = @Email,		
SendNewsletter = @SendNewsletter,
CountryID = @CountryID,
StateID = @StateID
WHERE UserID = @UserID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_AddOrder    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_AddOrder
(
	@OrderID 			char(24),
	@ModuleID	 			int,
	@UserID	 			int,
	@TotalGoods	 		money,
	@TotalShipping	 	money,
	@TotalTaxes	 	money,
	@TotalExpenses	 	money,
	@Status	 			int,
	@DateCreated	 	datetime,
	@DateModified		datetime,
	@PaymentMethod 		nvarchar(50),
	@ShippingMethod 	nvarchar(50),
	@TotalWeight	 	real,
	@ShippingData 		ntext,
	@BillingData	 	ntext
)
AS INSERT INTO rb_Orders 
(
	OrderID,
	ModuleID,
	UserID,
	TotalGoods,
	TotalShipping,
	TotalTaxes,
	TotalExpenses,
	Status,
	DateCreated,
	DateModified,
	PaymentMethod,
	ShippingMethod,
	TotalWeight,
	ShippingData,
	BillingData
) 
 
VALUES 
(
	 @OrderID,
	 @ModuleID,
	 @UserID,
	 @TotalGoods,
	 @TotalShipping,
	 @TotalTaxes,
	 @TotalExpenses,
	 @Status,
	 @DateCreated,
	 @DateModified,
	 @PaymentMethod,
	 @ShippingMethod,
	 @TotalWeight,
	 @ShippingData,
	 @BillingData
)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartList    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_CartList
(
    @CartID nvarchar(50),
    @ModuleID int,
    @WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
BEGIN
	SELECT 
	    rb_Products.ProductID, 
	    rb_Products.ModelName,
	    rb_Products.ModelNumber,
	    rb_Cart.Quantity,
	    rb_Products.UnitPrice as UnitCost,
	    Cast((rb_Products.UnitPrice * rb_Cart.Quantity) as money) as ExtendedAmount
	FROM 
	    rb_Products,
	    rb_Cart
	WHERE 
	    rb_Products.ProductID = rb_Cart.ProductID
	  AND 
	    rb_Cart.CartID = @CartID
	  AND
	    rb_Cart.ModuleID = @ModuleID
	ORDER BY 
	    rb_Products.ModelName, 
	    rb_Products.ModelNumber
END
ELSE
BEGIN
	SELECT 
	    rb_Products_st.ProductID, 
	    rb_Products_st.ModelName,
	    rb_Products_st.ModelNumber,
	    rb_Cart.Quantity,
	    rb_Products_st.UnitPrice as UnitCost,
	    Cast((rb_Products_st.UnitPrice * rb_Cart.Quantity) as money) as ExtendedAmount
	FROM 
	    rb_Products_st,
	    rb_Cart
	WHERE 
	    rb_Products_st.ProductID = rb_Cart.ProductID
	  AND 
	    rb_Cart.CartID = @CartID
	  AND
	    rb_Cart.ModuleID = @ModuleID
	ORDER BY 
	    rb_Products_st.ModelName, 
	    rb_Products_st.ModelNumber
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartTotal    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_CartTotal
(
    @CartID    nvarchar(50),
    @ModuleID int,
    @TotalCost money OUTPUT,
    @WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
BEGIN
	SELECT 
	    @TotalCost = SUM(rb_Products.UnitPrice * rb_Cart.Quantity)
	FROM 
	    rb_Cart,
	    rb_Products
	WHERE
	    rb_Cart.CartID = @CartID
	  AND
	    rb_Products.ProductID = rb_Cart.ProductID
	  AND
	    rb_Cart.ModuleID = @ModuleID
END
ELSE
BEGIN
	SELECT 
	    @TotalCost = SUM(rb_Products_st.UnitPrice * rb_Cart.Quantity)
	FROM 
	    rb_Cart,
	    rb_Products_st
	WHERE
	    rb_Cart.CartID = @CartID
	  AND
	    rb_Products_st.ProductID = rb_Cart.ProductID
	  AND
	    rb_Cart.ModuleID = @ModuleID
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartTotalShipping    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_CartTotalShipping
    (
        @CartID         nvarchar(50),
        @CountryID      nchar(2),
        @ModuleID int,
        @WorkflowVersion int,
        @TotalShipping  money OUTPUT
    )
    AS

SELECT @TotalShipping = 
        rb_ShipPrices.Price
        *
        CAST((
        	SELECT SUM(rb_Products_st.Weight * rb_Cart.Quantity) AS TotalWeight 
             FROM rb_Cart INNER JOIN rb_Products_st ON rb_Cart.ProductID = rb_Products_st.ProductID 
	WHERE (rb_Cart.CartID = @CartID AND rb_Cart.ModuleID = @ModuleID)
	) *100 AS int)
        /
       CAST(SUM(rb_ShipPrices.Weight)*100 AS int)
FROM         
	rb_ShipPrices INNER JOIN
             rb_ShipZones ON rb_ShipPrices.ShipPriceID = rb_ShipZones.ShipPriceID
WHERE     
	rb_ShipZones.CountryID = @CountryID

GROUP BY rb_ShipPrices.Weight, rb_ShipPrices.Price
/*
HAVING
(
    CAST((
	SELECT SUM(rb_Products_st.Weight * rb_Cart.Quantity) AS TotalWeight 
	FROM rb_Cart INNER JOIN rb_Products_st ON rb_Cart.ProductID = rb_Products_st.ProductID 
	WHERE (rb_Cart.CartID = @CartID AND rb_Cart.ModuleID = @ModuleID)
	) *100 AS int)
	%
	CAST(SUM(rb_ShipPrices.Weight)*100 AS int)
    = 0
)*/
/* Vengono accettati solo i multipli esatti  (2 cifre decimali - funziona solo con int) */
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateProduct    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROC rb_UpdateProduct
(
    @ProductID           int,
    @ModuleID         int,
    @CategoryID		int,
    @DisplayOrder	int,
    @ModelNumber         nvarchar(256),
    @ModelName         nvarchar(256),
    @UnitPrice	money,
    @FeaturedItem	bit,
    @LongDescription	ntext,
    @ShortDescription	ntext,
    @MetadataXml	ntext,
    @Weight		float,
    @TaxRate                  float
)
AS
IF (@ProductID=0) OR NOT EXISTS (SELECT * FROM rb_Products_st WHERE ProductID = @ProductID)
INSERT INTO rb_Products_st
(
    ModuleID,
    CategoryID,
    DisplayOrder,
    ModelNumber,
    ModelName,
    UnitPrice,
    FeaturedItem,
    LongDescription,
    ShortDescription,
    MetadataXml,
    Weight,
    TaxRate
)
VALUES
(
    @ModuleID,
    @CategoryID,
    @DisplayOrder,
    @ModelNumber,
    @ModelName,
    @UnitPrice,
    @FeaturedItem,
    @LongDescription,
    @ShortDescription,
    @MetadataXml,
    @Weight,
    @TaxRate
)
ELSE
BEGIN
UPDATE
    rb_Products_st
SET
    ModuleID = @ModuleID,
    CategoryID = @CategoryID,
    DisplayOrder = @DisplayOrder,
    ModelNumber = @ModelNumber,
    ModelName = @ModelName,
    UnitPrice = @UnitPrice,
    FeaturedItem = @FeaturedItem,
    LongDescription = @LongDescription,
    ShortDescription = @ShortDescription,
    MetadataXml = @MetadataXml,
    Weight = @Weight,
    TaxRate = @TaxRate
WHERE
    ProductID = @ProductID
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateOrder    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_UpdateOrder
(
	@OrderID 			char(24),
	@UserID	 			int,
	@TotalGoods	 		money,
	@TotalShipping	 	money,
	@TotalTaxes	 	money,
	@TotalExpenses	 	money,
	@Status	 			int,
	@PaymentMethod 		nvarchar(50),
	@ShippingMethod 	nvarchar(50),
	@TotalWeight	 	real,
	@ShippingData 		ntext,
	@BillingData		ntext
)
AS
UPDATE  rb_Orders
SET     UserID = @UserID,
		TotalGoods = @TotalGoods,
		TotalShipping = @TotalShipping,
		TotalTaxes = @TotalTaxes,
		TotalExpenses = @TotalExpenses,
		Status = @Status, 
        DateModified = GetDate(),
        PaymentMethod = @PaymentMethod, 
        ShippingMethod = @ShippingMethod, 
        TotalWeight = @TotalWeight, 
        ShippingData = @ShippingData, 
        BillingData = @BillingData
WHERE   (OrderID = @OrderID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrdersByUser    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_GetOrdersByUser
(
    @ModuleID Int,
    @UserID int
)
AS
SELECT  
        rb_Orders.OrderID,
        rb_Orders.ModuleID, 
        rb_Orders.UserID, 
        rb_Orders.TotalGoods, 
        rb_Orders.TotalShipping, 
        rb_Orders.TotalTaxes, 
        rb_Orders.TotalExpenses, 
        rb_Orders.Status, 
        rb_Orders.DateCreated, 
        rb_Orders.DateModified, 
        rb_Orders.PaymentMethod, 
        rb_Orders.ShippingMethod, 
        rb_Orders.TotalWeight, 
        rb_Orders.ShippingData, 
        rb_Orders.BillingData, 
        rb_Orders.TransactionID, 
        rb_Orders.AuthCode, 
        rb_Users.Name, 
        rb_Users.Company
FROM    rb_Orders LEFT JOIN rb_Users ON rb_Orders.UserID = rb_Users.UserID
WHERE (rb_Orders.ModuleID = @ModuleID) AND (rb_Users.UserID = @UserID)
order by rb_Orders.DateModified desc
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrders    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_GetOrders
(
    @ModuleID int
)
AS
SELECT  
        rb_Orders.OrderID,
        rb_Orders.ModuleID, 
        rb_Orders.UserID, 
        rb_Orders.TotalGoods, 
        rb_Orders.TotalShipping, 
        rb_Orders.TotalTaxes, 
        rb_Orders.TotalExpenses, 
        rb_Orders.Status, 
        rb_Orders.DateCreated, 
        rb_Orders.DateModified, 
        rb_Orders.PaymentMethod, 
        rb_Orders.ShippingMethod, 
        rb_Orders.TotalWeight, 
        rb_Orders.ShippingData, 
        rb_Orders.BillingData, 
        rb_Orders.TransactionID, 
        rb_Orders.AuthCode, 
        rb_Users.Name, 
        rb_Users.Company
FROM    rb_Orders LEFT JOIN
              rb_Users ON rb_Orders.UserID = rb_Users.UserID
WHERE     (rb_Orders.ModuleID = @ModuleID)
order by rb_Orders.DateModified desc
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleOrder    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_GetSingleOrder
(
    @OrderID char(24)
)
AS
SELECT     *
FROM         rb_Orders
WHERE     (OrderID = @OrderID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleProduct    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROC rb_GetSingleProduct
(
    @ProductID    int,
    @WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
BEGIN
	SELECT 
		OriginalProducts.ProductID, 
		(
			SELECT TOP 1
				ProductID
			FROM 
				rb_Products
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Products WHERE ProductID = OriginalProducts.ProductID)
				AND ProductID <> OriginalProducts.ProductID
				AND (DisplayOrder < OriginalProducts.DisplayOrder OR (DisplayOrder = OriginalProducts.DisplayOrder AND ProductID < OriginalProducts.ProductID))
			ORDER BY
				OriginalProducts.DisplayOrder - DisplayOrder, OriginalProducts.ProductID - ProductID
		) AS PreviousProductID,
		(
			SELECT TOP 1
				ProductID
			FROM 
				rb_Products
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Products WHERE ProductID = OriginalProducts.ProductID)
				AND ProductID <> OriginalProducts.ProductID
				AND (DisplayOrder > OriginalProducts.DisplayOrder OR (DisplayOrder = OriginalProducts.DisplayOrder AND ProductID > OriginalProducts.ProductID))
			ORDER BY
				DisplayOrder - OriginalProducts.DisplayOrder , ProductID - OriginalProducts.ProductID 
		) AS NextProductID,
		OriginalProducts.ModuleID,
		OriginalProducts.CategoryID,
		OriginalProducts.DisplayOrder,
		OriginalProducts.ModelNumber,
		OriginalProducts.ModelName,
		OriginalProducts.UnitPrice,
		OriginalProducts.FeaturedItem,
		OriginalProducts.LongDescription,
		OriginalProducts.ShortDescription,
		OriginalProducts.MetadataXml,
		OriginalProducts.Weight,
		OriginalProducts.TaxRate
	FROM 
		rb_Products As OriginalProducts
	WHERE 
		ProductID = @ProductID
END
ELSE
BEGIN
	SELECT 
		OriginalProducts.ProductID, 
		(
			SELECT TOP 1
				ProductID
			FROM 
				rb_Products_st
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Products_st WHERE ProductID = OriginalProducts.ProductID)
				AND ProductID <> OriginalProducts.ProductID
				AND (DisplayOrder < OriginalProducts.DisplayOrder OR (DisplayOrder = OriginalProducts.DisplayOrder AND ProductID < OriginalProducts.ProductID))
			ORDER BY
				OriginalProducts.DisplayOrder - DisplayOrder, OriginalProducts.ProductID - ProductID
		) AS PreviousProductID,
		(
			SELECT TOP 1
				ProductID
			FROM 
				rb_Products_st
			WHERE 
				ModuleID = (SELECT ModuleID FROM rb_Products_st WHERE ProductID = OriginalProducts.ProductID)
				AND ProductID <> OriginalProducts.ProductID
				AND (DisplayOrder > OriginalProducts.DisplayOrder OR (DisplayOrder = OriginalProducts.DisplayOrder AND ProductID > OriginalProducts.ProductID))
			ORDER BY
				DisplayOrder - OriginalProducts.DisplayOrder , ProductID - OriginalProducts.ProductID 
		) AS NextProductID,
		OriginalProducts.ModuleID,
		OriginalProducts.CategoryID,
		OriginalProducts.DisplayOrder,
		OriginalProducts.ModelNumber,
		OriginalProducts.ModelName,
		OriginalProducts.UnitPrice,
		OriginalProducts.FeaturedItem,
		OriginalProducts.LongDescription,
		OriginalProducts.ShortDescription,
		OriginalProducts.MetadataXml,
		OriginalProducts.Weight,
		OriginalProducts.TaxRate
	FROM 
		rb_Products_st As OriginalProducts
	WHERE 
		ProductID = @ProductID
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProducts    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE  PROCEDURE rb_GetProducts
(
	@CategoryID int,
	@ModuleID int,
	@WorkflowVersion int
)
AS
IF ( @WorkflowVersion = 1 )
BEGIN
	
	IF @CategoryID = -1
		SELECT
	                ProductID,
		    DisplayOrder,
		    ModelNumber,
		    ModelName,
		    UnitPrice,
		    FeaturedItem,
		    LongDescription,
		    ShortDescription,
		    MetadataXml,
		    Weight,
		    TaxRate
		FROM 
		    rb_Products
		WHERE 
		    FeaturedItem = 1 AND ModuleID = @ModuleID
		ORDER BY 
		    CategoryID, DisplayOrder, ModelName, ModelNumber
	ELSE
		SELECT 
		    ProductID,
		    DisplayOrder,
		    ModelNumber,
		    ModelName,
		    UnitPrice, 
		    FeaturedItem,
		    LongDescription,
		    ShortDescription,
		    MetadataXml,
		    Weight,
		    TaxRate
		FROM 
		    rb_Products
		WHERE 
		    CategoryID = @CategoryID AND ModuleID = @ModuleID
		ORDER BY 
		    DisplayOrder, ModelName, ModelNumber
END 
ELSE
BEGIN
	IF @CategoryID = -1
		SELECT
	                ProductID,
		    DisplayOrder,
		    ModelNumber,
		    ModelName,
		    UnitPrice,
		    FeaturedItem,
		    LongDescription,
		    ShortDescription,
		    MetadataXml,
		    Weight,
		    TaxRate
		FROM 
		    rb_Products_st
		WHERE 
		    FeaturedItem = 1 AND ModuleID = @ModuleID
		ORDER BY 
		    CategoryID, DisplayOrder, ModelName, ModelNumber
	ELSE
		SELECT 
		    ProductID,
		    DisplayOrder,
		    ModelNumber,
		    ModelName,
		    UnitPrice,
		    FeaturedItem,
		    LongDescription,
		    ShortDescription,
		    MetadataXml,
		    Weight,
		    TaxRate
		FROM 
		    rb_Products_st
		WHERE 
		    CategoryID = @CategoryID AND ModuleID = @ModuleID
		ORDER BY 
		    DisplayOrder, ModelName, ModelNumber
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_AddOrderDetails    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_AddOrderDetails 
	@OrderID char(24),
	@CartID nvarchar(50),
	@ModuleID int
AS
/* Copy items from given shopping cart to OrdersDetail table for given OrderID*/
INSERT INTO rb_OrderDetails
(
    OrderID, 
    ProductID, 
    Quantity, 
    ModelName,
    ModelNumber,
    UnitPrice
)
SELECT 
    @OrderID, 
    rb_Cart.ProductID, 
    Quantity, 
    rb_Products_st.ModelName,
    rb_Products_st.ModelNumber,
    rb_Products_st.UnitPrice
FROM 
    rb_Cart 
  INNER JOIN rb_Products_st ON rb_Cart.ProductID = rb_Products_st.ProductID
  
WHERE 
    CartID = @CartID AND rb_Cart.ModuleID = @ModuleID

/* Removal of  items from user's shopping cart */
exec rb_CartRemoveAllItems @CartID, @ModuleID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetOrderDetails    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE PROCEDURE rb_GetOrderDetails
(
    @OrderID    char(24)
)
AS

/* Then, return the recordset of info */
SELECT  
    ProductID, 
    ModelName,
    ModelNumber,
    UnitPrice,
    Quantity

FROM
    rb_OrderDetails
  
WHERE   
    OrderID = @OrderID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Trigger dbo.rb_Products_stModified    Script Date: 5/5/2003 7:08:24 PM ******/
CREATE TRIGGER [rb_Products_stModified]
ON [rb_Products_st]
FOR DELETE, INSERT, UPDATE 
AS 
BEGIN
DECLARE ChangedModules CURSOR FOR
SELECT ModuleID
FROM inserted
UNION
SELECT ModuleID
FROM deleted
 
DECLARE @ModID     int
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1705','1.2.8.1705', CONVERT(datetime, '05/05/2003', 101))
GO

