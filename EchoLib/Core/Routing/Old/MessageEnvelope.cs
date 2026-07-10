using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using EchoLib.Core.Routing.Exceptions;
using EchoLib.Protocol.Models.Data;
using EchoLib.Protocol.Models.Params.Generic;
using Newtonsoft.Json;

namespace EchoLib.Core.Routing;

public class MessageEnvelope<TParams>
{
	[JsonProperty("message-id")] public Guid MessageId { get; set; } = Guid.Empty;
	[JsonProperty("target")] [JsonRequired] public string Target { get; set; } = string.Empty;
	[JsonProperty("data")] [JsonRequired] public ActionWrapper<TParams> Data { get; set; } = new();

	public MessageEnvelope<ErrorParameters> FromError(ProtocolException ex, Guid messageId)
	{
		return new MessageEnvelope<ErrorParameters>
		{
			MessageId = messageId,
			Target = "error",
			Data = new ActionWrapper<ErrorParameters>
			{
				Action = "error",
				Params = new ErrorParameters
				{
					Message = ex.Message,
					Source = new JErrorSourceData {Target = Target, Action = Data.Action}
				}
			}
		};
	}
}