using Newtonsoft.Json;

namespace EchoLib.Models.Params;

public interface IParam
{
	[JsonIgnore] string Action { get; }
}