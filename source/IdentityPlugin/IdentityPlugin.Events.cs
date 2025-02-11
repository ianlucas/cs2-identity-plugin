/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API.Core;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public HookResult OnPlayerConnect(EventPlayerConnect @event, GameEventInfo _)
    {
        var controller = @event.Userid;
        if (controller != null)
            HandleConnectingPlayer(controller);
        return HookResult.Continue;
    }

    public HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo _)
    {
        var controller = @event.Userid;
        if (controller != null)
            HandleConnectingPlayer(controller);
        return HookResult.Continue;
    }

    public HookResult OnPlayerDisconnect(EventPlayerDisconnect @event, GameEventInfo _)
    {
        var controller = @event.Userid;
        if (controller != null)
            HandleDisconnectingPlayer(controller);
        return HookResult.Continue;
    }
}
