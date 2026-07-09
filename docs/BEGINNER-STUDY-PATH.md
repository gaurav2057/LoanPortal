# Beginner study path — start from zero

This project is your **practice lab** for company onboarding topics. Follow the weeks in order. Each week has **read**, **run**, and **say** (practice explaining aloud).

**Repo:** https://github.com/gaurav2057/LoanPortal

---

## Before week 1 — install tools (Windows)

1. Install [.NET 8 SDK](https://dotnet.microsoft.com/download)
2. Install [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or use **LocalDB** (comes with Visual Studio)
3. Install [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional but helpful)
4. Install [Visual Studio 2022](https://visualstudio.microsoft.com/) with **ASP.NET and web development** workload

See `docs/SQL-SERVER-SETUP.md` for connection strings.

---

## Week 1 — SQL basics

**Goal:** Understand tables, keys, and simple queries.

### Read

1. `database/01-create-tables.sql` — Customers, Loans, Payments
2. `database/02-seed-data.sql` — sample rows

### Concepts

| Term | Meaning in this project |
|------|-------------------------|
| Primary key | `CustomerId`, `LoanId` — unique row id |
| Foreign key | `Loans.CustomerId` → `Customers.CustomerId` |
| JOIN | Combine customer name with loan row |
| CHECK | `Status` must be Pending, Active, or Paid |

### Practice in SSMS

After first app run, connect to `(localdb)\mssqllocaldb`, database `LoanPortal`:

```sql
SELECT c.Name, l.Amount, l.Status
FROM dbo.Loans l
INNER JOIN dbo.Customers c ON c.CustomerId = l.CustomerId;
```

### Say aloud

> "Loans reference customers with a foreign key. Payments reference loans. That keeps data normalized."

---

## Week 2 — C# and ASP.NET Core API

**Goal:** HTTP, controllers, dependency injection.

### Read (in order)

1. `src/LoanPortal.Api/Models/` — C# classes for SQL rows
2. `src/LoanPortal.Api/Data/SqlConnectionFactory.cs`
3. `src/LoanPortal.Api/Repositories/LoanRepository.cs`
4. `src/LoanPortal.Api/Controllers/LoansController.cs`
5. `src/LoanPortal.Api/Program.cs`

### Run

```bash
cd src/LoanPortal.Api
dotnet run
```

Open `/swagger`. Try `GET /api/loans` and `POST /api/loans/{id}/approve`.

### Concepts

| Term | Where |
|------|-------|
| REST | GET = read, POST = create/change |
| DI | `Program.cs` registers `ILoanRepository` |
| Dapper | `connection.QueryAsync<T>(sql, params)` |

### Say aloud

> "Controllers are thin. SQL lives in repositories. Dapper maps rows to C# objects."

---

## Week 3 — Blazor Server UI

**Goal:** Razor components, SignalR, forms.

### Read

1. `src/LoanPortal.Web/Components/Pages/Loans.razor`
2. `src/LoanPortal.Web/Components/Layout/NavMenu.razor`
3. `src/LoanPortal.Web/Program.cs` (Blazor + services)

### Run

```bash
cd src/LoanPortal.Web
dotnet run
```

Login as `alice@example.com` / `Customer@123`. Open **Loans**.

### Concepts

| Term | Meaning |
|------|---------|
| Blazor Server | UI runs on server; browser gets updates via SignalR |
| `@page` | URL route |
| `@inject` | Get a service in the component |
| `@onclick` | Button handler |

### Say aloud

> "This is Blazor Server, not WebAssembly. The C# runs on the server and the page updates over a live connection."

---

## Week 4 — Identity and authorization

**Goal:** Login, roles, who can see what.

### Read

1. `src/LoanPortal.Web/Data/ApplicationDbContext.cs` — Identity tables
2. `src/LoanPortal.Web/Data/IdentitySeedData.cs` — roles and demo users
3. `Components/Account/Pages/Login.razor`
4. `[Authorize(Roles = ...)]` on `Loans.razor`, `SecurityDemo.razor`

### Run

1. Login as **Officer** — `officer@bank.com` / `Officer@123` — approve a pending loan
2. Login as **Customer** — `alice@example.com` / `Customer@123` — pay on active loan
3. Notice Customer cannot open **Security demo** or **Overdue alerts**

### Concepts

| Term | Meaning |
|------|---------|
| Identity | Built-in user/password store |
| Role | Officer vs Customer |
| Authorize | Page or API blocked without role |

### Say aloud

> "We use ASP.NET Core Identity, not a custom login table. Officers approve loans; customers only see their own data."

---

## Week 5 — Security (OWASP)

**Goal:** SQL injection and parameterized queries.

### Read

1. `src/LoanPortal.Web/Services/SecurityDemoService.cs`
2. `src/LoanPortal.Web/Components/Pages/SecurityDemo.razor`
3. `src/LoanPortal.Api/Controllers/DemoController.cs` (API version)

### Run

Login as Officer → **Security demo**.

1. Safe search: `alice@example.com`
2. Unsafe search: same email — compare
3. Optional lab only: `' OR '1'='1` on unsafe button

### Say aloud

> "Never concatenate user input into SQL. Dapper parameters treat input as data, not executable code."

---

## Week 6 — Background services

**Goal:** Code that runs on a timer without a user click.

### Read

1. `src/LoanPortal.Web/Background/OverdueLoanBackgroundService.cs`
2. `src/LoanPortal.Web/Services/OverdueLoanAlertStore.cs`
3. `GetOverdueAsync()` in `LoanRepository.cs`

### Run

1. Login as Officer
2. Wait ~10 seconds after app start (first check)
3. Open **Overdue alerts** — Bob's loan should appear (seed sets past due date)

### Say aloud

> "BackgroundService runs inside the web app. We scope a repository per check, log warnings, and store results for the UI."

---

## Week 7 — Present to mentor

Use `docs/DEMO-SCRIPT.md` end to end.

Checklist:

- [ ] Blazor Server (not WASM)
- [ ] Identity login with two roles
- [ ] SQL Server + SSMS can show tables
- [ ] Dapper with `@parameters`
- [ ] Security demo (safe vs unsafe)
- [ ] Overdue background job
- [ ] GitHub repo pushed

---

## Daily 30-minute habit

| Day | Activity |
|-----|----------|
| Mon | Re-read one SQL file + run one query in SSMS |
| Tue | Trace one API call from Swagger to repository |
| Wed | Change one label in `Loans.razor`, rebuild, refresh |
| Thu | Login both roles; list what each can do |
| Fri | Practice demo script without looking at notes |

---

## When stuck

| Problem | Fix |
|---------|-----|
| `dotnet` not found | Add SDK to PATH or use Visual Studio terminal |
| Cannot connect to SQL | See `docs/SQL-SERVER-SETUP.md` |
| Login fails | Delete DB and restart app (bootstrap + seed runs again) |
| No overdue alerts | Wait 10s; ensure Officer role; check Bob's loan in SSMS |

---

## Next steps after this repo

- Add Entity Framework for loans (compare EF vs Dapper)
- Add email sender for real overdue reminders
- Deploy to Azure App Service
- Add unit tests for repositories
