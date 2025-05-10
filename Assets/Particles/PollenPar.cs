using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Luminance.Common.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;


namespace CozmicVoid.Assets.Particles
{
    public class PollenPar : ModProjectile
    {
        public float Sizee = 1;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boralius");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.alpha = 250;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.Next(0, 360);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }

        public override void AI()
        {

            Projectile.velocity.Y -= Main.rand.NextFloat(0.01f, 0.03f);
            Sizee = float.Lerp(Sizee, 0, 0.05f);
            Projectile.scale = Sizee;

        }



        public int Time
        {
            get;
            set;
        }

        public override void PostDraw(Color lightColor)
        {

            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Particles/SmokePar");
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), new Color(Color.Goldenrod.R / 3, Color.Goldenrod.G / 3, Color.Goldenrod.B / 3, 50), Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);


        }

    }
}