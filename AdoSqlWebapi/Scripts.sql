use testDB
CREATE TABLE tblProduct (ProductId INT NOT NULL IDENTITY(1,1),ProductName NVARCHAR(20),ProductType NVARCHAR(20),ProductShopName NVARCHAR(20),ProductLocation NVARCHAR(20),PRIMARY KEY (ProductId));
GO
CREATE PROCEDURE GetProducts
AS 
BEGIN 
SELECT ProductId,ProductName,ProductType,ProductShopName,ProductLocation FROM tblProduct;
END;
GO
CREATE PROCEDURE InsertProduct
    @ProductName NVARCHAR(20),
    @ProductType NVARCHAR(20),
    @ProductShopName NVARCHAR(20),
    @ProductLocation NVARCHAR(20)
AS
BEGIN
    INSERT INTO tblProduct (ProductName,ProductType,ProductShopName,ProductLocation)
    VALUES (@ProductName,@ProductType,@ProductShopName,@ProductLocation);
END;
GO
CREATE PROCEDURE UpdateProduct @ProductId INT, @ProductName NVARCHAR(20), @ProductType NVARCHAR(20), @ProductShopName NVARCHAR(20), @ProductLocation NVARCHAR(20) 
AS 
BEGIN 
UPDATE tblProduct SET ProductName = @ProductName, ProductType = @ProductType, ProductShopName = @ProductShopName, ProductLocation = @ProductLocation WHERE ProductId = @ProductId; 
END;
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 12/4/2024 10:59:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteProduct] 
@ProductId INT 
AS 
BEGIN 
DELETE FROM tblProduct WHERE ProductId = @ProductId;
END; 