﻿using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CozmicVoid.ExampleContent
{
    internal abstract class BaseExplosionProjectile : ModProjectile
    {
        private Vector2[] _circlePos = new Vector2[32];
        private ref float _timer => ref Projectile.ai[0];
        private float _duration;
        private float _beamWidth;
        private Color _beamColor;
        public override string Texture => TrailRegistry.Empty;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            base.AI();
            _timer++;
            if(_timer == 1)
            {
                _duration = Projectile.timeLeft;
            }
 
            float progress = _timer / _duration;
            float r = RadiusFunction(progress);
            _beamWidth = BeamWidthFunction(progress);
            _beamColor = ColorFunction(progress);
            for (int f = 0; f < _circlePos.Length; f++)
            {
                float p = f / (float)_circlePos.Length;
                Vector2 circlePos = Projectile.Center + Vector2.UnitY.RotatedBy(p * MathHelper.TwoPi) * r;
                _circlePos[f] = circlePos;
            }
        }

        protected virtual Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.Black, p);
        }

        protected virtual float BeamWidthFunction(float p)
        {
            return _beamWidth;
        }

        private float WidthFunction(float p)
        {
            return _beamWidth;
        }
        private Color ColorFunctionReal(float p)
        {
            return _beamColor;
        }
        protected virtual float RadiusFunction(float p)
        {
            return MathHelper.Lerp(16, 64, Easing.OutExpo(p));
        }

        protected virtual BaseShader ReadyShader()
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;

            //Main trailing texture
            simpleTrailShader.TrailingTexture = TrailRegistry.StarTrail;

            //Blends with the main texture
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.StarTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            simpleTrailShader.TertiaryTrailingTexture = TrailRegistry.CrystalTrail;
            simpleTrailShader.PrimaryColor = Color.Red;
            simpleTrailShader.SecondaryColor = Color.Green;

            //Alpha Blend/Additive
            simpleTrailShader.BlendState = BlendState.Additive;
            simpleTrailShader.FillShape = true;
            return simpleTrailShader;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            BaseShader shader = ReadyShader();
            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                _circlePos,
                Projectile.oldRot,
                ColorFunctionReal,
                WidthFunction, shader, offset: Vector2.Zero);

            return base.PreDraw(ref lightColor);
        }
    }
}
