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
using Terraria.DataStructures;

namespace CozmicVoid.Assets.Impacts
{
    public class SmallImpact : ModProjectile
    {
        float timer = 0;
        public float opacity = 1f;
        public Color color = Color.White;
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
            Projectile.Opacity = 0.6f;
        }

        public override bool? CanDamage()
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.Next(0, 360);
        }

        float xScale = 0.3f;
        float yScale = 0.3f;
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
            Texture2D Tex = Mod.Assets.Request<Texture2D>("Assets/Effects/Masks/Impacts/flare_1").Value;

            Vector2 scale = new Vector2(xScale * size, yScale * size);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            //Activate Shader
            Main.spriteBatch.Draw(Tex, Projectile.Center - Main.screenPosition, null, color * Projectile.Opacity, Projectile.rotation, Tex.Size() / 2, scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Tex, Projectile.Center - Main.screenPosition, null, color * Projectile.Opacity, Projectile.rotation, Tex.Size() / 2, scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End(); //make this restart better later
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }




    }
}