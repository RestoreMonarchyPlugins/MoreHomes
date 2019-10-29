using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using RestoreMonarchy.MoreHomes.Utilities;
using RestoreMonarchy.MoreHomes.Models;

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

            PlayerHome home = pluginInstance.DataCache.GetPlayerBed(player.CSteamID.m_SteamID, command[0]);

            if (home == null)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeNotFound", command[0]), pluginInstance.MessageColor);
                return;
            }

            if (!pluginInstance.DataCache.RenameBed(home, command[1]))
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeAlreadyExists", command[1]));
                return;
            }
            UnturnedChat.Say(caller, pluginInstance.Translate("RenameHomeSuccess", command[0], command[1]), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "renamehome";

        public string Help => "Changes home's name to a new one";

        public string Syntax => "<HomeName> <NewName>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
