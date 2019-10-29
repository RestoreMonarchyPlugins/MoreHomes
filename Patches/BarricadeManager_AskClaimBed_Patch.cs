using Harmony;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;
using RestoreMonarchy.MoreHomes.Utilities;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(BarricadeManager), "askClaimBed")]
    public static class BarricadeManager_AskClaimBed_Patch
    {
        [HarmonyPrefix]
        public static bool AskClaimBed_Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion);
            InteractableBed interactableBed = barricadeRegion.drops[(int)index].interactable as InteractableBed;      
            Player player = PlayerTool.getPlayer(steamID);
            
            if (interactableBed != null && interactableBed.isClaimable && interactableBed.checkClaim(steamID))
            {
                if (interactableBed.isClaimed)
                {
                    // "Destroy" if unclaim
                    MoreHomesPlugin.Instance.DataCache.DestroyBed(interactableBed.transform);
                    if (plant == 65535)
                    {
                        __instance.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                                x,
                                y,
                                plant,
                                index,
                                CSteamID.Nil
                        });
                    }
                    else
                    {
                        __instance.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                                x,
                                y,
                                plant,
                                index,
                                CSteamID.Nil
                        });
                    }
                    return false;
                }
                else
                {
                    // Inserted Code
                    int bedsCount = MoreHomesPlugin.Instance.DataCache.GetBedsCount(player.channel.owner.playerID.steamID.m_SteamID);
                    if (bedsCount >= MoreHomesPlugin.Instance.GetMaxBeds(player.channel.owner.playerID.steamID.m_SteamID))
                    {
                        UnturnedChat.Say(steamID, MoreHomesPlugin.Instance.Translate("MaxHomesWarn"), MoreHomesPlugin.Instance.MessageColor);
                        return false;
                    }

                    MoreHomesPlugin.Instance.DataCache.ClaimBed(steamID.m_SteamID, interactableBed.transform);

                    if (plant == 65535)
                    {
                        __instance.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                                x,
                                y,
                                plant,
                                index,
                                player.channel.owner.playerID.steamID
                        });
                    }
                    else
                    {
                        __instance.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                                x,
                                y,
                                plant,
                                index,
                                player.channel.owner.playerID.steamID
                        });
                    }

                    
                }
                BitConverter.GetBytes(interactableBed.owner.m_SteamID).CopyTo(barricadeRegion.barricades[(int)index].barricade.state, 0);
            }
            return true;

        }
    }
}
