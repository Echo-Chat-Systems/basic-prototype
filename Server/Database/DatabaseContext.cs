using System.Data.Common;

namespace Server.Database;

public sealed class DatabaseContext(DbConnection connection)
{
	public DbConnection Connection { get; } = connection;
}