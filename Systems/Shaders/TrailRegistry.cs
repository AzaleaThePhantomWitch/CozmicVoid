using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CozmicVoid.Systems.Shaders
{
    internal static class TrailRegistry
    {
        public static string AssetDirectory => "CozmicVoid/Assets/Textures/";
        public static Asset<Texture2D> SpikyTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SpikyTrail");
    }
}
