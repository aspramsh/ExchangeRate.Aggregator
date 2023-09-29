FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ExchangeRate.Aggregator/ExchangeRate.Aggregator.csproj", "ExchangeRate.Aggregator/"]
RUN dotnet restore "ExchangeRate.Aggregator/ExchangeRate.Aggregator.csproj"
COPY . .
WORKDIR "/src/ExchangeRate.Aggregator"
RUN dotnet build "ExchangeRate.Aggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeRate.Aggregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeRate.Aggregator.dll"]
