﻿using CozmicVoid.Systems.MathHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace CozmicVoid.Systems.Shaders
{
    internal class SwordTrailDrawer
    {
        public static Matrix WorldViewPoint
        {
            get
            {
                GraphicsDevice graphics = Main.graphics.GraphicsDevice;
                Vector2 screenZoom = Main.GameViewMatrix.Zoom;
                int width = graphics.Viewport.Width;
                int height = graphics.Viewport.Height;

                var zoom = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) *
                    Matrix.CreateTranslation(width / 2f, height / -2f, 0) *
                    Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(screenZoom.X, screenZoom.Y, 1f);
                var projection = Matrix.CreateOrthographic(width, height, 0, 1000);
                return zoom * projection;
            }
        }

        public static Matrix WorldViewPoint2
        {
            get
            {
                Vector3 screenPosition = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0);
                Matrix world = Matrix.CreateTranslation(-screenPosition);
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);
                return world * view * projection;
            }
        }

        private static void ApplyPasses(Effect effect)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
        }

        private static Vector2 GetRotation(Vector2[] oldPos, int index)
        {
            if (oldPos.Length == 1)
                return oldPos[0];

            if (index == 0)
                return Vector2.Normalize(oldPos[1] - oldPos[0]).RotatedBy(MathHelper.Pi / 2);

            return (index == oldPos.Length - 1
                ? Vector2.Normalize(oldPos[index] - oldPos[index - 1])
                : Vector2.Normalize(oldPos[index + 1] - oldPos[index - 1])).RotatedBy(MathHelper.Pi / 2);
        }


        private static Vector2[] RemoveZeros(Vector2[] arr, Vector2 offset)
        {
            var valid = new List<Vector2>();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == Vector2.Zero || arr[i].HasNaNs())
                    break;
                if (i != 0)
                {
                    if (arr[i - 1] == arr[i])
                        continue;

                    var d = arr[i - 1] - arr[i];
                    if (d.X < -1000f || d.X > 1000f || d.Y < -1000f || d.Y > 1000f)
                    {
                        //Main.NewText(d + " = " + arr[i - 1] + " - " + arr[i]);
                        continue;
                    }
                }
                valid.Add(arr[i] + offset);
            }
            return valid.ToArray();
        }

        private static List<VertexPositionColorTexture> CalculateVertices(Vector2[] oldPos, 
            Func<float, Color> colorFunc,
            Func<float, float> widthFunc,
            Vector2? offset = null)
        {
            Vector2 o = offset == null ? Vector2.Zero : (Vector2)offset;
            var vertices = new List<VertexPositionColorTexture>();
            oldPos = RemoveZeros(oldPos, o);
            LerpTrailPoints(oldPos, out Vector2[] trailingPoints);
            /*
            for (int i = 0; i < trailingPoints.Length; i++)
            {
                float length = trailingPoints.Length;
                float uv = i / length;

                Vector2 width = widthFunc(uv) * Vector2.One;
                Color color = colorFunc(uv);
                Vector2 pos = trailingPoints[i];

                Vector2 top = pos + GetRotation(trailingPoints, i) * width;
                Vector2 bottom = pos - GetRotation(trailingPoints, i) * width;
                Vector3 finalTop = top.ToVector3();
                Vector3 finalBottom = bottom.ToVector3();

               
                vertices.Add(new VertexPositionColorTexture(finalTop, color, new Vector2(uv, 0)));
                vertices.Add(new VertexPositionColorTexture(finalBottom, color, new Vector2(uv, 1)));

            }*/

            for (int i = 0; i < trailingPoints.Length - 1; i++)
            {
                float uv = i / (float)trailingPoints.Length;
                float uv2 = (i + 1) / (float)trailingPoints.Length;
                Vector2 width = widthFunc(uv) * Vector2.One;
                Vector2 width2 = widthFunc(uv2) * Vector2.One;
                Vector2 pos1 = trailingPoints[i];
                Vector2 pos2 = trailingPoints[i + 1];

                Vector2 off1 = GetRotation(trailingPoints, i) * width;
                Vector2 off2 = GetRotation(trailingPoints, i + 1) * width2;

                Color col1 = colorFunc(uv);
                Color col2 = colorFunc(uv2);
                float uvAdd = 0;
                float uvMultiplier = 1;
                float coord1 = 0;
                float coord2 = 1;
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 + off1, 0f), col1, new Vector2((uv + uvAdd) * uvMultiplier, coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 - off1, 0f), col1, new Vector2((uv + uvAdd) * uvMultiplier, coord2)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 + off2, 0f), col2, new Vector2((uv2 + uvAdd) * uvMultiplier, coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 + off2, 0f), col2, new Vector2((uv2 + uvAdd) * uvMultiplier, coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 - off2, 0f), col2, new Vector2((uv2 + uvAdd) * uvMultiplier, coord2)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 - off1, 0f), col1, new Vector2((uv + uvAdd) * uvMultiplier, coord2)));
            }
            return vertices;
        }

        private static void LerpTrailPoints(Vector2[] oldPos, out Vector2[] trailingPoints)
        {
            float smoothFactor = 1;
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < oldPos.Length - 1; i++)
            {
                Vector2 current = oldPos[i];
                Vector2 next = oldPos[i + 1];
                for(float  j = 0;  j < smoothFactor; j++)
                {
                    float p = j / smoothFactor;
                    Vector2 smoothedPoint = Vector2.Lerp(current, next, p);
                    points.Add(smoothedPoint);
                }
            }
            trailingPoints = points.ToArray();
        }

        public static void Draw(SpriteBatch spriteBatch, 
            Vector2[] oldPos,
            float[] oldRot, 
            Func<float, Color> colorFunc, 
            Func<float, float> widthFunc,
            BaseShader shader,
            Vector2? offset = null)
        {
            //Apply passes
            shader.Apply();
            ApplyPasses(shader.Effect);
            if (shader.FillShape)
            {
                Vector2[] filledPos = new Vector2[oldPos.Length + 1];
                for(int i = 0; i < oldPos.Length; i++)
                {
                    filledPos[i] = oldPos[i];
                }
                filledPos[filledPos.Length - 1] = oldPos[0];
                oldPos = filledPos;
            }

            var vertices = CalculateVertices(oldPos, colorFunc, widthFunc, offset);
            DrawPrims(vertices, shader);
            shader.FillShape = false;
        }


        private static void DrawPrims(List<VertexPositionColorTexture> vertices, BaseShader shader)
        {
            if (vertices.Count % 6 != 0 || vertices.Count <= 3)
                return;

            GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
            BlendState originalBlendState = graphicsDevice.BlendState;
            CullMode oldCullMode = graphicsDevice.RasterizerState.CullMode;
            SamplerState originalSamplerState = graphicsDevice.SamplerStates[0];

            graphicsDevice.RasterizerState.CullMode = CullMode.None;
            graphicsDevice.BlendState = shader.BlendState;
            graphicsDevice.SamplerStates[0] = shader.SamplerState;

            graphicsDevice.DrawUserPrimitives(
              PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);

            graphicsDevice.RasterizerState.CullMode = oldCullMode;
            graphicsDevice.BlendState = originalBlendState;
            graphicsDevice.SamplerStates[0] = originalSamplerState;
        }
    }
}
