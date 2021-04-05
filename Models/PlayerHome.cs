using Newtonsoft.Json;
using SDG.Unturned;
using System.Reflection;
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

        public void Claim(Player player)
        {
            if (InteractableBed == null)
                return;

            if (!BarricadeManager.tryGetInfo(InteractableBed.transform, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                return;

            typeof(BarricadeManager).GetMethod("ServerSetBedOwnerInternal", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] {
                    InteractableBed, 
                    x, 
                    y, 
                    plant, 
                    index, 
                    region, 
                    player.channel.owner.playerID.steamID
                });
        }

        public void Unclaim()
        {
            if (InteractableBed == null)
                return;

            BarricadeManager.ServerUnclaimBed(InteractableBed);
        }

        public void Destroy()
        {
            if (!BarricadeManager.tryGetInfo(InteractableBed.transform, out var x, out var y, out var plant, out var index, out var region))
                return;
            
            BarricadeManager.destroyBarricade(region, x, y, plant, index);
        }
    }
}
