sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
matrix uWorldViewProjection;
float4 uShaderSpecificData;
float time;
matrix transformMatrix;
texture trailTexture;
sampler2D trailTex = sampler_state
{
    texture = <trailTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    //We have to multiply the position by the matrix so it appears in the correct spot
    VertexShaderOutput output;
    output.Position = mul(input.Position, transformMatrix);
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // Normalized pixel coordinates (from 0 to 1)
    float2 coords = input.TextureCoordinates;
    
    //Calculate Distorting Noise
    float s = tex2D(trailTex, coords + float2(time * -1.5, 0.0)).r;
  
    // Output to screen
    float4 finalColor = float4(input.Color.rgb * s, 0) * input.Color.w;
    return finalColor;
}

technique Technique1
{
    pass PrimitivesPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}