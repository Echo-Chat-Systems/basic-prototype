using System.Text.Json.Serialization;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Models.Params.Generic;

namespace EchoLib.Core.Routing;

public class MessageEnvelope<TParams>
{
	[JsonPropertyName("target")] public string Target { get; set; } = string.Empty;
	[JsonPropertyName("data")] public ActionWrapper<TParams> Data { get; set; } = new();

	public MessageEnvelope<ErrorParameters> FromError (ProtocolException ex)
	{
		return new MessageEnvelope<ErrorParameters>()
		{
			Target = "error",
			Data = new ActionWrapper<ErrorParameters>()
			{
				Action = ex.Action,
				Params = new ErrorParameters()
				{
					Source = this
				}
			}
		};
	}
}