using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade", 
        typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort))]
    class BarricadeManager_destroyBarricade_Patch
    {
        [HarmonyPrefix]
        static void destroyBarricade_Prefix(BarricadeDrop barricade, byte x, byte y, ushort plant)
        {
            InteractableBed interactableBed = barricade.interactable as InteractableBed;
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
