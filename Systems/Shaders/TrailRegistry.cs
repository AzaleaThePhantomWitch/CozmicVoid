﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace CozmicVoid.Systems.Shaders
{
    internal static class TrailRegistry
    {
        public static string AssetDirectory => "CozmicVoid/Assets/Textures/";
        public static Asset<Texture2D> SpikyTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SpikyTrail");
        public static Asset<Texture2D> StarTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}StarTrail");
        public static Asset<Texture2D> WhispTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}SmallWhispyTrail");
        public static Asset<Texture2D> WaterTrail =>
            ModContent.Request<Texture2D>($"{AssetDirectory}WaterTrail");
        public static Asset<Texture2D> VortexTrail =>
    ModContent.Request<Texture2D>($"{AssetDirectory}VortexTrail");
        public static Asset<Texture2D> LightningTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}LightningTrail");
        public static Asset<Texture2D> CausticTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}CausticTrail");
        public static Asset<Texture2D> CrystalTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}CrystalTrail");
        public static Asset<Texture2D> TerraTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}TerraTrail");
        public static Asset<Texture2D> BeamTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}BeamTrail");
        public static Asset<Texture2D> BulbTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}BulbTrail");
        public static Asset<Texture2D> WaveTrail =>
ModContent.Request<Texture2D>($"{AssetDirectory}WaveTrail");
    }
}
