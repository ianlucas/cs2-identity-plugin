/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public readonly List<ulong> UsersOnFetch = [];

    public bool IsEnabled()
    {
        return url.Value.Contains("{userId}");
    }

    public async void HandleConnectingPlayer(CCSPlayerController controller)
    {
        if (controller.IsBot || !IsEnabled() || UsersOnFetch.Contains(controller.SteamID))
            return;
        Logger.LogInformation("[Identity] Authenticating player {Name}", controller.PlayerName);
        UsersOnFetch.Add(controller.SteamID);
        var user = await FetchUser(controller.SteamID);
        UsersOnFetch.Remove(controller.SteamID);
        Server.NextFrame(() =>
        {
            if (!controller.IsValid || controller.Connected > PlayerConnectedState.PlayerConnecting)
            {
                Logger.LogWarning(
                    "[Identity] Player {Name} is invalid after user fetch.",
                    controller.PlayerName
                );
                return;
            }
            if (user == null)
            {
                if (strict.Value)
                    controller.Kick();
                return;
            }
            user.Controller = controller;
            if (force_nickname.Value || force_rating.Value)
            {
                UsersOnTick.TryAdd(controller.SteamID, user);
                Logger.LogInformation(
                    "[Identity] Player {name} is authenticated.",
                    controller.PlayerName
                );
            }
        });
    }

    public void HandleDisconnectingPlayer(CCSPlayerController controller)
    {
        if (controller.IsBot)
            return;
        UsersOnTick.Remove(controller.SteamID, out var _);
    }
}
