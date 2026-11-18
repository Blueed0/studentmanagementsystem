# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY StudentManagementSystem.csproj ./
RUN dotnet restore StudentManagementSystem.csproj

COPY . .
RUN dotnet publish StudentManagementSystem.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Interactive console menu needs a TTY
ENTRYPOINT ["dotnet", "StudentManagementSystem.dll"]
