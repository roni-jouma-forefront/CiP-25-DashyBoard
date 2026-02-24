# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["DashyBoard.sln", "."]

# Copy project files
COPY ["backend/DashyBoard.API/DashyBoard.API.csproj", "backend/DashyBoard.API/"]
COPY ["backend/DashyBoard.Application/DashyBoard.Application.csproj", "backend/DashyBoard.Application/"]
COPY ["backend/DashyBoard.Domain/DashyBoard.Domain.csproj", "backend/DashyBoard.Domain/"]
COPY ["backend/DashyBoard.Infrastructure/DashyBoard.Infrastructure.csproj", "backend/DashyBoard.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "backend/DashyBoard.API/DashyBoard.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/backend/DashyBoard.API"
RUN dotnet build "DashyBoard.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DashyBoard.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Create data directory for SQLite database
RUN mkdir -p /app/data

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DashyBoard.API.dll"]
