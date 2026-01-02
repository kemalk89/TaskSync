# If not already done, install dotnet-ef globally
# dotnet tool install --global dotnet-ef

# To see generated SQL only, without database interaction, you can run: 
# dotnet ef migrations add TmpPreview --project TaskSync.Infrastructure --startup-project TaskSync
# dotnet ef migrations script 0 --project TaskSync > schema.sql

dotnet ef database update 0 --project TaskSync
dotnet ef migrations remove --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef migrations add InitialCreate --project TaskSync.Infrastructure --startup-project TaskSync
dotnet ef database update --project TaskSync.Infrastructure --startup-project TaskSync