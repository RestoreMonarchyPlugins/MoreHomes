using Newtonsoft.Json;
using SDG.Unturned;
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
    }
}
