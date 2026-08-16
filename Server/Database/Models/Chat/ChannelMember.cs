using System.ComponentModel.DataAnnotations.Schema;
using Core.Models.Permissions;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models.Public;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class ChannelMember
{
	public required Snowflake Id { get; init; }
	public required TextChannelPermissions Permissions { get; init; }

	public required PublicSigningKey UserId { get; init; }
	public required Snowflake ChannelId { get; init; }

	[ForeignKey(nameof(UserId))] public virtual User User { get; init; } = null!;
	[ForeignKey(nameof(ChannelId))] public virtual Channel Channel { get; init; } = null!;
}