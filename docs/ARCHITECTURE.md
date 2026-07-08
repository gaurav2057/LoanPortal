# Architecture — Loan Portal

## Request flow

```
Browser (Swagger)
       │  HTTP GET/POST + JSON
       ▼
Controllers  (CustomersController, LoansController, ...)
       │  calls interface
       ▼
Repositories  (Dapper runs SQL)
       │  SqlConnection
       ▼
SQL Server  (Customers, Loans, Payments tables)
```

## Why each layer exists

| Layer | Responsibility | Why separate? |
|-------|----------------|---------------|
| **Controller** | HTTP in/out, status codes | Web concerns stay at the edge |
| **Repository** | SQL + mapping to C# | Easy to test and change queries |
| **Model** | Shape of data | One class per table or DTO |
| **DatabaseBootstrapper** | Run scripts on startup | Demo works without manual SSMS step |

## Database design (3NF)

- **Customers** — identity (name, email) stored once
- **Loans** — references `CustomerId` (foreign key)
- **Payments** — references `LoanId` (foreign key)

No duplicate customer name on every loan row.

## Dependency injection

Registered in `Program.cs`:

- `ISqlConnectionFactory` → creates connections from config
- `ILoanRepository` → `LoanRepository`
- Controllers receive interfaces via constructor

## Security note

`DemoController` shows **unsafe** string concatenation vs **safe** `@Email` parameter. Production code uses only parameterized queries in repositories.
