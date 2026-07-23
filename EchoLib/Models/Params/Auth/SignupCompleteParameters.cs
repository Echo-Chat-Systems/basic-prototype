using EchoLib.Models.Data.User;
using Newtonsoft.Json;

namespace EchoLib.Models.Params.Auth;

public class SignupCompleteParameters : IParam
{
	public string Action => "signup";

	[JsonProperty("user")] public required JUser User;
}