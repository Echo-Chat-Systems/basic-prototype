namespace Core.Models.Permissions;

/// <summary>
/// Guild permissions.
/// </summary>
[Flags]
public enum GuildPermissions : long
{
	Administrator = 0x8000_0000,
	ViewAuditLog = 0x4000_0000,
	ViewInsights = 0x2000_0000,
	ManageGuild = 0x1000_0000,
	DeleteRoles = 0x0800_0000,
	CreateRoles = 0x0400_0000,
	ManageRoles = 0x0200_0000,
	RemoveRoles = 0x0100_0000,
	AssignRoles = 0x0080_0000,
	DeleteChannels = 0x0040_0000,
	CreateChannels = 0x0020_0000,
	ManageChannels = 0x0010_0000,
	UnbanMembers = 0x0008_0000,
	BanMembers = 0x0004_0000,
	KickMembers = 0x0002_0000,
	ManageMembers = 0x0001_0000,
	ManageInvites = 0x0000_8000,
	DeleteEmojis = 0x0000_4000,
	CreateEmojis = 0x0000_2000,
	ManageEmojis = 0x0000_1000,
	DeleteStickers = 0x0000_0800,
	CreateStickers = 0x0000_0400,
	ManageStickers = 0x0000_0200,
	DeleteWebhooks = 0x0000_0100,
	CreateWebhooks = 0x0000_0080,
	ManageWebhooks = 0x0000_0040,
	DeleteEvents = 0x0000_0020,
	CreateEvents = 0x0000_0010,
	ManageEvents = 0x0000_0008,
	SendInvites = 0x0000_0004
}

/// <summary>
///  Extensions for <see cref="GuildPermissions"/> to handle OAuth scopes and permission checks.
/// </summary>
public static class GuildPermissionsExtensions
{
	public static Dictionary<string, GuildPermissions> OAuthMapping { get; } = new()
	{
		{ "guild:admin", GuildPermissions.Administrator },
		{ "guild:audit", GuildPermissions.ViewAuditLog },
		{ "guild:insights", GuildPermissions.ViewInsights },
		{ "guild:manage", GuildPermissions.ManageGuild },
		{ "guild:roles:delete", GuildPermissions.DeleteRoles },
		{ "guild:roles:create", GuildPermissions.CreateRoles },
		{ "guild:roles:manage", GuildPermissions.ManageRoles },
		{ "guild:roles:remove", GuildPermissions.RemoveRoles },
		{ "guild:roles:assign", GuildPermissions.AssignRoles },
		{ "guild:channels:delete", GuildPermissions.DeleteChannels },
		{ "guild:channels:create", GuildPermissions.CreateChannels },
		{ "guild:channels:manage", GuildPermissions.ManageChannels },
		{ "guild:members:unban", GuildPermissions.UnbanMembers },
		{ "guild:members:ban", GuildPermissions.BanMembers },
		{ "guild:members:kick", GuildPermissions.KickMembers },
		{ "guild:members:manage", GuildPermissions.ManageMembers },
		{ "guild:invites:manage", GuildPermissions.ManageInvites },
		{ "guild:emojis:delete", GuildPermissions.DeleteEmojis },
		{ "guild:emojis:create", GuildPermissions.CreateEmojis },
		{ "guild:emojis:manage", GuildPermissions.ManageEmojis },
		{ "guild:stickers:delete", GuildPermissions.DeleteStickers },
		{ "guild:stickers:create", GuildPermissions.CreateStickers },
		{ "guild:stickers:manage", GuildPermissions.ManageStickers },
		{ "guild:webhooks:delete", GuildPermissions.DeleteWebhooks },
		{ "guild:webhooks:create", GuildPermissions.CreateWebhooks },
		{ "guild:webhooks:manage", GuildPermissions.ManageWebhooks },
		{ "guild:events:delete", GuildPermissions.DeleteEvents },
		{ "guild:events:create", GuildPermissions.CreateEvents },
		{ "guild:events:manage", GuildPermissions.ManageEvents },
		{ "guild:invites:send", GuildPermissions.SendInvites }
	};

	/// <summary>
	/// Converts a list of scopes to a set of permissions.
	/// </summary>
	/// <param name="scopes">Source scopes list.</param>
	/// <returns>Permissions set.</returns>
	public static GuildPermissions FromOAuth(List<string> scopes)
	{
		GuildPermissions permissions = 0;

		foreach (string scope in scopes)
			if (OAuthMapping.TryGetValue(scope, out GuildPermissions permission))
				permissions |= permission;

		return permissions;
	}

	/// <summary>
	/// Converts a set of permissions to a list of OAuth scopes.
	/// </summary>
	/// <param name="permissions"></param>
	/// <returns></returns>
	public static List<string> ToOAuth(this GuildPermissions permissions)
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
	public static bool HasPermission(this GuildPermissions permissions, GuildPermissions permission)
	{
		return (permissions & permission) == permission;
	}

	/// <summary>
	/// Sets the specified permission.
	/// </summary>
	/// <param name="permissions">The permission set.</param>
	/// <param name="permission">The permission to add.</param>
	/// <returns>Updated permissions set.</returns>
	public static GuildPermissions AddPermission(this GuildPermissions permissions, GuildPermissions permission)
	{
		return permissions | permission;
	}

	/// <summary>
	/// Removes the specified permission.
	/// </summary>
	/// <param name="permissions">The permission set.</param>
	/// <param name="permission">The permission to remove.</param>
	/// <returns>Updated permissions set.</returns>
	public static GuildPermissions RemovePermission(this GuildPermissions permissions, GuildPermissions permission)
	{
		return permissions & ~permission;
	}
}