using Rocket.API;
using Rocket.Unturned.Chat;
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
                UnturnedChat.Say(caller, pluginInstance.Translate("RenameHomeFormat"));
                return;
            }

            var home = HomesHelper.GetPlayerHome(player.CSteamID, command[0]);
            if (home == null)
            {
                UnturnedChat.Say(player, pluginInstance.Translate("HomeNotFound", command[0]), pluginInstance.MessageColor);
                return;
            }

            if (HomesHelper.GetPlayerHome(player.CSteamID, command[1]) != null)
            {
                UnturnedChat.Say(player, pluginInstance.Translate("RenameHomeFail", command[1]), pluginInstance.MessageColor);
                return;
            }

            home.Name = command[1];
            UnturnedChat.Say(player, pluginInstance.Translate("RenameHomeSuccess", command[0], command[1]), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "renamehome";

        public string Help => "Changes home's name";

        public string Syntax => "<name> <rename>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
