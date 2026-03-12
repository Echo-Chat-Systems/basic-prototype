using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Auth;
using Org.BouncyCastle.Utilities.Encoders;

namespace Client.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";
	
	private static readonly SigninState State = new SigninState();

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
		NotStarted, Started, Challenged, ChallengeResponded, Completed
	}

	public AuthTarget(RoutingContext ctx) : base(ctx) {}

	public async Task SendSigninStart(SigninStartParameters parameters)
	{
		// State checks to ensure linear progression 
		if (State.Stage != SigninStage.NotStarted)
		{
			throw new SigninAlreadyStartedException();
		}
		
		State.Stage = SigninStage.Started;
		await _ctx.SendAsync(this, parameters);
	}

	[ActionHandler("signin-challenge")]
	private Task HandleChallenge(SigninChallengeParameters parameters)
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

		if (!Client.Keys.PrvEk.Decrypt(sigChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();
		
		// Encode response
		string signature = Base64.ToBase64String(sigBytes);
		string decrypted = Base64.ToBase64String(dcrBytes);
		
		SigninResponseParameters response = new SigninResponseParameters
		{
			Signature = signature,
			Decrypted = decrypted
		};
		
		// Send response
		return _ctx.SendAsync(this, response);
	}
}