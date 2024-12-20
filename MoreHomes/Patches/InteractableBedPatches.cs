using HarmonyLib;
using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace RestoreMonarchy.MoreHomes.Patches
{
    [HarmonyPatch(typeof(InteractableBed))]
    class InteractableBedPatches
	{
		[HarmonyPatch(nameof(InteractableBed.ReceiveClaimRequest))]
        [HarmonyPrefix]
        static bool ReceiveClaimRequestPrefix(InteractableBed __instance, in ServerInvocationContext context)
        {
			if (__instance == null)
			{
                return false;
            }

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

			if (__instance.isClaimable && __instance.checkClaim(steamID))
			{
				if (__instance.isClaimed)
				{
                    PlayerHome home = HomesHelper.GetPlayerHome(steamID, __instance);
					if (home != null)
					{
                        HomesHelper.RemoveHome(steamID, home);
                    }					
					home.Unclaim();
				}
				else
				{
					UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(player);
                    PlayerData playerData = HomesHelper.GetOrCreatePlayer(steamID);
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
                        MoreHomesPlugin.Instance.SendMessageToPlayer(unturnedPlayer, "MaxHomesWarn");
						return false;
					}

                    PlayerHome playerHome = new(playerData.GetUniqueHomeName(), __instance);
					playerData.Homes.Add(playerHome);
					playerHome.Claim(player);
                    MoreHomesPlugin.Instance.SendMessageToPlayer(unturnedPlayer, "HomeClaimed", playerHome.Name);
				}
			}

			return false;
		}
    }
}
