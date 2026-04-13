namespace Server.Database.Repositories.Impl;

public class BasePostgresRepo(IServiceProvider services)
{
	public readonly IServiceProvider Services = services;
	
	
}