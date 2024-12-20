using Newtonsoft.Json;
using RestoreMonarchy.MoreHomes.Helpers;
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
            {
                return;
            }

            if (!BarricadeManager.tryGetRegion(InteractableBed.transform, out byte x, out byte y, out ushort plant, out BarricadeRegion region))
            {
                return;
            }

            ReflectionHelper.ServerSetBedOwnerInternal(InteractableBed, x, y, plant, region, player.channel.owner.playerID.steamID);
        }

        public void Unclaim()
        {
            if (InteractableBed == null)
            {
                return;
            }   

            BarricadeManager.ServerUnclaimBed(InteractableBed);
        }

        public void Destroy()
        {
            if (InteractableBed == null)
            {
                return;
            }

            BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(InteractableBed.transform);
            if (drop == null)
            {
                return;
            }
            
            if (!BarricadeManager.tryGetRegion(InteractableBed.transform, out byte x, out byte y, out ushort plant, out _))
            {
                return;
            }

            Unclaim();
            BarricadeManager.destroyBarricade(drop, x, y, plant);
        }
    }
}
