using System;
using System.Reflection;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.MoreHomes
{
    public class Reflection
    {
        private static MethodInfo m_Method = typeof(BarricadeManager).GetMethod("ServerSetBedOwnerInternal", BindingFlags.NonPublic | BindingFlags.Static);
        
        public static void ServerSetBedOwnerInternal(InteractableBed bed, byte x, byte y, ushort plant, ushort index, BarricadeRegion region, CSteamID steamID)
        {
            m_Method.Invoke(null, new object[] {bed, x, y, plant, index, region, steamID});
        }
    }
}