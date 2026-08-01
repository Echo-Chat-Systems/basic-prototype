using System.Diagnostics;
using EchoLib.Core;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;
using EchoLib.Models.Params.Guilds;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Database.Models.Chat;
using Server.Targets.PreProcessors;

namespace Server.Targets;

public class GuildsTarget(
	ILogger<GuildsTarget> logger,
	EchoContext db
)
	: ITarget
{
	public string Name => "guilds";

	[Route("create")]
	[Authenticated]
	public async Task Create(RoutingContext ctx, GuildCreateParams para)
	{
		// Create a new guild and guild member item for this user
		Debug.Assert(ctx.User != null);

		EntityEntry<Guild> guild = await db.Guilds.AddAsync(new Guild
		{
			Id = SnowflakeGenerator.New(),
			Name = para.Name,
			Customisation = JGuildCustomisation.Empty,
			Config = JGuildConfig.Empty,
			OwnerId = ctx.User
		});

		await db.GuildMembers.AddAsync(new GuildMember
		{
			Id = SnowflakeGenerator.New(),
			GuildCustomisationOverride = JGuildCustomisation.Empty,
			UserProfileOverride = JProfile.Empty,
			GuildId = guild.Entity.Id,
			UserId = ctx.User
		});

		await db.SaveChangesAsync();
		await ctx.ReplyAsync(new GuildCreateResponseParams
		{
			Id = guild.Entity.Id
		});
	}

	[Route("delete")]
	[Authenticated]
	public async Task Delete(RoutingContext ctx, GuildDeleteParams para)
	{
	}

	[Route("get")]
	[Authenticated]
	public async Task Get(RoutingContext ctx, GuildGetParams para)
	{
		// Ensure that the user is meant to be able to see this guild
		// Debugging tests
		var temp = await db.GuildMembers.ToListAsync();
		
		// This isn't working because para.Id is 0
		if (
			 (await db.GuildMembers
				.Where(m => m.UserId == ctx.User)
				.ToListAsync()
				).Where(m => m.GuildId == para.Id).Count() != 1) throw new UnauthorizedException();

		await ctx.ReplyAsync(new GuildGetResponseParams
		{
			Guild = await BuildGuild((await db.Guilds.Where(g => g.Id == para.Id).FirstOrDefaultAsync())!)
		});
	}

	[Route("query")]
	[Authenticated]
	public async Task Query(RoutingContext ctx, GuildQueryParams para)
	{
		IEnumerable<Snowflake> ids = await db.GuildMembers.Where(m => m.User.Id == ctx.User!).Select(m => m.Guild.Id).ToListAsync();

		// Get all guilds
		List<Guild> dbGuilds = [];

		foreach (Snowflake id in ids)
		{
			Guild? guild = await db.Guilds.FindAsync(id);

			if (guild != null) dbGuilds.Add(guild);
		}

		// Construct each guild
		List<JGuild> guilds = [];

		foreach (Guild dbm in dbGuilds)
		{
			guilds.Add(await BuildGuild(dbm));
		}

		await ctx.ReplyAsync(new GuildQueryResponseParams
		{
			Guilds = guilds
		});
	}

	private async Task<JGuild> BuildGuild(Guild g)
	{
		List<JGuildMember> members = await db.GuildMembers
			.Where(m => m.Guild.Id == g.Id)
			.AsAsyncEnumerable()
			.Select(DtoMapper.Map<GuildMember, JGuildMember>)
			.ToListAsync();

		List<JChannel> channels = await db.Channels.Where(m => m.Guild.Id == g.Id)
			.AsAsyncEnumerable()
			.Select(DtoMapper.Map<Channel, JChannel>)
			.ToListAsync();

		return new JGuild
		{
			Id = g.Id,
			Name = g.Name,
			Owner = g.OwnerId,
			Members = members,
			Channels = channels,
			Config = g.Config,
			Customisation = g.Customisation
		};
	}
}