using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using RestoreMonarchy.Teleportation;
using RestoreMonarchy.Teleportation.Utils;
using System;
using RestoreMonarchy.MoreHomes.Helpers;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Commands
{
    public class HomeCommand : IRocketCommand
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        public void Execute(IRocketPlayer caller, string[] command)
        {
            PlayerHome home;
            UnturnedPlayer player = (UnturnedPlayer)caller;
            home = HomesHelper.GetPlayerHome(player.CSteamID, command.ElementAtOrDefault(0));

            if (home == null)
            {
                pluginInstance.SendMessageToPlayer(caller, "NoHome");
                return;
            }

            if (!ValidateTeleportation(player, home))
            {
                return;
            }                

            if (pluginInstance.PlayerCooldowns.TryGetValue(caller.Id, out DateTime cooldownExpire) && cooldownExpire > DateTime.Now)
            {
                pluginInstance.SendMessageToPlayer(caller, "HomeCooldown", Math.Round((cooldownExpire - DateTime.Now).TotalSeconds));
                return;
            }

            pluginInstance.PlayerCooldowns[caller.Id] = DateTime.Now.AddSeconds(VipHelper.GetPlayerHomeCooldown(caller.Id));

            float delay = VipHelper.GetPlayerHomeDelay(player.Id);

            if (delay > 0)
            {
                pluginInstance.SendMessageToPlayer(caller, "HomeDelayWarn", delay);
            }

            bool shouldCancel = false;

            if (pluginInstance.Configuration.Instance.CancelOnMove)
            {
                pluginInstance.MovementDetector.AddPlayer(player.Player, () =>
                {
                    shouldCancel = true;
                    pluginInstance.SendMessageToPlayer(player, "HomeCanceledYouMoved");
                });
            }

            TaskDispatcher.QueueOnMainThread(() =>
            {
                if (delay > 0 && pluginInstance.Configuration.Instance.CancelOnMove)
                {
                    if (shouldCancel)
                    {
                        return;
                    }
                }

                pluginInstance.MovementDetector.RemovePlayer(player.Player);

                if (!ValidateTeleportation(player, home))
                {
                    pluginInstance.PlayerCooldowns.Remove(caller.Id);
                    return;
                }

                if (!player.Player.teleportToLocation(home.LivePosition + new Vector3(0f, pluginInstance.Configuration.Instance.TeleportHeight, 0f), player.Rotation))
                {
                    pluginInstance.SendMessageToPlayer(caller, "HomeTeleportationFailed", home.Name);
                    pluginInstance.PlayerCooldowns.Remove(caller.Id);
                    return;
                }
                pluginInstance.SendMessageToPlayer(caller, "HomeSuccess", home.Name);
            }, delay);
        }

        private bool ValidateTeleportation(UnturnedPlayer player, PlayerHome home)
        {
            if (home.InteractableBed == null || !home.InteractableBed.isActiveAndEnabled || home.InteractableBed.owner != player.CSteamID)
            {
                HomesHelper.RemoveHome(player.CSteamID, home);
                pluginInstance.SendMessageToPlayer(player, "BedDestroyed");
                return false;
            }

            if (player.Stance == EPlayerStance.DRIVING)
            {
                pluginInstance.SendMessageToPlayer(player, "WhileDriving");
                return false;
            }

            if (pluginInstance.TeleportationPlugin != null)
            {
                if (!ValitedateRaidAndCombat(player))
                    return false;
            }
                        
            if (pluginInstance.Configuration.Instance.BlockUnderground)
            {
                Vector3 position = home.LivePosition;
                float height = LevelGround.getHeight(position);
                if (height > position.y)
                {
                    pluginInstance.SendMessageToPlayer(player, "CantTeleportToBedUnderground", home.Name);
                    return false;
                }
            }
            return true;
        }

        private bool ValitedateRaidAndCombat(UnturnedPlayer player)
        {
            TeleportationPlugin teleportation = pluginInstance.TeleportationPlugin as TeleportationPlugin;
            if (teleportation.IsPlayerInCombat(player.CSteamID))
            {
                pluginInstance.SendMessageToPlayer(player, "WhileCombat");
                return false;
            }

            if (teleportation.IsPlayerInRaid(player.CSteamID))
            {
                pluginInstance.SendMessageToPlayer(player, "WhileRaid");
                return false;
            }
            return true;
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "home";

        public string Help => "Teleports player to their bed";

        public string Syntax => "[name]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();
    }
}