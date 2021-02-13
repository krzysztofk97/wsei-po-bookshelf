USE master;
GO

CREATE DATABASE Bookshelf;
GO

USE Bookshelf;
GO

CREATE TABLE dbo.Shelfs (
    ShelfName NVARCHAR(64) PRIMARY KEY 
);
GO

CREATE TABLE dbo.Generes (
    GenereName NVARCHAR(32) PRIMARY KEY
);
GO

CREATE TABLE dbo.Books (
    BookID INT PRIMARY KEY IDENTITY(1, 1),
    Title NVARCHAR(128) NOT NULL,
    PurchaseDate DATE NOT NULL,
    GenereName NVARCHAR(32) NOT NULL,
    ReadCount INT NOT NULL DEFAULT 0,
    ShelfName NVARCHAR(64) NOT NULL,

    CONSTRAINT CHK_Books_PurchaseDate CHECK (PurchaseDate <= GETDATE()),

    CONSTRAINT FK_Books_Generes FOREIGN KEY (GenereName)
    REFERENCES dbo.Generes(GenereName),

    CONSTRAINT FK_Books_Shelfs FOREIGN KEY (ShelfName)
    REFERENCES dbo.Shelfs(ShelfName)
);
GO

CREATE TABLE dbo.Authors (
    AuthorID INT PRIMARY KEY IDENTITY(1, 1),
    FirstName NVARCHAR(32),
    LastName NVARCHAR(32)
);
GO

CREATE TABLE dbo.BookAuthors (
    BookID INT NOT NULL,
    AuthorID INT NOT NULL,

    PRIMARY KEY(BookID, AuthorID),

    CONSTRAINT FK_BookAuthors_Books FOREIGN KEY (BookID)
    REFERENCES dbo.Books(BookID),

    CONSTRAINT FK_BookAuthors_Authors FOREIGN KEY (AuthorID)
    REFERENCES dbo.Authors(AuthorID)
);
GO