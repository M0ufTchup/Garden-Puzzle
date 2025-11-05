extends Node3D
@onready var grass: MultiMeshInstance3D = $"../MultiMeshInstance3D"


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	grass.set_instance_shader_parameter("player_position", get_global_position());
