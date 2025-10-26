shader_type spatial;

float circle(in vec2 uv, in float radius, in float hardness)
{
	vec2 dist = uv - vec2(0.5);
	return 1.0 - smoothstep(radius - (radius * hardness), 
							radius + (radius * hardness), 
							dot(dist, dist * 4.0));
}
void fragment()
{
	EMISSION = COLOR.rgb;
	ALPHA = circle(UV, 0.5, 0.5);
}

