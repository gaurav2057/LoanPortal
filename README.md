# Loan Portal

Loan management demo for GitHub and live presentation: **Blazor Server** website, **ASP.NET Core Identity**, **Dapper**, and **SQL Server** (SSMS / LocalDB — **no Docker**).

**GitHub:** [gaurav2057/LoanPortal](https://github.com/gaurav2057/LoanPortal)

## Mentor requirements (implemented)

| Requirement | Implementation |
|-------------|----------------|
| Blazor **Server** | `LoanPortal.Web` — interactive server render mode |
| Not WebAssembly | No `blazorwasm` project |
| **ASP.NET Core Identity** | Built-in Identity with roles (not custom auth) |
| SQL Server | LocalDB or SSMS — see `docs/SQL-SERVER-SETUP.md` |
| GitHub | Push from `/home/aurav_ingh/LoanPortal` |

## Tech stack

| Layer | Technology |
|-------|------------|
| UI | Blazor Server (.NET 8) |
| Auth | ASP.NET Core Identity + roles |
| API (optional) | ASP.NET Core Web API + Swagger |
| Data access | Dapper (parameterized SQL) |
| Database | SQL Server (LocalDB / Express / SSMS) |

## Project structure

```
LoanPortal/
├── database/              # SQL scripts for Customers, Loans, Payments
├── docs/                  # Setup, demo script, architecture
├── src/
│   ├── LoanPortal.Web/    # Main app — Blazor Server + Identity (run this)
│   └── LoanPortal.Api/    # REST API + Swagger (optional separate demo)
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) + [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Visual Studio 2022 or VS Code (optional)

**No Docker required.**

## Quick start (Windows)

### 1. SQL Server

Use **LocalDB** (default) or see `docs/SQL-SERVER-SETUP.md` for SQL Express.

### 2. Run the website

```bash
cd src/LoanPortal.Web
dotnet run
```

Open the HTTPS URL shown in the terminal (e.g. `https://localhost:7xxx`).

### 3. Login and demo

| Role | Email | Password | Can do |
|------|-------|----------|--------|
| Officer | officer@bank.com | Officer@123 | Approve pending loans |
| Customer | alice@example.com | Customer@123 | View own loans, pay |

Go to **Loans** after login.

### 4. Optional — API + Swagger

```bash
cd src/LoanPortal.Api
dotnet run
```

Open `/swagger` for REST endpoints.

## Live demo script

See `docs/DEMO-SCRIPT.md` and `docs/STUDY-GUIDE.md`.

**Say in interview:**

> "This is Blazor Server, not WebAssembly. Authentication uses ASP.NET Core Identity with Officer and Customer roles. Loan data is in SQL Server; Dapper runs parameterized queries."

## Push to GitHub

```bash
cd LoanPortal
git add .
git commit -m "Phase 2: Blazor Server + Identity, no Docker"
git push origin main
```

## Connection string

Default (LocalDB) in `src/LoanPortal.Web/appsettings.json`:

```
Server=(localdb)\mssqllocaldb;Database=LoanPortal;Trusted_Connection=True;TrustServerCertificate=True;
```

Change server name in both Web and Api `appsettings.json` if you use SQL Express — details in `docs/SQL-SERVER-SETUP.md`.

## What you learn

- Blazor Server UI (C# in browser via SignalR)
- ASP.NET Core Identity (login, roles, authorization)
- Dapper + SQL JOINs
- REST API (secondary project)
- SSMS — view tables after first run
