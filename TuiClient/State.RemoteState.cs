using TuiClient.Events;

namespace TuiClient;

public class RemoteState
{
	public delegate void ServerNameChangedEventHandler(ServerNameChangedEventArgs e);

	public event ServerNameChangedEventHandler? OnServerNameChanged;

	public string? ServerName
	{
		get;
		set
		{
			string? oldName = field;
			field = value;

			OnServerNameChanged?.Invoke(new ServerNameChangedEventArgs(oldName, value));
		}
	}
}