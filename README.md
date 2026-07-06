# Meijer Products — Backend API

.NET 10 Web API exposing the two product endpoints over a real (SQLite) persistence layer, plus unit tests.

This is the **backend half** of the assessment. The MAUI app is shipped separately and talks to this API over HTTP.

## Projects

```
MeijerProducts.Backend.sln
├── src/MeijerProducts.Api/          ASP.NET Core Web API
│   ├── Controllers/                 ProductsController (thin HTTP layer)
│   ├── Services/                    IProductService / ProductService (app logic + mapping)
│   ├── Repositories/                IProductRepository / ProductRepository (EF Core)
│   ├── Data/                        DbContext, seeder, Seed/products-seed.json
│   ├── Models/Entities/             Product (persistence entity)
│   ├── Models/Dtos/                 ProductSummaryDto, ProductDetailDto (wire shapes)
│   └── Mapping/                     Entity → DTO projections
└── tests/MeijerProducts.Api.Tests/  xUnit + Moq + EF InMemory + FluentAssertions
```

## Architecture

Layered, dependencies point inward: `Controller → Service → Repository → EF Core DbContext`. Each layer is behind an interface so responsibilities stay separated and each is independently testable.

- **Controller** — HTTP only (routing, status codes, `ProblemDetails`).
- **Service** — application logic; maps entities to DTOs.
- **Repository** — the only code aware of EF Core.
- **DTOs vs. entity** — the stored `Product` is kept separate from the wire DTOs so storage and API shapes can evolve independently. The list DTO is lightweight (`id, title, summary, imageUrl`); the detail DTO adds `description` and `price`.

## Endpoints

| Method | Route                | Purpose              | Success | Not found |
|--------|----------------------|----------------------|---------|-----------|
| GET    | `/api/products`      | Product summary list | 200     | —         |
| GET    | `/api/products/{id}` | Full product detail  | 200     | 404 (`ProblemDetails`) |

Swagger UI is served at the site root in Development.

## Run

Requires the .NET 10 SDK.

```bash
cd src/MeijerProducts.Api
dotnet run
```

- Listens on **http://localhost:5080** (`Properties/launchSettings.json`).
- On first run it creates `products.db` (SQLite) and **seeds it from `Data/Seed/products-seed.json`**. Seeding is idempotent.
- Try it: open **http://localhost:5080/swagger**, use `MeijerProducts.Api.http`, or:

```bash
curl http://localhost:5080/api/products
curl http://localhost:5080/api/products/0
curl -i http://localhost:5080/api/products/9999   # 404 ProblemDetails
```

## Tests

```bash
dotnet test
```

## Notes / decisions

- **EF Core + SQLite** — a genuine file-backed relational store that needs no external server. Switching to SQL Server is a one-line change in `Program.cs` (`UseSqlServer`) plus a connection string.
- **`EnsureCreated()`** keeps the exercise simple; production would use checked-in EF **migrations**.
- The seed file (`products-seed.json`) merges the two provided sample files. Each product keeps **both** image URLs — `imageUrl` (list thumbnail) and `detailImageUrl` (full detail image) — because the samples ship different assets for each view.

## Connecting the app

The MAUI app defaults to `http://localhost:5080` (and `http://10.0.2.2:5080` on the Android emulator). Keep this API running before launching the app. CORS is open in Development.

See `AI-USAGE.md` for the AI-assistance disclosure required by the assessment.

