using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgRolesRepo))]
public interface IRolesRepo : IRepo<RoleDbm, Snowflake, RoleDbm.New>
{
	/// <summary>
	/// Queries for all roles in a specific guild.
	/// </summary>
	public IEnumerable<RoleDbm> Query(Snowflake guildId);

	/// <inheritdoc cref="IRolesRepo.Query(Snowflake)"/>
	public Task<IEnumerable<RoleDbm>> QueryAsync(Snowflake guildId);

	/// <summary>
	/// Queries for all roles a specific user has in.
	/// </summary>
	public IEnumerable<RoleDbm> Query(PublicSigningKey userId);

	/// <inheritdoc cref="IRolesRepo.Query(PublicSigningKey)"/>
	public Task<IEnumerable<RoleDbm>> QueryAsync(PublicSigningKey userId);
}