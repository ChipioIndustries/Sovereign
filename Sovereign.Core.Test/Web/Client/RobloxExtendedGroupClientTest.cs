using System.Net;
using Bouncer.Test.Web.Client.Shim;
using NUnit.Framework;
using Sovereign.Core.Web.Client;

namespace Sovereign.Core.Test.Web.Client;

public class RobloxExtendedGroupClientTest
{
    private TestHttpClient _testHttpClient;
    private RobloxExtendedGroupClient _client;

    [SetUp]
    public void SetUp()
    {
        this._testHttpClient = new TestHttpClient();
        this._client = new RobloxExtendedGroupClient(this._testHttpClient, this._testHttpClient);
    }

    [Test]
    public void TestGetGroupRoles()
    {
        
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles", HttpStatusCode.OK, "{\"groupId\":12345,\"groupRoles\":[{\"id\":1234,\"name\":\"TestName\",\"description\":\"TestDescription\",\"rank\":123,\"memberCount\":12}]}");
        var response = this._client.GetGroupRoles(12345).Result;
        Assert.That(response.GroupId, Is.EqualTo(12345));
        Assert.That(response.GroupRoles.Count, Is.EqualTo(1));
        Assert.That(response.GroupRoles[0].Id, Is.EqualTo(1234));
        Assert.That(response.GroupRoles[0].Name, Is.EqualTo("TestName"));
        Assert.That(response.GroupRoles[0].Description, Is.EqualTo("TestDescription"));
        Assert.That(response.GroupRoles[0].Rank, Is.EqualTo(123));
        Assert.That(response.GroupRoles[0].MemberCount, Is.EqualTo(12));
    }

    [Test]
    public void TestGetGroupMembersInRoleForPage()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/123/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"nextPageCursor\":\"TestCursor\",\"data\":[{\"hasVerifiedBadge\":true,\"userId\":123456,\"username\":\"TestUsername\",\"displayName\":\"TestDisplayName\"}]}");
        var response = this._client.GetGroupMembersInRoleForPage(12345, 123).Result;
        Assert.That(response.NextPageCursor, Is.EqualTo("TestCursor"));
        Assert.That(response.Data.Count, Is.EqualTo(1));
        Assert.That(response.Data[0].HasVerifiedBadge, Is.EqualTo(true));
        Assert.That(response.Data[0].UserId, Is.EqualTo(123456));
        Assert.That(response.Data[0].Username, Is.EqualTo("TestUsername"));
        Assert.That(response.Data[0].DisplayName, Is.EqualTo("TestDisplayName"));
    }

    [Test]
    public void TestGetGroupMembersInRole()
    {
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/123/users?cursor=&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"nextPageCursor\":\"TestCursor\",\"data\":[{\"hasVerifiedBadge\":true,\"userId\":123456,\"username\":\"TestUsername1\",\"displayName\":\"TestDisplayName1\"}]}");
        this._testHttpClient.SetResponse("https://groups.roblox.com/v1/groups/12345/roles/123/users?cursor=TestCursor&limit=100&sortOrder=Desc", HttpStatusCode.OK, "{\"data\":[{\"hasVerifiedBadge\":false,\"userId\":123457,\"username\":\"TestUsername2\",\"displayName\":\"TestDisplayName2\"}]}");
        var response = this._client.GetGroupMembersInRole(12345, 123).Result;
        Assert.That(response.Count, Is.EqualTo(2));
        Assert.That(response[0].HasVerifiedBadge, Is.EqualTo(true));
        Assert.That(response[0].UserId, Is.EqualTo(123456));
        Assert.That(response[0].Username, Is.EqualTo("TestUsername1"));
        Assert.That(response[0].DisplayName, Is.EqualTo("TestDisplayName1"));
        Assert.That(response[1].HasVerifiedBadge, Is.EqualTo(false));
        Assert.That(response[1].UserId, Is.EqualTo(123457));
        Assert.That(response[1].Username, Is.EqualTo("TestUsername2"));
        Assert.That(response[1].DisplayName, Is.EqualTo("TestDisplayName2"));
    }
}