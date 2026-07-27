using EchoLib.Core.Snowflake;

namespace EchoLib.Models.Data.Guild;

public class JRole
{
	public required Snowflake Id { get; init; }
	public required string Name { get; set; }
	public required JRolePermissionSet Permissions { get; set; }
}