using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager))]
    class BarricadeManagerPatches
    {
        [HarmonyPatch(nameof(BarricadeManager.salvageBarricade))]
        [HarmonyPrefix]
        static void salvageBarricadePrefix(Transform transform)
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
        static void destroyBarricadePrefix(BarricadeDrop barricade, byte x, byte y, ushort plant)
        {
            InteractableBed interactableBed = barricade.interactable as InteractableBed;
            if (interactableBed != null)
            {
                var home = HomesHelper.GetPlayerHome(interactableBed.owner, interactableBed);
                if (home != null)
                {
                    HomesHelper.RemoveHome(interactableBed.owner, home);
                    Player player = PlayerTool.getPlayer(interactableBed.owner);
                    if (player != null)
                    {
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(player);
                        MoreHomesPlugin.Instance.SendMessageToPlayer(unturnedPlayer, "HomeDestroyed", home.Name);
                    }
                }
            }
        }
    }
}
