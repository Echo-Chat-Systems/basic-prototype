using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Protocol.Models.Params.Auth;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;

namespace GuiClient.Routing;

public class AuthTarget: TargetBase<AuthTarget>
{
	public override string Name => "auth";

	public readonly StateStore _session;

	public AuthTarget(ILogger<AuthTarget> logger, StateStore sessionInfo) : base(logger)
	{
		_session = sessionInfo;
	}

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


	public async Task SendHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		await ctx.ReplyAsync(this, parameters);
	}

	[Route("server-hello")]
	public async Task HandleHello(RoutingContext ctx, ServerHelloParameters parameters)
	{
		// Get logger
		Logger.LogInformation("Server broadcast name: \"{ParametersServerName}\"", parameters.ServerName);

		// Set server name
		_session.ServerName = parameters.ServerName;

		// Begin login flow
		await SendSigninStart(ctx, new SigninStartParameters
		{
			Sk = _session.UserFile!.Keys.PubSk,
			Ek = _session.UserFile!.Keys.PubEk
		});
	}

	public async Task SendSigninStart(RoutingContext ctx, SigninStartParameters parameters)
	{
		// State checks to ensure linear progression
		if (State.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		State.Stage = SigninStage.Started;
		await ctx.ReplyAsync(this, parameters);
	}

	[Route("signin-challenge")]
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
		byte[] sigBytes = _session.UserFile!.Keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!_session.UserFile!.Keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);

		SigninResponseParameters response = new()
		{
			Signature = signature,
			Decrypted = decrypted
		};

		// Send response
		return ctx.ReplyAsync(this, response);
	}


}