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
    [HarmonyPatch(typeof(BarricadeManager), "askClaimBed")]
    public static class BarricadeManager_AskClaimBed_Patch
    {
        [HarmonyPrefix]
        public static bool AskClaimBed_Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion);
            InteractableBed bed = barricadeRegion.drops[(int)index].interactable as InteractableBed;      
            Player player = PlayerTool.getPlayer(steamID);

            if (bed != null && bed.isClaimable && bed.checkClaim(player.channel.owner.playerID.steamID))
            {
                if (bed.isClaimed)
                {
                    MoreHomes.Instance.Database.RemoveBedByPosition(x, y, bed.transform.position);
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
                    string bedName = MoreHomes.Instance.Database.GetNameForBed(steamID);
                    bool result = MoreHomes.Instance.Database.AddBed(steamID, bedName, x, y, bed.transform.position);
                    if (!result)
                    {
                        UnturnedChat.Say(steamID, MoreHomes.Instance.Translate("home_max_warn"), Color.red);
                        return false;

                    }
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
                BitConverter.GetBytes(bed.owner.m_SteamID).CopyTo(barricadeRegion.barricades[(int)index].barricade.state, 0);
            }
            return true;

        }
    }
}
