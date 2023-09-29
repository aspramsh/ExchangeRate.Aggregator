using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Aggregator.Shared.Abstractions.Modules;

public interface IModule
{
    string Name { get; }
    
    IEnumerable<string> Policies => null;
    
    void Register(IServiceCollection services, IConfiguration configuration);
    
    void Use(IApplicationBuilder app);
}