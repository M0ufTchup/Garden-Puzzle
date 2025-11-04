shader_type spatial;

uniform sampler2D texture;
uniform vec3 Color: source_color;
uniform float Metal;
uniform float Rough;
uniform sampler2D NormalMap;
uniform float NormalScale= 0.0;
uniform sampler2D NoiseDissolveTexture;
uniform float DissolveAmount = 0.3;
uniform vec3 DissolveEdgeColor : source_color;
uniform sampler2D NoiseMask;


float CheapContrast(float In, float constrast)
{
	float a = 0.0 - constrast;
	float b = constrast + 1.0;
	float lerp = mix(a, b, In);
	return clamp(lerp, 0.0, 1.0);
}

float OpacityMask(vec2 uv)
{
	vec4 noise = texture(NoiseDissolveTexture, uv);
	float a = 0.0;
	float b = 0.95;
	if(DissolveAmount > b)
	{
		a = DissolveAmount + 0.5;
	}
	else
	{
		a = DissolveAmount;
	}
	
	a = a - 0.6;
	
	float In = noise.r - a;
	float contrast = CheapContrast(In, 1.5);
	return clamp(contrast, 0.0, 1.0);
}

vec3 EmissiveColor(vec2 uv)
{
	vec4 noise = texture(NoiseMask, vec2(OpacityMask(uv), 0.0));
	
	float constrast = 1.0 - clamp(CheapContrast(noise.r, 1.5), 0.0, 1.0);
	
	vec3 color = vec3(0.0);
	if(OpacityMask(uv) > 0.0)
	{
		color = constrast * DissolveEdgeColor;
	}
	
	return color;
}

void fragment() 
{
	ALBEDO = texture(texture, UV).rgb * Color;
	METALLIC = Metal;
	ROUGHNESS = Rough;
	EMISSION = EmissiveColor(UV);
	NORMAL_MAP = texture(NormalMap, UV).rgb * NormalScale;
	ALPHA = texture(texture, UV).a * OpacityMask(UV);
}

