using CozmicVoid.Dusts;
using CozmicVoid.Systems.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Materials
{
    public class AurorienBar : ModItem
    {
        float timer2 = 0;
        int timer = 0;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A poisonous plant which weaves its way into entities");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100; // How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.gamepedia.com/Journey_Mode/Research_list for a list of commonly used research amounts depending on item type.
        }


        public override void SetDefaults()
        {
            Item.width = 20; // The item texture's width
            Item.height = 20; // The item texture's height

            Item.maxStack = Item.CommonMaxStack; // The item's max stack value
            Item.value = Item.buyPrice(copper: 40); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
        }

        public override bool CanPickup(Player player)
        {
            if (timer >= 100 || timer == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {

            DrawHelper.DrawItemShine(this.Item, Color.Turquoise, Color.MediumPurple, timer2, 1);

        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (!Main.dayTime)
            {
                gravity = 0;

                timer2 += 0.005f;
                timer++;
                if (timer >= 100)
                {
                    Main.LocalPlayer.GetModPlayer<EffectsPlayer>().ShakeAtPosition(Item.Center, 700f, 8f);
                    for (int i = 0; i <= 18; i++)
                    {
                        Dust.NewDust(base.Item.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(72, 78, 230), 0.5f);
                        Dust.NewDust(base.Item.Center, 22, 22, ModContent.DustType<GlowDust>(), 0f, 0f, 0, new Color(120, 83, 153), 0.5f);
                    }
                    for (int i = 0; i < 7; i++)
                    {
                        var velocity = Main.rand.NextVector2Circular(1, 1);
                        Gore.NewGorePerfect(Item.GetSource_FromThis(), Item.Center, velocity * 2, 16, Main.rand.NextFloat(0.5f, 1.3f));
                        Gore.NewGorePerfect(Item.GetSource_FromThis(), Item.Center, velocity * 2, 17, Main.rand.NextFloat(0.5f, 1.3f));
                    }
                    Item.NewItem(Item.GetSource_FromThis(), Item.Hitbox, ModContent.ItemType<MoonlitAurorienBar>(), Item.stack);
                    Item.TurnToAir();
                    //Item.ChangeItemType(ModContent.ItemType<StarFragment>());
                    //Item.type = ModContent.ItemType<StarFragment>();
                }
                else if (timer == 0)
                {
                    Item.velocity.Y -= 150;
                }
                else
                {
                    Item.velocity /= 1.02f;
                }
            }
            else
            {
                timer = 0;
            }

        }
    }
}