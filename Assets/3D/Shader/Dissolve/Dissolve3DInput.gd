extends MeshInstance3D


#@export var DissolveDirection : Vector2 = Vector2(0,0);

var _offset := -2.0
var _direction = 1

@export var testShader: bool = false
@export  var speed : float = 1.0
@export var from : float = -3.0
@export var to : float = 2.0

func _ready():
	var material: ShaderMaterial = mesh.surface_get_material(0)
	var material2: ShaderMaterial = mesh.surface_get_material(0)

	material.set_shader_parameter('global_transform', get_global_transform())
	material2.set_shader_parameter('global_transform', get_global_transform())
	_test_shader(testShader);

func _process(delta):
	if testShader:
		var material: ShaderMaterial = mesh.surface_get_material(0)
		var material2: ShaderMaterial = mesh.surface_get_material(0)

		_offset += speed * delta * _direction
		if abs(_offset) > to:
			_offset = to * _direction
			_direction *= -1

		material.set_shader_parameter('offset', _offset)
		material2.set_shader_parameter('offset', _offset)

func _test_shader(v) -> void:
	_offset = from
	testShader = v
