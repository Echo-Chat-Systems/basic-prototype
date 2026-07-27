using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgGuildsRepo(ILogger logger, IDbConnectionProvider connectionProvider) : BaseRepo<GuildDbm, Snowflake, GuildDbm.New>(logger, connectionProvider), IGuildsRepo
{

	protected override string GetQuery => "";
	protected override string InsertQuery => "";
	protected override string UpdateQuery => "";
	protected override string DeleteQuery => "";

	#region Generics

	public new GuildDbm? Get(Snowflake id) => base.Get(id);
	public new Task<GuildDbm?> GetAsync(Snowflake id) => base.GetAsync(id);
	public new GuildDbm Insert(GuildDbm.New item) => base.Insert(item);
	public new Task<GuildDbm> InsertAsync(GuildDbm.New item) => base.InsertAsync(item);
	public new GuildDbm Update(GuildDbm item) => base.Update(item);
	public new Task<GuildDbm> UpdateAsync(GuildDbm item) => base.UpdateAsync(item);
	public new void Delete(Snowflake item) => base.Delete(item);
	public new Task DeleteAsync(Snowflake item) => base.DeleteAsync(item);
	#endregion
}