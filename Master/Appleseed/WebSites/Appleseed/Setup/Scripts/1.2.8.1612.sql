---------------------
--1.2.8.1612.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Products_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Products] DROP CONSTRAINT FK_Products_Modules
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Orders_Users]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Orders] DROP CONSTRAINT FK_Orders_Users
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_OrderDetails_Orders]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_OrderDetails] DROP CONSTRAINT FK_OrderDetails_Orders
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrderDetails]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleOrderDetails]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrder]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleOrderAccount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleOrderAccount]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleOrderShipping]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleOrderShipping]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetUserForOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetUserForOrder]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProducts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProducts]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleProduct]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ProductsChangeCategory]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ProductsChangeCategory]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateProduct]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateUserFullNoPassword]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateUserFullNoPassword]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartAddItem]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartAddItem]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartEmpty]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartEmpty]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartItemCount]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartItemCount]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartList]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartMigrate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartMigrate]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartRemoveAbandoned]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartRemoveAbandoned]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartRemoveAllItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartRemoveAllItems]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartRemoveItem]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartRemoveItem]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartTotal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartTotal]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartUpdate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartUpdate]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteProduct]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProductsCategoryList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProductsCategoryList]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProductsPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProductsPaged]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_OrderDetails]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_OrderDetails]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Orders]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Orders]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Products]
GO

CREATE TABLE [rb_Cart] (
	[RecordID] [int] IDENTITY (1, 1) NOT NULL ,
	[CartID] [nvarchar] (50) NULL ,
	[Quantity] [int] NOT NULL ,
	[ProductID] [int] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[ModuleID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [rb_Orders] (
	[OrderID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserID] [int] NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[OrderDate] [datetime] NOT NULL ,
	[ShipDate] [datetime] NULL ,
	[Enquiries] [ntext] NULL ,
	[Delivered] [bit] NOT NULL ,
	[Valid] [bit] NOT NULL ,
	[Name] [nvarchar] (50) NOT NULL ,
	[Company] [nvarchar] (50) NOT NULL ,
	[Address] [nvarchar] (50) NOT NULL ,
	[City] [nvarchar] (50) NOT NULL ,
	[Zip] [nvarchar] (6) NOT NULL ,
	[CountryID] [nchar] (2) NOT NULL ,
	[StateID] [int] NOT NULL ,
	[Phone] [nvarchar] (50) NOT NULL ,
	[Fax] [nvarchar] (50) NOT NULL ,
	[SName] [nvarchar] (50) NOT NULL ,
	[SCompany] [nvarchar] (50) NOT NULL ,
	[SAddress] [nvarchar] (50) NOT NULL ,
	[SCity] [nvarchar] (50) NOT NULL ,
	[SZip] [nvarchar] (6) NOT NULL ,
	[SCountryID] [nchar] (2) NOT NULL ,
	[SStateID] [int] NOT NULL ,
	[SPhone] [nvarchar] (50) NOT NULL ,
	[SFax] [nvarchar] (50) NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [rb_Products] (
	[ProductID] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleID] [int] NOT NULL ,
	[CategoryID] [int] NOT NULL ,
	[DisplayOrder] [int] NOT NULL ,
	[ModelNumber] [nvarchar] (256) NULL ,
	[ModelName] [nvarchar] (256) NULL ,
	[RetailPrice] [money] NOT NULL ,
	[SalePrice] [money] NOT NULL ,
	[FeaturedItem] [bit] NOT NULL ,
	[LongDescription] [ntext] NULL ,
	[ShortDescription] [ntext] NULL ,
	[MetadataXml] [ntext] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [rb_OrderDetails] (
	[OrderID] [int] NOT NULL ,
	[ProductID] [int] NOT NULL ,
	[Quantity] [int] NOT NULL ,
	[ModelName] [nvarchar] (256) NOT NULL ,
	[ModelNumber] [nvarchar] (256) NOT NULL ,
	[SalePrice] [money] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [rb_Cart] WITH NOCHECK ADD 
	CONSTRAINT [DF_Cart_Quantity] DEFAULT (1) FOR [Quantity],
	CONSTRAINT [DF_Cart_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [PK_Cart] PRIMARY KEY  NONCLUSTERED 
	(
		[RecordID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Orders] WITH NOCHECK ADD 
	CONSTRAINT [DF_Orders_Delivered] DEFAULT (0) FOR [Delivered],
	CONSTRAINT [DF_Orders_Valid] DEFAULT (0) FOR [Valid],
	CONSTRAINT [PK_Orders] PRIMARY KEY  CLUSTERED 
	(
		[OrderID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Products] WITH NOCHECK ADD 
	CONSTRAINT [DF_Products_DisplayOrder] DEFAULT (0) FOR [DisplayOrder],
	CONSTRAINT [DF_Products_RetailPrice] DEFAULT (0) FOR [RetailPrice],
	CONSTRAINT [DF_Products_SalePrice] DEFAULT (0) FOR [SalePrice],
	CONSTRAINT [DF_Products_FeaturedItem] DEFAULT (0) FOR [FeaturedItem],
	CONSTRAINT [PK_Products] PRIMARY KEY  CLUSTERED 
	(
		[ProductID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Orders] ADD 
	CONSTRAINT [FK_Orders_Users] FOREIGN KEY 
	(
		[UserID]
	) REFERENCES [rb_Users] (
		[UserID]
	)
GO

ALTER TABLE [rb_Products] ADD 
	CONSTRAINT [FK_Products_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

ALTER TABLE [rb_OrderDetails] ADD 
	CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY 
	(
		[OrderID]
	) REFERENCES [rb_Orders] (
		[OrderID]
	)
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_DeleteProduct
(
    @ProductID int
)
AS
DELETE FROM
    rb_Products
WHERE
    ProductID = @ProductID

GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE   PROCEDURE rb_GetProductsCategoryList
(
@ModuleID int
)
AS
--Create a Temporary table to hold the tabs for this query
CREATE TABLE #TabTree
(
        [TabID] [int],
        [TabName] [nvarchar] (50),
        [ParentTabID] [int],
        [TabOrder] [int],
        [NestLevel] [int],
        [TreeOrder] [varchar] (1000)
)
SET NOCOUNT ON  -- Turn off echo of "... row(s) affected"
DECLARE @LastLevel smallint
SET @LastLevel = 0
-- First, the parent levels: just the TabID for the Shop Module
INSERT INTO     #TabTree
SELECT  rb_Tabs.TabID,
        rb_Tabs.TabName,
        rb_Tabs.ParentTabID,
        rb_Tabs.TabOrder,
        0,
        cast(100000000 + rb_Tabs.TabOrder as varchar)
FROM         rb_Modules INNER JOIN rb_Tabs ON rb_Modules.TabID = rb_Tabs.TabID
WHERE   ModuleID = @ModuleID
-- Next, the children levels
WHILE (@@rowcount > 0)
BEGIN
  SET @LastLevel = @LastLevel + 1
  INSERT        #TabTree (TabID, TabName, ParentTabID, TabOrder, NestLevel, TreeOrder) 
                SELECT  rb_Tabs.TabID,
                        Replicate('-', (@LastLevel-1) *2) + rb_Tabs.TabName,
                        rb_Tabs.ParentTabID,
                        rb_Tabs.TabOrder,
                        @LastLevel,
                        cast(#TabTree.TreeOrder as varchar) + '.' + cast(100000000 + rb_Tabs.TabOrder as varchar)
                FROM    rb_Tabs join #TabTree on rb_Tabs.ParentTabID= #TabTree.TabID
                WHERE   EXISTS (SELECT 'X' FROM #TabTree WHERE TabID = rb_Tabs.ParentTabID AND NestLevel = @LastLevel - 1)
                ORDER BY #TabTree.TabOrder
END
-- Reorder the tabs by using a 2nd Temp table AND an identity field to keep them straight.
-- Tiptopweb: Remove the first level as it is the TabID for the Shop
select IDENTITY(int,1,2) as ord , cast(TabID as varchar) as TabID into #tabs
from #TabTree
order by nestlevel, TreeOrder
-- Change the TabOrder in the sirt temp table so that tabs are ordered in sequence
update #TabTree set TabOrder=(select ord from #tabs WHERE cast(#tabs.TabID as int)=#TabTree.TabID) 
-- Return Temporary Table
SELECT TabID as categoryID, tabname as categoryName
FROM #TabTree 
WHERE nestlevel <> 0
order by TreeOrder
GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS OFF 
GO
CREATE     PROCEDURE rb_GetProductsPaged
(
	@ModuleID int,
        @CategoryID int,
	@Page int = 1,
	@RecordsPerPage int = 10
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
	RetailPrice	money,
	SalePrice	money,
	FeaturedItem	bit,
	LongDescription	ntext,
	ShortDescription ntext,
	MetadataXml	ntext
)

-- Insert the rows from tblItems into the temp. table
IF @CategoryID = -1

	INSERT INTO
	#TempItems
	(
		ProductID,
		DisplayOrder,
		ModelNumber,
		ModelName,
		RetailPrice,
		SalePrice,
		FeaturedItem,
		LongDescription,
		ShortDescription,
		MetadataXml
	)
	SELECT
	    	rb_Products.ProductID,
		rb_Products.DisplayOrder,
		rb_Products.ModelNumber,
		rb_Products.ModelName,
		rb_Products.RetailPrice,
		rb_Products.SalePrice,
		rb_Products.FeaturedItem,
		rb_Products.LongDescription,
		rb_Products.ShortDescription,
		rb_Products.MetadataXml
	FROM
		rb_Products
	WHERE 
	    rb_Products.FeaturedItem = 1 AND rb_Products.ModuleID = @ModuleID
	ORDER BY 
	    CategoryID, DisplayOrder, ModelName, ModelNumber

ELSE

	INSERT INTO
	#TempItems
	(
		ProductID,
		DisplayOrder,
		ModelNumber,
		ModelName,
		RetailPrice,
		SalePrice,
		FeaturedItem,
		LongDescription,
		ShortDescription,
		MetadataXml
	)
	SELECT
	    	rb_Products.ProductID,
		rb_Products.DisplayOrder,
		rb_Products.ModelNumber,
		rb_Products.ModelName,
		rb_Products.RetailPrice,
		rb_Products.SalePrice,
		rb_Products.FeaturedItem,
		rb_Products.LongDescription,
		rb_Products.ShortDescription,
		rb_Products.MetadataXml
	FROM
		rb_Products
	WHERE 
	    rb_Products.CategoryID = @CategoryID AND rb_Products.ModuleID = @ModuleID
	ORDER BY 
	    DisplayOrder, ModelName, ModelNumber

-- Find out the first AND last record we want
DECLARE @FirstRec int, @LastRec int
SELECT @FirstRec = (@Page - 1) * @RecordsPerPage
SELECT @LastRec = (@Page * @RecordsPerPage + 1)

-- Now, return the set of paged records, plus, an indication of we
-- have more records or not!
SELECT *, (SELECT COUNT(*) FROM #TempItems) RecordCount
FROM #TempItems
WHERE ID > @FirstRec AND ID < @LastRec
-- Turn NOCOUNT back OFF
SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartAddItem
(
    @CartID nvarchar(50),
    @ProductID int,
    @Quantity int,
    @ModuleID int
)
As
DECLARE @CountItems int
SELECT
    @CountItems = Count(ProductID)
FROM
    rb_Cart
WHERE
    ProductID = @ProductID
  AND
    CartID = @CartID
  AND
    ModuleID = @ModuleID
IF @CountItems > 0  /* There are items - update the current quantity */
    UPDATE
        rb_Cart
    SET
        Quantity = (@Quantity + rb_Cart.Quantity)
    WHERE
        ProductID = @ProductID
      AND
        CartID = @CartID
      AND
        ModuleID = @ModuleID
ELSE  /* New entry for this Cart.  Add a new record */
    INSERT INTO rb_Cart
    (
        CartID,
        Quantity,
        ProductID,
        ModuleID
    )
    VALUES
    (
        @CartID,
        @Quantity,
        @ProductID,
        @ModuleID
    )

GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartEmpty
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

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON
GO
CREATE PROCEDURE rb_CartItemCount
(
    @CartID    nvarchar(50),
    @ModuleID int,
    @ItemCount int OUTPUT
)
AS
SELECT 
    @ItemCount = COUNT(ProductID)
    
FROM 
    rb_Cart
    
WHERE 
    CartID = @CartID
AND
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartList
(
    @CartID nvarchar(50),
    @ModuleID int
)
AS
SELECT 
    rb_Products.ProductID, 
    rb_Products.ModelName,
    rb_Products.ModelNumber,
    rb_Cart.Quantity,
    rb_Products.SalePrice as UnitCost,
    Cast((rb_Products.SalePrice * rb_Cart.Quantity) as money) as ExtendedAmount
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

GO

SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartMigrate
(
    @OriginalCartID nvarchar(50),
    @NewCartID      nvarchar(50)
)
AS
DECLARE @itemCount int
/* check if items (for any module) in original cart */
SELECT 
    @ItemCount = COUNT(ProductID)
FROM 
    rb_Cart
WHERE 
    CartID = @OriginalCartID
/* if the original cart is not empty, clear the items already present in the destination cart */
IF (@ItemCount > 0)
BEGIN
DELETE FROM rb_Cart
   WHERE 
   CartID = @NewCartID
END
/* migrate the cart */
UPDATE 
    rb_Cart
SET 
    CartID = @NewCartID 
WHERE 
    CartID = @OriginalCartID

GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartRemoveAbandoned
AS
DELETE FROM rb_Cart
WHERE 
    DATEDIFF(dd, DateCreated, GetDate()) > 1

GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS OFF 
GO
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

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartRemoveItem
(
    @CartID nvarchar(50),
    @ProductID int,
    @ModuleID int
)
AS
DELETE FROM rb_Cart
WHERE 
    CartID = @CartID
  AND
    ProductID = @ProductID
  AND
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartTotal
(
    @CartID    nvarchar(50),
    @ModuleID int,
    @TotalCost money OUTPUT
)
AS
SELECT 
    @TotalCost = SUM(rb_Products.SalePrice * rb_Cart.Quantity)
FROM 
    rb_Cart,
    rb_Products
WHERE
    rb_Cart.CartID = @CartID
  AND
    rb_Products.ProductID = rb_Cart.ProductID
  AND
    rb_Cart.ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE rb_CartUpdate
(
    @CartID    nvarchar(50),
    @ProductID int,
    @Quantity  int,
    @ModuleID int
)
AS
UPDATE rb_Cart
SET 
    Quantity = @Quantity
WHERE 
    CartID = @CartID 
  AND 
    ProductID = @ProductID
  AND
    ModuleID = @ModuleID

GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_UpdateUserFullNoPassword
(
    @UserID		    int,
    @Name		    nvarchar(50),
    @Company	    nvarchar(50),
    @Address		nvarchar(50),
    @City		    nvarchar(50),
    @Zip		    nvarchar(6),
    @Phone		    nvarchar(50),
    @Fax		    nvarchar(50),
    @PIva		    nvarchar(11),
    @CFiscale	    nvarchar(16),
    @SendNewsletter	bit,
	@CountryID	nchar(2),  
	@StateID		int
)
AS
UPDATE rb_Users
SET
Name = @Name,
Company = @Company,
Address = @Address,		
City = @City,		
Zip = @Zip,		
Phone = @Phone,		
Fax = @Fax,		
PIva = @PIva,		
CFiscale = @CFiscale,	
SendNewsletter = @SendNewsletter,
CountryID = @CountryID,
StateID = @StateID
WHERE UserID = @UserID
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_AddOrder
(
    @UserID	    	int,
    @ModuleID	    	int,
    @Name	    	nvarchar(50),
    @Company	    	nvarchar(50),
    @Address	    	nvarchar(50),
    @City	    	nvarchar(50),
    @Zip		    	nvarchar(6),
    @Phone	    	nvarchar(50),
    @Fax	    	nvarchar(50),
    @CountryID    	nchar(2),
    @StateID		int,
    @SName	    	nvarchar(50),
    @SCompany	   	nvarchar(50),
    @SAddress	    	nvarchar(50),
    @SCity	    	nvarchar(50),
    @SZip		nvarchar(6),
    @SPhone	    	nvarchar(50),
    @SFax	    	nvarchar(50),
    @SCountryID		nchar(2),
    @SStateID		int,
    @OrderID		int OUTPUT
)
AS
INSERT INTO rb_Orders
(
    	ModuleID,
	UserID,
    	Name,
    	Company,
	Address,		
	City,		
	Zip,		
	Phone,		
	Fax,		
	CountryID,
	StateID,
    	SName,
    	SCompany,
	SAddress,		
	SCity,		
	SZip,		
	SPhone,		
	SFax,		
	SCountryID,
	SStateID,
	OrderDate,
	ShipDate
)
VALUES
(
    	@ModuleID,
	@UserID,
    	@Name,
    	@Company,
	@Address,	
	@City,	
	@Zip,	
	@Phone,	
	@Fax,	
	@CountryID,
	@StateID,
    	@SName,
    	@SCompany,
	@SAddress,	
	@SCity,	
	@SZip,	
	@SPhone,	
	@SFax,	
	@SCountryID,
	@SStateID,
	GetDate(),
	GetDate()
)
SELECT
    @OrderID = @@IDENTITY

GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_GetSingleOrderAccount
    	@OrderID int,
	@IDLang nchar(2) = 'EN'
AS
SELECT
	rb_Orders.Name,
	rb_Orders.Company,
	rb_Orders.Address,
	rb_Orders.City,
	rb_Orders.Zip,
	rb_Orders.Phone,
	rb_Orders.Fax,
	rb_States.Description AS State
FROM 
	rb_Orders, rb_States
WHERE
	rb_Orders.OrderID = @OrderID AND rb_Orders.StateID = rb_States.StateID

GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_GetSingleOrderShipping
    	@OrderID int,
	@IDLang nchar(2) = 'EN'
AS
SELECT
	rb_Orders.SName AS Name,
	rb_Orders.SCompany AS Company,
	rb_Orders.SAddress AS Address,
	rb_Orders.SCity AS City,
	rb_Orders.SZip AS Zip,
	rb_Orders.SPhone AS Phone,
	rb_Orders.SFax AS Fax,
	rb_States.Description AS State
FROM 
	rb_Orders, rb_States
WHERE
	rb_Orders.OrderID = @OrderID AND rb_Orders.StateID = rb_States.StateID

GO
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_GetUserForOrder
(
    @OrderID	    	int,
    @UserID		int OUTPUT
)
AS
SELECT @UserID = UserID 
FROM rb_Orders
WHERE OrderID = @OrderID

GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO
CREATE  PROCEDURE rb_GetProducts
(
    @CategoryID int,
    @ModuleID int
)
AS
IF @CategoryID = -1
	SELECT
                ProductID,
	    DisplayOrder,
	    ModelNumber,
	    ModelName,
	    RetailPrice,
	    SalePrice,
	    FeaturedItem,
	    LongDescription,
	    ShortDescription,
	    MetadataXml
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
	    RetailPrice, 
	    SalePrice,
	    FeaturedItem,
	    LongDescription,
	    ShortDescription,
	    MetadataXml
	FROM 
	    rb_Products
	WHERE 
	    CategoryID = @CategoryID AND ModuleID = @ModuleID
	ORDER BY 
	    DisplayOrder, ModelName, ModelNumber
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROC rb_GetSingleProduct
(
    @ProductID    int
)
AS
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
	OriginalProducts.RetailPrice,
	OriginalProducts.SalePrice,
	OriginalProducts.FeaturedItem,
	OriginalProducts.LongDescription,
	OriginalProducts.ShortDescription,
	OriginalProducts.MetadataXml
FROM 
	rb_Products As OriginalProducts
WHERE 
	ProductID = @ProductID
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROC rb_ProductsChangeCategory
(
	@CategoryID int,
	@NewCategoryID int
)
AS
UPDATE rb_Products
SET
CategoryID = @NewCategoryID
WHERE
CategoryID = @CategoryID
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO
CREATE PROC rb_UpdateProduct
(
    @ProductID           int,
    @ModuleID         int,
    @CategoryID		int,
    @DisplayOrder	int,
    @ModelNumber         nvarchar(256),
    @ModelName         nvarchar(256),
    @RetailPrice	money,
    @SalePrice	money,
    @FeaturedItem	bit,
    @LongDescription	ntext,
    @ShortDescription	ntext,
    @MetadataXml	ntext
)
AS
IF (@ProductID=0) OR NOT EXISTS (SELECT * FROM rb_Products WHERE ProductID = @ProductID)
INSERT INTO rb_Products
(
    ModuleID,
    CategoryID,
    DisplayOrder,
    ModelNumber,
    ModelName,
    RetailPrice,
    SalePrice,
    FeaturedItem,
    LongDescription,
    ShortDescription,
    MetadataXml
)
VALUES
(
    @ModuleID,
    @CategoryID,
    @DisplayOrder,
    @ModelNumber,
    @ModelName,
    @RetailPrice,
    @SalePrice,
    @FeaturedItem,
    @LongDescription,
    @ShortDescription,
    @MetadataXml
)
ELSE
BEGIN
UPDATE
    rb_Products
SET
    ModuleID = @ModuleID,
    CategoryID = @CategoryID,
    DisplayOrder = @DisplayOrder,
    ModelNumber = @ModelNumber,
    ModelName = @ModelName,
    RetailPrice = @RetailPrice,
    SalePrice = @SalePrice,
    FeaturedItem = @FeaturedItem,
    LongDescription = @LongDescription,
    ShortDescription = @ShortDescription,
    MetadataXml = @MetadataXml
WHERE
    ProductID = @ProductID
END
GO

SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_AddOrderDetails 
	@OrderID int,
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
    SalePrice
)
SELECT 
    @OrderID, 
    rb_Cart.ProductID, 
    Quantity, 
    rb_Products.ModelName,
    rb_Products.ModelNumber,
    rb_Products.SalePrice
FROM 
    rb_Cart 
  INNER JOIN rb_Products ON rb_Cart.ProductID = rb_Products.ProductID
  
WHERE 
    CartID = @CartID AND rb_Cart.ModuleID = @ModuleID
/* Removal of  items from user's shopping cart */
/* Do not clear the cart at this stage as the user may navigate back */
/*exec CartRemoveAllItems @CartID, @ModuleID */
GO

SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE rb_GetSingleOrderDetails
(
    @OrderID    int,
    @OrderTotal money OUTPUT,
    @OrderDate  datetime OUTPUT,
    @ShipDate   datetime OUTPUT,
    @Delivered bit OUTPUT,
    @Valid bit OUTPUT
)
AS
/* return the OrderTotal */
SELECT  
    @OrderTotal = Cast(SUM(rb_OrderDetails.Quantity * rb_OrderDetails.SalePrice) as money)
    
FROM    
    rb_OrderDetails
    
WHERE   
    OrderID= @OrderID
/* return the dates */
SELECT 
    @OrderDate = OrderDate,
    @ShipDate = ShipDate,
    @Delivered = Delivered,
    @Valid = Valid
    
FROM    
    rb_Orders
    
WHERE   
    OrderID = @OrderID
/* Then, return the recordset of info */
SELECT  
    rb_OrderDetails.ProductID, 
    rb_OrderDetails.ModelName,
    rb_OrderDetails.ModelNumber,
    rb_OrderDetails.SalePrice,
    rb_OrderDetails.Quantity,
    (rb_OrderDetails.Quantity * rb_OrderDetails.SalePrice) as ExtendedAmount
FROM
    rb_OrderDetails
WHERE   
    OrderID = @OrderID
GO
SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON 
GO

/* Add the new module */
INSERT INTO rb_GeneralModuleDefinitions
(
	GeneralModDefID, FriendlyName, DesktopSrc, MobileSrc, AssemblyName, ClassName, Admin, Searchable
)
VALUES
(
	'{EC24FABD-FB16-4978-8C81-1ADD39792377}','Products','DesktopModules/Products.ascx','','Appleseed.DLL','Appleseed.Content.Web.ModulesProducts',0,0
)
GO

INSERT INTO rb_ModuleDefinitions
(
	GeneralModDefID, PortalID
)
VALUES
(
	'{EC24FABD-FB16-4978-8C81-1ADD39792377}',0
)
GO

/* add version info */
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1612','1.2.8.1612', CONVERT(datetime, '04/12/2003', 101))
GO

