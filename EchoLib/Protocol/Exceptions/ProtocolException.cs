namespace EchoLib.Protocol.Exceptions;

public abstract class ProtocolException : Exception
{
	public abstract override string Message { get; }
}