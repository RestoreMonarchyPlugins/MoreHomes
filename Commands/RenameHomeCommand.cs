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
            HomesHelper.TryRenameHome(player.CSteamID, command[0], command[1]);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "renamehome";

        public string Help => "Changes home's name to a new one";

        public string Syntax => "<OldName> <NewName>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
