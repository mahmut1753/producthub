SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO

USE ProductHub
GO

IF DB_NAME() <> N'ProductHub' SET NOEXEC ON
GO

--
-- Create schema [AUTH]
--
PRINT (N'Create schema [AUTH]')
GO
CREATE SCHEMA AUTH
GO

--
-- Create table [AUTH].[Users]
--
PRINT (N'Create table [AUTH].[Users]')
GO
CREATE TABLE AUTH.Users (
  Id int IDENTITY,
  Username nvarchar(30) NOT NULL,
  PasswordHash nvarchar(100) NOT NULL,
  Role int NOT NULL,
  IsActive bit NOT NULL DEFAULT (1),
  CreatedAt datetime2 NOT NULL DEFAULT (sysdatetime()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Create or alter procedure [AUTH].[sp_User_Update]
--
GO
PRINT (N'Create or alter procedure [AUTH].[sp_User_Update]')
GO

CREATE OR ALTER PROCEDURE AUTH.sp_User_Update
    @Id INT,
    @PasswordHash NVARCHAR(255),
    @Role INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE auth.Users
    SET
        PasswordHash = @PasswordHash,
        Role = @Role,
        IsActive     = @IsActive
    WHERE Id = @Id;

END
GO

--
-- Create or alter procedure [AUTH].[sp_User_Insert]
--
GO
PRINT (N'Create or alter procedure [AUTH].[sp_User_Insert]')
GO
CREATE OR ALTER PROCEDURE AUTH.sp_User_Insert
    @Username NVARCHAR(30),
    @PasswordHash NVARCHAR(255),
    @Role INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO auth.Users
    (
        Username,
        PasswordHash,
        Role,
        IsActive
    )
    VALUES
    (
        @Username,
        @PasswordHash,
        @Role,
        @IsActive
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END;
GO

--
-- Create or alter procedure [AUTH].[sp_User_GetByUsername]
--
GO
PRINT (N'Create or alter procedure [AUTH].[sp_User_GetByUsername]')
GO
CREATE OR ALTER PROCEDURE AUTH.sp_User_GetByUsername
    @Username NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        PasswordHash,
        Role,
        IsActive,
        CreatedAt
    FROM auth.Users
    WHERE Username = @Username;
END;
GO

--
-- Create or alter procedure [AUTH].[sp_User_GetById]
--
GO
PRINT (N'Create or alter procedure [AUTH].[sp_User_GetById]')
GO
CREATE OR ALTER PROCEDURE AUTH.sp_User_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        PasswordHash,
        Role,
        IsActive,
        CreatedAt
    FROM auth.Users
    WHERE Id = @Id;
END;
GO

--
-- Create or alter procedure [AUTH].[sp_User_GetAll]
--
GO
PRINT (N'Create or alter procedure [AUTH].[sp_User_GetAll]')
GO
CREATE OR ALTER PROCEDURE AUTH.sp_User_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Username,
        PasswordHash,
        Role,
        IsActive,
        CreatedAt
    FROM auth.Users

	WHERE IsActive=1 
END;
GO

--
-- Create schema [CATALOG]
--
PRINT (N'Create schema [CATALOG]')
GO
CREATE SCHEMA CATALOG
GO

--
-- Create table [CATALOG].[Products]
--
PRINT (N'Create table [CATALOG].[Products]')
GO
CREATE TABLE CATALOG.Products (
  Id int IDENTITY,
  ExternalProductId int NULL,
  Name nvarchar(200) NOT NULL,
  Description nvarchar(1000) NOT NULL,
  Image nvarchar(500) NOT NULL,
  Category nvarchar(100) NOT NULL,
  Price decimal(18, 2) NOT NULL,
  IsActive bit NOT NULL DEFAULT (1),
  CreatedAt datetime2 NOT NULL DEFAULT (sysutcdatetime()),
  PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_Update]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_Update]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_Update
    @Id INT,
    @Name NVARCHAR(200),
    @Description NVARCHAR(1000),
    @Image NVARCHAR(500),
    @Category NVARCHAR(100),
    @Price DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE catalog.Products
    SET 
        Name = @Name,
        Description = @Description,
        Image = @Image,
        Category = @Category,
        Price = @Price
    WHERE Id = @Id AND IsActive = 1;
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_UnMatchExternal]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_UnMatchExternal]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_UnMatchExternal
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE catalog.Products
    SET ExternalProductId = NULL
    WHERE Id = @Id
      AND IsActive = 1
      AND ExternalProductId IS NOT NULL;
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_MatchExternal]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_MatchExternal]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_MatchExternal
    @Id INT,
    @ExternalProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE catalog.Products
    SET ExternalProductId = @ExternalProductId
    WHERE Id = @Id
      AND IsActive = 1
      AND ExternalProductId IS NULL;
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_Insert]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_Insert]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_Insert
    @Name NVARCHAR(200),
    @ExternalProductId INT = NULL,
    @Description NVARCHAR(1000),
    @Image NVARCHAR(500),
    @Category NVARCHAR(100),
    @Price DECIMAL(18,2),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO catalog.Products
    (
        Name, ExternalProductId, Description, Image, Category, Price, IsActive
    )
    VALUES
    (
        @Name, @ExternalProductId, @Description, @Image, @Category, @Price, @IsActive
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_GetById]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_GetById]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ExternalProductId,
        Name,
        Description,
        Image,
        Category,
        Price
    FROM catalog.Products
    WHERE Id = @Id AND IsActive = 1;
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_GetAll]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_GetAll]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ExternalProductId,
        Name,
        Description,
        Image,
        Category,
        Price
    FROM catalog.Products
    WHERE IsActive = 1;
END;
GO

--
-- Create or alter procedure [CATALOG].[sp_Product_Delete]
--
GO
PRINT (N'Create or alter procedure [CATALOG].[sp_Product_Delete]')
GO
CREATE OR ALTER PROCEDURE CATALOG.sp_Product_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE catalog.Products
    SET IsActive = 0
    WHERE Id = @Id AND IsActive = 1;
END;
GO

-- 
-- Dumping data for table Users
--
PRINT (N'Dumping data for table Users')
SET IDENTITY_INSERT AUTH.Users ON
GO
INSERT AUTH.Users(Id, Username, PasswordHash, Role, IsActive, CreatedAt) VALUES (2, N'admin', N'$2a$11$8oxEyuRd9qbm8rRi0m71BeVhYxL1kmf57cSKXocwIArYouQIYiD/6', 1, CONVERT(bit, 'True'), '2026-01-08 19:26:35.2767128')
GO
SET IDENTITY_INSERT AUTH.Users OFF
GO

-- 
-- Dumping data for table Products
--
PRINT (N'Dumping data for table Products')
SET IDENTITY_INSERT CATALOG.Products ON
GO
INSERT CATALOG.Products(Id, ExternalProductId, Name, Description, Image, Category, Price, IsActive, CreatedAt) VALUES (1, 1, N'Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops', N'Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday', N'https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_t.png', N'men''s clothing', 109.95, CONVERT(bit, 'True'), '2026-01-08 15:22:11.9579783')
INSERT CATALOG.Products(Id, ExternalProductId, Name, Description, Image, Category, Price, IsActive, CreatedAt) VALUES (2, 3, N'Mens Cotton Jacket', N'great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions, such as working, hiking, camping, mountain/rock climbing, cycling, traveling or other outdoors.', N'https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_t.png', N'men''s clothing', 55.99, CONVERT(bit, 'True'), '2026-01-08 15:23:50.6196469')
INSERT CATALOG.Products(Id, ExternalProductId, Name, Description, Image, Category, Price, IsActive, CreatedAt) VALUES (3, 15, N'BIYLACLESEN Women''s 3-in-1 Snowboard Jacket Winter Coats', N'Note:The Jackets is US standard size, Please choose size as your usual wear Material: 100% Polyester; Detachable Liner Fabric: Warm Fleece.', N'https://fakestoreapi.com/img/51Y5NI-I5jL._AC_UX679_t.png', N'women''s clothing', 56.99, CONVERT(bit, 'True'), '2026-01-08 15:24:57.4196478')
GO
SET IDENTITY_INSERT CATALOG.Products OFF
GO
SET NOEXEC OFF
GO