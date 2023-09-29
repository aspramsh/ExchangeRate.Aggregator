using System.Reflection;
using ExchangeRate.Aggregator.Shared.Abstractions.Modules;
using ExchangeRate.Aggregator.Shared.Infrastructure;
using ExchangeRate.Aggregator.Shared.Infrastructure.Modules;

namespace ExchangeRate.Aggregator;

public class Startup
{
    private readonly IList<Assembly> _assemblies;
    
    private readonly IList<IModule> _modules;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _assemblies = ModuleLoader.LoadAssemblies(configuration, "HighWay.Aggregator.Modules.");
        _modules = ModuleLoader.LoadModules(_assemblies);
    }
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddModularInfrastructure(_assemblies, _modules);

        foreach (var module in _modules)
        {
            module.Register(services, Configuration);
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseModularInfrastructure(Configuration, env);

        _assemblies.Clear();
        _modules.Clear();
    }
}