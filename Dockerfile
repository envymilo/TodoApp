# Use the official .NET 8.0 runtime as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Use the .NET 8.0 SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore dependencies
COPY ["TodoApp.API/TodoApp.API.csproj", "TodoApp.API/"]
COPY ["TodoApp.Core/TodoApp.Core.csproj", "TodoApp.Core/"]
COPY ["TodoApp.Infrastructure/TodoApp.Infrastructure.csproj", "TodoApp.Infrastructure/"]

RUN dotnet restore "TodoApp.API/TodoApp.API.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the application
WORKDIR "/src/TodoApp.API"
RUN dotnet publish -c Release -o /app/publish

# Final stage: run the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.API.dll"]
