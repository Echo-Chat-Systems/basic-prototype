using System.ComponentModel.DataAnnotations.Schema;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Channel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class Channel
{
	public required Snowflake Id { get; init; }

	public required string Name { get; set; }
	public required JChannelCustomisation Customisation { get; set; }
	public required JChannelConfig Config { get; set; }

	public int? Index { get; set; }
	
	public Snowflake? GuildId { get; init; }
	public Snowflake? ParentId { get; init; }

	[ForeignKey(nameof(GuildId))] public virtual Guild Guild { get; private init; } = null!;
	[ForeignKey(nameof(ParentId))] public virtual Channel Parent { get; private init; } = null!;

	public virtual List<ChannelMember> Members { get; } = [];
}

public class ChannelConfiguration : IEntityTypeConfiguration<Channel>
{
	public void Configure(EntityTypeBuilder<Channel> builder)
	{
	}
}