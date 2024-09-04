using Microsoft.Xna.Framework.Graphics;

namespace CozmicVoid.Systems.Shaders
{
    internal abstract class BaseShader
    {
        public Effect Effect { get; protected set; }
        public BlendState BlendState { get; set; } = BlendState.Additive;
        public SamplerState SamplerState { get; set; } = SamplerState.LinearWrap;
        public bool FillShape { get; set; }
        public abstract void Apply();
    }
}
