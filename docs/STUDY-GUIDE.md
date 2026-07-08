# Study guide — read this file by file

Use this while exploring the repo. Say each section aloud for demo practice.

---

## 1. `database/01-create-tables.sql`

**What:** Creates Customers, Loans, Payments tables.  
**Why:** Data must live in SQL Server, not in C# memory.  
**Key ideas:** Primary key, foreign key, CHECK constraint on Status.  
**Demo line:** "Loans reference CustomerId — that is normalization."

---

## 2. `database/02-seed-data.sql`

**What:** Inserts Alice, Bob, Carol and sample loans.  
**Why:** Swagger demo works immediately.  
**Demo line:** "Alice has a Pending loan — we will approve it live."

---

## 3. `docker-compose.yml`

**What:** Runs SQL Server in Docker on port 1433.  
**Why:** Same engine your company uses, no full Windows install required.  
**Command:** `docker compose up -d`

---

## 4. `Models/*.cs`

**What:** C# classes matching table columns.  
**Why:** Dapper maps SQL rows to objects.  
**Demo line:** "LoanId property maps to LoanId column."

---

## 5. `Data/SqlConnectionFactory.cs`

**What:** Reads connection string from appsettings.json.  
**Why:** One place to configure database; injected everywhere.  
**Demo line:** "Connection string points to localhost SQL Server."

---

## 6. `Data/DatabaseBootstrapper.cs`

**What:** On startup, creates DB and runs SQL scripts.  
**Why:** You do not need SSMS for first run.  
**Demo line:** "App self-initializes schema for local dev."

---

## 7. `Repositories/*.cs`

**What:** SQL queries using Dapper.  
**Why:** Controllers stay thin; SQL stays in one layer.  
**Key line to show:**

```csharp
WHERE LoanId = @LoanId
```

**Demo line:** "@LoanId is a parameter — safe from SQL injection."

---

## 8. `Controllers/*.cs`

**What:** HTTP endpoints — URL to C# method.  
**Why:** REST API for browsers, Swagger, future Blazor.  

| Controller | Role |
|------------|------|
| CustomersController | GET customers |
| LoansController | GET/POST loans, approve |
| PaymentsController | POST payment |
| DemoController | unsafe vs safe search |

**Demo line:** "GET reads data; POST creates or changes state."

---

## 9. `Program.cs`

**What:** App entry point — DI registration, Swagger, bootstrap.  
**Why:** ASP.NET Core pipeline starts here.  
**Demo line:** "I register ILoanRepository so the controller gets it automatically."

---

## 10. `appsettings.json`

**What:** Connection strings and logging.  
**Why:** Config outside code.  
**Note:** Dev password only — use secrets in real production.

---

## Practice order

1. Read SQL scripts  
2. Read Models  
3. Read one Repository + matching Controller  
4. Read Program.cs  
5. Run app and match Swagger calls to code paths
