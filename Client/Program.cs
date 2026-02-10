namespace Client;

class Program
{
	public static Client Client;
	
	static void Main(string[] args)
	{
		Console.WriteLine("Starting client...");
		Client = new Client("ws://localhost:69");
		Client.Run();
	}
}