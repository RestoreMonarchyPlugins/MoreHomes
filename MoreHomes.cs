using Harmony;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    
namespace MoreHomes
{
    public class MoreHomes : RocketPlugin<MoreHomesConfiguration>
    {
        internal static MoreHomes Instance;
        public const string HarmonyInstanceId = "com.restoremonarchy.morehomes";
        private HarmonyInstance HarmonyInstance;
        public MoreHomesDatabase Database;

        public MoreHomes()
        {
            this.Database = new MoreHomesDatabase(this);
        }


        protected override void Load()
        {
            HarmonyInstance = HarmonyInstance.Create(HarmonyInstanceId);
            HarmonyInstance.PatchAll(Assembly);

            Instance = this;

            Logger.Log("MoreHomes by MCrow");
            Logger.Log("Version 2.0 - Stable");
            Logger.Log("Made using Harmony and LiteDB");

        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"home_max_warn","You can't have more beds!"},
                    {"command_homes","Your beds:"},
                    {"command_home_not_found","Can't find any bed called {0}."},
                    {"command_home_destroyed","The bed you were trying to teleport got destroyed!"},
                    {"command_home_delay","You will be teleported to your bed in {0} seconds!"},
                    {"command_home_died","Teleportation canceled, because you died."},
                    {"no_home","You don't have any bed to teleport."},

                    {"command_rename_not_found","Couldn't rename bed {0}, because it doesn't exits."},
                    {"command_rename_format","Format: /renamehome <oldName> <newName>"},
                    {"command_rename_success","Successfully renamed {0} to {1}!"}
                };
            }
        }

        protected override void Unload()
        {
            HarmonyInstance?.UnpatchAll(HarmonyInstanceId);
            HarmonyInstance = null;
            Console.WriteLine("Unloading MoreHomes...");
        }

    }
}
