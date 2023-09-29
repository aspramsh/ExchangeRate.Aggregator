using ExchangeRate.Aggregator.Shared.Abstractions.Workers;

namespace ExchangeRate.Aggregator.Modules.Parsers;

public class Worker : BackgroundService, IWorker
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}