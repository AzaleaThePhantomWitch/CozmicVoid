using CozmicVoid.Content.NPCs.Night.StarDreamer;
using CozmicVoid.Dusts;
using CozmicVoid.ExampleContent;
using CozmicVoid.Systems.MathHelpers;
using CozmicVoid.Systems.Players;
using CozmicVoid.Systems.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Weapons.Night
{
    // This is a basic item template.
    // Please see tModLoader's ExampleMod for every other example:
    // https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod


    public class StarSculptor : ModItem
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
            Item.shoot = ModContent.ProjectileType<StarSculptorSwordSlash>();
            Item.autoReuse = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            ComboPlayer comboPlayer = player.GetModPlayer<ComboPlayer>();
            comboPlayer.ComboWaitTime = 80;

            int combo = comboPlayer.ComboCounter;
            int dir = comboPlayer.ComboDirection;
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback,
                player.whoAmI, combo, dir);

            comboPlayer.IncreaseCombo(maxCombo: 5);

            if (Main.netMode != NetmodeID.MultiplayerClient)
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<SculptorStar>(), damage, 1, Main.myPlayer, 0, 0);

            return false;
        }
    }

    public class StarSculptorSwordSlash : BaseSwingProjectile
    {
        public override string Texture => "CozmicVoid/Content/Items/Weapons/Night/StarSculptor";
        ref float ComboAtt => ref Projectile.ai[0];
        public bool Hit;
        public int HitsAc;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 0;

        }

        public override void SetDefaults()
        {
            HitsAc = 5;
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            holdOffset = 60;
            trailStartOffset = 0.2f;
            Projectile.timeLeft = SwingTime;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 50;
            Projectile.width = 56;
            Projectile.friendly = true;
            Projectile.scale = 1f;

            Projectile.extraUpdates = ExtraUpdateMult - 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 250;
        }


        protected override float SwingTimeFunction()
        {
            switch (ComboAtt)
            {
                default:
                case 0:
                    return 20;
                case 1:
                    return 20;
                case 2:
                    return 9;
                case 3:
                    return 9;
                case 4:
                    return 9;
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
                    swingXRadius = 178 / 2f;
                    swingYRadius = 34 / 2f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.OutSine(lerpValue);

                    break;
                case 1:
                    swingXRadius = 178 / 2f;
                    swingYRadius = 34 / 2f;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.OutSine(lerpValue);
                    break;

                case 2:
                    swingXRadius = 178 / 2;
                    swingYRadius = 136 / 2;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutSine(lerpValue);

                    break;
                case 3:
                    swingXRadius = 178 / 2;
                    swingYRadius = 136 / 2;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutSine(lerpValue);
                    break;

                case 4:
                    swingXRadius = 178 / 2;
                    swingYRadius = 136 / 2;
                    swingRange = MathHelper.Pi + MathHelper.PiOver2 + MathHelper.PiOver4;
                    swingProgress = Easing.InOutSine(lerpValue);
                    break;
            }
        }

        protected override void InitSwingAI()
        {
            base.InitSwingAI();
            switch (ComboAtt)
            {
                case 8:
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
                    OvalEasedSwingAI();
                    break;

                case 3:
                    OvalEasedSwingAI();
                    break;

                case 4:
                    OvalEasedSwingAI();
                    break;

            }
        }
        int Trys = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            base.OnHitNPC(target, hit, damageDone);
            if (Trys <= HitsAc)
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
        public override bool PreDraw(ref Color lightColor)
        {
            DrawHelper.DrawAdditiveAfterImage(Projectile, Color.Peru, Color.MediumPurple, ref lightColor);
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos2 = Projectile.position - Main.screenPosition + texture2.Size() / 2;
            Vector2 drawOrigin2 = texture2.Size() / 2;

            Main.EntitySpriteDraw(texture2, drawPos2, null, lightColor, Projectile.rotation, drawOrigin2, Projectile.scale, SpriteEffects.None, 0);

            if (Main.rand.Next(5) == 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Dust.NewDust(base.Projectile.Center, 1, 1, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(150, 80, 40), 0.3f);
                    //Dust.NewDust(base.Projectile.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                    Dust.NewDust(base.Projectile.Center, 1, 1, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.3f);
                }
            }

            return false;
        }
 
    }
}