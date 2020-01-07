using System;
using RestoreMonarchy.Teleportation;
using RestoreMonarchy.Teleportation.Models;
using RestoreMonarchy.Teleportation.Utils;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class HomeTeleport : ITeleport
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;

        public PlayerHome Home { get; set; }
        public UnturnedPlayer Player { get; set; }
        private bool cancel = false;
        private MovementDetector movementDetector;

        public HomeTeleport(PlayerHome home, UnturnedPlayer player)
        {
            Home = home;
            Player = player;
        }

        public void Execute(double delay)
        {
            if (delay > 0)
            {
                if (pluginInstance.TeleportationPlugin != null)
                {
                    movementDetector = pluginInstance.TeleportationPlugin.TryAddComponent<MovementDetector>();
                    if (movementDetector != null)
                        movementDetector.Initialize(this, Player);
                }
                UnturnedChat.Say(Player, pluginInstance.Translate("HomeDelayWarn", delay), pluginInstance.MessageColor);
            }

            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (!cancel)
                {
                    if (movementDetector != null)
                        UnityEngine.Object.Destroy(movementDetector);

                    if (ValidateTeleportation())
                    {
                        Player.Teleport(Home.Transform.position, Player.Rotation);
                        UnturnedChat.Say(Player, pluginInstance.Translate("HomeSuccess", Home.Name), pluginInstance.MessageColor);
                    }

                }
            }, (float)delay);
        }

        public bool ValidateTeleportation()
        {
            if (Home.Transform == null || Home.Owner == null)
            {
                UnturnedChat.Say(Player, pluginInstance.Translate("BedDestroyed"), pluginInstance.MessageColor);
                return false;
            }

            if (Player.Stance == EPlayerStance.DRIVING)
            {
                UnturnedChat.Say(Player, pluginInstance.Translate("WhileDriving"), pluginInstance.MessageColor);
                return false;
            }

            if (pluginInstance.TeleportationPlugin != null)
            {
                if (!ValitedateRaidAndCombat())
                    return false;
            }
            return true;
        }

        private bool ValitedateRaidAndCombat()
        {
            TeleportationPlugin teleportation = pluginInstance.TeleportationPlugin as TeleportationPlugin;
            if (teleportation.IsPlayerInCombat(Player.CSteamID))
            {
                UnturnedChat.Say(Player, pluginInstance.Translate("WhileCombat"), pluginInstance.MessageColor);
                return false;
            }

            if (teleportation.IsPlayerInRaid(Player.CSteamID))
            {
                UnturnedChat.Say(Player, pluginInstance.Translate("WhileRaid"), pluginInstance.MessageColor);
                return false;
            }
            return true;
        }

        public void Cancel(string message)
        {
            Logger.Log("Cancel triggered");
            cancel = true;
            UnturnedChat.Say(Player, pluginInstance.Translate("Cancel"), pluginInstance.MessageColor);
            if (movementDetector != null)
                UnityEngine.Object.Destroy(movementDetector);
        }
    }
}
