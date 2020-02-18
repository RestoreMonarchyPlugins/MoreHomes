using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.MoreHomes
{
    public class MoreHomesConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }        
        public double DefaultHomeCooldown { get; set; }
        public float DefaultHomeDelay { get; set; }
        public int DefaultMaxHomes { get; set; }        
        public List<VIPPermission> VIPCooldowns { get; set; }
        public List<VIPPermission> VIPDelays { get; set; }
        public List<VIPPermission> VIPMaxHomes { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            DefaultHomeCooldown = 20;
            DefaultHomeDelay = 10;
            DefaultMaxHomes = 2;

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
