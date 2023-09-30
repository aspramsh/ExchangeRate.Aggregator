using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.CreateRate;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Commands.EditRate;
using ExchangeRate.Aggregator.Modules.Rates.Application.Handlers.Queries.GetRateById;
using ExchangeRate.Aggregator.Shared.Infrastructure.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.Aggregator.Modules.Rates.Api.Controllers;

public class RatesController : BaseController
{
    private readonly IMediator _mediator;
    
    public RatesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    ///     Get rate by id
    /// </summary>
    /// <param name="rateId">Rate ID</param>
    /// <returns>Rate.</returns>
    [HttpGet("{rateId}")]
    [ProducesResponseType(typeof(RateDetailResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync(long rateId)
    {
        var result = await _mediator.Send(new GetRateQuery()
        {
            Id = rateId
        });

        return Ok(result);
    }
    
    /// <summary>
    ///     Create rate
    /// </summary>
    /// <param name="model">Rate model</param>
    /// <returns>Rate.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RateDetailResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(CreateRateCommand model)
    {
        var result = await _mediator.Send(model);

        return CreatedAtAction(nameof(GetAsync), new { rateId = result.Id }, result);
    }
    
    /// <summary>
    ///     Edit rate
    /// </summary>
    /// <param name="model">Rate model</param>
    /// <returns>Rate.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(RateDetailResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditAsync(EditRateCommand model)
    {
        var result = await _mediator.Send(model);

        return Ok(result);
    }
}