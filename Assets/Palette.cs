using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;



namespace CozmicVoid.Assets
{
    public class Palette
    {
        private readonly List<Vector4> colors = [];

        public Palette(params Color[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
                AddColor(colors[i]);
        }

        public Palette(Vector3[] colors)
        {
            for (int i = 0; i < colors.Length; i++)
                AddColor(colors[i]);
        }

        /// <summary>
        /// Adds a new <see cref="Vector3"/> color representation to the palette.
        /// </summary>
        public Palette AddColor(Vector3 color)
        {
            colors.Add(new Vector4(color, 1f));
            return this;
        }

        /// <summary>
        /// Adds a new <see cref="Vector4"/> color representation to the palette.
        /// </summary>
        public Palette AddColor(Vector4 color)
        {
            colors.Add(color);
            return this;
        }

        /// <summary>
        /// Adds a new <see cref="Color"/> color representation to the palette.
        /// </summary>
        public Palette AddColor(Color color) => AddColor(color.ToVector4());


        public Vector4 SampleVector(float interpolant)
        {
            if (colors.Count <= 1)
                return colors[0];

            // Apply interpolant safety checks.
            if (float.IsNaN(interpolant) || float.IsInfinity(interpolant))
                interpolant = 0f;
            interpolant = Utils.Clamp(interpolant, 0f, 0.999f);

            int gradientStartingIndex = (int)(interpolant * colors.Count);
            float currentColorInterpolant = interpolant * colors.Count % 1f;
            Vector4 gradientSubdivisionA = colors[gradientStartingIndex];
            Vector4 gradientSubdivisionB = colors[Utils.Clamp(gradientStartingIndex + 1, 0, colors.Count - 1)];
            return Vector4.Lerp(gradientSubdivisionA, gradientSubdivisionB, currentColorInterpolant);
        }


        /// <summary>
        /// Samples across this palette based on a given 0-1 interpolant.
        /// </summary>
        public Color SampleColor(float interpolant) => new Color(SampleVector(interpolant));
    }

}