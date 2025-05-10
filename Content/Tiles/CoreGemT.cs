using System;
using System.Collections.Generic;
using CozmicVoid.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace CozmicVoid.Content.Tiles
{
    public class CoreGemT : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
            Main.tileOreFinderPriority[Type] = 395; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 100; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;


            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Frile Ore");
            AddMapEntry(new Color(255, 169, 0), name);
            Main.tileMerge[TileID.Mud][Type] = true;
            DustType = DustID.RainbowMk2;
            HitSound = SoundID.DD2_CrystalCartImpact;
            MineResist = 1f;
            MinPick = 20;
            // name.SetDefault("Arnchar");
            RegisterItemDrop(ModContent.ItemType<CoreGem>());
        }

        public override void ModifyFrameMerge(int i, int j, ref int up, ref int down, ref int left, ref int right, ref int upLeft, ref int upRight, ref int downLeft, ref int downRight)
        {
            WorldGen.TileMergeAttempt(-2, TileID.Mud, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
        }


        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

        }
    }
}