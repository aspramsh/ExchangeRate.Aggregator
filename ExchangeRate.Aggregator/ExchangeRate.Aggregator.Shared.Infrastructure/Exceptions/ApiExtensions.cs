using Microsoft.AspNetCore.Builder;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;

public static class ApiExtensions
{
    public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<HttpExceptionHandlerMiddleware>();
}