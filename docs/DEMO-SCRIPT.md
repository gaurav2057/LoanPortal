# Live demo script — Loan Portal

**Duration:** 8–10 minutes  
**GitHub:** https://github.com/gaurav2057/LoanPortal

## Opening (30 sec)

> This is Loan Portal — a small lending API. Customers take loans and make payments. I used ASP.NET Core, Dapper, and SQL Server. Phase 1 uses Swagger as the UI.

## 1. Show repo structure (1 min)

- `database/` — SQL schema and seed data
- `Repositories/` — Dapper queries
- `Controllers/` — REST endpoints
- `docker-compose.yml` — SQL Server

## 2. Start services (30 sec)

```bash
docker compose up -d
cd src/LoanPortal.Api && dotnet run
```

Open Swagger URL from terminal.

## 3. GET loans (1 min)

`GET /api/loans`

> This query JOINs Customers and Loans and SUMs payments. Status Pending means waiting for officer approval.

## 4. Approve loan (2 min)

`POST /api/loans/1/approve` then `GET /api/loans/1`

> POST changes state. Only Pending loans can be approved. Server returns 400 if rules fail.

## 5. Record payment (1 min)

`POST /api/payments` body: `{ "loanId": 2, "amount": 500 }`

> Payment row inserted with foreign key to loan.

## 6. Dapper + injection (2 min)

Open `LoanRepository.cs` — show `@LoanId`.

`GET /api/demo/safe-search?email=alice@example.com`

> Parameters prevent SQL injection. Unsafe endpoint is for classroom demo only.

## Closing (30 sec)

> Code is on GitHub. Next: Blazor UI, authorization roles, and a nightly overdue-loan background job.
