# Development

## How to run project
For local development we need a developer certificate which can be setup with ``dotnet dev-certs https --trust``.

1. Start infrastructure (database etc) ``docker compose up -d``

2. Start the application
    - Option 1 - CLI: ``dotnet run --project TaskSync``

    - Option 2 - VSCode: Hit <kbd>F5</kbd>
3. Open the application
    - Swagger-UI: https://localhost:7190/swagger/index.html
    - Database-UI: http://localhost:8081/?pgsql=db&username=postgres&db=TaskSync&ns=public (pw: example)

## VSCode Settings
In this solution we are going to use file-scoped namespaces which is a new feature since C# 10.
In VSCode we can enable that in settings: `csharpextensions.useFileScopedNamespace`.

# Database Development
This scripts allows to update the initial migration script. 
```sh
dotnet ef database update 0 --project TaskSync
dotnet ef migrations remove --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef migrations add InitialCreate --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef database update --project TaskSync.Infrastructure --startup-project TaskSync
```
