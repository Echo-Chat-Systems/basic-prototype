using EchoLib.Protocol.Models.Data;
using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Params.Generic;

public class ErrorParameters : IParam
{
	public string Action => "error";

	[JsonProperty("message")] public required string Message { get; init; }
}