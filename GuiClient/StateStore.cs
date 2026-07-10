using System.ComponentModel;
using System.Runtime.CompilerServices;
using WebSocketSharper;

namespace GuiClient;

public class StateStore : INotifyPropertyChanged
{
	private WebSocket? _socket;
	private UserFile? _userFile;

	public bool SocketConnected { get; set; } = false;

	public string ServerName { get; set; } = "Design-time server name";

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

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}
}