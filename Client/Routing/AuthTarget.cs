using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Auth;

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
		
		// Complete challenges
		SigninResponseParameters response = new SigninResponseParameters
		{
			Signature = Client.Keys.PrvSk
		};
	} 

	
}