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
        public static MoreHomes Instance;

        protected override void Load()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("pw.cirno.extraconcentratedjuice");
            harmony.PatchAll(Assembly);

            Instance = this;

            Logger.LogWarning("MoreHomes by MCrow");
            Logger.LogWarning("Version 1.1 - BETA");
            Logger.LogWarning("If you didn't get this plugin from MCrow himself, please report it to admin@restoremonarchy.com");
            Logger.LogWarning("Remmember to always save the world before restart!");
            Logger.LogWarning("Made using Harmony and LiteDB");

        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"home_max_warn","You can't have more beds!"},
                    {"command_homes","Your beds:"},
                    {"command_home_not_found","Can't find any bed called {0}."},
                    {"command_home_delay","You will be teleported to your bed in {0} seconds!"},
                    {"command_rename_not_found","Couldn't rename bed {0}, because it doesn't exits."},
                    {"command_rename_format","Format: /renamehome <oldName> <newName>"},
                    {"command_rename_success","Successfully renamed {0} to {1}!"}
                };
            }
        }

        protected override void Unload()
        {
            Console.WriteLine("Unloading MoreHomes...");
        }

    }
}
