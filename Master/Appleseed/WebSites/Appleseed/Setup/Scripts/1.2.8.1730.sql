/* Update script Ecommerce - 1730 fixes, by Manu [manu-dea@hotmail dot it] */

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

--ADD A DEMO ROW ON rb_EcommerceMerchants for Gateway
IF NOT EXISTS (SELECT MerchantID FROM rb_EcommerceMerchants WHERE MerchantID = '001')
INSERT 
INTO rb_EcommerceMerchants ([MerchantID],[GatewayName],[Name], [MerchantEmail], [TechnicalEmail], [MetadataXml]) 
VALUES(N'001',	N'CreditTransfer', N'Credit Transfer DEMO', N'testemail@testsite.com',	N'testemail@testsite.com', N'<Metadata CreditInstitute="MyBank" BankCode="11222" FaxNumber="0005555-000000"/>')

--ADD A DEMO ROW ON rb_EcommerceMerchants for Shipping
IF NOT EXISTS (SELECT MerchantID FROM rb_EcommerceMerchants WHERE MerchantID = '100')
INSERT 
INTO rb_EcommerceMerchants ([MerchantID],[GatewayName],[Name], [MerchantEmail], [TechnicalEmail], [MetadataXml]) 
VALUES(N'100',	N'Shipping', N'Shipping DEMO', N'testemail@testsite.com', N'testemail@testsite.com', N'<Metadata Percentage="10" MaxValue="€ 400" MinValue="€ 10" />')

IF NOT EXISTS (SELECT MerchantID FROM rb_EcommerceMerchants WHERE MerchantID = '200')
INSERT
INTO rb_EcommerceMerchants ([MerchantID],[GatewayName],[Name], [MerchantEmail], [TechnicalEmail], [MetadataXml]) 
VALUES(N'200',	N'Shipping', N'Electronic Delivery', N'testemail@testsite.com', N'testemail@testsite.com', N'<Metadata />')
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartTotal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartTotal]
GO

CREATE PROCEDURE rb_CartTotal
(
    @CartID    nvarchar(50),
    @ModuleID int,
    @WorkflowVersion int,
    @TotalCost money OUTPUT,
    @ISOCurrencySymbol	char(3) OUTPUT
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

SELECT @ISOCurrencySymbol = (SELECT    SettingValue
                            FROM      rb_ModuleSettings
                            WHERE     (ModuleID = @ModuleID) AND (SettingName = N'Currency') )
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1730','1.2.8.1730', CONVERT(datetime, '07/28/2003', 101))
GO

--DELETE FROM [rb_Versions] WHERE [Version] = '1.2.8.1730'

