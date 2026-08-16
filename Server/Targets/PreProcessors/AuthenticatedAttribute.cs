using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server.Targets.PreProcessors;

public class AuthenticatedAttribute : BasePreProcessorAttribute
{
	private static ILogger<AuthenticatedAttribute>? _logger;

	public override async Task<bool> Run(RoutingContext ctx)
	{
		// Ensure we have a logger
		_logger ??= ctx.Services.GetRequiredService<ILogger<AuthenticatedAttribute>>();

		// Ensure the user is authenticated, and, if so, add userid to ctx
		ClientManager manager = ctx.Services.GetRequiredService<ClientManager>();

		ServerClient? client = manager.Get(ctx.Socket);

		if (client?.Authenticated != true)
		{
			_logger.LogInformation("Client {Id} failed auth check on authenticated route {target}:{action}", client?.Id, ctx.OriginalMessage.Target,
				ctx.OriginalMessage.Data.Action);
			return false;
		}

		ctx.User = client.Id;

		return true;
	}
}