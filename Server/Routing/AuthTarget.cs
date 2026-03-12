using EchoLib.Core.Routing;

namespace Server.Routing;

public class AuthTarget : TargetBase
{
	public override string Name => "auth";
	
	

	public AuthTarget(RoutingContext ctx) : base(ctx)
	{
	}
}