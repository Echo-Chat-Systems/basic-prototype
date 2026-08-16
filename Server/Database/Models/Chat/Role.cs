using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EchoLib.Core.Snowflake;
using EchoLib.Models.Data.Guild;
using Microsoft.EntityFrameworkCore;

namespace Server.Database.Models.Chat;

[PrimaryKey(nameof(Id))]
public class Role
{
	public required Snowflake Id { get; init; }
	public required string Name { get; set; }
	public required JRoleCustomisation Customisation { get; set; }
	public required JRolePermissionSet Permissions { get; set; }

	public required Snowflake GuildId { get; init; }

	[ForeignKey(nameof(GuildId))] public virtual Guild Guild { get; private init; } = null!;
}