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
COPY ["src/Services/Core/GICBankingSystem.Core.API/GICBankingSystem.Core.API.csproj", "src/Services/Core/GICBankingSystem.Core.API/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Application/GICBankingSystem.Core.Application.csproj", "src/Services/Core/GICBankingSystem.Core.Application/"]
COPY ["src/Libraries/GICBankingSystem.Shared/GICBankingSystem.Shared.csproj", "src/Libraries/GICBankingSystem.Shared/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Domain/GICBankingSystem.Core.Domain.csproj", "src/Services/Core/GICBankingSystem.Core.Domain/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Infrastructure/GICBankingSystem.Core.Infrastructure.csproj", "src/Services/Core/GICBankingSystem.Core.Infrastructure/"]
RUN dotnet restore "./src/Services/Core/GICBankingSystem.Core.API/GICBankingSystem.Core.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Core/GICBankingSystem.Core.API"
RUN dotnet build "./GICBankingSystem.Core.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GICBankingSystem.Core.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GICBankingSystem.Core.API.dll"]