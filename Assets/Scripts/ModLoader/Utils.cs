using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ModSystem
{
    internal static class Utils
    {
        public static readonly string SignatureFileName = "mod.json";
        public static ModConstants DeserializeModJson(string modDirectory)
        {
            var modJsonPath = Paths.Combine(modDirectory, SignatureFileName);
            string jsonFile = File.ReadAllText(modJsonPath);
            return JsonConvert.DeserializeObject<ModConstants>(jsonFile);
        }
        public static void SerializeModJson(string modDirectory, ModConstants modConstants)
        {
            var modJsonPath = Paths.Combine(modDirectory, SignatureFileName);
            var jsonString = JsonConvert.SerializeObject(modConstants, Formatting.Indented);
            File.WriteAllText(modJsonPath, jsonString);
        }
        public static bool IsModValid(string modDirectory)
        {
            var fileName = Paths.Combine(modDirectory, SignatureFileName);

            if (!File.Exists(fileName))
                return false;

            return true;
        }
        public static void UpdateModJson(ModConstants mod)
        {
            var modJsonPath = Paths.Combine(mod.ModDirectory, SignatureFileName);
            File.WriteAllText(modJsonPath, JsonConvert.SerializeObject(mod, Formatting.Indented));
        }
        public static List<ModConstants> ToList(this ModConstants modConstants)
        {
            return new List<ModConstants>() { modConstants };
        }
    }
}