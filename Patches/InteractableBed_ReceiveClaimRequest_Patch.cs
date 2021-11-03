using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System;
using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using Rocket.Unturned.Chat;

namespace RestoreMonarchy.MoreHomes.Patches
{
	[HarmonyPatch(typeof(InteractableBed), "ReceiveClaimRequest")]
    class InteractableBed_ReceiveClaimRequest_Patch
	{
        [HarmonyPrefix]
        static bool ReceiveClaimRequest_Prefix(InteractableBed __instance, in ServerInvocationContext context)
        {
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(__instance.transform, out x, out y, out plant, out region))
			{
				return false;
			}

			Player player = context.GetPlayer();			

			if (player == null)
			{
				return false;
			}
			if (player.life.isDead)
			{
				return false;
			}
			if ((__instance.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return false;
			}

			CSteamID steamID = player.channel.owner.playerID.steamID;

			if (__instance != null && __instance.isClaimable && __instance.checkClaim(player.channel.owner.playerID.steamID))
			{
				if (__instance.isClaimed)
				{
                    PlayerHome home = HomesHelper.GetPlayerHome(steamID, __instance);
					HomesHelper.RemoveHome(steamID, home);
					home.Unclaim();
				}
				else
				{
					var playerData = HomesHelper.GetOrCreatePlayer(steamID);
					int maxHomes = VipHelper.GetPlayerMaxHomes(steamID.ToString());
					if (maxHomes == 1 && playerData.Homes.Count == 1)
					{
						foreach (PlayerHome home in playerData.Homes.ToArray())
						{
							HomesHelper.RemoveHome(steamID, home);
							home.Unclaim();
						}
					}
					else if (maxHomes <= playerData.Homes.Count)
					{
						UnturnedChat.Say(steamID, MoreHomesPlugin.Instance.Translate("MaxHomesWarn"), MoreHomesPlugin.Instance.MessageColor);
						return false;
					}

                    PlayerHome playerHome = new PlayerHome(playerData.GetUniqueHomeName(), __instance);
					playerData.Homes.Add(playerHome);
					playerHome.Claim(player);
					UnturnedChat.Say(steamID, MoreHomesPlugin.Instance.Translate("HomeClaimed", playerHome.Name), MoreHomesPlugin.Instance.MessageColor);
				}
			}

			return false;
		}
    }
}
