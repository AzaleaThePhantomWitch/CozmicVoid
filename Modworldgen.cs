﻿using CozmicVoid.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CozmicVoid
{
    public class Modworldgen : ModSystem
    {
        public class World : ModSystem
        {
            public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
            {
                int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Gems"));

                if (ShiniesIndex != -1)
                {
                    tasks.Insert(ShiniesIndex + 1, new Aurorien_Rock("Aurorien Rock", 237.4298f));
                }
            }
        }
    }
}
public class Aurorien_Rock : GenPass
{

    public Aurorien_Rock(string name, float loadWeight) : base(name, loadWeight)
    {
    }
    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.0009); k++)
        {
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);//Main.maxTilesX / 2;
            int y = WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.maxTilesY + 200);
            Tile tile = Main.tile[x, y];

            if (tile.TileType == TileID.IceBlock || tile.TileType == TileID.SnowBlock)
            {
                WorldGen.TileRunner(x, y, 6, 3, ModContent.TileType<AurorienRockT>());
            }
        }

    }
}