using Harmony;
using SDG.Unturned;
using Steamworks;

namespace MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "tellTakeBarricade")]
    public static class BarricadeManager_TellTakeBarricade_Patch
    {
        [HarmonyPrefix]
        public static void TellTakeBarricade_Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {            
            if (BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion))
            {
                InteractableBed interactableBed = barricadeRegion.drops[(int)index].interactable as InteractableBed;

                if (interactableBed != null)
                {
                    Player player = PlayerTool.getPlayer(steamID);

                    MoreHomes.Instance.Database.RemoveBedByPosition(x, y, interactableBed.transform.position);
                }
            }            
        }
    }
}
