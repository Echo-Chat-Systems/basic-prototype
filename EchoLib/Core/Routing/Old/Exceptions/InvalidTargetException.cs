namespace EchoLib.Core.Routing.Exceptions;

public class InvalidTargetException : ProtocolException
{
	public override string Message => "invalid-target";
}