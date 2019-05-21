using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreHomes.Models
{
    public class PlayerBed
    {
        public PlayerBed(ulong steamId, string bedName, byte x, byte y, Vector3 position)
        {
            this.SteamId = steamId;
            this.BedName = bedName;
            this.X = x;
            this.Y = y;
            this.Position = position.ToString();
        }

        public PlayerBed() { }

        public ObjectId Id { get; set; }
        public ulong SteamId { get; set; }
        public string BedName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Position { get; set; }
    }
}
