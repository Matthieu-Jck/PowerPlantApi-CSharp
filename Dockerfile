FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Restore dependencies
COPY PowerPlantApi/PowerPlantApi.csproj PowerPlantApi/
RUN dotnet restore PowerPlantApi/PowerPlantApi.csproj

# Build the application
COPY PowerPlantApi/ PowerPlantApi/
RUN dotnet publish PowerPlantApi/PowerPlantApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8888

ENTRYPOINT ["dotnet", "PowerPlantApi.dll"]
