using EchoLib.Protocol.Models.Data;
using EchoLib.Protocol.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Params.Auth;

public class SigninCompleteParameters : IParam
{
	public string Action => "signin-complete";
	
	[JsonProperty("user")] 
	public required JUserModel User { get; init; }
}