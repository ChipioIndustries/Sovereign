using Bouncer.State;
using Sovereign.Api.Bans.Configuration;
using Sovereign.Core.Database;
using Sovereign.Core.Web.Client;

namespace Sovereign.Api.Bans.Web.Server.Controller.Shim;

public class BanControllerResources : IBanControllerResources
{
    /// <summary>
    /// Returns the current bans configuration.
    /// </summary>
    /// <returns>The current bans configuration.</returns>
    public BansConfiguration GetConfiguration()
    {
        return Configurations.GetConfiguration<BansConfiguration>();
    }
    
    /// <summary>
    /// Returns the client used to for group rank checks.
    /// </summary>
    /// <returns>The client used to for group rank checks.</returns>
    public RobloxExtendedGroupClient GetRobloxGroupClient()
    {
        return new RobloxExtendedGroupClient();
    }

    /// <summary>
    /// Returns the database context to use.
    /// </summary>
    /// <returns>The database context to use.</returns>
    public BansContext GetBansContext()
    {
        return new BansContext();
    }
}