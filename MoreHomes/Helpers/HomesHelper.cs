using RestoreMonarchy.MoreHomes.Models;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Helpers
{
    public class HomesHelper
    {
        private static MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;
        private static List<PlayerData> playersData => pluginInstance.DataService.PlayersData;

        public static PlayerData GetOrCreatePlayer(CSteamID steamID)
        {
            PlayerData player = playersData.FirstOrDefault(x => x.PlayerId == steamID.m_SteamID);
            if (player == null)
            {
                player = new PlayerData(steamID.m_SteamID);
                playersData.Add(player);
            }

            return player;
        }

        public static PlayerHome GetPlayerHome(CSteamID steamID, string name = null)
        {
            PlayerData player = GetOrCreatePlayer(steamID);

            return player.Homes.FirstOrDefault(x => name == null || x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static PlayerHome GetPlayerHome(CSteamID steamID, InteractableBed interactableBed)
        {
            PlayerData player = GetOrCreatePlayer(steamID);

            return player.Homes.FirstOrDefault(x => x.InteractableBed == interactableBed);
        }

        public static PlayerHome GetPlayerHome(CSteamID steamID, Vector3 position)
        {
            PlayerData player = GetOrCreatePlayer(steamID);

            return player.Homes.FirstOrDefault(x =>
                Math.Abs(x.Position.X - position.x) <= 1 &&
                Math.Abs(x.Position.Y - position.y) <= 1 &&
                Math.Abs(x.Position.Z - position.z) <= 1
            );
        }

        public static bool RemoveHome(CSteamID steamID, PlayerHome playerHome)
        {
            PlayerData player = GetOrCreatePlayer(steamID);

            return player.Homes.Remove(playerHome);
        }

        public static void ClearHomes()
        {
            playersData.Clear();
        }
    }
}