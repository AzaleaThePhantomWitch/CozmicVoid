using CozmicVoid.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

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
            base.Load();
            ShaderRegistry.LoadShaders();
        }
    }
}
