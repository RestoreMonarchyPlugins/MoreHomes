using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "tellTakeBarricade")]
    public static class BarricadeManager_TellTakeBarricade_Patch
    {
        [HarmonyPrefix]
        public static void TellTakeBarricade_Prefix(byte x, byte y, ushort plant, ushort index)
        {
            if (BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion))
            {
                InteractableBed interactableBed = barricadeRegion.drops[index].interactable as InteractableBed;
                if (interactableBed != null)
                {
                    var home = HomesHelper.GetPlayerHome(interactableBed.owner, interactableBed);
                    if (home != null)
                    {
                        HomesHelper.RemoveHome(interactableBed.owner, home);
                        if (PlayerTool.getPlayer(interactableBed.owner) != null)
                        {
                            UnturnedChat.Say(interactableBed.owner, MoreHomesPlugin.Instance.Translate("HomeDestroyed", home.Name), 
                                MoreHomesPlugin.Instance.MessageColor);
                        }
                    }                        
                }
            }            
        }
    }
}
