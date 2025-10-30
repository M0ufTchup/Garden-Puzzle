extends MeshInstance3D

var testShader: bool = false
@export var curve: Curve
@export var loop : bool = false;
@onready var particle: GPUParticles3D = $DespawnVFX

var time: float = 0.0
var dir: int = 1

func _input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		testShader = !testShader
		

func _process(delta):
	if testShader:
		time += delta * dir;
		var material: ShaderMaterial = mesh.surface_get_material(0)
		material.set_shader_parameter('DissolveAmount', curve.sample(time));
		particle.emitting = true;
		if time > 1: particle.emitting = false;
		if loop:
			if time > 1: dir = -1;
			elif time < 0: dir = 1;
			
		time = clamp(time, 0, 1)
