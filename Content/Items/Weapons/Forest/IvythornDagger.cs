
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

namespace CozmicVoid.Content.Items.Weapons.Forest
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class IvythornDagger : ModItem
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
			Item.shoot = ModContent.ProjectileType<IvythornDaggerProj>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }











    public class IvythornDaggerProj : ModProjectile, ShaderDraws
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
            Projectile.width = 12;
            Projectile.height = 23;
            Projectile.penetrate += 23;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }

        public static float MaxScale => 4.2f;
        public override void AI()
        {
            if (UnstableOverlayInterpolant <= 0.01f)
                Projectile.scale = MathF.Pow(InverseLerp(1f, 1, Time), 4.1f) * MaxScale;
            Projectile.ai[1] = 413;
            Projectile.ai[0] = 413;
        }
        public ref float UnstableOverlayInterpolant => ref Projectile.ai[1];

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
        public void RenderShineGlow(Vector2 drawPosition)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Noise/WavyBlotchNoise");
            ManagedShader shineShader = ShaderManager.GetShader("CozmicVoid.RadialShineShader");
            shineShader.Apply();

            Vector2 shineScale = Vector2.One * Projectile.width * Projectile.scale * 3.2f / texture.Size();
            Main.spriteBatch.Draw(texture, drawPosition, null, new Color(252, 242, 124) * 0.4f, Projectile.rotation, texture.Size() * 0.5f, shineScale, 0, 0f);
        }

        public void RenderSun(Vector2 drawPosition)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Noise/WavyBlotchNoise");
            Texture2D texture2 = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Noise/DendriticNoiseZoomedOut");
            Texture2D texture3 = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Textures/PsychedelicWingTextureOffsetMap");

            Color mainColor = Color.Lerp(new Color(204, 163, 79), new Color(100, 199, 255), Saturate(UnstableOverlayInterpolant).Cubed());
            Color darkerColor = Color.Lerp(new Color(204, 92, 25), new Color(255, 255, 255), Saturate(UnstableOverlayInterpolant).Cubed());

            var fireballShader = ShaderManager.GetShader("CozmicVoid.OrbShader");
            fireballShader.TrySetParameter("coronaIntensityFactor", UnstableOverlayInterpolant.Squared() * 1.92f + 0.044f);
            fireballShader.TrySetParameter("mainColor", mainColor);
            fireballShader.TrySetParameter("darkerColor", darkerColor);
            fireballShader.TrySetParameter("subtractiveAccentFactor", new Color(181, 0, 0));
            fireballShader.TrySetParameter("sphereSpinTime", Main.GlobalTimeWrappedHourly * 0.9f);
            fireballShader.SetTexture(texture, 1, SamplerState.LinearWrap);
            fireballShader.SetTexture(texture3, 2, SamplerState.LinearWrap);
            fireballShader.Apply();

            Vector2 scale = Vector2.One * Projectile.width * Projectile.scale * 1.5f / texture2.Size();
            Main.spriteBatch.Draw(texture2, drawPosition, null, Color.White with { A = 193 }, Projectile.rotation, texture2.Size() * 0.5f, scale, 0, 0f);
        }

        public void DrawWithShader(SpriteBatch spriteBatch)
        {


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            RenderShineGlow(drawPosition);
            RenderSun(drawPosition);


            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        public int Time
        {
            get;
            set;
        }


        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;

    }
}
