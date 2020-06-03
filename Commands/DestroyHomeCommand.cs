using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Chat;
using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Helpers;

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

            if (HomesHelper.TryRemoveHome(player.CSteamID, home.Transform))
                return;

            BarricadeManager.tryGetInfo(home.Transform, out byte b, out byte b2, out ushort num, out ushort num2, out _);
            BarricadeManager.instance.channel.send("tellTakeBarricade", ESteamCall.ALL, b, b2, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    b,
                    b2,
                    num,
                    num2
                });
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
