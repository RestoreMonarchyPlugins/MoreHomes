using HarmonyLib;
using RestoreMonarchy.MoreHomes.Models;
using RestoreMonarchy.MoreHomes.Utilities;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.MoreHomes
{
    public class MoreHomesPlugin : RocketPlugin<MoreHomesConfiguration>
    {        
        public static MoreHomesPlugin Instance { get; private set; }
        public IRocketPlugin TeleportationPlugin { get; private set; }
        public DataStorage DataStorage { get; set; }
        public Dictionary<string, DateTime> PlayerCooldowns { get; set; }
        public List<PlayerData> DataCache { get; set; }
        public Color MessageColor { get; set; }

        public const string HarmonyInstanceId = "com.restoremonarchy.morehomes";
        private Harmony HarmonyInstance;

        protected override void Load()
        {            
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            DataStorage = new DataStorage(Directory, "MoreHomesData.json");
            PlayerCooldowns = new Dictionary<string, DateTime>();
            HarmonyInstance = new Harmony(HarmonyInstanceId);
            HarmonyInstance.PatchAll(Assembly);

            R.Plugins.OnPluginsLoaded += OnPluginsLoaded;

            if (!Level.isLoaded)
            {
                Level.onLevelLoaded += (level) => LoadData();
            } else
            {
                LoadData();
            }

            SaveManager.onPostSave += () => DataStorage.SavePlayersData(DataCache);

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Brought to you by RestoreMonarchy.com", ConsoleColor.Yellow);
        }

        private void OnPluginsLoaded()
        {
            IRocketPlugin plugin = R.Plugins.GetPlugin("Teleportation");
            if (plugin != null)
            {   
                TeleportationPlugin = plugin;
            }
        }

        private void LoadData()
        {
            Logger.Log($"Loading Players data...", ConsoleColor.Yellow);
            if (DataStorage.LoadPlayersData(out List<PlayerData> data))
            {
                DataCache = data;
            } else
            {
                UnloadPlugin(PluginState.Cancelled);
                return;
            }
            Logger.Log($"Successfully loaded {DataCache.Count} Players!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new TranslationList(){
            { "HomeCooldownWarn", "You have to wait {0} to use this command again" },
            { "HomeDelayWarn", "You will be teleported to your bed in {0} seconds" },
            { "MaxHomesWarn", "You cannot have more beds" },
            { "NoBedsToTeleport", "You don't have any bed to teleport or name doesn't match any" },
            { "BedDestroyed", "Your bed got destroyed. Teleportation canceled" },
            { "WhileDriving", "You cannot teleport while driving" },
            { "HomeSuccess", "Successfully teleported You to your {0} bed!" },
            { "HomeList", "Your beds: " },
            { "NoHomes", "You don't have any bed claimed" },
            { "DestroyHomeFormat", "Format: /destroyhome <BedName>" },
            { "HomeNotFound", "No home match {0} name" },
            { "DestroyHomeSuccess", "Successfully destroyed and unclaimed your {0} home!" },
            { "RenameHomeFormat", "Format: /renamehome <HomeName> <NewName>" },
            { "HomeAlreadyExists", "You already have a home named {0}" },
            { "RenameHomeSuccess", "Successfully renamed home {0} to {1}!" },
            { "WhileRaid", "You can't teleport while in raiding" },
            { "WhileCombat", "You can't teleport while in combat" },
            { "RestoreHomesSuccess", "Successfully restored {0} homes!" }
        };

        protected override void Unload()
        {
            DataStorage.SavePlayersData(DataCache);

            HarmonyInstance?.UnpatchAll(HarmonyInstanceId);
            HarmonyInstance = null;

            R.Plugins.OnPluginsLoaded -= OnPluginsLoaded;

            Level.onLevelLoaded -= (level) => LoadData();

            SaveManager.onPostSave -= () => DataStorage.SavePlayersData(DataCache);

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }
    }
}
