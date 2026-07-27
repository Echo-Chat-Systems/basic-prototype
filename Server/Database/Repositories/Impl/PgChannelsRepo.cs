using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Server.Database.Models.Chat;

namespace Server.Database.Repositories.Impl;

public class PgChannelsRepo(ILogger<PgChannelsRepo> logger, IDbConnectionProvider connectionProvider)
	: BaseRepo<ChannelDbm, Snowflake, ChannelDbm.New>(logger, connectionProvider), IChannelsRepo
{
	protected override string GetQuery => "SELECT * FROM chat.channels WHERE id = @Id;";
	protected override string InsertQuery => "INSERT INTO chat.channels VALUES (@Id, @GuildId, @Name, @Parent, @Index, @Customisation, @Config) RETURNING *;";

	protected override string UpdateQuery =>
		"UPDATE chat.channels SET name = @Name, parent = @Parent, index = @Index, customisation = @Customisation, config = @Config WHERE id = @Id RETURNING *;";

	protected override string DeleteQuery => "DELETE FROM chat.channels WHERE id = @Id;";
	private const string GuildQuery = "SELECT * FROM chat.channels WHERE guild_id = @GuildId;";

	#region Generics

	public new ChannelDbm? Get(Snowflake id) => base.Get(id);

	public new Task<ChannelDbm?> GetAsync(Snowflake id) => base.GetAsync(id);

	public new ChannelDbm Insert(ChannelDbm.New item) => base.Insert(item);

	public new Task<ChannelDbm> InsertAsync(ChannelDbm.New item) => base.InsertAsync(item);

	public new ChannelDbm Update(ChannelDbm item) => base.Update(item);

	public new Task<ChannelDbm> UpdateAsync(ChannelDbm item) => base.UpdateAsync(item);

	public new void Delete(Snowflake id) => base.Delete(id);

	public new Task DeleteAsync(Snowflake id) => base.DeleteAsync(id);

	#endregion

	public IEnumerable<ChannelDbm> Query(Snowflake guildId) => Many(nameof(Query), GuildQuery, guildId);

	public Task<IEnumerable<ChannelDbm>> QueryAsync(Snowflake guildId) => ManyAsync(nameof(QueryAsync), GuildQuery, guildId);
}