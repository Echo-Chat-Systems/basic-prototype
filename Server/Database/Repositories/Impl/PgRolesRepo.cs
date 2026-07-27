using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgRolesRepo(ILogger logger, IDbConnectionProvider connectionProvider) : BaseRepo<RoleDbm, Snowflake, RoleDbm.New>(logger, connectionProvider), IRolesRepo
{
	protected override string GetQuery => "";
	protected override string InsertQuery => "";
	protected override string UpdateQuery => "";
	protected override string DeleteQuery => "";


	#region Generics

	public new RoleDbm? Get(Snowflake id) => base.Get(id);
	public new Task<RoleDbm?> GetAsync(Snowflake id) => base.GetAsync(id);
	public new RoleDbm Insert(RoleDbm.New item) => base.Insert(item);
	public new Task<RoleDbm> InsertAsync(RoleDbm.New item) => base.InsertAsync(item);
	public new RoleDbm Update(RoleDbm item) => base.Update(item);
	public new Task<RoleDbm> UpdateAsync(RoleDbm item) => base.UpdateAsync(item);
	public new void Delete(Snowflake id) => base.Delete(id);
	public new Task DeleteAsync(Snowflake id) => base.DeleteAsync(id);
	#endregion

	public IEnumerable<RoleDbm> Query(Snowflake guildId)
	{

	}

	public Task<IEnumerable<RoleDbm>> QueryAsync(Snowflake guildId)
	{

	}

	public IEnumerable<RoleDbm> Query(PublicSigningKey userId)
	{

	}

	public Task<IEnumerable<RoleDbm>> QueryAsync(PublicSigningKey userId)
	{
	}
}