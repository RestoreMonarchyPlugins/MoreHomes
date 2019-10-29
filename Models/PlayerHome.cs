using Newtonsoft.Json;
using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class PlayerHome
    {
        public PlayerHome() { }
        public PlayerHome(string name, Transform transform, PlayerData owner)
        {
            Name = name;
            Transform = transform;
            Position = new ConvertablePosition(Transform.position);
            Owner = owner;
        }

        public string Name { get; set; }
        public ConvertablePosition Position { get; set; }
        
        [JsonIgnore]
        public Transform Transform { get; set; }
        [JsonIgnore]
        public PlayerData Owner { get; set; }
    }
}
