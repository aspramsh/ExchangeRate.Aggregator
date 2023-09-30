FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExchangeRate.Aggregator.Modules.Parsers/ExchangeRate.Aggregator.Modules.Parsers.csproj", "ExchangeRate.Aggregator.Modules.Parsers/"]
COPY ["ExchangeRate.Aggregator.Modules.Parsers.Infrastructure/ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.csproj", "ExchangeRate.Aggregator.Modules.Parsers.Infrastructure/"]
COPY ["ExchangeRate.Aggregator.Modules.Parsers.Application/ExchangeRate.Aggregator.Modules.Parsers.Application.csproj", "ExchangeRate.Aggregator.Modules.Parsers.Application/"]
COPY ["ExchangeRate.Aggregator.Shared.Infrastructure/ExchangeRate.Aggregator.Shared.Infrastructure.csproj", "ExchangeRate.Aggregator.Shared.Infrastructure/"]
COPY ["ExchangeRate.Aggregator.Shared.Abstractions/ExchangeRate.Aggregator.Shared.Abstractions.csproj", "ExchangeRate.Aggregator.Shared.Abstractions/"]
RUN dotnet restore "ExchangeRate.Aggregator.Modules.Parsers/ExchangeRate.Aggregator.Modules.Parsers.csproj"
COPY . .
WORKDIR "/src/ExchangeRate.Aggregator.Modules.Parsers"
RUN dotnet build "ExchangeRate.Aggregator.Modules.Parsers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRate.Aggregator.Modules.Parsers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRate.Aggregator.Modules.Parsers.dll"]
