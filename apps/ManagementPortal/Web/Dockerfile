FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "MyCompanyName.FinanceServices.sln" "MyCompanyName.FinanceServices.sln"

# Applications
COPY "apps/auth-server/src/MyCompanyName.FinanceServices.AuthServer/MyCompanyName.FinanceServices.AuthServer.csproj" "apps/auth-server/src/MyCompanyName.FinanceServices.AuthServer/MyCompanyName.FinanceServices.AuthServer.csproj"
COPY "apps/public-web/src/MyCompanyName.FinanceServices.PublicWeb/MyCompanyName.FinanceServices.PublicWeb.csproj" "apps/public-web/src/MyCompanyName.FinanceServices.PublicWeb/MyCompanyName.FinanceServices.PublicWeb.csproj"
COPY *"apps/blazor/src/MyCompanyName.FinanceServices.Blazor/MyCompanyName.FinanceServices.Blazor.csproj" "apps/blazor/src/MyCompanyName.FinanceServices.Blazor/MyCompanyName.FinanceServices.Blazor.csproj"
COPY *"apps/blazor/src/MyCompanyName.FinanceServices.Blazor.Server/MyCompanyName.FinanceServices.Blazor.Server.csproj" "apps/blazor/src/MyCompanyName.FinanceServices.Blazor.Server/MyCompanyName.FinanceServices.Blazor.Server.csproj"
COPY *"apps/web/src/MyCompanyName.FinanceServices.Web/MyCompanyName.FinanceServices.Web.csproj" "apps/web/src/MyCompanyName.FinanceServices.Web/MyCompanyName.FinanceServices.Web.csproj"

# Shared Projects
COPY "shared/MyCompanyName.FinanceServices.DbMigrator/MyCompanyName.FinanceServices.DbMigrator.csproj" "shared/MyCompanyName.FinanceServices.DbMigrator/MyCompanyName.FinanceServices.DbMigrator.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Hosting/MyCompanyName.FinanceServices.Shared.Hosting.csproj" "shared/MyCompanyName.FinanceServices.Shared.Hosting/MyCompanyName.FinanceServices.Shared.Hosting.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Hosting.Gateways/MyCompanyName.FinanceServices.Shared.Hosting.Gateways.csproj" "shared/MyCompanyName.FinanceServices.Shared.Hosting.Gateways/MyCompanyName.FinanceServices.Shared.Hosting.Gateways.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Hosting.Microservices/MyCompanyName.FinanceServices.Shared.Hosting.Microservices.csproj" "shared/MyCompanyName.FinanceServices.Shared.Hosting.Microservices/MyCompanyName.FinanceServices.Shared.Hosting.Microservices.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Hosting.AspNetCore/MyCompanyName.FinanceServices.Shared.Hosting.AspNetCore.csproj" "shared/MyCompanyName.FinanceServices.Shared.Hosting.AspNetCore/MyCompanyName.FinanceServices.Shared.Hosting.AspNetCore.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Hosting/MyCompanyName.FinanceServices.Shared.Hosting.csproj" "shared/MyCompanyName.FinanceServices.Shared.Hosting/MyCompanyName.FinanceServices.Shared.Hosting.csproj"
COPY "shared/MyCompanyName.FinanceServices.Shared.Localization/MyCompanyName.FinanceServices.Shared.Localization.csproj" "shared/MyCompanyName.FinanceServices.Shared.Localization/MyCompanyName.FinanceServices.Shared.Localization.csproj"

# Microservices
COPY "services/administration/src/MyCompanyName.FinanceServices.AdministrationService.HttpApi.Host/MyCompanyName.FinanceServices.AdministrationService.HttpApi.Host.csproj" "services/administration/src/MyCompanyName.FinanceServices.AdministrationService.HttpApi.Host/MyCompanyName.FinanceServices.AdministrationService.HttpApi.Host.csproj"
COPY "services/identity/src/MyCompanyName.FinanceServices.IdentityService.HttpApi.Host/MyCompanyName.FinanceServices.IdentityService.HttpApi.Host.csproj" "services/identity/src/MyCompanyName.FinanceServices.IdentityService.HttpApi.Host/MyCompanyName.FinanceServices.IdentityService.HttpApi.Host.csproj"
COPY "services/saas/src/MyCompanyName.FinanceServices.SaasService.HttpApi.Host/MyCompanyName.FinanceServices.SaasService.HttpApi.Host.csproj" "services/saas/src/MyCompanyName.FinanceServices.SaasService.HttpApi.Host/MyCompanyName.FinanceServices.SaasService.HttpApi.Host.csproj"
COPY "services/product/src/FinanceServices.Services.FinanceServicesService.HttpApi.Host/FinanceServices.Services.FinanceServicesService.HttpApi.Host.csproj" "services/product/src/FinanceServices.Services.FinanceServicesService.HttpApi.Host/FinanceServices.Services.FinanceServicesService.HttpApi.Host.csproj"
# Gateways
COPY "gateways/web/src/MyCompanyName.FinanceServices.WebGateway/MyCompanyName.FinanceServices.WebGateway.csproj" "gateways/web/src/MyCompanyName.FinanceServices.WebGateway/MyCompanyName.FinanceServices.WebGateway.csproj"
COPY "gateways/web-public/src/MyCompanyName.FinanceServices.PublicWebGateway/MyCompanyName.FinanceServices.PublicWebGateway.csproj" "gateways/web-public/src/MyCompanyName.FinanceServices.PublicWebGateway/MyCompanyName.FinanceServices.PublicWebGateway.csproj"

COPY "NuGet.Config" "NuGet.Config"

RUN dotnet restore "MyCompanyName.FinanceServices.sln"

COPY . .
WORKDIR "/src/apps/web/src/MyCompanyName.FinanceServices.Web"
RUN dotnet publish -c Release -o /app
# Should be used after .net6 is out of preview for better performance
# RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyCompanyName.FinanceServices.Web.dll"]
