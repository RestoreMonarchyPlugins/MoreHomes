using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System;
using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;

namespace RestoreMonarchy.MoreHomes.Patches
{
    public static class BarricadeManager_ReceiveClaimBedRequest_Patch
    {
        public static bool Prefix(in ServerInvocationContext context, byte x, byte y, ushort plant, ushort index)
        {
	        if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out var barricadeRegion))
		        return false;
	        var player = context.GetPlayer();
	        var steamID = player.channel.owner.playerID.steamID;
	        try
	        {
				if (player == null)
				{
					return false;
				}
				if (player.life.isDead)
				{
					return false;
				}
				InteractableBed interactableBed = barricadeRegion.drops[index].interactable as InteractableBed;
				if ((interactableBed.transform.position - player.transform.position).sqrMagnitude > 400f)
				{
					return false;
				}
				if (interactableBed.isClaimable && interactableBed.checkClaim(player.channel.owner.playerID.steamID))
				{
					if (interactableBed.isClaimed)
					{
						var home = HomesHelper.GetPlayerHome(steamID, interactableBed);
						Console.WriteLine(home);
						HomesHelper.RemoveHome(steamID, home);
						home.Unclaim();
					}
					else
					{
						var playerData = HomesHelper.GetOrCreatePlayer(steamID);
						int maxHomes = VipHelper.GetPlayerMaxHomes(steamID.ToString());
						if (maxHomes == 1 && playerData.Homes.Count == 1)
						{
							foreach (var home in playerData.Homes.ToArray())
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
				            
						var playerHome = new PlayerHome(playerData.GetUniqueHomeName(), interactableBed);
						playerData.Homes.Add(playerHome);
						playerHome.Claim(steamID);
						UnturnedChat.Say(steamID, MoreHomesPlugin.Instance.Translate("HomeClaimed", playerHome.Name), MoreHomesPlugin.Instance.MessageColor);
					}
				}
	        }
	        catch (Exception e)
	        {
		        UnturnedChat.Say(steamID, MoreHomesPlugin.Instance.Translate("ReclaimTheBed"), MoreHomesPlugin.Instance.MessageColor);
	        }
	        
			return false;
		}
    }
}
