using System.Net;
using Bouncer.Test.Web.Client.Shim;
using NUnit.Framework;
using Sovereign.Api.Bans.Web.Client;
using Sovereign.Core.Web.Client;

namespace Sovereign.Api.Bans.Test.Web.Client;

public class GroupRankStateTest
{
    private TestHttpClient _testHttpClient;
    private RobloxExtendedGroupClient _client;
    private GroupRankState _groupRankState;

    [SetUp]
    public void SetUp()
    {
        this._testHttpClient = new TestHttpClient();
        this._client = new RobloxExtendedGroupClient(this._testHttpClient, this._testHttpClient);
        this._groupRankState = new GroupRankState(12345, this._client);
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles", HttpStatusCode.OK, "{\"groupRoles\":[{\"id\":1234,\"rank\":0,\"memberCount\":0},{\"id\":1235,\"rank\":50,\"memberCount\":1000},{\"id\":1236,\"rank\":100,\"memberCount\":1},{\"id\":1237,\"rank\":150,\"memberCount\":0},{\"id\":1238,\"rank\":255,\"memberCount\":1}]}");
        this._groupRankState.LoadRolesAsync().Wait();
    }

    [Test]
    public void TestCanActOnUserAsyncTooManyMembersGuest()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v2/users/123456/groups/roles", HttpStatusCode.OK, "{\"data\":[]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 0).Result, Is.True);
    }

    [Test]
    public void TestCanActOnUserAsyncTooManyMembersNonGuest()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v2/users/123456/groups/roles", HttpStatusCode.OK, "{\"data\":[{\"group\":{\"id\":12345},\"role\":{\"rank\":49}}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 0).Result, Is.False);
    }

    [Test]
    public void TestCanActOnUserAsyncTooManyMembersLowerRank()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v2/users/123456/groups/roles", HttpStatusCode.OK, "{\"data\":[{\"group\":{\"id\":12345},\"role\":{\"rank\":49}}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 50).Result, Is.True);
    }

    [Test]
    public void TestCanActOnUserAsyncTooManyMembersSameRank()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v2/users/123456/groups/roles", HttpStatusCode.OK, "{\"data\":[{\"group\":{\"id\":12345},\"role\":{\"rank\":50}}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 50).Result, Is.False);
    }

    [Test]
    public void TestCanActOnUserAsyncMemberInLowerRank()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/1236/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"userId\":123}]}");
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/1238/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"userId\":123}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 100).Result, Is.True);
    }

    [Test]
    public void TestCanActOnUserAsyncMemberInSameRank()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/1236/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"userId\":123456}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 100).Result, Is.False);
    }

    [Test]
    public void TestCanActOnUserAsyncMemberInHigherRank()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/1236/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"userId\":123}]}");
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/1238/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"userId\":123456}]}");
        Assert.That(this._groupRankState.CanActOnUserAsync(123456, 100).Result, Is.False);
    }
}