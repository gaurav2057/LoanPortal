-- Sample data for demos and Swagger testing

IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE Email = N'alice@example.com')
BEGIN
    INSERT INTO dbo.Customers (Name, Email) VALUES
        (N'Alice Johnson', N'alice@example.com'),
        (N'Bob Smith', N'bob@example.com'),
        (N'Carol Lee', N'carol@example.com');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Loans)
BEGIN
    INSERT INTO dbo.Loans (CustomerId, Amount, Status, DueDate)
    SELECT CustomerId, 10000.00, N'Pending', DATEADD(month, 12, CAST(GETDATE() AS DATE))
    FROM dbo.Customers WHERE Email = N'alice@example.com';

    INSERT INTO dbo.Loans (CustomerId, Amount, Status, DueDate)
    SELECT CustomerId, 5000.00, N'Active', DATEADD(month, 6, CAST(GETDATE() AS DATE))
    FROM dbo.Customers WHERE Email = N'bob@example.com';

    INSERT INTO dbo.Loans (CustomerId, Amount, Status, DueDate)
    SELECT CustomerId, 7500.00, N'Closed', DATEADD(month, -1, CAST(GETDATE() AS DATE))
    FROM dbo.Customers WHERE Email = N'carol@example.com';
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Payments)
BEGIN
    INSERT INTO dbo.Payments (LoanId, Amount, PaidAt)
    SELECT TOP 1 LoanId, 1500.00, DATEADD(day, -10, SYSUTCDATETIME())
    FROM dbo.Loans l
    INNER JOIN dbo.Customers c ON c.CustomerId = l.CustomerId
    WHERE c.Email = N'bob@example.com' AND l.Status = N'Active';
END
GO
