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
using Terraria.Map;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Weapons.Forest
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class IvythornBoomerang : ModItem
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
			Item.shoot = ModContent.ProjectileType<IvythornBoomerangProj>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }











    public class IvythornBoomerangProj : ModProjectile
    {
        public int Timer2;
        public int TimerCD;
        public int Timer;
        public int BoomerangAI;
        public bool BoomerangOnHit;
        public Vector2 BoomerangVel;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Boralius");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {

            Projectile.aiStyle = -1;
            Projectile.width = 12;
            Projectile.height = 23;
            Projectile.penetrate += 23;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void OnKill(int timeLeft)
        {


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            BoomerangAI = 1;
            target.AddBuff(BuffID.Poisoned, 180);
        }

        public override void AI()
        {
            switch (BoomerangAI)
            {
                case 0:
                    float duration = 8700f;

                    if(Timer2 >= 70)
                    {
                        BoomerangAI = 2;
                    }

                    Timer2++;
                    TimerCD++;
                    float p = TimerCD / duration;
                    float ep = Easing.OutExpo(p);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, ep);

                    TimerCD++;
                    if (TimerCD >= 10)
                    {
                        BoomerangOnHit = false;
                        Projectile.friendly = true;
                    }
                    Projectile.rotation += 0.29f;
                    break;
                case 1:
                    if (!BoomerangOnHit)
                    {

                        Projectile.friendly = false;
                        var entitySource = Projectile.GetSource_FromThis();
                        for (int j = 0; j < 12; j++)
                        {
                            int a = Gore.NewGore(entitySource, new Vector2(Projectile.Center.X + Main.rand.Next(-10, 10), Projectile.Center.Y + Main.rand.Next(-10, 10)), Projectile.velocity, 911);
                            Main.gore[a].timeLeft = 20;
                            Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
                        }
                        Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Projectile.Center, 700f, 8f);
                        BoomerangOnHit = true;
                        BoomerangVel = Projectile.velocity;
                        Projectile.velocity = Vector2.Zero;
                    }
                    Timer++;
                    Projectile.velocity = Vector2.Zero;
                    if (Timer == 7)
                    {
                        TimerCD = 0;
                        Timer = 0;
                        BoomerangAI = 0;
                        Projectile.velocity = BoomerangVel;
                        BoomerangOnHit = false;
                    }
                    break;
                case 2:
                    float RotationSpeed2 = 0;
                    RotationSpeed2 += 0.02f;

                    Projectile.rotation += 0.10f + RotationSpeed2;
                    Projectile.velocity /= 0.94f;
                    Vector2 ownerCenter = Main.player[Projectile.owner].Center;
                    Vector2 directionToPlayer = (ownerCenter - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Vector2 velocityToPlayer = directionToPlayer * Projectile.velocity.Length();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, velocityToPlayer, 1.9f);
                    if(Projectile.Center == ownerCenter)
                    {
                        Projectile.timeLeft = 1;
                    }
                    break;
            }
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
        public override bool PreDraw(ref Color lightColor)
        {
            DrawHelper.DrawAdditiveAfterImage(Projectile, Color.Gray, Color.Transparent, ref lightColor);
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos2 = Projectile.position - Main.screenPosition + texture2.Size() / 2;
            Vector2 drawOrigin2 = texture2.Size() / 2;
            Main.EntitySpriteDraw(texture2, drawPos2, null, lightColor, Projectile.rotation, drawOrigin2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
