namespace Client;

internal class Program
{
	public static Client Client;

	private static void Main(string[] args)
	{
		Client = new Client();
		Client.Run();
	}
}