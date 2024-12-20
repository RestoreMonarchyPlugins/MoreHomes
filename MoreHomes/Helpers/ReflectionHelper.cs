using SDG.Unturned;
using Steamworks;
using System.Reflection;

namespace RestoreMonarchy.MoreHomes.Helpers
{
    internal static class ReflectionHelper
    {
        private static MethodInfo BarricadeManager_ServerSetBedOwnerInternal = typeof(BarricadeManager).GetMethod("ServerSetBedOwnerInternal", BindingFlags.Static | BindingFlags.NonPublic);

        internal static void ServerSetBedOwnerInternal(InteractableBed interactableBed, byte x, byte y, ushort plant, BarricadeRegion region, CSteamID owner)
        {
            BarricadeManager_ServerSetBedOwnerInternal.Invoke(null, new object[] { interactableBed, x, y, plant, region, owner });
        }
    }
}
