using EchoLib.Configuration;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Encryption;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Database.Converters;
using Server.Database.Models.Chat;
using Server.Database.Models.Public;
using PublicSigningKeyConverter = Server.Database.Converters.PublicSigningKeyConverter;

namespace Server.Database;

public class EchoContext : Microsoft.EntityFrameworkCore.DbContext
{
	public DbSet<User> Users { get; set; }

	public DbSet<Channel> Channels { get; set; }
	public DbSet<ChannelMember> ChannelMembers { get; set; }
	public DbSet<Guild> Guilds { get; set; }
	public DbSet<GuildMember> GuildMembers { get; set; }

	public static string ConnectionString = null!;

	public EchoContext(DbContextOptions opt) : base(opt)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseLazyLoadingProxies()
			.UseNpgsql(ConnectionString);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder c)
	{
		c
			.Properties<PublicSigningKey>()
			.HaveConversion<PublicSigningKeyConverter>();
		c
			.Properties<PublicEncryptionKey>()
			.HaveConversion<PublicEncryptionKeyConverter>();
		c
			.Properties<Snowflake>()
			.HaveConversion<SnowflakeConverter>();

		// User
		c
			.Properties<JProfile>()
			.HaveConversion<JsonConverter<JProfile>>();

		// Channel
		c
			.Properties<JChannelCustomisation>()
			.HaveConversion<JsonConverter<JChannelCustomisation>>();
		c
			.Properties<JChannelConfig>()
			.HaveConversion<JsonConverter<JChannelConfig>>();

		// Guild
		c
			.Properties<JGuildCustomisation>()
			.HaveConversion<JsonConverter<JGuildCustomisation>>();
		c
			.Properties<JGuildConfig>()
			.HaveConversion<JsonConverter<JGuildConfig>>();

		// Role
		c
			.Properties<JRoleCustomisation>()
			.HaveConversion<JsonConverter<JRoleCustomisation>>();
		c
			.Properties<JRolePermissionSet>()
			.HaveConversion<JsonConverter<JRolePermissionSet>>();
	}

	protected override void OnModelCreating(ModelBuilder m)
	{
		base.OnModelCreating(m);
	}
}
