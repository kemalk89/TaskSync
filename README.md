# Development

## How to run project
1. Start infrastructure (database etc) ``docker compose up -d``

2. Start the application
    - Option 1 - CLI: ``dotnet run --project Times``

    - Option 2 - VSCode: Hit <kbd>F5</kbd>
3. Open the application
    - Frontend: https://localhost:7190/
    - Swagger-UI: https://localhost:7190/swagger/index.html
    - Database-UI: http://localhost:8081/?pgsql=db&username=postgres&db=times&ns=public (pw: example)

## VSCode Settings
In this solution we are going to use file-scoped namespaces which is a new feature since C# 10.
In VSCode we can enable that in settings: `csharpextensions.useFileScopedNamespace`.

# Database Development

```sh
dotnet ef database update 0 --project Times
dotnet ef migrations remove --project Times.Infrastructure --startup-project Times
dotnet ef migrations add InitialCreate --project Times.Infrastructure --startup-project Times
dotnet ef database update --project Times.Infrastructure --startup-project Times
```
