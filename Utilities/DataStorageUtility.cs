using RestoreMonarchy.MoreHomes.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.MoreHomes.Utilities
{
    public static class DataStorageUtility
    {
        public static bool LoadPlayersData(this DataStorage storage, out List<PlayerData> data)
        {
            if (storage.ReadObject<List<PlayerData>>(out data))
            {
                if (data != null)
                {
                    data.InitializeBeds();
                } else
                {
                    data = new List<PlayerData>();
                }
            } else
            {
                return false;
            }
            return true;
        }

        public static void SavePlayersData(this DataStorage storage, List<PlayerData> data)
        {
            data.UpdateBeds();
            storage.SaveObject(data);
        }
    }
}
