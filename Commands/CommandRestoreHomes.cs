using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace MoreHomes.Commands
{
    public class CommandRestoreHomes : IRocketCommand
    {
        public string Help
        {
            get { return "Restores the beds from the map (adds them to database)."; }
        }

        public string Name
        {
            get { return "restorehomes"; }
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
            get { return AllowedCaller.Both; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "restorehomes" };
            }
        }

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
                            if (MoreHomes.Instance.Database.RestoreBed(interactableBed.owner, MoreHomes.Instance.Database.GetNameForBed(interactableBed.owner), x, y, interactableBed.transform.position))
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
