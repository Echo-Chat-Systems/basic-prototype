using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgGuildMembersRepo(ILogger<PgGuildMembersRepo> logger, IDbConnectionProvider connectionProvider)
	: BaseRepo<GuildMemberDbm, Snowflake, GuildMemberDbm.New>(logger, connectionProvider), IGuildMembersRepo
{
	protected override string GetQuery => "SELECT * FROM chat.guild_members WHERE id = @Id;";
	protected override string InsertQuery => "";
	protected override string UpdateQuery => "";
	protected override string DeleteQuery => "";

	#region Generics

	public new GuildMemberDbm? Get(Snowflake id) => base.Get(id);
	public new Task<GuildMemberDbm?> GetAsync(Snowflake id) => base.GetAsync(id);
	public new GuildMemberDbm Insert(GuildMemberDbm.New item) => base.Insert(item);
	public new Task<GuildMemberDbm> InsertAsync(GuildMemberDbm.New item) => base.InsertAsync(item);
	public new GuildMemberDbm Update(GuildMemberDbm item) => base.Update(item);
	public new Task<GuildMemberDbm> UpdateAsync(GuildMemberDbm item) => base.UpdateAsync(item);
	public new void Delete(Snowflake item) => base.Delete(item);
	public new Task DeleteAsync(Snowflake item) => base.DeleteAsync(item);

	#endregion

	public IEnumerable<GuildMemberDbm> Query(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(Snowflake guildId)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<GuildMemberDbm> Query(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(PublicSigningKey userId)
	{
		throw new NotImplementedException();
	}
}