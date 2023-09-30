FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExchangeRate.Aggregator.Bootstrapper/ExchangeRate.Aggregator.Bootstrapper.csproj", "ExchangeRate.Aggregator.Bootstrapper/"]
COPY ["ExchangeRate.Aggregator.Shared.Infrastructure/ExchangeRate.Aggregator.Shared.Infrastructure.csproj", "ExchangeRate.Aggregator.Shared.Infrastructure/"]
COPY ["ExchangeRate.Aggregator.Shared.Abstractions/ExchangeRate.Aggregator.Shared.Abstractions.csproj", "ExchangeRate.Aggregator.Shared.Abstractions/"]
COPY ["ExchangeRate.Aggregator.Modules.Rates.Api/ExchangeRate.Aggregator.Modules.Rates.Api.csproj", "ExchangeRate.Aggregator.Modules.Rates.Api/"]
COPY ["ExchangeRate.Aggregator.Modules.Rates.Infrastructure/ExchangeRate.Aggregator.Modules.Rates.Infrastructure.csproj", "ExchangeRate.Aggregator.Modules.Rates.Infrastructure/"]
COPY ["ExchangeRate.Aggregator.Modules.Rates.Application/ExchangeRate.Aggregator.Modules.Rates.Application.csproj", "ExchangeRate.Aggregator.Modules.Rates.Application/"]
COPY ["ExchangeRate.Aggregator.Modules.Parsers.Infrastructure/ExchangeRate.Aggregator.Modules.Parsers.Infrastructure.csproj", "ExchangeRate.Aggregator.Modules.Parsers.Infrastructure/"]
COPY ["ExchangeRate.Aggregator.Modules.Parsers.Application/ExchangeRate.Aggregator.Modules.Parsers.Application.csproj", "ExchangeRate.Aggregator.Modules.Parsers.Application/"]
RUN dotnet restore "ExchangeRate.Aggregator.Bootstrapper/ExchangeRate.Aggregator.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/ExchangeRate.Aggregator.Bootstrapper"
RUN dotnet build "ExchangeRate.Aggregator.Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRate.Aggregator.Bootstrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRate.Aggregator.Bootstrapper.dll"]
