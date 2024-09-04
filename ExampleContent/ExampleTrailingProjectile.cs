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
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
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
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 / 24);
        }
        private Color ColorFunction(float p)
        {
            return Color.Lerp(Color.DeepSkyBlue, Color.Transparent, Easing.OutExpo(p, 6));
        }

        private float WidthFunction(float p)
        {
            return MathHelper.Lerp(35, 0, Easing.OutExpo(p, 6));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.75f * Main.essScale);
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;
            simpleTrailShader.Speed = 20;
            simpleTrailShader.TrailingTexture = TrailRegistry.VortexTrail;
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.VortexTrail;
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
