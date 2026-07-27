using Dapper;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Npgsql;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public class PgChannelMembersRepo(ILogger<PgChannelMembersRepo> logger, IDbConnectionProvider connectionProvider)
	: BaseRepo<ChannelMemberDbm, Snowflake, ChannelMemberDbm.New>(logger, connectionProvider), IChannelMembersRepo
{
	protected override string GetQuery => "SELECT * FROM chat.channel_members WHERE id = @Id;";
	protected override string InsertQuery => "INSERT INTO chat.channel_members VALUES (@Id, @UserId, @ChannelId, @Permissions) RETURNING *;";
	protected override string UpdateQuery => "UPDATE chat.channel_members SET permissions = @Permissions WHERE id = @Id RETURNING *;";
	protected override string DeleteQuery => "DELETE FROM chat.channel_members WHERE id = @Id";

	private const string ChanelQuery = "SELECT * FROM chat.channel_members WHERE channel_id = @ChannelId;";
	private const string UserQuery = "SELECT * FROM chat.channel_members WHERE user_id = @UserId";

	#region Generics

	public new ChannelMemberDbm? Get(Snowflake id) => base.Get(id);
	public new Task<ChannelMemberDbm?> GetAsync(Snowflake id) => base.GetAsync(id);
	public new ChannelMemberDbm Insert(ChannelMemberDbm.New item) => base.Insert(item);
	public new Task<ChannelMemberDbm> InsertAsync(ChannelMemberDbm.New item) => base.InsertAsync(item);
	public new ChannelMemberDbm Update(ChannelMemberDbm item) => base.Update(item);
	public new Task<ChannelMemberDbm> UpdateAsync(ChannelMemberDbm item) => base.UpdateAsync(item);
	public new void Delete(Snowflake id) => base.Delete(id);
	public new Task DeleteAsync(Snowflake id) => base.DeleteAsync(id);

	#endregion

	public IEnumerable<ChannelMemberDbm> Query(Snowflake channelId) => Many(nameof(Query), ChanelQuery, channelId);

	public Task<IEnumerable<ChannelMemberDbm>> QueryAsync(Snowflake channelId) => ManyAsync(nameof(QueryAsync), ChanelQuery, channelId);

	public IEnumerable<ChannelMemberDbm> Query(PublicSigningKey userId) => Many(nameof(Query), UserQuery, userId);

	public  Task<IEnumerable<ChannelMemberDbm>> QueryAsync(PublicSigningKey userId) => ManyAsync(nameof(QueryAsync), UserQuery, userId);
}