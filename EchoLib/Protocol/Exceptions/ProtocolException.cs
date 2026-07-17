namespace EchoLib.Protocol.Exceptions;

public abstract class ProtocolException(Guid eid) : Exception
{
	public abstract override string Message { get; }
	public Guid Eid { get; } = eid;

	protected ProtocolException() : this(Guid.NewGuid())
	{
	}
}