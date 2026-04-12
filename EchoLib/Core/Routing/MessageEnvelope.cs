using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Generic;
using Newtonsoft.Json;

namespace EchoLib.Core.Routing;

public class MessageEnvelope<TParams>
{
	[JsonProperty("target")] public string Target { get; set; } = string.Empty;
	[JsonProperty("data")] public ActionWrapper<TParams> Data { get; set; } = new();

	public MessageEnvelope<ErrorParameters> FromError(ProtocolException ex)
	{
		return new MessageEnvelope<ErrorParameters>
		{
			Target = "error",
			Data = new ActionWrapper<ErrorParameters>
			{
				Action = ex.Action,
				Params = new ErrorParameters
				{
					Source = this
				}
			}
		};
	}
}