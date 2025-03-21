sampler noiseTextureA : register(s1);
sampler noiseTextureB : register(s2);

float localTime;
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

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 coords = input.TextureCoordinates;
    float4 color = input.Color;
    
    coords.y = (coords.y - 0.5) / input.TextureCoordinates.z + 0.5;
    
    float n = 0.8 / (tex2D(noiseTextureA, coords * float2(0.3, 1.2) + float2(localTime * -0.6, 0)) + 0.1);
    n += 0.02 / tex2D(noiseTextureB, coords * float2(0.23, 1.01) + float2(localTime * -0.9, 0));
    
    float glow = saturate(0.1 / pow(1 - QuadraticBump(coords.y), (1 - coords.x) * 0.8));
    glow *= smoothstep(0.15, 0.3, glow);
    glow *= smoothstep(0, 0.4, coords.x);
    glow *= n;
    
    return color * glow;
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
