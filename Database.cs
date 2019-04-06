using LiteDB;
using Rocket.API;
using Steamworks;
using System.Linq;
using System.Text;
using uDB;
using UnityEngine;

namespace MoreHomes
{
    public static class Database
    {
        public static bool AddBed(string bedName, CSteamID steamId, Vector3 position)
        {
            using (LiteDatabase liteDb = DbFinder.GetLiteDb("BedsDatabase.db", null))
            {

                LiteCollection<TBedData> BedsData = liteDb.GetCollection<TBedData>("BedsData");

                int maxLimit = MoreHomes.Instance.Configuration.Instance.DefaultHomes;

                RocketPlayer player = new RocketPlayer(steamId.m_SteamID.ToString());
                
                foreach (var item in MoreHomes.Instance.Configuration.Instance.Permissions)
                {
                    if (player.HasPermission(item.SPermission))
                    {
                        maxLimit = item.MaxHomes;
                    }
                }

                var results = BedsData.Find(x => x.SteamId == steamId.m_SteamID);

                if (results.Count() >= maxLimit)
                {
                    return false;
                }

                TBedData bedData = new TBedData(steamId.m_SteamID, bedName.ToLower(), position.ToString());

                BedsData.Insert(bedData);
            }
            return true;
        }

        public static string GetBed(CSteamID steamId, string bedName)
        {
            using (LiteDatabase liteDb = DbFinder.GetLiteDb("BedsDatabase.db", null))
            {
                LiteCollection<TBedData> BedsData = liteDb.GetCollection<TBedData>("BedsData");

                if (!BedsData.Exists(x=> x.BedName == bedName.ToLower() && x.SteamId == steamId.m_SteamID))
                    return "null";

                TBedData bed = BedsData.FindOne(x => (x.SteamId == steamId.m_SteamID) && (x.BedName == bedName.ToLower()));
                return bed.BedPosition;
            }
        }

        public static void RemoveBed(string Id)
        {
            using (LiteDatabase liteDb = DbFinder.GetLiteDb("BedsDatabase.db", null))
            {
                LiteCollection<TBedData> BedsData = liteDb.GetCollection<TBedData>("BedsData");
                BedsData.Delete(Id);
            }
        }   

        public static string GetAllBeds(CSteamID steamId)
        {
            using (LiteDatabase liteDb = DbFinder.GetLiteDb("BedsDatabase.db", null))
            {
                LiteCollection<TBedData> BedsData = liteDb.GetCollection<TBedData>("BedsData");

                var beds = BedsData.Find(x => x.SteamId == steamId.m_SteamID);

                if (beds.Count() == 0)
                    return "You don't have any bed to teleport.";

                StringBuilder result = new StringBuilder(MoreHomes.Instance.Translate("command_homes"));

                foreach (TBedData bed in beds)
                {
                    result.Append($" {bed.BedName},");
                }

                string returnValue = result.ToString();
                char[] charsToTrim = { ',', ' ' };
                return returnValue.TrimEnd(charsToTrim);

            }
        }

        public static bool RenameBed(CSteamID steamId, string oldName, string newName)
        {
            using (LiteDatabase liteDb = DbFinder.GetLiteDb("BedsDatabase.db", null))
            {
                LiteCollection<TBedData> BedsData = liteDb.GetCollection<TBedData>("BedsData");

                if (!BedsData.Exists(x => x.BedName == oldName.ToLower() && x.SteamId == steamId.m_SteamID))
                    return false;

                TBedData bed = BedsData.FindOne(x => (x.SteamId == steamId.m_SteamID) && (x.BedName == oldName.ToLower()));

                bed.BedName = newName.ToLower();
                BedsData.Update(bed);
            }
            return true;
        }

        public class TBedData
        {
            public TBedData(ulong steamId, string bedName, string bedPosition)
            {
                this.SteamId = steamId;
                this.BedName = bedName;
                this.BedPosition = bedPosition;

            }

            public TBedData()
            {
            }

            [BsonId]
            public string Id
            {
                get
                {
                    return this.BedPosition;
                }
            }

            public ulong SteamId { get; set; }

            public string BedName { get; set; }
            public string BedPosition { get; set; }

        }
    }



}
