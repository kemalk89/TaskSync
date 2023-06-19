# Project Structure

- Times - Contains the React UI and the REST API
- Times.Domain - Contains the Domain Model
- Times.Common - Contains common / shared classes
- Times.Tests - Contains the Unit-Tests

# Development

## Database

To spin up the database run:

```sh
docker compose up -d
```

## Editor Settings

In this solution we are going to use file-scoped namespaces which is a new feature since C# 10.
In Visual Studio Code we can enable that in settings: `csharpextensions.useFileScopedNamespace`.

## Run the app

To run the app you have following options:

#### Option 1 - CLI

```sh
dotnet run --project Times
```

#### Option 2 - Visual Studio Code

Hit <kbd>F5</kbd>.

Finally you can visit following pages:

- Frontend: https://localhost:7190/
- Swagger-UI: https://localhost:7190/swagger/index.html
- Database-UI: http://localhost:8081/?pgsql=db&username=postgres&db=times&ns=public (pw: example)

# Database Development

```sh
dotnet ef database update 0 --project Times
dotnet ef migrations remove --project Times.Infrastructure --startup-project Times
dotnet ef migrations add InitialCreate --project Times.Infrastructure --startup-project Times
dotnet ef database update --project Times.Infrastructure --startup-project Times
```
