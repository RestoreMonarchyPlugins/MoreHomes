using Rocket.API;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.Unturned.Chat;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class RestoreHomesCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            int num = 0;
            foreach (BarricadeRegion region in BarricadeManager.regions)
            {
                foreach (BarricadeDrop drop in region.drops)
                {
                    InteractableBed bed = drop.interactable as InteractableBed;
                    if (bed != null && bed.isClaimed)
                    {
                        if (!pluginInstance.DataCache.Exists(x => x.Homes.Exists(y => y.Transform == bed.transform)))
                        {
                            pluginInstance.DataCache.ClaimBed(bed.owner.m_SteamID, bed.transform);
                            num++;
                        }
                    }
                }
            }

            UnturnedChat.Say(caller, pluginInstance.Translate("RestoreHomesSuccess", num), pluginInstance.MessageColor);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Console;

        public string Name => "restorehomes";

        public string Help => "Teleports player to their bed";

        public string Syntax => "[HomeName]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}
