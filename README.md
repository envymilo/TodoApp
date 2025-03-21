# TodoApp
Setup Instructions

**Without Docker (Code First)**

Clone the repo:

Update appsettings.json with your SQL Server details:

Run migrations & start the API:

dotnet ef migrations add InitialCreate --startup-project ../TodoApp.API --project ../TodoApp.Infrastructure (If there is no migration yet)

dotnet ef database update --startup-project ../TodoApp.API --project ../TodoApp.Infrastructure

**With Docker**

Run with Docker Compose:

API runs at: http://localhost:5000SQL Server runs at: localhost:1433
