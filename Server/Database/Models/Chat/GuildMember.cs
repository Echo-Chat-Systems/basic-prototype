using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.Guild;
using EchoLib.Models.Data.User;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models.Public;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class GuildMember
{
	public required Snowflake Id { get; init; }
	public string? Nickname { get; set; }
	public required JGuildCustomisation GuildCustomisationOverride { get; set; }
	public required JProfile UserProfileOverride { get; set; }

	public required Guild Guild { get; set; }
	public required User User { get; set; }
}