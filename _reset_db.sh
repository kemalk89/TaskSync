# If not already done, install dotnet-ef globally
# dotnet tool install --global dotnet-ef

dotnet ef database update 0 --project TaskSync
dotnet ef migrations remove --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef migrations add InitialCreate --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef database update --project TaskSync.Infrastructure --startup-project TaskSync