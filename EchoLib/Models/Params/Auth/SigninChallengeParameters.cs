using EchoLib.Core;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SigninChallengeParameters : IParam
{
	public string Action => RouteNames.Auth.SigninChallenge;

	[JsonProperty("sign-challenge")] public required string SignChallenge { get; init; }
	[JsonProperty("encrypt-challenge")] public required string EncryptChallenge { get; init; }
}