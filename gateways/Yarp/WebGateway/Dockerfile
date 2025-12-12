# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["gateways/WebGateway/WebGateway.csproj", "gateways/WebGateway/"]
COPY ["shared/Hosting.Gateways/Shared.Hosting.Gateways.csproj", "shared/Hosting.Gateways/"]
COPY ["shared/Hosting.Aspire/Shared.Hosting.Aspire.csproj", "shared/Hosting.Aspire/"]
COPY ["shared/Hosting.AspNetCore/Shared.Hosting.AspNetCore.csproj", "shared/Hosting.AspNetCore/"]
COPY ["shared/Hosting/Shared.Hosting.csproj", "shared/Hosting/"]
COPY ["shared/Shared.ConfigurationCenter/Shared.ConfigurationCenter.csproj", "shared/Shared.ConfigurationCenter/"]
COPY ["shared/Shared.Definition/Shared.Definition.csproj", "shared/Shared.Definition/"]
RUN dotnet restore "./gateways/WebGateway/WebGateway.csproj"
COPY . .
WORKDIR "/src/gateways/WebGateway"
RUN dotnet build "./WebGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebGateway.dll"]