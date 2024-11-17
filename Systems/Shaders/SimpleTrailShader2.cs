using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CozmicVoid.Systems.Shaders
{
    internal class SimpleTrailShader2 : BaseShader
    {
        private static SimpleTrailShader2 _instance;
        public SimpleTrailShader2()
        {
            Effect = ShaderRegistry.SimpleTrailEffect2.Shader;
            Speed = 5;
        }

        public static SimpleTrailShader2 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SimpleTrailShader2();
                return _instance;
            }
        }

        public Asset<Texture2D> TrailingTexture { get; set; }
        public float Speed { get; set; }

        public override void Apply()
        {
            Effect.Parameters["transformMatrix"].SetValue(TrailDrawer.WorldViewPoint2);
            Effect.Parameters["trailTexture"].SetValue(TrailingTexture.Value);
            Effect.Parameters["time"].SetValue(Main.GlobalTimeWrappedHourly * Speed);
        }
    }
}
