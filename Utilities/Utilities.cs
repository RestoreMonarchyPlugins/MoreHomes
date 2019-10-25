using LiteDB;
using MoreHomes.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreHomes.Helpers
{
    public static class Utilities
    {
        public static LiteDatabase GetLiteDb(string dbFileName, string subPath = null)
        {
            string path = Path.Combine(MoreHomes.Instance.Directory);
            if (string.IsNullOrEmpty(dbFileName))
            {
                throw new ArgumentNullException(nameof(dbFileName));
            }

            if (subPath != null)
            {
                path = Path.Combine(path, subPath);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return new LiteDatabase(Path.Combine(path, dbFileName));
        }

        public static Vector3 ToVector3(this string value)
        {
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Substring(1, value.Length - 2);
            }

            string[] sArray = value.Split(',');

            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));

            return result;
        }

        public static bool ShouldAllowTeleport(this PlayerBed bed)
        {
            Console.WriteLine(bed.Position.ToString());
            var barricadeRegion = BarricadeManager.regions[bed.X, bed.Y];
            
            foreach (var drop in barricadeRegion.drops)
            {
                Console.WriteLine(drop.model.position);
                Console.WriteLine("second: " + drop.interactable.transform.position);
            }

            var interactableBed = barricadeRegion?.drops?.FirstOrDefault(x => x.interactable.transform.position == bed.Vector3)?.interactable as InteractableBed;

            if (interactableBed != null)
            {
                Console.WriteLine("it's not null");
            }

            if (interactableBed != null && interactableBed.owner.m_SteamID == bed.SteamId && Level.checkSafeIncludingClipVolumes(interactableBed.transform.position))
            {
                Collider[] colliders = new Collider[2];
                Vector3 point = interactableBed.transform.position;
                int num2 = Physics.OverlapCapsuleNonAlloc(point + new Vector3(0f, PlayerStance.RADIUS, 0f), point + new Vector3(0f, 2.5f - PlayerStance.RADIUS, 0f), 
                    PlayerStance.RADIUS, colliders, RayMasks.BLOCK_STANCE, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < num2; i++)
                {
                    if (colliders[i].gameObject != interactableBed.gameObject)
                    {
                        Console.WriteLine("false");
                        return false;
                    }
                }
                return true;
            }

            Console.WriteLine("false2");
            return false;
        }

        public static bool IsDestroyed(this PlayerBed bed)
        {
            var barricadeRegion = BarricadeManager.regions[bed.X, bed.Y];

            if (barricadeRegion != null)
            {
                Console.WriteLine("barricadeRegion is not null");
            }
            var interactableBed = barricadeRegion?.drops?.FirstOrDefault(x => x.interactable.transform.position.Equals(bed.Vector3))?.interactable as InteractableBed ?? null;


            if (bed != null)
                return true;
            else
                return true;
        }
    }
}
