using System;

namespace Sovereign.Discord.Extension;

public static class DateTimeExtensions
{
    /// <summary>
    /// Formats a DateTime to a localized Discord time.
    /// </summary>
    /// <param name="this">DateTime to format.</param>
    /// <returns>Formatted date for Discord.</returns>
    public static string ToDiscordFormattedTime(this DateTime @this)
    {
        return $"<t:{new DateTimeOffset(@this.ToUniversalTime()).ToUnixTimeSeconds()}:F>";
    }
}