using Microsoft.Xna.Framework.Graphics;

namespace NoxusBoss.Core.Graphics.Automators;

public interface ShaderDraws
{
    public float LayeringPriority => 0f;

    public bool ShaderShouldDrawAdditively => false;

    public void DrawWithShader(SpriteBatch spriteBatch);
}