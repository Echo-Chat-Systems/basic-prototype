using Core.Models.Permissions;

namespace EchoLib.Models.Data.Guild;

public class JRolePermissionSet
{
	public GuildPermissions? Guild { get; set; }
	public TextChannelPermissions? Text { get; set; }
	public VoiceChannelPermissions? Voice { get; set; }
}