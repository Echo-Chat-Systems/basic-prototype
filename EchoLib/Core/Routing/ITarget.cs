namespace EchoLib.Core.Routing;

public interface ITarget
{
	string Name { get; }

	/// <summary>
	/// Called by the router when a message for this target is received.
	/// </summary>
	/// <param name="envelope">Message</param>
	/// <param name="ctx">Routing context.</param>
	Task HandleAsync(MessageEnvelope<object> envelope, RoutingContext ctx);
}