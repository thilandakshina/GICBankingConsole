FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Test/GICBankingSystem.Core.API.Tests/GICBankingSystem.Core.API.Tests.csproj", "src/Test/GICBankingSystem.Core.API.Tests/"]
COPY ["src/Libraries/GICBankingSystem.Shared/GICBankingSystem.Shared.csproj", "src/Libraries/GICBankingSystem.Shared/"]
COPY ["src/Services/Core/GICBankingSystem.Core.API/GICBankingSystem.Core.API.csproj", "src/Services/Core/GICBankingSystem.Core.API/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Application/GICBankingSystem.Core.Application.csproj", "src/Services/Core/GICBankingSystem.Core.Application/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Domain/GICBankingSystem.Core.Domain.csproj", "src/Services/Core/GICBankingSystem.Core.Domain/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Infrastructure/GICBankingSystem.Core.Infrastructure.csproj", "src/Services/Core/GICBankingSystem.Core.Infrastructure/"]

RUN dotnet restore "./src/Test/GICBankingSystem.Core.API.Tests/GICBankingSystem.Core.API.Tests.csproj"
COPY . .

WORKDIR "/src/src/Test/GICBankingSystem.Core.API.Tests"
RUN dotnet build "GICBankingSystem.Core.API.Tests.csproj" -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "test", "--no-build", "--verbosity", "normal"]