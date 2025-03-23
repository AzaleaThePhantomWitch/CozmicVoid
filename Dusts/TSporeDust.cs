using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CozmicVoid.Dusts
{
	public class TSporeDust : ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale *= Main.rand.NextFloat(0.8f, 2f);
            dust.frame = new Rectangle(0, Main.rand.Next(2) * 32, 32, 32);
            dust.rotation = Main.rand.NextFloat(6.28f);
        }

        public override bool Update(Dust dust)
        {

            dust.velocity.X *= 0.95f;
            dust.velocity.Y = -0.97f;


            if (dust.alpha > 100)
            {
                dust.scale *= 0.975f;
                dust.alpha += 2;
            }
            else
            {
                Lighting.AddLight(dust.position, dust.color.ToVector3() * 0.1f);
                dust.scale *= 0.985f;
                dust.alpha += 4;
            }

            dust.position += dust.velocity;
            dust.rotation += 0.01f;

            if (dust.alpha >= 255)
                dust.active = false;

            return false;
        }
    }
}
