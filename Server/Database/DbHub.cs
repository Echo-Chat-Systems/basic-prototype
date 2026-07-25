using Server.Database.Repositories;

namespace Server.Database;

public class DbHub
(
	IUsersRepo users
)
{
	public IUsersRepo Users { get; } = users;
}