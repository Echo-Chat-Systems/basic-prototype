namespace Core.Models.Permissions;

/// <summary>
/// Voice channel permissions.
/// </summary>
[Flags]
public enum VoiceChannelPermissions : long
{
	ServerDeafenMembers = 0x8000_0000,
	ServerMuteMembers = 0x4000_0000,
	MoveMembers = 0x2000_0000,
	StreamScreens = 0x0000_0040,
	ViewScreenStreams = 0x0000_0020,
	StreamCamera = 0x0000_0010,
	ViewCameras = 0x0000_0008,
	UseVoiceActivity = 0x0000_0004,
	Speak = 0x0000_0002,
	Listen = 0x0000_0001
}

/// <summary>
/// Extensions for <see cref="VoiceChannelPermissions"/> to handle OAuth scopes and permission checks.
/// </summary>
public static class VoiceChannelPermissionsExtensions
{
	public static Dictionary<string, VoiceChannelPermissions> OAuthMapping { get; } = new()
	{
		{ "voice:global_deafen", VoiceChannelPermissions.ServerDeafenMembers },
		{ "voice:global_mute", VoiceChannelPermissions.ServerMuteMembers },
		{ "voice:move", VoiceChannelPermissions.MoveMembers },
		{ "voice:stream", VoiceChannelPermissions.StreamScreens },
		{ "voice:view_streams", VoiceChannelPermissions.ViewScreenStreams },
		{ "voice:camera", VoiceChannelPermissions.StreamCamera },
		{ "voice:view_cameras", VoiceChannelPermissions.ViewCameras },
		{ "voice:voice_activity", VoiceChannelPermissions.UseVoiceActivity },
		{ "voice:speak", VoiceChannelPermissions.Speak },
		{ "voice:listen", VoiceChannelPermissions.Listen }
	};

	/// <summary>
	/// Converts a list of scopes to a set of permissions.
	/// </summary>
	/// <param name="scopes">Source scopes list.</param>
	/// <returns>Permissions set.</returns>
	public static VoiceChannelPermissions FromOAuth(List<string> scopes)
	{
		VoiceChannelPermissions permissions = 0;
		foreach (string scope in scopes)
			if (OAuthMapping.TryGetValue(scope, out VoiceChannelPermissions permission))
				permissions |= permission;

		return permissions;
	}

	/// <summary>
	/// Converts a set of permissions to a list of OAuth scopes.
	/// </summary>
	/// <param name="permissions"></param>
	/// <returns></returns>
	public static List<string> ToOAuth(this VoiceChannelPermissions permissions)
	{
		List<string> scopes = [];
		scopes.AddRange(from kvp in OAuthMapping where permissions.HasPermission(kvp.Value) select kvp.Key);

		return scopes;
	}

	/// <summary>
	/// Checks if the permission is set.
	/// </summary>
	/// <param name="permissions">The permissions to check.</param>
	/// <param name="permission">The permission to check for.</param>
	/// <returns>True if the permission is set, otherwise false.</returns>
	public static bool HasPermission(this VoiceChannelPermissions permissions, VoiceChannelPermissions permission)
	{
		return (permissions & permission) == permission;
	}

	/// <summary>
	/// Sets the specified permission.
	/// </summary>
	/// <param name="permissions">The permission set.</param>
	/// <param name="permission">The permission to add.</param>
	/// <returns>Updated permissions set.</returns>
	public static VoiceChannelPermissions AddPermission(this VoiceChannelPermissions permissions, VoiceChannelPermissions permission)
	{
		return permissions | permission;
	}

	/// <summary>
	/// Removes the specified permission.
	/// </summary>
	/// <param name="permissions">The permission set.</param>
	/// <param name="permission">The permission to remove.</param>
	/// <returns>Updated permissions set.</returns>
	public static VoiceChannelPermissions RemovePermission(this VoiceChannelPermissions permissions, VoiceChannelPermissions permission)
	{
		return permissions & ~permission;
	}
}