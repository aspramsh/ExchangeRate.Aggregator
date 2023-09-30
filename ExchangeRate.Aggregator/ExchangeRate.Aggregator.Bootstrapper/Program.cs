using ExchangeRate.Aggregator.Bootstrapper;
using ExchangeRate.Aggregator.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureModules();

// https://mobiletonster.com/blog/code/aspnet-core-6-how-to-deal-with-the-missing-startupcs-file
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();