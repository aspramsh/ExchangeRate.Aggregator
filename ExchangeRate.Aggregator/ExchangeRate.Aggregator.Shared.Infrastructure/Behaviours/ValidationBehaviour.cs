using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions.Http;
using ExchangeRate.Aggregator.Shared.Infrastructure.Helpers;
using FluentValidation;
using MediatR;

namespace ExchangeRate.Aggregator.Shared.Infrastructure.Behaviours;

/// <summary>
/// https://github.com/jasontaylordev/CleanArchitecture/tree/main/src/Application/Common/Behaviours
/// </summary>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var errorValidations = failures.Select(t => new ErrorModel
                {
                    MessageKey = t.ErrorMessage,
                     Field = t.PropertyName.ToCamelCase()
                 }).AsEnumerable();
                
                throw new BadRequestException(new ErrorModel(errorValidations));
            }
        }

        return await next();
    }
}