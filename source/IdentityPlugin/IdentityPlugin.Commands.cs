/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public void OnDebugCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (player != null && !AdminManager.PlayerHasPermissions(player, "@css/config"))
            return;
        var output = "[Identity Plugin Debug]\n";
        output += "UserOnTick={\n";
        foreach (var (steamId, user) in UsersOnTick)
            output += $"  SteamId={steamId} Nickname={user.Nickname} Rating={user.Rating}\n";
        output += "}\n";
        command.ReplyToCommand(output);
        if (player != null)
            Server.PrintToConsole(output);
    }
}
