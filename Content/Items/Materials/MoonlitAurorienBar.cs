
using Terraria;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CozmicVoid.Content.Items.Materials
{
    public class MoonlitAurorienBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A poisonous plant which weaves its way into entities");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100; // How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.gamepedia.com/Journey_Mode/Research_list for a list of commonly used research amounts depending on item type.
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3());

            DrawHelper.DrawItemShine(this.Item, Color.Turquoise, Color.MediumPurple, 0.2f, 1);


            DrawHelper.DrawGlow2InWorld(this.Item, spriteBatch, ref rotation, ref scale, whoAmI, Color.Teal, Color.Pink);
            if (Main.rand.NextBool(20))
            {
                Dust dust = Main.dust[Dust.NewDust(new Vector2(Item.position.X, Item.position.Y), 20, 26, 43, Item.velocity.X, Item.velocity.Y, 100, Color.Teal, Main.rand.NextFloat(.2f, .4f))];
                dust.velocity *= 0f;
                dust.noGravity = true;
                dust.fadeIn = 1.3f;
                dust.velocity.Y = -4f;
            }

            Color color = Color.White * 0.5f;
            Texture2D texture = (Texture2D)Mod.Assets.Request<Texture2D>("Content/Items/Materials/MoonlitAurorienBar");
            spriteBatch.Draw(texture, new Vector2(Item.position.X - Main.screenPosition.X + Item.width * 0.5f, Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f),
                new Rectangle(0, 0, texture.Width, texture.Height), color, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        }

        public override void SetDefaults()
        {

            Item.width = 20; // The item texture's width
            Item.height = 20; // The item texture's height
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = Item.CommonMaxStack; // The item's max stack value
            Item.value = Item.buyPrice(copper: 40); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}
	}
}