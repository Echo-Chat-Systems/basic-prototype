using EchoLib.Core.Routing;
using EchoLib.Core.Routing.Events;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Crypto;
using EchoLib.Models.Params.Auth;
using EchoLib.Models.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Utilities.Encoders;

namespace GuiClient.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";

	private SigninState? _state;

	public AuthTarget(EventBus bus)
	{
		bus.Subscribe<ProtocolErrorEvent>(HandleProtocolError);
	}

	private Task HandleProtocolError(ProtocolErrorEvent evt)
	{
		// Filter for any events targeting auth 
		if (evt.Source.Target != Name) return Task.CompletedTask;

		// Set error for flow to be whatever it is 
		_state!.Tcs.TrySetException((ProtocolException)Activator.CreateInstance(ExceptionsRegistry.Exceptions[evt.Message])!);
		return Task.CompletedTask;
	}
	
	private record SigninState
	{
		public SigninStage Stage;

		public TaskCompletionSource<(bool complete, string? msg)> Tcs { get; } = new(); 
		
		public string? SignChallenge;
		public string? EncryptChallenge;
		public string? SignResponse;
		public string? EncryptResponse;
	}

	public async Task SendHello(RoutingContext ctx, ClientHelloParameters parameters)
	{
		await ctx.SendAsync(this, parameters);
	}

	public async Task<Task<(bool complete, string? msg)>> SendSigninStart(RoutingContext ctx, SigninStartParameters parameters)
	{
		// State checks to ensure linear progression 
		if (_state != null) throw new SigninAlreadyStartedException();

		_state = new SigninState();
		_ = ctx.SendAsync(this, parameters);

		return _state.Tcs.Task;
	}

	[ActionHandler("signin-challenge")]
	private Task HandleChallenge(RoutingContext ctx, SigninChallengeParameters parameters)
	{
		// Get required services
		KeySetJm keys = ctx.Services.GetRequiredService<StateStore>().UserFile!.Keys;
		
		// State checks to ensure linear progression
		if (_state.Stage != SigninStage.Started) throw new SigninNotStartedException();

		// Update stage
		_state.Stage = SigninStage.Challenged;

		// Update state with challenge
		_state.SignChallenge = parameters.SignChallenge;
		_state.EncryptChallenge = parameters.EncryptChallenge;

		// Decode challenges from strings into bytes using base64
		byte[] sigChallengeBytes = Base64.Decode(parameters.SignChallenge);
		byte[] encChallengeBytes = Base64.Decode(parameters.EncryptChallenge);

		// Complete challenges
		byte[] sigBytes = keys.PrvSk.Sign(sigChallengeBytes);
		byte[] dcrBytes;

		if (!keys.PrvEk.Decrypt(encChallengeBytes, out dcrBytes)) throw new SigninChallengeFailedException();

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

	[ActionHandler("signin-complete")]
	private Task HandleSigninComplete(RoutingContext ctx, SigninCompleteParameters parameters)
	{
		_state?.Tcs.TrySetResult((true, ""));
		_state = null;

		return Task.CompletedTask;
	}
	
	[ActionHandler("server-hello")]
	private Task HandleHello(RoutingContext ctx, ServerHelloParameters parameters)
	{
		// Get logger
		ctx.Services.GetRequiredService<ILogger<AuthTarget>>().LogInformation("Server broadcast name: \"{ParametersServerName}\"", parameters.ServerName);

		// Set server name
		ctx.Services.GetRequiredService<StateStore>().ServerName = parameters.ServerName;

		return Task.CompletedTask;
	}
}