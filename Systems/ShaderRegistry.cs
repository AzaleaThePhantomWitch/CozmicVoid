using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace CozmicVoid.Systems
{
    internal class ShaderRegistry
    {
        public static AssetRepository Assets => CozmicVoid.Instance.Assets;
        public static MiscShaderData SimpleTrailEffect => GameShaders.Misc["CozmicVoid:SimpleTrail"];
        public static void LoadShaders()
        {
            Asset<Effect> miscShader = Assets.Request<Effect>("Assets/Effects/SimpleTrail", AssetRequestMode.ImmediateLoad);
            GameShaders.Misc["CozmicVoid:SimpleTrail"] = new MiscShaderData(miscShader, "PrimitivesPass");
        }
    }
}