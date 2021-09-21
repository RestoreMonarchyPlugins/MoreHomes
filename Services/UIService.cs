using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using Rocket.Core;
using Rocket.Core.Steam;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Services
{
    public class UIService : MonoBehaviour
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        private MoreHomesConfiguration config => pluginInstance.Configuration.Instance;

        private ushort EffectId => config.DeathUIEffectId;
        private const short Key = 31319;

        void Awake()
        {
            U.Events.OnPlayerConnected += OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerDeath += OnPlayerDeath;
            EffectManager.onEffectButtonClicked += OnButtonClicked;
        }

        void OnDestroy()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerDeath -= OnPlayerDeath;
            EffectManager.onEffectButtonClicked -= OnButtonClicked;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            player.Player.disablePluginWidgetFlag(EPluginWidgetFlags.ShowDeathMenu);
        }

        private void OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            Player killer = PlayerTool.getPlayer(murderer);
            UnturnedPlayer killerPlayer = null;
            if (killer != null)
            {
                killerPlayer = UnturnedPlayer.FromPlayer(killer);
            }
            Player victim = player.Player;
             
            if (config.OpenUICommand != null)
            {
                R.Commands.Execute(player, config.OpenUICommand);
            }

            EffectManager.sendUIEffect(EffectId, Key, victim.channel.GetOwnerTransportConnection(), true);

            if (killer != null && killerPlayer != null && cause != EDeathCause.SUICIDE)
            {
                EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Killer_Name", killerPlayer.DisplayName);
                if (cause == EDeathCause.GUN || cause == EDeathCause.MELEE)
                {
                    EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Silah", true);
                    EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Metre", true);

                    if (killer.equipment.asset != null)
                        EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, $"NTV_Silah_Name", "WEAPON: " + killer.equipment.asset.itemName);
                    else
                        EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, $"NTV_Silah_Name", "WEAPON: unkown");

                    EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, $"NTV_Metre_Yazi", "METRE: " + Math.Truncate(Vector3.Distance(killer.transform.position, player.Position)));
                } else
                {
                    EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Silah", true);
                    EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Silah", pluginInstance.Translate(cause.ToString()));
                    EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Metre", false);
                }
                 
                // Download player profile and send avatar url when it's done
                ThreadHelper.RunAsynchronously(() =>
                {
                    Profile profile = null;
                    
                    if (killerPlayer != null)
                    {                        
                        profile = killerPlayer.SteamProfile;
                    }

                    ThreadHelper.RunSynchronously(() =>
                    {
                        EffectManager.sendUIEffectImageURL(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Killer_Photo", profile.AvatarFull.ToString(), true, false);
                    });
                });
            }
            else
            {
                EffectManager.sendUIEffectImageURL(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Killer_Photo", config.DefaultKillerImageUrl, true, false);
                EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Killer_Name", pluginInstance.Translate(cause.ToString()));
                EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Silah", false);
                EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Metre", false);
            }

            
            EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, "NTV_Beds", true);

            PlayerData playerData = HomesHelper.GetOrCreatePlayer(player.CSteamID);

            for (int num = 0; num < playerData.Homes.Count; num++)
            {
                EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, string.Format("NTV_Bed_{0}", num), true);
                EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, string.Format("NTV_Bed_{0}_Text", num), true);
                EffectManager.sendUIEffectVisibility(Key, victim.channel.GetOwnerTransportConnection(), true, string.Format("NTV_Bed_{0}_Sil", num), true);
                EffectManager.sendUIEffectText(Key, victim.channel.GetOwnerTransportConnection(), true, string.Format("NTV_Bed_{0}_Text", num), playerData.Homes[num].Name);
            }
        }

        private void OnButtonClicked(Player player, string buttonName)
        {
            UnturnedPlayer untPlayer = UnturnedPlayer.FromPlayer(player);
            PlayerData playerData = HomesHelper.GetOrCreatePlayer(untPlayer.CSteamID);

            if (buttonName == "NTV_Respawn")
            {
                player.life.sendRespawn(false);
                player.life.ServerRespawn(false);
            } else if (buttonName.StartsWith("NTV_Bed_") && !buttonName.EndsWith("Sil"))
            {
                int index = int.Parse(Regex.Match(buttonName, @"\d+").Value);
                PlayerHome home = playerData.Homes.ElementAtOrDefault(index);
                if (home == null)
                {
                    UnturnedChat.Say(untPlayer, pluginInstance.Translate("HomeNotFound2"), pluginInstance.MessageColor);
                }
                if (buttonName.EndsWith("Sil"))
                {
                    EffectManager.sendUIEffectVisibility(Key, untPlayer.SteamPlayer().transportConnection, true, $"NTV_Bed_{index}", false);
                    home.Destroy();
                    HomesHelper.RemoveHome(untPlayer.CSteamID, home);
                    return;
                } else
                {
                    player.life.sendRespawn(false);
                    player.life.ServerRespawn(false);

                    untPlayer.Player.teleportToLocation(home.LivePosition + new Vector3(0f, config.TeleportHeight, 0f), 0);
                }
            }

            EffectManager.askEffectClearByID(EffectId, player.channel.GetOwnerTransportConnection());
            if (config.CloseUICommand != null)
            {
                R.Commands.Execute(untPlayer, config.CloseUICommand);
            }
        }
    }
}
