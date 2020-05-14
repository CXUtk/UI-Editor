sampler uImage0 : register(s0);
float uDegree;
float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs((c.xxx + K.xyz - floor(c.xxx + K.xyz)) * 6.0 - K.www);
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0 {
	float degree = coords.y;
	float4 c = float4(hsv2rgb(float3(degree, 1.0, 1.0)), 1.0);
	return c;
}

float4 PixelShaderFunction2(float2 coords : TEXCOORD0) : COLOR0 {
	float2 vec = float2(coords.x + 0.5, coords.y + 0.5);
	float4 c = float4(hsv2rgb(float3(uDegree, coords.x, 1 - coords.y)), 1.0);
	return c;
}

technique Technique1 {
	pass ColorBar {
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	pass ColorRect {
		PixelShader = compile ps_2_0 PixelShaderFunction2();
	}
}