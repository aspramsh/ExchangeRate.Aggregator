FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExchangeRate.Aggregator.Modules.Api/ExchangeRate.Aggregator.Modules.Api.csproj", "ExchangeRate.Aggregator.Modules.Api/"]
RUN dotnet restore "ExchangeRate.Aggregator.Modules.Api/ExchangeRate.Aggregator.Modules.Api.csproj"
COPY . .
WORKDIR "/src/ExchangeRate.Aggregator.Modules.Api"
RUN dotnet build "ExchangeRate.Aggregator.Modules.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRate.Aggregator.Modules.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRate.Aggregator.Modules.Api.dll"]
