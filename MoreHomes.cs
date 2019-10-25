using Harmony;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;

namespace MoreHomes
{
    public class MoreHomes : RocketPlugin<MoreHomesConfiguration>
    {
        internal static MoreHomes Instance { get; set; }
        public MoreHomesDatabase Database { get; set; }

        private HarmonyInstance HarmonyInstance;
        public const string HarmonyInstanceId = "com.restoremonarchy.morehomes";

        protected override void Load()
        {
            Instance = this;
            Database = new MoreHomesDatabase();

            HarmonyInstance = HarmonyInstance.Create(HarmonyInstanceId);
            HarmonyInstance.PatchAll(Assembly);

            Logger.Log($"{this.Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);

        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    { "home_max_warn","You can't have more beds!" },
                    { "command_homes","Your beds:" },
                    { "command_home_not_found","Can't find any bed called {0}." },
                    { "command_home_destroyed","The bed you were trying to teleport got destroyed!" },
                    { "command_home_delay","You will be teleported to your bed in {0} seconds!" },
                    { "command_home_died","Teleportation canceled, because you died." },
                    { "command_home_success", "You have successfully been teleported to your {0}!" },
                    { "no_home","You don't have any bed or you cannot be teleported to one, because it's blocked." },

                    { "command_rename_not_found","Couldn't rename bed {0}, because it doesn't exits." },
                    { "command_rename_format","Format: /renamehome <oldName> <newName>" },
                    { "command_rename_success","Successfully renamed {0} to {1}!" }
                };
            }
        }

        protected override void Unload()
        {
            HarmonyInstance?.UnpatchAll(HarmonyInstanceId);
            HarmonyInstance = null;
            Logger.Log($"{this.Name} has been unloaded!", ConsoleColor.Yellow);
        }
    }
}
