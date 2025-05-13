using System;
using NUnit.Framework;
using Sovereign.Discord.Extension;

namespace Sovereign.Discord.Test.Discord.Extension;

public class DateTimeExtensionsTest
{
    [Test]
    public void TestToDiscordFormattedTime()
    {
        var dateTime = new DateTime(2025, 1, 2, 3, 4, 5);
        Assert.That(dateTime.ToDiscordFormattedTime(), Is.EqualTo("<t:1735805045:F>"));
    }
}