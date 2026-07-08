namespace ModSystem
{
    internal sealed class ModConstants
    {
        public string ModName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string EntryPoint { get; set; }
        public bool Enabled { get; set; }
        public string ModDirectory { get; set; }
        public string DllPath { get; set; }
    }
}