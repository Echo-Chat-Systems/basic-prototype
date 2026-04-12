using EchoLib.Core.Routing;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Configuration;

namespace Server.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";


	[ActionHandler("client-hello")]
	private async Task HandleHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		// Get logger
		ILogger<AuthTarget> logger = ctx.Services.GetRequiredService<ILogger<AuthTarget>>();

		logger.LogInformation("New Client, Hello! Key: {PublicSigningKey}", parameters.Id);
		
		// Associate this client with their claimed ID (THIS DOES NOT MEAN THEY ARE AUTHENTICATED!!!!!!!!!)
		ClientManager.ServerClient? client = ctx.Services.GetRequiredService<ClientManager>().Get(ctx.Socket);
		if (client != null) client.Id = parameters.Id;
		
		// Respond with the server-hello
		await ctx.SendAsync(this, new ServerHelloParameters { ServerName = ctx.Services.GetRequiredService<Config>().Appearance.BroadcastName });
	}
}