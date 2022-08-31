using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Chat;
using Rocket.Core.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using RestoreMonarchy.Teleportation;
using RestoreMonarchy.Teleportation.Utils;
using System;
using RestoreMonarchy.MoreHomes.Helpers;
using UnityEngine;
using System.Threading;

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
                UnturnedChat.Say(caller, pluginInstance.Translate("NoHome"), pluginInstance.MessageColor);
                return;
            }

            if (!ValidateTeleportation(player, home))
                return;

            if (pluginInstance.PlayerCooldowns.TryGetValue(caller.Id, out DateTime cooldownExpire) && cooldownExpire > DateTime.Now)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeCooldown", System.Math.Round((cooldownExpire - DateTime.Now).TotalSeconds)), 
                    pluginInstance.MessageColor);
                return;
            }

            pluginInstance.PlayerCooldowns[caller.Id] = DateTime.Now.AddSeconds(VipHelper.GetPlayerHomeCooldown(caller.Id));

            float delay = VipHelper.GetPlayerHomeDelay(player.Id);

            if (delay > 0)
            {
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeDelayWarn", delay), pluginInstance.MessageColor);
            }

            bool shouldCancel = false;

            if (pluginInstance.Configuration.Instance.CancelOnMove)
            {
                pluginInstance.MovementDetector.AddPlayer(player.Player, () =>
                {
                    shouldCancel = true;
                    UnturnedChat.Say(player, pluginInstance.Translate("HomeCanceledYouMoved"), pluginInstance.MessageColor);
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
                    UnturnedChat.Say(caller, pluginInstance.Translate("HomeTeleportationFailed", home.Name), pluginInstance.MessageColor);
                    pluginInstance.PlayerCooldowns.Remove(caller.Id);
                    return;
                }
                UnturnedChat.Say(caller, pluginInstance.Translate("HomeSuccess", home.Name), pluginInstance.MessageColor);                
            }, delay);
        }

        private bool ValidateTeleportation(UnturnedPlayer player, PlayerHome home)
        {
            if (home.InteractableBed == null || !home.InteractableBed.isActiveAndEnabled || home.InteractableBed.owner != player.CSteamID)
            {
                HomesHelper.RemoveHome(player.CSteamID, home);
                UnturnedChat.Say(player, pluginInstance.Translate("BedDestroyed"), pluginInstance.MessageColor);                
                return false;
            }

            if (player.Stance == EPlayerStance.DRIVING)
            {
                UnturnedChat.Say(player, pluginInstance.Translate("WhileDriving"), pluginInstance.MessageColor);
                return false;
            }

            if (pluginInstance.TeleportationPlugin != null)
            {
                if (!ValitedateRaidAndCombat(player))
                    return false;
            }
            return true;
        }

        private bool ValitedateRaidAndCombat(UnturnedPlayer player)
        {
            TeleportationPlugin teleportation = pluginInstance.TeleportationPlugin as TeleportationPlugin;
            if (teleportation.IsPlayerInCombat(player.CSteamID))
            {
                UnturnedChat.Say(player, pluginInstance.Translate("WhileCombat"), pluginInstance.MessageColor);
                return false;
            }

            if (teleportation.IsPlayerInRaid(player.CSteamID))
            {
                UnturnedChat.Say(player, pluginInstance.Translate("WhileRaid"), pluginInstance.MessageColor);
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
