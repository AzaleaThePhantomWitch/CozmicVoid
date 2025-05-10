using CozmicVoid.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace CozmicVoid.Content.Tiles
{
    public class MoonTechScrapT : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.ChecksForMerge[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            Main.tileMerge[ModContent.TileType<MoonTechBrickT>()][Type] = true;


            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(86, 75, 143), name);

            DustType = DustID.SnowBlock;
            HitSound = SoundID.DD2_CrystalCartImpact;
            MineResist = 1f;
            MinPick = 20;
            // name.SetDefault("Arnchar");
            RegisterItemDrop(ModContent.ItemType<MoonTechScrap>());
        }
        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
            WorldGen.TileMergeAttempt(-2, ModContent.TileType<MoonTechBrickT>(), ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

        }
    }
    public class MoonTechScrap : ModItem
    {
        public Color TripColor;
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A poisonous plant which weaves its way into entities");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100; // How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.gamepedia.com/Journey_Mode/Research_list for a list of commonly used research amounts depending on item type.
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.maxStack = Item.CommonMaxStack;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.width = 20; // The item texture's width
            Item.height = 20; // The item texture's height
            Item.createTile = ModContent.TileType<MoonTechScrapT>();
            Item.maxStack = Item.CommonMaxStack; // The item's max stack value
            Item.value = Item.buyPrice(copper: 40); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
        }
    }
}