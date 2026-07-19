using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SigninCompleteParameters : IParam
{
	public string Action => "signin-complete";
	
	[JsonProperty("user")] 
	public required JUserModel User { get; init; }
}