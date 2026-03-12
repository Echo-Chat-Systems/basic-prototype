namespace EchoLib.Core.Routing.Exceptions;

public class SigninAlreadyStartedException : ProtocolException
{
	public override string Action => "already-started";
}

public class SigninNotStartedException : ProtocolException
{
	public override string Action => "not-started";
}

public class SigninChallengeFailedException : ProtocolException
{
	public override string Action => "challenge-failed";
}