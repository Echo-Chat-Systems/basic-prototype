using EchoLib.Models.Data;

namespace EchoLib.Core.Routing.Events;

public class ProtocolErrorEvent : IEvent
{
	public required string Message { get; init; }
	public required JErrorSourceData Source { get; init; } 
}