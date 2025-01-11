using System.Collections.Generic;
using System.Threading.Tasks;
using Bouncer.Web.Client;
using Bouncer.Web.Client.Shim;
using Sovereign.Core.Web.Client.Response;
using GroupRoleMember = Sovereign.Core.Web.Client.Response.GroupRoleMember;
using GroupRoleMembersResponse = Sovereign.Core.Web.Client.Response.GroupRoleMembersResponse;
using GroupRoleMembersResponseJsonContext = Sovereign.Core.Web.Client.Response.GroupRoleMembersResponseJsonContext;

namespace Sovereign.Core.Web.Client;

public class RobloxExtendedGroupClient : RobloxGroupClient
{
    /// <summary>
    /// Roblox Open Cloud client to send caching requests.
    /// </summary>
    private readonly RobloxOpenCloudClient _cachingRobloxClient;
    
    /// <summary>
    /// Creates an extended Roblox Group API client.
    /// </summary>
    /// <param name="httpClient">HTTP client to send requests with.</param>
    /// <param name="cachingHttpClient">Caching HTTP client to send requests with.</param>
    public RobloxExtendedGroupClient(IHttpClient httpClient, IHttpClient cachingHttpClient) : base(httpClient, cachingHttpClient)
    {
        this._cachingRobloxClient = new RobloxOpenCloudClient(cachingHttpClient);
    }
    
    /// <summary>
    /// Creates an extended Roblox Group API client.
    /// </summary>
    public RobloxExtendedGroupClient() : this(new HttpClientImpl(), CachingHttpClient.Instance)
    {
        
    }

    /// <summary>
    /// Returns the roles for a group.
    /// </summary>
    /// <param name="groupId">Id of the group to get the roles for.</param>
    /// <returns></returns>
    public async Task<GroupRolesResponse> GetGroupRoles(long groupId)
    {
        return await this._cachingRobloxClient.GetAsync($"https://groups.roblox.com/v1/groups/{groupId}/roles", GroupRolesResponseJsonContext.Default.GroupRolesResponse);
    }

    /// <summary>
    /// Returns the group members for a page of a role.
    /// </summary>
    /// <param name="groupId">Id of the group to fetch for.</param>
    /// <param name="roleId">Id of the role to fetch for.</param>
    /// <param name="cursor">Optional cursor to fetch users from.</param>
    /// <param name="limit">Optional maximum amount of users to return in the list.</param>
    /// <param name="sortOrder">Optional sort order for the members.</param>
    /// <returns></returns>
    public async Task<GroupRoleMembersResponse> GetGroupMembersInRoleForPage(long groupId, long roleId, string? cursor = null, int limit = 100, string sortOrder = "Desc")
    {
        return await this._cachingRobloxClient.GetAsync($"https://groups.roblox.com/v1/groups/{groupId}/roles/{roleId}/users?cursor={cursor ?? ""}&limit={limit}&sortOrder={sortOrder}", GroupRoleMembersResponseJsonContext.Default.GroupRoleMembersResponse);
    }

    /// <summary>
    /// Returns the group members with a role.
    /// This does not properly handle long lists of pages. It should be used for only a few hundred members at most.
    /// </summary>
    /// <param name="groupId">Id of the group to fetch for.</param>
    /// <param name="roleId">Id of the role to fetch for.</param>
    /// <returns>All the members in the role for a group.</returns>
    public async Task<List<GroupRoleMember>> GetGroupMembersInRole(long groupId, long roleId)
    {
        var roleMembers = new List<GroupRoleMember>();
        string? lastPageCursor = null;
        while (true)
        {
            var response = await this.GetGroupMembersInRoleForPage(groupId, roleId, cursor: lastPageCursor);
            roleMembers.AddRange(response.Data);
            lastPageCursor = response.NextPageCursor;
            if (lastPageCursor == null) break;
        }
        return roleMembers;
    }
}