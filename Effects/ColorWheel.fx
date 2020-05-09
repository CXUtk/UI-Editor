sampler uImage0 : register(s0);

float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs((c.xxx + K.xyz - floor(c.xxx + K.xyz)) * 6.0 - K.www);
    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0 {
	float2 diff = coords - float2(0.5, 0.5);
	float len = length(diff);
	if (len > 0.5) return float4(0, 0, 0, 0);
	float degree = atan2(diff.y, diff.x) / 6.28318;
	float4 c = float4(hsv2rgb(float3(degree, len * 2, 1.0)), 1.0);
	return c;
}


technique Technique1 {
	pass ColorWheel {
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}