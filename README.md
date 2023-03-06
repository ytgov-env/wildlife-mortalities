# Wildlife Mortalities

This service is used to manage wildlife mortality data collected by Yukon's Department of Environment. This includes [hunted and trapped harvest](https://yukon.ca/en/outdoor-recreation-and-wildlife/hunting-and-trapping/report-harvest-results) mortalities, [human-wildlife conflict](https://yukon.ca/en/report-human-wildlife-conflict) mortalities, research mortalities, and collared wildlife mortalities. Additionally, this service handles all deterministic checks for legality of reported harvests, preceeding any investigation by Conservation Officers (if required).

## Stack

* UI Component Library: [MudBlazor](https://github.com/MudBlazor/MudBlazor)
* Web Framework: [ASP.NET Core 6](https://github.com/dotnet/aspnetcore) and [Blazor Server](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
* Authentication: OpenID Connect
* Data Access: [Entity Framework Core](https://github.com/dotnet/efcore)
* Data Store: SQL Server
* Logging: [Serilog](https://github.com/serilog/serilog)

## How do I run this?

See <https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations> if you are new to Entity Framework Core.

Ensure that you have LocalDB installed, or override the "AppDbContext" connection string in appsettings.json

### Build the project and restore packages

Using the .NET CLI:

```bash
dotnet build
```

### Create and seed the database with test data

.NET CLI:

```bash
dotnet ef database update
```

Or, using VS Package Manager Console (PowerShell):

```pwsh
Update-Database
```

### Run the app

.NET CLI:

```bash
dotnet run
```
