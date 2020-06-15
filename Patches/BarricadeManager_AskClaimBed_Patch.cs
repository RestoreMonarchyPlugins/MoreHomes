using HarmonyLib;
using SDG.Unturned;
using Steamworks;
using System;
using RestoreMonarchy.MoreHomes.Helpers;

namespace RestoreMonarchy.MoreHomes.Patches
{
	[HarmonyPatch(typeof(BarricadeManager), "askClaimBed")]
    public static class BarricadeManager_AskClaimBed_Patch
    {
        [HarmonyPrefix]
        public static bool AskClaimBed_Prefix(BarricadeManager __instance, CSteamID steamID, byte x, byte y, ushort plant, ushort index)
        {
			BarricadeRegion barricadeRegion;
			if (Provider.isServer && BarricadeManager.tryGetRegion(x, y, plant, out barricadeRegion))
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return false;
				}
				if (player.life.isDead)
				{
					return false;
				}
				if (index >= barricadeRegion.drops.Count)
				{
					return false;
				}
				if ((barricadeRegion.drops[index].model.transform.position - player.transform.position).sqrMagnitude > 400f)
				{
					return false;
				}
				InteractableBed interactableBed = barricadeRegion.drops[index].interactable as InteractableBed;
				if (interactableBed != null && interactableBed.isClaimable && interactableBed.checkClaim(player.channel.owner.playerID.steamID))
				{
					if (interactableBed.isClaimed)
					{
						#region RemoveHome

						HomesHelper.TryRemoveHome(steamID, interactableBed);

						#endregion

                        if (plant == 65535)
						{
							BarricadeManager.instance.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
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
							BarricadeManager.instance.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								x,
								y,
								plant,
								index,
								CSteamID.Nil
							});
						}
					}
					else
					{
						#region AddHome

						if (!HomesHelper.TryClaimHome(steamID, interactableBed))
						{
							return false;
						}

						#endregion

						BarricadeManager.unclaimBeds(player.channel.owner.playerID.steamID);
						if (plant == 65535)
						{
							BarricadeManager.instance.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
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
							BarricadeManager.instance.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
							{
								x,
								y,
								plant,
								index,
								player.channel.owner.playerID.steamID
							});
						}
					}
					BitConverter.GetBytes(interactableBed.owner.m_SteamID).CopyTo(barricadeRegion.barricades[index].barricade.state, 0);
				}
			}
			return true;

		}
    }
}
