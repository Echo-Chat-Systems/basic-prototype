namespace EchoLib.Protocol.Exceptions;

public class NotFoundException : ProtocolException
{
	public override string Message => "not-found";
}