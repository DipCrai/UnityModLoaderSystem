using System.IO;
using System.Linq;
using UnityEngine;

public static class Paths
{
    public static readonly string DataPath = Application.dataPath;
    public static readonly string GamePath = Path.GetDirectoryName(DataPath);
    public static readonly string ModsPath = Combine(GamePath, "Mods");
    public static readonly string CompilationsPath = Combine(GamePath, "ModCompilations");

    public static string Combine(params string[] paths) => new FileInfo(paths.Aggregate("", Path.Combine)).FullName;
}