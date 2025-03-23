
using Luminance.Assets;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;


using Luminance.Common.DataStructures;
using System.Collections.Generic;
using static Terraria.GameContent.Animations.IL_Actions.Sprites;
using NoxusBoss.Core.Graphics.Automators;
using static System.Formats.Asn1.AsnWriter;
using CozmicVoid.Dusts;
using CozmicVoid.Systems.Players;
using CozmicVoid.ExampleContent;

namespace CozmicVoid.Content.Items.Weapons.Jungle
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class BallOfBees : ModItem
    {


        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CozmicVoid.hjson' file.
        public override void SetDefaults()
        {
            Item.consumable = true; 
            Item.maxStack = 999;
            Item.noUseGraphic = true;
            Item.damage = 10;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 12;
            Item.shoot = ModContent.ProjectileType<BallOfBeesProj>();
        }

    }











    public class BallOfBeesProj : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public float RandVar;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boralius");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {

            Projectile.DamageType = DamageClass.Throwing;
            Projectile.CloneDefaults(ProjectileID.Shuriken);
            AIType = ProjectileID.Shuriken;
            Projectile.width = 15;
            Projectile.height = 14;
            Projectile.penetrate = 4;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {
            int StarType = Main.rand.Next(0, 2);
            if (StarType == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/BallOfBeesExplode1"));
            }
            else
            {
                SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/BallOfBeesExplode2"));
            }
            for (int i = 0; i <= 18; i++)
            {
                Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 15f);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<TSporeDust>(), 0, 0, 50, Color.Gold);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, Color.Yellow, 0.5f);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, Color.Yellow, 0.5f);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, Color.Yellow, 0.5f);
            }
            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 20f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
            target.AddBuff(BuffID.Poisoned, 180);
        }
        public float BloodWidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.width * 1f;
            float smoothTipCutoff = MathHelper.Lerp(0f, 1f, InverseLerp(0.09f, 0.3f, completionRatio));
            return smoothTipCutoff * baseWidth;
        }

        public Color BloodColorFunction(float completionRatio)
        {
            Color color = Color.Lerp(Color.Goldenrod, Color.Green, InverseLerp(0.8f, 0.67f, completionRatio));

            return Projectile.GetAlpha(color);
        }

        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Noise/BubblyNoise");
            Texture2D texture2 = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Noise/DendriticNoiseZoomedOut");
            Rectangle viewBox = Projectile.Hitbox;
            Rectangle screenBox = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            viewBox.Inflate(540, 540);
            if (!viewBox.Intersects(screenBox))
                return;

            float lifetimeRatio = Time / 240f;
            float dissolveThreshold = InverseLerp(0.67f, 1f, lifetimeRatio) * 0.5f;
            ManagedShader bloodShader = ShaderManager.GetShader("CozmicVoid.BloodShader");
            bloodShader.TrySetParameter("localTime", Main.GlobalTimeWrappedHourly + Projectile.identity * 72.113f);
            bloodShader.TrySetParameter("dissolveThreshold", dissolveThreshold);
            bloodShader.TrySetParameter("accentColor", new Vector4(0.6f, 0.02f, -0.1f, 0f));
            bloodShader.SetTexture(texture, 1, SamplerState.LinearWrap);
            bloodShader.SetTexture(texture2, 2, SamplerState.LinearWrap);

            PrimitiveSettings settings = new PrimitiveSettings(BloodWidthFunction, BloodColorFunction, _ => Projectile.Size * 0.5f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.width * 0.56f, Pixelate: true, Shader: bloodShader);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 9);
        }

        public override void AI()
        {
            Projectile.scale = float.Lerp(Projectile.scale, 1, 0.1f);
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;

            if (Projectile.wet)
            {
                Projectile.Kill();
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            RandVar += 2;
            int StarType = Main.rand.Next(0, 2);
            if (StarType == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/BallOfBeesHit1"));
            }
            else
            {
                SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/BallOfBeesHit2"));
            }
            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 10f);
            Projectile.scale = 1.4f;
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;

                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;

            }
            for (int i = 0; i <= 18; i++)
            {
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<TSporeDust>(), 0,0, 100, Color.Gold, 0.5f);
            }
            return false;
        }


        public override void PostDraw(Color lightColor)
        {
            Color color = Color.White * 0.5f;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Jungle/BallOfBeesProj");
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + Main.rand.NextFloat(-RandVar, RandVar), Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f + Main.rand.NextFloat(-RandVar, RandVar)),
    new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            DrawHelper.DrawDimLight(Projectile, Color.Black, Color.Goldenrod, 7, 1);
        }

        public int Time
        {
            get;
            set;
        }


        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

    }
}
