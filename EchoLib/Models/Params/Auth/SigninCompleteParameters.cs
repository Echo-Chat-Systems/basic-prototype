using EchoLib.Core;
using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SigninCompleteParameters : IParam
{
	public string Action => RouteNames.Auth.SigninComplete;
	
	[JsonProperty("user")] 
	public required JUser User { get; init; }
}