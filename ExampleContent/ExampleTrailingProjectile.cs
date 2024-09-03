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
            ProjectileID.Sets.TrailingMode[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;     
        }

        private Color ColorFunction(float p)
        {
            return Color.Lerp(Main.DiscoColor, Color.Transparent, p);
        }

        private Vector2 WidthFunction(float p)
        {
            return Vector2.One  *  128;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;
            simpleTrailShader.TrailingTexture = TrailRegistry.SpikyTrail;
            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                Projectile.oldPos, 
                Projectile.oldRot, 
                ColorFunction,
                WidthFunction, simpleTrailShader);
            return base.PreDraw(ref lightColor);
        }
    }
}
