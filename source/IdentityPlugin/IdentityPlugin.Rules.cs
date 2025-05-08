/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.ValveConstants.Protobuf;
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
        Logger.LogInformation("Player {Name} is being authenticated.", controller.PlayerName);
        var steamId = controller.SteamID;
        UsersOnFetch.Add(steamId);
        var user = await FetchUser(steamId);
        UsersOnFetch.Remove(steamId);
        Server.NextFrame(() =>
        {
            if (!controller.IsValid || controller.Connected > PlayerConnectedState.PlayerConnecting)
                return;
            if (user == null)
            {
                if (strict.Value)
                    controller.Disconnect(
                        NetworkDisconnectionReason.NETWORK_DISCONNECT_REJECT_RESERVED_FOR_LOBBY
                    );
                return;
            }
            user.Controller = controller;
            if (force_nickname.Value || force_rating.Value)
            {
                UsersOnTick.TryAdd(controller.SteamID, user);
                Logger.LogInformation(
                    "Player {Name} has rating {Rating}.",
                    controller.PlayerName,
                    user.Rating
                );
            }

            if (user.Flags.Length > 0)
            {
                AdminManager.AddPlayerPermissions(controller, user.Flags);
                Logger.LogInformation(
                    "Player {Name} has flags {Flags}.",
                    controller.PlayerName,
                    user.Flags
                );
            }
        });
    }

    public void HandleDisconnectingPlayer(CCSPlayerController controller)
    {
        if (controller.IsBot)
            return;
        foreach (var (steamId, user) in UsersOnTick)
            if (user.Controller?.SteamID == controller.SteamID || user.Controller?.IsValid != true)
                UsersOnTick.TryRemove(steamId, out var _);
    }
}
