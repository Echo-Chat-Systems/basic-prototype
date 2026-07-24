namespace TuiClient.Events;

public class ServerNameChangedEventArgs(string? oldName, string? name)
{
	public string? OldName { get; } = oldName;
	public string NewName { get; } = name;
}

