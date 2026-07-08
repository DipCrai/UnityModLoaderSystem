using UnityEngine;
using ModSystem;

internal class MainMenuLoader : MonoBehaviour
{
    void Start()
    {
        ModCompiler.CompileMods();
        ModLoader.LoadMods(ModCompiler.CompiledMods);
    }
}
