using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Auth;
using EchoLib.Models.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Configuration;

namespace Server.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";

	public class SigninState
	{
		public SigninStage Stage = SigninStage.NotStarted;
		
		public string? SignChallenge;
		public string? EncryptChallenge;
	}
	
	[ActionHandler("client-hello")]
	private async Task HandleHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		// Get logger
		ILogger<AuthTarget> logger = ctx.Services.GetRequiredService<ILogger<AuthTarget>>();

		logger.LogInformation("New Client, Hello! Key: {PublicSigningKey}", parameters.Id);
		
		// Associate this client with their claimed ID (THIS DOES NOT MEAN THEY ARE AUTHENTICATED!!!!!!!!!)
		ServerClient? client = ctx.Services.GetRequiredService<ClientManager>().Get(ctx.Socket);
		if (client != null) client.Id = parameters.Id;
		
		// Respond with the server-hello
		await ctx.SendAsync(this, new ServerHelloParameters { ServerName = ctx.Services.GetRequiredService<Config>().Appearance.BroadcastName });
	}

	[ActionHandler("signin-start")]
	private async Task HandleSigninStart(RoutingContext ctx, SigninStartParameters parameters)
	{
		// Get required services
		ClientManager manager = ctx.Services.GetRequiredService<ClientManager>();
		
		// Get this client from the manager
		ServerClient? client = manager.Get(ctx.Socket);

		if (client is null)
		{
			// Client is somehow not in manager, this means they skipped the client-hello, disconnect them 
			ctx.Socket.CloseAsync();
			return;
		}
		
		// Check to ensure that this socket does not have an existing signin session 
		if (client.SigninState.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();
		
		// Generate a set of challenges for the client
		client
		
	}
}