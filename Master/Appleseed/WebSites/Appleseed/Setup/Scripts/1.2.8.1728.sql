/* Update script Ecommerce, by Manu [manu-dea@hotmail dot it] */

SET nocount ON
UPDATE rb_GeneralModuleDefinitions
SET DesktopSrc = 'ECommerce/DesktopModules/Products.ascx'
WHERE GeneralModDefID ='{EC24FABD-FB16-4978-8C81-1ADD39792377}'
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_EcommerceMerchants]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [rb_EcommerceMerchants] (
	[MerchantID] [nvarchar] (25) ,
	[GatewayName] [nvarchar] (50)  ,
	[Name] [nvarchar] (50)  ,
	[MerchantEmail] [nvarchar] (50)  ,
	[TechnicalEmail] [nvarchar] (50)  ,
	[MetadataXml] [nvarchar] (3000)  
) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_EcommerceGetMerchant]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_EcommerceGetMerchant]
GO

CREATE PROCEDURE rb_EcommerceGetMerchant
(
	@MerchantID nvarchar(25)
)
AS

SELECT     *
FROM         rb_EcommerceMerchants
WHERE     (MerchantID = @MerchantID)
GO

IF NOT EXISTS (SELECT name FROM syscolumns WHERE id = OBJECT_ID('rb_Orders') AND COLUMNPROPERTY(OBJECT_ID('rb_Orders'), 'ISOCurrencySymbol', 'AllowsNull') = 1)
BEGIN
ALTER TABLE [rb_Orders] ADD
	[ISOCurrencySymbol] [char] (3) NULL
PRINT 'Column ISOCurrencySymbol created'
END
GO

IF NOT EXISTS (SELECT name FROM syscolumns WHERE id = OBJECT_ID('rb_Orders') AND COLUMNPROPERTY(OBJECT_ID('rb_Orders'), 'WeightUnit', 'AllowsNull') = 1)
BEGIN
ALTER TABLE [rb_Orders] ADD
	[WeightUnit] [nvarchar] (15) NULL
PRINT 'Column WeightUnit created'
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrder]
GO

CREATE PROCEDURE rb_AddOrder
(
	@OrderID 		char(24),
	@ModuleID	 	int,
	@UserID	 		int,
	@TotalGoods	 	money,
	@TotalShipping	 	money,
	@TotalTaxes	 	money,
	@TotalExpenses	 	money,
	@ISOCurrencySymbol	char(3),
	@Status	 		int,
	@DateCreated	 	datetime,
	@DateModified		datetime,
	@PaymentMethod 		nvarchar(50),
	@ShippingMethod 	nvarchar(50),
	@TotalWeight	 	real,
	@WeightUnit		nvarchar(15),
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
	ISOCurrencySymbol,
	Status,
	DateCreated,
	DateModified,
	PaymentMethod,
	ShippingMethod,
	TotalWeight,
	WeightUnit,
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
	 @ISOCurrencySymbol,
	 @Status,
	 @DateCreated,
	 @DateModified,
	 @PaymentMethod,
	 @ShippingMethod,
	 @TotalWeight,
	 @WeightUnit,
	 @ShippingData,
	 @BillingData
)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateOrder]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateOrder]
GO

CREATE PROCEDURE rb_UpdateOrder
(
	@OrderID 		char(24),
	@UserID	 		int,
	@TotalGoods	 	money,
	@TotalShipping	 	money,
	@TotalTaxes	 	money,
	@TotalExpenses	 	money,
	@ISOCurrencySymbol	char(3),
	@Status	 		int,
	@PaymentMethod 		nvarchar(50),
	@ShippingMethod 	nvarchar(50),
	@TotalWeight	 	real,
	@WeightUnit		nvarchar(15),
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
	ISOCurrencySymbol = @ISOCurrencySymbol,
	Status = @Status, 
        DateModified = GetDate(),
        PaymentMethod = @PaymentMethod, 
        ShippingMethod = @ShippingMethod, 
        TotalWeight = @TotalWeight,
	WeightUnit = @WeightUnit, 
        ShippingData = @ShippingData, 
        BillingData = @BillingData
WHERE   (OrderID = @OrderID)
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartList]
GO

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
	    Cast((rb_Products.UnitPrice * rb_Cart.Quantity) as money) as ExtendedAmount,
	    (SELECT SettingValue FROM rb_ModuleSettings WHERE ModuleID = @ModuleID AND SettingName = 'Currency') as ISOCurrencySymbol,
	    rb_Products.Weight
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
	    Cast((rb_Products_st.UnitPrice * rb_Cart.Quantity) as money) as ExtendedAmount,
	    (SELECT SettingValue FROM rb_ModuleSettings WHERE ModuleID = @ModuleID AND SettingName = 'Currency') as ISOCurrencySymbol,
	    rb_Products_st.Weight
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

IF NOT EXISTS (SELECT name FROM syscolumns WHERE id = OBJECT_ID('rb_OrderDetails') AND COLUMNPROPERTY(OBJECT_ID('rb_OrderDetails'), 'Weight', 'AllowsNull') = 0)
BEGIN
ALTER TABLE [rb_OrderDetails] ADD
	[Weight] [real] NOT NULL default 0
PRINT 'Column Weight created'
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetOrderDetails]
GO

CREATE PROCEDURE rb_GetOrderDetails
(
    @OrderID    char(24)
)
AS

-- Then, return the recordset of info
SELECT     rb_OrderDetails.ProductID, rb_OrderDetails.ModelName, rb_OrderDetails.ModelNumber, rb_OrderDetails.UnitPrice, rb_OrderDetails.Quantity, 
                      rb_Orders.ISOCurrencySymbol, rb_OrderDetails.Weight
FROM         rb_OrderDetails INNER JOIN
                      rb_Orders ON rb_OrderDetails.OrderID = rb_Orders.OrderID
WHERE     (rb_OrderDetails.OrderID = @OrderID)
GO


IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_AddOrderDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_AddOrderDetails]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE rb_AddOrderDetails 
	@OrderID char(24),
	@CartID nvarchar(50),
	@ModuleID int
AS
-- Copy items from given shopping cart to OrdersDetail table for given OrderID
INSERT INTO rb_OrderDetails
(
    OrderID, 
    ProductID, 
    Quantity, 
    ModelName,
    ModelNumber,
    UnitPrice,
    Weight
)
SELECT 
    @OrderID, 
    rb_Cart.ProductID, 
    Quantity, 
    rb_Products_st.ModelName,
    rb_Products_st.ModelNumber,
    rb_Products_st.UnitPrice,
    rb_Products_st.Weight
FROM 
    rb_Cart 
  INNER JOIN rb_Products_st ON rb_Cart.ProductID = rb_Products_st.ProductID
  
WHERE 
    CartID = @CartID AND rb_Cart.ModuleID = @ModuleID

/* Removal of  items from user's shopping cart */
exec rb_CartRemoveAllItems @CartID, @ModuleID
GO

--ADD A DEMO ROW ON rb_EcommerceMerchants
IF NOT EXISTS (SELECT MerchantID FROM rb_EcommerceMerchants WHERE MerchantID = '001')
INSERT 
INTO rb_EcommerceMerchants ([MerchantID],[GatewayName],[Name], [MerchantEmail], [TechnicalEmail], [MetadataXml]) 
VALUES(N'001',	N'CreditTransfer', N'Credit Transfer DEMO', N'testemail@testsite.com',	N'testemail@testsite.com', N'<Metadata CreditInstitute="MyBank" BankCode="11222" FaxNumber="0005555-000000"/>')
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1728','1.2.8.1728', CONVERT(datetime, '07/10/2003', 101))
GO

