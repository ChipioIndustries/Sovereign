using System;
using NUnit.Framework;
using Sovereign.Discord.Extension;

namespace Sovereign.Discord.Test.Discord.Extension;

public class DateTimeExtensionsTest
{
    [Test]
    public void TestToDiscordFormattedTime()
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(123456789);
        Assert.That(dateTime.ToDiscordFormattedTime(), Is.EqualTo("<t:123456789:F>"));
    }
}