/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace IdentityPlugin;

public static class IdentityPluginExtensions
{
    static CCSGameRulesProxy? GameRulesProxy;

    public static CCSGameRules GetGameRules() =>
        (
            GameRulesProxy?.IsValid == true ? GameRulesProxy.GameRules
            : (
                GameRulesProxy = Utilities
                    .FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules")
                    .First()
            )?.IsValid == true
                ? GameRulesProxy?.GameRules
            : null
        ) ?? throw new Exception("Game rules not found.");

    public static void Kick(this CCSPlayerController controller)
    {
        if (controller.IsValid && controller.UserId.HasValue)
            Server.ExecuteCommand($"kickid {(ushort)controller.UserId}");
    }

    public static bool SetName(this CCSPlayerController controller, string name)
    {
        if (controller.PlayerName != name)
        {
            controller.PlayerName = name;
            Utilities.SetStateChanged(controller, "CBasePlayerController", "m_iszPlayerName");
            return true;
        }
        return false;
    }

    public static void SetRating(this CCSPlayerController controller, int rating)
    {
        controller.CompetitiveWins = 256;
        controller.CompetitiveRankType = 11;
        controller.CompetitiveRanking = rating;
        controller.CompetitiveRankingPredicted_Loss = 0;
        controller.CompetitiveRankingPredicted_Tie = 0;
        controller.CompetitiveRankingPredicted_Win = 0;
    }

    public static void HideRating(this CCSPlayerController controller)
    {
        controller.CompetitiveRankType = 0;
        Utilities.SetStateChanged(controller, "CCSPlayerController", "m_iCompetitiveRankType");
    }
}
