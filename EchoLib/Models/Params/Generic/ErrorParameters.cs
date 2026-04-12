using Newtonsoft.Json;

namespace EchoLib.Models.Params.Generic;

public class ErrorParameters : IParam
{
	public string Action => "error";

	[JsonProperty("source")] public required object Source { get; init; }
}