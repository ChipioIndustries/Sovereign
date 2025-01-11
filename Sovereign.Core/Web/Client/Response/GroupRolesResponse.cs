using System.Collections.Generic;
using System.Text.Json.Serialization;
using Bouncer.Web.Client.Response;

namespace Sovereign.Core.Web.Client.Response;

public class GroupRole
{
    /// <summary>
    /// Id of the role.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Name of the role.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Description of the role.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Rank of the role.
    /// </summary>
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
    
    /// <summary>
    /// Total members that have the role.
    /// </summary>
    [JsonPropertyName("memberCount")]
    public int MemberCount { get; set; }
}

public class GroupRolesResponse : BaseRobloxOpenCloudResponse
{
    /// <summary>
    /// Id of the group the request is for.
    /// </summary>
    [JsonPropertyName("groupId")]
    public long GroupId { get; set; }
    
    /// <summary>
    /// Roles in the group.
    /// </summary>
    [JsonPropertyName("groupRoles")]
    public List<GroupRole> GroupRoles { get; set; } = null!;
}

[JsonSerializable(typeof(GroupRolesResponse))]
[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class GroupRolesResponseJsonContext : JsonSerializerContext
{
}