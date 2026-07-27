using EchoLib.Core.Snowflake;

namespace Server.Database.Models;

public abstract class BaseDbm<TNew> where TNew : BaseDbm<TNew>.NewBase
{
	public Snowflake Id { get; set; }

	public abstract class NewBase
	{
		public Snowflake Id;
	}
}