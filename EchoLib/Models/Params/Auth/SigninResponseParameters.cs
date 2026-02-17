using System.Text.Json.Serialization;

namespace EchoLib.Models.Params.Auth;

public class SigninResponseParameters : IParam
{
	public string Action => "signin-response"; 
	
	[JsonPropertyName("signature")] public required string Signature { get; init; }
	[JsonPropertyName("decrypted")] public required string Decrypted { get; init; }
}