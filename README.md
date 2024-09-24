# Retail Application API

This is a simple API for managing products in a retail application. It includes CRUD operations, an approval queue, and business logic for handling product updates, deletions, and creations.

## Features

- View and search active products.
- Create, update, and delete products with approval logic.
- Approval queue management.
- Unit tests for business logic.

## Technology Stack

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQL Server (or SQLite for local testing)
- xUnit and Moq for unit testing

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html) for local development

## Install dotnet-ef tool using `dotnet tool install --global dotnet-ef`
## How to Run

1. Clone this repository.
2. Install the necessary packages via `dotnet restore`.
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.8
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8


3. Update the connection string in `appsettings.json` to point to your database.

4. Run the database migrations using `dotnet ef migrations add InitialCreate` and `dotnet ef database update`.

5. Build the application using `dotnet build`

6. Run the application using `dotnet run`.

7. Swagger will be available at `http://localhost:5000/swagger/index.html`.

## Testing

Run the unit tests using the following command:

```bash
dotnet test
