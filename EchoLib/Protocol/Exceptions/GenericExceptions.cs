namespace EchoLib.Protocol.Exceptions;

public class InternalServerException : ProtocolException
{
	public override string Message => "server-failed";
}

public class NotFoundException : ProtocolException
{
	public override string Message => "not-found";
}

