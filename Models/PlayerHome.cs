using Newtonsoft.Json;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class PlayerHome
    {
        public PlayerHome() { }
        public PlayerHome(string name, InteractableBed interactableBed)
        {
            Name = name;
            InteractableBed = interactableBed;
            Position = new ConvertablePosition(InteractableBed.transform.position);
        }

        public string Name { get; set; }
        public ConvertablePosition Position { get; set; }

        [JsonIgnore]
        public Vector3 LivePosition 
        { 
            get 
            {
                if (InteractableBed != null)
                    return InteractableBed.transform.position;
                else
                    return Position.ToVector3();
            }
        }

        [JsonIgnore]
        public InteractableBed InteractableBed { get; set; }

        public void Claim(CSteamID steamID)
        {
            if (InteractableBed == null ||
                !BarricadeManager.tryGetInfo(InteractableBed.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                return;

            if (plant == 65535)
            {
                BarricadeManager.instance.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    x,
                    y,
                    plant,
                    index,
                    steamID
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
                    steamID
                });
            }

            BitConverter.GetBytes(InteractableBed.owner.m_SteamID).CopyTo(region.barricades[index].barricade.state, 0);
        }


        public void Unclaim()
        {
            if (InteractableBed == null || 
                !BarricadeManager.tryGetInfo(InteractableBed.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                return;

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
            BitConverter.GetBytes(InteractableBed.owner.m_SteamID).CopyTo(region.barricades[index].barricade.state, 0);
        }

        public void Destroy()
        {
            if (!BarricadeManager.tryGetInfo(InteractableBed.transform, out var x, out var y, out var plant, out var index, out var region))
                return;
            
            BarricadeManager.destroyBarricade(region, x, y, plant, index);
        }
    }
}
