# NannyServices API

Minimal .NET 9 REST API for managing customers, products and orders.

## Stack
• .NET 9, Minimal API, MediatR  
• Entity Framework Core 9 (SQL Server / In-Memory)  
• FluentValidation  
• xUnit + FluentAssertions  
• Docker / Docker Compose


## API overview
The service exposes a minimal REST interface.

Main resources:
| Resource | Endpoints |
|----------|-----------|
| Customers | `GET /customers`, `GET /customers/{id}`, `POST /customers`, `PUT /customers/{id}`, `DELETE /customers/{id}`, `GET /customers/search`, customer reports `/customers/{id}/reports` |
| Products  | `GET /products`, `GET /products/{id}`, `POST /products`, `PUT /products/{id}`, `DELETE /products/{id}`, `GET /products/search` |
| Orders    | `GET /orders`, `GET /orders/{id}`, `POST /orders`, `PUT /orders/{id}/status`, `DELETE /orders/{id}` |
| Order lines | `POST /orders/{id}/order-lines`, `PUT /orders/{id}/order-lines`, `DELETE /orders/{id}/order-lines/{lineId}` |

All list endpoints support pagination (`page`, `pageSize`). Validation errors are returned as RFC-7807 *problem+json* responses.

### Features
* Full CRUD for customers, products and orders
* Order status workflow (Created → InProgress → Completed / Cancelled)
* Customer reports for arbitrary date range, week and month
* FluentValidation with automatic error formatting
* Clean Architecture: Domain / Application / Infrastructure / API
* Integration tests spinning up the API with an in-memory DB


## Run with Docker
```bash
# build & start API + SQL Server
docker compose up --build -d
# stop
docker compose down
```
• Swagger: http://localhost:5000/swagger  
• SQL Server: localhost:1433 | user: sa | pwd: Your_password123

## Run locally
```bash
# start API with local in-memory DB
 dotnet run --project src/NannyServices.Api
```
If *DefaultConnection* is set, SQL Server will be used automatically.

## Tests
```bash
dotnet test
```

## EF Core migrations (optional)
```bash
# add new migration
dotnet ef migrations add <Name> \
  --project src/NannyServices.Infrastructure \
  --startup-project src/NannyServices.Api

# apply to database
dotnet ef database update \
  --project src/NannyServices.Infrastructure \
  --startup-project src/NannyServices.Api
```