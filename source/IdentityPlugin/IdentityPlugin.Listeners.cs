/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System.Collections.Concurrent;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public readonly ConcurrentDictionary<ulong, User> UsersOnTick = [];

    public void OnTick()
    {
        var gameRules = IdentityPluginExtensions.GetGameRules();
        foreach (var user in UsersOnTick.Values)
            if (user.Controller?.IsValid == true)
            {
                if (force_nickname.Value)
                    user.Controller.SetName(user.Nickname);
                if (force_rating.Value)
                    if (gameRules.TeamIntroPeriod)
                        user.Controller.HideRating();
                    else
                        user.Controller.SetRating(user.Rating);
                else
                    user.Controller.HideRating();
            }
    }
}
