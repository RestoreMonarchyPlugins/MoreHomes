using Rocket.API;
using SDG.Unturned;
using System.Collections.Generic;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Helpers;

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
                    InteractableBed bed = drop.interactable as InteractableBed;
                    if (bed != null && bed.isClaimed)
                    {
                        HomesHelper.TryClaimHome(bed.owner, bed.transform);
                        num++;
                    }
                }
            }
            pluginInstance.DataService.SaveData();
            UnturnedChat.Say(caller, pluginInstance.Translate("RestoreHomesSuccess", num), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public string Name => "restorehomes";

        public string Help => "Clears and readds all homes on the map";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
