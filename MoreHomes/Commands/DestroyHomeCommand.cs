using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Helpers;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class DestroyHomeCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;        

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            string homeName = command.ElementAtOrDefault(0);
            
            if (homeName == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("DestroyHomeFormat"), pluginInstance.MessageColor);
                return;
            }

            PlayerHome home = HomesHelper.GetPlayerHome(player.CSteamID, homeName);
            if (home == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeNotFound", home.Name), pluginInstance.MessageColor);
                return;
            }

            HomesHelper.RemoveHome(player.CSteamID, home);
            home.Destroy();

            UnturnedChat.Say(caller, pluginInstance.Translate("DestroyHomeSuccess", home.Name), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "destroyhome";

        public string Help => "Destroys the bed and removes it from you home list";

        public string Syntax => "<name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
