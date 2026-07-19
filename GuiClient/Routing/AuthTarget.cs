using EchoLib.Models.Params.Auth;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;

namespace GuiClient.Routing;

public class AuthTarget: TargetBase<AuthTarget>
{
	public override string Name => "auth";

	private readonly StateStore _session;
	private readonly IMessageEndpoint _endpoint;

	public AuthTarget(ILogger<AuthTarget> logger, StateStore sessionInfo, IMessageEndpoint endpoint) : base(logger)
	{
		_session = sessionInfo;
		_endpoint = endpoint;
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


	public async Task<ServerHelloParameters> SendHello(ClientHelloParameters parameters)
	{
		return await _endpoint.RequestAsync<ServerHelloParameters, ClientHelloParameters>(Name, parameters);
	}

	public async Task<SigninCompleteParameters> SendSigninStart(SigninStartParameters parameters)
	{
		// State checks to ensure linear progression
		if (State.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		State.Stage = SigninStage.Started;
		SigninChallengeParameters challenge = await _endpoint.RequestAsync<SigninChallengeParameters, SigninStartParameters>(Name, parameters);

		// State checks to ensure linear progression
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
		try
		{
			return await _endpoint.RequestAsync<SigninCompleteParameters, SigninResponseParameters>(Name, new SigninResponseParameters
			{
				Signature = signature,
				Decrypted = decrypted
			});
		}
		catch (SigninChallengeFailedException)
		{
			Logger.LogDebug("Signin challenge failed!");
			throw;
		}	}




}