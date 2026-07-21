namespace TuiClient;

public class State(IServiceProvider services)
{
	private IServiceProvider _services = services;

	public readonly LocalState Local = new();
	public readonly NetState Net = new(services);
}