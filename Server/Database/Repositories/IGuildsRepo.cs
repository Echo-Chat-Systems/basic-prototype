using EchoLib.Core.Snowflake;
using Server.Database.Discovery;
using Server.Database.Models.Chat;
using Server.Database.Repositories.Impl;

namespace Server.Database.Repositories;

[Repo(typeof(PgGuildsRepo))]
public interface IGuildsRepo : IRepo<GuildDbm, Snowflake, GuildDbm.New>
{

}