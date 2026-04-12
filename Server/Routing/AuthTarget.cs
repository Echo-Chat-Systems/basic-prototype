using EchoLib.Core.Routing;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";


	[ActionHandler("client-hello")]
	private Task HandleHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		// Get logger
		ILogger<AuthTarget> logger = ctx.Services.GetRequiredService<ILogger<AuthTarget>>();

		logger.LogInformation("New Client, Hello! Key: {PublicSigningKey}", parameters.Id);
		return Task.CompletedTask;
	}
}