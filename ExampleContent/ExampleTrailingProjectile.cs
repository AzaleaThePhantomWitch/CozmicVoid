using CozmicVoid.Dusts;
using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CozmicVoid.ExampleContent
{
    internal class ExampleTrailingProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;     
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CrystalDust>(), (Vector2.One * Main.rand.Next(1, 3)).RotatedByRandom(19.0), 0, Color.DeepSkyBlue, 0.9f).noGravity = true;
            }
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<TSmokeDust>(), (Vector2.One * Main.rand.Next(1, 5)).RotatedByRandom(19.0), 0, Color.AliceBlue, 0.9f).noGravity = true;
            }

        }
        public override void AI()
        {
  
            base.AI();
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver4 / 48);
        }

        private Color ColorFunction(float p)
        {
            return Color.Lerp(Color.White, Color.DarkViolet, Easing.OutExpo(p, 7));
        }

        private float WidthFunction(float p)        
        {
            return MathHelper.Lerp(80, 0, Easing.OutExpo(p, 9));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.75f * Main.essScale);
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;
            simpleTrailShader.Speed = 40;
            //Main trailing texture
            simpleTrailShader.TrailingTexture = TrailRegistry.LazerTrail;

            //Blends with the main texture
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.GlowTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            simpleTrailShader.TertiaryTrailingTexture = TrailRegistry.GlowTrail;
            simpleTrailShader.PrimaryColor = Color.OrangeRed;
            simpleTrailShader.SecondaryColor = Color.Peru;

            //Alpha Blend/Additive
            simpleTrailShader.BlendState = BlendState.Additive;



            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                Projectile.oldPos,
                Projectile.oldRot,
                ColorFunction,
                WidthFunction, simpleTrailShader, offset: new Vector2(Projectile.width / 2, Projectile.height / 2));

            return base.PreDraw(ref lightColor);
        }
    }
}
