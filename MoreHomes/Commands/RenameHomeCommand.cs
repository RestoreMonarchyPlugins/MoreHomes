using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using RestoreMonarchy.MoreHomes.Helpers;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class RenameHomeCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length < 2)
            {
                pluginInstance.SendMessageToPlayer(caller, "RenameHomeFormat");
                return;
            }

            var home = HomesHelper.GetPlayerHome(player.CSteamID, command[0]);
            if (home == null)
            {
                pluginInstance.SendMessageToPlayer(player, "HomeNotFound", command[0]);
                return;
            }

            if (HomesHelper.GetPlayerHome(player.CSteamID, command[1]) != null)
            {
                pluginInstance.SendMessageToPlayer(player, "RenameHomeFail", command[1]);
                return;
            }

            home.Name = command[1];
            pluginInstance.SendMessageToPlayer(player, "RenameHomeSuccess", command[0], command[1]);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "renamehome";

        public string Help => "Changes home's name";

        public string Syntax => "<name> <rename>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}