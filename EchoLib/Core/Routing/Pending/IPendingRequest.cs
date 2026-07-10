using System.Text.Json;

namespace EchoLib.Core.Routing.Pending;

public interface IPendingRequest
{
	void Complete(JsonElement json);
}