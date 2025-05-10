using CozmicVoid.Assets;
using CozmicVoid.Assets.Impacts;
using CozmicVoid.Assets.Particles;
using CozmicVoid.ExampleContent;
using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Players;
using CozmicVoid.Systems.Shaders;
using Luminance.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Weapons.Forest
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class ExtremelySturdyStick : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.CozmicVoid.hjson' file.
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.useTime = 126;
            Item.useAnimation = 126;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 10;
            Item.shoot = ModContent.ProjectileType<ExtremelySturdyStickSlash>();
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 60;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);

            comboPlayer.IncreaseCombo(maxCombo: 2);
            return false;
        }
    }

    public class ExtremelySturdyStickSlash : BaseSwingProjectile, IPixelatedPrimitiveRenderer
    {
        public override string Texture => "CozmicVoid/Content/Items/Weapons/Forest/ExtremelySturdyStick";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;
        public int HitsAc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 200;
            ProjectileID.Sets.TrailingMode[Type] = 1;

        }

        public override void SetDefaults()
        {
            HitsAc = 3;
            ProjectileID.Sets.TrailCacheLength[Type] = 200;
            holdOffset = 60;
            trailStartOffset = 0.2f;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 12;
            Projectile.width = 12;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 50;
        }

        protected override float SwingTimeFunction()
        {
            switch (ComboAtt)
            {
                default:
                case 0:
                    return 15;
                case 1:
                    return 15;
            }
        }

        protected override void ModifySimpleSwingAI(float targetRotation, float lerpValue,
            ref float startSwingRot,
            ref float endSwingRot,
            ref float swingProgress)
        {
            switch (ComboAtt)
            {
                default:
                case 3:
                    Hit = false;
                    float circleRange = MathHelper.PiOver4 + MathHelper.PiOver4 + MathHelper.TwoPi;
                    startSwingRot = targetRotation - circleRange;
                    endSwingRot = targetRotation + circleRange;
                    swingProgress = Easing.OutCirc(lerpValue);
                    break;
            }
        }

        protected override void ModifyOvalSwingAI(float targetRotation, float lerpValue,
            ref float swingXRadius,
            ref float swingYRadius,
            ref float swingRange,
            ref float swingProgress)
        {

            switch (ComboAtt)
            {
                case 0:
                    swingXRadius = 178 / 3f;
                    swingYRadius = 34 / 3f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.OutCirc(lerpValue);

                    break;
                case 1:
                    swingXRadius = 178 / 3f;
                    swingYRadius = 34 / 3f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.OutCirc(lerpValue);
                    break;
            }
        }

        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            switch (ComboAtt)
            {
                case 5:
                    Projectile.localNPCHitCooldown = 2 * ExtraUpdateMult;
                    break;
            }
        }

        protected override void SwingAI()
        {

            switch (ComboAtt)
            {
                case 0:
                    OvalEasedSwingAI();
                    break;

                case 1:
                    OvalEasedSwingAI();
                    break;

                case 2:
                    SimpleEasedSwingAI();
                    break;
            }
        }
        int Trys = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_Death(), target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<SmallImpact>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, 0f, 0f);
            base.OnHitNPC(target, hit, damageDone);
            if (Trys <= HitsAc)
            {
                if (!Hit)
                {
                    Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                    Trys++;
                    Hit = true;
                    hitstopTimer = 2 * ExtraUpdateMult;
                }
            }

        }

        //TRAIL VISUALS
        protected override Vector2 GetFramingSize()
        {
            //Set this to the width and height of the sword sprite
            return new Vector2(68, 72);
        }

        protected override Vector2 GetTrailOffset()
        {
            //Moves the trail along the blade, negative goes towards the player, positive goes away the player
            return Vector2.One * 72;
        }

        public float BloodWidthFunction(float completionRatio)
        {
            return MathHelper.Lerp(38f, 5f, completionRatio) * Projectile.Opacity;
        }

        public Color BloodColorFunction(float completionRatio)
        {
            // Make the trail fade out at the end and fade in sharply at the start, to prevent the trail having a definitive, flat "start".
            float trailOpacity = InverseLerpBump(0f, 0.067f, 0.27f, 0.75f, completionRatio) * 0.3f;

            // Interpolate between a bunch of colors based on the completion ratio.
            Color startingColor = Color.Lerp(Color.White, Color.SandyBrown, 0.25f);
            Color middleColor = Color.Lerp(Color.SandyBrown, Color.Gray, 0.35f);
            Color endColor = Color.Lerp(Color.Gray, Color.Black, 0.35f);

            Palette palette = new Palette(startingColor, middleColor, endColor);
            Color color = palette.SampleColor(completionRatio) * trailOpacity;

            color.A = (byte)(trailOpacity * 255);
            return color * Projectile.Opacity;
        }
        public int Time
        {
            get;
            set;
        }    



        public void RenderPixelatedPrimitives(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Assets/Textures/TrailStreaks/StreakMagma");


            Projectile.localAI[0]++;


            Player player = Main.player[Projectile.owner];
            ManagedShader shader = ShaderManager.GetShader("CozmicVoid.FlameTrail");
            shader.SetTexture(texture, 1, SamplerState.LinearWrap);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            PrimitiveSettings settings = new PrimitiveSettings(BloodWidthFunction, BloodColorFunction, _ => Projectile.Size * 0.5f, Pixelate: true, Shader: shader);
            PrimitiveRenderer.RenderTrail(Projectile.oldPos, settings, 7);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}