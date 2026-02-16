using EchoLib.Models.Params;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebSocketSharp;

namespace EchoLib.Core.Routing;

public class RoutingContext
{
	public required WebSocket Socket { get; init; }
	public required IServiceProvider Services { get; init; }

	private ILogger<RoutingContext> _logger => Services.GetRequiredService<ILogger<RoutingContext>>();

	public async Task SendAsync<TParams>(ITarget invoker, TParams parameters) where TParams : IParam
	{
		_logger.LogDebug("Attempting to send {Target} {Action}", invoker.Name, parameters.Action);
		
		
	}
}