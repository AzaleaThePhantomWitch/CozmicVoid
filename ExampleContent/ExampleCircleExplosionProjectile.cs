using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CozmicVoid.ExampleContent
{
    internal class ExampleCircleExplosionProjectile : BaseExplosionProjectile
    {
        protected override float BeamWidthFunction(float p)
        {
            return MathHelper.Lerp(100, 0, Easing.OutCirc(p));
        }

        protected override Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.BurlyWood, p);
        }

        protected override float RadiusFunction(float p)
        {   
            return MathHelper.Lerp(4, 100, Easing.OutCirc(p));
        }

        
        protected override BaseShader ReadyShader()
        {
            var shader = SimpleTrailShader.Instance;

            //Main trailing texture
            shader.TrailingTexture = TrailRegistry.LazerTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TrailRegistry.WaveTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TrailRegistry.WhispTrail;
            shader.PrimaryColor = Color.MediumPurple;
            shader.SecondaryColor = Color.DarkTurquoise;
            shader.Speed = 20;

            //Alpha Blend/Additive
            shader.BlendState = BlendState.AlphaBlend;
            shader.SamplerState = SamplerState.PointWrap;
            shader.FillShape = true;
            return shader;
        }
    }
}
