# SQL Server setup (no Docker)

Use **SQL Server on Windows** with **SSMS** — as your mentor suggested. No Docker required.

## Option A — SQL Server LocalDB (easiest on Windows)

Usually installed with Visual Studio.

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to: `(localdb)\mssqllocaldb`
3. The app creates database `LoanPortal` automatically on first run

Default connection string (already in `appsettings.json`):

```
Server=(localdb)\mssqllocaldb;Database=LoanPortal;Trusted_Connection=True;TrustServerCertificate=True;
```

## Option B — SQL Server Express

1. Install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Install [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
3. Connect to: `localhost\SQLEXPRESS`
4. Update `appsettings.json` in **both** projects:

```
Server=localhost\SQLEXPRESS;Database=LoanPortal;Trusted_Connection=True;TrustServerCertificate=True;
```

Change these keys:

- `DefaultConnection` (Web only — Identity)
- `LoanPortal` (business tables via Dapper)
- `Master` (creates database on first run)

Files:

- `src/LoanPortal.Web/appsettings.json`
- `src/LoanPortal.Api/appsettings.json`

## Option C — SQL Server with username/password

If you use SQL authentication instead of Windows auth:

```
Server=localhost\SQLEXPRESS;Database=LoanPortal;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;
```

## First run

```bash
cd src/LoanPortal.Web
dotnet run
```

On startup the app will:

1. Create `LoanPortal` database if missing
2. Run Identity migrations (ASP.NET Core Identity tables)
3. Seed roles: `Officer`, `Customer`
4. Seed demo users and loan sample data

## Verify in SSMS

After first run, refresh SSMS. You should see:

- **Identity tables:** `AspNetUsers`, `AspNetRoles`, …
- **Business tables:** `Customers`, `Loans`, `Payments`

## Demo logins

| Role | Email | Password |
|------|-------|----------|
| Officer | officer@bank.com | Officer@123 |
| Customer | alice@example.com | Customer@123 |
