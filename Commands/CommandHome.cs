using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using MoreHomes.Models;
using MoreHomes.Helpers;

namespace MoreHomes.Commands
{
    public class CommandHome : IRocketCommand
    {
        public string Help => "Teleports player to their home";

        public string Name => "home";

        public string Syntax => "[name]";

        public List<string> Aliases => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public List<string> Permissions => new List<string>() { "home", "morehomes.admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;

            if (unturnedPlayer.Stance == EPlayerStance.DRIVING || unturnedPlayer.Stance == EPlayerStance.SITTING)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_teleport_while_driving_error", new object[0]));
                return;
            }

            string bedName = command.ElementAtOrDefault(0);

            PlayerBed bed = null;

            if (bedName != null)
            {
                bed = MoreHomes.Instance.Database.GetBedByName(unturnedPlayer.CSteamID, bedName);
            } else if (BarricadeManager.tryGetBed(unturnedPlayer.CSteamID, out Vector3 point, out _))
            {
                bed = MoreHomes.Instance.Database.GetBedByPosition(unturnedPlayer.CSteamID, point);
            }

            if (bed.ShouldAllowTeleport())
            {
                Console.WriteLine(bed.Position);
                if (MoreHomes.Instance.Configuration.Instance.TeleportationDelay > 0)
                {                    
                    Timer timer = new Timer(MoreHomes.Instance.Configuration.Instance.TeleportationDelay * 1000);
                    timer.AutoReset = false;
                    timer.Elapsed += (sender, e) =>
                    {
                        if (bed.IsDestroyed())
                        {
                            UnturnedChat.Say(caller, MoreHomes.Instance.Translate("command_home_destroyed"));
                        } else
                        {
                            unturnedPlayer.Teleport(bed.Vector3, unturnedPlayer.Rotation);
                            UnturnedChat.Say(caller, MoreHomes.Instance.Translate("command_home_success", bed.BedName));
                        }
                    };

                    UnturnedChat.Say(caller, MoreHomes.Instance.Translate("command_home_delay", MoreHomes.Instance.Configuration.Instance.TeleportationDelay));
                    timer.Start();
                }
                else
                {
                    unturnedPlayer.Teleport(bed.Vector3, unturnedPlayer.Rotation);
                    UnturnedChat.Say(caller, MoreHomes.Instance.Translate("command_home_success", bed.BedName));
                }
            }
            else
            {
                UnturnedChat.Say(caller, MoreHomes.Instance.Translate("no_home"));
            }
        }
    }
}
