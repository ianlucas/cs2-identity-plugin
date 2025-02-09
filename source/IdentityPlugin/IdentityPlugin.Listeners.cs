/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Concurrent;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.UserMessages;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public readonly ConcurrentDictionary<ulong, User> UsersOnTick = [];

    public void OnTick()
    {
        var gameRules = IdentityPluginExtensions.GetGameRules();
        var sendServerRankRevealAll = false;
        foreach (var user in UsersOnTick.Values)
            if (user.Controller?.IsValid == true)
            {
                if (force_nickname.Value)
                    user.Controller.SetName(user.Nickname);
                if (force_rating.Value)
                    if (gameRules.TeamIntroPeriod)
                        user.Controller.HideRating();
                    else
                    {
                        user.Controller.SetRating(user.Rating);
                        if (
                            !user.ReceivedServerRankRevealAll
                            && user.Controller.Connected == PlayerConnectedState.PlayerConnected
                        )
                        {
                            user.ReceivedServerRankRevealAll = true;
                            sendServerRankRevealAll = true;
                        }
                    }
                else
                    user.Controller.HideRating();
            }
        if (sendServerRankRevealAll)
        {
            var filter = new RecipientFilter();
            foreach (var user in UsersOnTick.Values)
                if (user.Controller?.IsValid == true)
                    filter.Add(user.Controller);
            if (filter.Count > 0)
            {
                UserMessage.FromId(350).Send(filter);
                Logger.LogInformation("Sent ServerRankRevealAll user message to all players.");
            }
        }
    }
}
