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
            return MathHelper.Lerp(500, 0, Easing.OutCirc(p));
        }

        protected override Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.LightGoldenrodYellow, p);
        }

        protected override float RadiusFunction(float p)
        {   
            return MathHelper.Lerp(4, 200, Easing.OutCirc(p));
        }

        
        protected override BaseShader ReadyShader()
        {
            var shader = SimpleTrailShader.Instance;

            //Main trailing texture
            shader.TrailingTexture = TrailRegistry.LazerTrail;

            //Blends with the main texture
            shader.SecondaryTrailingTexture = TrailRegistry.WhispTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            shader.TertiaryTrailingTexture = TrailRegistry.WhispTrail;
            shader.PrimaryColor = Color.OrangeRed;
            shader.SecondaryColor = Color.Peru;
            shader.Speed = 20;

            //Alpha Blend/Additive
            shader.BlendState = BlendState.Additive;
            shader.SamplerState = SamplerState.PointWrap;
            shader.FillShape = true;
            return shader;
        }
    }
}
