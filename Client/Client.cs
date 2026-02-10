using WebSocketSharp;

namespace Client;

public class Client
{
	public readonly WebSocket Socket;

	public Client(string url)
	{
		Socket = new WebSocket(url);
		
		Socket.OnMessage += OnMessage;
	}
	
	public void Run()
	{
		// Connect
		Socket.Connect();
		
		string? input;
		while (true)
		{
			Console.Write("Send message: ");
			input = Console.ReadLine();

			if (input is null) continue;

			Socket.Send(input);
		}
	}

	private void OnMessage(object? sender, MessageEventArgs e)
	{
		Console.WriteLine(e.Data);
	}
}