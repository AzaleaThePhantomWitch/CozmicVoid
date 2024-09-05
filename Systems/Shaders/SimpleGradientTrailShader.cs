using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace CozmicVoid.Systems.Shaders
{
    internal class SimpleGradientTrailShader : BaseShader
    {
        private static SimpleGradientTrailShader _instance;
        public SimpleGradientTrailShader()
        {
            Effect = ShaderRegistry.GradientTrailEffect.Shader;
            PrimaryColor = Color.White;
            SecondaryColor = Color.White;
            Speed = 5;
        }

        public static SimpleGradientTrailShader Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SimpleGradientTrailShader();
                return _instance;
            }
        }

        public Asset<Texture2D> TrailingTexture { get; set; }
        public Asset<Texture2D> SecondaryTrailingTexture { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public float Speed { get; set; }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["primaryColor"].SetValue(PrimaryColor.ToVector4());
            Effect.Parameters["secondaryColor"].SetValue(SecondaryColor.ToVector4());
            Effect.Parameters["trailTexture"].SetValue(TrailingTexture.Value);
            Effect.Parameters["secondaryTrailTexture"].SetValue(SecondaryTrailingTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}
