# Project Structure
This solution was created cia CLI using the following commands:
```sh
mkdir Times
cd Times

dotnet new sln -n Times

dotnet new react -o Times
dotnet new classlib -o Times.Common -f net6.0
dotnet new classlib -o Times.Domain -f net6.0
dotnet new classlib -o Times.Infrastructure -f net6.0
dotnet new nunit -o Times.Tests -f net6.0

dotnet sln Times.sln add Times/Times.csproj
dotnet sln Times.sln add Times.Domain/Times.Domain.csproj
dotnet sln Times.sln add Times.Common/Times.Common.csproj
dotnet sln Times.sln add Times.Infrastructure/Times.Infrastructure.csproj

dotnet add Times/Times.csproj reference Times.Domain/Times.Domain.csproj
dotnet add Times/Times.csproj reference Times.Infrastructure/Times.Infrastructure.csproj
dotnet add Times.Infrastructure/Times.Infrastructure.csproj reference Times.Domain/Times.Domain.csproj

dotnet add Times package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add Times package Microsoft.EntityFrameworkCore.Design
dotnet add Times.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add Times.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add Times package Swashbuckle.AspNetCore -v 6.2.3
dotnet add Times package Serilog.AspNetCore
```

* Times - Contains the React UI and the REST API
* Times.Domain - Contains the Domain Model
* Times.Common - Contains common / shared classes
* Times.Tests - Contains the Unit-Tests

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
* Frontend: https://localhost:7190/
* Swagger-UI: https://localhost:7190/swagger/index.html
* Database-UI: http://localhost:8081/?pgsql=db&username=postgres&db=times&ns=public (pw: example)
# Database Development
```sh
dotnet ef migrations add InitialCreate --project Times.Infrastructure --startup-project Times
dotnet ef database update --project Times.Infrastructure --startup-project Times
```

# Troubleshooting
## Microsoft.AspNetCore.SpaProxy.SpaProxyMiddleware: Information: SPA proxy is not ready. Returning temporary landing page
The cause of this proplem could be that you have installed NodeJs using third-party tools like `nvm`. For some editors like Visual Studio for Mac the commands `node` and `npm` are not available. In Visual Studio Code this issue does not occur. To fix that we have to set `symlinks`:
```sh
sudo ln -s $(which node) /usr/local/bin/node
sudo ln -s $(which npm) /usr/local/bin/npm
```