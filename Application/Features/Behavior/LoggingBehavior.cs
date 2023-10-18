using NLog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.Info($"Handling {typeof(TRequest).Name}");

        var response = await next();

        _logger.Info($"Handled {typeof(TResponse).Name}");

        return response;
    }
}
