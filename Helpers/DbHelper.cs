using LiteDB;
using System;
using System.IO;

namespace MoreHomes.Helpers
{
    internal static class DbHelper
    {
        public static LiteDatabase GetLiteDb(string dbFileName, string subPath = null)
        {
            string path = Path.Combine(Rocket.Core.Environment.PluginsDirectory + "/MoreHomes");
            if (string.IsNullOrEmpty(dbFileName))
            {
                throw new ArgumentNullException(nameof(dbFileName));
            }

            if (subPath != null)
            {
                path = Path.Combine(path, subPath);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return new LiteDatabase(Path.Combine(path, dbFileName));
        }
    }
}
