using Newtonsoft.Json;
using Rocket.Core.Logging;
using System.IO;

namespace RestoreMonarchy.MoreHomes.Utilities
{
    public class DataStorage
    {
        public string DataPath { get; private set; }
        public DataStorage(string dir, string fileName)
        {
            DataPath = Path.Combine(dir, fileName);
        }

        public void SaveObject(object obj)
        {
            string objData = JsonConvert.SerializeObject(obj, Formatting.Indented);

            using (StreamWriter stream = new StreamWriter(DataPath, false))
            {
                stream.Write(objData);
            }
        }

        ///<summary>We want to pass exception to the caller, therefore T is out parameter and return is boolean. 
        ///When caller gets false then you may want to unload plugin.</summary>
        public bool ReadObject<T>(out T type)
        {
            type = default(T);
            if (File.Exists(DataPath))
            {
                using (StreamReader stream = File.OpenText(DataPath))
                {
                    string dataText = stream.ReadToEnd();
                    T obj = default(T);
                    try
                    {
                        obj = JsonConvert.DeserializeObject<T>(dataText);
                    }
                    catch (JsonException e)
                    {
                        Logger.LogError(e.Message);
                        return false;
                    }

                    type = obj;
                }
            }
            return true;
        }
    }
}
