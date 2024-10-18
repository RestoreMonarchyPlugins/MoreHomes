using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;
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

            if (string.IsNullOrEmpty(homeName))
            {
                pluginInstance.SendMessageToPlayer(caller, "DestroyHomeFormat");
                return;
            }

            PlayerHome home = HomesHelper.GetPlayerHome(player.CSteamID, homeName);
            if (home == null)
            {
                pluginInstance.SendMessageToPlayer(caller, "HomeNotFound", homeName);
                return;
            }

            HomesHelper.RemoveHome(player.CSteamID, home);
            home.Destroy();

            pluginInstance.SendMessageToPlayer(caller, "DestroyHomeSuccess", home.Name);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "destroyhome";

        public string Help => "Destroys the bed and removes it from you home list";

        public string Syntax => "<name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}