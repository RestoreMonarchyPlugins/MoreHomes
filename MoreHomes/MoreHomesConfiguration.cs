using RestoreMonarchy.MoreHomes.Models;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.MoreHomes
{
    public class MoreHomesConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public string MessageIconUrl { get; set; } = "https://i.imgur.com/9TF5aB1.png";
        public int DefaultHomeCooldown { get; set; }
        public int DefaultHomeDelay { get; set; }
        public int DefaultMaxHomes { get; set; }
        public float TeleportHeight { get; set; }
        public bool CancelOnMove { get; set; }
        public float MoveMaxDistance { get; set; }
        public bool BlockUnderground { get; set; } = false;
        public List<VIPPermission> VIPCooldowns { get; set; }
        public List<VIPPermission> VIPDelays { get; set; }
        public List<VIPPermission> VIPMaxHomes { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            MessageIconUrl = "https://i.imgur.com/9TF5aB1.png";
            DefaultHomeCooldown = 20;
            DefaultHomeDelay = 10;
            DefaultMaxHomes = 2;
            TeleportHeight = 0.5f;
            CancelOnMove = true;
            MoveMaxDistance = 0.5f;
            BlockUnderground = false;

            VIPCooldowns = new List<VIPPermission>()
            {
                new VIPPermission("morehomes.vip", 10),
                new VIPPermission("morehomes.star", 5)
            }; 
            VIPDelays = new List<VIPPermission>()
            {
                new VIPPermission("morehomes.vip", 5),
                new VIPPermission("morehomes.star", 3)
            };
            VIPMaxHomes = new List<VIPPermission>()
            {
                new VIPPermission("morehomes.vip", 3),
                new VIPPermission("morehomes.star", 4)
            };
        }
    }
}
