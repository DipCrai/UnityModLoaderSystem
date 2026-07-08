using System.Collections.Generic;
using System.Reflection;

namespace ModSystem
{
    internal static class ModLoader
    {
        private static List<ModConstants> _disabledMods = new List<ModConstants>();
        private static List<ModConstants> _loadedMods = new List<ModConstants>();

        public static void LoadMods(List<ModConstants> mods)
        {
            if (mods == null)
                return;

            foreach (ModConstants mod in mods)
            {
                if (mod == null || _loadedMods.Contains(mod))
                    continue;

                if (!mod.Enabled)
                    _disabledMods.Add(mod);

                if (string.IsNullOrEmpty(mod.DllPath))
                    continue;

                ModAPI.Utils.ModConstants = mod;
                ModAPI.Utils.IsModConstantsValid = true;

                MethodInfo method = Assembly.LoadFile(mod.DllPath).GetType(mod.EntryPoint).GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                method.Invoke(null, null);

                ModAPI.Utils.IsModConstantsValid = false;
                _loadedMods.Add(mod);
            }
        }

        public static void ToggleMod(ModConstants mod)
        {
            if (mod.Enabled)
            {
                mod.Enabled = false;
            }
            else
            {
                mod.Enabled = true;
                _disabledMods.Remove(mod);
                LoadMods(mod.ToList());
            }
            Utils.UpdateModJson(mod);
        }
    }
}