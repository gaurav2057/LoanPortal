# Loan Portal

A learning and demo project: **customers**, **loans**, and **payments** managed through an **ASP.NET Core Web API** with **Dapper** and **SQL Server**.

**Author:** [gaurav2057](https://github.com/gaurav2057)

## Tech stack

| Layer | Technology |
|-------|------------|
| API | ASP.NET Core 8 Web API |
| Data access | Dapper (parameterized SQL) |
| Database | SQL Server 2022 (Docker) |
| API testing UI | Swagger |

## Features (Phase 1)

- List customers and loans (SQL JOIN with payment totals)
- Create loans (status starts as `Pending`)
- Approve loans (`Pending` → `Active`)
- Record payments
- Demo endpoints: unsafe vs safe SQL search (OWASP injection lesson)

## Project structure

```
LoanPortal/
├── database/           # SQL scripts (tables + seed data)
├── docker-compose.yml  # SQL Server container
├── docs/               # Architecture + demo script
└── src/LoanPortal.Api/
    ├── Controllers/    # HTTP endpoints
    ├── Repositories/   # Dapper + SQL
    ├── Models/         # C# classes matching tables
    └── Data/           # Connection factory + DB bootstrap
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for SQL Server)

## Quick start

### 1. Start SQL Server

```bash
cd LoanPortal
docker compose up -d
```

Wait ~30 seconds for SQL Server to be ready.

### 2. Run the API

```bash
export PATH="$HOME/.dotnet:$PATH"   # if dotnet installed to ~/.dotnet
cd src/LoanPortal.Api
dotnet run
```

### 3. Open Swagger

Browser: `https://localhost:7xxx/swagger` (port shown in terminal output)

## Demo flow (Swagger)

1. `GET /api/loans` — see Alice (Pending), Bob (Active), Carol (Closed)
2. `POST /api/loans/{id}/approve` — approve Alice's loan
3. `GET /api/loans/{id}` — confirm status is Active
4. `POST /api/payments` — `{ "loanId": 2, "amount": 500 }`
5. `GET /api/demo/safe-search?email=alice@example.com` — parameterized SQL

## API endpoints

| Method | URL | Description |
|--------|-----|-------------|
| GET | `/api/customers` | All customers |
| GET | `/api/customers/{id}` | One customer |
| GET | `/api/loans` | All loans with customer name |
| GET | `/api/loans/{id}` | Loan + customer + payments |
| POST | `/api/loans` | Create loan |
| POST | `/api/loans/{id}/approve` | Approve pending loan |
| POST | `/api/payments` | Record payment |
| GET | `/api/demo/unsafe-search` | SQL injection demo (local only) |
| GET | `/api/demo/safe-search` | Safe parameterized search |

## Connection string

Default in `appsettings.json` (Docker SQL Server):

```
Server=localhost,1433;Database=LoanPortal;User Id=sa;Password=LoanPortal@123;TrustServerCertificate=True;
```

**Do not commit real production passwords.** For GitHub this dev password is documented for local learning only.

## Push to GitHub

```bash
cd LoanPortal
git init
git add .
git commit -m "Phase 1: Loan Portal API with Dapper and SQL Server"
git branch -M main
git remote add origin https://github.com/gaurav2057/LoanPortal.git
git push -u origin main
```

Create the empty repo on GitHub first: https://github.com/new → name `LoanPortal`

## What I learned

- SQL tables, foreign keys, JOINs, GROUP BY
- Dapper `QueryAsync` / `ExecuteAsync` with `@parameters`
- REST API: GET vs POST, status codes
- Dependency injection in ASP.NET Core
- Swagger for testing without a UI

## Roadmap

- **Phase 2:** Blazor dashboard + role-based approve button
- **Phase 3:** Expand security lab (injection demos)
- **Phase 4:** BackgroundService for overdue loan alerts
