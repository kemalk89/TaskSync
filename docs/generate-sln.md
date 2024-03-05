This solution was created via CLI using the following commands:
```sh
mkdir TaskSync
cd TaskSync

dotnet new sln -n TaskSync
dotnet new gitignore

dotnet new react -o TaskSync
dotnet new classlib -o TaskSync.Common -f net6.0
dotnet new classlib -o TaskSync.Domain -f net6.0
dotnet new classlib -o TaskSync.Infrastructure -f net6.0
dotnet new nunit -o TaskSync.Tests -f net6.0

dotnet sln TaskSync.sln add TaskSync/TaskSync.csproj
dotnet sln TaskSync.sln add TaskSync.Domain/TaskSync.Domain.csproj
dotnet sln TaskSync.sln add TaskSync.Common/TaskSync.Common.csproj
dotnet sln TaskSync.sln add TaskSync.Infrastructure/TaskSync.Infrastructure.csproj

dotnet add TaskSync/TaskSync.csproj reference TaskSync.Domain/TaskSync.Domain.csproj
dotnet add TaskSync/TaskSync.csproj reference TaskSync.Infrastructure/TaskSync.Infrastructure.csproj

dotnet add TaskSync.Infrastructure/TaskSync.Infrastructure.csproj reference TaskSync.Domain/TaskSync.Domain.csproj
dotnet add TaskSync.Infrastructure package Microsoft.AspNetCore.Http
dotnet add TaskSync.Infrastructure package RestSharp
dotnet add TaskSync.Infrastructure package Microsoft.Extensions.Logging
dotnet add TaskSync.Infrastructure package Newtonsoft.Json

dotnet add TaskSync package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add TaskSync package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add TaskSync package Microsoft.EntityFrameworkCore.Design
dotnet add TaskSync.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add TaskSync.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add TaskSync package Swashbuckle.AspNetCore -v 6.2.3
dotnet add TaskSync package Serilog.AspNetCore
```