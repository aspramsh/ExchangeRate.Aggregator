using ExchangeRate.Aggregator.Modules.Parsers.Application.Handlers.Commands;
using ExchangeRate.Aggregator.Shared.Abstractions.Workers;
using MediatR;

namespace ExchangeRate.Aggregator.Modules.Parsers;

public class Worker : BackgroundService, IWorker
{
    private readonly ILogger<Worker> _logger;
    
    private readonly IServiceProvider _serviceProvider;

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator =
                    scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new GetRatesCommand(), stoppingToken);
            }
            
            // TODO: move to appsettings
            await Task.Delay(10000, stoppingToken);
        }
    }
}