namespace EchoLib.Core.Routing.Exceptions;

public abstract class ProtocolException : Exception
{
	public abstract string Action { get; }
}