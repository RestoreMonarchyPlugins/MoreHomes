using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Helpers;
using Steamworks;

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

            if (!HomesHelper.TryRemoveHome(player.CSteamID, home.InteractableBed) || home.InteractableBed == null)
                return;



            BarricadeManager.tryGetInfo(home.InteractableBed.transform, out var x, out var y, out var plant, out var index, out var region);
            if (home.InteractableBed != null)
                BarricadeManager.destroyBarricade(region, x, y, plant, index);

            UnturnedChat.Say(caller, pluginInstance.Translate("DestroyHomeSuccess", home.Name), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "destroyhome";

        public string Help => "Destroys the bed and removes it from you home list";

        public string Syntax => "<HomeName>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
