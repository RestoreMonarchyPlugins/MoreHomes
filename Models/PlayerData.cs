using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class PlayerData
    {
        public PlayerData() 
        {
            Homes = new List<PlayerHome>();
        }

        public PlayerData(ulong playerId)
        {
            PlayerId = playerId;
            DefaultHomeName = "bed";
            Homes = new List<PlayerHome>();
        }

        public string GetUniqueHomeName()
        {
            int num = 1;
            while (Homes.Exists(x => x.Name.Equals(DefaultHomeName + num, StringComparison.OrdinalIgnoreCase)))
            {
                num++;
            }
            return DefaultHomeName + num;
        }

        public ulong PlayerId { get; set; }
        public string DefaultHomeName { get; set; }
        public List<PlayerHome> Homes { get; set; }
    }
}
