using EchoLib.Models.Params.Auth;
using EchoLib.Models.States;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;
using TuiClient.Events;

namespace TuiClient.Targets;

public class AuthTarget(ILogger<AuthTarget> logger, State state) : TargetBase<AuthTarget>(logger)
{
	public override string Name => "auth";

	public delegate void SigninStageChangedEventHandler(SigninStageChangedEventArgs e);

	public event SigninStageChangedEventHandler? OnSigninStageChanged;

	public SigninStage SigninState
	{
		get => field;
		private set
		{
			SigninStage old = field;
			field = value;

			OnSigninStageChanged?.Invoke(new SigninStageChangedEventArgs(old, value));
		}
	} = SigninStage.NotStarted;

	public async Task<ServerHelloParameters> SendHello(IMessageEndpoint endpoint, ClientHelloParameters parameters)
	{
		Logger.LogDebug("Sending hello to server.");
		ServerHelloParameters hello = await endpoint.RequestAsync<ServerHelloParameters, ClientHelloParameters>(Name, parameters);

		Logger.LogInformation("Server broadcast name: \"{ParametersServerName}\"", hello.ServerName);
		return hello;
	}

	public async Task<SignupCompleteParameters> Signup(IMessageEndpoint endpoint, SignupParameters parameters)
	{
		// Request a new signup
		return await endpoint.RequestAsync<SignupCompleteParameters, SignupParameters>(Name, parameters);
	}

	public async Task<SigninCompleteParameters> Signin(IMessageEndpoint endpoint, SigninStartParameters parameters)
	{
		// State checks to ensure linear progression
		if (SigninState != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		SigninState = SigninStage.Started;
		SigninChallengeParameters challenge = await endpoint.RequestAsync<SigninChallengeParameters, SigninStartParameters>(Name, parameters);

		// State checks to preserve thread sanity
		if (SigninState != SigninStage.Started) throw new SigninNotStartedException();

		// Update stage
		SigninState = SigninStage.Challenged;

		// Decode challenges from strings into bytes using base64
		byte[] sigChallengeBytes = Base64.Decode(challenge.SignChallenge);
		byte[] encChallengeBytes = Base64.Decode(challenge.EncryptChallenge);

		// Complete challenges
		byte[] sigBytes = state.Keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!state.Keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);

		// Send response
		try
		{
			SigninState = SigninStage.ChallengeResponded;
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