using UnityEngine;
using ModSystem;

namespace ModAPI
{
    public static class Utils
    {
        internal static ModConstants ModConstants;
        internal static bool IsModConstantsValid;

        public static Texture2D LoadTexture() { return default; }
        public static Sprite LoadSprite() { return default; }
        public static AudioClip LoadAudio() { return default; }
    }
}