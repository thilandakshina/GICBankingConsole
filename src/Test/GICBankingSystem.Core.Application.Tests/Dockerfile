FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Test/GICBankingSystem.Core.Application.Tests/GICBankingSystem.Core.Application.Tests.csproj", "src/Test/GICBankingSystem.Core.Application.Tests/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Application/GICBankingSystem.Core.Application.csproj", "src/Services/Core/GICBankingSystem.Core.Application/"]
COPY ["src/Libraries/GICBankingSystem.Shared/GICBankingSystem.Shared.csproj", "src/Libraries/GICBankingSystem.Shared/"]
COPY ["src/Services/Core/GICBankingSystem.Core.Domain/GICBankingSystem.Core.Domain.csproj", "src/Services/Core/GICBankingSystem.Core.Domain/"]

RUN dotnet restore "./src/Test/GICBankingSystem.Core.Application.Tests/GICBankingSystem.Core.Application.Tests.csproj"
COPY . .

WORKDIR "/src/src/Test/GICBankingSystem.Core.Application.Tests"
RUN dotnet build "GICBankingSystem.Core.Application.Tests.csproj" -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "test", "--no-build", "--verbosity", "normal"]