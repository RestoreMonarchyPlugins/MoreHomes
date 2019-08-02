using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace MoreHomes.Commands
{
    public class CommandHomes : IRocketCommand
    {
        public string Help
        {
            get { return "Shows the list of your beds."; }
        }

        public string Name
        {
            get { return "homes"; }
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "homes" };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            
            UnturnedChat.Say(caller, MoreHomes.Instance.Database.GetAllBedsMessage(player.CSteamID));
        }
    }
}
