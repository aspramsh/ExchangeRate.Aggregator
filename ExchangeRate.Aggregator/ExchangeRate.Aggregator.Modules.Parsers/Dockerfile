FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExchangeRate.Aggregator.Parsers/ExchangeRate.Aggregator.Parsers.csproj", "ExchangeRate.Aggregator.Parsers/"]
RUN dotnet restore "ExchangeRate.Aggregator.Parsers/ExchangeRate.Aggregator.Parsers.csproj"
COPY . .
WORKDIR "/src/ExchangeRate.Aggregator.Parsers"
RUN dotnet build "ExchangeRate.Aggregator.Parsers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRate.Aggregator.Parsers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRate.Aggregator.Parsers.dll"]
