using EchoLib.Protocol.Models.Crypto;
using EchoLib.Protocol.Models.Data.User;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;

namespace EchoLib.Protocol.Models.Params.Auth;

public class ClientSignupParameters : IParam
{
	public string Action => "signup";

	[JsonProperty("keys")] public required PublicKeyPairJm Keys { get; init; }
	[JsonProperty("profile")] public required JProfileModel ProfileModel { get; init; }
}