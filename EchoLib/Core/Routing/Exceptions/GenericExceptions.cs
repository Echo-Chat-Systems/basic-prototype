namespace EchoLib.Core.Routing.Exceptions;

public class NotFoundException : ProtocolException
{
	public override string Message => "not-found";
}