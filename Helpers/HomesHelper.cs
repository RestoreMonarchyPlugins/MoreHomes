using RestoreMonarchy.MoreHomes.Models;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Helpers
{
    public class HomesHelper
    {
        private static MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        private static List<PlayerData> playersData => pluginInstance.DataService.PlayersData;

        public static PlayerData GetOrCreatePlayer(CSteamID steamID)
        {
            var player = playersData.FirstOrDefault(x => x.PlayerId == steamID.m_SteamID);
            if (player == null)
            {
                player = new PlayerData(steamID.m_SteamID);
                playersData.Add(player);
            }
            return player;
        }

        public static PlayerHome GetPlayerHome(CSteamID steamID, string name = null)
        {
            var player = GetOrCreatePlayer(steamID);
            return player.Homes.FirstOrDefault(x => name == null || x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static bool TryClaimHome(CSteamID steamID, InteractableBed interactableBed)
        {
            var player = GetOrCreatePlayer(steamID);
            if (player.Homes.Count >= VipHelper.GetPlayerMaxHomes(steamID.ToString()))
            {
                UnturnedChat.Say(steamID, pluginInstance.Translate("MaxHomesWarn"), pluginInstance.MessageColor);
                return false;
            }
            var home = new PlayerHome(player.GetUniqueHomeName(), interactableBed);
            player.Homes.Add(home);
            UnturnedChat.Say(steamID, pluginInstance.Translate("HomeClaimed", home.Name), pluginInstance.MessageColor);
            return true;
        }

        public static bool TryRemoveHome(CSteamID steamID, InteractableBed interactableBed)
        {
            var player = GetOrCreatePlayer(steamID);
            var home = player.Homes.FirstOrDefault(x => x.InteractableBed == interactableBed);
            if (home != null)
            {
                player.Homes.Remove(home);
                UnturnedChat.Say(steamID, pluginInstance.Translate("RemoveHome", home.Name), pluginInstance.MessageColor);
                return true;
            } else
            {
                UnturnedChat.Say(steamID, pluginInstance.Translate("RemoveHomeFail"), pluginInstance.MessageColor);
            }
            return false;
        }

        public static bool TryRenameHome(CSteamID steamID, string oldName, string newName)
        {
            var home = GetPlayerHome(steamID, oldName);
            if (home != null)
            {
                if (GetPlayerHome(steamID, newName) != null)
                {
                    UnturnedChat.Say(steamID, pluginInstance.Translate("RenameHomeFail", newName), pluginInstance.MessageColor);
                    return false;
                }

                home.Name = newName;
                UnturnedChat.Say(steamID, pluginInstance.Translate("RenameHomeSuccess", oldName, newName), pluginInstance.MessageColor);
                return true;
            }
            else
            {
                UnturnedChat.Say(steamID, pluginInstance.Translate("HomeNotFound", newName), pluginInstance.MessageColor);
                return false;
            }
        }

        public static void ClearHomes()
        {
            playersData.Clear();
        }
    }
}