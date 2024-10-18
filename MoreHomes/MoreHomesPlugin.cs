using HarmonyLib;
using RestoreMonarchy.MoreHomes.Components;
using RestoreMonarchy.MoreHomes.Services;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.MoreHomes
{
    public class MoreHomesPlugin : RocketPlugin<MoreHomesConfiguration>
    {        
        public static MoreHomesPlugin Instance { get; private set; }
        public IRocketPlugin TeleportationPlugin { get; private set; }

        public Dictionary<string, DateTime> PlayerCooldowns { get; set; }

        public DataService DataService { get; private set; }

        public MovementDetectorComponent MovementDetector { get; set; }

        public Color MessageColor { get; set; }

        public const string HarmonyInstanceId = "com.restoremonarchy.morehomes";
        private Harmony HarmonyInstance;

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            PlayerCooldowns = new Dictionary<string, DateTime>();
            
            HarmonyInstance = new Harmony(HarmonyInstanceId);
            HarmonyInstance.PatchAll(Assembly);

            DataService = gameObject.AddComponent<DataService>();

            MovementDetector = gameObject.AddComponent<MovementDetectorComponent>();

            R.Plugins.OnPluginsLoaded += OnPluginsLoaded;

            InvokeRepeating(nameof(RemoveExpiredCooldowns), 300, 300);

            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            HarmonyInstance?.UnpatchAll(HarmonyInstanceId);
            HarmonyInstance = null;

            Destroy(DataService);
            Destroy(MovementDetector);

            R.Plugins.OnPluginsLoaded -= OnPluginsLoaded;

            CancelInvoke(nameof(RemoveExpiredCooldowns));

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        private void RemoveExpiredCooldowns()
        {
            foreach (KeyValuePair<string, DateTime> cooldown in PlayerCooldowns.ToList())
            {
                if (cooldown.Value < DateTime.Now)
                {
                    PlayerCooldowns.Remove(cooldown.Key);
                }
            }
        }

        private void OnPluginsLoaded()
        {
            IRocketPlugin plugin = R.Plugins.GetPlugin("Teleportation");
            if (plugin != null)
            {   
                TeleportationPlugin = plugin;
            }
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "HomeCooldown", "Please wait [[b]]{0}[[/b]] seconds before using home again" },
            { "HomeDelayWarn", "You will be teleported to your home [[b]]{0}[[/b]] in seconds" },
            { "MaxHomesWarn", "You have reached the maximum number of beds" },
            { "BedDestroyed", "Home unavailable: bed destroyed or unclaimed. Teleportation canceled" },
            { "WhileDriving", "You can't teleport while driving" },
            { "NoHome", "No matching home found for teleportation" },
            { "HomeSuccess", "You were teleported to [[b]]{0}[[/b]] home" },
            { "HomeList", "Your homes [[b]][{0}/{1}][[/b]]: " },
            { "NoHomes", "You don't have any claimed beds" },
            { "DestroyHomeFormat", "Usage: /destroyhome [[name]]" },
            { "HomeNotFound", "No home found with the name [[b]]{0}[[/b]]" },
            { "DestroyHomeSuccess", "Home [[b]]{0}[[/b]] has been removed" },
            { "RenameHomeFormat", "Usage: /renamehome [[current name]] [[new name]]" },
            { "HomeAlreadyExists", "You already have home with the name [[b]]{0}[[/b]]" },
            { "RenameHomeSuccess", "Home renamed from [[b]]{0}[[/b]] to [[b]]{1}[[/b]]" },
            { "WhileRaid", "You can't teleport while in raiding mode" },
            { "WhileCombat", "You can't teleport while in combat mode" },
            { "RestoreHomesSuccess", "[[b]]{0}[[/b]] homes have been restored" },
            { "RemoveHome", "Your [[b]]{0}[[/b]] home has been removed" },
            { "HomeClaimed", "New home claimed with the name [[b]]{0}[[/b]]" },
            { "HomeTeleportationFailed", "Failed to teleport to [[b]]{0}[[/b]] home" },
            { "HomeDestroyed", "Your [[b]]{0}[[/b]] home was destroyed" },
            { "HomeCanceledYouMoved", "Home teleportation canceled because you moved" }
        };

        internal void SendMessageToPlayer(IRocketPlayer player, string translationKey, params object[] placeholder)
        {
            string msg = Translate(translationKey, placeholder);
            msg = msg.Replace("[[", "<").Replace("]]", ">");
            if (player is ConsolePlayer)
            {
                Logger.Log(msg);
                return;
            }

            UnturnedPlayer unturnedPlayer = (UnturnedPlayer)player;
            ChatManager.serverSendMessage(msg, MessageColor, null, unturnedPlayer.SteamPlayer(), EChatMode.SAY, Configuration.Instance.MessageIconUrl, true);
        }
    }
}
