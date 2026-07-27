using EchoLib.Core;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Params.Guilds;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Database.Models.Chat;
using Server.Targets.PreProcessors;

namespace Server.Targets;

public class GuildsTarget(
	ILogger<GuildsTarget> logger,
	DbHub hub
)
	: ITarget
{
	public string Name => "guilds";

	[Route("create")]
	[Authenticated]
	public async Task Create(RoutingContext ctx, GuildCreateParams para)
	{
	}

	[Route("delete")]
	public async Task Delete(RoutingContext ctx, GuildDeleteParams para)
	{
	}

	[Route("get")]
	public async Task Get(RoutingContext ctx, GuildGetParams para)
	{
	}

	[Route("query")]
	[Authenticated]
	public async Task Query(RoutingContext ctx, GuildQueryParams para)
	{
		logger.LogDebug("balls");

		IEnumerable<Snowflake> ids = (await hub.GuildMembers.QueryAsync(ctx.User!))
			.Select(m => m.GuildId);

		// Get all guilds
		List<GuildDbm> dbGuilds = [];

		foreach (Snowflake id in ids)
		{
			GuildDbm? guild = await hub.Guilds.GetAsync(id);

			if (guild != null) dbGuilds.Add(guild);
		}

		// Construct each guild
		List<JGuild> guilds = [];

		foreach (GuildDbm dbm in dbGuilds)
		{
			List<JGuildMember> members = (await hub.GuildMembers.QueryAsync(dbm.Id)).Select(DtoMapper.Map<GuildMemberDbm, JGuildMember>).ToList();
			List<JChannel> channels = (await hub.Channels.QueryAsync(dbm.Id)).Select(DtoMapper.Map<ChannelDbm, JChannel>).ToList();

			guilds.Add(new JGuild
			{
				Id = dbm.Id,
				Name = dbm.Name,
				Owner = dbm.OwnerId,
				Members = members,
				Channels = channels,
				Config = dbm.Config,
				Customisation = dbm.Customisation
			});
		}

		await ctx.ReplyAsync(new GuildQueryResponseParams
		{
			Guilds = guilds
		});


	}
}