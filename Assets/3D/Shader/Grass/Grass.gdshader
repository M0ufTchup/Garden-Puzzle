shader_type spatial;
render_mode cull_disabled, diffuse_toon, specular_schlick_ggx;
// Nice Shader by @_Malido ^^

uniform vec3 top_Color: source_color;
uniform vec3 bottom_color: source_color;
uniform float ambient_occlusion_factor: hint_range(0.0, 1.0, 0.1) = 0.3;
uniform float specular_strength: hint_range(0.0, 1.0, 0.1) = 0.4;
uniform float player_displacement_strength: hint_range(0.0, 1.0, 0.1) = 0.4;
uniform float player_displacement_size: hint_range(0.0, 2.0, 0.1) = 1.0;

uniform vec3 wind_direction; // Use a negative y component to give it an extra touch (For displacement effect and noise scroll direction)
uniform float wind_strength: hint_range(0.0, 1.0, 0.01) = 0.3;
uniform sampler2D wind_noise; // Perlin FBM Noise looks Best
uniform float wind_noise_size: hint_range(0.0, 1.0, 0.001) = 0.05; // high values dont work well
uniform float wind_noise_speed: hint_range(0.0, 1.0, 0.001) = 0.1;

instance uniform vec3 player_position;

void vertex() {
	vec3 worldPosition = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	
	vec3 windText = texture(wind_noise, worldPosition.xz * wind_noise_size 
	+ normalize(-wind_direction.xz) * (TIME * UV.y / 2.5) * wind_noise_speed).rgb;
	
	vec3 windVector = (vec4(windText * normalize(wind_direction) * wind_strength, 0.0) * MODEL_MATRIX).xyz;
	
	float playerHeight = smoothstep(1.0, 0.0, length(player_position.y - worldPosition.y + 0.3));
	vec3 pushDirection = vec3(worldPosition - player_position) * vec3(1, -0.3, 1);
	float playerPositionFactor = smoothstep(player_displacement_size, 0.0, length(pushDirection));
	
	vec3 playerPushVector = (vec4(normalize(pushDirection), 0.0) * MODELVIEW_MATRIX).xyz;
	
	// Apply Player Position displacement
	VERTEX += playerPushVector * (1.0 - UV.y) * playerPositionFactor * player_displacement_strength * playerHeight;
	// Apply Wind displacement linearly
	VERTEX += windVector * (1.0 - UV.y) * (1.0 - playerPositionFactor * 0.7);
	
	// A new normal correction, which aligns the normals of the mesh facing upwards no matter the original direction.
	NORMAL = vec3(0.0, 1.0, 0.0);
}

void fragment() {
	vec3 color = mix(bottom_color, top_Color, 1.0 - UV.y);
	
	float aoFallOff = pow(UV.y, 5.0);
	vec3 aoColor = bottom_color * (1.0 - ambient_occlusion_factor);
	
	ALBEDO = mix(color, aoColor, aoFallOff);
	ROUGHNESS = 0.4;
	
	SPECULAR *= (0.1 - UV.y) * specular_strength;
	
	if(!FRONT_FACING)
	{
		NORMAL = -NORMAL;
	}
}

