using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Sovereign.Core.Model.Response.Api;

public class BloxlinkRobloxIdResponse : BaseResponse
{
    [JsonPropertyName("robloxID")]
    public string RobloxID { get; set; }
    public override JsonTypeInfo GetJsonTypeInfo()
    {
        return BloxlinkRobloxIdResponseJsonContext.Default.BloxlinkRobloxIdResponse;
    }
}

[JsonSerializable(typeof(BloxlinkRobloxIdResponse))]
[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class BloxlinkRobloxIdResponseJsonContext : JsonSerializerContext
{
}