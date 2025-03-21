sampler noiseTexture : register(s1);

float noiseOffset;
float globalTime;
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
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    float4 pos = mul(input.Position, uWorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    output.TextureCoordinates.y = (output.TextureCoordinates.y - 0.5) / input.TextureCoordinates.z + 0.5;

    return output;
}

float QuadraticBump(float x)
{
    return x * (4 - x * 4);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float innerBrightnessIntensity = 2.7;
    float4 color = input.Color;
    float2 coords = input.TextureCoordinates;
    
    // Calculate the erasure noise. This will be used to create accented variation in the texture cutouts.
    float erasureNoise = tex2D(noiseTexture, coords * float2(0.2, 0.6) + float2(globalTime * -0.15 + noiseOffset, 0)) * 0.35;
    
    // Calculate a bump and sinusoidal curve for use by the erasure evaluation value.
    float horizontalCenterBump = (1 - QuadraticBump(coords.y)) * 0.2;
    float completionRatioPulse = cos(coords.x * 22 - globalTime * 20 + noiseOffset * 25) * 0.5 + 0.5;
    
    // Determine whether the color should be erased.
    float erasureEvaluationValue = completionRatioPulse * coords.x + horizontalCenterBump + smoothstep(0.6, 0.95, coords.x) + erasureNoise;
    bool erasePixel = erasureEvaluationValue >= 0.4;
    
    // Make colors at the front brighter and colors at the back darker.
    color += smoothstep(0.4, 0.15, coords.x) * color.a * 0.6;
    color -= float4(1, 1, 0.67, 0) * completionRatioPulse * coords.x * color.a * 1.7;
    
    // Apply posterization.
    color = round(color * 12) / 12;
    
    return color * (1 - erasePixel);
}

technique Technique1
{
    pass AutoloadPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
