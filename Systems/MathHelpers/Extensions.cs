using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozmicVoid.Systems.MathHelpers
{
    internal static class Extensions
    {
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.X, vector2.Y, 0);
        }
    }
}
