
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
using CozmicVoid.Assets;
using Terraria.Graphics.Renderers;
using CozmicVoid.Assets.Particles;

namespace CozmicVoid.Content.Items.Weapons.Desert
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
    public class TheBrambler : ModItem
    {


        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CozmicVoid.hjson' file.
        public override void SetDefaults()
        {
            Item.mana = 8;
            Item.staff[Item.type] = true;
            Item.damage = 8;
            Item.DamageType = DamageClass.Magic;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 12;
            Item.shoot = ModContent.ProjectileType<TheBramblerProj>();
        }

    }











    public class TheBramblerProj : ModProjectile, IPixelatedPrimitiveRenderer
    {
        public float Xvel;
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
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 14;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 40; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + Main.rand.Next(-70, 70), Projectile.Center.Y + Main.rand.Next(-70, 70), 0f, 0f, ModContent.ProjectileType<SmokePar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Color.SandyBrown.R, Color.SandyBrown.G, Color.SandyBrown.B);
            }
            Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 20f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }
        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Textures/TrailStreaks/StreakMagma");


            ManagedShader shader = ShaderManager.GetShader("CozmicVoid.StreakTrail");
            shader.SetTexture(texture, 1, SamplerState.LinearWrap);

            PrimitiveSettings settings = new PrimitiveSettings(FlameTrailWidthFunction, FlameTrailColorFunction, _ => Projectile.Size * 0.3f, Pixelate: true, Shader: shader);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 7);
        }
        public override string Texture => MiscTexturesRegistry.InvisiblePixelPath;


        public float FlameTrailWidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(40f, 5f, completionRatio) * Projectile.Opacity;
        }

        public Color FlameTrailColorFunction(float completionRatio)
        {
            // Make the trail fade out at the end and fade in sharply at the start, to prevent the trail having a definitive, flat "start".
            float trailOpacity = InverseLerpBump(0f, 0.067f, 0.27f, 0.75f, completionRatio) * 0.9f;

            // Interpolate between a bunch of colors based on the completion ratio.
            Color startingColor = Color.Lerp(Color.Gray, Color.SandyBrown, 0.25f);
            Color middleColor = Color.Lerp(Color.SandyBrown, Color.Gray, 0.35f);
            Color endColor = Color.Lerp(Color.Gray, Color.Black, 0.35f);

            Palette palette = new Palette(startingColor, middleColor, endColor);
            Color color = palette.SampleColor(completionRatio) * trailOpacity;

            color.A = (byte)(trailOpacity * 255);
            return color * Projectile.Opacity;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Xvel = Projectile.velocity.X;
        }
        public override void AI()
        {
            if(Projectile.velocity.X >= 0)
            {
                Projectile.rotation += 0.1f;
            }
            else
            {
                Projectile.rotation -= 0.1f;
            }


            Projectile.velocity.X = Xvel;
            Projectile.scale = float.Lerp(Projectile.scale, 1, 0.1f);
  
            int StarType = Main.rand.Next(0, 4);
            if (StarType == 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X + Main.rand.Next(-20, 20), Projectile.Center.Y + Main.rand.Next(-20, 20), 0f, 0f, ModContent.ProjectileType<SmokePar>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Color.SandyBrown.R, Color.SandyBrown.G, Color.SandyBrown.B);
                }
            }

            if (Projectile.wet)
            {
                Projectile.Kill();
            }
        }


        public override bool OnTileCollide(Vector2 oldVelocity)
        {



            Projectile.scale = 1.4f;
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                Projectile.Kill();
            else
            {

                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile .velocity.Y = -oldVelocity.Y / 2;

            }

            return false;
        }


        public override void PostDraw(Color lightColor)
        {
            Color color = Color.White * 0.5f;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Content/Items/Weapons/Desert/TheBramblerProj");
            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f, Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.Draw(texture, new Vector2(Projectile.position.X - Main.screenPosition.X + Projectile.width * 0.5f + Main.rand.NextFloat(-RandVar, RandVar), Projectile.position.Y - Main.screenPosition.Y + Projectile.height - texture.Height * 0.5f + 2f + Main.rand.NextFloat(-RandVar, RandVar)),
    new Rectangle(0, 0, texture.Width, texture.Height), color, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);


        }

        public int Time
        {
            get;
            set;
        }



    }
}
