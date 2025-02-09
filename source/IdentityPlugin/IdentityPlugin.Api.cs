/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Text.Json;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace IdentityPlugin;

public class User
{
    [JsonPropertyName("nickname")]
    public required string Nickname { get; set; }

    [JsonPropertyName("rating")]
    public required int Rating { get; set; }

    [JsonPropertyName("flags")]
    public required string[] Flags { get; set; }

    public CCSPlayerController? Controller;
    public bool ReceivedServerRankRevealAll = false;
}

public partial class IdentityPlugin
{
    public async Task<User?> FetchUser(ulong steamId)
    {
        try
        {
            using HttpClient client = new();
            var response = await client.GetAsync(url.Value.Replace("{userId}", steamId.ToString()));
            response.EnsureSuccessStatusCode();

            string jsonContent = response.Content.ReadAsStringAsync().Result;
            User? data = JsonSerializer.Deserialize<User>(jsonContent);
            return data;
        }
        catch (Exception error)
        {
            Logger.LogError("Unable to fetch user: {Message}", error.Message);
            return default;
        }
    }
}
