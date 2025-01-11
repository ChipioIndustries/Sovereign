using System.Collections.Generic;
using System.Threading.Tasks;
using Bouncer.Diagnostic;
using Sovereign.Core.Web.Client;
using Sovereign.Core.Web.Client.Response;

namespace Sovereign.Api.Bans.Web.Client;

public class GroupRankState
{
    /// <summary>
    /// Maximum members in a group to fetch the group members of.
    /// This is to prevent getting all the members of large roles.
    /// </summary>
    public const int MaxMembersPerRank = 500;
    
    /// <summary>
    /// Id of the group to store.
    /// </summary>
    private readonly long _groupId;
    
    /// <summary>
    /// Client used for fetching group data.
    /// </summary>
    private readonly RobloxExtendedGroupClient _robloxExtendedGroupClient;

    /// <summary>
    /// List of the group roles.
    /// </summary>
    private List<GroupRole> _groupRoles = null!;

    /// <summary>
    /// Creates a group rank state.
    /// </summary>
    /// <param name="groupId">Id of the group to store.</param>
    /// <param name="robloxExtendedGroupClient">Client used for fetching group data.</param>
    public GroupRankState(long groupId, RobloxExtendedGroupClient robloxExtendedGroupClient)
    {
        this._groupId = groupId;
        this._robloxExtendedGroupClient = robloxExtendedGroupClient;
    }
    
    /// <summary>
    /// Loads the roles for the group.
    /// </summary>
    public async Task LoadRolesAsync()
    {
        this._groupRoles = (await this._robloxExtendedGroupClient.GetGroupRoles(this._groupId)).GroupRoles;
    }

    /// <summary>
    /// Returns if the 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="rank"></param>
    /// <returns></returns>
    public async Task<bool> CanActOnUserAsync(long userId, int rank)
    {
        // Return the rank comparison directly if a rank has too many members.
        foreach (var role in this._groupRoles)
        {
            if (role.Rank < rank) continue;
            if (role.MemberCount <= MaxMembersPerRank) continue;
            Logger.Debug($"Checking user id {userId} can be acted by rank {rank} directly due to {role.Name} having {role.MemberCount} members.");
            var targetUserRank = await this._robloxExtendedGroupClient.GetRankInGroupAsync(userId, this._groupId);
            return (rank <= 0 && targetUserRank <= 0) || (rank > targetUserRank);
        }
        
        // Return if the user is in a higher or equal rank.
        foreach (var role in this._groupRoles)
        {
            if (role.Rank < rank) continue;
            if (role.MemberCount == 0) continue;
            var roleMembers = await this._robloxExtendedGroupClient.GetGroupMembersInRole(this._groupId, role.Id);
            foreach (var roleMember in roleMembers)
            {
                if (roleMember.UserId != userId) continue;
                return false;
            }
        }
        return true;
    }
}