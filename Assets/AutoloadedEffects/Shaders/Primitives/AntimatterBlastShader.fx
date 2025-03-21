sampler fadeTexture : register(s1);

float localTime;
float edgeVanishByDistanceSharpness;
float gradientCount;
float3 gradient[5];
matrix uWorldViewProjection;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float3 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float QuadraticBump(float x)
{
    return x * (4 - x * 4);
}

float3 PaletteLerp(float interpolant)
{
    int startIndex = clamp(interpolant * gradientCount, 0, gradientCount - 1);
    int endIndex = clamp(startIndex + 1, 0, gradientCount - 1);
    return lerp(gradient[startIndex], gradient[endIndex], frac(interpolant * gradientCount));
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
    coords.y = (coords.y - 0.5) / input.TextureCoordinates.z + 0.5;
    
    // Calculate noise that determines how the trail fades out at the back.
    float fadeOutNoise = tex2D(fadeTexture, coords * 0.2 - float2(localTime * 0.4, 0)) + tex2D(fadeTexture, coords * 0.267 - float2(localTime * 0.5, 0.5));
    float noiseOpacity = smoothstep(-0.32, -0.08, fadeOutNoise * 0.5 - coords.x);
    
    // Make the trail fade out at the sides.
    float edgeOpacity = smoothstep(0, 0.6, QuadraticBump(coords.y) - coords.x * edgeVanishByDistanceSharpness);
    
    // Combine opacity values.
    float opacity = noiseOpacity * edgeOpacity;
    
    // Calculate the hue for the given pixel.
    // This is influenced by noise, but is strongest at the tip of the blast.
    float hue = tex2D(fadeTexture, coords * 0.3 + float2(localTime * -1.4, 0)) * 0.8 + smoothstep(0.3, 0, coords.x);
    
    // Additively apply the vibrant, oily colors in accordance with the hue.
    color = saturate(color + float4(PaletteLerp(saturate(hue)), 0));
    
    return color * opacity;
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
