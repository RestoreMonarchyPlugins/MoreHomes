using System.Collections.Generic;

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

        public ulong PlayerId { get; set; }
        public string DefaultHomeName { get; set; }
        public List<PlayerHome> Homes { get; set; }
    }
}
