using Server.Database.Repositories;

namespace Server.Database;

public class DbHub
{
	public required IUsersRepo Users { get; init; }
}