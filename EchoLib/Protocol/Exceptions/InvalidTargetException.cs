namespace EchoLib.Protocol.Exceptions;

public class InvalidTargetException : ProtocolException
{
	public override string Message => "invalid-target";
}