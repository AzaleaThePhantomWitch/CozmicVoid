using Microsoft.Xna.Framework.Graphics;

namespace CozmicVoid.Systems.Shaders
{
    internal interface IShader
    {
        Effect Effect { get; }
        void Apply();
    }
}
