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

        public static PlayerHome GetPlayerHome(CSteamID steamID, InteractableBed interactableBed)
        {
            var player = GetOrCreatePlayer(steamID);
            return player.Homes.FirstOrDefault(x => x.InteractableBed == interactableBed);
        } 

        public static bool RemoveHome(CSteamID steamID, PlayerHome playerHome)
        {
            var player = GetOrCreatePlayer(steamID);
            return player.Homes.Remove(playerHome);
        }

        public static void ClearHomes()
        {
            playersData.Clear();
        }
    }
}