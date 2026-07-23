using EchoLib.Models.Crypto;

namespace TuiClient;

public class State(IServiceProvider services)
{
	private IServiceProvider _services = services;

	public readonly LocalState Local = new();
	public readonly NetState Net = new(services);
	public readonly RemoteState Remote = new();

	public EchoClient? ProtocolClient { get; set; }

	public JKeySet Keys => Local.UserFile is null ? throw new NullReferenceException() : Local.UserFile.Keys;
}