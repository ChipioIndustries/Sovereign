using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Bouncer.State;
using Bouncer.Web.Client.Shim;
using Sovereign.Core.Model;
using Sovereign.Core.Model.Request.Api;
using Sovereign.Core.Model.Response;
using Sovereign.Core.Model.Response.Api;
using Sovereign.Discord.Configuration;

namespace Sovereign.Discord.Web.BloxlinkClient;

public class BloxlinkClient
{
    /// <summary>
    /// HTTP client to send requests.
    /// </summary>
    private readonly IHttpClient _httpClient;

    /// <summary>
    /// Function to get the configuration.
    /// Intended to be replaced for unit tests.
    /// </summary>
    public Func<DiscordConfiguration> GetConfiguration { get; set; } = Configurations.GetConfiguration<DiscordConfiguration>;

    /// <summary>
    /// Creates a Bloxlink client.
    /// </summary>
    /// <param name="httpClient">HTTP client to send requests with.</param>
    public BloxlinkClient(IHttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    /// <summary>
    /// Creates a Bloxlink client.
    /// </summary>
    public BloxlinkClient() : this(new HttpClientImpl())
    {

    }

    /// <summary>
    /// Builds an authorization header for a request.
    /// </summary>
    /// <param name="domain">Domain of the request to get the secret key of.</param>
    /// <param name="data">Data used to create the authorization header.</param>
    /// <returns>Value of the authorization header.</returns>
    public string GetAuthorizationHeader(string data)
    {
        // Get the secret key.
        var configuration = this.GetConfiguration();
        return $"{configuration.Bloxlink.Authorization}";
        // var domainData = configuration.Domains!.FirstOrDefault(entry => string.Equals(entry.Name, domain, StringComparison.CurrentCultureIgnoreCase));
        // if (domainData?.ApiSecretKey == null)
        // {
        //     throw new InvalidDataException($"No apiSecretKey was configured for {domain}");
        // }

        // // Create the authorization header.
        // using var sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(domainData.ApiSecretKey));
        // var signature = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(data)));
        // return $"Signature {signature}";
    }

    /// <summary>
    /// Sends a GET request to Sovereign.
    /// </summary>
    /// <param name="domain">Sovereign ban domain to perform the request to.</param>
    /// <param name="urlPath">Additional path after the base URL to use with Sovereign.</param>
    /// <param name="query">Query parameters for the request, including the leading question mark.</param>
    /// <param name="jsonResponseTypeInfo">JSON type information for the response contents.</param>
    /// <typeparam name="TResponse">Type of the response.</typeparam>
    /// <returns>JSON response for the request.</returns>
    public async Task<TResponse> GetAsync<TResponse>(string urlPath, string query, JsonTypeInfo<TResponse> jsonResponseTypeInfo)
    {
        // Send the request.
        var authorizationHeader = this.GetAuthorizationHeader(query);
        var baseUrl = Environment.GetEnvironmentVariable("BLOXLINK_API_BASE_URL") ?? "http://localhost:8000";
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(baseUrl + urlPath + query),
            Headers =
            {
                {"Authorization", authorizationHeader},
            },
            Method = HttpMethod.Get,
        };
        var response = await this._httpClient.SendAsync(request);

        // Parse and return the response.
        return JsonSerializer.Deserialize<TResponse>(response.Content, jsonResponseTypeInfo)!;
    }

    /// <summary>
    /// Fetches a ban record for a Roblox user id.
    /// Due to the UI only showing 1 ban at a time, only 1 ban record at most is returned.
    /// </summary>
    /// <param name="domain">Domain of the bans to fetch.</param>
    /// <param name="robloxUserId">Roblox user id to fetch the bans of.</param>
    /// <param name="banIndex">Optional index of the ban to fetch.</param>
    /// <returns>Response of the ban record entry.</returns>
    public async Task<BloxlinkRobloxIdResponse> GetRobloxUserId(long discordId)
    {
        var query = $"{discordId}";
        return await this.GetAsync("/discord-to-roblox/", query, BloxlinkRobloxIdResponseJsonContext.Default.BloxlinkRobloxIdResponse);
    }
}