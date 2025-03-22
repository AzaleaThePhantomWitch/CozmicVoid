
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

namespace CozmicVoid.Content.Items.Weapons.Ice
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class SnowGlow : ModItem
    {


        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CozmicVoid.hjson' file.
        public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shootSpeed = 20;
			Item.shoot = ModContent.ProjectileType<SnowGlowProj>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}


    }








    


    public class SnowGlowProj : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public int We;
        public float RandVar;
        public float Timer;
        public Vector2 Pos;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boralius");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.penetrate = 5;
            Projectile.friendly = true;
            Projectile.timeLeft = 150;
        }
        public override void OnKill(int timeLeft)
        {
            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 20f);
            for (int i = 0; i <= 18; i++)
            {
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/SnowGlowHit") with { PitchVariance = 0.7f });
            Projectile.velocity = -Projectile.velocity;
            target.AddBuff(BuffID.Poisoned, 180);
        }
        public float IceTailWidthFunction(float completionRatio)
        {
            float baseWidth = Projectile.width + 5f;
            float tipSmoothenFactor = MathF.Sqrt(1f - InverseLerp(0.45f, 0.03f, completionRatio).Cubed());
            return Projectile.scale * baseWidth * tipSmoothenFactor;
        }

        public Color IceTailColorFunction(float completionRatio)
        {
            Color color = Color.Lerp(new Color(51, 218, 219), new Color(151, 71, 255), InverseLerp(0.2f, 0.67f, completionRatio));

            return Projectile.GetAlpha(color * 0.6f);
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

            ManagedShader iceTailShader = ShaderManager.GetShader("CozmicVoid.IceTailShader");
            iceTailShader.TrySetParameter("localTime", Main.GlobalTimeWrappedHourly + Projectile.identity * 0.32f);
            iceTailShader.SetTexture(texture, 1, SamplerState.LinearWrap);
            iceTailShader.SetTexture(texture2, 2, SamplerState.LinearWrap);
            if (Timer <= 50)
            {
                PrimitiveSettings settings = new PrimitiveSettings(IceTailWidthFunction, IceTailColorFunction, _ => Projectile.Size * 0.5f + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.scale * 0f, Shader: iceTailShader, Pixelate: true);
                PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 8);
            }

        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
            if (Projectile.penetrate <= 4)
            {
                Timer++;
            }
   
            if(Timer == 50)
            {
                int StarType = Main.rand.Next(0, 2);
                if (StarType == 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/SnowGlowCharge1"));
                }
                else
                {
                    SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/SnowGlowCharge2"));
                }
                Pos = Projectile.position;
            }
            if (Timer >= 50)
            {
                Projectile.scale += 0.02f;
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
                RandVar += 0.1f;
                Projectile.position.Y = Main.rand.NextFloat(Pos.Y - RandVar, Pos.Y + RandVar);
                Projectile.position.X = Main.rand.NextFloat(Pos.X - RandVar, Pos.X + RandVar);
            }
            if (Timer >= 90)
            {
                int StarType = Main.rand.Next(0, 2);
                if (StarType == 0)
                {
                    SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/SnowGlowBomb1"));
                }
                else
                {
                    SoundEngine.PlaySound(new SoundStyle("CozmicVoid/Assets/Sounds/Item/SnowGlowBomb2"));
                }
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ExampleCircleExplosionProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                Projectile.timeLeft = 1;
            }

            Projectile.velocity *= 0.93f;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }


        public override void PostDraw(Color lightColor)
        {
            DrawHelper.DrawDimLight(Projectile, Color.BlueViolet, Color.BlueViolet, 1);
            Color color = Color.White * 0.5f;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Ice/SnowGlowProj");
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);




            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f), new Rectangle(0, 0, texture.Width, texture.Height), new Color(color.R, color.G, color.B, 0), Main.GlobalTimeWrappedHourly * (1 + RandVar / 100), texture.Size() * 0.5f, Projectile.scale * 1.1f, SpriteEffects.None, 0);
            DrawHelper.DrawItemShine2(Projectile, Color.LightBlue, Color.MediumPurple, 3, RandVar / 5, 1);

        }

        public int Time
        {
            get;
            set;
        }


        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

    }
}
