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

        public override void AI()
        {
            base.AI();
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 / 24);
        }
        private Color ColorFunction(float p)
        {
            return Color.Lerp(Color.DarkSlateBlue, Color.Transparent, Easing.OutExpo(p, 6));
        }

        private float WidthFunction(float p)
        {
            return MathHelper.Lerp(10, 0, Easing.OutExpo(p, 6));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;
            simpleTrailShader.TrailingTexture = TrailRegistry.CrystalTrail;
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.CrystalTrail;
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
