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
    public static class Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
            BarricadeManager.tryGetRegion(x, y, plant, out BarricadeRegion barricadeRegion);
            InteractableBed interactableBed = barricadeRegion.drops[(int)index].interactable as InteractableBed;
            Player player = PlayerTool.getPlayer(steamID);
            Vector3 position = interactableBed.transform.position;

            if (interactableBed != null && interactableBed.isClaimable && interactableBed.checkClaim(player.channel.owner.playerID.steamID))
            {
                if (interactableBed.isClaimed)
                {
                    Database.RemoveBed(position.ToString());
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
                    bool result = Database.AddBed($"bed{index}", steamID, position);
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
                BitConverter.GetBytes(interactableBed.owner.m_SteamID).CopyTo(barricadeRegion.barricades[(int)index].barricade.state, 0);
            }
            return true;

        }

        [HarmonyPrefix]
        public static void Postfix(BarricadeManager __instance)
        {


        }
    }
}
