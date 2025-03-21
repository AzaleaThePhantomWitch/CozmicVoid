

global using static Luminance.Common.Utilities.Utilities;

using CozmicVoid.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;




namespace CozmicVoid
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class CozmicVoid : Mod
	{
        public CozmicVoid()
        {
            Instance = this;
        }

        public static CozmicVoid Instance;

        public override void Load()
        {
            if (!Main.dedServ)
            {
    
                Main.instance.LoadTiles(TileID.Dirt);
                TextureAssets.Tile[TileID.Dirt] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/DirtRE");

                Main.instance.LoadTiles(TileID.IceBlock);
                TextureAssets.Tile[TileID.IceBlock] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/IceRE");

                Main.instance.LoadTiles(TileID.SnowBlock);
                TextureAssets.Tile[TileID.SnowBlock] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/SnowRE");

                Main.instance.LoadTiles(TileID.Stone);
                TextureAssets.Tile[TileID.Stone] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/StoneRE");

                Main.instance.LoadTiles(TileID.Grass);
                TextureAssets.Tile[TileID.Grass] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/GrassRE");


                Main.instance.LoadTiles(TileID.ClayBlock);
                TextureAssets.Tile[TileID.ClayBlock] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/ClayRE");

                Main.instance.LoadTiles(TileID.Sand);
                TextureAssets.Tile[TileID.Sand] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/SandRE");

                Main.instance.LoadTiles(TileID.HardenedSand);
                TextureAssets.Tile[TileID.HardenedSand] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/HardSandRE");

                Main.instance.LoadTiles(TileID.Sandstone);
                TextureAssets.Tile[TileID.Sandstone] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/StoneSandRE");

                Main.instance.LoadTiles(TileID.Mud);
                TextureAssets.Tile[TileID.Mud] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/MudRE");

                Main.instance.LoadTiles(TileID.CrimsonGrass);
                TextureAssets.Tile[TileID.CrimsonGrass] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/CrimGrassRE");

                Main.instance.LoadTiles(TileID.JungleGrass);
                TextureAssets.Tile[TileID.JungleGrass] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/MudGrassRE");

                Main.instance.LoadTiles(TileID.CorruptGrass);
                TextureAssets.Tile[TileID.CorruptGrass] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/CrorpGrassRE");

                Main.instance.LoadTiles(TileID.Crimstone);
                TextureAssets.Tile[TileID.Crimstone] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/CrimStoneRE");

                Main.instance.LoadTiles(TileID.WoodBlock);
                TextureAssets.Tile[TileID.WoodBlock] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/WoodRE");

                Main.instance.LoadTiles(TileID.GrayBrick);
                TextureAssets.Tile[TileID.GrayBrick] = ModContent.Request<Texture2D>("CozmicVoid/TileTextures/StoneBrickRE");

            }
            base.Load();
            ShaderRegistry.LoadShaders();
        }
        private void UnloadTile(int tileID)
        {
            TextureAssets.Tile[tileID] = ModContent.Request<Texture2D>($"Terraria/Images/Tiles_{tileID}");
        }
        public override void Unload()
        {

            if (!Main.dedServ)
            {
                UnloadTile(TileID.Grass);
                UnloadTile(TileID.Dirt);
                UnloadTile(TileID.WoodBlock);
                UnloadTile(TileID.GrayBrick);
                UnloadTile(TileID.Stone);
                UnloadTile(TileID.ClayBlock);
                UnloadTile(TileID.IceBlock);
                UnloadTile(TileID.SnowBlock);
                UnloadTile(TileID.CrimsonGrass);
                UnloadTile(TileID.Sandstone);
                UnloadTile(TileID.HardenedSand);
                UnloadTile(TileID.Sand);
                UnloadTile(TileID.Crimstone);
                UnloadTile(TileID.JungleGrass);
                UnloadTile(TileID.Mud);
                UnloadTile(TileID.CorruptGrass);
            }
        }

    }
}
