using EchoLib.Models.Params.Auth;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;
using WebSocketSharper;

namespace Client.Routing;

public class AuthTarget(ILogger<AuthTarget> logger, Client.SessionInfo sessionInfo) : TargetBase<AuthTarget>(logger)
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

	public async Task SendHello(IMessageEndpoint endpoint, ClientHelloParameters parameters)
	{
		Logger.LogDebug("Sending hello to server.");
		ServerHelloParameters hello = await endpoint.RequestAsync<ServerHelloParameters, ClientHelloParameters>(Name, parameters);

		Logger.LogInformation("Server broadcast name: \"{ParametersServerName}\"", hello.ServerName);

		Console.Title = $"Connected to {hello.ServerName}";
		sessionInfo.ServerName = hello.ServerName;
	}

	public async Task<SigninCompleteParameters> SendSigninStart(IMessageEndpoint endpoint, SigninStartParameters parameters)
	{
		// State checks to ensure linear progression 
		if (State.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		State.Stage = SigninStage.Started;
		SigninChallengeParameters challenge = await endpoint.RequestAsync<SigninChallengeParameters, SigninStartParameters>(Name, parameters);

		// State checks to preserve thread sanity
		if (State.Stage != SigninStage.Started) throw new SigninNotStartedException();

		// Update stage
		State.Stage = SigninStage.Challenged;

		// Update state with challenge
		State.SignChallenge = challenge.SignChallenge;
		State.EncryptChallenge = challenge.EncryptChallenge;

		// Decode challenges from strings into bytes using base64
		byte[] sigChallengeBytes = Base64.Decode(challenge.SignChallenge);
		byte[] encChallengeBytes = Base64.Decode(challenge.EncryptChallenge);

		// Complete challenges
		byte[] sigBytes = Client.Keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!Client.Keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);

		// Send response
		try
		{
			return await endpoint.RequestAsync<SigninCompleteParameters, SigninResponseParameters>(Name, new SigninResponseParameters
			{
				Signature = signature,
				Decrypted = decrypted
			});
		}
		catch (SigninChallengeFailedException)
		{
			Logger.LogDebug("Signin challenge failed!");
			throw;
		}
	}
}