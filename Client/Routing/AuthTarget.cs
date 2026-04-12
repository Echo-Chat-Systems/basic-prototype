using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;
using WebSocketSharper;

namespace Client.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";

	private static readonly SigninState State = new();

	private record SigninState
	{
		public SigninStage Stage = SigninStage.NotStarted;
		public string? SignChallenge;
		public string? EncryptChallenge;
		public string? SignResponse;
		public string? EncryptResponse;
	}

	private enum SigninStage
	{
		NotStarted,
		Started,
		Challenged,
		ChallengeResponded,
		Completed
	}

	public AuthTarget()
	{
	}

	public async Task SendHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		await ctx.SendAsync(this, parameters);
	}

	public async Task SendSigninStart(RoutingContext ctx, SigninStartParameters parameters)
	{
		// State checks to ensure linear progression 
		if (State.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		State.Stage = SigninStage.Started;
		await ctx.SendAsync(this, parameters);
	}

	[ActionHandler("signin-challenge")]
	private Task HandleChallenge(RoutingContext ctx, SigninChallengeParameters parameters)
	{
		// State checks to ensure linear progression
		if (State.Stage != SigninStage.Started) throw new SigninNotStartedException();

		// Update stage
		State.Stage = SigninStage.Challenged;

		// Update state with challenge
		State.SignChallenge = parameters.SignChallenge;
		State.EncryptChallenge = parameters.EncryptChallenge;

		// Decode challenges from strings into bytes using base64
		byte[] sigChallengeBytes = Base64.Decode(parameters.SignChallenge);
		byte[] encChallengeBytes = Base64.Decode(parameters.EncryptChallenge);

		// Complete challenges
		byte[] sigBytes = Client.Keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!Client.Keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);

		SigninResponseParameters response = new()
		{
			Signature = signature,
			Decrypted = decrypted
		};

		// Send response
		return ctx.SendAsync(this, response);
	}

	[ActionHandler("server-hello")]
	private Task HandleHello(RoutingContext ctx, ServerHelloParameters parameters)
	{
		// Get logger
		ctx.Services.GetRequiredService<ILogger<AuthTarget>>().LogInformation("Server broadcast name: \"{ParametersServerName}\"", parameters.ServerName);

		// Set server name
		ctx.Services.GetRequiredService<Client.SessionInfo>().ServerName = parameters.ServerName;

		return Task.CompletedTask;
	}
}