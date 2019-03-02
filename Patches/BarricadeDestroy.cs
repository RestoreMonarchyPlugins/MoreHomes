using Harmony;
using LiteDB;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "tellTakeBarricade")]
    public static class BarricadeDestroy
    {
        [HarmonyPrefix]
        public static void Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion);

            Console.WriteLine(plant);

            try
            {
                InteractableBed interactableBed = barricadeRegion.drops[(int)index].interactable as InteractableBed;
                Player player = PlayerTool.getPlayer(steamID);

                Vector3 position = interactableBed.transform.position;
                Database.RemoveBed(position.ToString());
            } catch
            {

            }
            
        }

        [HarmonyPrefix]
        public static void Postfix(BarricadeManager __instance)
        {
        }
    }
}
