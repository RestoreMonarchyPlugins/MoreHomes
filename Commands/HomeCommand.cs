using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;

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

            if (pluginInstance.PlayerCooldowns.TryGetValue(caller.Id, out DateTime cooldownExpire) && cooldownExpire > DateTime.Now)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeCooldown", Math.Round((cooldownExpire - DateTime.Now).TotalSeconds)));
                return;
            }

            float delay = pluginInstance.GetDelay(player.CSteamID.m_SteamID);
            HomeTeleport teleport = new HomeTeleport(home, player);

            if (!teleport.ValidateTeleportation())
                return;

            pluginInstance.HomeTeleportations.Add(teleport);
            teleport.Execute(delay);
            pluginInstance.PlayerCooldowns[caller.Id] = DateTime.Now.AddSeconds(pluginInstance.GetCooldown(ulong.Parse(caller.Id)));
        }
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "home";

        public string Help => "Teleports player to their bed";

        public string Syntax => "[HomeName]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
