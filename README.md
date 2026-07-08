# Sample Unity Mod Loader System

A sample Unity project demonstrating a runtime mod loader. It compiles C# source files using **Roslyn** (`Microsoft.CodeAnalysis.CSharp`) and loads them into the game.

## How it works

1. Drop `.cs` files into `Mods/YourModDirectory/`.
2. Add a `mod.json` with metadata (name, author, entry point, etc.).
3. When game starts, the system compiles all mods into DLLs and invokes their `Main()` methods if they were found.
4. Mods get a basic API surface (`ModAPI.Utils`) to interact with the game. Implement it yourself.

## Project structure

```
Assets/Scripts/
├── MainMenuLoader.cs        # Entry point — compiles & loads mods on Start
├── Paths.cs                  # Path resolution (Mods/, ModCompilations/)
├── ModLoader/
│   ├── ModLoader.cs          # Loads compiled DLLs
│   ├── ModCompiler.cs        # Roslyn compilation pipeline
│   ├── ModConstants.cs       # Mod metadata model
│   ├── Utils.cs              # JSON (de)serialization, validation
│   └── ModAPI/
│       └── Utils.cs          # Public API surface for mods
```

## Example mod

```json
// mod.json

{
  "ModName": "MyMod",
  "Author": "me",
  "Version": "1.0",
  "EntryPoint": "MyMod.ModMain"
}
```

```csharp
// mod.cs

using ModAPI;

namespace MyMod
{
    public static class ModMain
    {
        public static void Main()
        {
            // your mod logic here
        }
    }
}
```

## Notes

- Requires `Microsoft.CodeAnalysis.CSharp` NuGet package (included in `Packages/`)
- Mods are compiled to `ModCompilations/` and loaded via `Assembly.LoadFile`
- Unloading mods at runtime is **not supported** — Mono/.NET Framework doesn't allow unloading assemblies
