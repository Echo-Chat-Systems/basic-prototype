using EchoLib.Core;
using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SignupCompleteParameters : IParam
{
	public string Action => RouteNames.Auth.Signup;

	[JsonProperty("user")] public required JUser User;
}