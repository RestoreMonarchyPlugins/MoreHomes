using Rocket.API;
using Rocket.Core;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.MoreHomes.Helpers
{
    public class VipHelper
    {
        private static MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;

        public static int GetPlayerMaxHomes(string playerId)
        {
            var rocketPlayer = new RocketPlayer(playerId);

            int maxHomes = pluginInstance.Configuration.Instance.DefaultMaxHomes;
            foreach (var vip in pluginInstance.Configuration.Instance.VIPMaxHomes)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    maxHomes = maxHomes < vip.Value ? vip.Value : maxHomes;
                }
            }
            return maxHomes;
        }

        public static int GetPlayerHomeDelay(string playerId)
        {
            var rocketPlayer = new RocketPlayer(playerId);

            int minDelay = pluginInstance.Configuration.Instance.DefaultHomeDelay;
            foreach (var vip in pluginInstance.Configuration.Instance.VIPDelays)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    minDelay = minDelay > vip.Value ? vip.Value : minDelay;
                }
            }
            return minDelay;
        }

        public static int GetPlayerHomeCooldown(string playerId)
        {
            var rocketPlayer = new RocketPlayer(playerId);

            int minCooldown = pluginInstance.Configuration.Instance.DefaultHomeCooldown;
            foreach (var vip in pluginInstance.Configuration.Instance.VIPCooldowns)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    minCooldown = minCooldown > vip.Value ? vip.Value : minCooldown;
                }
            }
            return minCooldown;
        }
    }
}
