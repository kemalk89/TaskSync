This solution was created via CLI using the following commands:
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

dotnet add Times package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add Times package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add Times package Microsoft.EntityFrameworkCore.Design
dotnet add Times.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add Times.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add Times package Swashbuckle.AspNetCore -v 6.2.3
dotnet add Times package Serilog.AspNetCore
```