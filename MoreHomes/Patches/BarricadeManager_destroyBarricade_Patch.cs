using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager))]
    class BarricadeManager_destroyBarricade_Patch
    {
        [HarmonyPatch("salvageBarricade")]
        [HarmonyPrefix]
        static void salvageBarricade_Prefix(Transform transform)
        {
            InteractableBed interactableBed = transform.GetComponent<InteractableBed>();
            if (interactableBed != null)
            {
                var home = HomesHelper.GetPlayerHome(interactableBed.owner, interactableBed);
                if (home != null)
                {
                    HomesHelper.RemoveHome(interactableBed.owner, home);
                }
            }
        }

        [HarmonyPatch("destroyBarricade", typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort))]
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
