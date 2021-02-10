USE master;
GO

CREATE DATABASE Bookshelf;
GO

USE Bookshelf;
GO

CREATE TABLE dbo.Rooms (
    RoomID INT PRIMARY KEY IDENTITY(1, 1),
    RoomName NVARCHAR(64) NOT NULL,
    FloorNumber INT NOT NULL
);
GO

CREATE TABLE dbo.Shelfs (
    ShelfID INT PRIMARY KEY IDENTITY(1, 1),
    ShelfName NVARCHAR(64) NOT NULL,
    RoomID INT NOT NULL,

    CONSTRAINT FK_Shelfs_Rooms FOREIGN KEY (RoomID)
    REFERENCES dbo.Rooms(RoomID)
);
GO

CREATE TABLE dbo.Generes (
    Genere NVARCHAR(32) PRIMARY KEY
);
GO

CREATE TABLE dbo.Books (
    BookID INT PRIMARY KEY IDENTITY(1, 1),
    Title NVARCHAR(128) NOT NULL,
    PurchaseDate DATE NOT NULL,
    Genere NVARCHAR(32) NOT NULL,
    ReadCount INT NOT NULL DEFAULT 0,
    ShelfID INT NOT NULL,

    CONSTRAINT CHK_Books_PurchaseDate CHECK (PurchaseDate <= GETDATE()),

    CONSTRAINT FK_Books_Generes FOREIGN KEY (Genere)
    REFERENCES dbo.Generes(Genere),

    CONSTRAINT FK_Books_Shelfs FOREIGN KEY (ShelfID)
    REFERENCES dbo.Shelfs(ShelfID)
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