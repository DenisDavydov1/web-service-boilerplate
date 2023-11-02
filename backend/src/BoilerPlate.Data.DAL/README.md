```bash
cd BoilerPlate.App.API
```

```bash
ASPNETCORE_ENVIRONMENT=Local dotnet ef migrations add Initial --project ../BoilerPlate.Data.DAL/BoilerPlate.Data.DAL.csproj --context BoilerPlateDbContext
```

```bash
ASPNETCORE_ENVIRONMENT=Local dotnet ef database update --context BoilerPlateDbContext
```
