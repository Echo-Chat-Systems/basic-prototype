namespace EchoLib.Core.Routing.Exceptions;

public abstract class ProtocolException : Exception
{
	public abstract override string Message { get; }
}