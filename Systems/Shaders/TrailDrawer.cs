using CozmicVoid.Systems.MathHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using Terraria;

namespace CozmicVoid.Systems.Shaders
{
    internal class TrailDrawer
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

        public static void Draw(SpriteBatch spriteBatch, 
            Vector2[] oldPos,
            float[] oldRot, 
            Func<float, Color> colorFunc, 
            Func<float, Vector2> widthFunc,
            Effect? effect = null,
            Vector2? framing = null,
            Vector2? offset = null)
        {
            GraphicsDevice graphicsDevice = Main.graphics.GraphicsDevice;
            List<VertexPositionColorTexture> vertices = new List<VertexPositionColorTexture>();

            //Apply passes
            if(effect != null)
            {
                ApplyPasses(effect);
            }

            Vector2 o = offset == null ? Vector2.Zero : (Vector2)offset;
            Vector2 f = framing == null ? Vector2.Zero : (Vector2)framing;
            oldPos = RemoveZeros(oldPos, o);
            float length = oldPos.Length;
            for(int i = 0; i < oldPos.Length-1; i++)
            {
                
                float uv = i / length;
                float uv2 = (i + 1) / length;
                Vector2 width = widthFunc(uv);
                Vector2 width2 = widthFunc(uv2);
                Vector2 pos1 = oldPos[i];
                Vector2 pos2 = oldPos[i + 1];
                Vector2 off1 = GetRotation(oldPos,i) * width;
                Vector2 off2 = GetRotation(oldPos,i + 1) * width2;
                float coord1 = off1.Length() < off2.Length() ? 1 : 0;
                float coord2 = off1.Length() < off2.Length() ? 0 : 1;
                Color col1 = colorFunc(uv);
                Color col2 = colorFunc(uv2);
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 + off1, 0f), col1, new Vector2((uv) , coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 - off1, 0f), col1, new Vector2((uv) , coord2)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 + off2, 0f), col2, new Vector2((uv2) , coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 + off2, 0f), col2, new Vector2((uv2) , coord1)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos2 - off2, 0f), col2, new Vector2((uv2), coord2)));
                vertices.Add(new VertexPositionColorTexture(new Vector3(pos1 - off1, 0f), col1, new Vector2((uv) , coord2)));
                
                
                /*
                float uv = (float)i / length;
                Vector2 width = widthFunc(uv);
                Color color = colorFunc(uv);
                Vector2 pos = oldPos[i];
        
  
                Vector2 top = pos + GetRotation(oldPos, i) * width;
                Vector2 bottom = pos - GetRotation(oldPos, i) * width;
                Vector3 finalTop = top.ToVector3();
                Vector3 finalBottom = bottom.ToVector3();
                vertices.Add(new VertexPositionColorTexture(finalTop, color, new Vector2(uv, 0)));
                vertices.Add(new VertexPositionColorTexture(finalBottom, color, new Vector2(uv, 1)));*/
            }

            BlendState originalBlendState = graphicsDevice.BlendState;
            CullMode oldCullMode = graphicsDevice.RasterizerState.CullMode;
            SamplerState originalSamplerState = graphicsDevice.SamplerStates[0];

            graphicsDevice.RasterizerState.CullMode = CullMode.None;
            graphicsDevice.BlendState = BlendState.Additive;
            graphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            
            graphicsDevice.DrawUserPrimitives(
                PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
           
            graphicsDevice.RasterizerState.CullMode = oldCullMode;
            graphicsDevice.BlendState = originalBlendState;
            graphicsDevice.SamplerStates[0] = originalSamplerState;
        }

        public static void Draw(SpriteBatch spriteBatch, 
            Vector2[] oldPos,
            float[] oldRot,
            Func<float, Color> colorFunc,
            Func<float, Vector2> widthFunc,
            IShader shader,
            Vector2? framing = null,
            Vector2? offset = null)
        {
            shader.Apply();
            Draw(spriteBatch, oldPos, oldRot, colorFunc, widthFunc, shader.Effect, framing, offset);
        }
    }
}
