using CozmicVoid.Content.Items.Materials;
using CozmicVoid.Dusts;
using CozmicVoid.Systems.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using System;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CozmicVoid.Content.NPCs.Night.StarDreamer
{

    public class StarDreamer : ModNPC
    {
        private float StarAmoutMax;
        private float StarAmout;
        private float StarRotationSpeed;
        private float StarHeadSize;
        private float StarRotation;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailCacheLength[NPC.type] = 10;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {

            //Base Stats
            NPC.damage = 45;
            NPC.defense = 3;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0.55f;

            //Size

            NPC.width = 36;
            NPC.height = 40;
            NPC.scale = 1.1f;

            //other
            NPC.value = 65f;
            NPC.alpha = 0;
            NPC.HitSound = new SoundStyle("CozmicVoid/Assets/Sounds/Npc/StarDreamer_Hit") with { PitchVariance = 0.1f };
            NPC.DeathSound = SoundID.NPCDeath56;

            //Effects
            NPC.lavaImmune = false;
            NPC.dontTakeDamage = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {


            NPC.frameCounter += 1f;

            if (NPC.frameCounter >= 7)
            {
                frame++;
                NPC.frameCounter = 0;
            }
            if (frame >= 6)
            {
                frame = 0;
            }
            NPC.frame.Y = frameHeight * frame;

        }

        public override void OnSpawn(IEntitySource source)
        {
            StarAmoutMax = Main.rand.Next(2, 6);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Player.ZonePurity)
                return 0;
            return SpawnCondition.OverworldDaySlime.Chance;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            base.ModifyNPCLoot(npcLoot);
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarFragment>(), minimumDropped: 1, maximumDropped: 4));
        }
        public override void AI()
        {
            Main.npcFrameCount[NPC.type] = 6;
            Player player = Main.player[NPC.target];
            NPC.rotation = NPC.velocity.X * 0.05f;
            NPC.spriteDirection = -NPC.direction;
            StarRotation -= StarRotationSpeed;

            StarRotationSpeed = MathHelper.Lerp(StarRotationSpeed, 0.04f, 0.1f);

            if (NPC.ai[3] == 200 || NPC.ai[3] == 220 || NPC.ai[3] == 240 || NPC.ai[3] == 260 || NPC.ai[3] == 280)
            {
                StarAmout += 1;
                if (StarAmout >= StarAmoutMax)
                {
                    StarAmoutMax = Main.rand.Next(2, 6);
                    StarAmout = 0;
                    NPC.ai[3] = 0;
                }
                StarRotationSpeed = 0.5f;
                NPC.alpha = 40;
                StarHeadSize = 0.35f;
                Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 8.5f;
                float PosoffsetX = Main.rand.Next(-60, 60);
                float PosoffsetY = Main.rand.Next(-60, 60);
                float offsetX = Main.rand.Next(-50, 50) * 0.01f;
                float offsetY = Main.rand.Next(-50, 50) * 0.01f;
                int damage = Main.expertMode ? 7 : 13;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + PosoffsetX, NPC.Center.Y + PosoffsetY, (direction.X + offsetX) / 4, (direction.Y + offsetY) / 4, ModContent.ProjectileType<StarFlare>(), damage, 1, Main.myPlayer, 0, 0);
            }
            if (NPC.ai[3] >= 300)
            {
                StarAmout = 0;
                StarAmoutMax = Main.rand.Next(2, 6);
                NPC.ai[3] = 0;
            }


            if (Main.netMode != NetmodeID.Server)
            {
                //Dust.NewDustPerfect(base.NPC.Center, ModContent.DustType<glow>(), (Vector2.One * Main.rand.Next(1, 12)).RotatedByRandom(19.0), 0, default(Color), 1f).noGravity = true;

                Dust dust = Dust.NewDustDirect(NPC.Center, NPC.width, NPC.height, ModContent.DustType<GlowDust>());
                dust.velocity *= -1f;
                dust.scale *= .8f;
                dust.noGravity = true;
                Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 200) * 0.06f);
                dust.velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 104f;
                dust.position = NPC.Center - vector2_3;
                NPC.netUpdate = true;
            }




            float num = 1f - (float)NPC.alpha / 255f;
            bool expertMode = Main.expertMode;
            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest(true);
            NPC.rotation = NPC.velocity.X * 0.05f;

            int Distance = (int)(NPC.Center - player.Center).Length();
            if (Distance < 350f)
            {
                StarHeadSize = MathHelper.Lerp(StarHeadSize, 0.25f, 0.1f);
                NPC.ai[3]++;
            }
            else
            {
                StarHeadSize = MathHelper.Lerp(StarHeadSize, 0.15f, 0.1f);
            }
            NPC.rotation = NPC.velocity.X * .08f;

            float velMax = 1f;
            float acceleration = 0.011f;
            NPC.TargetClosest(true);
            Vector2 center = NPC.Center;
            float deltaX = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2) - center.X;
            float deltaY = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2) - center.Y - 150;
            float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            if (NPC.ai[1] > 200.0)
            {

                if (NPC.ai[1] > 300.0)
                {
                    NPC.ai[1] = 0f;
                }
            }
            else if (distance < 120.0)
            {
                NPC.ai[0] += 0.9f;
                if (NPC.ai[0] > 0f)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.039f;
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.019f;
                }
                if (NPC.ai[0] < -100f || NPC.ai[0] > 100f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.029f;
                }
                else
                {
                    NPC.velocity.X = NPC.velocity.X - 0.029f;
                }
                if (NPC.ai[0] > 25f)
                {
                    NPC.ai[0] = -200f;
                }
            }
            if (Main.rand.NextBool(30) && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(2))
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.439f;
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.419f;
                }
                NPC.netUpdate = true;
            }
            if (distance > 350.0)
            {
                velMax = 5f;
                acceleration = 0.2f;
            }
            else if (distance > 300.0)
            {
                velMax = 3f;
                acceleration = 0.25f;
            }
            else if (distance > 250.0)
            {
                velMax = 2.5f;
                acceleration = 0.13f;
            }
            float stepRatio = velMax / distance;
            float velLimitX = deltaX * stepRatio;
            float velLimitY = deltaY * stepRatio;
            if (Main.player[NPC.target].dead)
            {
                velLimitX = (float)((NPC.direction * velMax) / 2.0);
                velLimitY = (float)((-velMax) / 2.0);
            }
            if (NPC.velocity.X < velLimitX)
                NPC.velocity.X = NPC.velocity.X + acceleration;
            else if (NPC.velocity.X > velLimitX)
                NPC.velocity.X = NPC.velocity.X - acceleration;
            if (NPC.velocity.Y < velLimitY)
                NPC.velocity.Y = NPC.velocity.Y + acceleration;
            else if (NPC.velocity.Y > velLimitY)
                NPC.velocity.Y = NPC.velocity.Y - acceleration;
        }
        public virtual string GlowTexturePath => Texture + "_Glow";
        private Asset<Texture2D> _glowTexture;
        public Texture2D GlowTexture => (_glowTexture ??= (ModContent.RequestIfExists<Texture2D>(GlowTexturePath, out var asset) ? asset : null))?.Value;

        public virtual string HeadTexturePath => Texture + "Head";
        private Asset<Texture2D> _HeadTexture;
        public Texture2D HeadTexture => (_HeadTexture ??= (ModContent.RequestIfExists<Texture2D>(HeadTexturePath, out var asset) ? asset : null))?.Value;


        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if (GlowTexture is not null)
            {

       
                SpriteEffects spriteEffects = SpriteEffects.None;
                Vector2 frameOrigin = NPC.frame.Size();
                Vector2 offset = new Vector2(NPC.width - frameOrigin.X - 17 + (NPC.rotation * 30), NPC.height - NPC.frame.Height - 55);
                Vector2 DrawPos = NPC.position - screenPos + frameOrigin + offset;


                spriteBatch.Draw(HeadTexture, DrawPos, null, drawColor, StarRotation, new Vector2 (48, 48), NPC.scale * StarHeadSize, spriteEffects, 0f);



                if (NPC.spriteDirection == 1)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                Vector2 halfSize = new Vector2(GlowTexture.Width / 2, GlowTexture.Height / Main.npcFrameCount[NPC.type] / 2);
                spriteBatch.Draw(
                    GlowTexture,
                    new Vector2(NPC.position.X - screenPos.X + (NPC.width / 2) - GlowTexture.Width * NPC.scale / 2f + halfSize.X * NPC.scale, NPC.position.Y - screenPos.Y + NPC.height - GlowTexture.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + halfSize.Y * NPC.scale + Main.NPCAddHeight(NPC) + NPC.gfxOffY),
                    NPC.frame,
                    Color.White,
                    NPC.rotation,
                    halfSize,
                    NPC.scale,
                    spriteEffects,
                0);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color lightColor)
        {
            Vector2 frameOrigin = NPC.frame.Size();
            Vector2 offset = new Vector2(NPC.width - frameOrigin.X - 17, NPC.height - NPC.frame.Height - 55);
            Lighting.AddLight(NPC.Center + offset, Color.LightYellow.ToVector3() * 1.5f * Main.essScale);


               SpriteEffects Effects = ((base.NPC.spriteDirection != -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            var drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Width() * 0.5f, NPC.height * 0.5f);
            for (int k = 0; k < NPC.oldPos.Length; k++)
            {
                Vector2 Trailoffset = new Vector2(NPC.width - frameOrigin.X - 3, NPC.height - NPC.frame.Height - 2);

                Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + NPC.Size / 2 + new Vector2(0f, NPC.gfxOffY);
                Color color = NPC.GetAlpha(Color.Lerp(new Color(120, 83, 153), new Color(72, 78, 230), 1f / NPC.oldPos.Length * k) * (1f - 1f / NPC.oldPos.Length * k));
                spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos + Trailoffset, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, Effects, 0f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(150, 80, 40), 0.2f);
                Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.2f);
                Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.2f);
            }
            if (NPC.life <= 0)
            {

                for (int i = 0; i <= 18; i++)
                {
                    Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(NPC.Center, 700f, 15f);
                    Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<TSmokeDust>(), base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, Color.Pink);
                    Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(150, 80, 40), 0.5f);
                    Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                    Dust.NewDust(base.NPC.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
                }
            }
        }
    }
}