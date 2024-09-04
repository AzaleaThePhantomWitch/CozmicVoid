using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Shaders;

namespace CozmicVoid.Systems
{
    internal class ShaderRegistry
    {
        public static AssetRepository Assets => CozmicVoid.Instance.Assets;
        public static MiscShaderData SimpleTrailEffect => GameShaders.Misc["CozmicVoid:SimpleTrail"];
        public static MiscShaderData BlendTrailEffect => GameShaders.Misc["CozmicVoid:BlendTrail"];

        private static void RegisterMiscShader(string name, string pass)
        {
            string assetPath = $"Assets/Effects/{name}";
            Asset<Effect> miscShader = Assets.Request<Effect>(assetPath, AssetRequestMode.ImmediateLoad);
            GameShaders.Misc[$"CozmicVoid:{name}"] = new MiscShaderData(miscShader, pass);
        }

        public static void LoadShaders()
        {
            RegisterMiscShader("SimpleTrail", "PrimitivesPass");
            RegisterMiscShader("BlendTrail", "PrimitivesPass");
        }
    }
}