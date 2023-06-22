# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy .csproj and restore as distinct layers
COPY "HFC.sln" "HFC.sln"
COPY "API/API.csproj" "API/API.csproj"
COPY "Infrastructure/Infrastructure.csproj" "Infrastructure/Infrastructure.csproj"
COPY "Application/Application.csproj" "Application/Application.csproj"
COPY "Domain/Domain.csproj" "Domain/Domain.csproj"
COPY "Persistence/Persistence.csproj" "Persistence/Persistence.csproj"


RUN dotnet restore "HFC.sln"

# Copy the remaining source code and build the application
COPY . .
RUN dotnet publish -c Release -o out

# Test stage
FROM build-env AS test-env
WORKDIR /app
RUN dotnet test

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "API.dll"]

