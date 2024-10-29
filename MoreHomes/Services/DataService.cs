using RestoreMonarchy.MoreHomes.Helpers;
using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Storage;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            List<InteractableBed> interactableBeds = [];

            foreach (BarricadeRegion region in BarricadeManager.regions)
            {
                foreach (BarricadeDrop drop in region.drops)
                {
                    if (drop.interactable is InteractableBed interactableBed)
                    {
                        interactableBeds.Add(interactableBed);
                    }                        
                }
            }

            foreach (VehicleBarricadeRegion region in BarricadeManager.vehicleRegions)
            {
                foreach (BarricadeDrop drop in region.drops)
                {
                    if (drop.interactable is InteractableBed interactableBed)
                    {
                        interactableBeds.Add(interactableBed);
                    }
                }
            }

            foreach (InteractableBed interactableBed in interactableBeds)
            {
                if (interactableBed.owner == CSteamID.Nil)
                {
                    continue;
                }                    

                PlayerHome home = HomesHelper.GetPlayerHome(interactableBed.owner, interactableBed.transform.position);
                if (home != null)
                {
                    home.InteractableBed = interactableBed;
                } else
                {
                    PlayerData player = HomesHelper.GetOrCreatePlayer(interactableBed.owner);
                    home = new PlayerHome(player.GetUniqueHomeName(), interactableBed);
                    player.Homes.Add(home);
                }
            }

            foreach (PlayerData player in PlayersData.ToList())
            {
                foreach (PlayerHome home in player.Homes.ToList())
                {
                    if (home.InteractableBed == null)
                    {
                        player.Homes.Remove(home);
                    }
                }

                if (player.Homes.Count == 0)
                {
                    PlayersData.Remove(player);
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
                    {
                        continue;
                    }
                    home.Position = new ConvertablePosition(home.LivePosition);
                }
            }
            PlayersDataStorage.Save(PlayersData);
        }
    }
}
