using EchoLib.Models.Crypto;
using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class ClientSignupParameters : IParam
{
	public string Action => "signup";

	[JsonProperty("keys")] public required PublicKeyPairJm Keys { get; init; }
	[JsonProperty("profile")] public required JProfileModel ProfileModel { get; init; }
}