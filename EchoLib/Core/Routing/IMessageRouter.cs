namespace EchoLib.Core.Routing;

public interface IMessageRouter
{
	Task RouteAsync(MessageEnvelope<object> envelope);
}