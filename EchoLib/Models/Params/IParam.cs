using System.Text.Json.Serialization;

namespace EchoLib.Models.Params;

public interface IParam
{
	[JsonIgnore] string Action { get; }
}