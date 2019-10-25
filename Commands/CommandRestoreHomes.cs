using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace MoreHomes.Commands
{
    public class CommandRestoreHomes : IRocketCommand
    {
        public string Help => "Deletes all beds from database, and then adds these found on map again.";

        public string Name => "restorehomes";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Permissions => new List<string>() { "restorehomes", "morehomes.admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int count = 0;
            foreach (var region in BarricadeManager.regions)
            {
                foreach (var drop in region.drops)
                {
                    InteractableBed interactableBed = drop.interactable as InteractableBed;
                    if (interactableBed != null)
                    {
                        if (interactableBed.isClaimed)
                        {
                            Regions.tryGetCoordinate(interactableBed.transform.position, out byte x, out byte y);
                            if (MoreHomes.Instance.Database.ClaimBed(interactableBed.owner, null, x, y, interactableBed.transform.position, false))
                                count++;
                        }                        
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            UnturnedChat.Say(caller, $"Mission accomplished, {count} beds restored!");
        }
    }
}
