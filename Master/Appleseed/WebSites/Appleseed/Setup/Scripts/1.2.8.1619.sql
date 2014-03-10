---------------------
--1.2.8.1619.sql
---------------------

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[FK_Products_st_Modules]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [rb_Products_st] DROP CONSTRAINT FK_Products_st_Modules
GO

/****** Object:  Trigger dbo.rb_Products_stModified    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products_stModified]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [rb_Products_stModified]
GO

/****** Object:  Stored PROCEDURE dbo.rb_UpdateProduct    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_UpdateProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_UpdateProduct]
GO

/****** Object:  Stored PROCEDURE dbo.rb_DeleteProduct    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_DeleteProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_DeleteProduct]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleProduct    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetSingleProduct]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetSingleProduct]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProducts    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProducts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProducts]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartList    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartList]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartList]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartTotal    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartTotal]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartTotal]
GO

/****** Object:  Stored PROCEDURE dbo.rb_ProductsChangeCategory    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_ProductsChangeCategory]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_ProductsChangeCategory]
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartAddItem    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_CartAddItem]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_CartAddItem]
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProductsPaged    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_GetProductsPaged]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [rb_GetProductsPaged]
GO

/****** Object:  Table [rb_Products_st]    Script Date: 4/19/2003 3:21:59 PM ******/
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[rb_Products_st]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [rb_Products_st]
GO

/****** Object:  Table [rb_Products_st]    Script Date: 4/19/2003 3:22:15 PM ******/
CREATE TABLE [rb_Products_st] (
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

ALTER TABLE [rb_Products_st] WITH NOCHECK ADD 
	CONSTRAINT [DF_Products_st_DisplayOrder] DEFAULT (0) FOR [DisplayOrder],
	CONSTRAINT [DF_Products_st_RetailPrice] DEFAULT (0) FOR [RetailPrice],
	CONSTRAINT [DF_Products_st_SalePrice] DEFAULT (0) FOR [SalePrice],
	CONSTRAINT [DF_Products_st_FeaturedItem] DEFAULT (0) FOR [FeaturedItem],
	CONSTRAINT [PK_Products_st] PRIMARY KEY  CLUSTERED 
	(
		[ProductID]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
GO

ALTER TABLE [rb_Products_st] ADD 
	CONSTRAINT [FK_Products_st_Modules] FOREIGN KEY 
	(
		[ModuleID]
	) REFERENCES [rb_Modules] (
		[ModuleID]
	) ON DELETE CASCADE  NOT FOR REPLICATION 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProductsPaged    Script Date: 4/19/2003 3:22:16 PM ******/
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
	RetailPrice	money,
	SalePrice	money,
	FeaturedItem	bit,
	LongDescription	ntext,
	ShortDescription ntext,
	MetadataXml	ntext
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
			RetailPrice,
			SalePrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml
		)
		SELECT
		    	rb_Products_st.ProductID,
			rb_Products_st.DisplayOrder,
			rb_Products_st.ModelNumber,
			rb_Products_st.ModelName,
			rb_Products_st.RetailPrice,
			rb_Products_st.SalePrice,
			rb_Products_st.FeaturedItem,
			rb_Products_st.LongDescription,
			rb_Products_st.ShortDescription,
			rb_Products_st.MetadataXml
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
			RetailPrice,
			SalePrice,
			FeaturedItem,
			LongDescription,
			ShortDescription,
			MetadataXml
		)
		SELECT
		    	rb_Products_st.ProductID,
			rb_Products_st.DisplayOrder,
			rb_Products_st.ModelNumber,
			rb_Products_st.ModelName,
			rb_Products_st.RetailPrice,
			rb_Products_st.SalePrice,
			rb_Products_st.FeaturedItem,
			rb_Products_st.LongDescription,
			rb_Products_st.ShortDescription,
			rb_Products_st.MetadataXml
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_CartAddItem    Script Date: 4/19/2003 3:22:16 PM ******/
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
        ModuleID,
        DateCreated
    )
    VALUES
    (
        @CartID,
        @Quantity,
        @ProductID,
        @ModuleID,
        GetDate()
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

/****** Object:  Stored PROCEDURE dbo.rb_UpdateProduct    Script Date: 4/19/2003 3:22:16 PM ******/
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
IF (@ProductID=0) OR NOT EXISTS (SELECT * FROM rb_Products_st WHERE ProductID = @ProductID)
INSERT INTO rb_Products_st
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
    rb_Products_st
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_DeleteProduct    Script Date: 4/19/2003 3:22:16 PM ******/
CREATE PROCEDURE rb_DeleteProduct
(
    @ProductID int
)
AS
DELETE FROM
    rb_Products_st
WHERE
    ProductID = @ProductID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetSingleProduct    Script Date: 4/19/2003 3:22:16 PM ******/
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
		OriginalProducts.RetailPrice,
		OriginalProducts.SalePrice,
		OriginalProducts.FeaturedItem,
		OriginalProducts.LongDescription,
		OriginalProducts.ShortDescription,
		OriginalProducts.MetadataXml
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_GetProducts    Script Date: 4/19/2003 3:22:16 PM ******/
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

END 
ELSE
BEGIN

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
		    RetailPrice, 
		    SalePrice,
		    FeaturedItem,
		    LongDescription,
		    ShortDescription,
		    MetadataXml
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

/****** Object:  Stored PROCEDURE dbo.rb_CartList    Script Date: 4/19/2003 3:22:16 PM ******/
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
END
ELSE
BEGIN
	SELECT 
	    rb_Products_st.ProductID, 
	    rb_Products_st.ModelName,
	    rb_Products_st.ModelNumber,
	    rb_Cart.Quantity,
	    rb_Products_st.SalePrice as UnitCost,
	    Cast((rb_Products_st.SalePrice * rb_Cart.Quantity) as money) as ExtendedAmount
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

/****** Object:  Stored PROCEDURE dbo.rb_CartTotal    Script Date: 4/19/2003 3:22:16 PM ******/
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
END
ELSE
BEGIN
	SELECT 
	    @TotalCost = SUM(rb_Products_st.SalePrice * rb_Cart.Quantity)
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
SET ANSI_NULLS ON 
GO

/****** Object:  Stored PROCEDURE dbo.rb_ProductsChangeCategory    Script Date: 4/19/2003 3:22:17 PM ******/
CREATE PROC rb_ProductsChangeCategory
(
	@CategoryID int,
	@NewCategoryID int
)
AS
UPDATE rb_Products_st
SET
CategoryID = @NewCategoryID
WHERE
CategoryID = @CategoryID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Trigger dbo.rb_Products_stModified    Script Date: 4/19/2003 3:22:17 PM ******/
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
INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('1619','1.2.8.1619', CONVERT(datetime, '04/19/2003', 101))
GO

