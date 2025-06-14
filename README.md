![build project](https://github.com/kemalk89/TaskSync/actions/workflows/dotnet.yml/badge.svg)

# Development

## How to run project
For local development we need a developer certificate which can be setup with ``dotnet dev-certs https --trust``.

1. Create `.env` file by duplicating `.env_template` located in project root

2. Start infrastructure (database etc) ``docker compose up -d``

3. Start the application
    - Option 1 - CLI: ``dotnet run --project TaskSync``
    - Option 2 - VSCode: Hit <kbd>F5</kbd>
4. Open the application
    - Swagger-UI: https://localhost:7190/swagger/index.html
    - Database-UI: http://localhost:8081/?pgsql=db&username=postgres&db=TaskSync&ns=public (pw: example)

## VSCode Settings
In this solution we are going to use file-scoped namespaces which is a new feature since C# 10.
In VSCode we can enable that in settings: `csharpextensions.useFileScopedNamespace`.

## Run with Docker
```shell
docker build -t tasksync -f Dockerfile .
docker run -it -p 8080:8080 --rm tasksync 
```

# Database Development
This scripts allows to update the initial migration script. 
```sh
# If not already done, install dotnet-ef globally
dotnet tool install --global dotnet-ef

dotnet ef database update 0 --project TaskSync
dotnet ef migrations remove --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef migrations add InitialCreate --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef database update --project TaskSync.Infrastructure --startup-project TaskSync
```

# Build the project
```sh
dotnet build TaskSync.sln
```

# Testing
## Manual Testing with SwaggerUI
### Auth0
Generate token:
```sh
curl --request POST \
--url 'https://dev-ng4mbx4gds2o61r1.eu.auth0.com/oauth/token' \
--header 'content-type: application/x-www-form-urlencoded' \
--data grant_type=client_credentials \
--data client_id=<CLIENT_ID> \
--data client_secret=<CLIENT_SECRET> \
--data audience=https://tasksync.api.de/api \
--data scope=openid
```
Copy the token and authorize in Swagger UI.

# DevOps
## Health Check Endpoint
The application exposes a basic health check endpoint used for monitoring and readiness probes:
```GET /api/health```

# Improvement Suggestions
- Use [SecretManager for development](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows)
