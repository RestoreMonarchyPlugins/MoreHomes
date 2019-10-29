using Rocket.API;
using Rocket.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.MoreHomes.Utilities
{
    public static class PlayerVipUtility
    {
        public static double GetMaxBeds(this MoreHomesPlugin pluginInstance, ulong playerId)
        {
            RocketPlayer rocketPlayer = new RocketPlayer(playerId.ToString());
            double maxCount = pluginInstance.Configuration.Instance.DefaultMaxHomes;

            foreach (VIPPermission vip in pluginInstance.Configuration.Instance.VIPMaxHomes)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    maxCount = maxCount > vip.Value ? maxCount : vip.Value;
                }
            }

            return maxCount;
        }

        public static float GetDelay(this MoreHomesPlugin pluginInstance, ulong playerId)
        {
            RocketPlayer rocketPlayer = new RocketPlayer(playerId.ToString());
            float delay = pluginInstance.Configuration.Instance.DefaultHomeDelay;

            foreach (VIPPermission vip in pluginInstance.Configuration.Instance.VIPDelays)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    delay = delay < vip.Value ? delay : vip.Value;
                }
            }

            return delay;
        }

        public static double GetCooldown(this MoreHomesPlugin pluginInstance, ulong playerId)
        {
            RocketPlayer rocketPlayer = new RocketPlayer(playerId.ToString());
            double cooldown = pluginInstance.Configuration.Instance.DefaultHomeCooldown;

            foreach (VIPPermission vip in pluginInstance.Configuration.Instance.VIPCooldowns)
            {
                if (rocketPlayer.HasPermission(vip.PermissionTag))
                {
                    cooldown = cooldown < vip.Value ? cooldown : vip.Value;
                }
            }

            return cooldown;
        }
    }
}
