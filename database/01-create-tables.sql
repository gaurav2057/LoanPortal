-- Loan Portal: create tables (run against LoanPortal database)
-- Customers hold borrower identity; Loans and Payments are separate tables (3NF).

IF OBJECT_ID('dbo.Payments', 'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID('dbo.Loans', 'U') IS NOT NULL DROP TABLE dbo.Loans;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

CREATE TABLE dbo.Customers (
    CustomerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE
);
GO

CREATE TABLE dbo.Loans (
    LoanId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL CHECK (Amount > 0),
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Active', 'Closed')),
    DueDate DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Loans_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(CustomerId)
);
GO

CREATE TABLE dbo.Payments (
    PaymentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    LoanId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL CHECK (Amount > 0),
    PaidAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Payments_Loans FOREIGN KEY (LoanId) REFERENCES dbo.Loans(LoanId)
);
GO

CREATE INDEX IX_Loans_CustomerId ON dbo.Loans(CustomerId);
CREATE INDEX IX_Loans_Status ON dbo.Loans(Status);
CREATE INDEX IX_Payments_LoanId ON dbo.Payments(LoanId);
GO
