using EchoLib.Core;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Channel;
using EchoLib.Models.Params.Channels;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Database.Models.Chat;
using Server.Targets.PreProcessors;

namespace Server.Targets;

public class ChannelsTarget(ILogger<ChannelsTarget> logger, EchoContext db) : TargetBase<ChannelsTarget>(logger)
{
	public override string Name => "channels";

	[Route(RouteNames.Channels.Create)]
	[Authenticated]
	public async Task HandleCreate(RoutingContext ctx, ChannelCreateParams para)
	{
		await GuildMemberCheck(ctx.User, para.Guild);
		
		// Now check their permissions 
		// TODO: Permissions checks here - !!! IMPORTANT !!!

		EntityEntry<Channel> channel = await db.Channels.AddAsync(new Channel
		{
			Id = SnowflakeGenerator.New(),
			Name = para.Name,
			Customisation = JChannelCustomisation.Empty,
			Config = JChannelConfig.Empty
		});
		await db.SaveChangesAsync();
		
		// Now get this channel
		await ctx.ReplyAsync(new ChannelCreateResponseParams
		{
			Id = channel.Entity.Id
		});
	}

	[Route(RouteNames.Channels.Get)]
	[Authenticated]
	public async Task HandleGet(RoutingContext ctx, ChannelGetParams para)
	{
		await GuildMemberCheck(ctx.User, para.Id);
		
		// Check if channel exists
		Channel? channel = await db.Channels.FindAsync(para.Id);
		if (channel == null) throw new NotFoundException();
		
		// Get all channel members
		// List<ChannelMember> members = await db.ChannelMembers.Where(c => c.ChannelId == channel.Id).ToListAsync();
		
		await ctx.ReplyAsync(new ChannelGetResponseParams
		{
			Channel = new JChannel
			{
				Name = channel.Name,
				Config = channel.Config,
				Customisation = channel.Customisation
			}
		});
	}
	
	private async Task GuildMemberCheck(PublicSigningKey? userId, Snowflake guildId)
	{
		if (userId is null) throw new UnauthorizedException();
		if (await db.GuildMembers.Where(m => m.GuildId == guildId && m.UserId == userId).FirstOrDefaultAsync() == null) throw new NotFoundException();
	}
}