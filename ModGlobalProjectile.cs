using CozmicVoid.Content.Items.Weapons.Jungle;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CozmicVoid
{
    public class ModGlobalProjectile : GlobalProjectile
    {
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            if (projectile.type == ProjectileID.BeeHive && Main.rand.NextBool(3))
            {
                Item.NewItem(projectile.GetSource_FromThis(), projectile.Hitbox, ModContent.ItemType<BallOfBees>(), Main.rand.Next(12, 20));
            }
        }

    }
}
