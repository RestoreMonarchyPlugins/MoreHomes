using Rocket.API;
using SDG.Unturned;
using System.Collections.Generic;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class RestoreHomesCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            int num = 0;
            HomesHelper.ClearHomes();
            foreach (BarricadeRegion region in BarricadeManager.regions)
            {
                foreach (BarricadeDrop drop in region.drops)
                {
                    InteractableBed interactableBed = drop.interactable as InteractableBed;
                    if (interactableBed != null && interactableBed.isClaimed)
                    {
                        PlayerData player = HomesHelper.GetOrCreatePlayer(interactableBed.owner);
                        PlayerHome playerHome = new PlayerHome(player.GetUniqueHomeName(), interactableBed);
                        player.Homes.Add(playerHome);
                        num++;
                    }
                }
            }
            pluginInstance.DataService.SaveData();
            pluginInstance.SendMessageToPlayer(caller, "RestoreHomesSuccess", num.ToString("N0"));
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public string Name => "restorehomes";

        public string Help => "Clears and readds all homes on the map";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
