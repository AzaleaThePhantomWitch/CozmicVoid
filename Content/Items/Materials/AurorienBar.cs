using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Materials
{

    public class AurorienBar : ModItem
    {
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
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            if (!Main.dayTime)
            {
                gravity = 0;

                timer++;
                if (timer >= 100)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var velocity = Main.rand.NextVector2Circular(1, 1);
                        Gore.NewGorePerfect(Item.GetSource_FromThis(), Item.position, velocity * 2, 16, Main.rand.NextFloat(0.5f, 1.3f));
                        Gore.NewGorePerfect(Item.GetSource_FromThis(), Item.position, velocity * 2, 17, Main.rand.NextFloat(0.5f, 1.3f));
                    }
                    Item.NewItem(Item.GetSource_FromThis(), Item.Hitbox, ModContent.ItemType<StarFragment>(), Item.stack);
                    Item.TurnToAir();
                    //Item.ChangeItemType(ModContent.ItemType<StarFragment>());
                    //Item.type = ModContent.ItemType<StarFragment>();
                }
                else if (timer == 0)
                {
                    Item.velocity.Y -= 92;
                }
                else
                {
                    Item.velocity /= 1.05f;
                }
            }
            else
            {
                timer = 0;
            }

        }
    }
}