using System.Text.Json.Serialization;

namespace EchoLib.Models.Params.Auth;

public class SigninChallengeParameters : IParam
{
	public string Action => "signin-challenge";
	
	[JsonPropertyName("sign-challenge")] public string SignChallenge { get; }
	[JsonPropertyName("encrypt-challenge")] public string EncryptChallenge { get; }
}