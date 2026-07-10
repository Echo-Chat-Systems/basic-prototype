using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Params;

public interface IParam
{
	[JsonIgnore] string Action { get; }
}