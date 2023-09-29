using ExchangeRate.Aggregator.Modules.Parsers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddParsersModule(hostContext.Configuration);
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();