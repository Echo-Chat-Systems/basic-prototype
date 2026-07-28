using System.ComponentModel.DataAnnotations.Schema;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models.Public;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public sealed class GuildMember
{
	public required Snowflake Id { get; init; }
	public required JGuildCustomisation GuildCustomisationOverride { get; set; }
	public required JProfile UserProfileOverride { get; set; }

	public string? Nickname { get; set; }

	public required Snowflake GuildId { get; init; }
	public required PublicSigningKey UserId { get; init; }


	[ForeignKey(nameof(GuildId))] public Guild Guild { get; init; } = null!;
	[ForeignKey(nameof(UserId))] public User User { get; init; } = null!;
}