using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Text;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class HomesCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            
            PlayerData playerData = HomesHelper.GetOrCreatePlayer(player.CSteamID);

            if (playerData.Homes.Count == 0)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("NoHomes"), pluginInstance.MessageColor);
                return;
            }

            StringBuilder sb = new StringBuilder(pluginInstance.Translate("HomeList"));
            
            foreach (PlayerHome home in playerData.Homes)
            {
                sb.Append($"{home.Name}, ");
            }
            string msg = sb.ToString().TrimEnd(',', ' ');

            UnturnedChat.Say(caller, msg, pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "homes";

        public string Help => "Displays a list of your claimed beds";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();        
    }
}
