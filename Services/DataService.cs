using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Storage;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.MoreHomes.Services
{
    public class DataService : MonoBehaviour
    {
        private MoreHomesPlugin pluginInstance => MoreHomesPlugin.Instance;

        public DataStorage<List<PlayerData>> PlayersDataStorage { get; set; }
        public List<PlayerData> PlayersData { get; set; }

        void Awake()
        {
            PlayersDataStorage = new DataStorage<List<PlayerData>>(pluginInstance.Directory, "MoreHomesData.json");

            SaveManager.onPostSave += SaveData;
        }

        void Start()
        {
            if (Level.isLoaded)
                ReloadData();
            else
                Level.onLevelLoaded += (i) => ReloadData();
        }

        void OnDestroy()
        {
            Level.onLevelLoaded -= (i) => ReloadData();
            SaveManager.onPostSave -= SaveData;
            SaveData();
        }

        public void ReloadData()
        {
            PlayersData = PlayersDataStorage.Read();
            if (PlayersData == null)
                PlayersData = new List<PlayerData>();

            List<InteractableBed> beds = new List<InteractableBed>();

            foreach (var region in BarricadeManager.regions)
            {
                foreach (var drop in region.drops)
                {
                    if (drop.interactable as InteractableBed != null)
                        beds.Add(drop.interactable as InteractableBed);
                }
            }

            foreach (var player in PlayersData)
            {
                foreach (var home in player.Homes)
                {
                    foreach (var bed in beds)
                    {
                        if (bed.transform.position.x == home.Position.X && bed.transform.position.y == home.Position.Y 
                            && bed.transform.position.z == home.Position.Z)
                        {
                            home.Transform = bed.transform;
                            beds.Remove(bed);
                            break;
                        }
                    }
                }
            }
        }

        public void SaveData()
        {
            foreach (var player in PlayersData)
            {
                foreach (var home in player.Homes)
                {
                    home.Position = new ConvertablePosition(home.LivePosition);
                }
            }
            PlayersDataStorage.Save(PlayersData);
            Logger.Log($"{PlayersData.Count} player homes data has been saved!");
        }
    }
}
