using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Storage;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
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
            string unturnedDirectory = UnturnedPaths.RootDirectory.FullName;
            string mapLevelPath = Path.Combine(ServerSavedata.directory, Provider.serverID, "Level", Provider.map);

            string directory = unturnedDirectory + mapLevelPath;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = Path.Combine(directory, "MoreHomesData.json");
            if (!File.Exists(filePath))
            {
                string oldFilePath = Path.Combine(pluginInstance.Directory, "MoreHomesData.json");
                if (File.Exists(oldFilePath))
                {
                    File.Move(oldFilePath, filePath);
                    string migrationFilePath = Path.Combine(pluginInstance.Directory, "Migration.txt");

                    string[] lines =
                    [
                        "Old data file has been moved to the map level location!",
                        $"Old file path: {oldFilePath}",
                        $"New file path: {filePath}",
                        $"Timestamp: {DateTime.Now}"
                    ];

                    using StreamWriter writer = File.CreateText(migrationFilePath);
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                        Logger.Log("[Migration] " + line, ConsoleColor.Yellow);
                    }
                }
            }

            PlayersDataStorage = new DataStorage<List<PlayerData>>(directory, "MoreHomesData.json");
            SaveManager.onPostSave += SaveData;
        }

        void Start()
        {
            if (Level.isLoaded)
            {
                ReloadData();
            }                
            else
            {
                Level.onLevelLoaded += ReloadData;
            }                
        }

        void OnDestroy()
        {
            Level.onLevelLoaded -= ReloadData;
            SaveManager.onPostSave -= SaveData;
            SaveData();
        }

        public void ReloadData(int i = 0)
        {
            PlayersData = PlayersDataStorage.Read();
            if (PlayersData == null)
            {
                PlayersData = new List<PlayerData>();
            }                

            var interactableBeds = new List<InteractableBed>();

            foreach (var region in BarricadeManager.regions)
            {
                foreach (var drop in region.drops)
                {
                    if (drop.interactable as InteractableBed != null)
                        interactableBeds.Add(drop.interactable as InteractableBed);
                }
            }

            foreach (var player in PlayersData)
            {
                foreach (var home in player.Homes)
                {
                    foreach (var interactableBed in interactableBeds)
                    {
                        if (interactableBed.transform.position.x == home.Position.X && interactableBed.transform.position.y == home.Position.Y 
                            && interactableBed.transform.position.z == home.Position.Z)
                        {
                            home.InteractableBed = interactableBed;
                            interactableBeds.Remove(interactableBed);
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
                    if (home == null || home.InteractableBed == null)
                        continue;
                    home.Position = new ConvertablePosition(home.LivePosition);
                }
            }
            PlayersDataStorage.Save(PlayersData);
            Logger.Log($"{PlayersData.Count} player homes data has been saved!");
        }
    }
}
