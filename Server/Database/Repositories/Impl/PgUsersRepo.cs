using Dapper;
using EchoLib.Crypto.Signing;
using Microsoft.Extensions.Logging;
using Npgsql;
using Server.Database.Discovery;
using Server.Database.Models.Public;

namespace Server.Database.Repositories.Impl;

public class PgUsersRepo(ILogger<PgUsersRepo> logger, IDbConnectionProvider connectionProvider)
	: BaseRepo<UserDbm, PublicSigningKey, UserDbm.New>(logger, connectionProvider), IUsersRepo
{
	protected override string GetQuery => "SELECT * FROM public.users WHERE id = @Id;";

	protected override string InsertQuery =>
		"INSERT INTO public.users VALUES (@Id, default, @Ek, @Username, @Tag, @Profile, @Settings, @LastOnline, @IsOnline, @IsBanned) RETURNING *;";

	protected override string UpdateQuery =>
		"UPDATE public.users SET username = @Username, tag = @Tag, profile = @Profile, settings = @Settings, last_online = @LastOnline, is_online = @IsOnline, is_banned = @IsBanned WHERE id = @Id RETURNING *";

	protected override string DeleteQuery => throw new InvalidOperationException("Cannot delete user.");

	#region Generics

	public new UserDbm? Get(PublicSigningKey id) => base.Get(id);
	public new Task<UserDbm?> GetAsync(PublicSigningKey id) => base.GetAsync(id);
	public new UserDbm Insert(UserDbm.New user) => base.Insert(user);
	public new Task<UserDbm> InsertAsync(UserDbm.New user) => base.InsertAsync(user);
	public new UserDbm Update(UserDbm item) => base.Update(item);
	public new Task<UserDbm> UpdateAsync(UserDbm item) => base.UpdateAsync(item);

	#endregion

	public new void Delete(PublicSigningKey item)
	{
		logger.LogError("Cannot delete users! Ban a user to remove their access.");
	}

	public new Task DeleteAsync(PublicSigningKey item)
	{
		logger.LogError("Cannot delete users! Ban a user to remove their access.");
		return Task.CompletedTask;
	}
}