using EchoLib.Models.Crypto;
using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SignupParameters : IParam
{
	public string Action => "signup";

	[JsonProperty("keys")] public required JPublicKeyPair Keys { get; init; }
	[JsonProperty("profile")] public required JProfile Profile { get; init; }
}