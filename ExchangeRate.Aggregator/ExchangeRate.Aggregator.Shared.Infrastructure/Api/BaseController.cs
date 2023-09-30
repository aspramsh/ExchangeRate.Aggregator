using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Api;

/// <summary>
/// Base controller
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : Controller
{
}