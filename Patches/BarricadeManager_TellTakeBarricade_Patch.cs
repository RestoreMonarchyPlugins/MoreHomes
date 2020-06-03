using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "tellTakeBarricade")]
    public static class BarricadeManager_TellTakeBarricade_Patch
    {
        [HarmonyPrefix]
        public static void TellTakeBarricade_Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {            
            if (BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion))
            {
                InteractableBed interactableBed = barricadeRegion.drops[index].interactable as InteractableBed;
                if (interactableBed != null)
                {
                    HomesHelper.TryRemoveHome(interactableBed.owner, interactableBed.transform);
                }
            }            
        }
    }
}
