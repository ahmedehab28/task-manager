# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# -------------------------
# Base image for runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only csproj files first (for restore caching)
COPY src/WebApi/WebApi.csproj WebApi/
COPY src/Application/Application.csproj Application/
COPY src/Infrastructure/Infrastructure.csproj Infrastructure/
COPY src/Domain/Domain.csproj Domain/

# Restore dependencies
RUN dotnet restore "./WebApi/WebApi.csproj"

# Copy the rest of the source code FROM the src/ folder (not the entire context root)
COPY src/ .

# Build the WebApi project
WORKDIR /src/WebApi
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build


# -------------------------
# Publish stage
# -------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------------------------
# Final runtime image
# -------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
