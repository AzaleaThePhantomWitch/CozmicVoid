using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace CozmicVoid.Systems.Shaders
{
    internal class SimpleTrailShader : IShader
    {
        private static SimpleTrailShader _instance;
        public SimpleTrailShader()
        {
            Effect = ShaderRegistry.SimpleTrailEffect.Shader;
            PrimaryColor = Color.White;
            SecondaryColor = Color.White;
            Speed = 5;
        }

        public static SimpleTrailShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SimpleTrailShader();
                return _instance;
            }
        }

        public Effect Effect { get; set; }
        public Asset<Texture2D> TrailingTexture { get; set; }
        public Asset<Texture2D> SecondaryTrailingTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public float Speed { get; set; }

        public void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector3());
            Effect.Parameters["secondaryColor"].SetValue(SecondaryColor.ToVector3());
            Effect.Parameters["trailTexture"].SetValue(TrailingTexture.Value);
            Effect.Parameters["secondaryTrailTexture"].SetValue(SecondaryTrailingTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}
