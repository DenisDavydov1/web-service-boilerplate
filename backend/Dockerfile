FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . /app
RUN dotnet publish src/BoilerPlate.App.API/BoilerPlate.App.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_HTTP_PORTS=80
ENTRYPOINT ["dotnet", "BoilerPlate.App.API.dll"]
