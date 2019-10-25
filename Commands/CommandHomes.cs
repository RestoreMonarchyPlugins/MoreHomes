using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MoreHomes.Commands
{
    public class CommandHomes : IRocketCommand
    {
        public string Help => "Displays a list of caller's beds.";

        public string Name => "homes";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public List<string> Permissions => new List<string>() { "homes", "morehomes.admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            
            UnturnedChat.Say(caller, MoreHomes.Instance.Database.GetAllBedsMessage(player.CSteamID));
        }
    }
}
