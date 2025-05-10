
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
using CozmicVoid.Assets;
using CozmicVoid.Dusts;
using CozmicVoid.ExampleContent;
using CozmicVoid.Systems.Players;
using CozmicVoid.Assets.Impacts;
using CozmicVoid.Assets.Particles;

namespace CozmicVoid.Content.Items.Weapons.Jungle
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class Pollenflace : ModItem
    {

        public Vector2 OGPos;
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CozmicVoid.hjson' file.
        public override void SetDefaults()
        {
            Item.staff[Item.type] = true;
            Item.damage = 10;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item45;
            Item.autoReuse = true;
            Item.shootSpeed = 8;
            Item.mana = 6;
            Item.shoot = ModContent.ProjectileType<PollenflaceProj>();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-7, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 normal = velocity.SafeNormalize(Vector2.UnitY).RotatedBy(MathHelper.PiOver2 * velocity.X.NonZeroSign());
            position -= normal * Main.rand.NextFloat(4f, 19f);
            velocity = velocity.RotatedByRandom(0.05f);
        }

    }





    public class PollenflaceSpore : ModProjectile
    {
        public float Sizee =  12;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boralius");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 200;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = -1;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
      
        public override void AI()
        {

            Projectile.velocity.Y -= Main.rand.NextFloat(0.01f, 0.03f);
            Sizee -= Main.rand.NextFloat(0.1f, 0.2f);
            if (Sizee <= 0)
            {
                Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3());

        }



        public int Time
        {
            get;
            set;
        }

        public override void PostDraw(Color lightColor)
        {
  
            DrawHelper.DrawDimLight(Projectile, new Color(Color.Orange.R, Color.Orange.G, Color.Orange.B, 200), new Color(Color.Orange.R, Color.Orange.G, Color.Orange.B, 200), Sizee, 1);
        }


    }





    public class PollenflaceProj : ModProjectile, IPixelatedPrimitiveRenderer
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
            Projectile.height = 18;

            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {

            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 10f);
            for (int i = 0; i <= 18; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X + Main.rand.NextFloat(-50f, 50f), Projectile.Center.Y + Main.rand.NextFloat(-50f, 50f), 0f, 0f, ModContent.ProjectileType<PollenPar>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<TSporeDust>(), 0, 0, 100, Color.Gold, 0.5f);
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }
        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Textures/TrailStreaks/StreakMagma");


            ManagedShader shader = ShaderManager.GetShader("CozmicVoid.FlameTrail");
            shader.SetTexture(texture, 1, SamplerState.LinearWrap);

            PrimitiveSettings settings = new PrimitiveSettings(FlameTrailWidthFunction, FlameTrailColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 7);
        }
        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;


        public float FlameTrailWidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(38f, 5f, completionRatio) * Projectile.Opacity;
        }

        public Color FlameTrailColorFunction(float completionRatio)
        {
            // Make the trail fade out at the end and fade in sharply at the start, to prevent the trail having a definitive, flat "start".
            float trailOpacity = InverseLerpBump(0f, 0.067f, 0.27f, 0.75f, completionRatio) * 0.9f;

            // Interpolate between a bunch of colors based on the completion ratio.
            Color startingColor = Color.Lerp(Color.PaleGoldenrod, Color.Goldenrod, 0.25f);
            Color middleColor = Color.Lerp(Color.Red, Color.Green, 0.35f);
            Color endColor = Color.Lerp(Color.Green, Color.Black, 0.35f);

            Palette palette = new Palette(startingColor, middleColor, endColor);
            Color color = palette.SampleColor(completionRatio) * trailOpacity;

            color.A = (byte)(trailOpacity * 255);
            return color * Projectile.Opacity;
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
            DrawHelper.DrawDimLight(Projectile, Color.Gold, Color.Goldenrod, 5, 1);
            DrawHelper.DrawDimLight(Projectile, Color.Orange, Color.Orange, 6, 1);
        }

        public int Time
        {
            get;
            set;
        }




    }
}
