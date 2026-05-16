using System.Security.Cryptography;
using EchoLib.Core.Crypto.Signing;
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
		
		public byte[]? SignChallenge;
		public byte[]? EncryptChallenge;
	}
	
	[ActionHandler("client-hello")]
	private async Task HandleHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		// Get logger
		ILogger<AuthTarget> logger = ctx.Services.GetRequiredService<ILogger<AuthTarget>>();

		logger.LogInformation("New Client, Hello! Key: {PublicSigningKey}", parameters.KeyPair.SigningKey);
		
		// Associate this client with their claimed ID (THIS DOES NOT MEAN THEY ARE AUTHENTICATED!!!!!!!!!)
		ServerClient? client = ctx.Services.GetRequiredService<ClientManager>().Get(ctx.Socket);
		
		// Check if client is null, if so somethings fucked 
		if (client is null) throw new SocketRegistryException();
		
		// Set client keys and id 
		client.Id = parameters.KeyPair.SigningKey;
		client.KeyPair = parameters.KeyPair;
		
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
		
		
		// Convert challenges to base64 strings for transport
		byte[] signChallenge = RandomNumberGenerator.GetBytes(64);
		byte[] encryptChallenge = RandomNumberGenerator.GetBytes(64);
		
		// Store the challenges in the client's signin state
		client.SigninState.SignChallenge = signChallenge;
		client.SigninState.EncryptChallenge = encryptChallenge;
		client.SigninState.Stage = SigninStage.Challenged;
		
		// Send the challenges to the client
		await ctx.SendAsync(this, new SigninChallengeParameters
		{
			SignChallenge = Convert.ToBase64String(signChallenge),
			EncryptChallenge = Convert.ToBase64String(client.KeyPair!.EncryptionKey!.Encrypt(encryptChallenge))  // Encrypt the encrypt challenge with the client's encryption key so only they can read it
		});
	}

	[ActionHandler("signin-response")]
	private async Task HandleSigninResponse(RoutingContext ctx, SigninResponseParameters parameters)
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
		
		// Check to ensure that this socket has an active signin session
		if (client.SigninState.Stage != SigninStage.Challenged) throw new SigninNotStartedException();
		
		// Update stage
		client.SigninState.Stage = SigninStage.ChallengeResponded;
		
		// Convert the response challenges back from base64
		Signature sig = new(parameters.Signature);
		byte[] encryptResponse = Convert.FromBase64String(parameters.Decrypted);
		
		// Verify the signature and encryption response
		bool sigValid = sig.Verify(client.Id!.KeyParams, client.SigninState.SignChallenge!);
		bool encryptVald = client.SigninState.EncryptChallenge == encryptResponse;
		
		if (sigValid && encryptVald)
		{
			// Authentication successful, update client state and respond with success
			client.SigninState.Stage = SigninStage.Authenticated;
			await ctx.SendAsync(this, new SigninSuccessParameters());
		}
		else
		{
			// Authentication failed, disconnect the client
			ctx.Socket.CloseAsync();
		}
	}
}