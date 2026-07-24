using System;
using System.Threading.Tasks;
using EchoLib.Models.Crypto;
using EchoLib.Models.Data.User;
using EchoLib.Models.Params.Auth;
using EchoLib.Models.States;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;

namespace AvaloniaClient.Targets;

public class AuthTarget(ILogger<AuthTarget> logger, AppState state) : ITarget
{
	public string Name => "auth";

	private IMessageEndpoint Endpoint => state.Net.Client.BuildEndpoint;

	private JKeySet Keys => state.Local.UserFile?.Keys ?? throw new NullReferenceException();
	private SigninStage _signinState = SigninStage.NotStarted;

	public async Task<ServerHelloParameters> SendHello()
	{
		if (Endpoint is null) throw new InvalidOperationException(nameof(Endpoint));
		return await Endpoint.RequestAsync<ServerHelloParameters, ClientHelloParameters>(Name, new ClientHelloParameters { KeyPair = Keys.ToPublicKeyPair() });
	}

	public async Task<SignupCompleteParameters> Signup(JProfile profile)
	{
		// Request a new signup
		if (Endpoint is null) throw new InvalidOperationException(nameof(Endpoint));
		return await Endpoint.RequestAsync<SignupCompleteParameters, SignupParameters>(Name, new SignupParameters
		{
			Keys = Keys.ToPublicKeyPair(),
			Profile = profile
		});
	}

	public async Task<SigninCompleteParameters> Signin()
	{
		if (Endpoint is null) throw new InvalidOperationException(nameof(Endpoint));
		// State checks to ensure linear progression
		if (_signinState != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

		_signinState = SigninStage.Started;
		SigninChallengeParameters challenge = await Endpoint.RequestAsync<SigninChallengeParameters, SigninStartParameters>(Name, new SigninStartParameters()
		{
			Ek = Keys.PubEk,
			Sk = Keys.PubSk
		});

		// State checks to preserve thread sanity
		if (_signinState != SigninStage.Started) throw new SigninNotStartedException();

		// Update stage
		_signinState = SigninStage.Challenged;

		// Decode challenges from strings into bytes using base64
		byte[] sigChallengeBytes = Base64.Decode(challenge.SignChallenge);
		byte[] encChallengeBytes = Base64.Decode(challenge.EncryptChallenge);

		// Complete challenges
		byte[] sigBytes = Keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!Keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);

		// Send response
		_signinState = SigninStage.ChallengeResponded;
		return await Endpoint.RequestAsync<SigninCompleteParameters, SigninResponseParameters>(Name, new SigninResponseParameters
		{
			Signature = signature,
			Decrypted = decrypted
		});
	}
}