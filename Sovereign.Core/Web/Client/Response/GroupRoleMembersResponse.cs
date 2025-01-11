using System.Collections.Generic;
using System.Text.Json.Serialization;
using Bouncer.Web.Client.Response;

namespace Sovereign.Core.Web.Client.Response;

public class GroupRoleMember
{
    /// <summary>
    /// Whether the user has the Verified badge.
    /// </summary>
    [JsonPropertyName("hasVerifiedBadge")]
    public bool HasVerifiedBadge { get; set; }
    
    /// <summary>
    /// Id of the Roblox user.
    /// </summary>
    [JsonPropertyName("userId")]
    public long UserId { get; set; }
    
    /// <summary>
    /// Username of the Roblox user.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Display name of the Roblox user.
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = null!;
}

public class GroupRoleMembersResponse : BaseRobloxOpenCloudResponse
{
    /// <summary>
    /// Cursor for the previous page of group members.
    /// </summary>
    [JsonPropertyName("previousPageCursor")]
    public string? PreviousPageCursor { get; set; }
    
    /// <summary>
    /// Cursor for the next page of group members.
    /// </summary>
    [JsonPropertyName("nextPageCursor")]
    public string? NextPageCursor { get; set; }

    /// <summary>
    /// List of members in the role for the current page.
    /// </summary>
    [JsonPropertyName("data")]
    public List<GroupRoleMember> Data { get; set; } = null!;
}

[JsonSerializable(typeof(GroupRoleMembersResponse))]
[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class GroupRoleMembersResponseJsonContext : JsonSerializerContext
{
}