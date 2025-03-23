using System;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using ReLogic.Content;

namespace CozmicVoid.Assets.Impacts
{
    public class BaseImpact : ModProjectile
    {
        float timer = 0;
        public float opacity = 1f;
        public Color color = Color.MediumPurple;
        public float size = 1f;
        public float speed = 0.2f;


        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("H3 Impact");
        }

        public override void SetDefaults()
        {
    
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;

        }

        public override bool? CanDamage()
        {
            return false;
        }

        float xScale = 0.3f;
        float yScale = 1f;
        public override void AI()
        {


            Player player = Main.player[Projectile.owner];



            if (timer > 1)
            {
                xScale = Math.Clamp(MathHelper.Lerp(xScale, 3, speed * 0.3f), 0, 10);
                yScale = Math.Clamp(MathHelper.Lerp(yScale, -0.2f, speed), 0.02f, 1);
            }


            if (yScale <= 0.02f)
                Projectile.active = false;

            Projectile.ai[0] = Math.Clamp(MathHelper.Lerp(Projectile.ai[0], 1.1f, 0.04f), 0, 1);


            timer++;


        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Tex = Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Impacts/flare_24").Value;
            Texture2D Tex3 = Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Impacts/flare_7").Value;

            Vector2 scale = new Vector2(xScale * size, yScale * size);

            Main.spriteBatch.Draw(Tex3, Projectile.Center - Main.screenPosition, null, Color.Black * (0.35f - Projectile.ai[0]), Projectile.ai[1], Tex3.Size() / 2, 0.25f + (0.75f * yScale), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(Tex3, Projectile.Center - Main.screenPosition, null, color * (1.25f - Projectile.ai[0]), Projectile.ai[1], Tex3.Size() / 2, 0.25f + (0.75f * yScale), SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tex3, Projectile.Center - Main.screenPosition, null, Color.White * (1.25f - Projectile.ai[0]), -Projectile.ai[1], Tex3.Size() / 2, 0.25f + (0.75f * yScale) * 0.5f, SpriteEffects.None, 0f);

            //Activate Shader
            Main.spriteBatch.Draw(Tex, Projectile.Center - Main.screenPosition, null, color * opacity, Projectile.rotation, Tex.Size() / 2, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tex, Projectile.Center - Main.screenPosition, null, color * opacity, Projectile.rotation, Tex.Size() / 2, scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End(); //make this restart better later
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }




    }
}