using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.Unturned.Chat;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using RestoreMonarchy.Teleportation;
using RestoreMonarchy.Teleportation.Utils;
using Steamworks;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class HomeCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            PlayerHome home;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            home = pluginInstance.DataCache.GetPlayerBed(player.CSteamID.m_SteamID, command.ElementAtOrDefault(0));

            if (home == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("NoBedsToTeleport"), pluginInstance.MessageColor);
                return;
            }

            float delay = pluginInstance.GetDelay(player.CSteamID.m_SteamID);
            HomeTeleport teleport = new HomeTeleport(home, player);

            if (!teleport.ValidateTeleportation())
                return;

            pluginInstance.HomeTeleportations.Add(teleport);
            teleport.Execute(delay);
        }
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "home";

        public string Help => "Teleports player to their bed";

        public string Syntax => "[HomeName]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
