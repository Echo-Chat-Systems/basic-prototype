using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TuiClient.Events;

namespace TuiClient;

public class RemoteState
{
	private readonly ILogger<RemoteState> _logger = Program.Services.GetRequiredService<ILogger<RemoteState>>();

	public delegate void ServerNameChangedEventHandler(ServerNameChangedEventArgs e);

	public event ServerNameChangedEventHandler? OnServerNameChanged;

	public string? ServerName
	{
		get;
		set
		{
			string? oldName = field;
			field = value;

			_logger.LogDebug("Server name updated {Old} -> {New}", oldName, value);

			OnServerNameChanged?.Invoke(new ServerNameChangedEventArgs(oldName, value));
		}
	}
}