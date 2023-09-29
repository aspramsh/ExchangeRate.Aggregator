using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;

 /// <summary>
    /// Central error/exception handler Middleware
    /// </summary>
    /// <seealso>
    ///     <cref>https://dev.to/boriszn/error-handling-and-validation-architecture-in-net-core-3lhe</cref>
    ///     <cref>https://github.com/Boriszn/DeviceManager.Api</cref>
    /// </seealso>
    public class HttpExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly RequestDelegate _request;
        private readonly ILogger<HttpExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">Logger.</param>
        public HttpExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<HttpExceptionHandlerMiddleware> logger)
        {
            _request = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        /// <returns>Async void.</returns>
        public Task Invoke(HttpContext context, IHostEnvironment host) => InvokeAsync(context, host);

        /// <summary>
        /// Configurates/maps exception to the proper HTTP error Type
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Status code.</returns>
        private BaseHttpException ConfigurateExceptionTypes(
            Exception exception,
            IHostEnvironment host)
        {
            var message = exception.Message;
            _logger.LogError(exception, "{message}", message);

            var httpException = exception as BaseHttpException;

            if (httpException != default)
            {
                return httpException;
            }

            return host.IsProduction() || host.IsStaging() ? new InternalServerException()
                : new InternalServerException(exception);
        }

        private async Task InvokeAsync(
            HttpContext context,
            IHostEnvironment host)
        {
            try
            {
                await _request(context);
            }
            catch (Exception exception)
            {
                var httpException = ConfigurateExceptionTypes(exception, host);

                var response = new ErrorModel(
                    httpException.ErrorObject?.MessageKey,
                    httpException.Message,
                    httpException.ErrorObject?.Field,
                    httpException.ErrorObject?.Errors);

                var payload = JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings
                    { 
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    });
                context.Response.ContentType = JsonContentType;
                context.Response.StatusCode = (int)httpException.StatusCode;

                await context.Response.WriteAsync(payload);
            }
        }
    }