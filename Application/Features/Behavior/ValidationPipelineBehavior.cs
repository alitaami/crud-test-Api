﻿using Application.Exceptions;
using Application.Features.Behavior.Contracts;
using FluentValidation;
using MediatR;

namespace Application.Features.Behavior
{
    public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IValidatable
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        // validate errors with any request that has been sent
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                List<string> errors = new();

                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task
                    .WhenAll(_validators
                    .Select(s => s.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(s => s.Errors)
                    .Where(e => e != null).ToList();

                if (failures.Count != 0)
                {
                    foreach (var failure in failures)
                    {
                        errors.Add(failure.ErrorMessage);
                    }
                    throw new CustomValidationException(errors, failures.Select(x => x.ErrorMessage).ToString());
                }
            }
            return await next();
        }
    }
}
