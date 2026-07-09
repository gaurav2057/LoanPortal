# Loan Portal

Loan management demo for GitHub and live presentation: **Blazor Server**, **ASP.NET Core Identity**, **Dapper**, **SQL Server** (SSMS / LocalDB), **OWASP security lab**, and **BackgroundService** — **no Docker**.

**GitHub:** [gaurav2057/LoanPortal](https://github.com/gaurav2057/LoanPortal)

## Study from basics

Start here: **`docs/BEGINNER-STUDY-PATH.md`** (7-week path from SQL to mentor demo).

Then: `docs/STUDY-GUIDE.md` (file-by-file) and `docs/DEMO-SCRIPT.md` (presentation).

## Mentor requirements (all implemented)

| Requirement | Implementation |
|-------------|----------------|
| Blazor **Server** | `LoanPortal.Web` — interactive server render mode |
| Not WebAssembly | No `blazorwasm` project |
| **ASP.NET Core Identity** | Built-in Identity with roles (not custom auth) |
| SQL Server | LocalDB or SSMS — see `docs/SQL-SERVER-SETUP.md` |
| GitHub | [gaurav2057/LoanPortal](https://github.com/gaurav2057/LoanPortal) |

## Four phases (complete)

| Phase | What you learn |
|-------|----------------|
| 1 | REST API + Dapper + SQL scripts |
| 2 | Blazor Server UI + Identity roles |
| 3 | OWASP SQL injection demo (Officer page) |
| 4 | BackgroundService overdue loan alerts |

## Tech stack

| Layer | Technology |
|-------|------------|
| UI | Blazor Server (.NET 8) |
| Auth | ASP.NET Core Identity + roles |
| API (optional) | ASP.NET Core Web API + Swagger |
| Data access | Dapper (parameterized SQL) |
| Database | SQL Server (LocalDB / Express / SSMS) |
| Background jobs | `IHostedService` / `BackgroundService` |

## Project structure

```
LoanPortal/
├── database/              # SQL scripts
├── docs/                  # Study path, demo script, setup
├── src/
│   ├── LoanPortal.Web/    # Main app — run this
│   └── LoanPortal.Api/    # Optional REST + Swagger
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) + [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (recommended)
- Visual Studio 2022 (optional)

**No Docker required.**

## Quick start (Windows)

### 1. SQL Server

Use **LocalDB** (default) or see `docs/SQL-SERVER-SETUP.md`.

### 2. Run the website

```bash
cd src/LoanPortal.Web
dotnet run
```

Open the HTTPS URL from the terminal.

### 3. Login and explore

| Role | Email | Password | Pages |
|------|-------|----------|-------|
| Officer | officer@bank.com | Officer@123 | Loans, approve, Security demo, Overdue alerts |
| Customer | alice@example.com | Customer@123 | Loans, pay own loans |

Wait ~10 seconds after start, then open **Overdue alerts** as Officer (Bob's loan is seeded overdue).

### 4. Optional — API + Swagger

```bash
cd src/LoanPortal.Api
dotnet run
```

Open `/swagger`.

## Connection string

Default in `src/LoanPortal.Web/appsettings.json`:

```
Server=(localdb)\mssqllocaldb;Database=LoanPortal;Trusted_Connection=True;TrustServerCertificate=True;
```

Update both Web and Api `appsettings.json` if using SQL Express.

## Push to GitHub

```bash
cd LoanPortal
git add .
git commit -m "Complete all phases: security demo + background service + study docs"
git push origin main
```

## What you learn

- SQL: tables, keys, JOINs, seed data
- C# / ASP.NET Core: DI, controllers, hosted services
- Blazor Server: components, authorization UI
- Identity: login, roles, Officer vs Customer
- Security: parameterized SQL vs injection
- SSMS: inspect `LoanPortal` database after first run
