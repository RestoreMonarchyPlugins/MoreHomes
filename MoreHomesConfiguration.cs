using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreHomes
{
    public class MoreHomesConfiguration : IRocketPluginConfiguration
    {
        public int TeleportationDelay { get; set; }
        public int DefaultHomes { get; set; }
        public List<Permission> Permissions  { get; set; }

        public void LoadDefaults()
        {
            TeleportationDelay = 5;
            DefaultHomes = 2;
            Permissions = new List<Permission>()
            {
                new Permission() { MaxHomes = 3, SPermission = "home.vip" },
                new Permission() { MaxHomes = 5, SPermission = "home.lord" }
            };
        }
    }

    public class Permission
    {
        public int MaxHomes { get; set; }
        public string SPermission { get; set; }
    }
}
