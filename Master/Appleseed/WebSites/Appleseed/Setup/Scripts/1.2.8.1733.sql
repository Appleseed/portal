/* Update script Ecommerce - 1732 fixes, by Manu [manu-dea@hotmail dot it] */

ALTER PROCEDURE rb_CartList
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
	    (rb_Products.UnitPrice + (rb_Products.UnitPrice * rb_Products.TaxRate / 100)) as UnitCostWithTaxes,	    
	    Cast(((rb_Products.UnitPrice + (rb_Products.UnitPrice * rb_Products.TaxRate / 100)) * rb_Cart.Quantity) as money) as ExtendedAmount,
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
	    (rb_Products_st.UnitPrice + (rb_Products_st.UnitPrice * rb_Products_st.TaxRate / 100)) as UnitCostWithTaxes,	    
	    Cast(((rb_Products_st.UnitPrice + (rb_Products_st.UnitPrice * rb_Products_st.TaxRate / 100)) * rb_Cart.Quantity) as money) as ExtendedAmount,
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

ALTER PROCEDURE rb_CartTotal
(
    @CartID    nvarchar(50),
    @IncludeTaxes bit = false,
    @ModuleID int,
    @WorkflowVersion int,
    @TotalCost money OUTPUT,
    @ISOCurrencySymbol	char(3) OUTPUT
)
AS
IF (@IncludeTaxes = 0)
BEGIN
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
END

ELSE

BEGIN
	IF ( @WorkflowVersion = 1 )
	BEGIN
		SELECT 
		    @TotalCost = SUM((rb_Products.UnitPrice + (rb_Products.UnitPrice * rb_Products.TaxRate / 100)) * rb_Cart.Quantity)
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
		    @TotalCost = SUM((rb_Products_st.UnitPrice + (rb_Products_st.UnitPrice * rb_Products_st.TaxRate / 100)) * rb_Cart.Quantity)
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
END

SELECT @ISOCurrencySymbol = (SELECT    SettingValue
                            FROM      rb_ModuleSettings
                            WHERE     (ModuleID = @ModuleID) AND (SettingName = N'Currency') )
GO

-- Installs updated links module
-- this is the recommended way for install new modules
DECLARE @GeneralModDefID uniqueidentifier
DECLARE @FriendlyName nvarchar(128)
DECLARE @DesktopSrc nvarchar(256)
DECLARE @MobileSrc nvarchar(256)
DECLARE @AssemblyName varchar(50)
DECLARE @ClassName nvarchar(128)
DECLARE @Admin bit
DECLARE @Searchable bit

SET @GeneralModDefID = '{476CF1CC-8364-479D-9764-4B3ABD7FFABD}'
SET @FriendlyName = 'Links'
SET @DesktopSrc = 'DesktopModules/Links/Links.ascx'
SET @MobileSrc = ''
SET @AssemblyName = 'Appleseed.DLL'
SET @ClassName = ''
SET @Admin = 0
SET @Searchable = 1

-- Installs module
EXEC [rb_AddGeneralModuleDefinitions] @GeneralModDefID, @FriendlyName, @DesktopSrc, @MobileSrc, @AssemblyName, @ClassName, @Admin, @Searchable
GO

INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1733','1.2.8.1733', CONVERT(datetime, '08/01/2003', 101))
GO

--DELETE FROM [rb_Versions] WHERE [Version] = '1.2.8.1733'

