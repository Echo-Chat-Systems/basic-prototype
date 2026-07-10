using Newtonsoft.Json;

namespace EchoLib.Core.Routing;

public class ActionWrapper<TParams>
{
	[JsonProperty("action")] public string Action { get; set; } = string.Empty;
	[JsonProperty("params")] public TParams Params { get; set; } = default!;
}