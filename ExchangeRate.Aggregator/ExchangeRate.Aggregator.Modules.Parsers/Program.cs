using ExchangeRate.Aggregator.Modules.Parsers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddParsersModule(hostContext.Configuration);
    })
    .Build();

host.Run();