using RestoreMonarchy.MoreHomes.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Utilities
{
    public static class PlayerDataUtility
    {
        public static bool RenameBed(this List<PlayerData> data, PlayerHome home, string newName)
        {
            if (home.Owner.Homes.Exists(x => x.Name.Equals(newName, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            home.Name = newName;
            return true;
        }

        public static void UpdateBeds(this List<PlayerData> data)
        {
            foreach (PlayerData player in data)
            {
                foreach (PlayerHome home in player.Homes)
                {
                    if (home.Transform.position != null)
                        home.Position = new ConvertablePosition(home.Transform.position);
                    else
                        player.Homes.Remove(home);
                }
            }
        }

        public static void InitializeBeds(this List<PlayerData> data)
        {
            foreach (PlayerData player in data)
            {
                foreach (PlayerHome home in player.Homes)
                {
                    home.Owner = player;
                    home.InitializeBed();
                }
            }
        }

        public static void InitializeBed(this PlayerHome home)
        {
            foreach (var region in BarricadeManager.regions)
            {
                for (int i = 0; i < region.drops.Count; i++)
                {
                    Transform transform = region.drops[i].model;
                    if (transform.position.x == home.Position.X && transform.position.y == home.Position.Y && transform.position.z == home.Position.Z)
                    {
                        home.Transform = transform;
                        return;
                    }
                }
            }
        }

        public static void DestroyBed(this List<PlayerData> data, Transform transform)
        {
            PlayerData player = data.FirstOrDefault(x => x.Homes.Exists(y => y.Transform == transform));

            if (player != null)
            {
                PlayerHome home = player.Homes.FirstOrDefault(x => x.Transform == transform);
                player.Homes.Remove(home);
                home.Owner = null;
            }
        }

        public static int GetBedsCount(this List<PlayerData> data, ulong playerId)
        {
            return data.GetPlayer(playerId).Homes.Count;
        }

        public static void ClaimBed(this List<PlayerData> data, ulong playerId, Transform transform)
        {
            PlayerData player = data.GetPlayer(playerId);
            player.Homes.Add(new PlayerHome(player.GetUniqueBedName(), transform, player));
        }       

        public static string GetUniqueBedName(this PlayerData player)
        {
            int num = player.Homes.Count;

            while (player.Homes.Exists(x => x.Name.Equals(player.DefaultHomeName + num, StringComparison.OrdinalIgnoreCase)))
            {
                num++;
            }

            return player.DefaultHomeName + num;
        }

        public static PlayerHome GetPlayerBed(this List<PlayerData> data, ulong playerId, string name = null)
        {
            PlayerData player = data.GetPlayer(playerId);
            return player.Homes.FirstOrDefault(x => name == null || x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static PlayerData GetPlayer(this List<PlayerData> data, ulong playerId)
        {
            PlayerData player = data.FirstOrDefault(x => x.PlayerId == playerId);
            if (player == null)
            {
                player = data.InitializePlayer(playerId);
            }
            return player;
        }

        public static PlayerData InitializePlayer(this List<PlayerData> data, ulong playerId)
        {
            PlayerData player = new PlayerData(playerId);
            data.Add(player);
            return player;
        }
    }
}
