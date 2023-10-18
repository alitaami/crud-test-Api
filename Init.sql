IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Crud-test-DB')
BEGIN
    CREATE DATABASE [Crud-test-DB];
END
GO

USE [Crud-test-DB];
GO

-- Create the Customers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        Id INT PRIMARY KEY,
        Firstname NVARCHAR(255),
        Lastname NVARCHAR(255),
        DateOfBirth DATE,
        PhoneNumber NVARCHAR(15),
        Email NVARCHAR(255),
        BankAccountNumber NVARCHAR(20)
    )
END
GO

-- Insert data if not exists
IF NOT EXISTS(SELECT * FROM Customers WHERE Id = 1)
BEGIN
    INSERT INTO Customers (Id, Firstname, Lastname, DateOfBirth, PhoneNumber, Email, BankAccountNumber)
    VALUES (1, 'Ali', 'Taami', '2002-10-11', '9301327634', 'alitaami81@gmail.com', '5859831001081461');
END

IF NOT EXISTS(SELECT * FROM Customers WHERE Id = 2)
BEGIN
    INSERT INTO Customers (Id, Firstname, Lastname, DateOfBirth, PhoneNumber, Email, BankAccountNumber)
    VALUES (2, 'Ata', 'Taami', '2012-10-11', '9301327624', 'atataami91@gmail.com', '5859831001081462');
END
GO
