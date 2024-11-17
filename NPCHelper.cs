using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CozmicVoid
{
    public static class NPCHelper
    {
        public static void DrawAdditiveAfterImage(NPC npc, Color startColor, Color endColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 vector2_3 = new Vector2(TextureAssets.Npc[npc.type].Value.Width / 2, TextureAssets.Npc[npc.type].Value.Height / Main.npcFrameCount[npc.type] / 2);
            Vector2 drawOrigin = new Vector2(TextureAssets.Npc[npc.type].Value.Width * 0.3f, (npc.height / Main.npcFrameCount[npc.type]) * 2f);
            if (npc.velocity != Vector2.Zero)
            {
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Vector2 DrawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(Color.Lerp(startColor, endColor, 1f / npc.oldPos.Length * k) * (1f - 1f / npc.oldPos.Length * k));
                    Main.spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, DrawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }



        public static void DrawPostDraw(NPC npc, Color startColor, Color endColor, Texture2D GlowTexture, Vector2 screenPos)
        {
            Lighting.AddLight(npc.Center, Color.LightGoldenrodYellow.ToVector3() * 1.75f * Main.essScale);
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            Vector2 halfSize = new Vector2(GlowTexture.Width / 2, GlowTexture.Height / Main.npcFrameCount[npc.type] / 2);
            Main.spriteBatch.Draw(
                GlowTexture,
                new Vector2(npc.position.X - screenPos.X + (npc.width / 2) - GlowTexture.Width * npc.scale / 2f + halfSize.X * npc.scale, npc.position.Y - screenPos.Y + npc.height - GlowTexture.Height * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + Main.NPCAddHeight(npc) + npc.gfxOffY),
                npc.frame,
                Color.White,
                npc.rotation,
                halfSize,
                npc.scale,
                spriteEffects,
            0);
        }
        public static void DrawDrugEffect(NPC npc, Vector2 screenPos, Color startColor, Color endColor)
        {
            Vector2 center = npc.Center + new Vector2(0f, npc.height * -0.1f);
            Lighting.AddLight(npc.Center, Color.Purple.ToVector3() * 0.25f * Main.essScale);
            // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
            Vector2 direction = Main.rand.NextVector2CircularEdge(npc.width * 0.6f, npc.height * 0.6f);
            float distance = 0.3f + Main.rand.NextFloat() * 0.5f;
            Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);
            Texture2D texture = TextureAssets.Npc[npc.type].Value;



            Vector2 frameOrigin = npc.frame.Size();
            Vector2 offset = new Vector2(npc.width - frameOrigin.X + 0, npc.height - npc.frame.Height + 5);
            Vector2 drawPos = npc.position - screenPos + frameOrigin + offset;

            float time = Main.GlobalTimeWrappedHourly;
            float timer = Main.GlobalTimeWrappedHourly / 2f + time * 0.04f;

            time %= 4f;
            time /= 2f;

            if (time >= 1f)
            {
                time = 2f - time;
            }

            time = time * 0.5f + 0.5f;
            SpriteEffects Effects = npc.spriteDirection != -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (float i = 0f; i < 1f; i += 0.25f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                Main.spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, npc.frame, new Color(startColor.R, startColor.G, startColor.B, 0), npc.rotation, frameOrigin, npc.scale, Effects, 0);
            }

            for (float i = 0f; i < 1f; i += 0.34f)
            {
                float radians = (i + timer) * MathHelper.TwoPi;

                Main.spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, npc.frame, new Color(startColor.R, startColor.G, startColor.B, 0), npc.rotation, frameOrigin, npc.scale, Effects, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
