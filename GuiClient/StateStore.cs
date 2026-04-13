using WebSocketSharper;

namespace GuiClient;

public class StateStore
{
	private WebSocket? _socket;
	private UserFile? _userFile;

	private string _serverName = "";

	public bool SocketConnected { get; set; } = false;

	public string ServerName
	{
		get => _serverName;
		set
		{
			ServerNameUpdated.Invoke(this, new ServerNameChangedEventArgs { OldName = _serverName, NewName = value });
			_serverName = value;
		}
	}


	#region Events

	public event EventHandler<ServerNameChangedEventArgs> ServerNameUpdated = null!;

	public class ServerNameChangedEventArgs : EventArgs
	{
		public required string OldName { get; set; }
		public required string NewName { get; set; }
	}

	#endregion

	public WebSocket? Socket
	{
		get => _socket;
		set
		{
			if (_socket != null) throw new Exception("Cannot set socket, already set!");
			_socket = value;
		}
	}

	public UserFile? UserFile
	{
		get => _userFile;
		set
		{
			if (_userFile != null) throw new Exception("Cannot set userfile, already set!");
			_userFile = value;
		}
	}
}