using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Rocket.Unturned.Skills;

namespace MoreHomes.Commands
{
    public class CommandHome : IRocketCommand
    {

        public string Help
        {
            get { return "Teleports you to your bed."; }
        }

        public string Name
        {
            get { return "home"; }
        }

        public string Syntax
        {
            get { return "<bedName>"; }
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
                return new List<string>() { "home" };
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;

            Vector3 position;
            byte angle = 0;

            if (unturnedPlayer.Stance == EPlayerStance.DRIVING || unturnedPlayer.Stance == EPlayerStance.SITTING)
            {
                UnturnedChat.Say(caller, U.Translate("command_generic_teleport_while_driving_error", new object[0]));
                throw new WrongUsageOfCommandException(caller, this);
            } else if (!BarricadeManager.tryGetBed(unturnedPlayer.CSteamID, out position, out angle))
            {
                UnturnedChat.Say(caller, String.Format(MoreHomes.Instance.Translate("command_home_not_found"), command[0]), Color.red);
                return;
            } else if (command.Count() == 1)
            {
                string sPosition = Database.GetBed(unturnedPlayer.CSteamID, command[0]);
                if (sPosition == "null")
                {
                    UnturnedChat.Say(caller, String.Format(MoreHomes.Instance.Translate("command_home_not_found"), command[0]), Color.red);
                    return;
                }
                position = Tools.StringToVector3(sPosition);
            }

            new Thread((ThreadStart)(() =>
            {
                UnturnedChat.Say(caller, String.Format(MoreHomes.Instance.Translate("command_home_delay"), MoreHomes.Instance.Configuration.Instance.TeleportationDelay), Color.grey);
                Thread.Sleep(MoreHomes.Instance.Configuration.Instance.TeleportationDelay * 1000);
                unturnedPlayer.Teleport(position, (float)angle);

            })){
                IsBackground = true
            }.Start();  


        }
    }
}
