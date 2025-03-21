using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using static Humanizer.In;
using CozmicVoid.Dusts;
using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Shaders;
using Terraria.DataStructures;
using CozmicVoid.Systems.Players;

namespace CozmicVoid.Content.Items.Weapons.Night
{ 
    internal class SculptorStar : ModProjectile
    {
        float StarYVel;
        float TrailSize;
        float StarSpeed;
        int StarType;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spirt Flare");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 250;
            Projectile.tileCollide = true;
            Projectile.damage = 45;
            Projectile.aiStyle = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.scale = 0f;
            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 10f);
            StarType = Main.rand.Next(0, 2);
            for (int i = 0; i <= 10; i++)
            {
                if (StarType == 0)
                {
                    Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
                }
                else
                {
  
                    Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                }
            }

        }
        public override bool PreAI()
        {
            Projectile.spriteDirection = Projectile.direction;
            return true;
        }
        float alphaCounter;
        public override void AI()
        {

            alphaCounter += 0.04f;


            if (Projectile.scale < 1)
            {
                TrailSize += 2f;
                Projectile.scale += 0.04f;
            }
            StarYVel -= 0.01f;


            Projectile.velocity.X *= 1f;

            if (StarType == 0)
            {
                Projectile.velocity.Y += StarYVel;
                Lighting.AddLight(Projectile.Center, Color.LightPink.ToVector3() * 1.5f * Main.essScale);
                Projectile.rotation -= 0.04f - StarSpeed;
                StarSpeed -= 0.002f;
            }
            else
            {
                Projectile.velocity.Y -= StarYVel;
                Lighting.AddLight(Projectile.Center, Color.SkyBlue.ToVector3() * 1.5f * Main.essScale);
                Projectile.rotation += 0.04f - StarSpeed;
                StarSpeed -= 0.002f;
            }

            Projectile.localAI[0] += 1f;
            if (Main.rand.Next(5) == 0)
            {
                for (int i = 0; i <= 1; i++)
                {
                    Dust.NewDust(Projectile.Center, 18, 18, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(150, 80, 40), 0.3f);
                }
            }
            if(StarType == 0)
            {
                if (Main.rand.Next(5) == 0)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        Dust.NewDust(Projectile.Center, 18, 18, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
                    }
                }
            }
            else
            {
                if (Main.rand.Next(5) == 0)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        Dust.NewDust(Projectile.Center, 18, 18, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                    }
                }
            }


        }
        private Color ColorFunction(float p)
        {
            if (StarType == 0)
            {
                return Color.Lerp(Color.White, Color.Violet, Easing.OutExpo(p, 9));
            }
            else
            {
                return Color.Lerp(Color.White, Color.RoyalBlue, Easing.OutExpo(p, 9));
            }

        }

        private float WidthFunction(float p)
        {
            return MathHelper.Lerp(TrailSize, 0, Easing.OutExpo(p, 15));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SimpleTrailShader simpleTrailShader = SimpleTrailShader.Instance;
            simpleTrailShader.Speed = 40;
            //Main trailing texture
            simpleTrailShader.TrailingTexture = TrailRegistry.BeamTrail;

            //Blends with the main texture d
            simpleTrailShader.SecondaryTrailingTexture = TrailRegistry.BeamTrail;

            //Used for blending the trail colors
            //Set it to any noise texture
            simpleTrailShader.TertiaryTrailingTexture = TrailRegistry.LightningTrail;

            if (StarType == 0)
            {
                simpleTrailShader.PrimaryColor = Color.Peru;
                simpleTrailShader.SecondaryColor = Color.MediumPurple;
            }
            else
            {
                simpleTrailShader.PrimaryColor = Color.MediumPurple;
                simpleTrailShader.SecondaryColor = Color.RoyalBlue;
            }


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
        public override void Kill(int timeLeft)
        {
            float spread = 0.4f;

            Vector2 direction = Projectile.Center.RotatedByRandom(spread);
            for (int i = 0; i <= 10; i++)
            {
                SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.position);
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 15f);
                //Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<TSmokeDust>(), base.Projectile.velocity.X * 0.5f, base.Projectile.velocity.Y * 0.5f, 0, Color.Pink);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(150, 80, 40), 0.5f);
                if (StarType == 0)
                {
                    Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
                }
                else
                {
                    Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                }

            }
        }
    }
}