
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

namespace CozmicVoid.Content.Items.Weapons.Ice
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class AurorasKnife : ModItem
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shootSpeed = 12;
			Item.shoot = ModContent.ProjectileType<AurorasKniferProj>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }











    public class AurorasKniferProj : ModProjectile, IPixelatedPrimitiveRenderer
    {

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
            Projectile.width = 18;
            Projectile.height = 46;
            Projectile.penetrate += 1;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
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

            PrimitiveSettings settings = new PrimitiveSettings(IceTailWidthFunction, IceTailColorFunction, _ => Projectile.Size * 0.5f + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * Projectile.scale * 0f, Shader: iceTailShader, Pixelate: true);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 8);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f + 3.14f;
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            var entitySource = Projectile.GetSource_FromThis();
            for (int j = 0; j < 12; j++)
            {
                int a = Gore.NewGore(entitySource, new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), Projectile.velocity, 911);
                Main.gore[a].timeLeft = 20;
                Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
            }
            return base.OnTileCollide(oldVelocity);
        }


        public override void PostDraw(Color lightColor)
        {
            Color color = Color.White * 0.5f;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Ice/AurorasKniferProj");
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
        }

        public int Time
        {
            get;
            set;
        }


        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

    }
}
