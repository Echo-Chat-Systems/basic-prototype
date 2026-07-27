using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgGuildMembersRepo))]
public interface IGuildMembersRepo : IRepo<GuildMemberDbm, Snowflake, GuildMemberDbm.New>
{
	/// <summary>
	/// Queries for all users in a specific guild.
	/// </summary>
	public IEnumerable<GuildMemberDbm> Query(Snowflake guildId);

	/// <inheritdoc cref="IGuildMembersRepo.Query(Snowflake)"/>
	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(Snowflake guildId);

	/// <summary>
	/// Queries for all guilds a specific user is in.
	/// </summary>
	public IEnumerable<GuildMemberDbm> Query(PublicSigningKey userId);

	/// <inheritdoc cref="IGuildMembersRepo.Query(PublicSigningKey)"/>
	public Task<IEnumerable<GuildMemberDbm>> QueryAsync(PublicSigningKey userId);
}