using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Models;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class RemoveHomeCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;        

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string homeName = command.ElementAtOrDefault(0);
            
            if (homeName == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("RemoveHomeFormat"), pluginInstance.MessageColor);
                return;
            }

            PlayerHome home = pluginInstance.DataCache.GetPlayerBed(player.CSteamID.m_SteamID, homeName);
            if (home == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeNotFound", home.Name), pluginInstance.MessageColor);
                return;
            }

            BarricadeManager.salvageBarricade(home.Transform);
            UnturnedChat.Say(caller, pluginInstance.Translate("RemoveHomeSuccess", home.Name), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "destroyhome";

        public string Help => "Destroys the bed and removes it from you home list";

        public string Syntax => "<HomeName>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
