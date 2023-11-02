FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . /app
RUN dotnet publish src/BoilerPlate.App.API/BoilerPlate.App.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BoilerPlate.App.API.dll"]
