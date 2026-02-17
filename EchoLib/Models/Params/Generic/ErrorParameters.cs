using System.Text.Json.Serialization;

namespace EchoLib.Models.Params.Generic;

public class ErrorParameters : IParam
{
	public string Action => "error";
	
	[JsonPropertyName("source")] public required object Source { get; init; }
}