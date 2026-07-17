using Newtonsoft.Json;

namespace EchoLib.Protocol.Models.Data.User;

public class JProfileModel
{
	[JsonProperty("username")] public required string Username { get; init; }
	[JsonProperty("tag")] public required ushort Tag { get; set; }
	[JsonProperty("pronouns")] public string? Pronouns { get; set; }
	[JsonProperty("bio")] public string? Bio { get; set; }
	[JsonProperty("css")] public string? Css { get; set; }
	[JsonProperty("pfp")] public string? Pfp { get; set; }
	[JsonProperty("bammer")] public string? Banner { get; set; }
	[JsonProperty("timezone")] public string? Timezone { get; set; }
	[JsonProperty("status")] public JStatusModel? Status { get; init; }
}

public class JStatusModel
{
	[JsonProperty("type")] public required string Type { get; init; }
	[JsonProperty("text")] public required string Text { get; init; }
}