using CozmicVoid.ExampleContent;
using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Players;
using CozmicVoid.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class CrystallineSlasher : ModItem
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
            Item.shoot = ModContent.ProjectileType<CrystallineSwordSlash>();
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

            comboPlayer.IncreaseCombo(maxCombo: 3);
            return false;
        }
    }

    public class CrystallineSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "CozmicVoid/Content/Items/CrystallineSlasher";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;
        public int HitsAc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            ProjectileID.Sets.TrailingMode[Type] = 0;

        }

        public override void SetDefaults()
        {
            HitsAc = 3;
            ProjectileID.Sets.TrailCacheLength[Type] = 100;
            holdOffset = 60;
            trailStartOffset = 0.2f;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 38;
            Projectile.width = 38;
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
                    return 25;
                case 1:
                    return 25;
                case 2:
                    return 35;
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
                    swingXRadius = 178 / 1.5f;
                    swingYRadius = 34 / 1.5f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.OutCirc(lerpValue);

                    break;
                case 1:
                    swingXRadius = 178 / 1.5f;
                    swingYRadius = 34 / 1.5f;
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
 
            base.OnHitNPC(target, hit, damageDone);
            if(Trys <= HitsAc)
            {
                if (!Hit)
                {
                    Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(target.Center, 1024f, 8f);
                    Trys++;
                    Hit = true;
                    hitstopTimer = 8 * ExtraUpdateMult;
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

        protected override float WidthFunction(float p)
        {
            float trailWidth = MathHelper.Lerp(200, 200, p);
            float fadeWidth = MathHelper.Lerp(trailWidth, 5, _smoothedLerpValue) * Easing.OutExpo(_smoothedLerpValue, 4);
            return fadeWidth;
        }

        protected override Color ColorFunction(float p)
        {
            Color trailColor = Color.Lerp(Color.White, Color.Black, p);
            Color fadeColor = Color.Lerp(trailColor, Color.White, _smoothedLerpValue);
            //This will make it fade out near the end
            return fadeColor;
        }

        protected override BaseShader ReadyShader()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.75f * Main.essScale);
            SimpleTrailShader2 simpleTrailShader = SimpleTrailShader2.Instance;

            //Main trailing texture
            simpleTrailShader.TrailingTexture = TrailRegistry.GlowTrail;

            //Blends with the main texture

            //Alpha Blend/Additive
            simpleTrailShader.BlendState = BlendState.AlphaBlend;



            SpriteBatch spriteBatch = Main.spriteBatch;
            TrailDrawer.Draw(spriteBatch,
                Projectile.oldPos,
                Projectile.oldRot,
                ColorFunction,
                WidthFunction, simpleTrailShader, offset: new Vector2(Projectile.width / 2, Projectile.height / 2));

            return simpleTrailShader;
        }
    }
}