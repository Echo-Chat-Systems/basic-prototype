namespace EchoLib.Core;

public static class RouteNames
{
	public static class Auth
	{
		public const string ClientHello = "hello";
		public const string ServerHello = "server-hello";
		
		public const string Signup = "signup";

		public const string SigninStart = "signin-start";
		public const string SigninChallenge = "signin-challenge";
		public const string SigninResponse = "signin-response";
		public const string SigninComplete = "signin-complete";
	}

	public static class Guilds
	{
		public const string Create = "create";
		public const string Delete = "delete";
		public const string Get = "get";
		public const string Query = "query";
	}

	public static class Channels
	{
		public const string Create = "create";
		public const string Delete = "delete";
		public const string Get = "get";
		
	}
}