# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TH.WebAPI/TH.WebAPI.csproj", "TH.WebAPI/"]
COPY ["Service/Asset/TH.Asset.ApplicationService/TH.Asset.ApplicationService.csproj", "Service/Asset/TH.Asset.ApplicationService/"]
COPY ["Service/Shared/TH.Constant/TH.Constant.csproj", "Service/Shared/TH.Constant/"]
COPY ["Service/Shared/TH.Shared.ApplicationService/TH.Shared.ApplicationService.csproj", "Service/Shared/TH.Shared.ApplicationService/"]
COPY ["Service/Asset/TH.Asset.Domain/TH.Asset.Domain.csproj", "Service/Asset/TH.Asset.Domain/"]
COPY ["Service/Asset/TH.Asset.Dtos/TH.Asset.Dtos.csproj", "Service/Asset/TH.Asset.Dtos/"]
COPY ["Service/Asset/TH.Asset.Infrastructure/TH.Asset.Infrastructure.csproj", "Service/Asset/TH.Asset.Infrastructure/"]
COPY ["Service/Auth/TH.Auth.ApplicationService/TH.Auth.ApplicationService.csproj", "Service/Auth/TH.Auth.ApplicationService/"]
COPY ["Service/Auth/TH.Auth.Domain/TH.Auth.Domain.csproj", "Service/Auth/TH.Auth.Domain/"]
COPY ["Service/Auth/TH.Auth.Dtos/TH.Auth.Dtos.csproj", "Service/Auth/TH.Auth.Dtos/"]
COPY ["Service/Auth/TH.Auth.Infrastructure/TH.Auth.Infrastructure.csproj", "Service/Auth/TH.Auth.Infrastructure/"]
RUN dotnet restore "./TH.WebAPI/TH.WebAPI.csproj"
COPY . .
WORKDIR "/src/TH.WebAPI"
RUN dotnet build "./TH.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TH.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TH.WebAPI.dll"]