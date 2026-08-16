using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EchoLib.Core.Snowflake;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data;
using EchoLib.Models.Data.Guild;
using Microsoft.EntityFrameworkCore;
using Server.Database.Models.Public;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class Guild
{
	public required Snowflake Id { get; init; }
	public required string Name { get; set; }
	public required JGuildCustomisation Customisation { get; set; }
	public required JGuildConfig Config { get; set; }

	public required PublicSigningKey OwnerId { get; init; }

	[ForeignKey(nameof(OwnerId))] public virtual User Owner { get; private init; } = null!;
}