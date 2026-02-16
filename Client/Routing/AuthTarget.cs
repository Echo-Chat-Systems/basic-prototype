using EchoLib.Core.Routing;
using EchoLib.Models.Params.Auth;

namespace Client.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";

	public AuthTarget(RoutingContext ctx) : base(ctx) { }

	public Task SendSigninStart(SigninStartParameters parameters) => _ctx.SendAsync(this, parameters);


}