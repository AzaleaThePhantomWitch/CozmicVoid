matrix transformMatrix;
texture trailTexture;
texture secondaryTrailTexture;
float4 primaryColor;
float4 secondaryColor;
float time;

sampler2D trailTex = sampler_state
{
    texture = <trailTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

sampler2D secondaryTrailTex = sampler_state
{
    texture = <secondaryTrailTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap; //循环UV
};

struct VertexShaderInput
{
    float4 Position : POSITION;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Color = input.Color;
    output.TexCoords = input.TexCoords;
    output.Position = mul(input.Position, transformMatrix);
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 offset = float2(time * -0.05, 0.0);
    float2 offset2 = float2(time * -0.025, 0.0);
    float4 primaryTexColor = tex2D(trailTex, input.TexCoords + offset).xyzw; 
    float4 secondaryTexColor = tex2D(secondaryTrailTex, input.TexCoords + offset2).xyzw;
    float4 color = (primaryColor + secondaryTexColor) / 2.0;
    
    float4 color2 = lerp(primaryColor, secondaryColor, input.TexCoords.x);
    float3 bright = color.xyz * (1.0 + color.x * 2.0) * color2.xyz + (color.r > 0.8 ? ((color.r - 0.8) * 3.5) : float3(0, 0, 0));
    return float4(bright, input.Color.w * color.r);
}

technique Technique1
{
    pass PrimitivesPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
};