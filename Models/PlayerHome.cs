using Newtonsoft.Json;
using SDG.Unturned;
using Steamworks;
using System;
using Rocket.Unturned.Player;
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

            Reflection.ServerSetBedOwnerInternal(InteractableBed, x, y, plant, index, region, steamID);
        }

        public void Unclaim()
        {
            if (InteractableBed == null || 
                !BarricadeManager.tryGetInfo(InteractableBed.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                return;

            Reflection.ServerSetBedOwnerInternal(InteractableBed, x, y, plant, index, region, CSteamID.Nil);
        }

        public void Destroy()
        {
            if (!BarricadeManager.tryGetInfo(InteractableBed.transform, out var x, out var y, out var plant, out var index, out var region))
                return;
            
            BarricadeManager.destroyBarricade(region, x, y, plant, index);
        }
    }
}
