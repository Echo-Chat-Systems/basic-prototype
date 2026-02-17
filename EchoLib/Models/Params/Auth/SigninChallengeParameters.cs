using System.Text.Json.Serialization;

namespace EchoLib.Models.Params.Auth;

public class SigninChallengeParameters : IParam
{
	public string Action => "signin-challenge";
	
	[JsonPropertyName("sign-challenge")] public required string SignChallenge { get; init; }
	[JsonPropertyName("encrypt-challenge")] public required string EncryptChallenge { get; init; }
}