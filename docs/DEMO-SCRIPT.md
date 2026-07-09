# Live demo script — Loan Portal (complete)

**Duration:** 12–15 minutes  
**GitHub:** https://github.com/gaurav2057/LoanPortal  
**Run:** `cd src/LoanPortal.Web && dotnet run`

---

## Opening (30 sec)

> This is Loan Portal — a small lending app for learning company stack: Blazor Server, ASP.NET Core Identity, SQL Server, and Dapper. Officers approve loans; customers pay. There is also a security lab and a background job for overdue loans.

---

## 1. Repo tour (1 min)

- `database/` — schema and seed SQL
- `src/LoanPortal.Web/` — main Blazor app (run this)
- `src/LoanPortal.Api/` — optional REST + Swagger
- `docs/BEGINNER-STUDY-PATH.md` — study from basics

**No Docker** — SQL Server LocalDB or SSMS.

---

## 2. SQL (1 min)

Open `database/01-create-tables.sql` in IDE or SSMS.

> Customers, Loans, Payments — foreign keys and CHECK on status. Seed data includes one overdue loan for the background demo.

---

## 3. Start app and Identity (2 min)

```bash
cd src/LoanPortal.Web
dotnet run
```

Login: **officer@bank.com** / **Officer@123**

> Authentication is ASP.NET Core Identity with roles Officer and Customer — not a custom users table.

Show **Account** menu and officer-only nav: **Overdue alerts**, **Security demo**.

---

## 4. Loans page (3 min)

Go to **Loans**.

> List comes from Dapper — JOIN customers, SUM payments. Pending loans can be approved by Officer.

1. Approve Alice's pending loan (if still pending)
2. Logout → login **alice@example.com** / **Customer@123**
3. Show customer can pay on active loan but cannot approve

> Authorization is role-based on the Blazor page and in the UI with AuthorizeView.

---

## 5. Dapper + SQL injection (2 min)

Login as Officer → **Security demo**.

1. Safe search: `alice@example.com`
2. Unsafe search: same input
3. Optional: `' OR '1'='1` on unsafe only

Open `Services/SecurityDemoService.cs` or `Repositories/LoanRepository.cs`.

> Production code uses `@Email` parameters. Concatenating strings is how SQL injection happens.

---

## 6. Background service (2 min)

Wait ~10 seconds after startup (or refresh page).

Open **Overdue alerts**.

Open `Background/OverdueLoanBackgroundService.cs`.

> Hosted services run on a timer inside the app — useful for nightly reports and reminders without a separate Windows Service.

---

## 7. Optional API (1 min)

```bash
cd src/LoanPortal.Api
dotnet run
```

Swagger: `GET /api/loans`, `GET /api/demo/safe-search`.

> Same repositories power both the website and the API.

---

## Closing (30 sec)

> Code is on GitHub. I studied SQL, C#, Blazor Server, Identity, OWASP basics, and background services in one repo. Connection string targets LocalDB; SSMS can inspect the same tables.

---

## Demo logins

| Role | Email | Password |
|------|-------|----------|
| Officer | officer@bank.com | Officer@123 |
| Customer | alice@example.com | Customer@123 |
