/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Ian Lucas. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using CounterStrikeSharp.API.Core;

namespace IdentityPlugin;

public partial class IdentityPlugin : BasePlugin
{
    public override string ModuleAuthor => "Ian Lucas";
    public override string ModuleDescription => "https://github.com/ianlucas/cs2-identity-plugin";
    public override string ModuleName => "IdentityPlugin";
    public override string ModuleVersion => "1.0.0";

    public override void Load(bool hotReload)
    {
        RegisterListener<Listeners.OnTick>(OnTick);

        RegisterEventHandler<EventPlayerConnect>(OnPlayerConnect);
        RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);
        RegisterEventHandler<EventPlayerDisconnect>(OnPlayerDisconnect);
    }
}
