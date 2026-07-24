using System.Collections.Generic;
using EchoLib.Routing.Identification;

namespace AvaloniaClient.Targets;

public class TargetHub : ITargetHub
{
	public AuthTarget Auth { get; private set; } = null!;
}