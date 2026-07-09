# Study guide — file by file

Use with `docs/BEGINNER-STUDY-PATH.md` for week order. This file explains **what each part does**.

---

## Database

### `database/01-create-tables.sql`

Creates `Customers`, `Loans`, `Payments`. Foreign keys link loans to customers and payments to loans. `Status` has a CHECK constraint.

### `database/02-seed-data.sql`

Inserts Alice, Bob, Carol and sample loans. Updates Bob's active loan to a **past due date** so the background service has demo data.

---

## API project (`src/LoanPortal.Api`)

### `Models/*.cs`

C# types matching SQL columns. Dapper maps query results to these classes.

### `Data/SqlConnectionFactory.cs`

Reads `ConnectionStrings:DefaultConnection` from `appsettings.json` and opens `SqlConnection`.

### `Data/DatabaseBootstrapper.cs`

On startup: create database if needed, run `01` and `02` SQL scripts. Shared by Web and Api.

### `Repositories/*.cs`

Parameterized SQL via Dapper. **Always** use `@ParameterName`, never string concatenation for user input.

Key method: `LoanRepository.GetOverdueAsync()` — active loans where `DueDate < today`.

### `Controllers/*.cs`

| Controller | Purpose |
|------------|---------|
| CustomersController | List customers |
| LoansController | List, create, approve loans |
| PaymentsController | Record payments |
| DemoController | API unsafe vs safe search |

### `Program.cs`

Registers repositories, Swagger, bootstrap on startup.

---

## Web project (`src/LoanPortal.Web`) — main app

### `Program.cs`

- Blazor Server (`AddInteractiveServerComponents`)
- Identity + SQL Server EF store
- Roles: Officer, Customer
- Dapper repositories (same as API)
- `SecurityDemoService`, `OverdueLoanAlertStore`, `OverdueLoanBackgroundService`

### `Data/ApplicationDbContext.cs`

EF Core context for **Identity only** (users, roles, logins). Business data uses Dapper.

### `Data/IdentitySeedData.cs`

Creates roles, officer and customer users, links `alice@example.com` to customer record.

### `Components/Pages/Home.razor`

Landing page with study links and demo logins.

### `Components/Pages/Loans.razor`

Main business UI: list loans, officer approve, customer pay.

### `Components/Pages/SecurityDemo.razor`

Officer-only OWASP lab: unsafe string SQL vs safe Dapper parameters.

### `Components/Pages/OverdueAlerts.razor`

Officer-only view of latest background check results.

### `Services/SecurityDemoService.cs`

Implements unsafe and safe customer search for teaching.

### `Services/OverdueLoanAlertStore.cs`

In-memory store of latest overdue check (singleton).

### `Background/OverdueLoanBackgroundService.cs`

`BackgroundService` — first run after 10 seconds, then every hour. Logs and updates alert store.

### `Components/Layout/NavMenu.razor`

Navigation; officer-only links wrapped in `AuthorizeView Roles="Officer"`.

### `Components/Account/*`

Scaffolded Identity UI: Login, Register, Manage.

---

## Configuration

### `appsettings.json` (Web and Api)

```text
Server=(localdb)\mssqllocaldb;Database=LoanPortal;Trusted_Connection=True;TrustServerCertificate=True;
```

Change server name if using SQL Express — see `docs/SQL-SERVER-SETUP.md`.

---

## Practice order (quick)

1. SQL scripts
2. One model + one repository
3. `LoansController` or `Loans.razor`
4. Identity seed + login
5. Security demo + background service
6. Full demo from `docs/DEMO-SCRIPT.md`

---

## Interview one-liners

| Topic | Line |
|-------|------|
| Blazor | "Server-side rendering with SignalR — not WebAssembly." |
| Identity | "Built-in auth with roles; Identity tables in SQL Server via EF." |
| Data | "Business queries use Dapper; Identity uses EF Core." |
| Security | "All production SQL uses parameters; unsafe code is isolated for demo." |
| Background | "Hosted service checks overdue loans on a timer and logs reminders." |
