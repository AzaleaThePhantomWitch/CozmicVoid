using Microsoft.Xna.Framework.Graphics;

namespace CozmicVoid.Systems.Shaders
{
    internal abstract class BaseShader
    {
        public Effect Effect { get; protected set; }
        public SamplerState SamplerState { get; set; } = SamplerState.PointWrap;
        public BlendState BlendState { get; set; } = BlendState.Additive;
        public abstract void Apply();
    }
}
