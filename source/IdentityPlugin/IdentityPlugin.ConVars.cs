/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API.Modules.Cvars;

namespace IdentityPlugin;

public partial class IdentityPlugin
{
    public readonly FakeConVar<string> url = new(
        "identity_url",
        "URL for fetching player identity.",
        ""
    );

    public readonly FakeConVar<bool> strict = new(
        "identity_strict",
        "Whether to kick the player if we fail to get their data.",
        true
    );

    public readonly FakeConVar<bool> force_nickname = new(
        "identity_force_nickname",
        "Whether to force player nickname.",
        true
    );

    public readonly FakeConVar<bool> force_rating = new(
        "identity_force_rating",
        "Whether to force player rating.",
        true
    );
}
