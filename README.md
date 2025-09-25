![build project](https://github.com/kemalk89/TaskSync/actions/workflows/dotnet.yml/badge.svg)
[![Quality gate](https://sonarcloud.io/api/project_badges/quality_gate?project=kemalk89_TaskSync)](https://sonarcloud.io/summary/new_code?id=kemalk89_TaskSync)

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
    - Database-Explorer: Use a tool like pgAdmin - username=postgres, pw=example, db=TaskSync

## Run with Docker
```shell
docker compose up -d
docker build -t tasksync -f Dockerfile_Local .
docker run -it -p 7190:7190 --name tasksync-container-local tasksync
```

# Database Development
This scripts allows to update the initial migration script. 
```sh
sh _reset_db.sh
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
