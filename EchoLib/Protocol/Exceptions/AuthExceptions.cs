namespace EchoLib.Protocol.Exceptions;

public class SocketRegistryException : ProtocolException
{
	public override string Message => "socket-registry";
}

public class InvalidKeyException : ProtocolException
{
	public override string Message => "invalid-key";
}

public class SigninAlreadyStartedException : ProtocolException
{
	public override string Message => "already-started";
}

public class SigninNotStartedException : ProtocolException
{
	public override string Message => "not-started";
}

public class SigninChallengeFailedException : ProtocolException
{
	public override string Message => "challenge-failed";
}

public class KeyConflictException : ProtocolException
{
	public override string Message => "key-conflict";
}